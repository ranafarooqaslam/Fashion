using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form To Add, Edit Cheques
/// </summary>
public partial class Forms_frmChequeEntry : System.Web.UI.Page
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
            DrpStatus.Items.Add(new ListItem("Cheque Received", "527"));
            DrpStatus.Items.Add(new ListItem("Cheque Deposit", "528"));
            DrpStatus.Items.Add(new ListItem("Cheque Realize", "529"));
            DrpStatus.Items.Add(new ListItem("Cheque Bounce", "530"));
            DrpStatus.Items.Add(new ListItem("Cheque Cancel", "560"));
            this.LoadAccountHead();
            
            this.LoadDistributor();
            //this.LoadArea();
            this.LoadData();
            this.SelectCreditInvoice();
            this.LoadReceviedCheque();
          //  this.LoadDeliveryman();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");

        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }

    /// <summary>
    /// Loads Routes To Route Combo
    /// </summary>
    //private void LoadArea()
    //{

    //    DistributorAreaController mController = new DistributorAreaController();
    //    DataTable dt = mController.SelectDist_Area(Constants.LongNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, null, null);
    //    clsWebFormUtil.FillDropDownList(DrpRoute, dt, 0, 6, true);

    //}

    /// <summary>
    /// Loads Customers To Customer Combo
    /// </summary>
    private void LoadData()
    {
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
      
        CustomerDataController mController = new CustomerDataController();

        if (DrpChequeType.SelectedIndex == 1)
        {
            DataTable dtCustomer = CustomerDataController.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), 0, Constants.IntNullValue, Constants.IntNullValue);
            clsWebFormUtil.FillDropDownList(this.DrpCustomer, dtCustomer, 0, 4, true);
           // DrpRoute.Enabled = true;
        }
        else
        {
            if (drpDistributor.Items.Count > 0)
            {
                LedgerController LedgerCtl = new LedgerController();
                DataTable dtCredit = LedgerCtl.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedValue.ToString()), 0, Constants.LongNullValue, Constants.IntNullValue);
                clsWebFormUtil.FillDropDownList(this.DrpCustomer, dtCredit, 0, 1, true);

              //  DrpRoute.Enabled = false;
            }
        }
    }

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    private void ClearAll()
    {
        txtChequeNo.Text = "";
        txtAmount.Text = "";
        txtBankName.Text = "";
        txtStartDate.Text = "";
        btnSave.Text = "Save";
        txtReceivedDate.Text = "";
        txtSlipNo.Text = "";
        txtRemarks.Text = "";

    }

    
    /// <summary>
    /// Loads Cheques To Grid
    /// </summary>
    private void LoadReceviedCheque()
    {
        if (DrpStatus.SelectedValue.ToString() != Constants.Cheque_Clear.ToString())
        {
            ChequeEntryController CController = new ChequeEntryController();
            DataTable dt = CController.SelectChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), 0, DrpChequeType.SelectedIndex);
            this.Session.Add("dt", dt);
            GrdOrder.DataSource = dt;
            GrdOrder.DataBind();
        }
        else
        {
            GrdOrder.DataSource = null;
            GrdOrder.DataBind();
        }
    }

    /// <summary>
    /// Loads Crdit Invoices To Grid
    /// </summary>
    private void SelectCreditInvoice()  
    {
        LedgerController CDC = new LedgerController();
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();

        if (DrpCustomer.Items.Count > 0 && DrpChequeType.SelectedIndex != 1)
        {
            DataTable dtCredit = CDC.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedValue.ToString()), 0, long.Parse(DrpCustomer.SelectedValue.ToString()), 0);
            GrdCredit.DataSource = dtCredit;
            GrdCredit.DataBind();
        }
    }
    
    /// <summary>
    /// Saves Cheque Realization
    /// </summary>
    private void ChequeRealization()
    {
        LedgerController LController = new LedgerController();
        string MaxDocumentId = LController.SelectLedgerMaxDocumentId(Constants.Bank_Voucher, int.Parse(drpDistributor.SelectedValue.ToString()), 0);
        decimal OfferAmount = decimal.Parse(txtAmount.Text);
        decimal realizeAmount = 0;//For Advance

        if (DrpChequeType.SelectedIndex == 0)
        {
            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked == true)
                {
                    if (decimal.Parse(dr.Cells[3].Text) >= OfferAmount)
                    {
                        realizeAmount += OfferAmount;

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), 107, int.Parse(drpDistributor.SelectedValue.ToString()), 0, OfferAmount,
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "");

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount, 0,
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "");

                        OfferAmount = decimal.Parse(dr.Cells[3].Text) - OfferAmount;
                        LController.UpdateSaleInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount);
                        break;
                    }
                    else if (decimal.Parse(dr.Cells[3].Text) <= OfferAmount)
                    {

                        realizeAmount += decimal.Parse(dr.Cells[3].Text);

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), 107, int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(dr.Cells[3].Text),
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "");

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(dr.Cells[3].Text), 0,
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "");

                        OfferAmount = OfferAmount - decimal.Parse(dr.Cells[3].Text);
                        LController.UpdateSaleInvoice(long.Parse(dr.Cells[1].Text), int.Parse(drpDistributor.SelectedValue.ToString()), 0);
                    }
                }
            }
            if (realizeAmount < decimal.Parse(txtAmount.Text))
            {
                LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), 107, int.Parse(drpDistributor.SelectedValue.ToString()), 0, OfferAmount,
                         DateTime.Parse(Session["CurrentWorkDate"].ToString()), "Cheque Realization (Advance)", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                         txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "");

                LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount, 0,
                DateTime.Parse(Session["CurrentWorkDate"].ToString()), "Cheque Realization (Advance)", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, "");

            }
        }
        else
        {
            LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), 107, int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(txtAmount.Text),
                DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Advance", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Constants.LongNullValue, null, Constants.IntNullValue, txtSlipNo.Text, Constants.DateNullValue, 20, "");

            LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(txtAmount.Text), 0,
                DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Advance", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), 0,
                txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Constants.LongNullValue, null, Constants.IntNullValue, txtSlipNo.Text, Constants.DateNullValue, 20, "");
        }

    }
    
    /// <summary>
    /// Loads Deliverymen To Deliverman Comb
    /// </summary>
    //private void LoadDeliveryman()
    //{
    //    if (drpDistributor.Items.Count > 0)
    //    {
    //        SaleForceController mDController = new SaleForceController();
    //        DataTable m_dt = mDController.SelectSaleForceAssignedArea(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()));
    //        clsWebFormUtil.FillDropDownList(this.DrpDeliveryMan, m_dt, 0, 3, true);
    //    }

    //}
    
    /// <summary>
    /// Loads Account Heads To Account Combo
    /// </summary>
    private void LoadAccountHead()
    {
        CORNCommon.Classes.Configuration.GetAccountHead();
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dt = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, long.Parse(CORNCommon.Classes.Configuration.BankDefaultType));
        clsWebFormUtil.FillDropDownList(DrpBankAccount, dt, 0, 4, true);
    }

    /// <summary>
    /// Save Or Updates a Cheque
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ChequeEntryController CController = new ChequeEntryController();

        DateTime ChequeDate;

        if (txtStartDate.Text.Length == 10)
        {
            ChequeDate = DateTime.Parse(ConvertDate.British_To_American(txtStartDate.Text));

        }
        else
        {
            ChequeDate = DateTime.Now;
        }

        int InvoiceCount = Constants.IntNullValue;
        foreach (GridViewRow dr in GrdCredit.Rows)
        {
            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
            if (chRelized.Checked == true)
            {
                InvoiceCount++;
                break;
            }
        }

        if (InvoiceCount == Constants.IntNullValue)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Must Select Invoice');", true);
            return;
        }

        if (btnSave.Text == "Save")
        {
            if (DrpStatus.SelectedIndex == 0)
            {                
                HFChqueProcessId.Value = CController.InsertChequeEntry(int.Parse(drpDistributor.SelectedValue.ToString()),0, long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate,
                    DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue, decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Now, DrpChequeType.SelectedIndex, txtSlipNo.Text, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()),0);


                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    if (chRelized.Checked == true)
                    {
                        CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                    }
                }
            }
        }
        else
        {
            if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Pending && txtReceivedDate.Text == DateTime.Parse(this.Session["CurrentWorkDate"].ToString()).ToString("dd/MM/yyyy"))
            {
                CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), int.Parse(drpDistributor.SelectedValue.ToString()), 0, long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue,
                   decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));

                CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    if (chRelized.Checked == true)
                    {
                        CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                    }
                }
            }
            else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Deposit)
            {
                CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue,0, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue,
                     Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));
            }
            else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Bons || int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Cancel)
            {
                CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
                                                 Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

            }

            else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Clear)
            {

                CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, 0, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
                                                 Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

                CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    if (chRelized.Checked == true)
                    {
                        CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                    }
                }
                this.ChequeRealization();
                this.LoadData();
                this.SelectCreditInvoice();
            }
        }
        this.ClearAll();
        this.LoadReceviedCheque();
    }

    /// <summary>
    /// Cancels Cheque Entry
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.ClearAll();
    }

    /// <summary>
    /// Deletes Cheque
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ChequeEntryController CController = new ChequeEntryController();
        CController.DeleteChequeEntry(long.Parse(GrdOrder.Rows[e.RowIndex].Cells[0].Text));
        this.LoadReceviedCheque();
    }

    /// <summary>
    /// Loads Credit Invoices
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SelectCreditInvoice();
    }

    /// <summary>
    /// Loads Routes, Customers, Credit Invoices And Cheques
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
       // this.LoadArea();
        this.LoadData();
        this.SelectCreditInvoice();
        this.LoadReceviedCheque();
    }

   

    /// <summary>
    /// Sets Cheque Data For Edit. This Function Runs When An Existing Cheque Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ChequeEntryController Ccontroller = new ChequeEntryController();
            HFChqueProcessId.Value = GrdOrder.Rows[e.NewEditIndex].Cells[0].Text;
          //  DrpRoute.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[11].Text;

            this.LoadData();

            DrpCustomer.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[1].Text;

            txtChequeNo.Text = GrdOrder.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;","");
            txtBankName.Text = GrdOrder.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
            txtStartDate.Text = GrdOrder.Rows[e.NewEditIndex].Cells[5].Text;
            txtReceivedDate.Text = GrdOrder.Rows[e.NewEditIndex].Cells[6].Text;
            txtAmount.Text = GrdOrder.Rows[e.NewEditIndex].Cells[8].Text;
            txtSlipNo.Text = GrdOrder.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
            txtRemarks.Text = GrdOrder.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");
            DrpBankAccount.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[13].Text;
            try
            {
             //   DrpDeliveryMan.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[14].Text;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Sale Force not found.');", true);
            }
            
            btnSave.Text = "Update";
            this.SelectCreditInvoice();

            DataTable dt = Ccontroller.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 0);

            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                foreach (DataRow dbr in dt.Rows)
                {

                    if (Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]) == Convert.ToInt64(dbr["SALE_INVOICE_ID"]))
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        chRelized.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Invoice not found for selected cheque');", true);
        }
    }

    /// <summary>
    /// Loads Cheques
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save")
        {            
            this.LoadReceviedCheque();
        }
    }

    /// <summary>
    /// Loads Customers And Cheques
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpChequeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadData();
        this.LoadReceviedCheque();
    }

    /// <summary>
    /// Loads Customers And Credit Invoices
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadData();
        this.SelectCreditInvoice();
    }

    /// <summary>
    /// Filters Cheuqe Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)this.Session["dt"];
        switch (ddSearchType.SelectedIndex)
        {
            case 1:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 2:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 3:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 4:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            default:
                dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'";
                break;
        }
        GrdOrder.DataSource = dt.DefaultView;
        GrdOrder.DataBind();
    }
}
