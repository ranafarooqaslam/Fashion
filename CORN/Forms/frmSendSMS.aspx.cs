using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
public partial class Forms_frmSendSMS : System.Web.UI.Page
{
    //GsmCommMain comm;
    //readonly ConfigurationController _cController = new ConfigurationController();
    readonly SaleForceController UController = new SaleForceController();
    DataTable SMSNO;

    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadCustomers();
            loadData();
            txtPhoneNo.Focus();            
            txtSMS.Attributes["onkeyup"] = String.Format("count('{0}')", txtSMS.ClientID);
            GetSMSBalance();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }
    private void CreateTable()
    {
        SMSNO = new DataTable();
        SMSNO.Columns.Add("CONTACT_NO", typeof(string));
        Session.Add("SMSNO", SMSNO);

    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2);
    }
    private void loadData()
    {
        SaleForceController UController = new SaleForceController();
        int dId = int.Parse(HttpContext.Current.Session["DISTRIBUTOR_ID"].ToString());
        DataTable dtArea = UController.SelectSalesForceCustomers(dId, Constants.IntNullValue, Constants.LongNullValue, 5);
        Session.Add("dbSMSSetting", dtArea);
    }
    //private void LoadCustomers()
    //{
    //    DataTable dtArea = UController.SelectSalesForceCustomers(Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, 3);
    //    clsWebFormUtil.FillListBox(LstCustomer, dtArea, 8, 1, false);
    //}
    private void LoadCustomers()
    {
        LstCustomer.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            DataTable dtArea = UController.SelectSalesForceCustomers(int.Parse(drpDistributor.SelectedValue), Constants.IntNullValue, Constants.LongNullValue, 8);
            clsWebFormUtil.FillListBox(LstCustomer, dtArea, 0, 3, false);
        }
        lblCustomerCount.Text = LstCustomer.Items.Count.ToString();
    }
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        CreateTable();
        if (txtSMS.Text.Equals(""))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('message is required.');", true);
            txtSMS.Focus();
            return;
        }
        SMSNO = (DataTable)Session["SMSNO"];
        for (int i = 0; i < LstCustomer.Items.Count; i++)
        {
            if (LstCustomer.Items[i].Selected == true)
            {
                string No = LstCustomer.Items[i].Value.ToString();
                string ChkNo = CheckNumber(No);
                if (ChkNo.Length == 12)
                {
                    DataRow dr = SMSNO.NewRow();
                    dr["CONTACT_NO"] = ChkNo;
                    SMSNO.Rows.Add(dr);
                }
            }
        }
        if (!txtPhoneNo.Text.Equals(""))
        {
            string s = txtPhoneNo.Text;
            string[] values = s.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                string No = values[i];
                string ChkNo = CheckNumber(No);
                if (ChkNo.Length == 12)
                {
                    DataRow dr = SMSNO.NewRow();
                    dr["CONTACT_NO"] = ChkNo;
                    SMSNO.Rows.Add(dr);
                }
            }
        }
        if (SMSNO.Rows.Count > 0)
        {
            int Count = 0;
            string Type = "";
            foreach (DataRow dr in SMSNO.Rows)
            {
                string Msg = txtSMS.Text;
                string Contact_No = dr["CONTACT_NO"].ToString();
                //   string MsgType = "NeedGarment";// "Puri Center";
                /////////////////////////////////////////////
                try
                {

                    {
                        DataTable dtArea = (DataTable)HttpContext.Current.Session["dbSMSSetting"];

                        if (dtArea.Rows.Count > 0)
                        {
                            string smsMSg = dtArea.Rows[0]["MESSAGE"].ToString();
                            string UserId = dtArea.Rows[0]["USERID"].ToString();
                            string pass = dtArea.Rows[0]["PASSWORD"].ToString();
                            string MsgType = dtArea.Rows[0]["MASK"].ToString();
                            string url = dtArea.Rows[0]["URL"].ToString();
                            // string Msg = smsMSg + value;
                            //string Contact_No = CheckNumber(custnumber);

                            SendSMS(Contact_No, Msg, MsgType, url, UserId, pass);
                        }
                    }

                }
                catch (Exception eeee)
                {
                    eeee.ToString();
                }
                ///////////////////////////////////////////


                // SendSMS(Contact_No, Msg, MsgType, "http://www.outreach.pk/api/sendsms.php/sendsms/url");
                Count += 1;
            }
            if (Count == 1)
            {
                Type = "message is";
            }
            else
            {
                Type = "messages are";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + Count + " " + Type + " sent successfully.');", true);
            this.txtPhoneNo.Text = "";
            this.txtSMS.Text = "";
            Session.Remove("SMSNO");
            GetSMSBalance();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('You didn't select or write number to send this message.');", true);
        }
    }
    public string CheckNumber(string CNO)
    {
        string Customer_No = "";
        string CONTACT_NO = CNO;
        if (CONTACT_NO.Length == 11) // 0300xxxxxxx
        {
            string str = CNO.Substring(0, 2);
            if (str.ToString() == "03")
            {
                string str1 = CNO.Substring(1, 10);
                Customer_No = "92" + str1;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 12) // 92300xxxxxxx
        {
            string str = CNO.Substring(0, 3);
            if (str.ToString() == "923")
            {
                Customer_No = CNO;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 13) // 920300xxxxxxx
        {
            string str = CNO.Substring(0, 3);
            if (str.ToString() == "920")
            {
                string str1 = CNO.Substring(0, 2);
                string str2 = CNO.Substring(3, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }

        }
        else if (CONTACT_NO.Length == 14) // 0092300xxxxxxx
        {
            string str = CNO.Substring(0, 5);
            if (str.ToString() == "00923")
            {
                string str1 = CNO.Substring(2, 2);
                string str2 = CNO.Substring(4, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 15) // 00920300xxxxxxx
        {
            string str = CNO.Substring(0, 5);
            if (str.ToString() == "00920")
            {
                string str1 = CNO.Substring(2, 2);
                string str2 = CNO.Substring(5, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }
        }
        return Customer_No;
    }
    //public string SendSMS(string customerNo, string msg, string msgType, string Url)
    //{
    //    //String url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
    //    String result = "";
    //    //String message = HttpUtility.UrlEncode("Hello this is a test msg from Ijaz Jamil Akhtar");
    //    String strPost = "id=rchneedgarment&pass=msa@1234&msg=" + msg + "&to=" + customerNo + "" + "&mask=" + msgType + "&type=xml&lang=English";
    //    StreamWriter myWriter = null;
    //    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Url);

    //    objRequest.Method = "POST";
    //    objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
    //    objRequest.ContentType = "application/x-www-form-urlencoded";
    //    try
    //    {
    //        myWriter = new StreamWriter(objRequest.GetRequestStream());
    //        myWriter.Write(strPost);
    //    }
    //    catch (Exception e)
    //    {
    //        return e.Message;
    //    }
    //    finally
    //    {
    //        myWriter.Close();
    //    }
    //    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
    //    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
    //    {
    //        result = sr.ReadToEnd();   // Close and clean up the StreamReader   
    //        sr.Close();
    //    }
    //    return result;
    //}
    public string SendSMS(string customerNo, string msg, string msgType, string Url, string userId, string password)
    {
        //String url = "http://www.outreach.pk/api/sendsms.php/sendsms/url";
        String result = "";
        //String message = HttpUtility.UrlEncode("Hello this is a test msg from Ijaz Jamil Akhtar");
        String strPost = "id=" + userId + "&pass=" + password + "&msg=" + msg + "&to=" + customerNo + "" + "&mask=" + msgType + "&type=xml&lang=English";
        StreamWriter myWriter = null;
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Url);

        objRequest.Method = "POST";
        objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
        objRequest.ContentType = "application/x-www-form-urlencoded";
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(strPost);
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            myWriter.Close();
        }
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();   // Close and clean up the StreamReader   
            sr.Close();
        }
        return result;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.txtPhoneNo.Text = "";
        this.txtSMS.Text = "";
    }
    private void GetSMSBalance()
    {
        DataTable dbSMSSetting = (DataTable)HttpContext.Current.Session["dbSMSSetting"];
        if (dbSMSSetting != null)
        {
            if (dbSMSSetting.Rows.Count > 0)
            {
                String result = "";
                String strPost = "id=" + dbSMSSetting.Rows[0]["USERID"].ToString() + "&pass=" + dbSMSSetting.Rows[0]["PASSWORD"].ToString() + "&mask=" + dbSMSSetting.Rows[0]["MASK"].ToString() + "&type=xml&lang=English";
                StreamWriter myWriter = null;

                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create("http://www.outreach.pk/api/sendsms.php/balance/status");

                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(strPost);
                }
                catch (Exception e)
                {
                }
                finally
                {
                    myWriter.Close();
                }
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                try
                {
                    string[] Balance = result.Split(new[] { "response>" }, StringSplitOptions.None);
                    lblSMSBalance.Text = "SMS Balance : " + Balance[1].ToString();
                }
                catch (Exception ex)
                {
                }

                try
                {
                    string[] expiry = result.Split(new[] { "expiry>" }, StringSplitOptions.None);
                    string expiry2 = expiry[1].ToString().Replace("</", "");
                    lblExpiry.Text = "Expiry Date : " + Convert.ToDateTime(expiry2).ToString("dd-MMM-yyyy");
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
    protected void drpDistributor_SelectedIndexChanged1(object sender, EventArgs e)
    {
        LoadCustomers();
    }
    protected void btnFiler_Click(object sender, EventArgs e)
    {
        LstCustomer.Items.Clear();
        if (drpDistributor.Items.Count > 0)
        {
            DataTable dtArea = UController.GetCustomerByDate(int.Parse(drpDistributor.SelectedValue), Convert.ToDateTime(txtStartDate.Text),Convert.ToDateTime(txtEndDate.Text), 1);
            clsWebFormUtil.FillListBox(LstCustomer, dtArea, 0, 3, false);
        }
        lblCustomerCount.Text = LstCustomer.Items.Count.ToString();
    }
}