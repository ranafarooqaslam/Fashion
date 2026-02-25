using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// From To Adjust Stock
/// </summary>
public partial class Forms_frmOpeningStock : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly DataControl dc = new DataControl();
    readonly General general = new General();
    DataTable PurchaseSKU;
    private static int decmalPlaces = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CreatTable();
            GetDocumentNo();
            LoadDistributor();
            LoadSKUDetail();
            DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
            if (dtAppSetting != null)
            {
                DataRow[] drAppSetting = dtAppSetting.Select("strColumnName='QtyDecPlaces'");
                if (drAppSetting.Length > 0)
                {
                    decmalPlaces = Convert.ToInt32(dc.chkNull_0(drAppSetting[0]["strColumnValue"].ToString()));
                    txtQuantity.Text = general.DecimalValue(1, decmalPlaces);
                }
            }
        }
    }

    /// <summary>
    /// Creates Datatable For Document
    /// </summary>
    private void CreatTable()
    {
        PurchaseSKU = new DataTable();
        PurchaseSKU.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        PurchaseSKU.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKU.Columns.Add("SKU_Code", typeof(string));
        PurchaseSKU.Columns.Add("SKU_Name", typeof(string));
        PurchaseSKU.Columns.Add("BATCH_NO", typeof(string));
        PurchaseSKU.Columns.Add("PRICE", typeof(decimal));
        PurchaseSKU.Columns.Add("Quantity", typeof(decimal));
        PurchaseSKU.Columns.Add("FREE_SKU", typeof(decimal));
        PurchaseSKU.Columns.Add("AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("PACKSIZE", typeof(string));
        PurchaseSKU.Columns.Add("COLOR", typeof(string));

        Session.Add("PurchaseSKU", PurchaseSKU);
    }

    /// <summary>
    /// Gets Document Nos
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetDocumentNo();
    }

    /// <summary>
    /// Gets Document Nos
    /// </summary>
    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(int.Parse(DrpDocumentType.SelectedValue.ToString()), Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);
        drpDocumentNo.Items.Add(new clsListItems("New", Constants.LongNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(drpDocumentNo, dt, 0, 0);
    }

    /// <summary>
    /// Loads Document Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            LoadGird();
            drpDistributor.Enabled = true;
            DrpDocumentType.Enabled = true;
            ClearAll();
            txtDocumentNo.Text = "";
            DisAbaleOption(false);
        }
        else
        {
            LoadDocumentDetail();
            LoadSKUDetail();
        }
    }



    /// <summary>
    /// Loads SKU Detail To ListBox
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }

    /// <summary>
    /// Loads Document Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSKUDetail();
    }

    /// <summary>
    /// Loads SKU Detail To ListBox
    /// </summary>
    private void LoadSKUDetail()
    {
        DataTable Dtsku_Price = null;
        if (drpDistributor.Items.Count > 0)
        {

            Dtsku_Price = PController.SelectDataPrice2(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 3, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillListBox(lstCode, Dtsku_Price, "SKU_CODE", "SKU_DETAIL", true);

            Session.Add("Dtsku_Price", Dtsku_Price);

        }
        else
        {
            Session.Add("Dtsku_Price", Dtsku_Price);
        }
    }

    private void LoadGird()
    {
        PurchaseSKU = (DataTable)Session["PurchaseSKU"];
        GrdPurchase.EditIndex = -1;
        GrdPurchase.DataSource = PurchaseSKU;
        GrdPurchase.DataBind();
    }


    /// <summary>
    /// Sets Dcoment Detail Data For Edit. This Function Runs When An Existing Document Detail Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hfRowNo.Value = Convert.ToString(e.NewEditIndex);

        txtskuCode.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[1].Text;
        txtskuName.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[2].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        txtsize.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        txtcolor.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtskuCode.Enabled = false;
        txtQuantity.Focus();
        btnSave.Text = "Update Sku";
        for (int i = 0; i < GrdPurchase.Rows.Count; i++)
        {
            GrdPurchase.Rows[i].Cells[8].Enabled = false;
            GrdPurchase.Rows[i].Cells[9].Enabled = false;
        }
    }

    /// <summary>
    /// Deletes Document Detail
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PurchaseSKU = (DataTable)Session["PurchaseSKU"];
        if (PurchaseSKU.Rows.Count > 0)
        {
            PurchaseSKU.Rows.RemoveAt(e.RowIndex);
            Session.Add("PurchaseSKU", PurchaseSKU);
            LoadGird();
        }
    }

    /// <summary>
    /// Loads Document Detail To Document Detail Grid
    /// </summary>
    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();

            txtDocumentNo.Text = dt.Rows[0][2].ToString();
            PurchaseSKU = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
            Session.Add("PurchaseSKU", PurchaseSKU);
            LoadGird();
        }
    }

    /// <summary>
    /// Checks Duplicate SKU in Grid
    /// </summary>
    /// <returns>bool</returns>
    private bool CheckDublicateSKU()
    {
        PurchaseSKU = (DataTable)Session["PurchaseSKU"];

        DataRow[] foundRows = PurchaseSKU.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds Document Detail To Document Detail Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>  
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = null;
        foundRows = Dtsku_Price.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        if (foundRows.Length > 0)
        {
            if (Convert.ToDecimal(dc.chkNull_0(foundRows[0]["DISTRIBUTOR_PRICE"].ToString())) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Purchase Price');", true);
                return;
            }
            PurchaseSKU = (DataTable)Session["PurchaseSKU"];
            if (txtQuantity.Text == "" || txtQuantity.Text.Equals(null))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must enter Quantity !');", true);
                txtQuantity.Focus();
                return;
            }
            else
            {
                if (DrpDocumentType.SelectedValue == "8")
                {
                    decimal CurrentStock = CheckStockStatus(int.Parse(dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));
                    if (CurrentStock != -1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Current closing Stock is " + CurrentStock.ToString() + "');", true);
                        return;
                    }
                }

                if (btnSave.Text == "Add Sku")
                {
                    if (CheckDublicateSKU())
                    {
                        DataRow dr = PurchaseSKU.NewRow();
                        dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                        dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                        dr["COLOR"] = foundRows[0]["COLOR"];
                        dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                        dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                        dr["Quantity"] = decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                        dr["AMOUNT"] = decimal.Parse(foundRows[0]["DISTRIBUTOR_PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                        PurchaseSKU.Rows.Add(dr);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Already Exists ');", true);
                        txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                        return;
                    }
                }
                else if (btnSave.Text == "Update Sku")
                {
                    DataRow dr = PurchaseSKU.Rows[Convert.ToInt32(hfRowNo.Value)];
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                    dr["COLOR"] = foundRows[0]["COLOR"];
                    dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                    dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                    dr["Quantity"] = decimal.Parse(txtQuantity.Text);
                    dr["AMOUNT"] = decimal.Parse(foundRows[0]["DISTRIBUTOR_PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                }
            }
            Session.Add("PurchaseSKU", PurchaseSKU);
            ClearAll();
            LoadGird();
            DisAbaleOption(true);
            ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong SKU please check in list');", true);
        }
    }

    /// <summary>
    /// Saves Document
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        DistributorController mDayClose = new DistributorController();
        DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()));
        if (dt.Rows.Count > 0)
        {
            DateTime MWorkDate = DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString());

            PurchaseController mController = new PurchaseController();
            DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKU"];
            decimal mTotalAmount = 0;

            foreach (DataRow dr in dtPurchaseDetail.Rows)
            {
                mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());

            }
            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = mController.InsertPurchaseDocumentDecimal(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                      , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0, mTotalAmount, false, dtPurchaseDetail, 0, null, int.Parse(Session["UserId"].ToString()), 0, 0);
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocumentDecimal(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                   , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0, mTotalAmount, false, dtPurchaseDetail, 0, null, int.Parse(Session["UserId"].ToString()), 0, 0);
            }

            lblErrorMsg.Text = "Record Upated";
            PurchaseSKU = (DataTable)Session["PurchaseSKU"];
            PurchaseSKU.Rows.Clear();
            Session.Add("PurchaseSKU", PurchaseSKU);
            LoadGird();
            GetDocumentNo();
            drpDistributor.Enabled = true;
            DrpDocumentType.Enabled = true;
            ClearAll();
            txtDocumentNo.Text = "";
            DisAbaleOption(false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong Location or Unassigne');", true);
        }
    }
    private decimal CheckStockStatus(int SKU_ID)
    {
        if (DrpDocumentType.SelectedIndex == 0)
        {
            return -1;
        }
        else
        {
            //lblStock.Text = "Closing Stock: 0";
            PhaysicalStockController mController = new PhaysicalStockController();
            DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.SelectedValue.ToString()), SKU_ID, "N/A", DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            if (dt != null && dt.Rows.Count > 0)
            {
                if (decimal.Parse(dt.Rows[0][0].ToString()) >= decimal.Parse(txtQuantity.Text))
                {
                    //lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", decimal.Parse(dt.Rows[0][0].ToString()));
                    return -1;
                }
                else if (decimal.Parse(dt.Rows[0][0].ToString()) <= 0)
                {
                    //lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", decimal.Parse(dt.Rows[0][0].ToString()));
                    return 0;
                }
                else
                {
                    //lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", decimal.Parse(dt.Rows[0][0].ToString()));
                    return decimal.Parse(dt.Rows[0][0].ToString());
                }
            }
        }

        return 0;
    }
    /// <summary>
    /// Resets Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CreatTable();
        LoadGird();
        drpDistributor.Enabled = true;
        DrpDocumentType.Enabled = true;
        ClearAll();
        txtDocumentNo.Text = "";
        DisAbaleOption(false);
    }

    /// <summary>
    /// Enables/Disables Controls
    /// </summary>
    /// <param name="IsDisable">bool</param>
    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            DrpDocumentType.Enabled = false;
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;

        }
        else
        {

            DrpDocumentType.Enabled = true;
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Clears Form Controls
    /// </summary>
    private void ClearAll()
    {
        txtskuCode.Text = "";
        txtskuName.Text = "";
        txtQuantity.Text = "";
        txtcolor.Text = "";
        txtsize.Text = "";
        txtskuCode.Enabled = true;
        btnSave.Text = "Add Sku";
        lblErrorMsg.Text = "";
    }

    protected void GrdPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.DataItem != null)
            {
                e.Row.Cells[7].Text = general.DecimalValue(Convert.ToDecimal(dc.chkNull_0(e.Row.Cells[7].Text)), decmalPlaces);
            }
        }
    }
}