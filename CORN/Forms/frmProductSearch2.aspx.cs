using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

public partial class Forms_frmProductSearch2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtSearch.Focus();
    }
   
    private void loadSkuDetail()
    {

        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable Dtsku_Price = PController.SelectAllSKUS(txtSearch.Text.ToString(),0);
        if (Dtsku_Price.Rows.Count > 0)
        {
            GrdPurchase.DataSource = Dtsku_Price;
            // this.Session.Add("skuDetail", Dtsku_Price);
            GrdPurchase.DataBind();
            lblNoRecords.Visible = false;
        }
        else
        {
            lblNoRecords.Visible = true;
        }
    }
   
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        loadSkuDetail();
    }
}