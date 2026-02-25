using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Text;

/// <summary>
/// Form For Stock Valuation Report
/// </summary>
public partial class Forms_RptStockValuation : System.Web.UI.Page
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
            ///this.LoadPrincipal();
            LoadCategories();
            LoadSubCategories();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadCategories()
    {
        LstCategory.Items.Clear();

        SkuHierarchyController sController = new SkuHierarchyController();
        DataTable dt = sController.SelectSKUCategories(Constants.SKUCategory, true);

        //LstAccountHead.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillListBox(LstCategory, dt, 0, 3, false);

        foreach (ListItem item in LstCategory.Items)
        {
            item.Selected = true;
        }
    }
    private void LoadSubCategories()
    {
        LstSubCategory.Items.Clear();

        StringBuilder categoryIds = new StringBuilder();
        for (int i = 0; i < this.LstCategory.Items.Count; i++)
        {
            if (this.LstCategory.Items[i].Selected == true)
            {
                categoryIds.Append(LstCategory.Items[i].Value);
                categoryIds.Append(",");
            }
        }

        SkuHierarchyController sController = new SkuHierarchyController();
        DataTable dt = sController.SelectChildCategories(categoryIds.ToString());

        //LstAccountHead.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillListBox(LstSubCategory, dt, 0, 3, false);

        foreach (ListItem item in LstSubCategory.Items)
        {
            item.Selected = true;
        }
    }
    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    //private void LoadPrincipal()
    //{
    //    SKUPriceDetailController PController = new SKUPriceDetailController();
    //    DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
    //    DrpPrincipal.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));       
    //    clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    //}

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));       
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);
    }

    /// <summary>
    /// Shows Stock Valuation Report in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Shows Stock Valuation Report in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }

    /// <summary>
    /// Shows Stock Valuation Report in Either PDF Or in Excel
    /// </summary>
    /// <param name="p_Report_Type"></param>
    private void ShowReport(int p_Report_Type)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

        int categoryCount = 0;
        int subCategoryCount = 0;

        StringBuilder categoryIds = new StringBuilder();
        StringBuilder subCategoryIds = new StringBuilder();
        for (int i = 0; i < this.LstCategory.Items.Count; i++)
        {
            if (this.LstCategory.Items[i].Selected == true)
            {
                categoryIds.Append(LstCategory.Items[i].Value);
                categoryIds.Append(",");
                categoryCount++;
            }
        }

        for (int i = 0; i < this.LstSubCategory.Items.Count; i++)
        {
            if (this.LstSubCategory.Items[i].Selected == true)
            {
                subCategoryIds.Append(LstSubCategory.Items[i].Value);
                subCategoryIds.Append(",");
                subCategoryCount++;
            }
        }

        if (rblReportType.SelectedValue == "0" && subCategoryCount == 0 && categoryCount == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg",
              "alert('Please select Category Or Sub Category');", true);

            return;
        }

        DataSet ds = RptInventoryCtl.SelectStockValuation(DateTime.Parse(txtEndDate.Text), 
            int.Parse(drpDistributor.SelectedValue.ToString()),
            Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()),
            Convert.ToInt32(rblReportType.SelectedValue), categoryIds.ToString(), subCategoryIds.ToString());

        if (rblReportType.SelectedValue == "0")
        {
            if (drpDistributor.SelectedItem.Text == "All")
            {
                CORNBusinessLayer.Reports.CrpStockValuationDetailAllLocations CrpReportDetailAll = new CORNBusinessLayer.Reports.CrpStockValuationDetailAllLocations();
                CrpReportDetailAll.SetDataSource(ds);
                CrpReportDetailAll.Refresh();
                CrpReportDetailAll.SetParameterValue("division", drpDistributor.SelectedItem.Text);
                CrpReportDetailAll.SetParameterValue("todate", txtEndDate.Text);
                CrpReportDetailAll.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
               // CrpReportDetailAll.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
                this.Session.Add("CrpReport", CrpReportDetailAll);
                this.Session.Add("ReportType", p_Report_Type);
                string url = "'Default.aspx'";
                string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = this.GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            else
            {
                CORNBusinessLayer.Reports.CrpStockValuationDetailSingleLocations CrpReportDetailSingle = new CORNBusinessLayer.Reports.CrpStockValuationDetailSingleLocations();
                CrpReportDetailSingle.SetDataSource(ds);
                CrpReportDetailSingle.Refresh();
                CrpReportDetailSingle.SetParameterValue("division", drpDistributor.SelectedItem.Text);
                CrpReportDetailSingle.SetParameterValue("todate", txtEndDate.Text);
                CrpReportDetailSingle.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
                //CrpReportDetailSingle.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
                this.Session.Add("CrpReport", CrpReportDetailSingle);
                this.Session.Add("ReportType", p_Report_Type);
                string url = "'Default.aspx'";
                string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = this.GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
        }
        else
        {
            CORNBusinessLayer.Reports.CrpStockValuationSummary CrpReportStockValuationSummary = new CORNBusinessLayer.Reports.CrpStockValuationSummary();
            CrpReportStockValuationSummary.SetDataSource(ds);
            CrpReportStockValuationSummary.Refresh();
            CrpReportStockValuationSummary.SetParameterValue("division", drpDistributor.SelectedItem.Text);
            CrpReportStockValuationSummary.SetParameterValue("todate", txtEndDate.Text);
            CrpReportStockValuationSummary.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            this.Session.Add("CrpReport", CrpReportStockValuationSummary);
            this.Session.Add("ReportType", p_Report_Type);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }
    protected void rblReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblReportType.SelectedValue == "0")
        {
            categoryRow.Visible = true;
            catgeorySelectAllRow.Visible = true;
        }
        else
        {
            categoryRow.Visible = false;
            catgeorySelectAllRow.Visible = false;
        }
    }

    protected void ChbAllCategory_CheckedChanged(object sender, EventArgs e)
    {
        LoadSubCategories();
    }

    protected void LstCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubCategories();
    }
}
