using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;


public partial class Forms_frmChequeEntryVendor : System.Web.UI.Page
{

    readonly LedgerController LController = new LedgerController();
    readonly VenderEntryController VController = new VenderEntryController();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DrpStatus.Items.Add(new ListItem("Cheque Paid", "527"));
            DrpStatus.Items.Add(new ListItem("Cheque Clear", "529"));
            DrpStatus.Items.Add(new ListItem("Cheque Bounce", "530"));
            DrpStatus.Items.Add(new ListItem("Cheque Cancel", "560"));

            LoadAccountHead();
            LoadDistributor();
            
            LoadVendor();
            
            SelectCreditInvoice();
            LoadReceviedCheque();

            btnSave.Attributes.Add("onclick", "return ValidateForm();");
          //  LoadPaymentRecieved();
            txtStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    
    private void toggleControls(string pAccountType)
    {
        if (pAccountType == "21")
        {
            lblChequeNo.Visible = false;
            txtChequeNo.Visible = false;
            lblChequeDate.Visible = false;
            txtStartDate.Visible = false;
            ibtnStartDate.Visible = false;
            DrpStatus.Visible = false;
            lblStatus.Visible = false;
            GrdCO.Visible = true;
            GrdCheque.Visible = false;
           
        }
        else if (pAccountType == "33")
        {
            lblChequeNo.Visible = false;
            txtChequeNo.Visible = false;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            DrpStatus.Visible = false;
            lblStatus.Visible = false;
            lblChequeDate.Text = "Transfer Date";
            GrdCO.Visible = true;
            GrdCheque.Visible = false;
            
        }
        else
        {

            lblChequeNo.Visible = true;
            txtChequeNo.Visible = true;
            lblChequeDate.Visible = true;
            txtStartDate.Visible = true;
            ibtnStartDate.Visible = true;
            DrpStatus.Visible = true;
            lblStatus.Visible = true;
            lblChequeDate.Text = "Cheque Date";
            GrdCO.Visible = false;
            GrdCheque.Visible = true;
            
        }
    }
  
    #region Load

    private void LoadPaymentRecieved()
    {
        //if (drpDistributor.Items.Count > 0)
        //{
        //    DataSet dsReceived = LController.SelectBankCashTransction(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, 21,
        //        DateTime.Parse(Session["CurrentWorkDate"].ToString()));

        //    DataTable dtRealized = dsReceived.Tables[2];

        //    if (dtRealized.Rows.Count > 0)
        //    {
        //        lblAmount.Text = string.Format("{0:0,0.00}", Convert.ToDecimal(dtRealized.Rows[0][0].ToString()));

        //    }

        //}
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }

    private void LoadVendor()
    {
        drpVendor.Items.Clear();

        VenderEntryController VendorCtl = new VenderEntryController();

        DataTable dtVendor = VendorCtl.GetVendor(Constants.IntNullValue);

        if (dtVendor != null)
        {
            clsWebFormUtil.FillDropDownList(drpVendor, dtVendor, 0, 2, true);
        }
    }
   
   
    private void ClearAll()
    {
        txtChequeNo.Text = "";
        txtAmount.Text = "";
        txtBankName.Text = "";
        txtStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        btnSave.Text = "Save";
        txtReceivedDate.Text = "";
        
        txtRemarks.Text = "";

    }

