using System;
using System.Web;
using System.Web.UI;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;

public partial class Forms_rptItemLedger : System.Web.UI.Page
{
    readonly SkuController SKUCtl = new SkuController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");

            LoadDistributor();
            LoadSKU();
        }

    }
    private void LoadSKU()
    {
        ddlSKU.Items.Clear();
        DataTable dt = SKUCtl.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow r in dt.Rows)
            {
                r["SKU_NAME"] = Convert.ToString(r["SKU_CODE"]) + " - " + Convert.ToString(r["SKU_NAME"]) + " - " + Convert.ToString(r["PACKSIZE"]);
            }

            clsWebFormUtil.FillDropDownList(ddlSKU, dt, "SKU_ID", "SKU_NAME");
            ddlSKU.SelectedIndex = 0;
        }
    }

    private void showReport(int Type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        ReportDocument CrpReport = new CORNBusinessLayer.Reports.CrpSKULedger();
        string location = string.Empty;
        if (drpDistributor.SelectedItem.Value.ToString() == Constants.IntNullValue.ToString())
        {
            foreach (ListItem li in drpDistributor.Items)
            {
                location += li.Value + ",";
            }
        }
        else
        {
            location = drpDistributor.SelectedItem.Value.ToString();
        }
        DataTable dt = mController.SelectReportTitle(Convert.ToInt32(drpDistributor.SelectedItem.Value));
        DataSet ds = null;
        DataTable dtOpening = RptInventoryCtl.GetSKULedgerDataOpening(
            Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text + " 23:59:59"),
            Constants.IntNullValue, location, Convert.ToInt32(ddlSKU.SelectedItem.Value),
            Constants.IntNullValue);

        decimal OpeningUnit = 0;
        decimal OpeningBalance = 0;
        if(dtOpening.Rows.Count>0)
        {
            OpeningUnit = Convert.ToDecimal(dtOpening.Rows[0]["Opening"]);
            OpeningBalance = Convert.ToDecimal(dtOpening.Rows[0]["DISTRIBUTOR_PRICE"]);
        }
        ds = RptInventoryCtl.GetSKULedgerData(Convert.ToDateTime(txtStartDate.Text),
            Convert.ToDateTime(txtEndDate.Text + " 23:59:59"), Constants.IntNullValue,
            location, Convert.ToInt32(ddlSKU.SelectedItem.Value), Constants.IntNullValue);

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("FromDate", txtStartDate.Text);
        CrpReport.SetParameterValue("To_date", txtEndDate.Text);
        CrpReport.SetParameterValue("UserName", Session["UserName"].ToString());
        CrpReport.SetParameterValue("OpeningUnit", OpeningUnit);
        CrpReport.SetParameterValue("OpeningBalance", OpeningBalance);
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", Type);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        showReport(0);
      
       
    }
    
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        showReport(1);
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }    
}