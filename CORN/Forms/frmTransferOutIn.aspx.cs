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
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// From To Transfer In Stock
/// </summary>
public partial class Forms_frmTransferOutIn : System.Web.UI.Page
{
    private static int RowNo;
    DataTable PurchaseSKUS;

    private void CreateTable()
    {
        PurchaseSKUS = new DataTable();
        PurchaseSKUS.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKUS.Columns.Add("SKU_Code", typeof(string));
        PurchaseSKUS.Columns.Add("SKU_Name", typeof(string));
        PurchaseSKUS.Columns.Add("Quantity", typeof(int));
        this.Session.Add("PurchaseSKUS", PurchaseSKUS);
    }
    /// <summary>
    /// Page_Load Function Populates All Combos And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            this.GetDocumentNo();
            this.LoadToDistributor();
           
            this.LoadDocumentDetail();
            txtBuiltyNo.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Gets Document Nos
    /// </summary>
    private void GetDocumentNo()
    {
        drpDocumentNo.Items.Clear();
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelecttransferOutDocuments(55,
            Constants.IntNullValue, Constants.LongNullValue, int.Parse(this.Session["UserId"].ToString()), 
            Constants.IntNullValue,Constants.IntNullValue, Convert.ToInt16(0));

        clsWebFormUtil.FillDropDownList(this.drpDocumentNo, dt, 0, 0);
    }

    /// <summary>
    /// Loads Document Detail Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadDocumentDetail();
    }

    /// <summary>
    ///  Loads Document Detail Grid
    /// </summary>
    private void LoadDocumentDetail()
    {
        if (drpDocumentNo.Items.Count > 0)
        {
            PurchaseController mPurchase = new PurchaseController();
            DataTable dt = mPurchase.SelectPurchaseDocumentNo(55, 
                Constants.IntNullValue, long.Parse(drpDocumentNo.SelectedValue.ToString()),
                int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue);

            if (dt.Rows.Count > 0)
            {
                txtDocumentNo.Text = dt.Rows[0]["ORDER_NUMBER"].ToString();
                txtBuiltyNo.Text = dt.Rows[0]["BUILTY_NO"].ToString();
               
                DrpTransferFor.SelectedValue = dt.Rows[0]["SOLD_FROM"].ToString();
                drpDistributor.SelectedValue = dt.Rows[0]["SOLD_TO"].ToString();
                DataTable PurchaseSKUS = mPurchase.SelectPurchaseDetail(Constants.IntNullValue, long.Parse(dt.Rows[0][0].ToString()));
                GrdPurchase.DataSource = PurchaseSKUS;
                GrdPurchase.DataBind();
                this.Session.Add("PurchaseSKUS", PurchaseSKUS);
            }
        }
    }

    /// <summary>
    /// Loads Locations To Location From Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }

    /// <summary>
    /// Loads Locations To Location To Combo
    /// </summary>
    private void LoadToDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpTransferFor, dt, 0, 2, true);
    }
        
    
    /// <summary>
    /// Saves Document
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnTransferIn_Click(object sender, EventArgs e)
    {
        PurchaseController mController = new PurchaseController();
        DataTable dtPurchaseDetail = (DataTable)this.Session["PurchaseSKUS"];
        decimal mTotalAmount = 0;

        foreach (DataRow dr in dtPurchaseDetail.Rows)
        {
            mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());

        }

        bool mResult = mController.InsertTransferInDocument(long.Parse(drpDocumentNo.SelectedValue.ToString()),int.Parse(drpDistributor.SelectedValue.ToString()), txtDocumentNo.Text, Constants.Document_Transfer_In,
               DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpTransferFor.SelectedValue.ToString())
              , mTotalAmount, false, dtPurchaseDetail, 1, txtBuiltyNo.Text, int.Parse(this.Session["UserId"].ToString()), 0);

        if (mResult == true)
        {
            Session.Remove("PurchaseSKUS");
            GetDocumentNo();
            LoadDocumentDetail();
            txtBuiltyNo.Text = "";
            txtDocumentNo.Text = "";
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Transfer In saved successfully.');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured.');", true);
        }
    }

    private void LoadGird()
    {
        //int TotalValue = 0;
        PurchaseSKUS = (DataTable)this.Session["PurchaseSKUS"];
        GrdPurchase.DataSource = PurchaseSKUS;
        GrdPurchase.DataBind();
        //foreach (DataRow dr in PurchaseSKUS.Rows)
        //{
        //    TotalValue += int.Parse(dr["Quantity"].ToString());

        //}
        //txtTotalQuantity.Text = TotalValue.ToString();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
      //  Respo
    }
}
