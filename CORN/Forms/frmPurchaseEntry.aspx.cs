using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Linq;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return and Damage
/// </summary>
/// 
public partial class Forms_frmPurchaseEntry : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    General genral = new General();
    readonly DataControl dc = new DataControl();
    public static int decmalPlaces = 0;

    private static decimal PrivouseQty, FreePrivousQty;
    DataTable PurchaseSKUS;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblStock.Visible = false;
            GrdPurchase.Columns[10].Visible = false;
            txtPrice.Enabled = true;
            LoadPrincipal();
            LoadDistributor();
            LoadSKUDetail();
            CreatTable();
            GetDocumentNo();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtAmount.Attributes.Add("readonly", "readonly");

            DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
            if (dtAppSetting != null)
            {
                DataRow[] drAppSetting = dtAppSetting.Select("strColumnName='QtyDecPlaces'");
                if (drAppSetting.Length > 0)
                {
                    decmalPlaces = Convert.ToInt32(dc.chkNull_0(drAppSetting[0]["strColumnValue"].ToString()));
                    txtQuantity.Text = genral.DecimalValue(1, decmalPlaces);
                }
            }
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
        PurchaseSKUS.Columns.Add("Quantity", typeof(decimal));
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
        lblDiscount.Visible = false;
        lblNetAmount.Visible = false;
        txtDiscount.Visible = false;
        txtNetAmount.Visible = false;
        lblStock.Visible = false;
        if (DrpDocumentType.SelectedValue == "2")
        {
            lblfromLocation.Text = "Purchase For";
            drpDistributor.Enabled = true;
            DrpTransferFor.Visible = false;
            Label4.Visible = false;
            lbltoLocation.Visible = true;
            drpPrincipal.Visible = true;
            lblDiscount.Visible = true;
            lblNetAmount.Visible = true;
            txtDiscount.Visible = true;
            txtNetAmount.Visible = true;
            GetDocumentNo();

            if (Session["dtAppSetting"] != null)
            {
                var dt = (DataTable)Session["dtAppSetting"];
                if (dt.Rows.Count > 0)
                {
                    DataRow[] drAppSetting = dt.Select("strColumnName='ShowPurPriceInStockReg'");
                    if (drAppSetting.Length > 0)
                    {
                        var showPurPrice = drAppSetting[0]["strColumnValue"].ToString();
                        if (showPurPrice == "1")
                        {
                            ShowPurchasePrice();
                            txtPrice.Enabled = false;
                        }
                    }
                }
            }
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
            showPrice();
            lblStock.Visible = true;
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

        else
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
            txtPrice.Enabled = true;
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

        Session.Add("dtLocationInfo", dt);
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
        DataTable dt = mPurchase.SelectPurchaseDocumentNo(Constants.IntNullValue, Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            if (DrpDocumentType.SelectedIndex == 0)
            {
                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
                drpPrincipal.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
                txtDiscount.Text = Math.Round(Convert.ToDecimal(dt.Rows[0]["DISCOUNT"]),4).ToString();
            }
            else if(DrpDocumentType.SelectedIndex == 4)
            {
                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            }
            else if (DrpDocumentType.SelectedIndex == 1)
            {
                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
                DrpTransferFor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            }
            else if (DrpDocumentType.SelectedIndex == 2)
            {
                drpPrincipal.SelectedValue = dt.Rows[0]["PRINCIPAL_ID"].ToString();
                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            }
            else
            {
                DrpTransferFor.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
                txtDocumentNo.Text = dt.Rows[0][2].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
            }
            PurchaseSKUS = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
            Session.Add("PurchaseSKUS", PurchaseSKUS);
            LoadGird();
        }
    }

    private void LoadSKUDetail()
    {
        txtPrice.Enabled = true;
        if (drpPrincipal.Items.Count > 0)
        {
            DateTime CurrentWorkDate = Constants.DateNullValue;
            DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
            foreach (DataRow dr in dtLocationInfo.Rows)
            {
                if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.SelectedValue.ToString())
                {
                    if (dr["MaxDayClose"].ToString().Length > 0)
                    {
                        CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                        lblWorkDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
                        break;
                    }
                }
            }


            DataTable Dtsku_Price = PController.SelectDataPrice2(Constants.IntNullValue, 
                Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                int.Parse(drpDistributor.SelectedValue.ToString()),
                int.Parse(Session["UserId"].ToString()), Constants.IntNullValue,
                9, CurrentWorkDate);

            clsWebFormUtil.FillDropDownList(drpSkus, Dtsku_Price, "SKU_ID", "SKU_DETAIL", true);

            Session.Add("Dtsku_Price", Dtsku_Price);

            if (DrpDocumentType.SelectedValue == "2")
            {
                if (Session["dtAppSetting"] != null)
                {
                    var dt = (DataTable)Session["dtAppSetting"];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow[] drAppSetting = dt.Select("strColumnName='ShowPurPriceInStockReg'");
                        if (drAppSetting.Length > 0)
                        {
                            var showPurPrice = drAppSetting[0]["strColumnValue"].ToString();
                            if (showPurPrice == "1")
                            {
                                ShowPurchasePrice();
                                txtPrice.Enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void LoadGird()
    {
        decimal TotalValue = 0;
        decimal TotalAmount = 0;
        decimal Discount = Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text));
        PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];
        GrdPurchase.EditIndex = -1;
        GrdPurchase.DataSource = PurchaseSKUS;
        GrdPurchase.DataBind();
        foreach (DataRow dr in PurchaseSKUS.Rows)
        {
            TotalValue += decimal.Parse(dr["Quantity"].ToString());
            TotalAmount += decimal.Parse(dr["Amount"].ToString());
        }
        txtTotalQuantity.Text = TotalValue.ToString();
        txtTotalAmount.Text = TotalAmount.ToString();
        txtNetAmount.Text = (TotalAmount - Discount).ToString();
    }

    private void LoadPrincipal()
    {
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


        if (chkScan.Checked)
        {
            txtskuCode.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[1].Text;
            txtskuName.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[2].Text;
        }
        else
        {
            drpSkus.SelectedValue = GrdPurchase.Rows[e.NewEditIndex].Cells[0].Text;
        }

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

    #region Click Operations

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (DrpDocumentType.SelectedValue == "2" || DrpDocumentType.SelectedValue == "5")
        {
            if (Convert.ToDecimal(dc.chkNull_0(txtPrice.Text)) <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "PriceError();", true);
                return;
            }
        }

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
            if (DrpDocumentType.SelectedValue != "2" && DrpDocumentType.SelectedValue != "5")
            {
                if (Convert.ToDecimal(dc.chkNull_0(foundRows[0]["DISTRIBUTOR_PRICE"].ToString())) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "PriceError();", true);
                    return;
                }
            }

            PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];

            decimal CurrentStock = CheckStockStatus(int.Parse(dc.chkNull_0(foundRows[0]["SKU_ID"].ToString())));

            if (btnSave.Text == "Add")
            {
                bool checkDuplicateSKUS = false; // either to check duplicate allowed or not.

                if (DrpDocumentType.SelectedValue == "2" || DrpDocumentType.SelectedValue == "5")
                {
                    if (Session["dtAppSetting"] != null)
                    {
                        var dt = (DataTable)Session["dtAppSetting"];
                        if (dt.Rows.Count > 0)
                        {
                            DataRow[] drAppSetting = dt.Select("strColumnName='CheckDuplicateSKUinStock'");
                            if (drAppSetting.Length > 0)
                            {
                                var checkduplicate = drAppSetting[0]["strColumnValue"].ToString();
                                if (checkduplicate == "1")
                                {
                                    checkDuplicateSKUS = true;
                                }
                            }
                        }
                    }
                }

                if (DrpDocumentType.SelectedValue == "2")
                {
                    if (CheckDublicateSKU(checkDuplicateSKUS))
                    {
                        if (CurrentStock == -1)
                        {
                            DataRow dr = PurchaseSKUS.NewRow();
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                            dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                            dr["COLOR"] = foundRows[0]["COLOR"];
                            dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                            dr["Quantity"] = decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                            dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                            dr["AMOUNT"] = decimal.Parse(dc.chkNull_0(txtPrice.Text)) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                            dr["FREE_SKU"] = 0;

                            PurchaseSKUS.Rows.Add(dr);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Current closing Stock is " + CurrentStock.ToString() + "');", true);
                            txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Already Exists ');", true);
                        txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                        return;
                    }
                }
                else if (DrpDocumentType.SelectedValue == "5")
                {
                    if (CheckDublicateSKU(checkDuplicateSKUS))
                    {
                        if (CurrentStock == -1)
                        {
                            DataRow dr = PurchaseSKUS.NewRow();
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                            dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                            dr["COLOR"] = foundRows[0]["COLOR"];
                            dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                            dr["Quantity"] = decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                            dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                            dr["AMOUNT"] = decimal.Parse(dr["PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));

                            PurchaseSKUS.Rows.Add(dr);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Current closing Stock is " + CurrentStock.ToString() + "');", true);
                            txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Already Exists ');", true);
                        txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                        return;
                    }
                }
                else
                {
                    if (CheckDublicateSKU(true))
                    {
                        if (CurrentStock == -1)
                        {
                            DataRow dr = PurchaseSKUS.NewRow();
                            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                            dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];

                            dr["COLOR"] = foundRows[0]["COLOR"];
                            dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];

                            dr["Quantity"] = decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                            dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                            dr["AMOUNT"] = decimal.Parse(foundRows[0]["DISTRIBUTOR_PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));

                            PurchaseSKUS.Rows.Add(dr);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Current closing Stock is " + CurrentStock.ToString() + "');", true);
                            txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + " Already Exists ');", true);
                        txtskuName.Text = foundRows[0]["SKU_NAME"].ToString();
                        return;
                    }
                }
            }
            else if (btnSave.Text == "Update")
            {
                if (CurrentStock == -1)
                {
                    DataRow dr = PurchaseSKUS.Rows[Convert.ToInt32(hfRowNo.Value)];
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                    dr["COLOR"] = foundRows[0]["COLOR"];
                    dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                    dr["BATCH_NO"] ="";

                    dr["Quantity"] = decimal.Parse(txtQuantity.Text);
                    dr["FREE_SKU"] = 0;

                    if (DrpDocumentType.SelectedValue == "2")
                    {
                        dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                        dr["AMOUNT"] = decimal.Parse(dc.chkNull_0(txtPrice.Text)) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));

                    }
                    else if (DrpDocumentType.SelectedValue == "5")
                    {
                        dr["PRICE"] = decimal.Parse(dc.chkNull_0(txtPrice.Text));
                        dr["AMOUNT"] = decimal.Parse(dr["PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));

                    }
                    else
                    {
                        dr["PRICE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                        dr["AMOUNT"] = decimal.Parse(foundRows[0]["DISTRIBUTOR_PRICE"].ToString()) * decimal.Parse(dc.chkNull_0(txtQuantity.Text));
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
            txtQuantity.Text = "1";
            //drpSkus_SelectedIndexChanged(null, null);
            ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Wrong SKU Select');", true);

        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        
        if (DrpDocumentType.SelectedIndex == 1 || DrpDocumentType.SelectedIndex == 3)
        {
            if (drpDistributor.SelectedValue.ToString() == DrpTransferFor.SelectedValue.ToString())
            {
               
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "LocationError();", true);
                return;
            }
        }
        PurchaseSKUS = (DataTable)Session["PurchaseSKUS"];

        if (PurchaseSKUS.Rows.Count > 0)
        {
            if (DrpDocumentType.SelectedIndex == 0 || DrpDocumentType.SelectedIndex == 3)
            {
               
            }
            else
            {
                var result = from row in PurchaseSKUS.AsEnumerable()
                             group row by row.Field<int>("SKU_ID") into skuGroup
                             select new
                             {
                                 SKU_ID = skuGroup.Key,
                                 TotalQty = skuGroup.Sum(r => r.Field<decimal>("Quantity"))
                             };

                foreach (var item in result)
                {
                    //Console.WriteLine($"SKU_ID: {item.SKU_ID}, Total Amount: {item.TotalAmount}");
                    decimal closingStock = CheckItemClosingStock(item.SKU_ID);

                    if (item.TotalQty > closingStock)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('  " + txtskuCode.Text + "Current closing Stock is " + closingStock.ToString() + "');", true);
                    }
                }
            }

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
        txtDocumentNo.Text = "";
        txtBuiltyNo.Text = "";
        txtDocumentNo.Text = "";
    }

    #endregion

    private bool CheckDublicateSKU(bool checkDuplicate)
    {
        if (checkDuplicate == false)
        {
            return true;
        }

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
        if (DrpDocumentType.SelectedValue == "2")//Purchase
        {
            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = mController.InsertPurchaseDocumentDecimal(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()),Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)));
                return mResult;
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocumentDecimal(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()), Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)));
                return mResult;
            }
        }
        else if (DrpDocumentType.SelectedValue == "5")
        {
            long mResult=0;

            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                  mResult = mController.InsertTransferOutDocument(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(DrpTransferFor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString())
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()),0);

            }
            else
            {
                mResult = mController.UpdateTransferOutDocument(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(DrpTransferFor.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString())
                , mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), 0);
            }
            if (mResult > 0)
            {
                ShowReport(mResult, MWorkDate);
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
                bool mResult = mController.InsertPurchaseDocumentDecimal(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0, mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()),0);
                return mResult;
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocumentDecimal(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), int.Parse(drpPrincipal.SelectedValue.ToString()),0);
                return mResult;
            }
        }
        else
        {
            if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
            {
                bool mResult = mController.InsertPurchaseDocumentDecimal(int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0, mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), 0,0);
                return mResult;
            }
            else
            {
                bool mResult = mController.UpdatePurchaseDocumentDecimal(int.Parse(drpDocumentNo.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, int.Parse(DrpDocumentType.SelectedValue.ToString())
                , MWorkDate, int.Parse(drpDistributor.SelectedValue.ToString()), 0, mTotalAmount, false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()),0,0);
                return mResult;
            }
        }
    }

    private bool ValidDayClose(string closingDate) {

        if (DrpDocumentType.SelectedIndex == 1)
        {
            DistributorController mDayClose = new DistributorController();
            DataTable dt = mDayClose.SelectMaxDayClose(Constants.IntNullValue, int.Parse(DrpTransferFor.SelectedValue.ToString()));

            if (dt.Rows[0]["CLOSING_DATE"].ToString() == closingDate)
            {
                return true;
            }
            return false;
        }
        return true;
    }
    private decimal CheckStockStatus(int SKU_ID)
    {
        if (DrpDocumentType.SelectedIndex == 0)
        {
            return -1;
        }
        else
        {
            lblStock.Text = "Closing Stock: 0";
            PhaysicalStockController mController = new PhaysicalStockController();
            DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.SelectedValue.ToString()), SKU_ID, "N/A", DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            if (dt != null && dt.Rows.Count > 0)
            {
                if (decimal.Parse(dt.Rows[0][0].ToString()) >= decimal.Parse(txtQuantity.Text))
                {
                    lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", decimal.Parse(dt.Rows[0][0].ToString()));
                    return -1;
                }
                else if (decimal.Parse(dt.Rows[0][0].ToString()) <= 0)
                {
                    lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", decimal.Parse(dt.Rows[0][0].ToString()));
                    return 0;
                }
                else
                {
                    lblStock.Text = "Closing Stock: " + String.Format("{0:0.00}", decimal.Parse(dt.Rows[0][0].ToString()));
                    return decimal.Parse(dt.Rows[0][0].ToString());
                }
            }
        }

        return 0;
    }

    private decimal CheckItemClosingStock(int SKU_ID)
    {
        decimal closingStock = 0;
        PhaysicalStockController mController = new PhaysicalStockController();
        DataTable dt = mController.SelectSKUClosingStock2(int.Parse(drpDistributor.SelectedValue.ToString()),
            SKU_ID, "N/A", DateTime.Parse(Session["CurrentWorkDate"].ToString()));

        if (dt != null && dt.Rows.Count > 0)
        {
            closingStock = decimal.Parse(dt.Rows[0][0].ToString());
        }

        return closingStock;
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
        txtQuantity.Text = "";
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

    private void ShowReport(long Id, DateTime MWorkDate)
    {
        DocumentPrintController mController = new DocumentPrintController();
        RptInventoryController RptInventoryCtl = new RptInventoryController();
        DataTable dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));
        DataSet ds = RptInventoryCtl.SelectTransferDocument(
            int.Parse(drpDistributor.SelectedValue.ToString()), Id,
            MWorkDate,
            MWorkDate, 5);

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
        txtPrice.Enabled = true;

        if (DrpDocumentType.SelectedValue == "5")
        {
            showPrice();
        }
        else if (DrpDocumentType.SelectedValue == "2")
        {
            if (Session["dtAppSetting"] != null)
            {
                var dt = (DataTable)Session["dtAppSetting"];
                if (dt.Rows.Count > 0)
                {
                    DataRow[] drAppSetting = dt.Select("strColumnName='ShowPurPriceInStockReg'");
                    if (drAppSetting.Length > 0)
                    {
                        var showPurPrice = drAppSetting[0]["strColumnValue"].ToString();
                        if (showPurPrice == "1")
                        {
                            ShowPurchasePrice();
                            txtPrice.Enabled = false;
                        }
                    }
                }
            }
        }

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
            CheckStockStatus(int.Parse(foundRows[0]["SKU_ID"].ToString()));
        }
    }

    protected void txtskuCode_TextChanged(object sender, EventArgs e)
    {
        txtPrice.Enabled = true;

        if (DrpDocumentType.SelectedValue == "5")
        {
            showPrice();
        }
        else if (DrpDocumentType.SelectedValue == "2")
        {
            if (Session["dtAppSetting"] != null)
            {
                var dt = (DataTable)Session["dtAppSetting"];
                if (dt.Rows.Count > 0)
                {
                    DataRow[] drAppSetting = dt.Select("strColumnName='ShowPurPriceInStockReg'");
                    if (drAppSetting.Length > 0)
                    {
                        var showPurPrice = drAppSetting[0]["strColumnValue"].ToString();
                        if (showPurPrice == "1")
                        {
                            ShowPurchasePrice();
                            txtPrice.Enabled = false;
                        }
                    }
                }
            }
        }

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
            CheckStockStatus(int.Parse(foundRows[0]["SKU_ID"].ToString()));
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
            txtPrice.Text = foundRows[0]["DISTRIBUTOR_PRICE"].ToString();

            if (Session["dtAppSetting"] != null)
            {
                var dt = (DataTable)Session["dtAppSetting"];
                if (dt.Rows.Count > 0)
                {
                    DataRow[] drAppSetting = dt.Select("strColumnName='ShowRetailPriceInTransferOut'");
                    if (drAppSetting.Length > 0)
                    {
                        var showPurPrice = drAppSetting[0]["strColumnValue"].ToString();
                        if (showPurPrice == "1")
                        {
                            txtPrice.Text = foundRows[0]["RETAIL_PRICE"].ToString();
                            txtPrice.Enabled = false;
                        }
                    }
                }
            }
        }
        
    }
    private void ShowPurchasePrice()
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
            if (Convert.ToDecimal(dc.chkNull_0(foundRows[0]["PURCHASE_PRICE"].ToString())) <= 0)
            {
                txtPrice.Text = "0.00";
                return;
            }
            else
            {
                txtPrice.Text = foundRows[0]["PURCHASE_PRICE"].ToString();
            }
        }
    }

    protected void GrdPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType ==  DataControlRowType.DataRow)
        {
            if(e.Row.DataItem != null)
            {
                e.Row.Cells[7].Text = genral.DecimalValue(Convert.ToDecimal(dc.chkNull_0(e.Row.Cells[7].Text)), decmalPlaces);
            }
        }
    }
}