using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return and Damage
/// </summary>
/// 
public partial class Forms_frmPurchaseEntryTrasferout  : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly PhaysicalStockController PhyscialCtrl = new PhaysicalStockController();
    readonly DataControl dc = new DataControl();

    private static decimal PrivouseQty, FreePrivousQty;
    DataTable PurchaseSKUS;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GrdPurchase.Columns[10].Visible = false;
            LoadPrincipal();
            LoadDistributor();
            LoadSKUDetail();
            CreatTable();
            GetDocumentNo();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtAmount.Attributes.Add("readonly", "readonly");
            DrpDocumentType_SelectedIndexChanged(null, null);

        }
    }

   
    private void CreatTable()
    {
        PurchaseSKUS = new DataTable();
        PurchaseSKUS.Columns.Add("PURCHASE_DETAIL_ID", typeof(long));
        PurchaseSKUS.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKUS.Columns.Add("SKU_Code", typeof(string));
        PurchaseSKUS.Columns.Add("SKU_Name", typeof(string));
        PurchaseSKUS.Columns.Add("BATCH_NO", typeof(string));
        PurchaseSKUS.Columns.Add("PRICE", typeof(decimal));
        PurchaseSKUS.Columns.Add("Quantity", typeof(int));
        PurchaseSKUS.Columns.Add("FREE_SKU", typeof(int));
        PurchaseSKUS.Columns.Add("AMOUNT", typeof(decimal));
        PurchaseSKUS.Columns.Add("PACKSIZE", typeof(string));
        PurchaseSKUS.Columns.Add("COLOR", typeof(string));
        Session.Add("PurchaseSKUS", PurchaseSKUS);

    }


    #region Index Change

    protected void DrpDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableDisable();

        lblInvoice.Text = "INV/DC  No";

        if (DrpDocumentType.SelectedValue == "2")
        {
            lblfromLocation.Text = "Purchase For";
            drpDistributor.Enabled = true;

            DrpTransferFor.Visible = false;
            Label4.Visible = false;

            lbltoLocation.Visible = true;
            drpPrincipal.Visible = true;

            GetDocumentNo();

        }
        else if (DrpDocumentType.SelectedValue == "5")
        {
            lbltoLocation.Visible = false;
            drpPrincipal.Visible = false;

            lblfromLocation.Text = "Transfer From";

            drpDistributor.Enabled = true;
            LoadToDistributor();
            DrpTransferFor.Visible = true;
            lblInvoice.Text = "Driver Name";
            Label4.Visible = true;
            Label4.Text = "Transfer To";

            GetDocumentNo();
        }
        else if (DrpDocumentType.SelectedValue == "3")
        {
            lblfromLocation.Text = "Return From";
            drpDistributor.Enabled = true;

            DrpTransferFor.Visible = false;
            Label4.Visible = false;
            GetDocumentNo();
            lbltoLocation.Visible = true;
            drpPrincipal.Visible = true;
        }

        else //dAMAGE
        {
            lblfromLocation.Text = "Location";
            drpDistributor.Enabled = true;

            DrpTransferFor.Visible = false;
            Label4.Visible = false;
            GetDocumentNo();

            lbltoLocation.Visible = false;
            drpPrincipal.Visible = false;
        }
    }

    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableDisable();

        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            Session.Add("PurchaseSKUS", PurchaseSKUS);
            LoadGird();
            ClearAll();
            txtQuantity.Text = "1";
            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            DrpDocumentType.Enabled = true;
        }
        else
        {
            txtBuiltyNo.Text = "";
            txtDocumentNo.Text = "";
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;
            DrpDocumentType.Enabled = false;
            LoadDocumentDetail();
            LoadSKUDetail();
        }
    }
    

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSKUDetail();
    }

    protected void chkScan_CheckedChanged(object sender, EventArgs e)
    {
        txtPrice.Text = string.Empty;
        if (chkScan.Checked)
        {
            drpSkus.Visible = false;
            txtskuCode.Visible = true;
            GrdPurchase.Columns[10].Visible = false;
        }
        else
        {
            drpSkus.Visible = true;
            txtskuCode.Visible = false;
            GrdPurchase.Columns[10].Visible = true;
        }

        drpSkus_SelectedIndexChanged(null, null);
    }

    #endregion

    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(int.Parse(DrpDocumentType.SelectedValue.ToString()), Constants.IntNullValue, Constants.LongNullValue, int.Parse(Session["UserId"].ToString()), 0);
        drpDocumentNo.Items.Add(new clsListItems("New", Constants.LongNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(drpDocumentNo, dt, 0, 0);
    }

    private void EnableDisable()
    {
        txtPrice.Text = "";
        if (DrpDocumentType.SelectedValue == "2")
        {
            txtPrice.Enabled = true;
            txtAmount.Enabled = true;
        }
        else if (DrpDocumentType.SelectedValue == "5")
        {
            txtPrice.Enabled = false;
            txtAmount.Enabled = false;
        }
        else
        {
            txtPrice.Enabled = false;
            txtAmount.Enabled = false;
        }
    }


    #region Load

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
    }

    private void LoadToDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(DrpTransferFor, dt, 0, 2, true);
    }

    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
            DrpTransferFor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
            txtDocumentNo.Text = dt.Rows[0][2].ToString();
            txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            txtDocumentNo.Text = dt.Rows[0][2].ToString();
            txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            PurchaseSKUS = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
            Session.Add("PurchaseSKUS", PurchaseSKUS);
            Session.Add("PurchaseSKUSDocument", PurchaseSKUS);
            LoadGird();
        }
    }

    private void LoadSKUDetail()
    {
        if (drpPrincipal.Items.Count > 0)
        {
            DataTable Dtsku_Price = PController.SelectDataPrice2(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 8, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillDropDownList(drpSkus, Dtsku_Price, "SKU_ID", "SKU_DETAIL", true);

            Session.Add("Dtsku_Price", Dtsku_Price);
        }
    }

    private void LoadGird()
    {
        decimal TotalValue = 0;
        PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];
        GrdPurchase.EditIndex = -1;
        GrdPurchase.DataSource = PurchaseSKUS;
        GrdPurchase.DataBind();
        foreach (DataRow dr in PurchaseSKUS.Rows)
        {
            TotalValue += decimal.Parse(dr["Quantity"].ToString());

        }
        txtTotalQuantity.Text = TotalValue.ToString();
    }

    private void LoadPrincipal()
    {
        //DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));

        VenderEntryController VendorCtl = new VenderEntryController();
        try
        {

            DataTable dtVendor = VendorCtl.GetVendor(Constants.IntNullValue);
            if (dtVendor != null)
            {
                clsWebFormUtil.FillDropDownList(drpPrincipal, dtVendor, 0, 2, true);
            }
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion 

    #region Grid Operations

    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {

        hfRowNo.Value = Convert.ToString(e.NewEditIndex);

        txtskuCode.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[1].Text;
        txtskuName.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[2].Text;
        txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        PrivouseQty = decimal.Parse(GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text);
        FreePrivousQty = 0;
        txtsize.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        txtcolor.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        if (DrpDocumentType.SelectedValue == "2")
        {
            txtPrice.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text;
            txtAmount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[9].Text;
        }
        if (DrpDocumentType.SelectedValue == "5")
        {
            txtPrice.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text;
            txtAmount.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[9].Text;
        }
        drpSkus.Enabled = false;
        txtskuCode.Enabled = false;
        txtQuantity.Focus();
        for (int i = 0; i < GrdPurchase.Rows.Count; i++)
        {
            GrdPurchase.Rows[i].Cells[10].Enabled = false;
            GrdPurchase.Rows[i].Cells[11].Enabled = false;
        }
        btnSave.Text = "Update";

    }
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];
        if (PurchaseSKUS.Rows.Count > 0)
        {
            PurchaseSKUS.Rows.RemoveAt(e.RowIndex);
            Session.Add("PurchaseSKUS", PurchaseSKUS);
            LoadGird();
        }
    }

    #endregion

    /// <summary>
    /// Enables/Disables Batch No TextBox
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    //protected void ChbBatchNo_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (ChbBatchNo.Checked == true)
    //    {
    //        //lblBatchNo.Enabled = true;
    //        //txtBatchNo.Enabled = true;
    //    }
    //    else
    //    {
    //        //txtBatchNo.Text = "N/A"; 
    //        //lblBatchNo.Enabled = false;
    //        //txtBatchNo.Enabled = false;
    //    }
    //}

    /// <summary>
    /// Enables/Disables Apply Free SKU TextBox
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    //protected void ChbFreeSKU_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (ChbFreeSKU.Checked == true)
    //    {
    //        //lblFreeSKU.Enabled = true;
    //        //txtFreeSKU.Enabled = true;
    //    }
    //    else
    //    {
    //        //txtFreeSKU.Text = "0";
    //        //lblFreeSKU.Enabled = false;
    //        //txtFreeSKU.Enabled = false;
    //    }
    //}

    #region Click Operations

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = null;

        if (chkScan.Checked)
        {
            foundRows = Dtsku_Price.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        }
        else
        {
            foundRows = Dtsku_Price.Select("SKU_ID  = '" + drpSkus.SelectedValue + "'");
        }

        if (foundRows.Length > 0)
        {
            if (Convert.ToDecimal(dc.chkNull_0(foundRows[0]["DISTRIBUTOR_PRICE"].ToString())) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "PriceError();", true);
                return;
            }

            PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];

            decimal CurrentStock = CheckStockStatus(int.Parse(dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));
            decimal CurrentStockTo = CheckStockStatusTo(int.Parse(dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));
            if (btnSave.Text == "Add")
            {
                if (CurrentStock == -1)
                {
                    if (CurrentStockTo == -1)
                    {
                        DataRow[] foundRowsaddd = null;
                        if (chkScan.Checked)
                        {
                            foundRowsaddd = PurchaseSKUS.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
                        }
                        else
                        {
                            foundRowsaddd = PurchaseSKUS.Select("SKU_ID  = '" + drpSkus.SelectedValue + "'");
                        }
                        if (foundRowsaddd.Length > 0)
                        {
                            foundRowsaddd[0]["Quantity"] = decimal.Parse(foundRowsaddd[0]["Quantity"].ToString()) + decimal.Parse(txtQuantity.Text);
                        }
                        else
                        {
                            DataRow dr = PurchaseSKUS.NewRow();
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                            dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                            dr["COLOR"] = foundRows[0]["COLOR"];
                            dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                            dr["Quantity"] = decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                            if (Convert.ToDecimal(dc.chkNull_0(foundRows[0]["DISTRIBUTOR_PRICE"].ToString())) <= 0)
                            {
                                txtPrice.Text = "0.00";
                                return;
                            }
                            else
                            {
                                txtPrice.Text = foundRows[0]["DISTRIBUTOR_PRICE"].ToString();
                            }
                            dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                            dr["AMOUNT"] = decimal.Parse(dr["PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                            PurchaseSKUS.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " allowed Trasfer Qty is " + CurrentStockTo.ToString() + "');", true);
                        txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Current closing Stock is " + CurrentStock.ToString() + "');", true);
                    txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                    return;
                }
            }
            else if (btnSave.Text == "Update")
            {
                if (CurrentStock == -1)
                {
                    if (CurrentStockTo == -1)
                    {
                        DataRow[] foundRowsaddd = null;
                        if (chkScan.Checked)
                        {
                            foundRowsaddd = PurchaseSKUS.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
                        }
                        else
                        {
                            foundRowsaddd = PurchaseSKUS.Select("SKU_ID  = '" + drpSkus.SelectedValue + "'");
                        }
                        if (foundRowsaddd.Length > 0)
                        {
                            if (chkScan.Checked)
                            {
                                foundRowsaddd[0]["Quantity"] = decimal.Parse(foundRowsaddd[0]["Quantity"].ToString()) + decimal.Parse(txtQuantity.Text);
                            }
                            else
                            {
                                foundRowsaddd[0]["Quantity"] = decimal.Parse(txtQuantity.Text);
                            }
                        }
                        else
                        {
                            DataRow dr = PurchaseSKUS.Rows[Convert.ToInt32(hfRowNo.Value)];
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                            dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                            dr["COLOR"] = foundRows[0]["COLOR"];
                            dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                            dr["BATCH_NO"] = "";
                            dr["Quantity"] = decimal.Parse(txtQuantity.Text);
                            dr["FREE_SKU"] = 0;
                            if (DrpDocumentType.SelectedValue == "5")
                            {
                                if (Convert.ToDecimal(dc.chkNull_0(foundRows[0]["DISTRIBUTOR_PRICE"].ToString())) <= 0)
                                {
                                    txtPrice.Text = "0.00";
                                    return;
                                }
                                else
                                {
                                    txtPrice.Text = foundRows[0]["DISTRIBUTOR_PRICE"].ToString();
                                }
                                dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                                dr["AMOUNT"] = decimal.Parse(dr["PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " allowed Transfer Qty is " + CurrentStock.ToString() + "');", true);
                        txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + "Current closing Stock is " + CurrentStock.ToString() + "');", true);
                    txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                    return;
                }
            }
            Session.Add("PurchaseSKUS", PurchaseSKUS);
            ClearAll();
            LoadGird();
            DisAbaleOption(true);
            ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong SKU Select');", true);
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        if (drpDistributor.SelectedValue.ToString() == DrpTransferFor.SelectedValue.ToString())
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "LocationError();", true);
            return;
        }       
        PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];
        if (PurchaseSKUS.Rows.Count > 0)
        {
            DistributorController mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()));
            if (ValidDayClose(dt.Rows[0]["CLOSING_DATE"].ToString()))
            {
                if (dt.Rows.Count > 0)
                {
                    if (CalculatePurchase(DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString())))
                    {
                        PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];
                        PurchaseSKUS.Rows.Clear();
                        Session.Add("PurchaseSKUS", PurchaseSKUS);
                        LoadGird();
                        GetDocumentNo();
                        drpDistributor.Enabled = true;
                        drpPrincipal.Enabled = true;
                        DrpDocumentType.Enabled = true;
                        ClearAll();
                        txtQuantity.Text = "1";
                        txtBuiltyNo.Text = "";
                        txtDocumentNo.Text = "";
                        txtDocumentNo.Text = "";
                        DisAbaleOption(false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "WrongLocation();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "DayClose();", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "DetailError();", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DisAbaleOption(false);
        CreatTable();
        LoadGird();
        ClearAll();
        txtQuantity.Text = "1";
        txtDocumentNo.Text = "";
        txtBuiltyNo.Text = "";
        txtDocumentNo.Text = "";
    }

    #endregion

    private bool CheckDublicateSKU()
    {
        PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];

        DataRow[] foundRows = null;

        if (chkScan.Checked)
        {
            foundRows = PurchaseSKUS.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        }
        else
        {
            foundRows = PurchaseSKUS.Select("SKU_ID  = '" + drpSkus.SelectedValue + "'");
        }
        if (foundRows.Length == 0)
        {
            return true;
        }
        return false;
    }

    private bool CalculatePurchase(DateTime MWorkDate)
    {
        decimal mTotalAmount = 0;

        PurchaseController mController = new PurchaseController();
        DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKUS"];
        foreach (DataRow dr in dtPurchaseDetail.Rows)
        {
            mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());

        }
        if (DrpDocumentType.SelectedValue == "2")
        {
            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = mController.InsertPurchaseDocument(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()));
                return mResult;
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocument(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()));
                return mResult;
            }
            //dtPurchaseDetail

        }
        else if (DrpDocumentType.SelectedValue == "5")
        {
            long mResult = 0;

            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                bool flag = true;
                foreach (DataRow dr in dtPurchaseDetail.Rows)
                {
                    flag = CheckStockStatusTo2(Convert.ToInt32(dr["SKU_ID"]), Convert.ToInt32(dr["QUANTITY"]), 0);
                    if (!flag)
                    {
                        break;
                    }
                }
                if (flag)
                {
                    mResult = mController.InsertTransferOutDocument(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                  , MWorkDate, int.Parse(DrpTransferFor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString())
                  , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), 0);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Transfer Qty is greater than allowed stock');", true);
                }
            }
            else
            {
                DataTable PurchaseSKUSDocument = (DataTable)Session["PurchaseSKUSDocument"];
                bool flag = true;
                foreach (DataRow dr in dtPurchaseDetail.Rows)
                {
                    int PrevQty = 0;
                    foreach(DataRow dr2 in PurchaseSKUSDocument.Rows)
                    {
                        if(dr["SKU_ID"].ToString() == dr2["SKU_ID"].ToString())
                        {
                            PrevQty = Convert.ToInt32(dr2["QUANTITY"]);
                            break;
                        }

                    }
                    flag = CheckStockStatusTo2(Convert.ToInt32(dr["SKU_ID"]), Convert.ToInt32(dr["QUANTITY"]), PrevQty);
                    if (!flag)
                    {
                        break;
                    }
                }
                if (flag)
                {
                    mResult = mController.UpdateTransferOutDocument(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                  , MWorkDate, int.Parse(DrpTransferFor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString())
                  , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), 0);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Transfer Qty is greater than allowed stock');", true);
                }
            }
            if (mResult > 0)
            {
                ShowReport(mResult);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (DrpDocumentType.SelectedValue == "3")
        {
            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = mController.InsertPurchaseDocument(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()));
                return mResult;
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocument(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpTransferFor.SelectedValue.ToString())
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()));
                return mResult;
            }
        }
        else
        {
            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = mController.InsertPurchaseDocument(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), 0);
                return mResult;
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocument(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), 0);
                return mResult;
            }
        }
    }

    private bool ValidDayClose(string closingDate) {

       
       
            DistributorController mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(DrpTransferFor.SelectedValue.ToString()));

            if (dt.Rows[0]["CLOSING_DATE"].ToString() == closingDate)
            {
                return true;
            }
            return false;
       
    }
    private decimal CheckStockStatus(int SKU_ID)
    {
       
        {
            PhaysicalStockController mController = new PhaysicalStockController();
            DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.SelectedValue.ToString()), SKU_ID, "N/A", DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            if (dt.Rows.Count > 0)
            {
                if (decimal.Parse(dt.Rows[0][0].ToString()) + PrivouseQty + FreePrivousQty >= decimal.Parse(txtQuantity.Text))
                {
                    return -1;
                }
                else
                {
                    return decimal.Parse(dt.Rows[0][0].ToString()) + PrivouseQty + FreePrivousQty;
                }
            }
        }

        return 0;
    }

    private decimal CheckStockStatusTo(int SKU_ID)
    {

        {
            PhaysicalStockController mController = new PhaysicalStockController();
            DataTable dt = mController.SelectSKUClosingStock2(int.Parse(DrpTransferFor.SelectedValue.ToString()), SKU_ID, "N/A", DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            int StockLevel = GetMaxStockLevel(Convert.ToInt32(DrpTransferFor.SelectedValue), SKU_ID);
            if(StockLevel == -1)
            {
                return -1;
            }

            if (dt.Rows.Count > 0)
            {
                if (StockLevel -(decimal.Parse(dt.Rows[0][0].ToString()) - PrivouseQty - FreePrivousQty) >= decimal.Parse(txtQuantity.Text))
                {
                    return -1;
                }
                else
                {
                    return StockLevel - (decimal.Parse(dt.Rows[0][0].ToString()) + PrivouseQty + FreePrivousQty);
                }
            }
        }
        return 0;
    }
    private bool CheckStockStatusTo2(int SKU_ID,int Qty,int PrivouseQty2)
    {
        PhaysicalStockController mController = new PhaysicalStockController();
        int StockLevel = GetMaxStockLevel(Convert.ToInt32(DrpTransferFor.SelectedValue), SKU_ID);
        if (StockLevel == -1)
        {
            return true;
        }
        DataTable dt = mController.SelectSKUClosingStock2(int.Parse(DrpTransferFor.SelectedValue.ToString()), SKU_ID, "N/A", DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        if (dt.Rows.Count > 0)
        {
            if (StockLevel - (decimal.Parse(dt.Rows[0][0].ToString()) - PrivouseQty2) >= Qty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private void DisAbaleOption(bool IsDisable)
    {
        if (IsDisable == true)
        {
            DrpDocumentType.Enabled = false;
            drpPrincipal.Enabled = false;
            drpDistributor.Enabled = false;
            drpDocumentNo.Enabled = false;
        }
        else
        {
            DrpDocumentType.Enabled = true;
            drpPrincipal.Enabled = true;
            drpDistributor.Enabled = true;
            drpDocumentNo.Enabled = true;
            drpDocumentNo.SelectedIndex = 0;
        }
    }

    private void ClearAll()
    {
        txtskuCode.Text = "";
        txtskuName.Text = "";        
        txtskuCode.Enabled = true;
        drpSkus.Enabled = true;
        btnSave.Text = "Add";
        PrivouseQty = 0;
        FreePrivousQty = 0;
        txtcolor.Text = "";
        txtsize.Text = "";
        txtPrice.Text = "";
        txtAmount.Text = "";
    }

    private void ShowReport(long Id)
    {
        //try
        //{
        //    CORNBusinessLayer.Reports.LatestDataSet ds = new CORNBusinessLayer.Reports.LatestDataSet();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("BarCode", typeof(string));
        //    dt.Columns.Add("Description", typeof(string));
        //    dt.Columns.Add("QTY", typeof(decimal));
        //    dt.Columns.Add("From", typeof(string));
        //    dt.Columns.Add("To", typeof(string));
        //    dt.Columns.Add("DriverName", typeof(string));
        //    dt.Columns.Add("TransferDate", typeof(DateTime));
        //    int i = 0;
        //    DataTable dtPurchaseDetail = (DataTable)Session["PurchaseSKUS"];
        //    foreach (DataRow dr in dtPurchaseDetail.Rows)
        //    {
        //        dt.Rows.Add();
        //        dt.Rows[i]["BarCode"] = dr["SKU_Code"];
        //        dt.Rows[i]["Description"] = dr["SKU_Name"];
        //        dt.Rows[i]["QTY"] = Convert.ToDecimal(dr["Quantity"]);
        //        dt.Rows[i]["From"] = DrpTransferFor.SelectedItem.Text;
        //        dt.Rows[i]["To"] = drpDistributor.SelectedItem.Text;
        //        dt.Rows[i]["DriverName"] = txtDocumentNo.Text;
        //        dt.Rows[i]["TransferDate"] = Convert.ToDateTime(Session["CurrentWorkDate"].ToString());
        //        i += 1;
        //    }
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        ds.Tables["StockRegisterTransferOut"].ImportRow(dr);
        //    }
        //    CORNBusinessLayer.Classes.DocumentPrintController DPrint = new CORNBusinessLayer.Classes.DocumentPrintController();

        //    CORNBusinessLayer.Reports.CRPStockRegisterTransferOut CrpReport = new CORNBusinessLayer.Reports.CRPStockRegisterTransferOut();
        //    CrpReport.SetDataSource(ds);
        //    CrpReport.Refresh();


        //    Session.Add("CrpReport", CrpReport);
        //    Session.Add("ReportType", 0);
        //    string url = "'Default.aspx'";
        //    string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        //    Type cstype = GetType();
        //    ClientScriptManager cs = Page.ClientScript;
        //    cs.RegisterStartupScript(cstype, "OpenWindow1", script);
        //}
        //catch (Exception ex)
        //{
        //    ex.Message.ToString();
        //}
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();

        

        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        DataSet ds = RptInventoryCtl.SelectTransferDocument(int.Parse(drpDistributor.SelectedValue.ToString()), Id, DateTime.Parse(Session["CurrentWorkDate"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString()), 5);


        CrystalDecisions.CrystalReports.Engine.ReportDocument CrpReport = new CORNBusinessLayer.Reports.CrpTransferDocument2();
        
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();


        CrpReport.SetParameterValue("DocumentType", "Transfer Out Document");
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());

        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }

    protected void drpSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedValue == "5")
        {
            showPrice();
        }
    }

    protected void txtskuCode_TextChanged(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedValue == "5")
        {
            showPrice();
        }
    }
    private void showPrice()
    {
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = null;

        if (chkScan.Checked)
        {
            foundRows = Dtsku_Price.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        }
        else
        {
            foundRows = Dtsku_Price.Select("SKU_ID  = '" + drpSkus.SelectedValue + "'");
        }



        if (foundRows.Length > 0)
        {
            if (Convert.ToDecimal(dc.chkNull_0(foundRows[0]["DISTRIBUTOR_PRICE"].ToString())) <= 0)
            {
                txtPrice.Text = "0.00";
                return;
            }
            else
            {
                txtPrice.Text = foundRows[0]["DISTRIBUTOR_PRICE"].ToString();
            }
        }
    }

    private int GetMaxStockLevel(int pDistributorID,int pSKUID)
    {
        int stock = -1;
        DataTable dtStockLevel = PhyscialCtrl.GetItemMaxStockLevel(pDistributorID, pSKUID);
        if(dtStockLevel.Rows.Count > 0)
        {
            stock = Convert.ToInt32(dtStockLevel.Rows[0]["MAX_LEVEL"]);
        }
        return stock;
    }
}