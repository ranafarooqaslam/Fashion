using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using ZXing;
using ZXing.Rendering;
using ZXing.QrCode;

public partial class Forms_frmBarcodeBulk : Page
{
    Graphics graphics;
    DataTable _dt = new DataTable();
    readonly SkuController _cn = new SkuController();
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    readonly SkuHierarchyController sController = new SkuHierarchyController();
  
  

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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadCategories();
          
            this.LoadSubCategory();
            this.LoadSKUDetail();
        }
        btnGenerate.Attributes.Add("onclick", "return ValidateForm();");
    }
    private Bitmap Generatecode(string ItemCode)
    {
        BarcodeWriter barcode = new BarcodeWriter();
        barcode.Format = BarcodeFormat.CODE_128;
        barcode.Renderer = new BitmapRenderer() {
            TextFont = new Font("Calibri", 13f, FontStyle.Bold)
        };

        var qrCodeWriter = new ZXing.BarcodeWriterPixelData();
        qrCodeWriter.Options = new QrCodeEncodingOptions {
            Margin  = 0
        };

        var barcodeInBitmap = barcode.Write(ItemCode);

        return barcodeInBitmap;
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSubCategory();
        this.LoadSKUDetail();
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        DataTable dtSKU = Session["dtSKU"] as DataTable;
        DataRow[] foundRows = null;
        _cn.TruncateBarcode();
        foreach (ListItem li in cblCategory.Items)
        {
            if (li.Selected == true)
            {
                foundRows = dtSKU.Select("SKU_ID = '" + li.Value + "'");
                if (foundRows.Length > 0)
                { var oBitmap = Generatecode(foundRows[0]["SKU_CODE"].ToString());
                    var ms = new MemoryStream();
                    oBitmap.Save(ms, ImageFormat.Png);
                    var bytearray = ms.ToArray();
                    var base64Data = Convert.ToBase64String(ms.ToArray());
                    _cn.InsertBarcodeBulk((this.Session["COMPANY_NAME"].ToString()), foundRows[0]["SKU_NAME"].ToString()
                        , Convert.ToString("Rs. " + foundRows[0]["TRADE_PRICE"].ToString()),
                        foundRows[0]["PACKSIZE"].ToString(), foundRows[0]["COLOR"].ToString(), bytearray);
                }
            }
        }
        var sk = new SkuController();
        _dt = sk.SelectSkuBarcode();

        DataTable _dtNew = _dt.Clone();

        foreach (DataRow row in _dt.Rows)
        {
            for (int j = 0; j < int.Parse(txt_row.Text); j++)
            {
                _dtNew.ImportRow(row);
            }
        }
        if (ddlSheet.Value == "1")
        {
            var crpReport = new CrpBarcodeBulk();
            crpReport.SetDataSource(_dtNew);
            crpReport.Refresh();
            crpReport.SetParameterValue("ShowCompany", cbCompany.Checked);
            crpReport.SetParameterValue("ShowName", cbName.Checked);
            crpReport.SetParameterValue("ShowPrice", cbPrice.Checked);
            crpReport.SetParameterValue("ShowSize", cbSize.Checked);
            crpReport.SetParameterValue("ShowColor", cbColor.Checked);
            Session.Add("ReportType", 0);
            Session.Add("CrpReport", crpReport);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            var cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else if (ddlSheet.Value == "2")
        {
            var crpReport = new CrpBarcodeA4Sheet();
            crpReport.SetDataSource(_dtNew);
            crpReport.Refresh();
            crpReport.SetParameterValue("ShowCompany", cbCompany.Checked);
            crpReport.SetParameterValue("ShowName", cbName.Checked);
            crpReport.SetParameterValue("ShowPrice", cbPrice.Checked);
            crpReport.SetParameterValue("ShowSize", cbSize.Checked);
            crpReport.SetParameterValue("ShowColor", cbColor.Checked);
            Session.Add("ReportType", 0);
            Session.Add("CrpReport", crpReport);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            var cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else if (ddlSheet.Value == "3")
        {
            var crpReport = new CrpBarcodeSingleStickerBarcodePrinter();
            crpReport.SetDataSource(_dtNew);
            crpReport.Refresh();
            crpReport.SetParameterValue("ShowCompany", cbCompany.Checked);
            crpReport.SetParameterValue("ShowName", cbName.Checked);
            crpReport.SetParameterValue("ShowPrice", cbPrice.Checked);
            crpReport.SetParameterValue("ShowSize", cbSize.Checked);
            crpReport.SetParameterValue("ShowColor", cbColor.Checked);
            Session.Add("ReportType", 0);
            Session.Add("CrpReport", crpReport);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            var cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }

    protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadSKUDetail();
    }
}