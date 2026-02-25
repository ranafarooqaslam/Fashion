using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using CrystalDecisions.CrystalReports.Engine;


public partial class Forms_frmVendorBalanceSummary : System.Web.UI.Page
{
   
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadCreditCustomer();

            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

        }
    }

    

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2);
    }


    /// <summary>
    /// Loads Credit Customers To Customer Combo
    /// </summary>
    private void LoadCreditCustomer()
    {
        DrpCustomer.Items.Clear();

        VenderEntryController VendorCtl = new VenderEntryController();

        try
        {
            DataTable dtVendor = VendorCtl.GetVendor(Constants.IntNullValue);

            if (dtVendor != null)
            {
                DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
                clsWebFormUtil.FillDropDownList(DrpCustomer, dtVendor, 0, 2);
            }
        }
        catch (Exception)
        {

            throw;
        }
        //DrpCustomer.Items.Clear();

        //SKUPriceDetailController PController = new SKUPriceDetailController();
        //DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        //DrpCustomer.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        //clsWebFormUtil.FillDropDownList(this.DrpCustomer, m_dt, 0, 1);
    }

    /// <summary>
    /// Shows Credit Report in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Shows Credit Report in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }

    /// <summary>
    /// Shows Credit Report Either in PDF Or in Excel
    /// </summary>
    /// <param name="p_Report_Type">ReportType</param>
    private void ShowReport(int p_Report_Type)
    {
        RptCustomerController RptCustomerCtl = new RptCustomerController();
        DocumentPrintController mController = new DocumentPrintController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        ReportDocument CrpReport = new ReportDocument();
     CrpReport = new CrpVendorBalanceSummary();
       DataSet ds = RptCustomerCtl.SelectVendorBalanceSummary(int.Parse(drpDistributor.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text)
            , int.Parse(Session["UserId"].ToString()),int.Parse(DrpCustomer.SelectedValue.ToString()),6, 0,0);        
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("Distributor", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("from_date", txtStartDate.Text);
        CrpReport.SetParameterValue("To_date", txtEndDate.Text); 
             
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("puser", Session["UserName2"].ToString());
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", p_Report_Type);
        const string url = "'MobileReports.aspx'";
        const  string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

 
}