using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form To Add, Edit Branch Expense And Salary
/// </summary>
public partial class frmExpenseEntry : System.Web.UI.Page
{
    LedgerController mLController = new LedgerController();
    /// <summary>
    /// Page_Load Function Populates All Combos And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadPrincipal();
            LoadAccountHead();
            CreatTable();
            LoadVoucherNo();
            LoadClosingBal();
            btnAddNew.Attributes.Add("onclick", "return ValidateForm();");
            ScriptManager.GetCurrent(Page).SetFocus(drpDistributor);
            lblRowId.Text = "-1";
        }
    }

    private void LoadClosingBal()
    {
        DataTable dt = mLController.GetCashClosing(int.Parse(drpDistributor.SelectedValue.ToString()), Convert.ToDateTime("2000-12-12" + " 00:00:00"), Convert.ToDateTime(Session["Currentworkdate"]), int.Parse(Session["UserId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtClosinBal.Text = Convert.ToString(Convert.ToDecimal(dt.Rows[0]["CashInHand"]));
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
    /// Loads Account Heads To AccountHead Combo
    /// </summary>
    private void LoadAccountHead()
    {
        
            AccountHeadController mAccountController = new AccountHeadController();
            DataTable dt = mAccountController.GetAssignAccountHead(Constants.IntNullValue, Constants.IntNullValue);
            clsWebFormUtil.FillDropDownList(DrpAccountHead, dt, 0, 4, true);
        
    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    private void LoadPrincipal()
    {

        //DataTable table = new DataTable();
        //table.Columns.Add("Principal_ID", typeof(int));
        //table.Columns.Add("Principal", typeof(string));
        //table.Rows.Add(0, "General Entry");
        //DrpPrincipal.Items.Clear();
        //clsWebFormUtil.FillDropDownList(DrpPrincipal, table, 0, 1);
        DrpPrincipal.Enabled = true;
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Clear();
        DrpPrincipal.Items.Add(new clsListItems("General Entry", "0"));
        clsWebFormUtil.FillDropDownList(DrpPrincipal, m_dt, 0, 1);
        LoadAccountHead();
    }
    
    /// <summary>
    /// Creates Datatable For Expense And Salary
    /// </summary>
    private void CreatTable()
    {
        DataTable  dtVoucher = new DataTable();
        dtVoucher.Columns.Add("Account_Head_Id", typeof(long));
        dtVoucher.Columns.Add("Account_Code", typeof(string));
        dtVoucher.Columns.Add("Account_Name", typeof(string));
        dtVoucher.Columns.Add("Debit", typeof(decimal));
        dtVoucher.Columns.Add("Credit", typeof(decimal));
        dtVoucher.Columns.Add("Remarks", typeof(string));
        dtVoucher.Columns.Add("Principal_Id", typeof(string));
        dtVoucher.Columns.Add("Principal", typeof(string));
        dtVoucher.Columns.Add("ACCOUNT_PARENT_ID", typeof(string));
        Session.Add("dtVoucher", dtVoucher);

    }

    /// <summary>
    /// Loads Voucher Nos To Voucher No Combo
    /// </summary>
    private void LoadVoucherNo()
    {
        LedgerController LController = new LedgerController();
        DataTable  dt  = LController.SelectVoucherNo(((DateTime)Session["CurrentWorkDate"]), int.Parse(drpDistributor.SelectedValue.ToString()), Constants.Journal_Voucher, int.Parse(Session["UserId"].ToString()),null);
        DrpVoucherNo.Items.Clear();
        DrpVoucherNo.Items.Add(new ListItem("New", "0"));
        clsWebFormUtil.FillDropDownList(DrpVoucherNo, dt, 3, 3);   
    }

    /// <summary>
    /// Deletes Expense And Salary
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable  dtVoucher = (DataTable)Session["dtVoucher"];
        dtVoucher.Rows.RemoveAt(e.RowIndex);
        GrdOrder.DataSource = dtVoucher;
        GrdOrder.DataBind();
        lblRowId.Text = "-1";

        decimal TotalDebit = 0;

        foreach (DataRow dr in dtVoucher.Rows)
        {
            TotalDebit += decimal.Parse(dr["DEBIT"].ToString());
        }
        txtMainCash.Text = TotalDebit.ToString();
  
    }

    /// <summary>
    /// Sets Expense And Salary Data For Edit. This Function Runs When An Existing Expense And Salary Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblRowId.Text  = e.NewEditIndex.ToString();
        LoadAccountHead(); 
        DrpAccountHead.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[0].Text;
        txtRemarks.Text = GrdOrder.Rows[e.NewEditIndex].Cells[2].Text;
        txtAmount.Text = GrdOrder.Rows[e.NewEditIndex].Cells[3].Text;
    }

    /// <summary>
    /// Adds Record To Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        DataControl dc = new DataControl();
        DataTable  dtVoucher = (DataTable)Session["dtVoucher"];

        if (decimal.Parse(dc.chkNull_0(txtAmount.Text)) > decimal.Parse(dc.chkNull_0(txtClosinBal.Text)))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Entered Amount cannot be greater than Cash In Hand')", true);
            return;
        }

        if (lblRowId.Text == "-1")
        {
            DataRow dr = dtVoucher.NewRow();
            dr["ACCOUNT_HEAD_ID"] = DrpAccountHead.SelectedValue.ToString();
            dr["ACCOUNT_NAME"] = DrpAccountHead.SelectedItem.Text;
            dr["DEBIT"] = decimal.Parse(dc.chkNull_0(txtAmount.Text));
            dr["Credit"] = "0";
            dr["REMARKS"] = txtRemarks.Text;
            dr["Principal"] = DrpPrincipal.SelectedItem.Text;
            dr["Principal_id"] = DrpPrincipal.SelectedItem.Value;
            dr["ACCOUNT_PARENT_ID"] = 61;//Branch Expenses  (FDM EXP)
            dtVoucher.Rows.Add(dr);

        }
        else
        {
            DataRow dr = dtVoucher.Rows[int.Parse(lblRowId.Text)];
            dr["ACCOUNT_HEAD_ID"] = DrpAccountHead.SelectedValue.ToString();
            dr["ACCOUNT_NAME"] = DrpAccountHead.SelectedItem.Text;
            dr["DEBIT"] = decimal.Parse(dc.chkNull_0(txtAmount.Text));
            dr["Credit"] = "0";
            dr["REMARKS"] = txtRemarks.Text;
            dr["Principal"] = DrpPrincipal.SelectedItem.Text;
            dr["Principal_id"] = DrpPrincipal.SelectedItem.Value;
            dr["ACCOUNT_PARENT_ID"] = 61; //Branch Expenses  (FDM EXP)
        }
        
        GrdOrder.DataSource = dtVoucher;
        GrdOrder.DataBind();
        txtAmount.Text = "";
        txtRemarks.Text = "";
        lblRowId.Text = "-1";
        ScriptManager.GetCurrent(Page).SetFocus(DrpAccountHead);
        
        decimal TotalDebit = 0;

        foreach (DataRow dr in dtVoucher.Rows)
        {
            TotalDebit += decimal.Parse(dr["DEBIT"].ToString());
        }
        txtMainCash.Text = TotalDebit.ToString();
    }

    /// <summary>
    /// Save Or Updates Expenses And Salary
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        CORNCommon.Classes.Configuration.GetAccountHead();
        DataControl dc = new DataControl();
        DataTable dtVoucher = (DataTable)Session["dtVoucher"];
        bool IsSaveVoucher;

        string MaxDocumentId;

        if (dtVoucher.Rows.Count > 0)
        {
            DataRow dr = dtVoucher.NewRow();
            dr["ACCOUNT_HEAD_ID"] = CORNCommon.Classes.Configuration.PeatyCash;
            dr["ACCOUNT_NAME"] = "Peatty Cash";
            dr["DEBIT"] = 0;
            dr["Credit"] = decimal.Parse(dc.chkNull_0(txtMainCash.Text));
            dr["REMARKS"] = "Patty Cash Credit";
            dr["Principal"] = DrpPrincipal.SelectedItem.Text;
            dr["Principal_id"] = DrpPrincipal.SelectedItem.Value;

            dtVoucher.Rows.Add(dr);

            if (DrpVoucherNo.SelectedValue.ToString() == "0")
            {
                MaxDocumentId = mLController.SelectMaxVoucherId(Constants.Journal_Voucher, int.Parse(drpDistributor.SelectedValue.ToString()), ((DateTime)Session["CurrentWorkDate"]));

                IsSaveVoucher = mLController.Add_Voucher(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), MaxDocumentId, Constants.Journal_Voucher,
                       ((DateTime)Session["CurrentWorkDate"]), Constants.Document_Petty_Cash, null, "Petty Voucher", Constants.DateNullValue, null, dtVoucher, int.Parse(Session["UserId"].ToString()), txtslipNo.Text);
            }
            else
            {
                MaxDocumentId = DrpVoucherNo.SelectedValue.ToString();

                IsSaveVoucher = mLController.Add_Voucher(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), MaxDocumentId, Constants.Journal_Voucher,
                       ((DateTime)Session["CurrentWorkDate"]), Constants.Document_Petty_Cash, null, "Petty Voucher", Constants.DateNullValue, null, dtVoucher, int.Parse(Session["UserId"].ToString()), txtslipNo.Text);
            }

            if (IsSaveVoucher == true)
            {
                dtVoucher.Rows.Clear();
                GrdOrder.DataSource = null;
                GrdOrder.DataBind();
                txtAmount.Text = "";
                txtslipNo.Text = "";
                txtRemarks.Text = "";
                lblRowId.Text = "-1";
                ScriptManager.GetCurrent(Page).SetFocus(DrpAccountHead);

                decimal TotalDebit = 0;

                foreach (DataRow dr1 in dtVoucher.Rows)
                {
                    TotalDebit += decimal.Parse(dr1["DEBIT"].ToString());
                }
                txtMainCash.Text = TotalDebit.ToString();
                LoadVoucherNo();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some Error in Voucher')", true);
                return;
            }
        }
        else if (lblRowId.Text == "-1")
        {
            MaxDocumentId = DrpVoucherNo.SelectedValue.ToString();

            IsSaveVoucher = mLController.Delete_Voucher(int.Parse(drpDistributor.SelectedValue.ToString()),MaxDocumentId, Constants.Journal_Voucher);
            Session.Remove("dtVoucher");
            LoadVoucherNo();
        }
    }

    /// <summary>
    /// Loads Expense And Salary Data To Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpVoucherNo_SelectedIndexChanged(object sender, EventArgs e)
    {       
        LedgerController LController = new LedgerController();
        DataTable dtVoucher = (DataTable)Session["dtVoucher"];

        if (DrpVoucherNo.SelectedValue.ToString() != "0")
        {
            dtVoucher = LController.SelectVoucherDetail(int.Parse(drpDistributor.SelectedValue.ToString()), DrpVoucherNo.SelectedValue.ToString(), Constants.Journal_Voucher);
            for (int i = 0; i < dtVoucher.Rows.Count; i++)
            {
                if (Decimal.Parse(dtVoucher.Rows[i]["CREDIT"].ToString()) > 0)
                {
                    dtVoucher.Rows.RemoveAt(i);
                }
            }
            Session.Add("dtVoucher", dtVoucher);
            GrdOrder.DataSource = dtVoucher;
            GrdOrder.DataBind();
        }
        else
        {
            dtVoucher.Rows.Clear();  
            GrdOrder.DataSource = null;
            GrdOrder.DataBind();

        }
        lblRowId.Text = "0";
        Session.Add("dtVoucher", dtVoucher);
       
    }

    /// <summary>
    /// Loads Account Heads
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAccountHead();
     
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        LoadClosingBal();
    }
    /// <summary>
    /// Loads Principals And Account Heads
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    //protected void rblDetailAccountType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (rblDetailAccountType.SelectedValue == "55")
    //    {
    //        DrpPrincipal.Enabled = false;
    //        LoadPrincipal();
    //        LoadAccountHead();
    //    }
    //    else
    //    {
    //        DrpPrincipal.Enabled = true;
    //        SKUPriceDetailController PController = new SKUPriceDetailController();
    //        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
    //        DrpPrincipal.Items.Clear();            
    //        clsWebFormUtil.FillDropDownList(DrpPrincipal, m_dt, 0, 1);
    //        LoadAccountHead();
    //    }
    //}
}
