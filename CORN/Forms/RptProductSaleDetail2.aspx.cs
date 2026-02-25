using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CORNBusinessLayer.Classes;
using System.Data;
using CORNCommon.Classes;
using System.Text;

public partial class Forms_RptProductSaleDetail2 : System.Web.UI.Page
{

    RptSaleController RptCustomerCtl = new RptSaleController();

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

    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);
    }



    protected void btnViewPDF_Click(object sender, EventArgs e)
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

        DataControl dc = new DataControl();
        ds = RptCustomerCtl.SelectSKUSaleReport(sbDistributorIDs.ToString(), int.Parse(DrpPrincipal.SelectedValue.ToString()),
           DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 1, "", "");

        CORNBusinessLayer.Reports.CrpProductSaleDetail2 CrpReport = new CORNBusinessLayer.Reports.CrpProductSaleDetail2();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
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

        DataControl dc = new DataControl();

        ds = RptCustomerCtl.SelectSKUSaleReport(sbDistributorIDs.ToString(), int.Parse(DrpPrincipal.SelectedValue.ToString()),
         DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), 1, "", "");

        CORNBusinessLayer.Reports.CrpProductSaleDetail2 CrpReport = new CORNBusinessLayer.Reports.CrpProductSaleDetail2();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();

        CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
        CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
        CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
        CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);

        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());


        string path = CORNCommon.Classes.Configuration.GetAppInstallationPath() + "\\Exported.xls";

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