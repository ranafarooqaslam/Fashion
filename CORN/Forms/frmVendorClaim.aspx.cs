using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using CORNCommon.Classes;
using CORNBusinessLayer.Classes;

public partial class Forms_frmVendorClaim : System.Web.UI.Page
{
    DataTable ClaimedSKU; 
    private static int URowId;
    readonly LedgerController LController = new LedgerController();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadClaimType();
            LoadAccountHead();

            CreatTableValue();

            LoadVendor();
            LoadGrid();

            btnAddNew.Attributes.Add("onclick", "return ValidateValueForm();");
            ScriptManager.GetCurrent(Page).SetFocus(drpDistributor);
            lblRowId.Text = "-1";
        }
    }

    private void LoadDistributor()
    {
        var dController = new DistributorController();
        var dt = dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }

    private void LoadClaimType()
    {
        RbdClaimType.Items.Add(new ListItem("Credit", Constants.CreditClaim.ToString()));
        RbdClaimType.Items.Add(new ListItem("Debit", Constants.DebitClaim.ToString()));
        RbdClaimType.SelectedIndex = 0;
    }

    private void LoadAccountHead()
    {
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dt = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, Constants.LongNullValue);
        clsWebFormUtil.FillDropDownList(DrpAccountHead, dt, 0, 4, true);

    }

    private void CreatTableValue()
    {
        DataTable dtVoucher = new DataTable();
        dtVoucher.Columns.Add("Account_Head_Id", typeof(long));
        dtVoucher.Columns.Add("Account_Code", typeof(string));
        dtVoucher.Columns.Add("Account_Name", typeof(string));
        dtVoucher.Columns.Add("Debit", typeof(decimal));
        dtVoucher.Columns.Add("Credit", typeof(decimal));
        dtVoucher.Columns.Add("Remarks", typeof(string));
        Session.Add("dtVoucher", dtVoucher);
        GrdOrder.DataSource = dtVoucher;
        GrdOrder.DataBind();

    }



    private void LoadVendor()
    {
        drpVendor.Items.Clear();

            VenderEntryController VendorCtl = new VenderEntryController();

        try
        {
            DataTable dtVendor = VendorCtl.GetVendor(Constants.IntNullValue);

            if (dtVendor != null)
            {
                clsWebFormUtil.FillDropDownList(drpVendor, dtVendor, 0, 2, true);
            }
        }
        catch (Exception)
        {
            
            throw;
        }

        
    }
    private void LoadGrid()
    {
        if (drpDistributor.Items.Count > 0)
        {
            var lController = new LedgerController();
            var dt = lController.SelectClaimDetail(int.Parse(drpDistributor.SelectedValue), int.Parse(RbdClaimType.SelectedValue),
                DateTime.Parse(Session["CurrentWorkDate"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()),1);
            GrdOrder.EditIndex = -1;
            GrdOrder.DataSource = dt;
            GrdOrder.DataBind();
        }
    }


    private void ClearGrd_Order()
    {
        txtRemarks.Text = "";
        txtAmount.Text = "";
        btnAddNew.Text = "Save";

    }

    protected void GrdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LedgerController LController = new LedgerController();
        DataControl dc = new DataControl();

        //uPDATE  VENDOR lEDGER,gl MASTER DETAIL

        LController.UpdateVendorLedger(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(dc.chkNull_0(GrdOrder.Rows[e.RowIndex].Cells[3].Text)),
        long.Parse(dc.chkNull_0(GrdOrder.Rows[e.RowIndex].Cells[6].Text)), Convert.ToString(GrdOrder.Rows[e.RowIndex].Cells[8].Text), decimal.Parse(dc.chkNull_0(GrdOrder.Rows[e.RowIndex].Cells[7].Text)), 2);

        ClearGrd_Order();
        LoadGrid();

    }
    private void EnableDisable(bool flag)
    {
        if (flag == true)
        {
            drpDistributor.Enabled = false;
            drpVendor.Enabled = false;
        }
        else
        {
            drpDistributor.Enabled = true;
            drpVendor.Enabled = true;
        }
    }
    protected void GrdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        URowId = Convert.ToInt32(GrdOrder.Rows[e.NewEditIndex].Cells[0].Text);
        
        drpVendor.SelectedValue = Convert.ToString(GrdOrder.Rows[e.NewEditIndex].Cells[2].Text);
     //   drpPrincipal.SelectedValue = Convert.ToString(GrdOrder.Rows[e.NewEditIndex].Cells[9].Text);
        DrpAccountHead.SelectedValue = Convert.ToString(GrdOrder.Rows[e.NewEditIndex].Cells[1].Text);
        txtAmount.Text = Convert.ToString(GrdOrder.Rows[e.NewEditIndex].Cells[7].Text);
        txtRemarks.Text = Convert.ToString(GrdOrder.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", ""));

        for (int i = 0; i < GrdOrder.Rows.Count; i++)
        {
            GrdOrder.Rows[i].Cells[9].Enabled = false;
            GrdOrder.Rows[i].Cells[10].Enabled = false;
        }
        EnableDisable(true);

        btnAddNew.Text = "Update";
    }

    

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnAddNew.Text == "Save")
            {
                string MaxDocumentId = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, int.Parse(drpDistributor.SelectedValue.ToString()), 1);
                Session.Add("VoucherNo", MaxDocumentId);

                if (RbdClaimType.SelectedValue == Constants.CreditClaim.ToString())
                {

                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), int.Parse(DrpAccountHead.SelectedValue), int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(txtAmount.Text),
                                   DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtRemarks.Text, DateTime.Now,int.Parse(drpVendor.SelectedValue.ToString()), 0, Constants.IntNullValue,
                                   null, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.CreditClaim, null, null, Constants.DateNullValue);

                    
                }
                else
                {

                    LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), int.Parse(DrpAccountHead.SelectedValue), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(txtAmount.Text), 0,
                                   DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtRemarks.Text, DateTime.Now, int.Parse(drpVendor.SelectedValue.ToString()),0, Constants.IntNullValue,
                                   null, int.Parse(Session["UserId"].ToString()), 0, "0", Constants.DebitClaim, null, null, Constants.DateNullValue);
                  
                }
            }
            else
            {
                DataControl dc = new DataControl();


                //uPDATE  VENDOR lEDGER,gl MASTER DETAIL
                LController.UpdateVendorLedgerClaim(int.Parse(drpDistributor.SelectedValue),URowId, long.Parse(DrpAccountHead.SelectedValue), decimal.Parse(dc.chkNull_0(txtAmount.Text)), txtRemarks.Text, int.Parse(RbdClaimType.SelectedValue));

                EnableDisable(false);

            }


            ClearGrd_Order();
            LoadGrid();
        }
        catch (Exception ex)
        {
 
        }
    }

    #region Index/Change

    protected void RbdClaimType_SelectedIndexChanged(object sender, EventArgs e)
    {

        LoadGrid();
        btnAddNew.Text = "Save";
        EnableDisable(false);

    }
    #endregion


}