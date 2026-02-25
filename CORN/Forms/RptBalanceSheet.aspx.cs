using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form For Gross Profit Report
/// </summary>
public partial class Forms_RptBalanceSheet : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    readonly DistributorController _dController = new DistributorController();

    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
       
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        this.drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);
    }

    /// <summary>
    /// Shows Gross Profit Report in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {

        DataSet ds = _rptSaleCtl.GetBalanceSheet(0, Convert.ToInt32(drpDistributor.SelectedValue), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));

        DataTable dt = null;
        if (drpDistributor.SelectedIndex == 0)

        { dt = _dPrint.SelectReportTitle(Constants.IntNullValue); }

        else

        { dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue)); }

        CrystalDecisions.CrystalReports.Engine.ReportDocument crpReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.ReportDocument srAssets = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.ReportDocument srLiabilities = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

        if (DrpLevel.SelectedValue == "4")
        {
            crpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet4();
        }
        else if (DrpLevel.SelectedValue == "3")
        {
            crpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet3();
        }
        else if (DrpLevel.SelectedValue == "2")
        {
            crpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet2();
        }
        else if (DrpLevel.SelectedValue == "1")
        {
            crpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet1();
        }

        srAssets = crpReport.OpenSubreport("srAssets");
        srLiabilities = crpReport.OpenSubreport("srLiabilities");
        //CrpReport.SetDataSource(ds);
        srAssets.SetDataSource(ds);
        srLiabilities.SetDataSource(ds);

        crpReport.Refresh();

        crpReport.SetParameterValue("Company_name", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("FromDate", Convert.ToDateTime(txtStartDate.Text));
        crpReport.SetParameterValue("ToDate", Convert.ToDateTime(txtEndDate.Text));

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    /// <summary>
    /// Shows Gross Profit Report in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {

       

        DataSet ds = _rptSaleCtl.GetBalanceSheet(0, Convert.ToInt32(drpDistributor.SelectedValue), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));

        DataTable dt = null;
        if (drpDistributor.SelectedIndex == 0)
        { dt = _dPrint.SelectReportTitle(Constants.IntNullValue); }
        else
        { dt = _dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue)); }

        CrystalDecisions.CrystalReports.Engine.ReportDocument CrpReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.ReportDocument srAssets = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.ReportDocument srLiabilities = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

        if (DrpLevel.SelectedValue == "4")
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet4();
        }
        else if (DrpLevel.SelectedValue == "3")
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet3();
        }
        else if (DrpLevel.SelectedValue == "2")
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet2();
        }
        else if (DrpLevel.SelectedValue == "1")
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpBalanceSheet1();
        }

        srAssets = CrpReport.OpenSubreport("srAssets");
        srLiabilities = CrpReport.OpenSubreport("srLiabilities");
        //CrpReport.SetDataSource(ds);
        srAssets.SetDataSource(ds);
        srLiabilities.SetDataSource(ds);

        CrpReport.Refresh();

        CrpReport.SetParameterValue("Company_name", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("FromDate", Convert.ToDateTime(txtStartDate.Text));
        CrpReport.SetParameterValue("ToDate", Convert.ToDateTime(txtEndDate.Text));

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 1);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}
