using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Text;

/// <summary>
/// Form For Date Wise Discount Report
/// </summary>
public partial class Forms_RptInvoiceItemWiseDiscount : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadDistributor();
            this.LoadPrincipal();
            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));       
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    }

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
    /// Shows Date Wise Discount in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        showReport(0);
    }

    /// <summary>
    /// Shows Date Wise Discount in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
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
        RptSaleController RptSaleCtl = new RptSaleController();

        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        DataSet ds = RptSaleCtl.SelectDateWiseDiscount(DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), sbDistributorIDs.ToString(), int.Parse(DrpPrincipal.SelectedValue.ToString()), int.Parse(RadioButtonList1.SelectedValue));
        CORNBusinessLayer.Reports.CrpInvoiceItemWiseDiscount CrpReport = new CORNBusinessLayer.Reports.CrpInvoiceItemWiseDiscount();

        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        CrpReport.SetParameterValue("LOCATION", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("princple", DrpPrincipal.SelectedItem.Text);
        if (int.Parse(RadioButtonList1.SelectedValue) == 3)
        {
            CrpReport.SetParameterValue("reportName", "Invoice Wise Discount Report");
        }
        if (int.Parse(RadioButtonList1.SelectedValue) == 4)
        {
            CrpReport.SetParameterValue("reportName", "Item Wise Discount Report");
        }
        CrpReport.SetParameterValue("user", Session["UserName"].ToString());//
        string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\ExportedFile.xls";

        CrpReport.SetDatabaseLogon("sa", "Laislabonitamac2065");

        CrpReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, path);

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
    private void showReport(int repttype)
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
        RptSaleController RptSaleCtl = new RptSaleController();

        DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        DataSet ds = RptSaleCtl.SelectDateWiseDiscount(DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), sbDistributorIDs.ToString(), int.Parse(DrpPrincipal.SelectedValue.ToString()), int.Parse(RadioButtonList1.SelectedValue));
        if (int.Parse(RadioButtonList1.SelectedValue) == 3)
        {
            CORNBusinessLayer.Reports.CrpInvoiceitemwiseDiscReport CrpReport = new CORNBusinessLayer.Reports.CrpInvoiceitemwiseDiscReport();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("LOCATION", drpDistributor.SelectedItem.Text);
            CrpReport.SetParameterValue("princple", DrpPrincipal.SelectedItem.Text);
            
                CrpReport.SetParameterValue("reportName", "Invoice Wise Discount Report");
          
            CrpReport.SetParameterValue("user", Session["UserName"].ToString());//
            this.Session.Add("ReportType", repttype);
            this.Session.Add("CrpReport", CrpReport);
        }
        if (int.Parse(RadioButtonList1.SelectedValue) == 4)
        {
            CORNBusinessLayer.Reports.CrpInvoiceItemWiseDiscount CrpReport = new CORNBusinessLayer.Reports.CrpInvoiceItemWiseDiscount();
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            CrpReport.SetParameterValue("LOCATION", drpDistributor.SelectedItem.Text);
            CrpReport.SetParameterValue("princple", DrpPrincipal.SelectedItem.Text);
            CrpReport.SetParameterValue("reportName", "Item Wise Discount Report");

            
            CrpReport.SetParameterValue("user", Session["UserName"].ToString());//
            this.Session.Add("ReportType", repttype);
            this.Session.Add("CrpReport", CrpReport);
        }
        
        string url = "'Default.aspx'";
        string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
}
