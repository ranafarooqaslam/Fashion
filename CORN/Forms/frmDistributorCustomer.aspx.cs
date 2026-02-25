using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// From To Add, Edit Customer
/// </summary>
public partial class Forms_frmDistributorCustomer : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();

            LoadVolumeType();
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            btnSearch.Attributes.Add("onclick", "return SearchRecord()");
            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtRegdate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    private void LoadDistributor()
    {
        DistributorController mController = new DistributorController();
        DataTable dt = mController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(DrpDistributor, dt, 0, 2, true);
    }

    protected void DrpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {

        this.SetTableSorter();
    }



    private void LoadVolumeType()
    {
        SLASHCodesController mController = new SLASHCodesController();

        DataTable dt = mController.SelectSlashCodes(Constants.IntNullValue, null, Constants.CustomerVolumeClassType, null, Constants.IntNullValue, bool.Parse("True"));
        clsWebFormUtil.FillDropDownList(DrpVolumeClass, dt, 0, 2, true);

        this.DrpVolumeClass.SelectedValue = "88";
    }

    /// <summary>
    /// Loads Customers To Customer Grid
    /// </summary>
    private void LoadCustomer()
    {
        if (DrpDistributor.Items.Count > 0)  //&& DrpRoute.Items.Count > 0 && DrpMarket.Items.Count > 0
        {
            CustomerDataController mController = new CustomerDataController();
            DataTable dt = mController.UspSelectCustomer(int.Parse(DrpDistributor.SelectedValue.ToString()), ddSearchType.SelectedValue.ToString(), txtSeach.Text);
            Grid_users.EditIndex = -1;
            this.Grid_users.DataSource = dt;
            this.Grid_users.DataBind();
        }
    }

    /// <summary>
    /// Sets Customer Data For Edit. This Function Runs When An Existing Customer Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void Grid_users_RowEditing(object sender, GridViewEditEventArgs e)
    {

        try
        {
            this.Session.Add("CustomerId", long.Parse(Grid_users.Rows[e.NewEditIndex].Cells[0].Text));

            DrpDistributor.SelectedValue = Grid_users.Rows[e.NewEditIndex].Cells[1].Text;
            DrpVolumeClass.SelectedValue = Grid_users.Rows[e.NewEditIndex].Cells[2].Text;
            txtCustomerName.Text = Grid_users.Rows[e.NewEditIndex].Cells[4].Text.Replace("amp;", "");
            txtPhoneNo.Text = Grid_users.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
            txtEmailAddress.Text = Grid_users.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
            txtAddress.Text = Grid_users.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");

            txtRegdate.Text = (DateTime.Parse(Grid_users.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy")))).ToString("dd-MMM-yyyy");
            txtCNIC.Text = Grid_users.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
            txtCreditLimit.Text = Grid_users.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "0");

            chkIsActive.Checked = bool.Parse(Grid_users.Rows[e.NewEditIndex].Cells[11].Text);
            btnSave.Text = "Update";
            for (int i = 0; i < Grid_users.Rows.Count; i++)
            {
                Grid_users.Rows[i].Cells[12].Enabled = false;
              
            }
            this.SetTableSorter();
        }
        catch (Exception)
        {
            
            throw;
        }
    }




    /// <summary>
    /// Save Or Updates a Customer
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        CustomerDataController mController = new CustomerDataController();
        DataControl dc = new DataControl();
        if (btnSave.Text == "Save")
        {
            SETTINGS_TABLE_Controller mSettingsTableControl = new SETTINGS_TABLE_Controller();
            DataTable dtSettingsTable = mSettingsTableControl.Select_SETTINGS_TABLE("CUSTOMER", "CUSTOMER_ID", int.Parse(DrpDistributor.SelectedValue.ToString()));

            if (dtSettingsTable.Rows.Count > 0)
            {
                long CustomerId = long.Parse(dtSettingsTable.Rows[0]["Value"].ToString()) + 1;
                string StrCode = "";


                if (CustomerId.ToString().Length == 1)
                {
                    StrCode = "OT4000" + CustomerId.ToString();
                }
                else if (CustomerId.ToString().Length == 2)
                {
                    StrCode = "OT400" + CustomerId.ToString();
                }
                else if (CustomerId.ToString().Length == 3)
                {
                    StrCode = "OT40" + CustomerId.ToString();
                }
                else if (CustomerId.ToString().Length == 4)
                {
                    StrCode = "OT4" + CustomerId.ToString();
                }
                else if (CustomerId.ToString().Length == 5)
                {
                    StrCode = "OT" + CustomerId.ToString();
                }

                mController.InsertCustomer(CustomerId, false, chkIsActive.Checked, Constants.IntNullValue, int.Parse(DrpVolumeClass.SelectedValue.ToString()),
                    Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
                    int.Parse(DrpDistributor.SelectedValue.ToString()), "", "", txtPhoneNo.Text, txtEmailAddress.Text, StrCode, txtCustomerName.Text,
                    txtAddress.Text, DateTime.Parse(txtRegdate.Text), 1, 1, txtCNIC.Text, "", Convert.ToDecimal(dc.chkNull_0(txtCreditLimit.Text)));
               
                this.Session.Add("CustomerId", CustomerId);
            }
        }
        else
        {
            mController.UpdateCustomer(long.Parse(this.Session["CustomerId"].ToString()), false, chkIsActive.Checked,
            Constants.IntNullValue, int.Parse(DrpVolumeClass.SelectedValue.ToString()),
            Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, int.Parse(DrpDistributor.SelectedValue.ToString()), "", "",
            txtPhoneNo.Text, txtEmailAddress.Text, null, txtCustomerName.Text, txtAddress.Text, DateTime.Parse(txtRegdate.Text), 1, 1, txtCNIC.Text, ""
            , Convert.ToDecimal(dc.chkNull_0(txtCreditLimit.Text)));
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record Update');", true);
        this.ClearAll();
    }

    /// <summary>
    /// Clears All Controls Through ClearAll() Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.ClearAll();
    }

    /// <summary>
    /// Filters Customer From Customer Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.LoadCustomer();
        this.SetTableSorter();
    }

    /// <summary>
    /// Set Customer Grid For JQuery Sorting
    /// </summary>
    private void SetTableSorter()
    {
        if (Grid_users.Rows.Count > 1)
        {
            Grid_users.UseAccessibleHeader = true;
            Grid_users.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    /// <summary>
    /// Clears All  Constrols
    /// </summary>
    private void ClearAll()
    {
        txtCustomerName.Text = "";
        txtAddress.Text = "";
        txtPhoneNo.Text = "";
        txtSeach.Text = "";
        txtEmailAddress.Text = "";
        txtCNIC.Text = string.Empty;
        btnSave.Text = "Save";

        txtRegdate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        txtCreditLimit.Text = "";

        Grid_users.DataSource = null;
        Grid_users.DataBind();

    }
}
