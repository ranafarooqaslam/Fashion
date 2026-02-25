using System;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.HtmlControls;
using CORNBusinessLayer.Reports;
//using OnBarcode.Barcode;
using OnBarcode.Barcode;
using ZXing;
using ZXing.Rendering;
using ZXing.QrCode;

public partial class Forms_frmBarcode : Page
{
    BarcodeLib.Barcode b = new BarcodeLib.Barcode();
    readonly SKUPriceDetailController PController = new SKUPriceDetailController();
    Graphics graphics;
    DataTable _dt = new DataTable();
    readonly SkuController _cn = new SkuController();       
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadSKUDetail();
        }
        btn_generate.Attributes.Add("onclick", "return ValidateForm2();");
    }
    private void LoadSKUDetail()
    {
        DataTable Dtsku_Price = PController.SelectDataPrice2(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 5, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(ddlSKU, Dtsku_Price, "SKU_ID", "SKU_DETAIL", true);
        Session.Add("Dtsku_Price", Dtsku_Price);
        ddlSKU_SelectedIndexChanged(null, null);
    }
    protected void btn_generate_Click(object sender, EventArgs e)
    {

        var oBitmap = Generatecode();
        lbl_color.Text = txtColor.Text;
        lbl_pcode.Text = txt_productName.Text + " ("+ txtSize.Text +")";
        lbl_pprice.Text =txt_price.Text;

        var ms = new MemoryStream();
        oBitmap.Save(ms, ImageFormat.Png);

        var bytearray = ms.ToArray();
        var base64Data= Convert.ToBase64String(ms.ToArray());

        img_brcode.ImageUrl = "data:image/Jpeg;base64," + base64Data; 

        Session["base64Data"] = base64Data;

        if (Session["base64Data"] != null)
        {
            Panel1.Visible = true;
            img_brcode.Visible = true;
        }

        _cn.TruncateBarcode();

        _cn.InsertBarcode((this.Session["COMPANY_NAME"].ToString()),
            Convert.ToString(txt_productName.Text), Convert.ToString("Rs. "+ txt_price.Text),
            txtSize.Text, txtColor.Text, bytearray);

        var sk = new SkuController();
        _dt = sk.SelectSkuBarcode();
        for (int i = 1; i < int.Parse(txt_row.Text); i++)
        {
            var dr = _dt.Rows[0];
            _dt.ImportRow(dr);
        }
        if (ddlSheet.Value == "1")
        {
            var crpReport = new CrystalDecisions.CrystalReports.Engine.ReportClass();
            if (ddlStickerSize.Value == "2")
            {
                crpReport = new CrpBarcodeSmall();
            }
            else
            {
                crpReport = new CrpBarcode();
            }
            crpReport.SetDataSource(_dt);
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
            crpReport.SetDataSource(_dt);
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
            var crpReport = new CrystalDecisions.CrystalReports.Engine.ReportClass();
            if(ddlStickerSize.Value=="2")
            {
                crpReport = new CrpBarcodeSingleStickerBarcodePrinterSmall();
            }
            else
            {
                crpReport = new CrpBarcodeSingleStickerBarcodePrinter();
            }
            crpReport.SetDataSource(_dt);
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
    protected void btn_print_Click(object sender, EventArgs e)
    {
       
        if ((txt_row.Text != null) && (txt_col.Text != null))
        {
            int tblRows = Convert.ToInt32(txt_row.Text);
            int tblCols = Convert.ToInt32(txt_col.Text);
           
            Unit width = new Unit(500, UnitType.Pixel);

            Table tbl = new Table();
            tbl.ID = "tcResource"; 
            tbl.Width = width;
            tbl.Height = Unit.Pixel(1200);
            tbl.BorderStyle = BorderStyle.Double;
            tbl.BorderWidth = 2;
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;

            tbl.HorizontalAlign = HorizontalAlign.Center;
            

            for (int i = 0; i < tblRows; i++)
            {
                TableRow tr = new TableRow();
                tr.HorizontalAlign = HorizontalAlign.Center;
                for (int j = 0; j < tblCols; j++)
                {
                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                    Label lblpname = new Label();
                    
                    lblpname.Font.Size = new FontUnit(10);
                    Label lblprice = new Label();
                    
                    lblprice.Font.Size = new FontUnit(10);
                    Label lblCompany = new Label();

                    TableCell tc = new TableCell();
                  
                    tc.Width = Unit.Pixel(125);
                    tc.HorizontalAlign = HorizontalAlign.Center;
                    tc.BorderStyle = BorderStyle.Double;
                    tc.BorderWidth = 2;

                    Panel dpnl1 = new Panel();
                    Panel dpnl2 = new Panel();
                    Panel dpnl4 = new Panel();

                    lblCompany.Text = "need";
                    lblpname.Text = txt_productName.Text + " " + " " +"Rs." + " " + txt_price.Text;
                    
                    
                    var data = Session["base64Data"];
                    img.ImageUrl = "data:image/Jpeg;base64," + data;
                    img.Height = Unit.Pixel(60);
                    img.Width = Unit.Pixel(300);
                    // Add the control to the TableCell

                    dpnl1.Controls.Add(lblCompany);
                    dpnl2.Controls.Add(lblpname);
                    //dpnl3.Controls.Add(lblprice);
                    dpnl4.Controls.Add(img);

                    tc.Controls.Add(dpnl1);
                    tc.Controls.Add(dpnl2);
                    //tc.Controls.Add(dpnl3);
                    tc.Controls.Add(dpnl4);
                    tr.Cells.Add(tc);
                }
                // Add the TableRow to the Table
                tbl.Rows.Add(tr);
                // Panel1.Controls.Add(tbl);
                Session["ctrl"] = tbl;
            }
        }
        Control ctrl = (Control)Session["ctrl"];
      //  PrintHelper.PrintWebControl(ctrl);
        ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");
        //string Script;
        StringWriter stringWrite = new StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
        if (ctrl is WebControl)
        {
            Unit w = new Unit(40, UnitType.Percentage);
            ((WebControl)ctrl).Width = w;
        }
        Page pg = new Page();
        pg.EnableEventValidation = false;
        HtmlForm frm = new HtmlForm();
        
        pg.Controls.Add(frm);
        
        frm.Attributes.Add("runat", "server");
        frm.Controls.Add(ctrl);
        pg.DesignerInitialize();
        pg.RenderControl(htmlWrite);
        string strHTML = stringWrite.ToString();
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Write(strHTML);
        //HttpContext.Current.Response.Write("<script>window.print();</script>");
        HttpContext.Current.Response.End();

       
    }
    // private Bitmap Generatecode()
    // {
    // // BarcodeLib.Barcode.Linear barcode = new BarcodeLib.Barcode.Linear();
    //  //barcode.Type = BarcodeType.CODE128;

    //  Linear barcode = new Linear();
    //  barcode.Type = BarcodeType.CODE128;
    //  barcode.Data = txt_code.Text;
    //     barcode.TextFont = new Font("Arial", 11f, FontStyle.Bold);


    //  barcode.LeftMargin = 0;
    //  barcode.RightMargin = 0;
    //  barcode.TopMargin = 0;
    //  barcode.BottomMargin = 0;

    //  barcode.Resolution = 72;      
    ////  barcode.Rotate = RotateOrientation.BottomFacingDown;

    //  barcode.UOM = UnitOfMeasure.PIXEL;


    //  byte[] barcodeInBytes = barcode.drawBarcodeAsBytes();

    //  // generate barcode to <strong>Graphics object<strong>
    //  string code = txt_code.Text;
    //     int w = code.Length * 10;
    //     var oBitmap = new Bitmap(w, 100);
    //  Graphics graphics = Graphics.FromImage(oBitmap); 
    //  barcode.drawBarcode(graphics);



    //  // generate barcode and output to Bitmap object
    //    Bitmap barcodeInBitmap = barcode.drawBarcode();
    //    return barcodeInBitmap;

    // }
    private Bitmap Generatecode()
    {
        BarcodeWriter barcode = new BarcodeWriter();
        barcode.Format = BarcodeFormat.CODE_128;
        barcode.Renderer = new BitmapRenderer()
        {
            TextFont = new Font("Calibri", 13f, FontStyle.Bold)
        };

        var qrCodeWriter = new BarcodeWriterPixelData();
        qrCodeWriter.Options = new QrCodeEncodingOptions
        {
            Margin = 0
        };

        var barcodeInBitmap = barcode.Write(txt_code.Text);

        return barcodeInBitmap;
    }
    protected void ddlSKU_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];
        foreach(DataRow dr in Dtsku_Price.Rows)
        {
            if(dr["SKU_ID"].ToString() == ddlSKU.SelectedValue)
            {
                txt_price.Text = Convert.ToDecimal(dr["TRADE_PRICE"]).ToString("0.00");
                txt_code.Text = dr["SKU_CODE"].ToString();
                txt_productName.Text = dr["SKU_NAME"].ToString();
                txtColor.Text = dr["COLOR"].ToString();
                txtSize.Text = dr["PACKSIZE"].ToString();
                break;
            }
        }
    }
}