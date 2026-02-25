using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;

/// <summary>
/// Form To Add Opening Credit
/// </summary>
public partial class Forms_frmItemStockLevel : System.Web.UI.Page
{
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly SkuHierarchyController sController = new SkuHierarchyController();
    LedgerController LedgerCtl = new LedgerController();
    /// <summary>
    /// Page_Load Function Populates All Combos, ListBox And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    /// 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
         

            LoadDistributor();
            this.LoadCategories();

            this.LoadSubCategory();
            this.LoadSKUDetail();
            loadGrid();


        }
    }
   
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSubCategory();
        this.LoadSKUDetail();
    }
    protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSKUDetail();
    }
    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
       // clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);
        clsWebFormUtil.FillListBox(chbDistributor, dt, 0, 2);
    }
    private void LoadCategories()
    {
        ddlCategory.Items.Clear();

        DataTable dt = sController.SelectSKUCategories(Constants.SKUCategory, true);
        clsWebFormUtil.FillDropDownList(ddlCategory, dt, 0, 3, false);
    }

    private void LoadSubCategory()
    {
        if (ddlCategory.Items.Count > 0)
        {
            DataTable dt = sController.SelectSkuHierarchy(Constants.SKUSubCategory, Constants.IntNullValue, int.Parse(ddlCategory.SelectedValue), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(ddlSubCategory, dt, "SKU_HIE_ID", "SKU_HIE_NAME", true);
        }
    }
    private void LoadSKUDetail()
    {
        cblCategory.Items.Clear();
        if (ddlCategory.Items.Count > 0 && ddlSubCategory.Items.Count > 0)
        {
            DataTable dtSKU = PController.SelectDataPrice2(Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(ddlSubCategory.SelectedValue), Constants.IntNullValue, int.Parse(Session["DISTRIBUTOR_ID"].ToString()), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 4, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillListBox(cblCategory, dtSKU, "SKU_ID", "SKU_NAME2");
            Session.Add("dtSKU", dtSKU);
        }
    }

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    private void ClearAll()
    {
        txtReOrderLevel.Text = "";
        txtMinStkLevel.Text = "";
        txtmaxStockLevel.Text = "";
        btnSave.Text = "Save";
       // btnSave.Text = "Update";
        ddlCategory.Enabled = true;
        cblCategory.Enabled = true;
        ChbAllCategory.Enabled = true;
        chbDistributor.Enabled = true;
        chbAllDistributor.Enabled = true;
        ddlSubCategory.Enabled = true;
    }

    private void loadGrid()
    {
        string distids = "";
        foreach (ListItem dist in chbDistributor.Items)
        {
            if (dist.Selected == true)
            {
                distids = distids + dist.Value + ",";
            }
        }
                {
            CustomerDataController cdc = new CustomerDataController();
            DataTable dt = cdc.selectStockLevel(distids, int.Parse(ddlCategory.SelectedValue), 2);
            GrdOrder.EditIndex = -1;
            GrdOrder.DataSource = dt;
            GrdOrder.DataBind();
        }
    }
    /// <summary>
    /// Sets Opening Credit Data For Edit. This Function Runs When An Existing Opening Credit Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
       ddlCategory.SelectedValue = (GrdOrder.Rows[e.NewEditIndex].Cells[2].Text);
       ddlCategory_SelectedIndexChanged(null,null);
       ddlSubCategory_SelectedIndexChanged(null,null);
        chbDistributor.ClearSelection();
        cblCategory.ClearSelection();
        chbDistributor.SelectedValue =( GrdOrder.Rows[e.NewEditIndex].Cells[0].Text);
       cblCategory.SelectedValue = (GrdOrder.Rows[e.NewEditIndex].Cells[1].Text);
        txtmaxStockLevel.Text = GrdOrder.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "0");
        txtMinStkLevel.Text = GrdOrder.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "0");
        txtReOrderLevel.Text = GrdOrder.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "0");
        for (int i = 0; i < GrdOrder.Rows.Count; i++)
        {
            GrdOrder.Rows[i].Cells[9].Enabled = false;
        }

        btnSave.Text = "Update";
        ddlCategory.Enabled = false;
        cblCategory.Enabled = false;
        ChbAllCategory.Enabled = false;
        chbDistributor.Enabled = false;
        chbAllDistributor.Enabled = false;
        ddlSubCategory.Enabled = false;
       // drpDistributor.Enabled = false;
    }

    /// <summary>
    /// Deletes Opening Credit Record
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //hfLegendID.Value = GrdOrder.Rows[e.RowIndex].Cells[5].Text;
        //hfCustomerID.Value = GrdOrder.Rows[e.RowIndex].Cells[0].Text;
        //hfSaleInvoiceID.Value = GrdOrder.Rows[e.RowIndex].Cells[7].Text;


        LedgerCtl.insertStockLevel(Convert.ToInt32(GrdOrder.Rows[e.RowIndex].Cells[0].Text), 0, Convert.ToInt32(GrdOrder.Rows[e.RowIndex].Cells[1].Text), 0,0, 0,
                                        Convert.ToDateTime(Session["CurrentWorkDate"]), 0, 4, 0);
        ClearAll();
        loadGrid();


    }

    /// <summary>
    /// Saves/Updates Opening Credit Record
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
       
        try
        {
          
            if (btnSave.Text == "Update")
            {
                LedgerCtl.insertStockLevel(Convert.ToInt32(chbDistributor.SelectedValue), 0, Convert.ToInt32(cblCategory.SelectedValue), int.Parse(txtMinStkLevel.Text), Convert.ToInt32(txtmaxStockLevel.Text), Convert.ToInt32(txtReOrderLevel.Text),
                                     Convert.ToDateTime(Session["CurrentWorkDate"]), Convert.ToInt32(Session["UserId"]), 3, int.Parse(ddlCategory.SelectedValue));
            }

          else
            {
                foreach (ListItem dist in chbDistributor.Items)
                {
                    if (dist.Selected == true)
                    {
                        foreach (ListItem sku in cblCategory.Items)
                        {
                            if (sku.Selected == true)
                            {

                                LedgerCtl.insertStockLevel(Convert.ToInt32(dist.Value), 0, Convert.ToInt32(sku.Value), int.Parse(txtMinStkLevel.Text), Convert.ToInt32(txtmaxStockLevel.Text), Convert.ToInt32(txtReOrderLevel.Text),
                                    Convert.ToDateTime(Session["CurrentWorkDate"]), Convert.ToInt32(Session["UserId"]),1,int.Parse(ddlCategory.SelectedValue));
                            }
                        }
                    }
                }
            }
           
          
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message + "');", true);
        }
        ClearAll();
        loadGrid();
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

    protected void chbDistributor_SelectedIndexChanged1(object sender, EventArgs e)
    {
        loadGrid();
    }
}
