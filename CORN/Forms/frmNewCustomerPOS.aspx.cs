using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// From To Add, Edit Customer
/// </summary>
public partial class Forms_frmNewCustomerPOS : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function Populates All Combos And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadDistributor();
            //this.LoadTown();
            // this.LoadRoute();
            // this.LoadMarket();
            // this.LoadChannelType();
            //  this.LoadBusinessType();
            this.LoadVolumeType();
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            btnSearch.Attributes.Add("onclick", "return SearchRecord()");
            CORNCommon.Classes.Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtRegdate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtRegdate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DistributorController mController = new DistributorController();
        DataTable dt = mController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(DrpDistributor, dt, 0, 2, true);
    }


    /// <summary>
    /// Loads Towns To Town Combo, Routes To Routes Comb And Markets To Market Combo
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        this.SetTableSorter();
    }


  
    protected void DrpRoute_SelectedIndexChanged(object sender, EventArgs e)
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

        this.Session.Add("CustomerId", long.Parse(Grid_users.Rows[e.NewEditIndex].Cells[0].Text));

        DrpDistributor.SelectedValue = Grid_users.Rows[e.NewEditIndex].Cells[1].Text;
        // DrpBusinessType.SelectedValue = Grid_users.Rows[e.NewEditIndex].Cells[2].Text;
        DrpVolumeClass.SelectedValue = Grid_users.Rows[e.NewEditIndex].Cells[3].Text;
     
        txtCustomerName.Text = Grid_users.Rows[e.NewEditIndex].Cells[9].Text.Replace("amp;", "");
        //  txtContactPerson.Text = Grid_users.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");
        txtPhoneNo.Text = Grid_users.Rows[e.NewEditIndex].Cells[11].Text.Replace("&nbsp;", "");
        txtEmailAddress.Text = Grid_users.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
        txtAddress.Text = Grid_users.Rows[e.NewEditIndex].Cells[13].Text.Replace("&nbsp;", "");
        // txtIsRegister.Text = Grid_users.Rows[e.NewEditIndex].Cells[14].Text.Replace("&nbsp;", "");
        if (Grid_users.Rows[e.NewEditIndex].Cells[14].Text.Trim() == "&nbsp;")
        {
            //   txtIsRegister.Text = "";
            //  ChbIsRegister.Checked = false;
        }
        else
        {
            //  ChbIsRegister.Checked = true;
            // txtIsRegister.Text = Grid_users.Rows[e.NewEditIndex].Cells[14].Text;
        }
        chkIsActive.Checked = bool.Parse(Grid_users.Rows[e.NewEditIndex].Cells[19].Text);
        txtRegdate.Text = (DateTime.Parse(Grid_users.Rows[e.NewEditIndex].Cells[20].Text.Replace("&nbsp;", CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy")))).ToString("dd-MMM-yyyy");
        txtCNIC.Text = Grid_users.Rows[e.NewEditIndex].Cells[23].Text.Replace("&nbsp;", "");
        //  txtNTN.Text = Grid_users.Rows[e.NewEditIndex].Cells[24].Text.Replace("&nbsp;", "");
        txtCreditLimit.Text = Grid_users.Rows[e.NewEditIndex].Cells[24].Text.Replace("&nbsp;", "0");
        btnSave.Text = "Update";
        this.SetTableSorter();
    }

    /// <summary>
    /// Sets PageIndex Of Customer Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        this.LoadCustomer();
        this.SetTableSorter();
    }

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
                    StrCode = "OT0000" + CustomerId.ToString();
                }
                else if (CustomerId.ToString().Length == 2)
                {
                    StrCode = "OT000" + CustomerId.ToString();
                }
                else if (CustomerId.ToString().Length == 3)
                {
                    StrCode = "OT00" + CustomerId.ToString();
                }
                else if (CustomerId.ToString().Length == 4)
                {
                    StrCode = "OT0" + CustomerId.ToString();
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
               
                Response.Redirect("frmOrderPOS.aspx");
            }
        }
        else
        {
            mController.UpdateCustomer(long.Parse(this.Session["CustomerId"].ToString()), false, chkIsActive.Checked,
            Constants.IntNullValue, int.Parse(DrpVolumeClass.SelectedValue.ToString()),
            Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
            Constants.IntNullValue, int.Parse(DrpDistributor.SelectedValue.ToString()), "", "",
            txtPhoneNo.Text, txtEmailAddress.Text, null, txtCustomerName.Text, txtAddress.Text, Constants.DateNullValue, 1, 1, txtCNIC.Text, ""
            , Constants.DecimalNullValue);
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record Update');", true);
        Response.Redirect("frmOrderPOS.aspx");
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
       // this.SetTableSorter();
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
        //  txtContactPerson.Text = "";
        txtAddress.Text = "";
        txtPhoneNo.Text = "";
        txtSeach.Text = "";
        // txtIsRegister.Text = "";
        //  txtNTN.Text = string.Empty;
        txtCNIC.Text = string.Empty;
        btnSave.Text = "Save";


        txtCreditLimit.Text = "";


        Grid_users.DataSource = null;
        Grid_users.DataBind();
        txtRegdate.Text = CORNCommon.Classes.Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

    }
   
   
    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmOrderPOS.aspx");
    }
}
