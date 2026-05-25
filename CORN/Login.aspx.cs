using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

public partial class Login : Page
{
    readonly UserController _mController = new UserController();
    readonly DistributorController _mDist = new DistributorController();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                bool IsLicense = true;
                try
                {
                    if (Session["LicenseMessage"].ToString().Length > 0)
                    {
                        IsLicense = false;
                    }
                }
                catch (Exception ex)
                {
                    IsLicense = true;
                }
                if (Convert.ToInt32(Session["UserID"]) > 0 && IsLicense)
                {
                    Response.Redirect("Forms/Home.aspx");
                }
            }
            catch (Exception)
            {
                Session.Remove("LicenseMessage");
                txtLogin.Focus();
            }
        }
    }

    public void ValidateUser()
    {

        if (txtLogin.Text == "" && txtPassword.Text == "")
        {
            return;
        }
        else
        {
            DataTable dt = _mController.SelectSlashUser(txtLogin.Text, txtPassword.Text);
            Session.Clear();
            if (dt.Rows.Count > 0)
            {
                Session.Add("UserID", Convert.ToInt32(dt.Rows[0]["USER_ID"].ToString()));
                Session.Add("UserName", dt.Rows[0]["USER_DETAIL"].ToString());
                Session.Add("COMPANY_NAME", dt.Rows[0]["COMPANY_NAME"].ToString());
                Session.Add("DISTRIBUTOR_ID", Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"]));
                Session.Add("CompanyId", Convert.ToInt32(dt.Rows[0]["COMPANY_ID"].ToString()));
                Session.Add("RoleID", Convert.ToInt32(dt.Rows[0]["ROLE_ID"]));
                LastClosedDay(Convert.ToInt32(dt.Rows[0]["USER_ID"]), Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"]));
                Session.Add("UserName2", dt.Rows[0]["USER_NAME"].ToString());
                Session.Add("UserName", dt.Rows[0]["USER_DETAIL"].ToString());
                long userLogId = _mController.InsertUserLoginTime(Convert.ToInt32(dt.Rows[0]["USER_ID"]));
                Session.Add("User_Log_ID", userLogId);
                Session.Add("DISCOUNT_ALLOWD", dt.Rows[0]["DISCOUNT_ALLOWD"].ToString());
                Session.Add("PROMOTION_ON", dt.Rows[0]["PROMOTION_ON"].ToString());
                Session.Add("IMAGE_PATH", dt.Rows[0]["IMAGE_PATH"].ToString());
                Session.Add("CanRefund", dt.Rows[0]["CanRefund"].ToString());
                this.GetAppSetting();
                DataTable dtLicenseData = _mDist.GetLicenseData(Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"]));
                if (dtLicenseData.Rows.Count > 0)
                {
                    DateTime dtMaxClosingDate = Constants.DateNullValue;
                    DateTime dtMaxDate = Constants.DateNullValue;
                    try
                    {
                        dtMaxClosingDate = Convert.ToDateTime(dtLicenseData.Rows[0]["MaxClosingDate"]);
                        dtMaxDate = Convert.ToDateTime(Cryptography.Decrypt(dtLicenseData.Rows[0]["LICENSE_DATE"].ToString(), "b0tin@74"));
                    }
                    catch (Exception)
                    {
                        lblLicenseMsg.Text = "CORN POS license has been expired. Please pay monthly subscription fee to continue uninterpreted services. Thank you!";
                        btnSignIn.Visible = false;
                        txtLogin.Visible = false;
                        txtPassword.Visible = false;
                        dvLicense.Visible = true;
                        Session.Clear();
                        return;
                    }
                    if (dtMaxClosingDate >= dtMaxDate)
                    {
                        lblLicenseMsg.Text = "CORN POS license has been expired. Please pay monthly subscription fee to continue uninterpreted services. Thank you!";
                        btnSignIn.Visible = false;
                        txtLogin.Visible = false;
                        txtPassword.Visible = false;
                        dvLicense.Visible = true;
                        Session.Clear();
                        return;
                    }
                    else
                    {
                        if ((dtMaxDate - dtMaxClosingDate).TotalDays <= 5)
                        {
                            double _remaingindays = (dtMaxDate - dtMaxClosingDate).TotalDays;
                            lblLicenseMsg.Text = string.Format("CORN POS license will be expired after {0} Day(s), Please pay monthly subscription fee to continue uninterpreted services. Thank you!", _remaingindays);
                            Session.Add("LicenseMessage", lblLicenseMsg.Text);
                        }
                    }
                }
                Response.Redirect(dt.Rows[0]["ROLE_ID"].ToString() == "49" ? "Forms/frmOrderPOS.aspx" : "Forms/Home.aspx");
            }
            else
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Wrong User Id/Password";
                return;
            }
        }
    }

    private void LastClosedDay(int userId, int pDistributor)
    {
        DataTable dt = _mDist.SelectMaxDayClose(userId, pDistributor);
        Session.Add("CurrentWorkDate",
            dt.Rows.Count > 0 ? DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString()) : DateTime.Now);
    }

    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        //if (System.Configuration.ConfigurationManager.AppSettings["ComputerInfo"] == CORNCommon.Classes.Cryptography.Encrypt(CORNCommon.Classes.ComputerInfo.Value(), "b0tin@74"))
        //{
         ValidateUser();
        //}
        //else
        //{
        //    lblErrorMsg.Visible = true;
        //    lblErrorMsg.Text = "Invalid Key. Contact to Administrator.";
        //}
        
    }

    private void GetAppSetting()
    {
        CompanyController Company = new CompanyController();
        DataTable dtAppSetting = Company.GetAppSetting();
        this.Session.Add("dtAppSetting", dtAppSetting);
    }
}