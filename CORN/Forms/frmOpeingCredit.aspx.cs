using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;

/// <summary>
/// Form To Add Opening Credit
/// </summary>
public partial class Forms_frmOpeingCredit : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function Populates All Combos, ListBox And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtFromdate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtFromdate.Attributes.Add("readonly", "readonly");

            LoadDistributor();
           
            LoadData();
            LoadOpeningCredit();

        }
    }
   
    private void LoadData()
    {
        ddlCustomer.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            DataTable dtCustomer = CustomerDataController.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue);
            clsWebFormUtil.FillDropDownList(ddlCustomer, dtCustomer, "CUSTOMER_ID", "CUSTOMER_DETAIL");
        }
    }
    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }


    /// <summary>
    /// Resets Form Controls
    /// </summary>
    private void ClearAll()
    {
        txtInvoiceNo.Text = "";

        txtAmount.Text = "";
        txtRemarks.Text = "";
        btnSave.Text = "Save";
        ddlCustomer.Focus();
        drpDistributor.Enabled = true;
    }

    /// <summary>
    /// Loads Opening Credits Detail To Grid
    /// </summary>
    private void LoadOpeningCredit()
    {
        if (drpDistributor.Items.Count > 0)
        {
            CustomerDataController cdc = new CustomerDataController();
            DataTable dt = cdc.SelectOpeningCredit(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.LongNullValue, DateTime.Parse(txtFromdate.Text), int.Parse(DrpCreditType.SelectedValue));

            GrdOrder.DataSource = dt;
            GrdOrder.DataBind();
        }
    }


    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {

        LoadData();
        LoadOpeningCredit();

        drpDistributor.Enabled = true;
        btnSave.Text = "Save";
    }

    protected void DrpBusinessType_SelectedIndexChanged(object sender, EventArgs e)
    {

        LoadData();

    }

    /// <summary>
    /// Checks Invoice No in System
    /// </summary>
    /// <param name="p_Type">Type</param>
    /// <returns>True</returns>
    private bool IsBillBookNoExist(int p_Type)
    {
        bool flag = false;
        if (txtInvoiceNo.Text.Trim().Length > 0)
        {
            OrderEntryController OEC = new OrderEntryController();
            DataTable dtBillBookNo = OEC.SelectBillBookNo(Convert.ToInt32(drpDistributor.SelectedValue), txtInvoiceNo.Text, p_Type);
            if (dtBillBookNo.Rows.Count > 0)
            {
                flag = true;
            }
        }

        return flag;
    }

    /// <summary>
    /// Sets Opening Credit Data For Edit. This Function Runs When An Existing Opening Credit Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DrpCreditType.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[5].Text;

        txtFromdate.Text = Convert.ToDateTime(GrdOrder.Rows[e.NewEditIndex].Cells[6].Text).ToString("dd-MMM-yyyy");


        txtInvoiceNo.Text = GrdOrder.Rows[e.NewEditIndex].Cells[3].Text;
        txtAmount.Text = GrdOrder.Rows[e.NewEditIndex].Cells[4].Text;

        txtRemarks.Text = GrdOrder.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
        hfLegendID.Value = GrdOrder.Rows[e.NewEditIndex].Cells[5].Text;

        hfCustomerID.Value = GrdOrder.Rows[e.NewEditIndex].Cells[0].Text;
        hfSaleInvoiceID.Value = GrdOrder.Rows[e.NewEditIndex].Cells[7].Text;
        
        LoadData();
        ddlCustomer.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[0].Text;

        btnSave.Text = "Update";
        drpDistributor.Enabled = false;
    }

    /// <summary>
    /// Deletes Opening Credit Record
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        hfLegendID.Value = GrdOrder.Rows[e.RowIndex].Cells[5].Text;
        hfCustomerID.Value = GrdOrder.Rows[e.RowIndex].Cells[0].Text;
        hfSaleInvoiceID.Value = GrdOrder.Rows[e.RowIndex].Cells[7].Text;

        LedgerController LedgerCtl = new LedgerController();
        if (LedgerCtl.DeleteOpeningCredit(Convert.ToInt32(drpDistributor.SelectedValue), 0, Convert.ToInt32(hfLegendID.Value), Convert.ToDateTime(txtFromdate.Text), Convert.ToInt32(hfCustomerID.Value), Convert.ToInt64(hfSaleInvoiceID.Value), Convert.ToInt32(Session["UserId"])))
        {
            ClearAll();
            LoadOpeningCredit();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured. Opening Credit not deleted.');", true);
        }

    }

    /// <summary>
    /// Saves/Updates Opening Credit Record
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        LedgerController LedgerCtl = new LedgerController();
        try
        {
            if (btnSave.Text == "Update")
            {
                LedgerCtl.DeleteOpeningCredit(Convert.ToInt32(drpDistributor.SelectedValue), 0, Convert.ToInt32(hfLegendID.Value), Convert.ToDateTime(txtFromdate.Text), Convert.ToInt32(hfCustomerID.Value), Convert.ToInt64(hfSaleInvoiceID.Value), Convert.ToInt32(Session["UserId"]));
            }

            if (!IsBillBookNoExist(1))
            {

                LedgerCtl.OpeningCredit(int.Parse(drpDistributor.SelectedValue.ToString()), txtInvoiceNo.Text.Trim().ToUpper(), DateTime.Parse(txtFromdate.Text), long.Parse(ddlCustomer.SelectedValue),
                -1, 0, decimal.Parse(txtAmount.Text), int.Parse(Session["UserId"].ToString()), int.Parse(DrpCreditType.SelectedValue.ToString()), txtRemarks.Text.Trim());
                ClearAll();
                LoadOpeningCredit();

                ScriptManager.GetCurrent(Page).SetFocus(ddlCustomer);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('This Invoice No already exist,Kindly enter different Invoice No');", true);
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message + "');", true);
        }
    }
    /// <summary>
    /// Cancels Opening Credit Entry
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
}
