using System;
using System.Data;
using System.Web.UI;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;

/// <summary>
/// Form For General Ledger Report
/// </summary>
public partial class Forms_RptLedgerReport2 : System.Web.UI.Page
{

    static string opType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadVendor();
            LoadDistributor();

            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    private void LoadVendor()
    {

        VenderEntryController VendorCtl = new VenderEntryController();

        try
        {
            DataTable dtVendor = VendorCtl.GetVendor(Constants.IntNullValue);

            if (dtVendor != null)
            {
                clsWebFormUtil.FillDropDownList(drpVendor, dtVendor, 0, 2, true);
            }
        }
        catch (Exception)
        {
            
            throw;
        }

    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();

        try
        {
            DataTable dt3 = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

            clsWebFormUtil.FillDropDownList(this.drpDistributor, dt3, 0, 2, true);
        }
        catch (Exception)
        {
            
            throw;
        }
        
    }

   
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        VenderEntryController RptCustCtl = new VenderEntryController();

        DataSet ds = null;

        {
            ds = RptCustCtl.GetVendorLedger(int.Parse(drpVendor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()),
                      DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));

            DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
           
            CrpVendorLedger CrpReport = new CrpVendorLedger();
            ReportDocument subReport = CrpReport.OpenSubreport("SubReport");

            CrpReport.SetDataSource(ds);
            subReport.SetDataSource(ds);

            CrpReport.Refresh();

            CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
            CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
            CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
           
            CrpReport.SetParameterValue("Vendor", drpVendor.SelectedItem.Text);
            CrpReport.SetParameterValue("Op_Balance", LoadVendoerOpBalance());
            CrpReport.SetParameterValue("opType", opType);
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }

    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        
        DocumentPrintController DPrint = new DocumentPrintController();
        VenderEntryController RptCustCtl = new VenderEntryController();

        DataSet ds = null;

        {
      
            DataControl dc = new DataControl();
            ds = RptCustCtl.GetVendorLedger(int.Parse(drpVendor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()),
                      DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"));

            DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

            CrpVendorLedger CrpReport = new CrpVendorLedger();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("FromDate", DateTime.Parse(txtStartDate.Text));
            CrpReport.SetParameterValue("To_date", DateTime.Parse(txtEndDate.Text));
            CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            CrpReport.SetParameterValue("Vendor", drpVendor.SelectedItem.Text);
            CrpReport.SetParameterValue("Op_Balance", LoadVendoerOpBalance());
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("opType", opType);

            Session.Add("CrpReport", CrpReport);
            Session.Add("ReportType", 1);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }

    private decimal LoadVendoerOpBalance()
    {
        if (drpDistributor.Items.Count > 0 && drpVendor.Items.Count > 0)
        {
            VenderEntryController mController = new VenderEntryController();
            DataTable dt = mController.GetVendoerOpening(int.Parse(drpVendor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()),
                      DateTime.Parse(txtStartDate.Text + " 00:00:00"));
            if (decimal.Parse(dt.Rows[0][0].ToString()) > 0)
            {
                opType = "CR";
                return decimal.Parse(dt.Rows[0][0].ToString());
            }
            else if ((decimal.Parse(dt.Rows[0][0].ToString()) == 0))
            {
                opType = "";
                return decimal.Parse(dt.Rows[0][0].ToString());
            }
            else
            {
                return decimal.Parse(dt.Rows[0][0].ToString()) * -1;
            }

        }
        return 0;
    }
}