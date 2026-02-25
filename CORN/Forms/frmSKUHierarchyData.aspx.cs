using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;

/// <summary>
/// From To Add, Edit, Delete SKU Hierarchy
/// </summary>
public partial class frmSKUHierarchyData : System.Web.UI.Page
{
    SkuHierarchyController mController = new SkuHierarchyController();
    static int PrincipalId;
    static int DivisionId;
    static int CategoryId;
    static int SubCategoryId;
    static int BrandId;
    static int GenderId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LoadPrincipal();
            this.LoadDivision();
//this.LoadCategoryDivision();
          //  this.LoadBrandDivision();
            this.LoadCategroy();
            this.LoadSubCategory();
            this.LoadBrand();
          //  this.loadType();
            this.LoadTag();
            TabContainer1.ActiveTabIndex = 0;
        }
    }

    #region Principal Tab

    /// <summary>
    /// Loads Principals To Principal Grid On Principal Tab And To All Principal Combos On Form
    /// </summary>
    private void LoadPrincipal()
    {
        DataTable dt = mController.SelectPrincipal(Constants.SKUPrincipal, int.Parse(this.Session["CompanyId"].ToString()));
        GrdPrincipal.EditIndex = -1;
        GrdPrincipal.DataSource = dt;
        GrdPrincipal.Columns[0].Visible = true;
        GrdPrincipal.Columns[4].Visible = true;
        GrdPrincipal.Columns[5].Visible = true;
        GrdPrincipal.Columns[6].Visible = true;
        GrdPrincipal.DataBind();
        GrdPrincipal.Columns[0].Visible = false;
        GrdPrincipal.Columns[4].Visible = false;
        GrdPrincipal.Columns[5].Visible = false;
        GrdPrincipal.Columns[6].Visible = false;
       // clsWebFormUtil.FillDropDownList(this.dddivisonPrincipal, dt, 0, 3, true);
       // clsWebFormUtil.FillDropDownList(this.DrpCategoryPrincipal, dt, 0, 3, true);
      //  clsWebFormUtil.FillDropDownList(this.DrpBrandPrincipal, dt, 0, 3, true);
     //   clsWebFormUtil.FillDropDownList(this.drpTagPrincipal, dt, 0, 3, true);
    }

    /// <summary>
    /// Loads Divisions To Division Grid On Division Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void dddivisonPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadDivision();
    }

    /// <summary>
    /// Loads Categories To Category Grid On Category Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpCategoryPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
       // this.LoadCategoryDivision();
        this.LoadCategroy();
    }

    /// <summary>
    /// Loads Brands To Brand Grid On Brand Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpBrandPrincipal_SelectedIndexChanged(object sender, EventArgs e) 
    {
       // this.LoadBrandDivision();
        //this.LoadBrandCategory();
        this.LoadBrand();
    }


    /// <summary>
    /// Sets Principal Data For Edit. This Function Runs When An Existing Principal Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPrincipal_RowEditing(object sender, GridViewEditEventArgs e)
    {
        PrincipalId = int.Parse(GrdPrincipal.Rows[e.NewEditIndex].Cells[0].Text);
        txtPrincipalCode.Text = GrdPrincipal.Rows[e.NewEditIndex].Cells[1].Text;
        txtPrincipalName.Text = GrdPrincipal.Rows[e.NewEditIndex].Cells[2].Text;
        if (GrdPrincipal.Rows[e.NewEditIndex].Cells[3].Text == "True")
        {
            ChIsMunalDiscount.Checked = true;
        }
        else
        {
            ChIsMunalDiscount.Checked = false;
        }
        txtAddress.Text = GrdPrincipal.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
        txtNTN.Text = GrdPrincipal.Rows[e.NewEditIndex].Cells[5].Text;
        txtSTRN.Text = GrdPrincipal.Rows[e.NewEditIndex].Cells[6].Text;
        txtPrincipalName.Enabled = true;
        txtAddress.Enabled = true;
        txtNTN.Enabled = true;
        txtSTRN.Enabled = true;
        txtPrincipalCode.Enabled = true;
        btnSavePrincipal.Text = "Update";
        for (int i = 0; i < GrdPrincipal.Rows.Count; i++)
        {
            GrdPrincipal.Rows[i].Cells[7].Enabled = false;
            GrdPrincipal.Rows[i].Cells[8].Enabled = false;
        }
    }

    /// <summary>
    /// Deletes A Principal
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPrincipal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PrincipalId = int.Parse(GrdPrincipal.Rows[e.RowIndex].Cells[0].Text);
        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUDivision, PrincipalId);
        if (dt.Rows.Count > 0)
        {
            lblErrorMsg.Text = "Wrong Command: first delete associated division";
        }
        else
        {
            mController.UpdateHierarchy(Constants.SKUPrincipal, PrincipalId, Constants.IntNullValue, null, null, null, false, int.Parse(this.Session["CompanyId"].ToString()));
            this.LoadPrincipal();
            lblErrorMsg.Text = "";
        }
    }

    /// <summary>
    /// Sets PageIndex Of Principal Grid On Principal Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void GrdPrincipal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GrdPrincipal.PageIndex = e.NewPageIndex;
        this.LoadPrincipal();

    }

    /// <summary>
    /// Save Or Updates a Principal
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSavePrincipal_Click(object sender, EventArgs e)
    {
        lblErrorMsg.Text = "";
        if (btnSavePrincipal.Text == "New")
        {
           // txtPrincipalCode.Text = this.GetAutoCode("SC", 0);
            txtPrincipalName.Enabled = true;
            txtAddress.Enabled = true;
            txtNTN.Enabled = true;
            txtSTRN.Enabled = true;
            txtPrincipalCode.Enabled = true;
            txtPrincipalCode.Focus();
            btnSavePrincipal.Text = "Save";
            ScriptManager.GetCurrent(Page).SetFocus(txtPrincipalCode);
        }
        else if (btnSavePrincipal.Text == "Save")
        {
            if (txtPrincipalName.Text.Length == 0||txtPrincipalCode.Text.Length==0)
            {
                lblErrorMsg.Text = "Must Enter Principal Name and Code";
                return;
            }
            mController.InsertPrincipal(Constants.SKUPrincipal, Constants.IntNullValue, txtPrincipalCode.Text, txtPrincipalName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()), ChIsMunalDiscount.Checked, txtAddress.Text, txtNTN.Text, txtSTRN.Text);
           // this.SetAutoCode("SC", long.Parse(txtPrincipalCode.Text.Substring(2)));
            btnSavePrincipal.Text = "New";
            txtPrincipalCode.Text = "";
            txtPrincipalName.Text = "";
            txtAddress.Text = "";
            txtNTN.Text = "";
            txtSTRN.Text = "";
            txtPrincipalName.Enabled = false;
            txtAddress.Enabled = false;
            txtNTN.Enabled = false;
            txtSTRN.Enabled = false;
            txtPrincipalCode.Enabled = false;
            this.LoadPrincipal();

        }
        else if (btnSavePrincipal.Text == "Update")
        {
            if (txtPrincipalName.Text.Length == 0)
            {
                lblErrorMsg.Text = "Must Enter Principal Name";
                return;
            }
            mController.UpdatePrincipal(Constants.SKUPrincipal, PrincipalId, Constants.IntNullValue, txtPrincipalCode.Text, txtPrincipalName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()), ChIsMunalDiscount.Checked, txtAddress.Text, txtNTN.Text, txtSTRN.Text);
            btnSavePrincipal.Text = "New";
            txtPrincipalCode.Text = "";
            txtPrincipalName.Text = "";
            txtAddress.Text = "";
            txtNTN.Text = "";
            txtSTRN.Text = "";
            txtPrincipalName.Enabled = false;
            txtAddress.Enabled = false;
            txtNTN.Enabled = false;
            txtSTRN.Enabled = false;
            txtPrincipalCode.Enabled = false;
            this.LoadPrincipal();
        }

    }

    #endregion

    #region Division Tab

    /// <summary>
    /// Loads Divisions To Division Grid On Division Tab
    /// </summary>
    private void LoadDivision()
    {
        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUDivision, Constants.IntNullValue,Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
        GrdDivision.EditIndex = -1;
        GrdDivision.DataSource = dt;
       
        GrdDivision.DataBind();

    }

    /// <summary>
    /// Sets Division Data For Edit. This Function Runs When An Existing Division Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdDivision_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DivisionId = int.Parse(GrdDivision.Rows[e.NewEditIndex].Cells[0].Text);
        txtDivisionCode.Text = GrdDivision.Rows[e.NewEditIndex].Cells[1].Text;
        txtDivisionName.Text = GrdDivision.Rows[e.NewEditIndex].Cells[2].Text;
        txtDivisionName.Enabled = true;
        txtDivisionCode.Enabled = true;
        btnSaveDivison.Text = "Update";
        for (int i = 0; i < GrdDivision.Rows.Count; i++)
        {
            GrdDivision.Rows[i].Cells[3].Enabled = false;
            GrdDivision.Rows[i].Cells[4].Enabled = false;
        }
    }

    /// <summary>
    /// Deletes A Division
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdDivision_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DivisionId = int.Parse(GrdDivision.Rows[e.RowIndex].Cells[0].Text);
        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUCategory, DivisionId);
        if (dt.Rows.Count > 0)
        {
            lblErrorMsgDivsion.Text = "Wrong Command: first delete associated category";
        }
        else
        {
            mController.UpdateHierarchy(Constants.SKUDivision, DivisionId, Constants.IntNullValue, null, null, null, false, int.Parse(this.Session["CompanyId"].ToString()));
            this.LoadDivision();
            lblErrorMsgDivsion.Text = "";
        }
    }

    /// <summary>
    /// Sets PageIndex Of Division Grid On Division Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void GrdDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GrdDivision.PageIndex = e.NewPageIndex;
        this.LoadDivision();
    }

    /// <summary>
    /// Save Or Updates a Division
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveDivison_Click(object sender, EventArgs e)
    {
        if (btnSaveDivison.Text == "New")
        {
           // txtDivisionCode.Text = this.GetAutoCode("DV", 0);
            txtDivisionName.Enabled = true;
            txtDivisionCode.Enabled = true;
            txtDivisionCode.Focus();
            btnSaveDivison.Text = "Save";
          //  ScriptManager.GetCurrent(Page).SetFocus(dddivisonPrincipal);
        }
        else if (btnSaveDivison.Text == "Save")
        {
            if (txtDivisionName.Text.Length == 0)
            {
                lblErrorMsgDivsion.Text = "Must Entry Division Name";
                return;
            }
            mController.InsertHierarchy(Constants.SKUDivision, Constants.IntNullValue, txtDivisionCode.Text, txtDivisionName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
           // this.SetAutoCode("DV", long.Parse(txtDivisionCode.Text.Substring(2)));
            btnSaveDivison.Text = "New";
            txtDivisionCode.Text = "";
            txtDivisionName.Text = "";
            txtDivisionCode.Text = "";
            txtDivisionName.Enabled = false;
            this.LoadDivision();
            lblErrorMsgDivsion.Text = "";

        }
        else if (btnSaveDivison.Text == "Update")
        {
            if (txtDivisionName.Text.Length == 0)
            {
                lblErrorMsgDivsion.Text = "Must Enter Division Name";
                return;
            }
            mController.UpdateHierarchy(Constants.SKUDivision, DivisionId, Constants.IntNullValue, txtDivisionCode.Text, txtDivisionName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            btnSaveDivison.Text = "New";
            txtDivisionName.Text = "";
            txtDivisionName.Text = "";
            txtDivisionCode.Text = "";
            txtDivisionCode.Enabled = false;
            txtDivisionName.Enabled = false;
            this.LoadDivision();
        }

    }

    #endregion

    #region Category Tab

    //private void LoadCategoryDivision()
   // {
       // DataTable dt = mController.SelectSkuHierarchy(Constants.SKUDivision, Constants.IntNullValue, int.Parse(DrpCategoryPrincipal.SelectedValue.ToString()), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
       // clsWebFormUtil.FillDropDownList(this.ddCategoryDivision, dt, 0, 3, true);
    //}

    protected void ddCategoryDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadCategroy();
    }

    private void LoadCategroy()
    {
        //if (ddCategoryDivision.Items.Count > 0)
        {
            DataTable dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            GrdCategory.EditIndex = -1;
            GrdCategory.DataSource = dt;
           
            GrdCategory.DataBind();

            clsWebFormUtil.FillDropDownList(ddSubCategory, dt, 0, 3, true);
        }
    }

    protected void GrdCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //ddCategoryDivision.SelectedValue = GrdCategory.Rows[e.NewEditIndex].Cells[1].Text;
        CategoryId = int.Parse(GrdCategory.Rows[e.NewEditIndex].Cells[0].Text);
        txtCategoryCode.Text = GrdCategory.Rows[e.NewEditIndex].Cells[1].Text;
        txtCategoryName.Text = GrdCategory.Rows[e.NewEditIndex].Cells[2].Text;
        txtCategoryName.Enabled = true;
        txtCategoryCode.Enabled = true;
        btnSaveCategory.Text = "Update";
        for (int i = 0; i < GrdCategory.Rows.Count; i++)
        {
            GrdCategory.Rows[i].Cells[3].Enabled = false;
            GrdCategory.Rows[i].Cells[4].Enabled = false;
        }
    }

    protected void GrdCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        CategoryId = int.Parse(GrdCategory.Rows[e.RowIndex].Cells[0].Text);
        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUSubCategory, CategoryId,int.Parse(Session["CompanyId"].ToString()));
        if (dt.Rows.Count > 0)
        {
            lblErrorMsgCategory.Text = "Wrong Command: first delete associated Sub Category";
        }
        else
        {
            mController.UpdateHierarchy(Constants.SKUCategory, CategoryId, Constants.IntNullValue, null, null, null, false, int.Parse(this.Session["CompanyId"].ToString()));
            this.LoadCategroy();
        }
    }

    protected void GrdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GrdCategory.PageIndex = e.NewPageIndex;
        this.LoadCategroy();
    }

    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        lblErrorMsgCategory.Text = "";
        if (btnSaveCategory.Text == "New")
        {
          //  txtCategoryCode.Text = this.GetAutoCode("CA", 0);
            txtCategoryName.Enabled = true;
            txtCategoryCode.Enabled = true;
            txtCategoryCode.Focus();
            btnSaveCategory.Text = "Save";
           // ScriptManager.GetCurrent(Page).SetFocus(ddCategoryDivision);
        }
        else if (btnSaveCategory.Text == "Save")
        {
            if (txtCategoryName.Text.Length <= 0)
            {
                lblErrorMsgCategory.Text = "Must Enter Category";
                return;
            }
            mController.InsertHierarchy(Constants.SKUCategory, Constants.IntNullValue, txtCategoryCode.Text, txtCategoryName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
           // this.SetAutoCode("CA", long.Parse(txtCategoryCode.Text.Substring(2)));
            btnSaveCategory.Text = "New";
            txtCategoryCode.Text = "";
            txtCategoryName.Text = "";
            txtCategoryName.Enabled = false;
            txtCategoryCode.Enabled = false;
            this.LoadCategroy();
            lblErrorMsgCategory.Text = "";

        }
        else if (btnSaveCategory.Text == "Update")
        {
            if (txtCategoryName.Text.Length <= 0)
            {
                lblErrorMsgCategory.Text = "Must Enter Category";
                return;
            }
            mController.UpdateHierarchy(Constants.SKUCategory, CategoryId, Constants.IntNullValue, txtCategoryCode.Text, txtCategoryName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            btnSaveCategory.Text = "New";
            txtCategoryCode.Text = "";
            txtCategoryName.Text = "";
            txtCategoryCode.Enabled = false;
            txtCategoryName.Enabled = false;
            this.LoadCategroy();
        }
    }

    #endregion

    #region Sub Category Tab

    private void LoadSubCatCategory()
    {
            DataTable dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(this.ddSubCategory, dt, 0, 3, true);
        
    }

    protected void ddSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnSaveSubCategory.Text = "New";
        txtSubCategoryCode.Text = "";
        txtSubCategoryName.Text = "";
        txtSubCategoryCode.Enabled = false;
        txtSubCategoryName.Enabled = false;

        LoadSubCategory();

    }

    private void LoadSubCategory()
    {
        GrdSubCategory.DataSource = null;
        GrdSubCategory.DataBind();

         if (ddSubCategory.Items.Count > 0)
        {
            DataTable dt = mController.SelectSkuHierarchy(Constants.SKUSubCategory, Constants.IntNullValue, int.Parse(ddSubCategory.SelectedValue), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            GrdSubCategory.EditIndex = -1;
            GrdSubCategory.DataSource = dt;
            GrdSubCategory.DataBind();
        }
    }

    protected void GrdSubCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        SubCategoryId = int.Parse(GrdSubCategory.Rows[e.NewEditIndex].Cells[0].Text);
        txtSubCategoryCode.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[1].Text;
        txtSubCategoryName.Text = GrdSubCategory.Rows[e.NewEditIndex].Cells[2].Text;
        txtSubCategoryName.Enabled = true;
        txtSubCategoryCode.Enabled = true;
        btnSaveSubCategory.Text = "Update";
        for (int i = 0; i < GrdSubCategory.Rows.Count; i++)
        {
            GrdSubCategory.Rows[i].Cells[3].Enabled = false;
            GrdSubCategory.Rows[i].Cells[4].Enabled = false;
        }
    }

    protected void GrdSubCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        SkuController msku = new SkuController();

        SubCategoryId = int.Parse(GrdSubCategory.Rows[e.RowIndex].Cells[0].Text);
        //DataTable dt = msku.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, GenderId, Constants.IntNullValue);
        //if (dt.Rows.Count > 0)
        //{
        //    lblErrorMsgBrand.Text = "Wrong Command: first delete associated SKUS";
        //}
        //else
        //{
            mController.UpdateHierarchy(Constants.SKUSubCategory, SubCategoryId, Constants.IntNullValue, null, null, null, false, int.Parse(this.Session["CompanyId"].ToString()));
            this.LoadSubCategory();
        //}
       
    }

    protected void btnSaveSubCategory_Click(object sender, EventArgs e)
    {
        lblErrorMsgSubCategory.Text = "";
        if (btnSaveSubCategory.Text == "New")
        {
            //txtBrandCode.Text = this.GetAutoCode("BN", 0);
            txtSubCategoryName.Enabled = true;
            txtSubCategoryCode.Enabled = true;
            txtSubCategoryCode.Focus();
            btnSaveSubCategory.Text = "Save";
            //ScriptManager.GetCurrent(Page).SetFocus(ddBrandCategory);
        }
        else if (btnSaveSubCategory.Text == "Save")
        {
            if (txtSubCategoryName.Text.Length == 0)
            {
                lblErrorMsgSubCategory.Text = "Must Enter Sub Category";
                return;
            }
            mController.InsertHierarchy(Constants.SKUSubCategory, int.Parse(ddSubCategory.SelectedValue), txtSubCategoryCode.Text, txtSubCategoryName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            //  this.SetAutoCode("BN", long.Parse(txtBrandCode.Text.Substring(2)));
            btnSaveSubCategory.Text = "New";
            txtSubCategoryCode.Text = "";
            txtSubCategoryName.Text = "";
            txtSubCategoryCode.Enabled = false;
            txtSubCategoryName.Enabled = false;
            this.LoadSubCategory();
            lblErrorMsgSubCategory.Text = "";
        }
        else if (btnSaveSubCategory.Text == "Update")
        {
            if (txtSubCategoryName.Text.Length == 0)
            {
                lblErrorMsgSubCategory.Text = "Must Enter Sub Category";
                return;
            }
            mController.UpdateHierarchy(Constants.SKUSubCategory, SubCategoryId, int.Parse(ddSubCategory.SelectedValue), txtSubCategoryCode.Text, txtSubCategoryName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            btnSaveSubCategory.Text = "New";
            txtSubCategoryCode.Text = "";
            txtSubCategoryName.Text = "";
            txtSubCategoryCode.Enabled = false;
            txtSubCategoryName.Enabled = false;
            this.LoadSubCategory();
        }
    }

    #endregion

    #region Brand Tab

    /// <summary>
    /// Loads Divisions To Division Combo On Brand Tab
    /// </summary>
    //private void LoadBrandDivision()
    //{
    //    if (DrpBrandPrincipal.Items.Count > 0)
    //    {
    //        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUDivision, Constants.IntNullValue, int.Parse(DrpBrandPrincipal.SelectedValue.ToString()), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
    //        clsWebFormUtil.FillDropDownList(this.DrpBrandDivision, dt, 0, 3, true);
    //    }
    //}

    /// <summary>
    /// Loads Categories To Category Combo And Brands To Brand Grid On Brand Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpBrandDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
      //  this.LoadBrandCategory();
        this.LoadBrand();
    }

    /// <summary>
    /// Loads Categories To Category Combo On Brand Tab
    /// </summary>
    //private void LoadBrandCategory()
    //{
    //    if (DrpBrandDivision.Items.Count > 0)
    //    {
    //        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, int.Parse(DrpBrandDivision.SelectedValue.ToString()), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
    //        clsWebFormUtil.FillDropDownList(this.ddBrandCategory, dt, 0, 3, true);
    //    }
    //}

    /// <summary>
    /// Loads Brands To Brand Grid On Brand Tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddBrandCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadBrand();
    }

    /// <summary>
    /// Loads Brands To Brand Grid On Brand Tab
    /// </summary>
    private void LoadBrand()
    {
       // if (ddBrandCategory.Items.Count > 0)
        {
            DataTable dt = mController.SelectSkuHierarchy(Constants.SKUBrand, Constants.IntNullValue,Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            GrdBrand.EditIndex = -1;
            GrdBrand.DataSource = dt;
           
            GrdBrand.DataBind();
        }
    }

    /// <summary>
    /// Sets Brand Data For Edit. This Function Runs When An Existing Brand Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdBrand_RowEditing(object sender, GridViewEditEventArgs e)
    {
        BrandId = int.Parse(GrdBrand.Rows[e.NewEditIndex].Cells[0].Text);
        txtBrandCode.Text = GrdBrand.Rows[e.NewEditIndex].Cells[1].Text;
        txtBrandName.Text = GrdBrand.Rows[e.NewEditIndex].Cells[2].Text;
        txtBrandName.Enabled = true;
        txtBrandCode.Enabled = true;
        btnSaveBrand.Text = "Update";
        for (int i = 0; i < GrdBrand.Rows.Count; i++)
        {
            GrdBrand.Rows[i].Cells[3].Enabled = false;
            GrdBrand.Rows[i].Cells[4].Enabled = false;
        }
    }

    /// <summary>
    /// Deletes A Brand
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdBrand_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SkuController msku = new SkuController();
        BrandId = int.Parse(GrdBrand.Rows[e.RowIndex].Cells[0].Text);
        DataTable dt = msku.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,Constants.IntNullValue, BrandId, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            lblErrorMsgBrand.Text = "Wrong Command: first delete associated SKUS";
        }
        else
        {
            mController.UpdateHierarchy(Constants.SKUBrand, BrandId, Constants.IntNullValue, null, null, null, false, int.Parse(this.Session["CompanyId"].ToString()));
            this.LoadBrand();
        }
    }

    /// <summary>
    /// Sets PageIndex Of Brand Grid On Brand Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void GrdBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GrdBrand.PageIndex = e.NewPageIndex;
        this.LoadBrand();
    }

    /// <summary>
    /// Save Or Updates a Brand
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveBrand_Click(object sender, EventArgs e)
    {
        lblErrorMsgBrand.Text = "";
        if (btnSaveBrand.Text == "New")
        {
            //txtBrandCode.Text = this.GetAutoCode("BN", 0);
            txtBrandName.Enabled = true;
            txtBrandCode.Enabled = true;
            txtBrandCode.Focus();
            btnSaveBrand.Text = "Save";
            //ScriptManager.GetCurrent(Page).SetFocus(ddBrandCategory);
        }
        else if (btnSaveBrand.Text == "Save")
        {
            if (txtBrandName.Text.Length == 0)
            {
                lblErrorMsgBrand.Text = "Must Enter Brand";
                return;
            }
            mController.InsertHierarchy(Constants.SKUBrand, Constants.IntNullValue, txtBrandCode.Text, txtBrandName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
          //  this.SetAutoCode("BN", long.Parse(txtBrandCode.Text.Substring(2)));
            btnSaveBrand.Text = "New";
            txtBrandCode.Text = "";
            txtBrandName.Text = "";
            txtBrandCode.Enabled = false;
            txtBrandName.Enabled = false;
            this.LoadBrand();
            lblErrorMsgBrand.Text = "";
        }
        else if (btnSaveBrand.Text == "Update")
        {
            if (txtBrandName.Text.Length == 0)
            {
                lblErrorMsgBrand.Text = "Must Enter Brand";
                return;
            }
            mController.UpdateHierarchy(Constants.SKUBrand, BrandId,Constants.IntNullValue, txtBrandCode.Text, txtBrandName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            btnSaveBrand.Text = "New";
            txtBrandCode.Text = "";
            txtBrandName.Text = "";
            txtBrandCode.Enabled = false;
            txtBrandName.Enabled = false;
            this.LoadBrand();
        }
    }

    #endregion

    #region Gender Tab

    //private void LoadTagDivision()
    //{
    //    if (DrpBrandPrincipal.Items.Count > 0)
    //    {
    //        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUDivision, Constants.IntNullValue, int.Parse(DrpBrandPrincipal.SelectedValue.ToString()), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
    //        clsWebFormUtil.FillDropDownList(this.DrpTagDivision, dt, 0, 3, true);
    //    }
    //}

    /// <summary>
    /// Loads Categories To Category Combo And Brands To Brand Grid On Brand Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpTagDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
       // this.LoadBrandCategory();
       // this.LoadBrand();
        LoadTag();
    }

    //private void LoadtagCategory()
    //{
    //    if (DrpBrandDivision.Items.Count > 0)
    //    {
    //        DataTable dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, int.Parse(DrpTagDivision.SelectedValue.ToString()), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
    //        clsWebFormUtil.FillDropDownList(this.DrptagCategory, dt, 0, 3, true);
    //    }
    //}

    protected void drpTagPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
      //  this.LoadTagDivision();
      //  this.LoadtagCategory();
       // this.loadType();
        LoadTag();
    }

    //private void loadType()
    //{
    //    if(DrptagCategory.Items.Count>0){
    //    DataTable dt = mController.SelectSkuHierarchy(Constants.SKUBrand, Constants.IntNullValue, int.Parse(DrptagCategory.SelectedValue.ToString()), null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
    //    clsWebFormUtil.FillDropDownList(this.drpTagType,dt,0,3,true);
    //}
    //}

    //protected void DrpTagDivision_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //this.LoadBrand();
    //}
    protected void DrptagCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
       // this.loadType();
        LoadTag();
    }

    private void LoadTag()
    {
        //if (drpTagType.Items.Count>0)
        {
            DataTable dt = mController.SelectSkuHierarchy(Constants.SKUTAG, Constants.IntNullValue,Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            grdTag.EditIndex = -1;
            grdTag.DataSource = dt;
           
            grdTag.DataBind();
        }
    }

    protected void drpTagType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadTag();
    }
    
    protected void grdTag_RowEditing(object sender, GridViewEditEventArgs e)
    {
        BrandId = int.Parse(grdTag.Rows[e.NewEditIndex].Cells[0].Text);
        txtTagCode.Text = grdTag.Rows[e.NewEditIndex].Cells[1].Text;
        txtTagName.Text = grdTag.Rows[e.NewEditIndex].Cells[2].Text;
        txtTagCode.Enabled = true;
        txtTagName.Enabled = true;
        btnSaveTag.Text = "Update";
        for (int i = 0; i < grdTag.Rows.Count; i++)
        {
            grdTag.Rows[i].Cells[3].Enabled = false;
            grdTag.Rows[i].Cells[4].Enabled = false;
        }
    }

    protected void grdTag_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SkuController msku = new SkuController();
        GenderId = int.Parse(grdTag.Rows[e.RowIndex].Cells[0].Text);
        DataTable dt = msku.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,Constants.IntNullValue, GenderId, Constants.IntNullValue);
        if (dt.Rows.Count > 0)
        {
            lblErrorMsgBrand.Text = "Wrong Command: first delete associated SKUS";
        }
        else
        {
            mController.UpdateHierarchy(Constants.SKUTAG, GenderId, Constants.IntNullValue, null, null, null, false, int.Parse(this.Session["CompanyId"].ToString()));
           // this.loadType();
        }
    }

    /// <summary>
    /// Sets PageIndex Of Brand Grid On Brand Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void grdTag_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.grdTag.PageIndex = e.NewPageIndex;
        this.LoadTag();
    }

    /// <summary>
    /// Save Or Updates a Brand
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveTag_Click(object sender, EventArgs e)
    {
        lblErrorMsgTag.Text = "";
        if (btnSaveTag.Text == "New")
        {
           // txtBrandCode.Text = this.GetAutoCode("BN", 0);
            txtTagCode.Enabled = true;
            txtTagCode.Focus();
            txtTagName.Enabled = true;
            btnSaveTag.Text = "Save";
           // ScriptManager.GetCurrent(Page).SetFocus(drpTagType);
        }
        else if (btnSaveTag.Text == "Save")
        {
            if (txtTagName.Text.Length == 0||txtTagCode.Text.Length==0)
            {
                lblErrorMsgBrand.Text = "Must Enter Tag name and Code";
                return;
            }
            mController.InsertHierarchy(Constants.SKUTAG,Constants.IntNullValue, txtTagCode.Text, txtTagName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            //this.SetAutoCode("BN", long.Parse(txtBrandCode.Text.Substring(2)));
            btnSaveTag.Text = "New";
            txtTagCode.Text = "";
            txtTagName.Text = "";
            txtTagCode.Enabled = false;
            txtTagName.Enabled = false;
            this.LoadTag();
            lblErrorMsgTag.Text = "";
        }
        else if (btnSaveTag.Text == "Update")
        {
            if (txtTagName.Text.Length == 0 || txtTagCode.Text.Length == 0)
            {
                lblErrorMsgBrand.Text = "Must Enter Tag name and Code";
                return;
            }
            mController.UpdateHierarchy(Constants.SKUTAG, GenderId, Constants.IntNullValue, txtTagCode.Text, txtTagName.Text, null, true, int.Parse(this.Session["CompanyId"].ToString()));
            btnSaveTag.Text = "New";
            txtTagCode.Text = "";
            txtTagName.Text = "";
            txtTagCode.Enabled = false;
            txtTagName.Enabled = false;
            this.LoadTag();
            lblErrorMsgTag.Text = "";
        }
    }

    #endregion

    /// <summary>
    /// Gets Code For New Principal, Division, Category And Brand
    /// </summary>
    /// <param name="PreeFix">Prefix</param>
    /// <param name="CodeType">Type</param>
    /// <returns>Code As String</returns>
    private string GetAutoCode(string PreeFix,int CodeType)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        return AutoCode.GetAutoCode(PreeFix, CodeType, Constants.LongNullValue);
    }
    
    /// <summary>
    /// Sets Code For Principal, Division, Category And Brand
    /// </summary>
    /// <param name="PreeFix">Prefix</param>
    /// <param name="CValue">Value</param>
    private void SetAutoCode(string PreeFix, long CValue)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        string result = AutoCode.GetAutoCode(PreeFix, 1, CValue);
    }
    

   
    
    
    
    
    
    
    
}
