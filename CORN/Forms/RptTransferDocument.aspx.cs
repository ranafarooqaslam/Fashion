using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

public partial class Forms_RptTransferDocument : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

   

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        drpDistributor.Items.Clear();
        drpDistributorTo.Items.Clear();

        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

        if (RdbTransferType.SelectedValue == "100")
            this.drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));

        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);

        this.drpDistributorTo.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributorTo, dt, 0, 2);
    }

    /// <summary>
    /// Shows Purchase Document in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        

        ReportDocument CrpReport = new ReportDocument();

        if (RdbTransferType.SelectedValue == "100")
        {
            DataSet ds = RptInventoryCtl.SelectTransferInOutSummary(int.Parse(drpDistributor.SelectedValue.ToString()),
                int.Parse(drpDistributorTo.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"),
                DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(RdbTransferType.SelectedValue));

            CrpReport = new CORNBusinessLayer.Reports.CrpTransferInOutSummary();

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("DocumentType", RdbTransferType.SelectedItem.Text);
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        }
        else
        {
            DataSet ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.LongNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(RdbTransferType.SelectedValue));

            if (chbWithImage.Checked)
            {

                if (drpReportType.SelectedValue == "1")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocumentImage();
                }
                else
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument2Image();
                }
            }
            else
            {
                if (drpReportType.SelectedValue == "1")
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument();
                }
                else
                {
                    CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument2();
                }
            }

            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();
            string imgfolder = Server.MapPath("~/SkuImages/").ToString();

            CrpReport.SetParameterValue("DocumentType", RdbTransferType.SelectedItem.Text + " " + "Document");
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("imgfolder", imgfolder);
        }
        
        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    /// <summary>
    /// Shows Purchase Document in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
      
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        DataSet ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.LongNullValue, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(RdbTransferType.SelectedValue));

        ReportDocument CrpReport = new ReportDocument();
        if (drpReportType.SelectedValue == "1")
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument();
        }
        else
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument2();
        }
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("DocumentType", RdbTransferType.SelectedItem.Text + " " + "Document");
        
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 1);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void RdbTransferType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RdbTransferType.SelectedValue == "100")
        {
            rptTypeRow.Visible = false;
            chbWithImage.Visible = false;
            tolocationRow.Visible = true;
            lbllocation.InnerText = "From Location";
        }
        else
        {
            rptTypeRow.Visible = true;
            chbWithImage.Visible = true;
            tolocationRow.Visible = false;
            lbllocation.InnerText = "Location";
        }

        LoadDistributor();
    }
}