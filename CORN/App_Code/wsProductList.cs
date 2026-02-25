using System.Collections.Generic;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System;

/// <summary>
/// Summary description for wsProductList
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class wsProductList : System.Web.Services.WebService
{

    public wsProductList()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetProducts(string prefixText)
    {
        List<string> Products = new List<string>();
        SkuController mSKUController = new SkuController();
        DataTable Dtsku_Price = mSKUController.SearchProduct(prefixText);

        for (int i = 0; i < Dtsku_Price.Rows.Count; i++)
        {
            Products.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Dtsku_Price.Rows[i]["SKU_NAME"].ToString(), Dtsku_Price.Rows[i]["SKU_CODE"].ToString()));
        }

        return Products.ToArray();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetProducts2(string prefixText)
    {
        List<string> products = new List<string>();
        SkuController mSkuController = new SkuController();
        
        DataTable dtskuPrice = mSkuController.SearchProduct(prefixText);
       
        for (int i = 0; i < dtskuPrice.Rows.Count; i++)
        {
            products.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dtskuPrice.Rows[i]["SKUDETAIL"].ToString(), dtskuPrice.Rows[i]["SKU_ID"].ToString()));
        }

        return products.ToArray();
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetPosProducts(string prefixText,int count, string contextKey)
   {
        var products = new List<string>();
        var mSkuController = new SkuController();
        int distributorId = Constants.IntNullValue;
        if (contextKey != null)
        {
            distributorId = Convert.ToInt32(contextKey);
        }
        DataTable dtskuPrice = mSkuController.SearchProduct(prefixText, distributorId);

        for (int i = 0; i < dtskuPrice.Rows.Count; i++)
        {
            products.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dtskuPrice.Rows[i]["SKUDETAIL2"].ToString(), dtskuPrice.Rows[i]["SKU_ID"].ToString()));
        }

        return products.ToArray();
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetPosProductsWithSKU_ID(string prefixText, int count, string contextKey)
    {
        var products = new List<string>();
        var mSkuController = new SkuController();
        int distributorId = Constants.IntNullValue;
        if (contextKey != null)
        {
            distributorId = Convert.ToInt32(contextKey);
        }
        DataTable dtskuPrice = mSkuController.SearchProduct(prefixText, distributorId);

        for (int i = 0; i < dtskuPrice.Rows.Count; i++)
        {
            products.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dtskuPrice.Rows[i]["SKUDETAIL2"].ToString(), dtskuPrice.Rows[i]["SKU_ID1"].ToString()));
        }

        return products.ToArray();
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetBarcodeProducts(string prefixText)
    {
        List<string> products = new List<string>();
        SkuController mSkuController = new SkuController();
        DataTable dtskuPrice = mSkuController.SearchProduct(prefixText);

        for (int i = 0; i < dtskuPrice.Rows.Count; i++)
        {
            products.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dtskuPrice.Rows[i]["SKUDETAIL2"].ToString(), dtskuPrice.Rows[i]["SKU_ID"].ToString()));
        }

        return products.ToArray();
    }
    
}
