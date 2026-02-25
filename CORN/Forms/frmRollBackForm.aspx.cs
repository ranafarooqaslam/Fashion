using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

/// <summary>
/// Form To Rollback Order, Invoice, Sale Return And Realized Cheque
/// </summary>
public partial class Forms_frmRollBackForm : System.Web.UI.Page
{
    static string IsTaxAuthorityIntegration = "0";
    /// <summary>
    /// Page_Load Function Populates All Combos And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            this.LoadOrderBooker();
            this.LoadLegend();

            DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
            DataRow[] drAppSetting4 = dtAppSetting.Select("strColumnName='IsTaxAuthorityIntegration'");
            Session.Add("IsTaxAuthorityIntegration", "0");
            if (drAppSetting4.Length > 0)
            {
                if (drAppSetting4[0]["strColumnValue"].ToString() == "1")
                {
                    CompanyController objCompny = new CompanyController();
                    DataTable dtFBR = objCompny.GetFBRIntegration(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
                    Session.Add("dtFBR", dtFBR);
                    IsTaxAuthorityIntegration = "1";
                }
            }
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }


    /// <summary>
    /// Loads Legends To Legend Combo
    /// </summary>
    private void LoadLegend()
    {
        OrderEntryController or = new OrderEntryController();
        DataTable m_dt = or.SelectLegend();
        clsWebFormUtil.FillDropDownList(this.DrpLenged, m_dt, 0, 2, true);
    }

    /// <summary>
    /// Load OrderBookers To OrderBooker Combo
    /// </summary>
    private void LoadOrderBooker()
    {
        if (drpDistributor.Items.Count > 0)
        {
            SaleForceController mDController = new SaleForceController();
            DataTable m_dt = mDController.SelectRollBackInvoiceSaleForce(int.Parse(DrpDocumentType.SelectedValue.ToString()), 0, int.Parse(drpDistributor.SelectedValue.ToString()));
            clsWebFormUtil.FillDropDownList(this.DrpOrderBooker, m_dt, 0, 1, true);
        }
        else
        {
            DrpOrderBooker.Items.Clear();
        }
    }

    /// <summary>
    /// Loads Rollback Order, Invoice And Sale Return Data To Grid
    /// </summary>
    private void LoadRollbackDocument()
    {
        OrderEntryController or = new OrderEntryController();
        DataTable dtOrder = or.SelectRollBackDocument(int.Parse(drpDistributor.SelectedValue.ToString()), 0, int.Parse(DrpOrderBooker.SelectedValue.ToString()), int.Parse(DrpDocumentType.SelectedValue.ToString()), DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        GrdOrder.DataSource = dtOrder;
        GrdOrder.DataBind();
    }

    /// <summary>
    /// Loads Rollback Cheques Data To Grid
    /// </summary>
    private void LoadRollbackCheque()
    {
        ChequeEntryController CController = new ChequeEntryController();
        DataTable dt = CController.SelectChequeEntry(Constants.Cheque_Clear, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), 0, 0);
        GrdCheque.DataSource = dt;
        GrdCheque.DataBind();
    }

