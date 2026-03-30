using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using CORNBusinessLayer.Reports;

public partial class Forms_frmPysicalStockteken : System.Web.UI.Page
{
    DataControl dc = new DataControl();
    readonly General general = new General();
    PhaysicalStockController MController = new PhaysicalStockController();
    private static int RowNo;
    private static int decmalPlaces = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            this.LoadSKUDetail();
            CreatTable();
            this.LoadGird();
            GetDocumentNo();
            LoadMAXDOC_NO();
            DataTable dtAppSetting = (DataTable)Session["dtAppSetting"];
            if (dtAppSetting != null)
            {
                DataRow[] drAppSetting = dtAppSetting.Select("strColumnName='QtyDecPlaces'");
                if (drAppSetting.Length > 0)
                {
                    decmalPlaces = Convert.ToInt32(dc.chkNull_0(drAppSetting[0]["strColumnValue"].ToString()));
                }
            }

            txtQuantity.Text = "1";
            txtskuCode.Focus();
            txtStartDate.Attributes.Add("readonly", "readonly");
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
                        txtStartDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
                        break;
                    }
                }
            }
        }
    }
    private void CreatTable()
    {
        DataTable PurchaseSKUS;
        PurchaseSKUS = new DataTable();
        PurchaseSKUS.Columns.Add("PHYSICAL_STOCK_ID", typeof(long));
        PurchaseSKUS.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKUS.Columns.Add("SKU_Code", typeof(string));
        PurchaseSKUS.Columns.Add("SKU_Name", typeof(string));
        PurchaseSKUS.Columns.Add("UNIT_RATE", typeof(decimal));
        PurchaseSKUS.Columns.Add("SALEABLE_QUANTITY", typeof(decimal));
        PurchaseSKUS.Columns.Add("PACKSIZE", typeof(string));
        PurchaseSKUS.Columns.Add("COLOR", typeof(string));
        Session.Add("PhysicalStock", PurchaseSKUS);

    }
    private void LoadMAXDOC_NO()
    {
        DataTable dt = MController.SelectMaxDocNo();
        if (dt.Rows.Count > 0)
        {
            hfMaxDOCID.Value = dt.Rows[0]["DOC_NO"].ToString();
        }
        else
        {
            hfMaxDOCID.Value = Constants.LongNullValue.ToString();
        }
    }
    private void GetDocumentNo()
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
                    break;
                }
            }
        }
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            drpDocumentNo.Items.Clear();
            DataTable dt = MController.SelectPysicalStock(int.Parse(drpDistributor.SelectedValue.ToString()),
                0, Constants.IntNullValue, CurrentWorkDate, Constants.LongNullValue);

            var uniqueDocNo = dt.AsEnumerable().Select(s => new {
                PHYSICAL_STOCK_ID = s.Field<long>("PHYSICAL_STOCK_ID"),
            }).Distinct().ToList().ToDataTable();

            drpDocumentNo.Items.Add(new ListItem("New", Constants.LongNullValue.ToString()));

            if (uniqueDocNo.Rows.Count > 0)
            {
                clsWebFormUtil.FillDropDownList(drpDocumentNo, uniqueDocNo, 0, 0, false);
            }
            drpDocumentNo.SelectedIndex = 0;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }
    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);

        AutoComplete.ContextKey = drpDistributor.SelectedValue.ToString();

        Session.Add("dtLocationInfo", dt);
    }

    /// <summary>
    /// Loads SKU Detail To ListBox
    /// </summary>
    private void LoadSKUDetail()
    {
       
            SKUPriceDetailController PController = new SKUPriceDetailController();
            DataTable Dtsku_Price = PController.SelectDataPrice2(Constants.IntNullValue, Constants.IntNullValue, 
                Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), 
                int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 7,
                DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));

            clsWebFormUtil.FillListBox(this.lstCode, Dtsku_Price, "SKU_DETAIL2", "SKU_DETAIL2", true);
            clsWebFormUtil.FillDropDownList(this.drpSkus, Dtsku_Price, "SKU_ID", "SKU_DETAIL2", true);  

        this.Session.Add("Dtsku_Price", Dtsku_Price);
   
       
    }
    [WebMethod]
    [ScriptMethod]
    public static string GetSKUDetail(string itemID)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

        DataTable dtSkus = (DataTable)HttpContext.Current.Session["Dtsku_Price"];
        DataRow[] foundRows = dtSkus.Select("SKU_ID  = '" + itemID + "'");

        if (foundRows.Length > 0)
        {
            DataTable newDt = foundRows.CopyToDataTable();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in newDt.Rows)
            {
                row = new Dictionary<string, object>();

                foreach (DataColumn col in newDt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
        }

        return serializer.Serialize(rows);
    }

    /// <summary>
    ///  Loads Document Detail To Document Detail Grid
    /// </summary>
    private void LoadGird()
    {
        DataTable PurchaseSKUS = (DataTable)Session["PhysicalStock"];

        GrdPurchase.DataSource = PurchaseSKUS;
        GrdPurchase.DataBind();
    }
    
    /// <summary>
    /// Saves/Updates Document
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EvemtArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
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
                        break;
                    }
                }
            }

            if (CurrentWorkDate < DateTime.Parse(txtStartDate.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Date Cannot be greater than Location working Date');", true);
                return;
            }

            DataTable PurchaseSKU = (DataTable)this.Session["PhysicalStock"];
            string success = "false";
            var recordId = long.Parse(hfMaxDOCID.Value);
            if (PurchaseSKU.Rows.Count > 0)
            {
                if (btnSave.Text == "Save")
                {
                    success = MController.InsertPysicalStock(int.Parse(drpDistributor.SelectedValue.ToString()),
                        DateTime.Parse(txtStartDate.Text), 0, 0, long.Parse(hfMaxDOCID.Value),
                        PurchaseSKU);
                }
                else
                {
                    MController.UpdatePysicalStock(int.Parse(drpDistributor.SelectedValue.ToString()),
                        DateTime.Parse(txtStartDate.Text), 0, 0,
                        PurchaseSKU);
                }

                DataTable tempTable = PurchaseSKU;
                CreatTable();
                this.LoadGird();
                this.ClearAll();
                LoadMAXDOC_NO();
                GetDocumentNo();

                ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record added Successfully');", true);

                if (success == "true")
                {
                    ShowReport(DateTime.Parse(txtStartDate.Text), recordId, tempTable);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please add some details in Grid');", true);
                return;
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Clears Form Controls
    /// </summary>
    private void ClearAll()
    {
        txtskuCode.Text = "";
        txtskuName.Text = "";
        txtQuantity.Text = "1";
        txtcolor.Text = "";
        txtsize.Text = "0";
        txtskuCode.Enabled = true;
        btnSave.Text = "Save";
    }    

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable PurchaseSKUS = (DataTable)Session["PhysicalStock"];
        if (drpDocumentNo.SelectedValue != Constants.LongNullValue.ToString())
        {
            if (MController.DELETEPysicalStock(Convert.ToInt32(drpDistributor.SelectedValue),
                Convert.ToDateTime(Session["CurrentWorkDate"]),
                Convert.ToInt32(GrdPurchase.Rows[e.RowIndex].Cells[0].Text),
                Convert.ToInt64(drpDocumentNo.SelectedValue)))
            {

            }
        }

        PurchaseSKUS.Rows.RemoveAt(e.RowIndex);
        Session.Add("PhysicalStock", PurchaseSKUS);
        GrdPurchase.DataSource = PurchaseSKUS;
        GrdPurchase.DataBind();
    }

    protected void GrdPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.DataItem != null)
            {
                e.Row.Cells[5].Text = general.DecimalValue(Convert.ToDecimal(dc.chkNull_0(e.Row.Cells[5].Text)), decmalPlaces);
            }
        }
    }

    protected void drpSkus_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtcolor.Text = "";
        txtsize.Text = "";
        txtUnitRate.Text = "";

        DataTable dtSkus = (DataTable)HttpContext.Current.Session["Dtsku_Price"];
        DataRow[] foundRows = dtSkus.Select("SKU_ID  = '" + drpSkus.SelectedValue + "'");

        if (foundRows.Length > 0)
        {
            txtcolor.Text = foundRows[0]["COLOR"].ToString();
            txtsize.Text = foundRows[0]["PACKSIZE"].ToString();
            txtUnitRate.Text = foundRows[0]["DISTRIBUTOR_PRICE"].ToString();
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        AutoComplete.ContextKey = drpDistributor.SelectedValue.ToString();
        LoadSKUDetail();
        ClearAll();
        CreatTable();
        LoadGird();

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
                    txtStartDate.Text = CurrentWorkDate.ToString("dd-MMM-yyyy");
                    break;
                }
            }
        }
    }

    protected void chkScan_CheckedChanged(object sender, EventArgs e)
    {
        txtcolor.Text = "";
        txtsize.Text = "";
        txtUnitRate.Text = "";

        if (chkScan.Checked == true)
        {
            txtskuCode.Visible = true;
            drpSkus.Visible = false;
        }
        else
        {
            txtskuCode.Text = "";
            txtskuCode.Visible = false;
            drpSkus.Visible = true;
            drpSkus_SelectedIndexChanged(null, null);
        }
    }
    protected void btnAddRecord_Click(object sender, EventArgs e)
    {
        DataTable PurchaseSKUS = (DataTable)Session["PhysicalStock"];
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        DataRow[] foundRows = null;

        if (string.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) <= 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Qty');", true);
            return;
        }

        if (chkScan.Checked)
        {
            if (string.IsNullOrEmpty(txtskuCode.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter Item Code');", true);
                return;
            }

            foundRows = PurchaseSKUS.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        }
        else
        {
            if (drpSkus.SelectedIndex == -1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Item');", true);
                return;
            }

            foundRows = PurchaseSKUS.Select("SKU_ID  = '" + drpSkus.SelectedValue + "'");
        }
        if (foundRows.Length > 0)
        {
            foundRows[0]["SALEABLE_QUANTITY"] = decimal.Parse(foundRows[0]["SALEABLE_QUANTITY"].ToString())
                + decimal.Parse(dc.chkNull_0(txtQuantity.Text));
        }
        else
        {

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
                DataRow dr = PurchaseSKUS.NewRow();
                dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                dr["PHYSICAL_STOCK_ID"] = 0;
                dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                dr["COLOR"] = foundRows[0]["COLOR"];
                dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                dr["SALEABLE_QUANTITY"] = decimal.Parse(dc.chkNull_0(txtQuantity.Text));
                dr["UNIT_RATE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                PurchaseSKUS.Rows.Add(dr);
            }
        }
        Session.Add("PhysicalStock", PurchaseSKUS);
        ClearAll();
        GrdPurchase.DataSource = PurchaseSKUS;
        GrdPurchase.DataBind();
        txtskuCode.Focus();
    }
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDocumentNo.SelectedValue.ToString() == Constants.LongNullValue.ToString())
        {
            CreatTable();
            LoadGird();
            drpDistributor.Enabled = true;
            ClearAll();
            LoadMAXDOC_NO();
        }
        else
        {
            drpDistributor.Enabled = false;
            LoadDocumentDetail();
        }
    }
    protected void LoadDocumentDetail()
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
                    break;
                }
            }
        }

        CreatTable();
        DataTable PurchaseSKUS = (DataTable)Session["PhysicalStock"];
        PhaysicalStockController MController = new PhaysicalStockController();
        DataTable dt = MController.SelectPysicalStock(int.Parse(drpDistributor.SelectedValue.ToString()),
            0, Constants.IntNullValue, CurrentWorkDate, long.Parse(drpDocumentNo.SelectedValue));

        hfMaxDOCID.Value = drpDocumentNo.SelectedValue;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = PurchaseSKUS.NewRow();
                dr["SKU_ID"] = item["SKU_ID"];
                dr["PHYSICAL_STOCK_ID"] = item["PHYSICAL_STOCK_ID"];
                dr["SKU_Code"] = item["SKU_CODE"];
                dr["SKU_Name"] = item["SKU_NAME"];
                dr["COLOR"] = item["COLOR"];
                dr["PACKSIZE"] = item["PACKSIZE"];
                dr["SALEABLE_QUANTITY"] = item["SALEABLE_QUANTITY"];
                dr["UNIT_RATE"] = item["UNIT_RATE"];
                PurchaseSKUS.Rows.Add(dr);
            }
        }

        Session.Add("PhysicalStock", PurchaseSKUS);
        LoadGird();
    }
    private void ShowReport(DateTime CurrentWorkDate, long recordId, DataTable dtPurchaseDetail)
    {
        try
        {
            DsReport ds = new DsReport();
            DataTable dt = new DataTable();
            dt.Columns.Add("SKU_CODE", typeof(string));
            dt.Columns.Add("SKU_NAME", typeof(string));
            dt.Columns.Add("SALE QUANTITY", typeof(int));
            dt.Columns.Add("STOCK_DATE", typeof(DateTime));
            dt.Columns.Add("TRADE_PRICE", typeof(decimal));
            dt.Columns.Add("DOC_NO", typeof(long));
            dt.Columns.Add("UNSALE QUANTITY", typeof(int));
            dt.Columns.Add("QUANTITY", typeof(int));
            dt.Columns.Add("Difference", typeof(int));
            dt.Columns.Add("Remarks", typeof(string));
            int i = 0;
            foreach (DataRow dr in dtPurchaseDetail.Rows)
            {
                dt.Rows.Add();
                dt.Rows[i]["SKU_CODE"] = dr["SKU_CODE"];
                dt.Rows[i]["SKU_NAME"] = dr["SKU_NAME"];
                dt.Rows[i]["SALE QUANTITY"] = Convert.ToDecimal(dr["SALEABLE_QUANTITY"]);
                dt.Rows[i]["STOCK_DATE"] = CurrentWorkDate;
                dt.Rows[i]["TRADE_PRICE"] = Convert.ToDecimal(dr["UNIT_RATE"]);
                dt.Rows[i]["DOC_NO"] = recordId;
                dt.Rows[i]["UNSALE QUANTITY"] = 0;
                dt.Rows[i]["QUANTITY"] = 0;
                dt.Rows[i]["Difference"] = 0;
                dt.Rows[i]["Remarks"] = "";
                i += 1;
            }
            foreach (DataRow dr in dt.Rows)
            {
                ds.Tables["PhysicalStockTaking"].ImportRow(dr);
            }
            var crpReport = new CrpPhysicalStockTakingNew();
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Location", drpDistributor.SelectedItem.Text);
            crpReport.SetParameterValue("user", this.Session["UserName2"].ToString());
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
}