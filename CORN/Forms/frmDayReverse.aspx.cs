using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_frmDayReverse : System.Web.UI.Page
{
    readonly GeoHierarchyController gController = new GeoHierarchyController();
    readonly SkuHierarchyController mController = new SkuHierarchyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dtMasterDistributor = DController.SelectDistributorInfoByTownbyUser(
            Constants.IntNullValue, Constants.IntNullValue,
            int.Parse(this.Session["UserID"].ToString()), Constants.IntNullValue, 
            Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, 1);

        clsWebFormUtil.FillDropDownList(drpDistributor, dtMasterDistributor, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME", true);
        Session.Add("DistributorFrom", dtMasterDistributor);

        drpDistributor_SelectedIndexChanged(null, null);
    }

    protected void btnViewPDF_Click(object sender, EventArgs e)
    {

        string workingDate = workDate.InnerText;
        DateTime finalDate = Constants.DateNullValue;
        if (!string.IsNullOrEmpty(workingDate))
        {
            finalDate = Convert.ToDateTime(workingDate);
        }

        if (finalDate != null && finalDate != Constants.DateNullValue)
        {
            DistributorController mDayClose = new DistributorController();
            var success = mDayClose.UspDayReverse(finalDate,
                int.Parse(drpDistributor.SelectedValue.ToString()),
                int.Parse(this.Session["UserID"].ToString()));
            if (success == true)
            {
                UserController mController = new UserController();
                if (mController.InsertUserLogoutTime(Convert.ToInt64(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"])) == "Logout Time Inserted")
                {
                    this.Session.Clear();
                    Response.Redirect("~/Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some error in Day Reverse Contact System Administrator');", true);
                }

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Day Reversed Successfully !');", true);
                //Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some error in Day Reverse Contact System Administrator');", true);
            }
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Distributor = (DataTable)Session["DistributorFrom"];
        DateTime WorkingDate = Constants.DateNullValue;
        foreach (DataRow dr in Distributor.Rows)
        {
            if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.SelectedValue)
            {
                WorkingDate = Convert.ToDateTime(dr["CLOSING_DATE"]);
                break;
            }
        }

        workDate.InnerText = WorkingDate.ToString("dd-MMM-yyyy");
    }
}