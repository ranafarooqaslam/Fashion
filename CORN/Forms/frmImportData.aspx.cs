using System;
using System.Data;
using System.IO;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using OfficeOpenXml;
using System.Web;
using System.Linq;

/// <summary>
/// Form To Import Route, Market,  Customer, SKU And SKU Price Data From Text Files
/// </summary>
public partial class Forms_frmImportData : System.Web.UI.Page
{
    CustomerDataController mCustomer = new CustomerDataController();
    DataControl dc = new DataControl();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadPrincipal();
           
        }
    }
   
    private void LoadDistributor()
    {
        DistributorController mController = new DistributorController();
        DataTable dt = mController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(DrpDistributor, dt, 0, 2, true);

        Session.Add("dtLocationInfo", dt);
    }

    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1, true);
    }
        
    /// <summary>
    /// Imports File Data To Database
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region FileValidation
        if (txtFile.PostedFile.ContentLength == 0)
        {
            lblErrorMessage.Text = "Please select a file and then upload";
            return;
        }
        else if (cboFileTypes.SelectedValue == "6" && txtFile.PostedFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            lblErrorMessage.Text = "Only excel file is supported";
            return;
        }
        else if (txtFile.PostedFile.ContentType != "text/plain" && cboFileTypes.SelectedValue != "6")
        {
            lblErrorMessage.Text = "Only text file are supported";
            return;
        }
        
        #endregion
        bool flag=true;
        try
        {
            if (cboFileTypes.SelectedValue != "6")
            {
                if (!Directory.Exists(Constants.fldOtherDataFolder))
                {
                    Directory.CreateDirectory(Constants.fldOtherDataFolder);
                }

                string path = System.IO.Path.GetFullPath(txtFile.PostedFile.FileName);
                string filename = path.Substring(path.LastIndexOf('\\'), path.Length - path.LastIndexOf('\\'));
                if (File.Exists(Constants.fldOtherDataFolder + filename))
                {
                    lblErrorMessage.Text = "File already Exist in folder. Save file with other name";
                    return;
                }
                else
                {
                    txtFile.PostedFile.SaveAs(Constants.fldOtherDataFolder + filename);
                    path = Constants.fldOtherDataFolder + filename;
                    int index = cboFileTypes.SelectedIndex;
                    this.lblErrorMessage.Text = "";

                    if (cboFileTypes.SelectedValue == "3")//For Item
                    {
                        SkuController SKUCtl = new SkuController();
                        flag = SKUCtl.ImportSKUS(int.Parse(DrpDistributor.SelectedValue.ToString()), path, int.Parse(DrpPrincipal.SelectedValue.ToString()), int.Parse(this.Session["CompanyId"].ToString()), Convert.ToInt32(Session["UserID"]));
                        if (!flag)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occurred.');", true);
                            return;
                        }
                    }
                    else if (cboFileTypes.SelectedValue == "4")// For Item Price
                    {
                        SKUPriceDetailController SKUPriceCtl = new SKUPriceDetailController();
                        flag = SKUPriceCtl.ImportSKUPrices(int.Parse(DrpDistributor.SelectedValue.ToString()), path, int.Parse(DrpPrincipal.SelectedValue.ToString()));
                        if (!flag)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occurred.');", true);
                            return;
                        }
                    }
                    else if (cboFileTypes.SelectedValue == "7")// For Opening Stock
                    {
                        PurchaseController PurCtl = new PurchaseController();
                        DistributorController DistrCtl = new DistributorController();
                        DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(DrpDistributor.SelectedValue), 3);
                        flag = PurCtl.ImportOpeningStock(int.Parse(DrpDistributor.SelectedValue.ToString()), path, int.Parse(DrpPrincipal.SelectedValue.ToString()), Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]), Convert.ToInt32(Session["UserID"]));
                        if (!flag)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occurred.');", true);
                            return;
                        }
                    }
                    else if (cboFileTypes.SelectedValue == "8")//For Customer
                    {
                        CustomerDataController CustData = new CustomerDataController();
                        flag = CustData.ImportCustomer(Constants.IntNullValue, int.Parse(DrpDistributor.SelectedValue.ToString()), path);
                        if (!flag)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occurred.');", true);
                            return;
                        }
                    }
                    else if (cboFileTypes.SelectedValue == "5") //For Purchase
                    {
                        PurchaseController PurCtl = new PurchaseController();
                        DistributorController DistrCtl = new DistributorController();
                        DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(DrpDistributor.SelectedValue), 3);
                        flag = PurCtl.ImportPurchaseStock(int.Parse(DrpDistributor.SelectedValue.ToString()), path, int.Parse(DrpPrincipal.SelectedValue.ToString()), Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]), Convert.ToInt32(Session["UserID"]));
                        if (!flag)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occurred.');", true);
                            return;
                        }
                    }
                }
            }
            if (cboFileTypes.SelectedValue == "6")
            {
                SavePhyscialStockTakingByExcel(txtFile.PostedFile);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record insert successfully.');", true);
            LoadDistributor();
            this.LoadPrincipal();
            cboFileTypes.SelectedIndex = 0;
        }
        catch (Exception excp)
        {
            lblErrorMessage.Text = excp.ToString();
            cboFileTypes.SelectedIndex = 0;
            return;
        }
    }

    protected void lnkFormat_Click(object sender, EventArgs e)
    {
        lblErrorMessage.ForeColor = System.Drawing.Color.Red;
        lblErrorMessage.Text = string.Empty;
        if (cboFileTypes.SelectedValue == "3")
        {
            DownLoadFile(Server.MapPath("~/Docs/ImportItem.xlsx"));
        }
        else if (cboFileTypes.SelectedValue == "4")
        {
            DownLoadFile(Server.MapPath("~/Docs/ImportItemPrice.xlsx"));
        }
        else if (cboFileTypes.SelectedValue == "5")
        {
            DownLoadFile(Server.MapPath("~/Docs/ImportPurchase.xlsx"));
        }
        else if (cboFileTypes.SelectedValue == "6")
        {
            DownloadPhysicalStockTakingFormat();
        }
        else if (cboFileTypes.SelectedValue == "7")
        {
            DownLoadFile(Server.MapPath("~/Docs/ImportOpening.xlsx"));
        }
        else if (cboFileTypes.SelectedValue == "8")
        {
            DownLoadFile(Server.MapPath("~/Docs/ImportCustomer.xlsx"));
        }
        else
        {
            lblErrorMessage.Text = "Format not found";
        }
    }
    #region Phyiscal Stock Taking
    public void DownloadPhysicalStockTakingFormat()
    {
        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        using (ExcelPackage p = new ExcelPackage())
        {
            ExcelWorksheet ws = p.Workbook.Worksheets.Add("Physcial Stock Taking");

            GenerateOpeningStockColumns(ws, p);

            Byte[] fileBytes = p.GetAsByteArray();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PhysicalStockTakingFormat.xlsx");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
            Response.End();
        }
    }
    public void GenerateOpeningStockColumns(ExcelWorksheet ws, ExcelPackage p)
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();

        var headerCells = ws.Cells[1, 1, 1, 1];
        headerCells.Value = "Bar Code";
        var headerFont = headerCells.Style.Font;
        headerFont.Bold = true;
        headerCells.AutoFitColumns();


        headerCells = ws.Cells[1, 2, 1, 2];
        headerCells.Value = "Qty";
        headerFont = headerCells.Style.Font;
        headerFont.Bold = true;
        headerCells.AutoFitColumns();

    }
    public void SavePhyscialStockTakingByExcel(HttpPostedFile userPostedFile)
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var package = new ExcelPackage(userPostedFile.InputStream);

        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
        var start = 2;
        var end = workSheet.Dimension.End.Column;

        int totalRows = workSheet.Dimension.End.Row;
        lblErrorMessage.ForeColor = System.Drawing.Color.Red;

        DataTable m_dt = PController.SelectDataPrice2(Constants.IntNullValue, Constants.IntNullValue,
                Constants.IntNullValue, Constants.IntNullValue, int.Parse(DrpDistributor.SelectedValue.ToString()),
                int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 7,
                DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));

        DataTable dtPurchaseDetail = new DataTable();
        dtPurchaseDetail.Columns.Add("PHYSICAL_STOCK_ID", typeof(long));
        dtPurchaseDetail.Columns.Add("SKU_ID", typeof(int));
        dtPurchaseDetail.Columns.Add("SKU_Code", typeof(string));
        dtPurchaseDetail.Columns.Add("SKU_Name", typeof(string));
        dtPurchaseDetail.Columns.Add("UNIT_RATE", typeof(decimal));
        dtPurchaseDetail.Columns.Add("SALEABLE_QUANTITY", typeof(decimal));
        dtPurchaseDetail.Columns.Add("PACKSIZE", typeof(string));
        dtPurchaseDetail.Columns.Add("COLOR", typeof(string));

        for (int row = start; row <= totalRows; row++)
        {
            decimal qty = 0;
            string skuCode = "";

            for (int col = 1; col <= end; col++)
            { // ... Cell by cell...
                var cellValue = workSheet.Cells[row, col].Text;
                if (!string.IsNullOrEmpty(cellValue))
                {
                    cellValue = cellValue.Trim();

                    if (col == 1)
                    {
                        skuCode = cellValue;
                    }
                    else if (col == 2)
                    {
                        qty = decimal.Parse(dc.chkNull_0(cellValue));
                    }
                }
            }

            if (!string.IsNullOrEmpty(skuCode) && qty > 0)
            {
                DataRow[] foundRows = m_dt.Select("SKU_CODE  = '" + skuCode + "'");

                if (foundRows.Length > 0)
                {
                    DataRow dr = dtPurchaseDetail.NewRow();
                    dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                    dr["PHYSICAL_STOCK_ID"] = 0;
                    dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                    dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                    dr["COLOR"] = foundRows[0]["COLOR"];
                    dr["PACKSIZE"] = foundRows[0]["PACKSIZE"];
                    dr["SALEABLE_QUANTITY"] = qty;
                    dr["UNIT_RATE"] = foundRows[0]["DISTRIBUTOR_PRICE"];
                    dtPurchaseDetail.Rows.Add(dr);
                }
            }
        }

        //DataTable groupedSKU = dtPurchaseDetail.AsEnumerable().GroupBy(x => x.Field<int>("SKU_ID"))
        //    .Select(x=> x.CopyToDataTable()).FirstOrDefault();
        if (dtPurchaseDetail != null && dtPurchaseDetail.Rows.Count > 0)
        {
            var groupedData = dtPurchaseDetail.AsEnumerable()
    .GroupBy(x => x.Field<int>("SKU_ID"))
    .Select(x => new
    {
        SKU_ID = x.Key,
        PHYSICAL_STOCK_ID = long.Parse("0"),
        SKU_Code = x.FirstOrDefault().Field<string>("SKU_Code"),
        SKU_Name = x.FirstOrDefault().Field<string>("SKU_Name"),
        COLOR = x.FirstOrDefault().Field<string>("COLOR"),
        PACKSIZE = x.FirstOrDefault().Field<string>("PACKSIZE"),
        SALEABLE_QUANTITY = x.Sum(y => y.Field<decimal>("SALEABLE_QUANTITY")),
        UNIT_RATE = x.FirstOrDefault().Field<decimal>("UNIT_RATE"),
    }).ToList();

            DataTable groupedSKU = dtPurchaseDetail.Clone();

            foreach (var result in groupedData)
            {
                DataRow row = groupedSKU.NewRow();
                row["SKU_ID"] = result.SKU_ID;
                row["PHYSICAL_STOCK_ID"] = result.PHYSICAL_STOCK_ID;
                row["SKU_Code"] = result.SKU_Code;
                row["SKU_Name"] = result.SKU_Name;
                row["COLOR"] = result.COLOR;
                row["PACKSIZE"] = result.PACKSIZE;
                row["SALEABLE_QUANTITY"] = result.SALEABLE_QUANTITY;
                row["UNIT_RATE"] = result.UNIT_RATE;
                groupedSKU.Rows.Add(row);
            }

            try
            {
                PhaysicalStockController MController = new PhaysicalStockController();

                DateTime CurrentWorkDate = Constants.DateNullValue;
                DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
                foreach (DataRow dr in dtLocationInfo.Rows)
                {
                    if (dr["DISTRIBUTOR_ID"].ToString() == DrpDistributor.SelectedValue.ToString())
                    {
                        if (dr["MaxDayClose"].ToString().Length > 0)
                        {
                            CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                            break;
                        }
                    }
                }



                long maxDocID = LoadMAXDOC_NO();

                string success = MController.InsertPysicalStock(int.Parse(DrpDistributor.SelectedValue.ToString()),
                            CurrentWorkDate, 0, 0, maxDocID,
                            groupedSKU);

                lblErrorMessage.ForeColor = System.Drawing.Color.Green;
                lblErrorMessage.Text = "Imported Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    private long LoadMAXDOC_NO()
    {
        long docNo = Constants.LongNullValue;
        PhaysicalStockController MController = new PhaysicalStockController();
        DataTable dt = MController.SelectMaxDocNo();
        if (dt.Rows.Count > 0)
        {
            docNo = long.Parse(dt.Rows[0]["DOC_NO"].ToString());
        }

        return docNo;
    }
    #endregion
    private void DownLoadFile(string path)
    {
        FileInfo fi = new FileInfo(path);
        if (fi.Exists)
        {
            long sz = fi.Length;
            Response.ClearContent();
            Response.ContentType = MimeType(Path.GetExtension(path));
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename = {0}", System.IO.Path.GetFileName(path)));
            Response.AddHeader("Content-Length", sz.ToString("F0"));
            Response.TransmitFile(path);
            Response.End();
        }
    }
    public static string MimeType(string Extension)
    {
        string mime = "application/octetstream";
        if (string.IsNullOrEmpty(Extension))
            return mime;
        string ext = Extension.ToLower();
        Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (rk != null && rk.GetValue("Content Type") != null)
            mime = rk.GetValue("Content Type").ToString();
        return mime;
    }
}