    private void LoadReceviedCheque()
    {
        ChequeEntryController CController = new ChequeEntryController();
       
        decimal chqAmount = 0;
        if (DrpAccountType.SelectedValue == "21" || DrpAccountType.SelectedValue == "33")
        {
            GrdCO.DataSource = null;
            GrdCO.DataBind();
            if (drpDistributor.Items.Count > 0)
            {
                DataTable dt = LController.VendorBankCashTransction(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(DrpAccountType.SelectedValue),
                           DateTime.Parse(Session["CurrentWorkDate"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()));

                Session.Add("dt", dt);

                GrdCO.DataSource = dt;
                GrdCO.DataBind();

                if (dt != null)
                {
                    foreach (DataRow gvr in dt.Rows)
                    {
                        chqAmount += Convert.ToDecimal(gvr["CHEQUE_AMOUNT"]);
                    }
                    lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                }
            }
        }

        else
        {
            GrdCheque.DataSource = null;
            GrdCheque.DataBind();
            if (DrpStatus.SelectedValue.ToString() != Constants.Cheque_Clear.ToString())
            {
                DataTable dt = null;

                if (drpVendor.Items.Count > 0)
                {
                    dt = CController.SelectVendorChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpVendor.SelectedValue), DrpAccountType.SelectedIndex);
                    Session.Add("dt", dt);

                    GrdCheque.DataSource = dt;
                    GrdCheque.DataBind();
                }
                if (dt != null)
                {
                    foreach (DataRow gvr in dt.Rows)
                    {
                        chqAmount += Convert.ToDecimal(gvr["CHEQUE_AMOUNT"]);
                    }
                }
                    lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                
            }
            else
            {
                ///Load Cash Realization Detail
                if (Convert.ToInt32(DrpStatus.SelectedValue) < 530)
                {
                    DataSet ds = CController.SelectVendorChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), DrpAccountType.SelectedIndex);
                    DataTable dt2 = ds.Tables[1];
                    if (dt2.Rows.Count > 0)
                    {
                        //grdRealize.DataSource = dt2;
                      //  grdRealize.DataBind();

                      
                        foreach (DataRow dr in dt2.Rows)
                        {
                            chqAmount += Convert.ToDecimal(dr[2].ToString());
                        }
                        lblTotalAmount.Text = string.Format("{0:0.00}", chqAmount);
                    }
                }
                ////////////////////////////////////////////////
                DataTable dt = CController.SelectVendorChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, DrpAccountType.SelectedIndex);
                Session.Add("dt", dt);

                GrdCheque.DataSource = dt;
                GrdCheque.DataBind();

                foreach (GridViewRow gvr in GrdCheque.Rows)
                {
                    LinkButton row = gvr.FindControl("btnEdit") as LinkButton;
                    row.Enabled = false;
                }

            }
        }
    }

    private void checkDuplication()
    {
        DataTable dt = (DataTable)Session["dt"];

        DataRow[] foundRows = dt.Select("VENDOR_ID = '" + drpVendor.SelectedValue + "' and CHEQUE_NO='" + txtChequeNo.Text + "'");

        if (foundRows.Length > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Cheque No Already exist against this vendor!');", true);
        }
    }

    private void SelectCreditInvoice()
    {
        LedgerController CDC = new LedgerController();

        GrdCredit.DataSource = null;
        GrdCredit.DataBind();

        if (drpVendor.Items.Count > 0)
        {
            DataTable dtCredit = CDC.SelectCreditPendingInvoice2(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(drpVendor.SelectedValue), 0);
            GrdCredit.DataSource = dtCredit;
            GrdCredit.DataBind();
            decimal totalAmount = 0;
            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                chRelized.Checked = true;
                totalAmount += decimal.Parse(dr.Cells[4].Text);
            }
            txtInvoiceToalAmount.Text = totalAmount.ToString();
        }
    }

    private void LoadAccountHead()
    {
        Configuration.GetAccountHead();
        AccountHeadController mAccountController = new AccountHeadController();

        if (DrpAccountType.SelectedValue == "21")
        {
            DataTable dt = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, long.Parse(CORNCommon.Classes.Configuration.CashDefaultType));
            clsWebFormUtil.FillDropDownList(DrpBankAccount, dt, 0, 4, true);
        }
        else
        {
            DataTable dt = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, long.Parse(CORNCommon.Classes.Configuration.BankDefaultType));
            clsWebFormUtil.FillDropDownList(DrpBankAccount, dt, 0, 4, true);
        }
    }

    #endregion

    private void ChequeRealization()
    {
        DateTime ChequeDate=Constants.DateNullValue;
        try
        {
            if (DrpAccountType.SelectedValue == "18" || DrpAccountType.SelectedValue == "33")
            {
                if (txtStartDate.Text.Length == 10)
                {
                    ChequeDate = DateTime.Parse(ConvertDate.British_To_American(txtStartDate.Text));

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
            return;
        }

        LedgerController LController = new LedgerController();
        string MaxDocumentId = "";
        int VoucherType = Constants.Expanse_Voucher;
      
        decimal OfferAmount = decimal.Parse(txtAmount.Text);
        decimal realizeAmount = 0;
       
        string remarks="";

        DateTime DocDate = DateTime.Parse(Session["CurrentWorkDate"].ToString());
        DocDate = DateTime.Parse(DocDate.ToShortDateString() + " 00:00:00");
        
        if (DrpAccountType.SelectedValue == "18")
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedValue.ToString()), 1);
            remarks = "Chq# " + txtChequeNo.Text + ", " + DrpBankAccount.SelectedItem.Text + ", " + txtRemarks.Text;
        }
        else if (DrpAccountType.SelectedValue == "33")
        {
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedValue.ToString()), 1);
            remarks = "Online Transfer " + DrpAccountType.SelectedItem.Text + ", " + txtRemarks.Text;

        }
        else if (DrpAccountType.SelectedValue == "21")
        {

            VoucherType = Constants.Cash_Voucher;
            MaxDocumentId = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedValue.ToString()), 1);

            remarks = "Cash " + txtRemarks.Text;
        }
       // Session.Add("VoucherNo", MaxDocumentId);

            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked == true)
                {
                    string manualNo = "0";

                    if (Convert.ToString(dr.Cells[5].Text) == "opng")
                    {
                        manualNo = "opng";
                    }
                    if (decimal.Parse(dr.Cells[4].Text) >= OfferAmount)
                    {
                        realizeAmount += OfferAmount;

                        LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), int.Parse(Configuration.PayableAccount), int.Parse(drpDistributor.SelectedValue.ToString()), 0, OfferAmount,
                        DocDate, remarks, DateTime.Now,int.Parse(drpVendor.SelectedValue.ToString()), 0, Constants.Document_PrincipalInvoice,
                        txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo,int.Parse(DrpAccountType.SelectedValue), "", "", ChequeDate);

                        LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount, 0,
                         DocDate, remarks, DateTime.Now, int.Parse(drpVendor.SelectedValue.ToString()), 0, Constants.Document_PrincipalInvoice,
                         txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo, int.Parse(DrpAccountType.SelectedValue), "", "", ChequeDate);


                        OfferAmount = decimal.Parse(dr.Cells[4].Text) - OfferAmount;

                        if (manualNo == "opng") 
                        {
                            LController.UpdatePurchaseInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), int.Parse(drpDistributor.SelectedValue), OfferAmount);

                        }
                        else
                        {
                            VController.Update_VenderInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), int.Parse(drpDistributor.SelectedValue), OfferAmount);

                        } 
                        break;
                    }
                    else if (decimal.Parse(dr.Cells[4].Text) <= OfferAmount)
                    {
                        realizeAmount += decimal.Parse(dr.Cells[4].Text);
                        LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), int.Parse(Configuration.PayableAccount), int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(dr.Cells[4].Text),
                        DocDate, remarks, DateTime.Now, int.Parse(drpVendor.SelectedValue.ToString()), 0, Constants.Document_PrincipalInvoice,
                         txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo, int.Parse(DrpAccountType.SelectedValue), "", "", ChequeDate);

                        LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(dr.Cells[4].Text), 0,
                        DocDate, remarks, DateTime.Now, int.Parse(drpVendor.SelectedValue.ToString()),0, Constants.Document_PrincipalInvoice,
                        txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), manualNo, int.Parse(DrpAccountType.SelectedValue), "", "", ChequeDate);

                        OfferAmount = OfferAmount - decimal.Parse(dr.Cells[4].Text);
                        if (manualNo == "opng")
                        {
                            LController.UpdatePurchaseInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), int.Parse(drpDistributor.SelectedValue), OfferAmount);

                        }
                        else
                        {
                            VController.Update_VenderInvoice(long.Parse(dr.Cells[1].Text), int.Parse(drpDistributor.SelectedValue), 0);
                        }
                    }
                }
            }
            if (realizeAmount < decimal.Parse(txtAmount.Text))
            {

                LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), int.Parse(Configuration.PayableAccount), int.Parse(drpDistributor.SelectedValue.ToString()), 0, OfferAmount,
                                      DocDate, remarks + " (Advance)", DateTime.Now, int.Parse(drpVendor.SelectedValue), 0, Constants.Document_PrincipalInvoice,
                                      txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0", int.Parse(DrpAccountType.SelectedValue), "", "", ChequeDate);

                LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount, 0,
                                      DocDate, remarks + " (Advance)", DateTime.Now, int.Parse(drpVendor.SelectedValue),0, Constants.Document_PrincipalInvoice,
                                      txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0", int.Parse(DrpAccountType.SelectedValue), "", "", ChequeDate);

            }
        
       

    }

    protected void GrdCheque_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ChequeEntryController CController = new ChequeEntryController();
        CController.DeleteChequeEntry(long.Parse(GrdCheque.Rows[e.RowIndex].Cells[0].Text));

        SelectCreditInvoice();
        LoadReceviedCheque();
    }

    protected void GrdCheque_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ChequeEntryController Ccontroller = new ChequeEntryController();
            HFChqueProcessId.Value = GrdCheque.Rows[e.NewEditIndex].Cells[0].Text;
           
            

            drpVendor.SelectedValue = GrdCheque.Rows[e.NewEditIndex].Cells[1].Text;
            txtChequeNo.Text = GrdCheque.Rows[e.NewEditIndex].Cells[3].Text;
            txtStartDate.Text = GrdCheque.Rows[e.NewEditIndex].Cells[4].Text;
            txtReceivedDate.Text = GrdCheque.Rows[e.NewEditIndex].Cells[5].Text;
            txtAmount.Text = GrdCheque.Rows[e.NewEditIndex].Cells[6].Text;
            txtRemarks.Text = GrdCheque.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;","");
            DrpBankAccount.SelectedValue = GrdCheque.Rows[e.NewEditIndex].Cells[9].Text;
           
            btnSave.Text = "Update";
            SelectCreditInvoice();
            if (DrpAccountType.SelectedValue == "18")
            {
                DataTable dt = Ccontroller.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 0);

                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    foreach (DataRow dbr in dt.Rows)
                    {

                        if (Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]) == Convert.ToInt64(dbr["SALE_INVOICE_ID"]))
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            chRelized.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Invoice not found for selected cheque');", true);
        }
    }

    protected void GrdCO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataControl dc = new DataControl();

            if (DrpAccountType.SelectedValue == "21" || DrpAccountType.SelectedValue == "33")
            {
                if (LController.DeleteVendorBankCashTransction(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(dc.chkNull_0(GrdCO.Rows[e.RowIndex].Cells[11].Text)),
                 int.Parse(dc.chkNull_0(GrdCO.Rows[e.RowIndex].Cells[10].Text)), long.Parse(dc.chkNull_0(GrdCO.Rows[e.RowIndex].Cells[12].Text))
                 , decimal.Parse(dc.chkNull_0(GrdCO.Rows[e.RowIndex].Cells[5].Text)), int.Parse(dc.chkNull_0(GrdCO.Rows[e.RowIndex].Cells[9].Text))))
                {
                   
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record remove successfully.')", true);
                    SelectCreditInvoice();
                    LoadReceviedCheque();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred.')", true);
                }
            }
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred.')", true);
        }
    }

    #region Sel/Index Change
    
   
    protected void drpVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectCreditInvoice();
        LoadReceviedCheque();
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        
        SelectCreditInvoice();
        LoadReceviedCheque();
    }
    
    protected void DrpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save")
        {
            LoadReceviedCheque();
        }
    }
    protected void DrpAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAccountHead();
        toggleControls(DrpAccountType.SelectedValue);
        
        SelectCreditInvoice();
        LoadReceviedCheque();
    }
  

    #endregion

    #region Click

    private bool InvalidDate()
    {
        //if (DrpAccountType.SelectedValue == "18")
        //{
        //    if (DrpStatus.SelectedValue == "527")
        //    {
        //        DateTime dt = DateTime.ParseExact(Session["CurrentWorkDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);//DateTime.Parse(Session["CurrentWorkDate"].ToString()).ToString("dd/MM/yyyy"));//
        //        if (Convert.ToDateTime(ConvertDate.British_To_American(txtStartDate.Text)) <  dt)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Invalid Cheque Date');", true);
                 
        //            return false;
        //        }
        //    }
            
        //}
        return true;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (InvalidDate())
            {

                ChequeEntryController CController = new ChequeEntryController();


                DateTime ChequeDate;
                try
                {
                    if (txtStartDate.Text.Length == 10)
                    {
                        ChequeDate = DateTime.Parse(ConvertDate.British_To_American(txtStartDate.Text));

                    }
                    else
                    {
                        ChequeDate = DateTime.Now;
                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Correct Cheque Date Pattern is DD/MM/YYYY.');", true);
                    return;
                }

                int InvoiceCount = Constants.IntNullValue;
                if (DrpAccountType.SelectedIndex == 0)//on Cash Realization check Invoice Selection
                {

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
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must Select Invoice');", true);
                        return;
                    }

                }

                if (btnSave.Text == "Save")
                {
                    if (DrpAccountType.SelectedIndex != 0)//on Cash Realization check Invoice Selection
                    {

                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                InvoiceCount++;
                                break;
                            }
                        }
                        if (InvoiceCount == Constants.IntNullValue && DrpAccountType.SelectedIndex != 0)
                        {
                            CashAdvance();

                        }
                        else if (InvoiceCount != Constants.IntNullValue && DrpAccountType.SelectedIndex != 0)
                        {
                            ChequeRealization();// used as All cash, online, cheque
                            
                            SelectCreditInvoice();
                        }
                    }
                    else if (DrpAccountType.SelectedIndex == 0)//on Cash Realization check Invoice Selection
                    {
                        checkDuplication();
                        if (DrpStatus.SelectedIndex == 0)
                        {
                            HFChqueProcessId.Value = CController.InsertChequeEntry(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpVendor.SelectedValue.ToString()), 0, txtChequeNo.Text, txtBankName.Text, ChequeDate,
                                DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue, decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Now, DrpAccountType.SelectedIndex, "", txtRemarks.Text
                                , long.Parse(DrpBankAccount.SelectedValue.ToString()),1);


                            foreach (GridViewRow dr in GrdCredit.Rows)
                            {
                                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                                if (chRelized.Checked == true)
                                {
                                    CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]));
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Pending)// && txtReceivedDate.Text == DateTime.Parse(Session["CurrentWorkDate"].ToString()).ToString("dd/MM/yyyy"))
                    {
                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpVendor.SelectedValue.ToString()), 0, txtChequeNo.Text, txtBankName.Text, ChequeDate, DateTime.Parse(Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue,
                           decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, "", DrpAccountType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));

                        CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]));
                            }
                        }
                    }
                   
                    else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Bons || int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Cancel)
                    {
                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(Session["CurrentWorkDate"].ToString()),
                                                         Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, "", DrpAccountType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

                    }

                    else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Clear)
                    {

                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(Session["CurrentWorkDate"].ToString()),
                                                         Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, "", DrpAccountType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

                        CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]));
                            }
                        }
                        ChequeRealization();
                        
                        
                        SelectCreditInvoice();
                    }
                }

                ClearAll();
                LoadReceviedCheque();
                LoadPaymentRecieved();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dt"];
        if (DrpAccountType.SelectedValue == "18")
        {
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
            GrdCheque.DataSource = dt.DefaultView;
            GrdCheque.DataBind();
        }
        else
        {
            switch (ddSearchType.SelectedIndex)
            {
                case 1:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 2:
                    dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                default:
                    dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'";
                    break;
            }
            GrdCO.DataSource = dt.DefaultView;
            GrdCO.DataBind();
        }
    }

    #endregion

    private void CashAdvance()
    {
        string remarks = "Cash " + txtRemarks.Text;
        DateTime chqDate = Constants.DateNullValue;
        int VoucherType = Constants.Cash_Payment;

        DateTime DocDate=DateTime.Parse(Session["CurrentWorkDate"].ToString());
        DocDate = DateTime.Parse(DocDate.ToShortDateString() + " 00:00:00");

        if (DrpAccountType.SelectedValue == "33")
        {
            chqDate = DateTime.Parse(txtStartDate.Text);
            chqDate = DateTime.Parse(chqDate.ToShortDateString() + " 00:00:00"); //Convert.ToDateTime(txtStartDate.Text);


            VoucherType = Constants.Expanse_Voucher;
            remarks = "Online Transfer " + DrpBankAccount.SelectedItem.Text  + ", " + txtRemarks.Text;
        }
        string MaxDocumentID = LController.SelectLedgerMaxDocumentId(VoucherType, int.Parse(drpDistributor.SelectedValue.ToString()), 1);

        LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentID), int.Parse(Configuration.PayableAccount), int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(txtAmount.Text),
                       DocDate, remarks, DateTime.Now,int.Parse(drpVendor.SelectedValue.ToString()), 0, Constants.Document_PrincipalInvoice,
                       txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0",int.Parse(DrpAccountType.SelectedValue), null, null, chqDate);
        
        LController.PostingPrinvipalInvoiceAccount(VoucherType, long.Parse(MaxDocumentID), int.Parse(DrpBankAccount.SelectedValue), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(txtAmount.Text),0,
                       DocDate, remarks, DateTime.Now,int.Parse(drpVendor.SelectedValue.ToString()), 0, Constants.Document_PrincipalInvoice,
                       txtChequeNo.Text, int.Parse(Session["UserId"].ToString()), 0, "0", int.Parse(DrpAccountType.SelectedValue), null, null, chqDate);
    }
 


}