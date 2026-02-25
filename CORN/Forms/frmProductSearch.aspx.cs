using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

public partial class Forms_frmProductSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        loadSkuDetail();
        txtSearch.Focus();
    }
    protected void table_example_PreRender(object sender, EventArgs e)
    {
        //if (GrdPurchase.Rows.Count > 0)
        //{
        //    GrdPurchase.UseAccessibleHeader = true;
        //    GrdPurchase.HeaderRow.TableSection = TableRowSection.TableHeader;
        //}
    }
    private void loadSkuDetail()
     {
         int t = int.Parse(this.Session["DISTRIBUTOR_ID"].ToString());
         SKUPriceDetailController PController = new SKUPriceDetailController();
         DataTable Dtsku_Price = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()),
         int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
              
         GrdPurchase.DataSource = Dtsku_Price;
         this.Session.Add("skuDetail",Dtsku_Price);
         GrdPurchase.DataBind();
     }
    protected  void selectValuandGOtoPos(object sender, GridViewRowEventArgs e)
     {
         if (e.Row.RowType == DataControlRowType.DataRow)
         {
             // javascript function to call on row-click event
             string st = e.Row.Cells[0].Text;
             
             e.Row.Attributes.Add("onClick", "javascript:void SelectRow("+ st +");");
         }

         //if (e.Row.RowType == DataControlRowType.DataRow)
         //{

         //    string queryString = string.Empty;



         //    for (int x = 0; x < GrdPurchase.Columns.Count; x++)
         //    {

         //        string separator = (x == 0 ? "?" : "&");

         //        queryString += string.Format("{0}{1}={2}", separator, GrdPurchase.Columns[x].HeaderText, e.Row.Cells[x].Text);

         //    }



         //    e.Row.Attributes["ondblclick"] = string.Format("popItUp({0})", queryString);

         //}

     }
    protected void btnrefresh_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmOrderPOS.aspx?skuid=-1");
    }
}