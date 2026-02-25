using System;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Data;

public partial class Forms_fmPrincipalOpening : System.Web.UI.Page
{
    
    private static long masterId=Constants.LongNullValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            
            LoadVendor();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtOpeningDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            LoadOpeningInformation();
            txtOpeningDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDistributor()
    {
        var dController = new DistributorController();
        var dt = dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
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
    #region Opening Balance

    private void LoadOpeningInformation()
    {
        PurchaseController mPurchase = new PurchaseController();

        if (drpVendor.Items.Count > 0)
        {
            DataTable dtOpening = mPurchase.SelectPrincipalOpening(Convert.ToInt32(drpDistributor.SelectedValue), int.Parse(drpVendor.SelectedValue));
            if (dtOpening.Rows.Count > 0)
            {
                if (string.Format("{0:0.00}", dtOpening.Rows[0]["TOTAL_AMOUNT"]) == dtOpening.Rows[0]["DEBIT_AMOUNT"].ToString())
                {
                    txtOpeningDate.Text = Convert.ToDateTime(dtOpening.Rows[0]["DOCUMENT_DATE"]).ToString("dd-MMM-yyyy");

                    txtOpeningBalance.Text = string.Format("{0:0.00}", dtOpening.Rows[0]["TOTAL_AMOUNT"]);
                    txtOpeningBalanceRemarks.Text = dtOpening.Rows[0]["BUILTY_NO"].ToString();
                    rblOpening.SelectedValue = dtOpening.Rows[0]["TYPE_ID"].ToString();
                    masterId = Convert.ToInt64(dtOpening.Rows[0]["PURCHASE_MASTER_ID"].ToString());
                    btnSaveOpeningBalance.Enabled = true;
                    btnSaveOpeningBalance.Text = "Update";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Plz Delete Realization for Update.');", true);
                    btnSaveOpeningBalance.Enabled = false;
                }
            }
            else
            {

                Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
                txtOpeningDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

                txtOpeningBalance.Text = "0";
                txtOpeningBalanceRemarks.Text = "";
                rblOpening.SelectedValue = "0";
                masterId = Constants.LongNullValue;
                btnSaveOpeningBalance.Enabled = true;
                btnSaveOpeningBalance.Text = "Save";
            }
        }
        else
        {

            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtOpeningDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtOpeningBalance.Text = "0";
            txtOpeningBalanceRemarks.Text = "";
            rblOpening.SelectedValue = "0";
            masterId = Constants.LongNullValue;
            btnSaveOpeningBalance.Enabled = true;
            btnSaveOpeningBalance.Text = "Save";
        }
    }

    protected void btnOpeningBalance_Click(object sender, EventArgs e)
    {
        DataControl dc = new DataControl();
        LedgerController ledgerCtl = new LedgerController();
        DateTime dtOpening = Constants.DateNullValue;
       
        if (txtOpeningDate.Text.Length > 0)
        {
            dtOpening = Convert.ToDateTime(txtOpeningDate.Text);
        }
        if (btnSaveOpeningBalance.Text == "Save" && masterId==Constants.LongNullValue)
        {

            if (ledgerCtl.InsertVendorOpening(Convert.ToInt32(drpDistributor.SelectedValue), txtOpeningBalanceRemarks.Text, int.Parse(rblOpening.SelectedValue)
                , Convert.ToDateTime(txtOpeningDate.Text), int.Parse(drpVendor.SelectedValue), Convert.ToDecimal(dc.chkNull_0(txtOpeningBalance.Text))
                , int.Parse(this.Session["UserId"].ToString()), ref masterId))
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Opening Added Successfully.');", true);

                btnSaveOpeningBalance.Text = "Update";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
            }
        }
        else
        {

            if (ledgerCtl.DeletePrincipalOpening(Convert.ToInt32(drpDistributor.SelectedValue), Convert.ToInt32(drpVendor.SelectedValue), masterId))
            {
                if (ledgerCtl.InsertVendorOpening(Convert.ToInt32(drpDistributor.SelectedValue), txtOpeningBalanceRemarks.Text, int.Parse(rblOpening.SelectedValue)
                    , Convert.ToDateTime(txtOpeningDate.Text), int.Parse(drpVendor.SelectedValue), Convert.ToDecimal(dc.chkNull_0(txtOpeningBalance.Text))
                    , int.Parse(this.Session["UserId"].ToString()),ref masterId))
                {
                  ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Opening Updated Successfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
            }
        }
    }
   
    #endregion
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOpeningInformation();
    }
  
    protected void drpVendor_SelectedIndexChanged(object sender, EventArgs e)
    {       
        LoadOpeningInformation();
    }
}