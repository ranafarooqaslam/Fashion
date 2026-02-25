using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Collections.Generic;
using System.Web.Services;
using System.Drawing;

/// <summary>
/// Form For Day Close
/// </summary>
public partial class Forms_frmDayCloseStatus : System.Web.UI.Page
{
    DistributorController mController = new DistributorController();
    DataControl DC = new DataControl();
    DataSet ds = null;
    private System.Drawing.Printing.PrintDocument prnDocument;
    public int printType;
    public static string CompanyName;
    public static string CompanyPhonNmbr;
    private static string HirarchyNameQuiz;
    private static string Location;

    #region Variables
    // for Report:
    private int CurrentY;
    private int CurrentX;

    private int leftMargin;
    private int rightMargin;
    private int topMargin;
    private int bottomMargin;
    private int InvoiceWidth;
    private int InvoiceHeight;

    // for Invoice Head:
    private string InvTitle;
    private string InvSubTitle1;
    private string InvSubTitle2;
    private string InvSubTitle3;

    // Font and Color:------------------
    // Title Font
    private Font InvTitleFont = new Font("Arial", 24, FontStyle.Regular);
    // Title Font height
    private int InvTitleHeight;
    // SubTitle Font
    private Font InvSubTitleFont = new Font("Arial", 10, FontStyle.Regular);
    // SubTitle Font height
    private int InvSubTitleHeight;
    // Invoice Font
    private Font InvoiceFont = new Font("Arial", 8, FontStyle.Regular);
    private Font InvoiceFont2 = new Font("Arial", 6, FontStyle.Bold);
    private Font InvoiceFont3 = new Font("Arial", 6, FontStyle.Bold);
    private Font InvoiceFont4 = new Font("Arial", 7, FontStyle.Bold);
    private Font SaleFont = new Font("Arial", 8, FontStyle.Bold);


    // Blue Color
    private SolidBrush BlueBrush = new SolidBrush(Color.Blue);
    // Red Color
    private SolidBrush RedBrush = new SolidBrush(Color.Red);
    // Black Color
    private SolidBrush BlackBrush = new SolidBrush(Color.Black);

