using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;    
using CORNBusinessLayer.Reports;

/// <summary>
/// Form For Sale Person DSR Report
/// </summary>
public partial class Forms_frmSalePersonDSR : System.Web.UI.Page
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
            this.DistributorType();
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
        DrpPrincipal.Items.Add(new ListItem("All",Constants.IntNullValue.ToString()));       
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    }

    private void DistributorType()
    {
        DistributorController dController = new DistributorController();

        DataTable dt = dController.SelectDistributorTypeInfo(Constants.IntNullValue);
        ddDistributorType.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(ddDistributorType, dt, 0, 2);
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
       // DistributorController DController = new DistributorController();
        UserController mUserController = new UserController();
        drpDistributor.Items.Clear();
        if (ddDistributorType.Items.Count > 0)
        {
            //DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
            //drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));

            //clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2);

           
            DataTable dt = mUserController.SelectUserAssignment(int.Parse(this.Session["UserId"].ToString()), int.Parse(ddDistributorType.SelectedValue.ToString()), 1, int.Parse(this.Session["CompanyId"].ToString()));
            drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 1);
        }
    }

    /// <summary>
    /// Shows Sale Person DSR in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }

    /// <summary>
    /// Shows Sale Person DSR in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        ShowReport(1);
    }


    private void ShowReport(int p_ReprotType)
    {
            DocumentPrintController DPrint = new DocumentPrintController();
            RptSaleController RptSaleCtl = new RptSaleController();
            DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
            DataSet ds = null;

        try
        {
            if (RbReportType.SelectedIndex == 0)
            {
                DataControl dc = new DataControl();
                ds = RptSaleCtl.SelectSalePersonDSR(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString())
                , RbReportType.SelectedIndex, int.Parse(ddDistributorType.SelectedValue));

                CORNBusinessLayer.Reports.CrpSalePersonDSR CrpReport = new CORNBusinessLayer.Reports.CrpSalePersonDSR();
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();

                CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
                CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
                CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
                CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
                CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

                this.Session.Add("CrpReport", CrpReport);
                this.Session.Add("ReportType", p_ReprotType);
                string url = "'Default.aspx'";
                string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = this.GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            else if (RbReportType.SelectedIndex == 1)
            {
                DataControl dc = new DataControl();
                ds = RptSaleCtl.SelectSalePersonDSR(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString())
                , RbReportType.SelectedIndex,  int.Parse(ddDistributorType.SelectedValue));

                CORNBusinessLayer.Reports.CrpSalePersonDSR2 CrpReport = new CORNBusinessLayer.Reports.CrpSalePersonDSR2();
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();

                CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
                CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
                CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
                CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
                CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

                this.Session.Add("CrpReport", CrpReport);
                this.Session.Add("ReportType", p_ReprotType);
                string url = "'Default.aspx'";
                string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = this.GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
            else
            {
                DataControl dc = new DataControl();
                ds = RptSaleCtl.SelectSalePersonDSR(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString())
                , RbReportType.SelectedIndex, int.Parse(ddDistributorType.SelectedValue));

                CORNBusinessLayer.Reports.CrpSalePersonDSR3 CrpReport = new CORNBusinessLayer.Reports.CrpSalePersonDSR3();
                CrpReport.SetDataSource(ds);
                CrpReport.Refresh();

                CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
                CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
                CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
                CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
                CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

                this.Session.Add("CrpReport", CrpReport);
                this.Session.Add("ReportType", p_ReprotType);
                string url = "'Default.aspx'";
                string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
                Type cstype = this.GetType();
                ClientScriptManager cs = Page.ClientScript;
                cs.RegisterStartupScript(cstype, "OpenWindow", script);
            }
        }
        catch (Exception)
        {

            throw;
        }
                
    }
    protected void ddDistributorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadDistributor();
    }
    /// <summary>
    /// Gets Sale Person DSR And Shows Either in Excel or PDF
    /// </summary>
    /// <param name="p_ReportType">Type</param>
    //private void ShowReport(int p_ReprotType)
    //{
    //    DocumentPrintController DPrint = new DocumentPrintController();
    //    RptSaleController RptSaleCtl = new RptSaleController();
    //    DataTable dt = DPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
    //    DataSet ds = null;

    //    if (RbReportType.SelectedIndex == 0)
    //    {
    //        if (DrpSaleForceType.SelectedIndex == 0)
    //        {
    //            DataControl dc = new DataControl();
    //            ds = RptSaleCtl.SelectOrderBookerDSRProDuctWise(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
    //                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()));

    //            CORNBusinessLayer.Reports.CrpSaleReport_ProductWise CrpReport = new CORNBusinessLayer.Reports.CrpSaleReport_ProductWise();
    //            CrpReport.SetDataSource(ds);
    //            CrpReport.Refresh();

    //            CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
    //            CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
    //            CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
    //            CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
    //            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

    //            this.Session.Add("CrpReport", CrpReport);
    //            this.Session.Add("ReportType", p_ReprotType);
    //            string url = "'Default.aspx'";
    //            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
    //            Type cstype = this.GetType();
    //            ClientScriptManager cs = Page.ClientScript;
    //            cs.RegisterStartupScript(cstype, "OpenWindow", script);
    //        }
    //        else
    //        {
    //            DataControl dc = new DataControl();
    //            ds = RptSaleCtl.SelectSalePersonDSRProDuctWise(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
    //                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()));

    //            CORNBusinessLayer.Reports.CrpSaleReport_ProductWise CrpReport = new CORNBusinessLayer.Reports.CrpSaleReport_ProductWise();
    //            CrpReport.SetDataSource(ds);
    //            CrpReport.Refresh();

    //            CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
    //            CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
    //            CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
    //            CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
    //            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

    //            this.Session.Add("CrpReport", CrpReport);
    //            this.Session.Add("ReportType", p_ReprotType);
    //            string url = "'Default.aspx'";
    //            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
    //            Type cstype = this.GetType();
    //            ClientScriptManager cs = Page.ClientScript;
    //            cs.RegisterStartupScript(cstype, "OpenWindow", script);
    //        }
    //    }
    //    else
    //    {
    //        if (DrpSaleForceType.SelectedIndex == 0)
    //        {
    //            DataControl dc = new DataControl();
    //            ds = RptSaleCtl.SelectOrderBookerDSR(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
    //                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()));

    //            CORNBusinessLayer.Reports.CrpSalePersonDSR CrpReport = new CORNBusinessLayer.Reports.CrpSalePersonDSR();
    //            CrpReport.SetDataSource(ds);
    //            CrpReport.Refresh();

    //            CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
    //            CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
    //            CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
    //            CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
    //            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

    //            this.Session.Add("CrpReport", CrpReport);
    //            this.Session.Add("ReportType", p_ReprotType);
    //            string url = "'Default.aspx'";
    //            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
    //            Type cstype = this.GetType();
    //            ClientScriptManager cs = Page.ClientScript;
    //            cs.RegisterStartupScript(cstype, "OpenWindow", script);
    //        }
    //        else
    //        {
    //            DataControl dc = new DataControl();
    //            ds = RptSaleCtl.SelectSalePersonDSR(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
    //                DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(this.Session["UserId"].ToString()));

    //            CORNBusinessLayer.Reports.CrpSalePersonDSR CrpReport = new CORNBusinessLayer.Reports.CrpSalePersonDSR();
    //            CrpReport.SetDataSource(ds);
    //            CrpReport.Refresh();

    //            CrpReport.SetParameterValue("Distributor_Name", drpDistributor.SelectedItem.Text);
    //            CrpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
    //            CrpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
    //            CrpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
    //            CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

    //            this.Session.Add("CrpReport", CrpReport);
    //            this.Session.Add("ReportType", p_ReprotType);
    //            string url = "'Default.aspx'";
    //            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
    //            Type cstype = this.GetType();
    //            ClientScriptManager cs = Page.ClientScript;
    //            cs.RegisterStartupScript(cstype, "OpenWindow", script);
    //        }
    //    }
    //}
}
