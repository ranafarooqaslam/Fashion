using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using System.Data;
using CORNCommon.Classes;
using System.Drawing;
using QRCoder;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

public partial class Forms_frmPendingTaxAuthority : System.Web.UI.Page
{
    readonly OrderEntryController _OrderEntry = new OrderEntryController();
    UserController userControl = new UserController();
    RptCustomerController _CustomerCtrl = new RptCustomerController();
    static DataTable dtLocation;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtstartDate.Text = (DateTime.Parse(Session["CurrentWorkDate"].ToString())).ToString("dd-MMM-yyyy");
            txtEndDate.Text = (DateTime.Parse(Session["CurrentWorkDate"].ToString())).ToString("dd-MMM-yyyy");
            dtLocation = userControl.SelectSlashUser2(int.Parse(Session["UserId"].ToString()));
            LoadDistributor();
            LoadGridInvoices();
            DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
            DataRow[] drAppSetting4 = dtAppSetting.Select("strColumnName='IsTaxAuthorityIntegration'");
            Session.Add("IsTaxAuthorityIntegration", "0");
            if (drAppSetting4.Length > 0)
            {
                if (drAppSetting4[0]["strColumnValue"].ToString() == "1")
                {
                    Session.Add("IsTaxAuthorityIntegration", "1");
                }
            }
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }
    protected void btnGetDate_Click(object sender, EventArgs e)
    {
        LoadGridInvoices();
    }
    private void LoadGridInvoices()
    {
        if (drpDistributor.Items.Count > 0)
        {
            DataTable piDt = _OrderEntry.GetPendingTaxAuthorityInvoices(int.Parse(drpDistributor.SelectedValue), 0, DateTime.Parse(txtstartDate.Text), DateTime.Parse(txtEndDate.Text));
            gvPendingInvoices.DataSource = piDt;
            gvPendingInvoices.DataBind();
        }
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridInvoices();
    }
    protected void btnSyncData_Click(object sender, EventArgs e)
    {
        DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
        if (dtAppSetting != null)
        {
            DataRow[] drAppSetting6 = dtAppSetting.Select("strColumnName='InvoiceCalculation'");
            if (drAppSetting6.Length > 0)
            {
                if (drAppSetting6[0]["strColumnValue"].ToString() == "0")
                {
                    PostDataToFBR();
                }
                else
                {
                    PostDataToFBRInclusiveGSTPrices();
                }
            }
        }
    }
    protected void gvPendingInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        e.Row.BackColor = Color.White;
    }
    private void PostDataToFBR()
    {
        if (Session["IsTaxAuthorityIntegration"].ToString() == "1")
        {
            int count = 0;
            CompanyController objCompny = new CompanyController();
            DataTable dtFBR = objCompny.GetFBRIntegration(int.Parse(drpDistributor.SelectedValue.ToString()));
            DataTable dtDetail = new DataTable();
            foreach (GridViewRow gvr in gvPendingInvoices.Rows)
            {
                CheckBox cbInvoice = (CheckBox)gvr.Cells[0].FindControl("cbInvoice");
                if (cbInvoice.Checked)
                {
                    dtDetail = _CustomerCtrl.GetInvoiceDetail(int.Parse(drpDistributor.SelectedValue.ToString()), 3, Convert.ToInt64(gvr.Cells[1].Text));
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
                        TaxRate = Convert.ToDouble(dtDetail.Rows[0]["GST_RATE"]);
                        foreach (DataRow drDetail in dtDetail.Rows)
                        {
                            InvoiceFBRDetail ObjInvoiceDetail = new InvoiceFBRDetail();
                            ObjInvoiceDetail.ItemCode = drDetail["SKU_ID"].ToString();
                            ObjInvoiceDetail.ItemName = drDetail["SKU_NAME"].ToString();
                            ObjInvoiceDetail.Quantity = Convert.ToInt32(drDetail["QUANTITY_UNIT"]);
                            ObjInvoiceDetail.SaleValue = Convert.ToDouble(drDetail["AMOUNT"]);
                            ObjInvoiceDetail.TaxCharged = Convert.ToDouble(drDetail["AMOUNT"]) * TaxRate / 100;
                            ObjInvoiceDetail.TaxRate = TaxRate;
                            ObjInvoiceDetail.TotalAmount = Convert.ToDouble(drDetail["AMOUNT"]) - ObjInvoiceDetail.TaxCharged;                            
                            ObjInvoiceDetail.PCTCode = "10101";
                            ObjInvoiceDetail.FurtherTax = 0;
                            ObjInvoiceDetail.InvoiceType = 1;
                            ObjInvoiceDetail.Discount = Discount / dtDetail.Rows.Count;
                            ObjInvoiceDetail.RefUSIN = null;
                            lstItems.Add(ObjInvoiceDetail);
                            TotalQty += ObjInvoiceDetail.Quantity;
                            GrossValue += ObjInvoiceDetail.SaleValue;
                            NetAmount += ObjInvoiceDetail.TotalAmount;
                            TotalTax += ObjInvoiceDetail.TaxCharged;
                        }
                        objInvoice.Items = lstItems;
                        objInvoice.InvoiceNumber = string.Empty;
                        objInvoice.POSID = dtFBR.Rows[0]["POSID"].ToString();
                        objInvoice.USIN = gvr.Cells[1].Text;
                        objInvoice.DateTime = Convert.ToDateTime(gvr.Cells[10].Text);
                        objInvoice.BuyerNTN = "";
                        objInvoice.BuyerCNIC = "";
                        objInvoice.BuyerName = "";
                        objInvoice.BuyerPhoneNumber = "";
                        objInvoice.PaymentMode = PaymentMode;
                        objInvoice.TotalSaleValue = GrossValue;
                        objInvoice.TotalQuantity = TotalQty;
                        objInvoice.TotalBillAmount = NetAmount;
                        objInvoice.TotalTaxCharged = TotalTax;
                        objInvoice.Discount = Discount;
                        objInvoice.FurtherTax = 0;
                        objInvoice.InvoiceType = 1;
                        objInvoice.RefUSIN = null;

                        HttpClient Client = new HttpClient();

                        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dtFBR.Rows[0]["Token"].ToString());
                        var content = new StringContent(JsonConvert.SerializeObject(objInvoice), Encoding.UTF8, "application/json");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        HttpResponseMessage response = Client.PostAsync(dtFBR.Rows[0]["FBRURL"].ToString(), content).Result;

                        string InvoiceNumberFBR = string.Empty;
                        string CodeFBR = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string responseFBR = response.Content.ReadAsStringAsync().Result;
                            InvoiceNumberFBR = JObject.Parse(responseFBR)["InvoiceNumber"].ToString();
                            CodeFBR = JObject.Parse(responseFBR)["Code"].ToString();
                            _OrderEntry.UpdateInvoiceNumberRollBackTaxAuthority(Convert.ToInt64(gvr.Cells[1].Text), InvoiceNumberFBR, 2);
                            count++;
                        }
                    }
                }
            }
            if (count > 0)
            {
                LoadGridInvoices();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Selected Invoice(s) synced successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No Invocie selected or Tax Authority Server is down.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('This client is not integrated to any server.');", true);
        }
    }
    private void PostDataToFBRInclusiveGSTPrices()
    {
        if (Session["IsTaxAuthorityIntegration"].ToString() == "1")
        {
            int count = 0;
            CompanyController objCompny = new CompanyController();
            DataTable dtFBR = objCompny.GetFBRIntegration(int.Parse(drpDistributor.SelectedValue.ToString()));
            DataTable dtDetail = new DataTable();
            foreach (GridViewRow gvr in gvPendingInvoices.Rows)
            {
                CheckBox cbInvoice = (CheckBox)gvr.Cells[0].FindControl("cbInvoice");
                if (cbInvoice.Checked)
                {
                    dtDetail = _CustomerCtrl.GetInvoiceDetail(int.Parse(drpDistributor.SelectedValue.ToString()), 3, Convert.ToInt64(gvr.Cells[1].Text));
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
                        TaxRate = Convert.ToDouble(dtDetail.Rows[0]["GST_RATE"]);
                        foreach (DataRow drDetail in dtDetail.Rows)
                        {
                            InvoiceFBRDetail ObjInvoiceDetail = new InvoiceFBRDetail();
                            ObjInvoiceDetail.ItemCode = drDetail["SKU_ID"].ToString();
                            ObjInvoiceDetail.ItemName = drDetail["SKU_NAME"].ToString();
                            ObjInvoiceDetail.Quantity = Convert.ToInt32(drDetail["QUANTITY_UNIT"]);                            
                            ObjInvoiceDetail.TaxCharged = Convert.ToDouble(drDetail["AMOUNT"]) - (Convert.ToDouble(drDetail["AMOUNT"]) / (100 + Convert.ToDouble(drDetail["GST_RATE"]))) * 100;
                            ObjInvoiceDetail.SaleValue = Convert.ToDouble(drDetail["AMOUNT"]) - ObjInvoiceDetail.TaxCharged;
                            ObjInvoiceDetail.TotalAmount = Convert.ToDouble(drDetail["AMOUNT"]);
                            ObjInvoiceDetail.TaxRate = TaxRate;                            
                            ObjInvoiceDetail.PCTCode = "10101";
                            ObjInvoiceDetail.FurtherTax = 0;
                            ObjInvoiceDetail.InvoiceType = 1;
                            ObjInvoiceDetail.Discount = Discount / dtDetail.Rows.Count;
                            ObjInvoiceDetail.RefUSIN = null;
                            lstItems.Add(ObjInvoiceDetail);
                            TotalQty += ObjInvoiceDetail.Quantity;
                            GrossValue += ObjInvoiceDetail.SaleValue;
                            NetAmount += ObjInvoiceDetail.TotalAmount;
                            TotalTax += ObjInvoiceDetail.TaxCharged;
                        }
                        objInvoice.Items = lstItems;
                        objInvoice.InvoiceNumber = string.Empty;
                        objInvoice.POSID = dtFBR.Rows[0]["POSID"].ToString();
                        objInvoice.USIN = gvr.Cells[1].Text;
                        objInvoice.DateTime = Convert.ToDateTime(gvr.Cells[10].Text);
                        objInvoice.BuyerNTN = "";
                        objInvoice.BuyerCNIC = "";
                        objInvoice.BuyerName = "";
                        objInvoice.BuyerPhoneNumber = "";
                        objInvoice.PaymentMode = PaymentMode;
                        objInvoice.TotalSaleValue = GrossValue;
                        objInvoice.TotalQuantity = TotalQty;
                        objInvoice.TotalBillAmount = NetAmount;
                        objInvoice.TotalTaxCharged = TotalTax;
                        objInvoice.Discount = Discount;
                        objInvoice.FurtherTax = 0;
                        objInvoice.InvoiceType = 1;
                        objInvoice.RefUSIN = null;

                        HttpClient Client = new HttpClient();

                        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dtFBR.Rows[0]["Token"].ToString());
                        var content = new StringContent(JsonConvert.SerializeObject(objInvoice), Encoding.UTF8, "application/json");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        HttpResponseMessage response = Client.PostAsync(dtFBR.Rows[0]["FBRURL"].ToString(), content).Result;

                        string InvoiceNumberFBR = string.Empty;
                        string CodeFBR = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string responseFBR = response.Content.ReadAsStringAsync().Result;
                            InvoiceNumberFBR = JObject.Parse(responseFBR)["InvoiceNumber"].ToString();
                            CodeFBR = JObject.Parse(responseFBR)["Code"].ToString();
                            _OrderEntry.UpdateInvoiceNumberRollBackTaxAuthority(Convert.ToInt64(gvr.Cells[1].Text), InvoiceNumberFBR, 2);
                            count++;
                        }
                    }
                }
            }
            if (count > 0)
            {
                LoadGridInvoices();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Selected Invoice(s) synced successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No Invocie selected or Tax Authority Server is down.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('This client is not integrated to any server.');", true);
        }
    }
}