    /// <summary>
    /// Loads OrderBookers To OrderBooker Combo
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadOrderBooker();
    }

    /// <summary>
    /// Loads OrderBookers To OrderBooker Combo And Principals To Principal Combo
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {

        this.LoadOrderBooker();
    }

    /// <summary>
    /// Rollbacks Order, Invoice, Sale Return And Realized Cheque
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedIndex == 1)
        {
            foreach (GridViewRow dr in GrdCheque.Rows)
            {
                CheckBox ChbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
                if (ChbInvoice.Checked == true)
                {
                    ChequeEntryController CController = new ChequeEntryController();
                    DataControl dc = new DataControl();
                    CController.RollbackChequeEntry(0, int.Parse(drpDistributor.SelectedValue.ToString()), dr.Cells[6].Text, long.Parse(dr.Cells[0].Text), long.Parse(dr.Cells[5].Text));
                }
            }
            LoadRollbackCheque();
        }
        else
        {
            RptCustomerController _CustomerCtrl = new RptCustomerController();
            foreach (GridViewRow dr in GrdOrder.Rows)
            {
                CheckBox ChbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
                if (ChbInvoice.Checked == true)
                {
                    OrderEntryController ORD = new OrderEntryController();
                    DataControl dc = new DataControl();
                    if (ORD.UpdateRollBackDocument(Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(DrpDocumentType.SelectedValue.ToString()), int.Parse(DrpLenged.SelectedValue.ToString())))
                    {
                        AccountPosting(int.Parse(drpDistributor.SelectedValue.ToString()), Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), int.Parse(this.Session["UserId"].ToString()), Convert.ToDecimal(dr.Cells[7].Text), Convert.ToDecimal(dr.Cells[10].Text), Convert.ToDecimal(dr.Cells[12].Text), Convert.ToDecimal(dr.Cells[9].Text));
                        if (IsTaxAuthorityIntegration == "1")
                        {
                            DataTable dtFBR = Session["dtFBR"] as DataTable;
                            if (dtFBR.Rows.Count > 0)
                            {
                                DataTable dtDetail = _CustomerCtrl.GetInvoiceDetail(int.Parse(drpDistributor.SelectedValue.ToString()), 3, Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                                if (dtDetail.Rows.Count > 0)
                                {
                                    InvoiceFBR objInvoice = new InvoiceFBR();
                                    List<InvoiceFBRDetail> lstItems = new List<InvoiceFBRDetail>();
                                    int TotalQty = 0;
                                    double GrossValue = 0;
                                    double NetAmount = 0;
                                    double TotalTax = 0;
                                    double TaxRate = 0;
                                    decimal Discount = 0;
                                    int PaymentMode = 1;

                                    if (dtDetail.Rows[0]["PaymentMode"].ToString() == "214")
                                    {
                                        PaymentMode = 1;//Cash
                                    }
                                    else if (dtDetail.Rows[0]["PaymentMode"].ToString() == "215")
                                    {
                                        PaymentMode = 2;//Credit Card
                                    }
                                    else if (dtDetail.Rows[0]["PaymentMode"].ToString() == "217")
                                    {
                                        PaymentMode = 5;//Mixed
                                    }
                                    else
                                    {
                                        PaymentMode = 1;
                                    }
                                    Discount = Convert.ToDecimal(dtDetail.Rows[0]["Discount"]);
                                    TotalTax = Convert.ToDouble(dtDetail.Rows[0]["GST"]);
                                    TaxRate = Convert.ToDouble(dtDetail.Rows[0]["GST"]) / Convert.ToDouble(dtDetail.Rows[0]["GrossAmount"]) * 100;
                                    foreach (DataRow drDetail in dtDetail.Rows)
                                    {
                                        InvoiceFBRDetail ObjInvoiceDetail = new InvoiceFBRDetail();
                                        ObjInvoiceDetail.ItemCode = drDetail["SKU_ID"].ToString();
                                        ObjInvoiceDetail.ItemName = drDetail["SKU_NAME"].ToString();
                                        ObjInvoiceDetail.Quantity = Convert.ToInt32(drDetail["QUANTITY_UNIT"]);
                                        ObjInvoiceDetail.SaleValue = Convert.ToDouble(drDetail["AMOUNT"]);
                                        ObjInvoiceDetail.TaxCharged = Convert.ToDouble(drDetail["AMOUNT"]) * TaxRate / 100;
                                        ObjInvoiceDetail.TaxRate = TaxRate;
                                        ObjInvoiceDetail.TotalAmount = Convert.ToDouble(drDetail["AMOUNT"]) + ObjInvoiceDetail.TaxCharged;
                                        ObjInvoiceDetail.PCTCode = "10101";
                                        ObjInvoiceDetail.FurtherTax = 0;
                                        ObjInvoiceDetail.InvoiceType = 3;//1=New,2=Debit,3=Credit
                                        ObjInvoiceDetail.Discount = Discount / dtDetail.Rows.Count;
                                        ObjInvoiceDetail.RefUSIN = GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"].ToString();
                                        lstItems.Add(ObjInvoiceDetail);
                                        TotalQty += ObjInvoiceDetail.Quantity;
                                        GrossValue += ObjInvoiceDetail.SaleValue;
                                        NetAmount += ObjInvoiceDetail.TotalAmount;
                                        TotalTax += ObjInvoiceDetail.TaxCharged;
                                    }

                                    objInvoice.Items = lstItems;
                                    objInvoice.InvoiceNumber = string.Empty;
                                    objInvoice.POSID = dtFBR.Rows[0]["POSID"].ToString();
                                    objInvoice.USIN = GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"].ToString() + "Rtrn";
                                    objInvoice.DateTime = DateTime.Now;
                                    objInvoice.BuyerNTN = "";
                                    objInvoice.BuyerCNIC = "";
                                    objInvoice.BuyerName = "";
                                    objInvoice.BuyerPhoneNumber = "";
                                    objInvoice.PaymentMode = PaymentMode;//1=Cash,2=Card,3=Gift Voucher,4=Loyality Card,5=Mixed,6=Cheque
                                    objInvoice.TotalSaleValue = GrossValue;
                                    objInvoice.TotalQuantity = TotalQty;
                                    objInvoice.TotalBillAmount = NetAmount;
                                    objInvoice.TotalTaxCharged = TotalTax;
                                    objInvoice.Discount = Discount;
                                    objInvoice.FurtherTax = 0;
                                    objInvoice.InvoiceType = 3;
                                    objInvoice.RefUSIN = GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"].ToString();
                                    try
                                    {
                                        HttpClient Client = new HttpClient();

                                        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dtFBR.Rows[0]["Token"].ToString());
                                        var content = new StringContent(JsonConvert.SerializeObject(objInvoice), Encoding.UTF8, "application/json");
                                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                                        HttpResponseMessage response = Client.PostAsync(dtFBR.Rows[0]["FBRURL"].ToString(), content).Result;

                                        string InvoiceNumberFBR = string.Empty;
                                        string CodeFBR = string.Empty;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            string responseFBR = response.Content.ReadAsStringAsync().Result;
                                            InvoiceNumberFBR = JObject.Parse(responseFBR)["InvoiceNumber"].ToString();
                                            CodeFBR = JObject.Parse(responseFBR)["Code"].ToString();

                                            ORD.UpdateInvoiceNumberRollBackTaxAuthority(Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), InvoiceNumberFBR,1);
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.LoadRollbackDocument();
        }
    }

    /// <summary>
    /// Loads Order, Invoice, Sale Return And Realized Cheque Data To Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnGetOrder_Click(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedIndex == 1)
        {
            this.LoadRollbackCheque();
            GrdOrder.Visible = false;
            GrdCheque.Visible = true;
        }
        else
        {
            GrdOrder.Visible = true;
            GrdCheque.Visible = false;
            this.LoadRollbackDocument();
        }
    }

    private void AccountPosting(int p_Distributor_id, long p_SALE_INVOICE_ID, DateTime p_DocumentDate, int p_UserId, decimal p_TOTAL_AMOUNT, decimal p_GST_AMOUNT, decimal p_EXTRA_DISCOUNT_AMOUNT, decimal p_SCHEME_AMOUNT)
    {
        #region Account Posting
        LedgerController LController = new LedgerController();
        Configuration.GetAccountHead();
        string VoucherNo2 = LController.SelectMaxVoucherId(Constants.Journal_Voucher, p_Distributor_id, p_DocumentDate);
        if (LController.PostingGLMaster(p_Distributor_id, 0, VoucherNo2, Constants.Journal_Voucher, p_DocumentDate, Constants.Document_SaleReturn, Convert.ToString(p_SALE_INVOICE_ID), "Sale Rollback Voucher", p_UserId, "Rollback", Constants.Document_SaleReturn, p_SALE_INVOICE_ID))
        {
            //Dr Cash in Hand
            //Cr Cash Sale
            if (p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT > 0)
            {
                //352-3002120004-Cash
                LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 352, 0, p_TOTAL_AMOUNT + p_GST_AMOUNT - p_EXTRA_DISCOUNT_AMOUNT - p_SCHEME_AMOUNT, "Cash In Hand Sale Rollback Voucher");
            }
            if (p_TOTAL_AMOUNT > 0)
            {
                //762-4001010013-Cash Sales
                LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 762, p_TOTAL_AMOUNT, 0, "Sale Rollback Voucher");
            }
            if (p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT > 0)
            {
                //764-4001020001-Discount on Sale
                LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 764, 0, p_EXTRA_DISCOUNT_AMOUNT + p_SCHEME_AMOUNT, "Discount Sale Rollback Voucher");
            }
            if (p_GST_AMOUNT > 0)
            {
                //73-2002020004-Sales Tax Paid
                LController.PostingGLDetail(p_Distributor_id, 0, Constants.Journal_Voucher, VoucherNo2, 73, p_GST_AMOUNT, 0, "GST Sale Rollback Voucher");
            }

        }
        #endregion
    }
}