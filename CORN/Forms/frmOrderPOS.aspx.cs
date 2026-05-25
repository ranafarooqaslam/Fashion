using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using QRCoder;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Net.Http;

public partial class Forms_frmOrderPOS : System.Web.UI.Page
{
    #region Variables
    static Forms_frmOrderPOS  temp = new Forms_frmOrderPOS();
    readonly DistributorController _mDist = new DistributorController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    readonly OrderEntryController _or = new OrderEntryController();
    readonly SKUGroupController _groupCtl = new SKUGroupController();
    readonly RptCustomerController rcc = new RptCustomerController();
    readonly CompanyController objCompny = new CompanyController();
    UserController userControl = new UserController();
    readonly General general = new General();
    string ExpiryDate;
    DataTable PurchaseSKU;
    DataControl _dc = new DataControl();
    public int onhold = 0;
    public long newCustomerId;
    long saleInvoiceID = 0;
    public long IvoiceIdDistW;
    public int printType;
    public static string CompanyName;
    public static string CompanyPhonNmbr;
    private static string _hirarchyNameQuiz;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadProduct();
            try
            {
                lblLicense.Text = Session["LicenseMessage"].ToString();
                if (lblLicense.Text.Trim().Length > 0)
                {
                    dvLicense.Visible = true;
                }
            }
            catch (Exception)
            {
            }            
            DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
            txtQuantity.Text = "1";
            if (dtAppSetting != null)
            {
                DataRow[] drAppSetting = dtAppSetting.Select("strColumnName='QtyDecPlaces'");
                if (drAppSetting.Length > 0)
                {
                    txtQuantity.Text = general.DecimalValue(1, Convert.ToInt32(_dc.chkNull_0(drAppSetting[0]["strColumnValue"].ToString())));
                    hfdecmalPlaces.Value = drAppSetting[0]["strColumnValue"].ToString();
                    Session.Add("decmalPlaces", hfdecmalPlaces.Value);
                }

                hfItemType.Value = "0";
                DataTable dtItem = (DataTable)Session["Dtsku_Price"];
                clsWebFormUtil.FillDropDownList(ddlItem, dtItem, "SKU_ID", "SkuDetail");

                DataRow[] drAppSetting3 = dtAppSetting.Select("strColumnName='OpenRateOnPOS'");
                if (drAppSetting3.Length > 0)
                {
                    hfOpenRate.Value = drAppSetting3[0]["strColumnValue"].ToString();
                }                
                DataRow[] drAppSetting4 = dtAppSetting.Select("strColumnName='IsTaxAuthorityIntegration'");
                if (drAppSetting4.Length > 0)
                {
                    if (drAppSetting4[0]["strColumnValue"].ToString() == "1")
                    {
                        DataTable dtFBR = objCompny.GetFBRIntegration(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
                        Session.Add("dtFBR", dtFBR);
                        if(dtFBR.Rows.Count>0)
                        {
                            Session.Add("IsTaxAuthorityIntegration", "1");
                        }
                        else
                        {
                            Session.Add("IsTaxAuthorityIntegration", "0");
                        }
                    }
                }

                DataRow[] drAppSetting5 = dtAppSetting.Select("strColumnName='IsCustomerInfoMandatory'");
                if (drAppSetting5.Length > 0)
                {
                    if (drAppSetting5[0]["strColumnValue"].ToString() == "1")
                    {
                        hfIsCustomerInfoMandatory.Value = "1";                        
                    }
                }

                DataRow[] drAppSetting6 = dtAppSetting.Select("strColumnName='InvoiceCalculation'");
                if (drAppSetting6.Length > 0)
                {
                    hfInvoiceCalculation.Value = drAppSetting6[0]["strColumnValue"].ToString();                    
                }
            }
            Session.Add("InvoiceCalculation", hfInvoiceCalculation.Value);
            DataTable dtLocation = userControl.SelectSlashUser2(int.Parse(Session["UserId"].ToString()));
            hfAutoPromotion.Value = dtLocation.Rows[0]["PROMOTION_ON"].ToString();
            Session.Add("dtLocation", dtLocation);
            string fileName = Request.MapPath("~/Pics");
            Session.Add("fileName", fileName);
            txtDiscount.Text = "0";
            txtskuCode.Focus();
            LoadSaleForce();
            txtstartDate.Text = (DateTime.Parse(Session["CurrentWorkDate"].ToString())).ToString("dd-MMM-yyyy");
            txtEndDate.Text = (DateTime.Parse(Session["CurrentWorkDate"].ToString())).ToString("dd-MMM-yyyy");
            lblCurrentWorkingDate.Text = (DateTime.Parse(Session["CurrentWorkDate"].ToString())).ToString("dd-MMM-yyyy");
            hfCurrentTime.Value = DateTime.Now.ToString("hh:mm:ss");
            LoadloginDetail();
            txtGrossAmount.Attributes.Add("readonly", "readonly");
            numtxtTotalExtraDiscnt.Attributes.Add("readonly", "readonly");
            numTxtTotalGST.Attributes.Add("readonly", "readonly");
            numTxtTotlAmnt.Attributes.Add("readonly", "readonly");
            txtBalance.Attributes.Add("readonly", "readonly");            
            bool PromotionAllowed = bool.Parse(Session["PROMOTION_ON"].ToString());
            if (PromotionAllowed == true)
            {
                txtDiscount.Enabled = false;
                DrpDiscount.Disabled = true;
                txtExtraDiscountPer.Enabled = false;
                txtExtraDiscountValue.Enabled = false;
            }
            else
            {
                bool disAll = bool.Parse(Session["DISCOUNT_ALLOWD"].ToString());
                if (disAll == true)
                {
                    txtDiscount.Enabled = true;
                    DrpDiscount.Disabled = false;
                    txtExtraDiscountPer.Enabled = true;
                    txtExtraDiscountValue.Enabled = true;
                }
                else
                {
                    txtDiscount.Enabled = false;
                    DrpDiscount.Disabled = true;
                    txtExtraDiscountPer.Enabled = false;
                    txtExtraDiscountValue.Enabled = false;
                }
            }            
            if (ddlCustomer.Items.Count > 0)
            {
                ddlCustomer.SelectedValue = "1";
            }            
            int distributerId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());
            if (Session["IMAGE_PATH"].ToString().Length > 0)
            {
                imgSKU.ImageUrl = "~/Pics/" + distributerId + HttpContext.Current.Session["IMAGE_PATH"].ToString();
            }
            SaleForceController UController = new SaleForceController();
            DataTable dtSMS = UController.SelectSalesForceCustomers(distributerId, Constants.IntNullValue, Constants.LongNullValue, 5);
            Session.Add("dtSMS", dtSMS);
            hfMaxId.InnerText = OrderEntryController.GetMaxInvoiceId(distributerId);         
            if(Session["CanRefund"].ToString() == "No")
            {
                hfCanRefund.Value = "0";
            }
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static string LoadCustomerData()
    {
        DataTable dtCustomer = CustomerDataController.LoadCustomerPromotion(int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()),Convert.ToDateTime(HttpContext.Current.Session["CurrentWorkDate"]));

        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dtCustomer.Rows)
        {
            row = new Dictionary<string, object>();

            foreach (DataColumn col in dtCustomer.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

    [WebMethod]
    [ScriptMethod]
    public static string GetPromotionDetail()
    {
        OrderEntryController or = new OrderEntryController();
        DataTable dt = or.GetPromotion(int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), Convert.ToDateTime(HttpContext.Current.Session["CurrentWorkDate"]));
        return GetJson(dt);
    }

    [WebMethod]
    [ScriptMethod]
    public static string GetGroupDetail()
    {
        SKUGroupController or = new SKUGroupController();
        DataTable dt = or.GetGroupDetail();
        return GetJson(dt);
    }

    [WebMethod]
    [ScriptMethod]
    public static string LoadSaleReport(string type,string userid,string startdate,string enddate)
    {
        var rptSale = new RptSaleController();
        if (type == "1")
        {
            DataTable dtSaleReport = rptSale.SelectSaleReportPOS(int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), int.Parse(userid), DateTime.Parse(startdate), DateTime.Parse(enddate), -1);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dtSaleReport.Rows)
            {
                row = new Dictionary<string, object>();

                foreach (DataColumn col in dtSaleReport.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            return serializer.Serialize(rows);
        }
        else
        {
            return "";
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static void LoadSaleReport2(string type, string userid, string startdate, string enddate)
    {
        var rptSale = new RptSaleController();
        if (type == "1")
        {
            try
            {
                CORNBusinessLayer.Reports.CRPSalesReportSummary CrpReport = new CORNBusinessLayer.Reports.CRPSalesReportSummary();
                CORNBusinessLayer.Classes.DocumentPrintController DPrint = new CORNBusinessLayer.Classes.DocumentPrintController();
                DataTable dt = DPrint.SelectReportTitle(int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()));
                DataSet ds = null;
                ds = rptSale.SelectSaleReport(int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString()), int.Parse(userid), DateTime.Parse(startdate), DateTime.Parse(enddate), -1);
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                if (string.IsNullOrEmpty(_hirarchyNameQuiz))
                {
                    _hirarchyNameQuiz = "Need";
                }
                CrpReport.SetParameterValue("COMPANY_NAME", dt.Rows[0]["COMPANY_NAME"].ToString());
                CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(startdate));
                CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(enddate));
                CrpReport.SetParameterValue("LOCATION", HttpContext.Current.Session["LocationName"].ToString());

                HttpContext.Current.Session.Add("CrpReport", CrpReport);
                HttpContext.Current.Session.Add("ReportType", 0);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }

    #region Clear

    /// <summary>
    /// Clears Some Of Controls
    /// </summary>
    private void ClearDetail()
    {
        try
        {
            txtskuCode.Text = "";
            txtskuName.Text = "";
            txtUnitRate.Text = "";
            hfToggleMode.Value = "SALE MODE";
            if (hfToggleMode.Value != "SALE MODE")
            {
                txtQuantity.Text = "-1";
            }
            else
            {
                txtQuantity.Text = "1";
            }
            txtDiscount.Text = "0";
            txtcolor.Text = "";
            txtsize.Text = "";

          //  btnSave.ToolTip = "Add Sku";
            btnSaveOrder.Enabled = true;

            txtskuCode.Enabled = true;
            txtskuCode.Focus();

        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }

    }

    private void ClearAll()
    {      

    }

    /// <summary>
    /// Clears All Controls
    /// </summary>
    private void ClearMasterAll()
    {
        try
        {
            Session.Remove("CustName");
            Session.Remove("CustCode");
            txtGrossAmount.Text = "";
            numtxtTotalExtraDiscnt.Text = "";
            txtBalance.Text = "";
            numTxtTotalGST.Text = "";
            numTxtTotlAmnt.Text = "";
            txtCashRecieved2.Text = "";
            txtNewCustomer.Text = "";
            txtNewCustomerCOntactNumer.Text = "";
            txtAuthorisedBy.Text = "";
            hfToggleMode.Value = "SALE MODE";
            if (hfToggleMode.Value != "SALE MODE")
            {
                txtQuantity.Text = "-1";
            }
            else
            {
                txtQuantity.Text = "1";
            }
            if (DrpPayMode.SelectedValue == "215" || DrpPayMode.SelectedValue == "218")
            {
                txtCashRecieved2.Text = "";
                txtBalance.Text = numTxtTotlAmnt.Text;
                txtCashRecieved2.Attributes.Add("readonly", "readonly");
            }
            else {
               txtCashRecieved2.ReadOnly = false;
            }      
        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }

    #endregion

    #region Click OPerations

    [WebMethod]
    [ScriptMethod]
    public static void InsertInvoice(string orderedProducts, string amountDue, string author, string discount,string extradiscount, string netAmount, string paidIn,string payType, string Gst, string manualId, string customerId, string saleForce, string NewCustomerNam, string NewCustomerContactNumber,string CustomerName,string CustomerContactNo,string InvoiceCalculation)
    {
        long DistWiseId = 0;
        string SMSAllCustomer = "0";
        string POSSMSTemplate = "0";
        string ShowThankYou = "0";
        var _CustomerCtrl = new RptCustomerController();
        DataSet ds = null;
        try
        {
            DataTable dtAppSetting = (DataTable)HttpContext.Current.Session["dtAppSetting"];
            if (dtAppSetting != null)
            {
                DataRow[] drAppSetting = dtAppSetting.Select("strColumnName='SMSAllCustomer'");
                if (drAppSetting.Length > 0)
                {
                    SMSAllCustomer = drAppSetting[0]["strColumnValue"].ToString();
                }

                drAppSetting = dtAppSetting.Select("strColumnName='POSSMSTemplate'");
                if (drAppSetting.Length > 0)
                {
                    POSSMSTemplate = drAppSetting[0]["strColumnValue"].ToString();
                }
                DataRow[] drAppSetting1 = dtAppSetting.Select("strColumnName='ShowThankOnSalePrint'");
                if (drAppSetting1.Length > 0)
                {
                    ShowThankYou = drAppSetting1[0]["strColumnValue"].ToString();
                }
            }
            DataControl _dc = new DataControl();
            var manualId2 = manualId == "SALE MODE" ? "2" : "1";
            DateTime currentWorkDate = DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString());
            int userId = int.Parse(HttpContext.Current.Session["UserId"].ToString());
            int distributerId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());
            var SaleSKU = (DataTable)JsonConvert.DeserializeObject(orderedProducts, (typeof(DataTable)));
            if (SaleSKU != null && SaleSKU.Rows.Count > 0)
            {
                string InvoiceNumberFBR = "";
                string IsTaxAuthorityIntegration = "0";
                if(HttpContext.Current.Session["IsTaxAuthorityIntegration"] != null)
                {
                    IsTaxAuthorityIntegration = HttpContext.Current.Session["IsTaxAuthorityIntegration"].ToString();
                }
                int PaymentMode = 1;
                if (payType == "214")
                {
                    PaymentMode = 1;//Cash
                }
                else if (payType == "215")
                {
                    PaymentMode = 2;//Credit Card
                }
                else if (payType == "217")
                {
                    PaymentMode = 5;//Mixed
                }
                else
                {
                    PaymentMode = 1;
                }
                if (IsTaxAuthorityIntegration == "1")
                {                    
                    double TaxRate = double.Parse(_dc.chkNull_0(Gst)) / double.Parse(_dc.chkNull_0(amountDue)) * 100;
                    decimal Discount = decimal.Parse(_dc.chkNull_0(discount)) + decimal.Parse(_dc.chkNull_0(extradiscount));
                    if (InvoiceCalculation == "0")
                    {
                        PostDataToFBR(DistWiseId.ToString(), "", "", "", "", PaymentMode, TaxRate, 0, Discount, 1, manualId2, SaleSKU);
                    }
                    else
                    {
                        PostDataToFBRInclusiveGSTPrices(DistWiseId.ToString(), "", "", "", "", PaymentMode, TaxRate, 0, Discount, 1, manualId2, SaleSKU);
                    }
                    InvoiceNumberFBR = HttpContext.Current.Session["InvoiceNumberFBR"].ToString();
                }
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(InvoiceNumberFBR, QRCodeGenerator.ECCLevel.Q);
                Bitmap bitmap = qrCode.GetGraphic(20);
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                if (NewCustomerNam != null && NewCustomerNam != "" && !NewCustomerNam.Equals(""))
                {
                    long CustomerID = SaveNewCustomer(distributerId, currentWorkDate, NewCustomerNam, NewCustomerContactNumber);
                    DistWiseId = OrderEntryController.Add_Invoice2(distributerId, manualId2, 0, CustomerID, CustomerID, 0, Convert.ToInt32(saleForce), 0,decimal.Parse(_dc.chkNull_0(amountDue)), decimal.Parse(_dc.chkNull_0(discount)), decimal.Parse(_dc.chkNull_0(paidIn)), decimal.Parse(_dc.chkNull_0(Gst)), Decimal.Parse(_dc.chkNull_0(netAmount)), decimal.Parse(_dc.chkNull_0(extradiscount)), int.Parse(payType),SaleSKU, userId, 0, currentWorkDate, 0, 0, _dc.chkNull_0(author), PaymentMode, InvoiceNumberFBR, InvoiceCalculation);
                    ds = _CustomerCtrl.PrintInvoice(distributerId, Constants.IntNullValue, 2, DistWiseId, Constants.DateNullValue, Constants.DateNullValue, byteImage);
                    if (SMSAllCustomer == "1")
                    {
                        sendSmsToCUstomer(distributerId, NewCustomerNam, NewCustomerContactNumber, netAmount, POSSMSTemplate, ds.Tables["spCustomerInvoicePrint"].Rows[0]["AMOUNT"].ToString());
                    }
                }
                else
                {
                    DistWiseId = OrderEntryController.Add_Invoice2(distributerId, manualId2, 0, long.Parse(customerId), long.Parse(customerId), 0, Convert.ToInt32(saleForce), 0,decimal.Parse(_dc.chkNull_0(amountDue)), decimal.Parse(_dc.chkNull_0(discount)), decimal.Parse(_dc.chkNull_0(paidIn)), decimal.Parse(_dc.chkNull_0(Gst)), Decimal.Parse(_dc.chkNull_0(netAmount)), decimal.Parse(_dc.chkNull_0(extradiscount)), int.Parse(payType),SaleSKU, userId, 0, currentWorkDate, 0, 0, _dc.chkNull_0(author), PaymentMode, InvoiceNumberFBR,InvoiceCalculation);
                    ds = _CustomerCtrl.PrintInvoice(distributerId, Constants.IntNullValue, 2, DistWiseId, Constants.DateNullValue, Constants.DateNullValue, byteImage);
                    if (SMSAllCustomer=="1")
                    {
                        sendSmsToCUstomer(distributerId, CustomerName, CustomerContactNo, netAmount, POSSMSTemplate, ds.Tables["spCustomerInvoicePrint"].Rows[0]["AMOUNT"].ToString());
                    }
                }

                if(DistWiseId > 0)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportClass crpReport = new CrystalDecisions.CrystalReports.Engine.ReportClass();
                    if (HttpContext.Current.Session["PosReportType"].ToString() == "0")
                    {
                        if (InvoiceCalculation == "0")
                        {
                            crpReport = new CORNBusinessLayer.Reports.CrpPrintInvoice();
                        }
                        else
                        {
                            crpReport = new CORNBusinessLayer.Reports.CrpPrintInvoiceGSTInclusiveInPrice();
                        }
                    }
                    else
                    {
                        crpReport = new CORNBusinessLayer.Reports.CrpPrintInvoice2();
                    }
                    
                    var rcc = new RptCustomerController();
                    var DPrint = new DocumentPrintController();
                    DataTable dt = DPrint.SelectReportTitle(distributerId);
                    
                    DataTable dtNotes = rcc.GetNotes(distributerId);

                    string notes = "";
                    string lastNote = "";
                    if (dtNotes.Rows.Count > 0)
                    {
                        if (HttpContext.Current.Session["PosReportType"].ToString() == "0"
                            && ShowThankYou == "1")
                        {
                            for (int i = 0; i < dtNotes.Rows.Count; i++)
                            {
                                if (i == dtNotes.Rows.Count - 1)
                                {
                                    lastNote = dtNotes.Rows[i]["SLIP_NOTE"].ToString();
                                }
                                else
                                {
                                    notes = notes + ". " + dtNotes.Rows[i]["SLIP_NOTE"].ToString() + "\n";
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtNotes.Rows.Count; i++)
                            {
                                notes = notes + ". " + dtNotes.Rows[i]["SLIP_NOTE"].ToString() + "\n";
                            }
                        }
                    }
                    crpReport.SetDataSource(ds);
                    crpReport.Refresh();
                    crpReport.SetParameterValue("COMPANY_NAME", HttpContext.Current.Session["fileName"].ToString() + dt.Rows[0]["IMAGEPATH"].ToString());
                    DataTable dtLocation = (DataTable)HttpContext.Current.Session["dtLocation"];
                    if (HttpContext.Current.Session["PosReportType"].ToString() == "1")
                    {
                        crpReport.SetParameterValue("Location", dtLocation.Rows[0]["DISTRIBUTOR_NAME"].ToString());
                        crpReport.SetParameterValue("LocationAddress", dtLocation.Rows[0]["address1"].ToString());
                        crpReport.SetParameterValue("LocationAddress2", "Email: " + dtLocation.Rows[0]["address2"].ToString());
                        crpReport.SetParameterValue("LocationContact", "PH: " + dtLocation.Rows[0]["CONTACT_NUMBER"].ToString());
                        crpReport.SetParameterValue("notes", notes);                        
                    }
                    else
                    {
                        crpReport.SetParameterValue("PHONE_NUMBER", dt.Rows[0]["CONTACT_NUMBER"].ToString());
                        crpReport.SetParameterValue("UserLogin", HttpContext.Current.Session["UserName"].ToString());
                        crpReport.SetParameterValue("notes", notes);
                        crpReport.SetParameterValue("lastNote", lastNote);
                        if(InvoiceCalculation == "1")
                        {
                            crpReport.SetParameterValue("NTN", "NTN: " + dtLocation.Rows[0]["NTN_NO"].ToString());
                            crpReport.SetParameterValue("STRN", "STRN: " + dtLocation.Rows[0]["GST_NUMBER"].ToString());
                        }
                    }
                    crpReport.SetParameterValue("DecimalPoint", Convert.ToInt32(HttpContext.Current.Session["decmalPlaces"]));                    
                    crpReport.SetParameterValue("BillType", "Sale Invoice");
                    HttpContext.Current.Session.Add("CrpReport", crpReport);
                    HttpContext.Current.Session.Add("ReportType", 0);
                                        
                }
            }
        }
        catch (Exception ex)
        {            
            ExceptionPublisher.PublishException(ex);
        }
    }

    private static long SaveNewCustomer(int distributerId, DateTime currentWorkDate, string custname, string custnumber)
    {
        CustomerDataController mController = new CustomerDataController();
        DataControl dc = new DataControl();

        SETTINGS_TABLE_Controller mSettingsTableControl = new SETTINGS_TABLE_Controller();
        DataTable dtSettingsTable = mSettingsTableControl.Select_SETTINGS_TABLE("CUSTOMER", "CUSTOMER_ID", distributerId);

        if (dtSettingsTable.Rows.Count > 0)
        {
            long CustomerId = long.Parse(dtSettingsTable.Rows[0]["Value"].ToString()) + 1;
            string StrCode = "";


            if (CustomerId.ToString().Length == 1)
            {
                StrCode = "OT0000" + CustomerId.ToString();
            }
            else if (CustomerId.ToString().Length == 2)
            {
                StrCode = "OT000" + CustomerId.ToString();
            }
            else if (CustomerId.ToString().Length == 3)
            {
                StrCode = "OT00" + CustomerId.ToString();
            }
            else if (CustomerId.ToString().Length == 4)
            {
                StrCode = "OT0" + CustomerId.ToString();
            }
            else if (CustomerId.ToString().Length == 5)
            {
                StrCode = "OT" + CustomerId.ToString();
            }
            long customerId = mController.InsertCustomer(CustomerId, false, true, Constants.IntNullValue, 88, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, distributerId, "", "", custnumber, "", StrCode, custname, "", currentWorkDate, 1, 1, "", "", 0);
            return customerId;
        }
        else
        {
            return Constants.IntNullValue;
        }
    }
    private static void sendSmsToCUstomer(int distributerId, string custname, string custnumber, string value,string POSSMSTemplate, string OrderID)
    {
        try
        {
            DataTable dtSMS = (DataTable)HttpContext.Current.Session["dtSMS"];
            if (dtSMS.Rows.Count > 0)
            {
                string smsMSg = dtSMS.Rows[0]["MESSAGE"].ToString();
                string UserId = dtSMS.Rows[0]["USERID"].ToString();
                string pass = dtSMS.Rows[0]["PASSWORD"].ToString();
                string MsgType = dtSMS.Rows[0]["MASK"].ToString();
                string url = dtSMS.Rows[0]["URL"].ToString();
                string Msg = smsMSg + OrderID + Environment.NewLine + "Total bill Rs. " + value;
                if(POSSMSTemplate=="1")
                {
                    Msg = string.Empty;
                    Msg += "Dear " + custname;
                    Msg += " Thank you for placing your order with " + MsgType;
                    Msg += " Your order number "+ OrderID;
                    Msg += " of the amount " + value;
                    Msg += " has been processed.";
                    Msg += " Regards, " + MsgType;
                }
                string Contact_No = CheckNumber(custnumber);
                SendSMS(Contact_No, Msg, MsgType, url, UserId, pass);
            }
        }
        catch (Exception eeee)
        {
            eeee.ToString();
        }
    }
    public static string CheckNumber(string CNO)
    {
        string Customer_No = "";
        string CONTACT_NO = CNO;
        if (CONTACT_NO.Length == 11) // 0300xxxxxxx
        {
            string str = CNO.Substring(0, 2);
            if (str.ToString() == "03")
            {
                string str1 = CNO.Substring(1, 10);
                Customer_No = "92" + str1;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 12) // 92300xxxxxxx
        {
            string str = CNO.Substring(0, 3);
            if (str.ToString() == "923")
            {
                Customer_No = CNO;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 13) // 920300xxxxxxx
        {
            string str = CNO.Substring(0, 3);
            if (str.ToString() == "920")
            {
                string str1 = CNO.Substring(0, 2);
                string str2 = CNO.Substring(3, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }

        }
        else if (CONTACT_NO.Length == 14) // 0092300xxxxxxx
        {
            string str = CNO.Substring(0, 5);
            if (str.ToString() == "00923")
            {
                string str1 = CNO.Substring(2, 2);
                string str2 = CNO.Substring(4, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 15) // 00920300xxxxxxx
        {
            string str = CNO.Substring(0, 5);
            if (str.ToString() == "00920")
            {
                string str1 = CNO.Substring(2, 2);
                string str2 = CNO.Substring(5, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }
        }
        return Customer_No;
    }
    public static string SendSMS(string customerNo, string msg, string msgType, string Url,string userId,string password)
    {
        //String url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
        String result = "";
        //String message = HttpUtility.UrlEncode("Hello this is a test msg from Ijaz Jamil Akhtar");
        String strPost = "id="+ userId + "&pass="+password+"&msg=" + msg + "&to=" + customerNo + "" + "&mask=" + msgType + "&type=xml&lang=English";
        StreamWriter myWriter = null;
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Url);

        objRequest.Method = "POST";
        objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
        objRequest.ContentType = "application/x-www-form-urlencoded";
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(strPost);
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            myWriter.Close();
        }
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();   // Close and clean up the StreamReader   
            sr.Close();
        }
        return result;
    }
    
    protected void btnVoid_Click(object sender, EventArgs e)//cancel button click 
    {
        ClearAll();
        ClearMasterAll();
        btnToggleMode.Disabled = false;
        txtskuCode.Focus();
    }
    
    /// <summary>
    /// Saves/Updates Order, Invoice And Sale Return
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {
        try
        {
            string manualId = null;
            if (hfToggleMode.Value == "SALE MODE")
            {
                manualId = "2";//for order invoice
            }
            else
            {
                manualId = "1";//for sale return
            }

            DataTable PurchaseSKU = (DataTable)JsonConvert.DeserializeObject(tab.Value, (typeof(DataTable)));
            OrderEntryController mOrderController = new OrderEntryController();

            if (PurchaseSKU.Rows.Count > 0)
            {
                if (btnSaveOrder.ToolTip == "Save")
                {
                    saleInvoiceID = 0;
                    if (saleInvoiceID == -2 || saleInvoiceID == -1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Insertion Failed!!!.');", true);
                    }
                    else
                    {
                        ClearMasterAll();
                        btnToggleMode.Disabled = false;
                        ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
                        CORNBusinessLayer.Reports.CrpPrintInvoice crpReport = new CORNBusinessLayer.Reports.CrpPrintInvoice();
                        DataSet ds = null;
                        ds = rcc.PrintInvoice(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(Session["PRINCIPAL_ID"].ToString()), 2, int.Parse(saleInvoiceID.ToString()), Constants.DateNullValue, Constants.DateNullValue,null);
                        DataTable dtNotes = rcc.GetNotes(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
                        string notes = "";
                        if (dtNotes.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtNotes.Rows.Count; i++)
                            {
                                notes = notes + ". " + dtNotes.Rows[i]["SLIP_NOTE"].ToString() + ". \r";
                            }
                        }
                        crpReport.SetDataSource(ds);
                        crpReport.Refresh();
                        if (string.IsNullOrEmpty(_hirarchyNameQuiz))
                        {
                            _hirarchyNameQuiz = "Need";
                        }
                        crpReport.SetParameterValue("COMPANY_NAME", _hirarchyNameQuiz);
                        crpReport.SetParameterValue("INVOICENO", Convert.ToString(saleInvoiceID));
                        crpReport.SetParameterValue("LOCATION", lblLoacation.Text);
                        crpReport.SetParameterValue("PHONE_NUMBER", CompanyPhonNmbr);
                        crpReport.SetParameterValue("CASHIER", lbluserlogin.Text);
                        crpReport.SetParameterValue("notes", notes);
                        Session.Add("CrpReport", crpReport);
                        Session.Add("ReportType", 0);
                        const string url = "'Default.aspx'";
                        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=500,height=550,left=20,top=20\");</script>";
                        Type cstype = GetType();
                        var cs = Page.ClientScript;
                        cs.RegisterStartupScript(cstype, "OpenWindow", script);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No product added');", true);
            }
            ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
        }
        catch (Exception eee)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred');", true);
        }
    }
    #endregion    
    
    private void CreditLimit()
    {
        CustomerDataController cdCtrl = new CustomerDataController();       
        lblCreditLimit.Text = "0";
        lblLedgerBalance.Text = "0";
        lblAllowLimit.Text = "0";
        if (ddlCustomer.Items.Count > 0)
        {
            DataTable dt = cdCtrl.SelectCustomerCreditBalance(long.Parse(ddlCustomer.SelectedValue), Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()), Constants.Credit);           
            if (dt == null) return;
            if (dt.Rows.Count < 0) return;
            {
                //This Limit is AllowLimit + Ledger Balance
                lblAllowLimit.Text = Convert.ToString(decimal.Parse(_dc.chkNull_0(dt.Rows[0][0].ToString())));
                //This Limit is entered by user while adding customer
                lblCreditLimit.Text = Convert.ToString(decimal.Parse(_dc.chkNull_0(dt.Rows[0][1].ToString())));
                lblLedgerBalance.Text = Convert.ToString(decimal.Parse(_dc.chkNull_0(dt.Rows[0][2].ToString())));
            }
        }
    }

    #region Load Functions

    /// <summary>
    /// Checks SKU in Order Grid
    /// </summary>
    /// <returns>True On Success And False On Failure</returns>
    private bool CheckDublicateSku()
    {
        bool flag = true;

        PurchaseSKU = (DataTable)Session["PurchaseSKU"];

        DataRow[] foundRows2 = PurchaseSKU.Select("SKU_CODE = '" + txtskuCode.Text + "'");

        if (foundRows2.Length != 0)
        {
            foreach (GridViewRow dr2 in GrdPurchase.Rows)
            {
                string code = dr2.Cells[1].Text;
                int chkdel = Convert.ToInt32(dr2.Cells[10].Text);

                if ((txtskuCode.Text == code) && (chkdel == 1))
                {
                    int index = dr2.RowIndex;
                    PurchaseSKU.Rows.RemoveAt(index);
                    flag = true;
                    break;
                }
                else if ((txtskuCode.Text == code) && (chkdel == 0))
                {
                    flag = false;
                    break;
                }
            }
        }
        return flag;
    }
    /// <summary>
    /// Verifies Customer Code
    /// </summary>
    /// <returns>True On Success And False On Failure</returns>
    /// 
    public static string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }
    private void LoadloginDetail()
    {
        try
        {
            DataTable dtNotes = rcc.GetNotes(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
            string notes = "";
            if (dtNotes.Rows.Count > 0)
            {
                for (int i = 0; i < dtNotes.Rows.Count; i++)
                {
                    notes = notes + ". " + dtNotes.Rows[i]["SLIP_NOTE"].ToString() +". <br />" ;
                }
            }
            UserController userControl = new UserController();
            DataTable dt = userControl.SelectSlashUser2(int.Parse(Session["UserId"].ToString()));
            DateTime closingdate = Convert.ToDateTime(Session["CurrentWorkDate"].ToString());
            lbllogintimedate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");// closingdate.ToString("dd-MMM-yyyy hh:mm:ss");////DateTime.Now.ToString("MM/dd/yyyy");
            lbluserlogin.Text = dt.Rows[0]["USER_NAME"].ToString();

            lblLoacation.Text = dt.Rows[0]["DISTRIBUTOR_NAME"].ToString();
            Session.Add("LocationName", lblLoacation.Text);
            hfLocationName.Value = lblLoacation.Text;
            hfLocationPic.Value = "../Pics/" +(Session["DISTRIBUTOR_ID"].ToString()) + dt.Rows[0]["IMAGE_PATH"].ToString(); 
             CompanyName = dt.Rows[0]["COMPANY_NAME"].ToString();
            hfCompanyName.Value = CompanyName;
            CompanyPhonNmbr = dt.Rows[0]["CONTACT_NUMBER"].ToString();//Location Contact Number
            hfContactNo.Value = "PH: "+CompanyPhonNmbr;
         //   hfNots.Value = notes;
            ltrnotes.Text = notes;
            hfPosReportType.Value = (dt.Rows[0]["pos_report"].ToString());
            Session.Add("PosReportType", dt.Rows[0]["pos_report"].ToString());
            AutoComplete.ContextKey = dt.Rows[0]["DISTRIBUTOR_ID"].ToString();
            hfAddess.Value = dt.Rows[0]["address1"].ToString();
            hfaddress2.Value= dt.Rows[0]["address2"].ToString();
            Session.Add("DISTRIBUTOR_ID", dt.Rows[0]["DISTRIBUTOR_ID"].ToString());
            
            DataTable dt2 = userControl.SelectUserPrincipal(int.Parse(Session["UserId"].ToString()));
            if (dt2 != null)
            {
                _hirarchyNameQuiz = dt2.Rows[0]["SKU_HIE_NAME"].ToString();

              //  Session.Add("PRINCIPAL_ID", dt2.Rows[0]["PRINCIPAL_ID"].ToString());
            }

        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }
    

    /// <summary>
    /// Enables/Disables Discount Fields For Manual And Auto Promotion And Loads Promotion Controler For Auto Promotion
    /// </summary>
  
    private void LoadSaleForce()
    {
        Distributor_UserController UController = new Distributor_UserController();
        
        DataTable dt = UController.SelectDistributorUser(37, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(Session["CompanyId"].ToString()));//37 is the ref id  for sale force 
        clsWebFormUtil.FillDropDownList(ddsalesForce, dt, "USER_ID", "USER_NAME");
        ddsalesForce.SelectedValue = Convert.ToString(Session["UserId"]);
        ddl_saleforce2.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(ddl_saleforce2, dt, "USER_ID", "USER_NAME", false);

    }
    public void LoadOnHoldInvoicNumber()
    {
        OrderEntryController or = new OrderEntryController();
        DataTable dtOrder = or.SelectPendingOrder(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), 0, 0, 0, 0, Constants.Order_Pending_Id, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Convert.ToDateTime(Session["CurrentWorkDate"]));
        ((TextBox)(Master.FindControl("lblonHold"))).Text = Convert.ToString(dtOrder.Rows.Count);

    }
    private void LoadProduct()
    {
        try
        {
            SKUPriceDetailController PController = new SKUPriceDetailController();
            DataTable dtProduct = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(Session["CurrentWorkDate"].ToString()));

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                hfProduct.Value = GetJson(dtProduct);
                Session.Add("Dtsku_Price", dtProduct);
            }
           
        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }   

    #endregion

    #region Sale Report

    protected void btnViewSalesReport_Click(object sender, EventArgs e)
    {

        ClearAll();
        ClearMasterAll();

        if (ddlReportType.SelectedValue == "2")
        {
            try
            {

                CORNBusinessLayer.Reports.crpSalesReportPos CrpReport = new CORNBusinessLayer.Reports.crpSalesReportPos();

                DataSet ds = null;
                CORNBusinessLayer.Classes.DocumentPrintController DPrint = new CORNBusinessLayer.Classes.DocumentPrintController();
                DataTable dt = DPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));
                ds = _rptSaleCtl.SelectSaleReport(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(ddl_saleforce2.SelectedValue), DateTime.Parse(txtstartDate.Text), DateTime.Parse(txtEndDate.Text), Constants.LongNullValue);
               
               
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                if (string.IsNullOrEmpty(_hirarchyNameQuiz))
                {
                    _hirarchyNameQuiz = "Need";
                }
                CrpReport.SetParameterValue("COMPANY_NAME", dt.Rows[0]["COMPANY_NAME"].ToString());
                CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtstartDate.Text));
                CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
                CrpReport.SetParameterValue("USER_NAME", Convert.ToString(ddl_saleforce2.SelectedItem.Text));
                CrpReport.SetParameterValue("LOCATION", lblLoacation.Text);
                CrpReport.SetParameterValue("PHONE_NUMBER", CompanyPhonNmbr);

                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", 0);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=450,height=450,left=40,top=40\");</script>";
                Type cstype = GetType();
                var cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        else
        {
            try
            {


                CORNBusinessLayer.Reports.CRPSalesReportSummary CrpReport = new CORNBusinessLayer.Reports.CRPSalesReportSummary();
                CORNBusinessLayer.Classes.DocumentPrintController DPrint = new CORNBusinessLayer.Classes.DocumentPrintController();
                DataTable dt = DPrint.SelectReportTitle(int.Parse(Session["DISTRIBUTOR_ID"].ToString()));

                DataSet ds = null;

                ds = _rptSaleCtl.SelectSaleReport(int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(ddl_saleforce2.SelectedValue), DateTime.Parse(txtstartDate.Text), DateTime.Parse(txtEndDate.Text), -1);

                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();
                if (string.IsNullOrEmpty(_hirarchyNameQuiz))
                {
                    _hirarchyNameQuiz = "Need";
                }
                CrpReport.SetParameterValue("COMPANY_NAME", dt.Rows[0]["COMPANY_NAME"].ToString());
                CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtstartDate.Text));
                CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
                CrpReport.SetParameterValue("LOCATION", lblLoacation.Text);
                
                Session.Add("CrpReport", CrpReport);
                Session.Add("ReportType", 0);
                const string url = "'Default.aspx'";
                const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=450,height=450,left=40,top=40\");</script>";
                Type cstype = GetType();
                var cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
    }

    #endregion

    #region FBR Integration
    [WebMethod]
    [ScriptMethod]
    public static void PostDataToFBR(string InvoiceNo, string ClientNTN, string ClientCNIC, string ClientName, string ClientPhone, int PaymentMode, double TaxRate, decimal FurtherTax, decimal Discount, int InvoiceType,string PaymentType, DataTable dtDetail)
    {
        try
        {
            int multiplyFactor = 1;
            if(PaymentType == "1")
            {
                multiplyFactor = -1;
            }
            DataTable dtFBR = (DataTable)HttpContext.Current.Session["dtFBR"];
            if (dtFBR.Rows.Count > 0)
            {
                InvoiceFBR objInvoice = new InvoiceFBR();
                List<InvoiceFBRDetail> lstItems = new List<InvoiceFBRDetail>();                
                int TotalQty = 0;
                double GrossValue = 0;
                double NetAmount = 0;
                double TotalTax = 0;
                foreach (DataRow dr in dtDetail.Rows)
                {
                    InvoiceFBRDetail ObjInvoiceDetail = new InvoiceFBRDetail();
                    ObjInvoiceDetail.ItemCode = dr["SKU_ID"].ToString();
                    ObjInvoiceDetail.ItemName = dr["SKU_Name"].ToString();
                    ObjInvoiceDetail.Quantity = Convert.ToInt32(dr["QUANTITY_UNIT"]) * multiplyFactor;
                    ObjInvoiceDetail.SaleValue = Convert.ToDouble(dr["AMOUNT"]);
                    ObjInvoiceDetail.TaxCharged = (Convert.ToDouble(dr["AMOUNT"]) * TaxRate / 100) * multiplyFactor;
                    ObjInvoiceDetail.TotalAmount = (Convert.ToDouble(dr["AMOUNT"]) + ObjInvoiceDetail.TaxCharged) * multiplyFactor;
                    ObjInvoiceDetail.TaxRate = TaxRate * multiplyFactor;
                    ObjInvoiceDetail.PCTCode = "10101";
                    ObjInvoiceDetail.FurtherTax = 0;
                    ObjInvoiceDetail.InvoiceType = InvoiceType;//1=New,2=Debit,3=Credit
                    ObjInvoiceDetail.Discount = (Discount / dtDetail.Rows.Count) * multiplyFactor;
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
                if (PaymentType == "2")
                {
                    objInvoice.USIN = InvoiceNo;
                }
                else
                {
                    objInvoice.USIN = InvoiceNo + "Rtrn";
                }
                objInvoice.DateTime = DateTime.Now;
                objInvoice.BuyerNTN = ClientNTN;
                objInvoice.BuyerCNIC = ClientCNIC;
                objInvoice.BuyerName = ClientName;
                objInvoice.BuyerPhoneNumber = ClientPhone;
                objInvoice.PaymentMode = PaymentMode;//1=Cash,2=Card,3=Gift Voucher,4=Loyality Card,5=Mixed,6=Cheque
                objInvoice.TotalSaleValue = GrossValue;
                objInvoice.TotalQuantity = TotalQty;
                objInvoice.TotalBillAmount = NetAmount;
                objInvoice.TotalTaxCharged = TotalTax;
                objInvoice.Discount = Discount * multiplyFactor;
                objInvoice.FurtherTax = FurtherTax * multiplyFactor;
                objInvoice.InvoiceType = InvoiceType;
                if (PaymentType == "2")
                {
                    objInvoice.RefUSIN = null;
                }
                else
                {
                    objInvoice.USIN = InvoiceNo;
                }               

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
                    HttpContext.Current.Session.Add("InvoiceNumberFBR", InvoiceNumberFBR);
                }
                else
                {
                    InvoiceNumberFBR = response.StatusCode.ToString();
                    CodeFBR = response.StatusCode.ToString();
                    HttpContext.Current.Session.Add("InvoiceNumberFBR", InvoiceNumberFBR);
                }
            }
        }
        catch (Exception ex)
        {            
        }
    }
    public static void PostDataToFBRInclusiveGSTPrices(string InvoiceNo, string ClientNTN, string ClientCNIC, string ClientName, string ClientPhone, int PaymentMode, double TaxRate, decimal FurtherTax, decimal Discount, int InvoiceType, string PaymentType, DataTable dtDetail)
    {
        try
        {
            int multiplyFactor = 1;
            if (PaymentType == "1")
            {
                multiplyFactor = -1;
            }
            DataTable dtFBR = (DataTable)HttpContext.Current.Session["dtFBR"];
            if (dtFBR.Rows.Count > 0)
            {
                InvoiceFBR objInvoice = new InvoiceFBR();
                List<InvoiceFBRDetail> lstItems = new List<InvoiceFBRDetail>();
                int TotalQty = 0;
                double GrossValue = 0;
                double NetAmount = 0;
                double TotalTax = 0;
                foreach (DataRow dr in dtDetail.Rows)
                {
                    TaxRate = Convert.ToDouble(dr["GST_RATE"]);
                    InvoiceFBRDetail ObjInvoiceDetail = new InvoiceFBRDetail();
                    ObjInvoiceDetail.ItemCode = dr["SKU_ID"].ToString();
                    ObjInvoiceDetail.ItemName = dr["SKU_Name"].ToString();
                    ObjInvoiceDetail.Quantity = Convert.ToInt32(dr["QUANTITY_UNIT"]) * multiplyFactor;
                    ObjInvoiceDetail.TaxCharged = (Convert.ToDouble(dr["AMOUNT"]) - (Convert.ToDouble(dr["AMOUNT"]) / (100 + Convert.ToDouble(dr["GST_RATE"]))) * 100) * multiplyFactor;
                    ObjInvoiceDetail.SaleValue = (Convert.ToDouble(dr["AMOUNT"]) - ObjInvoiceDetail.TaxCharged) * multiplyFactor;                     
                    ObjInvoiceDetail.TotalAmount = Convert.ToDouble(dr["AMOUNT"]) * multiplyFactor;
                    ObjInvoiceDetail.TaxRate = TaxRate;
                    ObjInvoiceDetail.PCTCode = "10101";
                    ObjInvoiceDetail.FurtherTax = 0;
                    ObjInvoiceDetail.InvoiceType = InvoiceType;//1=New,2=Debit,3=Credit
                    ObjInvoiceDetail.Discount = (Discount / dtDetail.Rows.Count) * multiplyFactor;
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
                if (PaymentType == "2")
                {
                    objInvoice.USIN = InvoiceNo;
                }
                else
                {
                    objInvoice.USIN = InvoiceNo + "Rtrn";
                }
                objInvoice.DateTime = DateTime.Now;
                objInvoice.BuyerNTN = ClientNTN;
                objInvoice.BuyerCNIC = ClientCNIC;
                objInvoice.BuyerName = ClientName;
                objInvoice.BuyerPhoneNumber = ClientPhone;
                objInvoice.PaymentMode = PaymentMode;//1=Cash,2=Card,3=Gift Voucher,4=Loyality Card,5=Mixed,6=Cheque
                objInvoice.TotalSaleValue = GrossValue;
                objInvoice.TotalQuantity = TotalQty;
                objInvoice.TotalBillAmount = NetAmount;
                objInvoice.TotalTaxCharged = TotalTax;
                objInvoice.Discount = Discount * multiplyFactor;
                objInvoice.FurtherTax = FurtherTax * multiplyFactor;
                objInvoice.InvoiceType = InvoiceType;
                if (PaymentType == "2")
                {
                    objInvoice.RefUSIN = null;
                }
                else
                {
                    objInvoice.RefUSIN = InvoiceNo;
                }

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
                    HttpContext.Current.Session.Add("InvoiceNumberFBR", InvoiceNumberFBR);
                }
                else
                {
                    InvoiceNumberFBR = response.StatusCode.ToString();
                    CodeFBR = response.StatusCode.ToString();
                    HttpContext.Current.Session.Add("InvoiceNumberFBR", InvoiceNumberFBR);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            HttpContext.Current.Session.Add("InvoiceNumberFBR", "FBR Server Down");
        }
    }
    #endregion
}