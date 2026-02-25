using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Text;
using System.IO;

/// <summary>
/// Form For Petty Expense Report
/// </summary>
public partial class Forms_RptPetyCashStatment : System.Web.UI.Page
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
            this.LoadPrincipal();
            this.LoadAccountParent();
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
        DrpPrincipal.Items.Add(new ListItem("General Expense", "0"));
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    }

    /// <summary>
    /// Loads Parent Accounts To Account Combo
    /// </summary>
    private void LoadAccountParent()
    {
        CORNCommon.Classes.Configuration.GetAccountHead();
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dt = mAccountController.SelectAccountHead(Constants.AC_DetailTypeId, long.Parse(CORNCommon.Classes.Configuration.ExpensDefaultType));
        DrpMasterHead.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(DrpMasterHead, dt, 0, 4);
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        drpDistributor.Items.Clear();   
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        if (RbReportType.SelectedIndex == 1)
        {
            drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        }
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);
    }
    
    /// <summary>
    /// Loads Locations
    /// </summary>
    protected void RbReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadDistributor();
    }

    /// <summary>
    /// Shows Petty Expense Report in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptAccountController RptAccountCtl = new RptAccountController();
        
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

        if (RbReportType.SelectedIndex == 0)
        {
            CORNBusinessLayer.Reports.CrpPetyCashSummary CrpReport = new CORNBusinessLayer.Reports.CrpPetyCashSummary();
            DataSet ds = RptAccountCtl.SelectPetyCashStatment(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()), int.Parse(DrpMasterHead.SelectedValue.ToString()));
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
            CrpReport.SetParameterValue("Distributor", drpDistributor.SelectedItem.Text);
            CrpReport.SetParameterValue("FROM_DATE", txtStartDate.Text);
            CrpReport.SetParameterValue("TO_DATE", txtEndDate.Text);
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            this.Session.Add("CrpReport", CrpReport);
            this.Session.Add("ReportType", 0);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else
        {
            CORNBusinessLayer.Reports.CrpPettyCaahSummery CrpReport = new CORNBusinessLayer.Reports.CrpPettyCaahSummery();

            DataSet ds = RptAccountCtl.SelectPetyCashSummary(int.Parse(drpDistributor.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()));
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("FROM_DATE", txtStartDate.Text);
            CrpReport.SetParameterValue("TO_DATE", txtEndDate.Text);
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            this.Session.Add("CrpReport", CrpReport);
            this.Session.Add("ReportType", 0);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }

    /// <summary>
    /// Shows Petty Expense Report in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        //RptAccountController RptAccountCtl = new RptAccountController();
        //DocumentPrintController mController = new DocumentPrintController();

        //DataSet ds = new DataSet();


        //if (RbReportType.SelectedIndex == 0)
        //{
        //    CORNBusinessLayer.Reports.CrpPetyCashSummary CrpReport = new CORNBusinessLayer.Reports.CrpPetyCashSummary();
        //   //  ds = RptAccountCtl.SelectPetyCashStatmentexc(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()), int.Parse(DrpMasterHead.SelectedValue.ToString()));
           
        //}
        //else
        //{
        //   CORNBusinessLayer.Reports.CrpPettyCaahSummery CrpReport = new CORNBusinessLayer.Reports.CrpPettyCaahSummery();
        //     //ds = RptAccountCtl.SelectPetyCashSummaryexc(int.Parse(drpDistributor.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()));
          
        //}
        RptAccountController RptAccountCtl = new RptAccountController();
        DocumentPrintController mController = new DocumentPrintController();

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

        if (RbReportType.SelectedIndex == 0)
        {
            CORNBusinessLayer.Reports.CrpPetyCashSummaryexc CrpReport = new CORNBusinessLayer.Reports.CrpPetyCashSummaryexc();
            DataSet ds = RptAccountCtl.SelectPetyCashStatment(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()), int.Parse(DrpMasterHead.SelectedValue.ToString()));
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
            CrpReport.SetParameterValue("Distributor", drpDistributor.SelectedItem.Text);
            CrpReport.SetParameterValue("FROM_DATE", txtStartDate.Text);
            CrpReport.SetParameterValue("TO_DATE", txtEndDate.Text);
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            this.Session.Add("CrpReport", CrpReport);
            this.Session.Add("ReportType", 1);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else
        {
            CORNBusinessLayer.Reports.CrpPettyCaahSummery CrpReport = new CORNBusinessLayer.Reports.CrpPettyCaahSummery();
            DataSet ds = RptAccountCtl.SelectPetyCashSummary(int.Parse(drpDistributor.SelectedValue.ToString()), DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()));
            CrpReport.SetDataSource(ds);
            CrpReport.Refresh();

            CrpReport.SetParameterValue("FROM_DATE", txtStartDate.Text);
            CrpReport.SetParameterValue("TO_DATE", txtEndDate.Text);
            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

            this.Session.Add("CrpReport", CrpReport);
            this.Session.Add("ReportType", 1);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    


    //GridView GridView1 = new GridView();
    //GridView1.AllowPaging = false;
    //GridView1.DataSource = ds.Tables[0];
    //GridView1.DataBind();
    //DataTable dt = ds.Tables[0];

    //Response.Clear();
    //Response.Buffer = true;
    //Response.AddHeader("content-disposition",
    //    "attachment;filename=DataTable.csv");
    //Response.Charset = "";
    //Response.ContentType = "application/text";
    //StringBuilder sb = new StringBuilder();
    //for (int k = 0; k < dt.Columns.Count; k++)
    //{
    //    //add separator
    //    sb.Append(dt.Columns[k].ColumnName + ',');
    //}
    ////append new line
    //sb.Append("\r\n");
    //for (int i = 0; i < dt.Rows.Count; i++)
    //{
    //    for (int k = 0; k < dt.Columns.Count; k++)
    //    {
    //        //add separator
    //        sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
    //    }
    //    //append new line
    //    sb.Append("\r\n");
    //}
    //Response.Output.Write(sb.ToString());
    //Response.Flush();
    //Response.End();
    //StringWriter sw = new StringWriter();
    //HtmlTextWriter hw = new HtmlTextWriter(sw);
    //GridView1.RenderControl(hw);
    //Response.Output.Write(sw.ToString());
    //Response.Flush();
    //Response.End();
}
}