    #endregion
    
   
    string startdate;

  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LastClosedDay(int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["Distributor_Id"].ToString()));
            if (Grid_Hierarchy.Rows.Count > 1)
            {
                Grid_Hierarchy.UseAccessibleHeader = true;
                Grid_Hierarchy.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            loadloginDetail();
            txtstartdate.Text = (DateTime.Parse(this.Session["CurrentWorkDate"].ToString())).ToString("dd-MMM-yyyy");
            startdate = (DateTime.Parse(this.Session["CurrentWorkDate"].ToString())).ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Gets Location(s) Last Day Close(s)
    /// </summary>
    /// <param name="UserId">User</param>
    /// <param name="p_Distributor">Location</param>
    private void LastClosedDay(int UserId, int p_Distributor)
    {
        DistributorController mDayClose = new DistributorController();
        DataTable dt = mDayClose.SelectMaxDayClose(UserId, p_Distributor);
        if (dt.Rows.Count > 0)
        {
            this.Session.Add("CurrentWorkDate", DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString()));
            btnDayClose.Visible = true;
        }
        else
        {
           this.Session.Add("CurrentWorkDate", DateTime.Now);
           rblDistributorTypes.Visible = true;
        }
        GetLastClosedDay(UserId, p_Distributor, 0);       
    }
    
    /// <summary>
    /// Loads Location(s) Last Day Close(s) To Grid
    /// </summary>
    /// <param name="UserId">User</param>
    /// <param name="p_Distributor">Location</param>
    /// <param name="p_Status">Status</param>
    private void GetLastClosedDay(int UserId, int p_Distributor, int p_Status)
    {
        DataTable dtable = mController.MaxDayClose(int.Parse(this.Session["UserId"].ToString()), p_Status);
        Grid_Hierarchy.DataSource = dtable;
        Grid_Hierarchy.DataBind();
    }
    private bool GetDocumentNo(int p_distributor_ID)
    {
        
        PurchaseController mPurchase = new PurchaseController();
        DataTable dt = mPurchase.SelecttransferOutDocuments(Constants.Document_Transfer_Out, Constants.IntNullValue,
            Constants.LongNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue,
            p_distributor_ID, Convert.ToInt16(0));
        
        if (dt.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Loads Locations(Active/InActive/All) Last Day Close(s) To Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void rblDistributorTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLastClosedDay(int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["Distributor_Id"].ToString()), Convert.ToInt32(rblDistributorTypes.SelectedValue));
        if (Grid_Hierarchy.Rows.Count > 1)
        {
            Grid_Hierarchy.UseAccessibleHeader = true;
            Grid_Hierarchy.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    /// <summary>
    /// Performs Following Tasks And LogOuts
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// Closes Day Transactions
    /// </item>
    /// <item>
    /// Inserts Cash Data To GL
    /// </item>
    /// <item>
    /// Inserts Cheques Data To GL
    /// </item>
    /// <item>
    /// Inserts Expenses Data To GL
    /// </item>
    /// <item>
    /// Inserts Sales And Sales Return TO GL
    /// </item>
    /// <iterm>
    /// Inserts Purchase Data To GL
    /// </iterm>
    /// <item>
    /// Inserts Purchase Return To GL
    /// </item>
    /// <item>
    /// Inserts Rate Difference Data To GL
    /// </item>
    /// <item>
    /// Inserts LogOut Time
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnDayClose_Click(object sender, EventArgs e)
    {
        if (btnDayClose.Visible == true)
        {
            DistributorController mDayClose = new DistributorController();

            bool isRowsExists = false;

            var checkedCount = 0;
            bool dt = false;

            foreach (GridViewRow item in Grid_Hierarchy.Rows)
            {
                CheckBox checkedItem = (CheckBox)item.FindControl("ChbIsAssigned");
                if (checkedItem.Checked == true)
                {
                    checkedCount++;
                }
            }
            if (checkedCount == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please select some record');", true);
                return;
            }

            foreach (GridViewRow item in Grid_Hierarchy.Rows)
            {
                CheckBox checkedItem = (CheckBox)item.FindControl("ChbIsAssigned");
                if (checkedItem.Checked == true)
                {
                    int locationId = int.Parse(item.Cells[1].Text);
                    isRowsExists = GetDocumentNo(locationId);

                    if (isRowsExists == true)
                    {
                        break;
                    }
                }
            }


            if (!isRowsExists)
            {
                foreach (GridViewRow item in Grid_Hierarchy.Rows)
                {
                    CheckBox checkedItem = (CheckBox)item.FindControl("ChbIsAssigned");
                    if (checkedItem.Checked == true)
                    {
                        string dateString = item.Cells[5].Text;
                        string format = "dd-MM-yyyy";

                        DateTime dateTime;
                        if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out dateTime))
                        {
                            var closingDate = dateTime.AddDays(1);

                            dt = mDayClose.UspDayClose(closingDate,
                                int.Parse(item.Cells[1].Text), int.Parse(this.Session["UserID"].ToString()));
                        }
                        else
                        {
                            //Console.WriteLine("Conversion failed. Invalid date format.");
                        }
                    }
                }


                if (dt == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some error in day Close Contact System Administrator');", true);
                }
                else
                {

                    UserController mController = new UserController();
                    if (mController.InsertUserLogoutTime(Convert.ToInt64(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"])) == "Logout Time Inserted")
                    {
                        // btnViewSalesReport_Click(sender, e);
                        if (System.Configuration.ConfigurationManager.AppSettings["IsPrint"].ToString() == "1")
                        {
                            this.PrintReport(1);
                        }
                        this.Session.Clear();
                        Response.Redirect("~/Login.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some error in day Close Contact System Administrator');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Please Complete your Transfer IN Process');", true);
            }
        }
  
    }

    #region Sale Report

    private void loadloginDetail()
    {
        try
        {
            UserController userControl = new UserController();
            DataTable dt = userControl.SelectSlashUser2(int.Parse(this.Session["UserId"].ToString()));
           // lbllogintimedate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");//DateTime.Now.ToString("MM/dd/yyyy");
           // lbluserlogin.Text = dt.Rows[0]["USER_NAME"].ToString();

            Location = dt.Rows[0]["DISTRIBUTOR_NAME"].ToString();
            CompanyName = dt.Rows[0]["COMPANY_NAME"].ToString();
            CompanyPhonNmbr = dt.Rows[0]["CONTACT_NUMBER"].ToString();//Location Contact Number

           Session.Add("DISTRIBUTOR_ID", dt.Rows[0]["DISTRIBUTOR_ID"].ToString());

            DataTable dt2 = userControl.SelectUserPrincipal(int.Parse(this.Session["UserId"].ToString()));

            HirarchyNameQuiz = dt2.Rows[0]["SKU_HIE_NAME"].ToString();
           // Session.Add("PRINCIPAL_ID", dt2.Rows[0]["PRINCIPAL_ID"].ToString());


        }
        catch (Exception eee)
        {
            eee.Message.ToString();
        }
    }


    #endregion

    private void PrintReport(int type)
    {
        try
        {
            printType = type;
            this.prnDocument = new System.Drawing.Printing.PrintDocument();
            prnDocument.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
            // The Event of 'PrintPage'
            prnDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(prnDocument_PrintPage);
            prnDocument.Print();
        }
        catch (Exception e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + e.Message + "');", true);
        }
    }

    private void prnDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        leftMargin = (int)e.MarginBounds.Left;//100
        rightMargin = (int)e.MarginBounds.Right;//215
        topMargin = (int)e.MarginBounds.Top;//100
        topMargin = 1;
        bottomMargin = (int)e.MarginBounds.Bottom;//1069
        InvoiceWidth = (int)e.MarginBounds.Width;//115
        InvoiceHeight = (int)e.MarginBounds.Height;//969
       
            SetSaleReportHead(e.Graphics);
    }
    private void ReadInvoiceHead()
    {
        //Titles and Image of invoice:
        InvTitle = HirarchyNameQuiz;// CompanyName;//lblCompanyName.Text;
        InvSubTitle1 = Location;
        InvSubTitle2 = "PH#:  " + CompanyPhonNmbr; //lblphone.Text; 
      
        //InvImage = Application.StartupPath + @"\Images\" + "InvPic.jpg";
    }
    private void SetSaleReportHead(Graphics g)
    {
        OrderEntryController or = new OrderEntryController();
        DataTable dt = or.SelectSaleReport(int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), Constants.IntNullValue, DateTime.Parse(txtstartdate.Text), DateTime.Parse(txtstartdate.Text),Constants.LongNullValue);
        decimal totalGrossSale = 0;
        decimal totalDiscount = 0;
        decimal totalGST = 0;
        decimal totalNetAMount = 0;
        decimal totalCredit = 0;

        if (dt.Rows[0]["TOTAL_AMOUNT"] != DBNull.Value)
        {
            totalGrossSale = Math.Round(decimal.Parse(dt.Rows[0]["TOTAL_AMOUNT"].ToString()), 2);
            totalDiscount = Math.Round(decimal.Parse(dt.Rows[0]["DISCOUNT"].ToString()), 2);
            totalGST = Math.Round(decimal.Parse(dt.Rows[0]["GST_AMOUNT"].ToString()), 2);
          
            totalCredit = Math.Round(decimal.Parse(dt.Rows[0]["CREDIT_AMOUNT"].ToString()), 2);
           // totalNetAMount = Math.Round(decimal.Parse(dt.Rows[0]["NET_TOTAL"].ToString()), 2);
            totalNetAMount = Math.Round((totalGrossSale - totalDiscount + totalGST - totalCredit),2);
        
        }
        else
        {
            totalGrossSale = 0;
            totalDiscount = 0;
            totalGST = 0;
            totalNetAMount = 0;
            totalCredit = 0;
        }
        ReadInvoiceHead();

        CurrentY = topMargin;
        CurrentX = leftMargin;
        int ImageHeight = 0;
        InvTitleHeight = (int)(InvTitleFont.GetHeight(g));
        InvSubTitleHeight = (int)(InvSubTitleFont.GetHeight(g));

        // Get Titles Length:
        int lenInvTitle = (int)g.MeasureString(InvTitle, InvTitleFont).Width;
        int lenInvSubTitle1 = (int)g.MeasureString(InvSubTitle1, InvSubTitleFont).Width;
        int lenInvSubTitle2 = (int)g.MeasureString(InvSubTitle2, InvSubTitleFont).Width;
        int lenInvSubTitle3 = (int)g.MeasureString(InvSubTitle3, InvSubTitleFont).Width;
        //  int lenInvSubTitle4 = (int)g.MeasureString(InvSubTitle4, InvSubTitleFont).Width;
        // Set Titles Left:
        int xInvTitle = CurrentX + (InvoiceWidth - lenInvTitle) / 2;
        int xInvSubTitle1 = CurrentX + (InvoiceWidth - lenInvSubTitle1) / 2;
        int xInvSubTitle2 = CurrentX + (InvoiceWidth - lenInvSubTitle2) / 2;
        int xInvSubTitle3 = CurrentX + (InvoiceWidth - lenInvSubTitle3) / 2;
        //  int xInvSubTitle4 = CurrentX + (InvoiceWidth - lenInvSubTitle4) / 2;

        // Draw Invoice Head:
        if (InvTitle != "")
        {
            CurrentY = CurrentY + ImageHeight;
            g.DrawString(InvTitle, InvTitleFont, BlueBrush, xInvTitle, CurrentY);
        }
        if (InvSubTitle1 != "")
        {
            CurrentY = CurrentY + InvTitleHeight;
            g.DrawString(InvSubTitle1, InvSubTitleFont, BlueBrush, xInvSubTitle1, CurrentY);
        }
        if (InvSubTitle2 != "")
        {
            CurrentY = CurrentY + InvSubTitleHeight;
            g.DrawString(InvSubTitle2, InvSubTitleFont, BlueBrush, xInvSubTitle2, CurrentY);
        }
        InvSubTitle3 = "SALES REPORT";//
        CurrentY = CurrentY + InvSubTitleHeight + 10;
        g.DrawString(InvSubTitle3, InvSubTitleFont, BlueBrush, 100, CurrentY);

        CurrentY = CurrentY + InvSubTitleHeight + 15;
        g.DrawString("Sales Date:" + DateTime.Parse(txtstartdate.Text).ToString("dd-MMM-yyyy"), InvSubTitleFont, BlueBrush, 10, CurrentY); //+ "   To  :" + enddate, InvSubTitleFont, BlueBrush, 10, CurrentY);

        // Draw line:

        // totalGrossSale=totalGrossSale-totalDiscount-totalGST;
        CurrentY = CurrentY + InvSubTitleHeight + 8;
        g.DrawLine(new Pen(Brushes.Black, 2), 10, CurrentY, 300, CurrentY);

        CurrentY = CurrentY + 20;
        g.DrawString("Gross Sale  : ", SaleFont, BlueBrush, 30, CurrentY);
        g.DrawString(totalGrossSale.ToString(), SaleFont, BlueBrush, 200, CurrentY);
        CurrentY = CurrentY + 30;

        g.DrawString("Discount  : ", SaleFont, BlueBrush, 30, CurrentY);
        g.DrawString(totalDiscount.ToString(), SaleFont, BlueBrush, 200, CurrentY);

        CurrentY = CurrentY + 30;
        g.DrawString("GST  : ", SaleFont, BlueBrush, 30, CurrentY);
        g.DrawString(totalGST.ToString(), SaleFont, BlueBrush, 200, CurrentY);

        CurrentY = CurrentY + 30;
        g.DrawString("Credit Card  : ", SaleFont, BlueBrush, 30, CurrentY);
        g.DrawString(totalCredit.ToString(), SaleFont, BlueBrush, 200, CurrentY);

        CurrentY = CurrentY + 30;
        g.DrawString("Net Sale  : ", SaleFont, BlueBrush, 30, CurrentY);
        g.DrawString(totalNetAMount.ToString(), SaleFont, BlueBrush, 200, CurrentY);

        CurrentY = CurrentY + 30;

        g.DrawLine(new Pen(Brushes.Black, 1), 10, CurrentY, 300, CurrentY);
        CurrentY = CurrentY + 15;

        g.DrawString("Cash In Hand  : ", SaleFont, BlueBrush, 30, CurrentY);
        g.DrawString(totalNetAMount.ToString(), SaleFont, BlueBrush, 200, CurrentY);
        CurrentY = CurrentY + 30;

        g.DrawLine(new Pen(Brushes.Black, 2), 10, CurrentY, 300, CurrentY);
        
        CurrentY = CurrentY + 10;

        g.DrawString("Sales Person  : " + "All", SaleFont, BlueBrush, 10, CurrentY);
       
        CurrentY = CurrentY + 15;

        g.DrawString("Print Date : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"), SaleFont, BlueBrush, 10, CurrentY);

        CurrentY = CurrentY + 30;

        g.DrawString("POWERD BY:  www.fastservices.pk.", InvoiceFont3, BlackBrush, 10, CurrentY);
    }
    
}
