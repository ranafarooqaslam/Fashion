using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form For Sales & Closing Stock Report
/// </summary>
public partial class Forms_RptDistributorReports : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.DistributorType();
            this.LoadAssingned();
          //  this.LoadPrincipal();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Location Types
    /// </summary>
    private void DistributorType()
    {
        DistributorController dController = new DistributorController();
        DataTable dt = dController.SelectDistributorTypeInfo(Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(ddDistributorType, dt, 0, 2);
    }

    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        if (ddDistributorType.Items.Count > 0)
        {
            drpDistributor.Items.Clear();    
            UserController mUserController = new UserController();
            DataTable dt = mUserController.SelectUserAssignment(int.Parse(this.Session["UserId"].ToString()), int.Parse(ddDistributorType.SelectedValue.ToString()), 1, int.Parse(this.Session["CompanyId"].ToString()));
            drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 1);
        }
    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    //private void LoadPrincipal()
    //{
    //    SKUPriceDetailController PController = new SKUPriceDetailController();
    //    DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
    //    DrpPrincipal.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
    //    clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1);
    //}

    /// <summary>
    /// Shows Sales & Closing Stock in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        
        DocumentPrintController mDocumentPrntControl = new DocumentPrintController();
        RptSaleController RptSaleCtl = new RptSaleController();
        DataSet ds = RptSaleCtl.GetRegionSaleDetail(int.Parse(this.ddDistributorType.SelectedItem.Value), Constants.IntNullValue, Constants.IntNullValue
            , int.Parse(drpDistributor.SelectedValue.ToString()), DateTime.Parse(this.txtStartDate.Text), DateTime.Parse(this.txtEndDate.Text)
            , Constants.IntNullValue, DrpReportType.SelectedIndex, DrpUnitType.SelectedIndex, int.Parse(this.Session["UserId"].ToString()));
        DataTable dt = mDocumentPrntControl.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

        CORNBusinessLayer.Reports.RegionWiseSaleReport CrpReport = new CORNBusinessLayer.Reports.RegionWiseSaleReport();
        CrpReport.SetDataSource(ds);
        CrpReport.Refresh();
        CrpReport.SetParameterValue("fromDate", txtStartDate.Text);
        CrpReport.SetParameterValue("todate", txtEndDate.Text);
        CrpReport.SetParameterValue("ReportTitle", "Daily " + DrpReportType.SelectedItem.Text + " In " + DrpUnitType.SelectedItem.Text);
        CrpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        Session.Add("CrpReport", CrpReport);
        Session.Add("ReportType", 0);  
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script); 

    }

    /// <summary>
    /// Loads Locations
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void ddDistributorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadAssingned();
    }

    /// <summary>
    /// Shows Sales & Closing Stock in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        DocumentPrintController mDocumentPrntControl = new DocumentPrintController();
        RptSaleController RptSaleCtl = new RptSaleController();

        DataTable ds = RptSaleCtl.GetRegionSaleDetailDataTable(int.Parse(this.ddDistributorType.SelectedItem.Value), Constants.IntNullValue, Constants.IntNullValue
            , int.Parse(drpDistributor.SelectedValue.ToString()), DateTime.Parse(this.txtStartDate.Text), DateTime.Parse(this.txtEndDate.Text)
            , Constants.IntNullValue, DrpReportType.SelectedIndex, DrpUnitType.SelectedIndex, int.Parse(this.Session["UserId"].ToString()));

        if (ds != null)
        {
            string fileName = "";
            string excelHeading = "";

            if (DrpReportType.SelectedValue == "0")
            {
                fileName = "SalesReport";
                excelHeading = "Sales Report";
            }
            else if (DrpReportType.SelectedValue == "1")
            {
                fileName = "SalesReturnReport";
                excelHeading = "Sales Return Report";
            }
            else if (DrpReportType.SelectedValue == "2")
            {
                fileName = "ShopDamageReport";
                excelHeading = "Shop Damage Report";
            }
            else if (DrpReportType.SelectedValue == "3")
            {
                fileName = "ImportDamageReport";
                excelHeading = "Import Damage Report";
            }
            else if (DrpReportType.SelectedValue == "4")
            {
                fileName = "ClosingStockReport";
                excelHeading = "Closing Stock Report";
            }
            else if (DrpReportType.SelectedValue == "5")
            {
                fileName = "PurchaseReport";
                excelHeading = "Purchase Report";
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\"+ fileName + ".xls";
            exportToExcelCustom(ds, path, excelHeading);
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                Response.Write("This file does not exist.");
            }
        }
    }
    public static void exportToExcelCustom(DataTable source, string fileName, string excelHeading)
    {

        System.IO.StreamWriter excelDoc;

        excelDoc = new System.IO.StreamWriter(fileName);
        const string startExcelXML = "<xml version>\r\n<Workbook " +
              "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
              " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
              "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
              "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
              "office:spreadsheet\">\r\n <Styles>\r\n " +
              "<Style ss:ID=\"Defarult\" ss:Name=\"Normal\">\r\n " +
              "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
              "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
              "\r\n <Protection/>\r\n </Style>\r\n " +
              "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
              "x:farmily=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
              "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
              " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
              "ss:ID=\"Decimal\">\r\n <NumberFormat " +
              "ss:Format=\"0.0000\"/>\r\n </Style>\r\n " +
              "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
              "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
              "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
              "ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n " +
              "</Styles>\r\n ";
        const string endExcelXML = "</Workbook>";

        int rowCount = 0;
        int sheetCount = 1;
        excelDoc.Write(startExcelXML);
        excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
        excelDoc.Write("<Table>");
        excelDoc.Write("<Row>");
        excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
        excelDoc.Write(excelHeading);
        excelDoc.Write("</Data></Cell>");
        excelDoc.Write("</Row>");
        excelDoc.Write("<Row>");
        for (int x = 0; x < source.Columns.Count; x++)
        {
            excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
            excelDoc.Write(source.Columns[x].ColumnName);

            excelDoc.Write("</Data></Cell>");
        }
        excelDoc.Write("</Row>");
        foreach (DataRow x in source.Rows)
        {
            rowCount++;
            //if the number of rows is > 64000 create a new page to continue output
            if (rowCount == 64000)
            {
                rowCount = 0;
                sheetCount++;
                excelDoc.Write("</Table>");
                excelDoc.Write(" </Worksheet>");
                excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                excelDoc.Write("<Table>");
            }
            excelDoc.Write("<Row>"); //ID=" + rowCount + "
            for (int y = 0; y < source.Columns.Count; y++)
            {
                System.Type rowType;
                rowType = x[y].GetType();
                switch (rowType.ToString())
                {
                    case "System.String":
                        string XMLstring = x[y].ToString();
                        XMLstring = XMLstring.Trim();
                        XMLstring = XMLstring.Replace("&", "&");
                        XMLstring = XMLstring.Replace(">", ">");
                        XMLstring = XMLstring.Replace("<", "<");
                        excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                       "<Data ss:Type=\"String\">");
                        excelDoc.Write(XMLstring);
                        excelDoc.Write("</Data></Cell>");
                        break;
                    case "System.DateTime":
                        //Excel has a specific Date Format of YYYY-MM-DD followed by  
                        //the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000
                        //The Following Code puts the date stored in XMLDate 
                        //to the format above
                        DateTime XMLDate = (DateTime)x[y];
                        string XMLDatetoString = ""; //Excel Converted Date
                        XMLDatetoString = XMLDate.Year.ToString() +
                             "-" +
                             (XMLDate.Month < 10 ? "0" +
                             XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
                             "-" +
                             (XMLDate.Day < 10 ? "0" +
                             XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
                             "T" +
                             (XMLDate.Hour < 10 ? "0" +
                             XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
                             ":" +
                             (XMLDate.Minute < 10 ? "0" +
                             XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
                             ":" +
                             (XMLDate.Second < 10 ? "0" +
                             XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
                             ".000";
                        excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
                                     "<Data ss:Type=\"DateTime\">");
                        excelDoc.Write(XMLDatetoString);
                        excelDoc.Write("</Data></Cell>");
                        break;
                    case "System.Boolean":
                        excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                    "<Data ss:Type=\"String\">");
                        excelDoc.Write(x[y].ToString());
                        excelDoc.Write("</Data></Cell>");
                        break;
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
                                "<Data ss:Type=\"Number\">");
                        excelDoc.Write(x[y].ToString());
                        excelDoc.Write("</Data></Cell>");
                        break;
                    case "System.Decimal":
                    case "System.Double":
                        excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
                              "<Data ss:Type=\"Number\">");
                        excelDoc.Write(x[y].ToString());
                        excelDoc.Write("</Data></Cell>");
                        break;
                    case "System.DBNull":
                        excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                              "<Data ss:Type=\"String\">");
                        excelDoc.Write("");
                        excelDoc.Write("</Data></Cell>");
                        break;
                        defarult:
                        throw (new Exception(rowType.ToString() + " not handled."));
                }
            }
            excelDoc.Write("</Row>");
        }
        excelDoc.Write("</Table>");
        excelDoc.Write(" </Worksheet>");
        excelDoc.Write(endExcelXML);

        excelDoc.Close();
    }
}
