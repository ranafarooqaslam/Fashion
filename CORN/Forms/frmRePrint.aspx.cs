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

public partial class Forms_frmRePrint : System.Web.UI.Page
{
    readonly RptCustomerController _CustomerCtrl = new RptCustomerController();
    UserController userControl = new UserController();
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
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);


    }

    protected void btnRePrint_Click(object sender, EventArgs e)
    {
        LoadGridInvoices();
    }
    private void LoadGridInvoices()
    {

        if (drpDistributor.Items.Count > 0)
        {
            long InvoiceID = 0;
            if(txtInvoiceNo.Text.Length > 0)
            {
                InvoiceID = Convert.ToInt64(txtInvoiceNo.Text);
            }
            DataTable piDt = _CustomerCtrl.SelectCustomerInvoicePrint(int.Parse(drpDistributor.SelectedValue), Constants.IntNullValue, 0,InvoiceID, DateTime.Parse(txtstartDate.Text), DateTime.Parse(txtEndDate.Text));
            GrdPrintInvoice.DataSource = piDt;
            GrdPrintInvoice.DataBind();
        }
    }
    protected void GrdPrintInvoice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int DecimalPoint = 0;
        string ShowThankYou = "0";
        string InvoiceCalculation = "0";
        int ReportType = Convert.ToInt32(dtLocation.Rows[0]["pos_report"]);
        DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
        if (dtAppSetting != null)
        {
            DataRow[] drAppSetting = dtAppSetting.Select("strColumnName='QtyDecPlaces'");
            if (drAppSetting.Length > 0)
            {
                DecimalPoint = Convert.ToInt32(drAppSetting[0]["strColumnValue"]);
            }

            DataRow[] drAppSetting1 = dtAppSetting.Select("strColumnName='ShowThankOnSalePrint'");
            if (drAppSetting1.Length > 0)
            {
                ShowThankYou = drAppSetting1[0]["strColumnValue"].ToString();
            }
            DataRow[] drAppSetting2 = dtAppSetting.Select("strColumnName='InvoiceCalculation'");
            if (drAppSetting2.Length > 0)
            {
                InvoiceCalculation = drAppSetting2[0]["strColumnValue"].ToString();
            }
        }

        RptCustomerController rcc = new RptCustomerController();
        DocumentPrintController DPrint = new DocumentPrintController();
        int did = int.Parse(drpDistributor.SelectedValue);
        if (did == 1234)
        {
            did = Constants.IntNullValue;
        }
        DataTable dt = DPrint.SelectReportTitle(did);
        int sale_inv_id = Convert.ToInt32(GrdPrintInvoice.Rows[e.RowIndex].Cells[0].Text);
        CrystalDecisions.CrystalReports.Engine.ReportClass crpReport = new CrystalDecisions.CrystalReports.Engine.ReportClass();
        if(ReportType == 0)
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
        DataSet ds = null;
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(GrdPrintInvoice.Rows[e.RowIndex].Cells[10].Text, QRCodeGenerator.ECCLevel.Q);
        Bitmap bitmap = qrCode.GetGraphic(20);
        MemoryStream ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        byte[] byteImage = ms.ToArray();

        ds = _CustomerCtrl.PrintInvoice(did, Constants.IntNullValue, 2, int.Parse(sale_inv_id.ToString()), Constants.DateNullValue, Constants.DateNullValue, byteImage);
        DataTable dtNotes = rcc.GetNotes(did);
        string notes = "";
        string lastNote = "";
        if (dtNotes.Rows.Count > 0)
        {
            if (ReportType == 0 && ShowThankYou == "1")
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
        var fileName = Request.MapPath("~/Pics");
        crpReport.SetParameterValue("COMPANY_NAME", fileName + dt.Rows[0]["IMAGEPATH"].ToString());
        if (ReportType == 1)
        {
            crpReport.SetParameterValue("Location", dtLocation.Rows[0]["DISTRIBUTOR_NAME"].ToString());
            crpReport.SetParameterValue("LocationAddress", dtLocation.Rows[0]["address1"].ToString());
            crpReport.SetParameterValue("LocationAddress2", "Email: " + dtLocation.Rows[0]["address2"].ToString());
            crpReport.SetParameterValue("LocationContact", "PH: " +  dtLocation.Rows[0]["CONTACT_NUMBER"].ToString());
            crpReport.SetParameterValue("notes", notes);
        }
        else
        {            
            crpReport.SetParameterValue("PHONE_NUMBER", dt.Rows[0]["CONTACT_NUMBER"].ToString());
            crpReport.SetParameterValue("UserLogin", Session["UserName"].ToString());
            crpReport.SetParameterValue("notes", notes);
            crpReport.SetParameterValue("lastNote", lastNote);
            if (InvoiceCalculation == "1")
            {
                crpReport.SetParameterValue("NTN", "NTN: " + dtLocation.Rows[0]["NTN_NO"].ToString());
                crpReport.SetParameterValue("STRN", "STRN: " + dtLocation.Rows[0]["GST_NUMBER"].ToString());
            }
        }
        crpReport.SetParameterValue("DecimalPoint", DecimalPoint);        
        crpReport.SetParameterValue("BillType", "Duplicate Bill");
        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=500,height=550,left=20,top=40\");</script>";
        Type cstype = GetType();
        var cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void GrdPrintInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        var lnkbtn = (LinkButton)e.Row.FindControl("lnkbtn_print");
        var mgr = ScriptManager.GetCurrent(Page);
        if (mgr != null)
            mgr.RegisterPostBackControl(lnkbtn);


        e.Row.BackColor = e.Row.Cells[7].Text == "Sales Refund" ? Color.Lavender : Color.White;
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridInvoices();
    }
}