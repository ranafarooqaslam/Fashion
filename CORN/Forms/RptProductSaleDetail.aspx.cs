using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CORNBusinessLayer.Classes;
using System.Data;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System.Text;

public partial class Forms_RptProductSaleDetail : System.Web.UI.Page
{
  
    RptSaleController RptCustomerCtl = new RptSaleController();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadDistributor();
            LoadCategories();
            LoadSubCategories();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }

    }

 
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);
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
    protected void ChbAllCategory_CheckedChanged(object sender, EventArgs e)
    {
        LoadSubCategories();
    }

    protected void LstCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubCategories();
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        DocumentPrintController DPrint = new DocumentPrintController();
        StringBuilder sbDistributorIDs = new StringBuilder();
        if (drpDistributor.SelectedValue == Constants.IntNullValue.ToString())
        {
            foreach (ListItem li in drpDistributor.Items)
            {
                sbDistributorIDs.Append(li.Value);
                sbDistributorIDs.Append(",");
            }
        }
        else
        {
            sbDistributorIDs.Append(drpDistributor.SelectedValue);
        }
        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

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

        DataControl dc = new DataControl();
        ds = RptCustomerCtl.SelectSKUSaleReport(sbDistributorIDs.ToString(), Constants.IntNullValue,
           DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),
           int.Parse(rblRptType.SelectedValue), categoryIds.ToString(), subCategoryIds.ToString());

        ReportDocument CrpReport = new ReportDocument();

        if (int.Parse(rblRptType.SelectedValue) == 0)
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpProductSaleDetail();
        }
        else
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpProductSaleDetailInvoiceWise();
        }
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
       

        this.Session.Add("CrpReport", CrpReport);
        this.Session.Add("ReportType", 0);
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        StringBuilder sbDistributorIDs = new StringBuilder();
        if (drpDistributor.SelectedValue == Constants.IntNullValue.ToString())
        {
            foreach (ListItem li in drpDistributor.Items)
            {
                sbDistributorIDs.Append(li.Value);
                sbDistributorIDs.Append(",");
            }
        }
        else
        {
            sbDistributorIDs.Append(drpDistributor.SelectedValue);
        }
        DocumentPrintController DPrint = new DocumentPrintController();
       
        DataSet ds = null;
        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

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

        DataControl dc = new DataControl();

        ds = RptCustomerCtl.SelectSKUSaleReport(sbDistributorIDs.ToString(), Constants.IntNullValue,
         DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"),
         int.Parse(rblRptType.SelectedValue), categoryIds.ToString(), subCategoryIds.ToString());

        ReportDocument CrpReport = new ReportDocument();

        if (int.Parse(rblRptType.SelectedValue) == 0)
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpProductSaleDetail();
        }
        else
        {
            CrpReport = new CORNBusinessLayer.Reports.CrpProductSaleDetailInvoiceWise();
        }
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
       
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
     

        string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\ProductSaleReport.xls";

        CrpReport.SetDatabaseLogon("sa", "Laislabonitamac2065");

        CrpReport.ExportToDisk(ExportFormatType.Excel, path);

        System.IO.FileInfo file = new System.IO.FileInfo(path);

        if (file.Exists)
        {

            Response.Clear();

            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

            Response.AddHeader("Content-Length", file.Length.ToString());

            Response.ContentType = "application/octet-stream";

            Response.WriteFile(file.FullName);

            Response.End();

        }
        else
        {

            Response.Write("This file does not exist.");

        }
    }
}