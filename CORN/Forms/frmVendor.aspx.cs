using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;

public partial class Forms_frmVendor : System.Web.UI.Page
{
    readonly VenderEntryController VendorCtl = new VenderEntryController();

    readonly SkuHierarchyController mController = new SkuHierarchyController();

    public static int VendorId = Constants.IntNullValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadVendor();

            btnSave.Attributes.Add("onclick", "return ValidateForm()");
        }
    }

    #region Basic Information

   
    private void LoadVendor()
    {
       
            DataTable dtVendor = VendorCtl.GetVendor(Constants.IntNullValue,0);

            if (dtVendor != null)
            {
                Session.Add("dtVendor", dtVendor);
            gvVendor.EditIndex = -1;
                gvVendor.DataSource = dtVendor;
                gvVendor.DataBind();
            }
        
    }

  
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtVendor"];

        switch (ddSearchType.SelectedIndex)
        {
            case 1:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 2:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 3:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 4:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            case 5:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                break;
            default:
                dt.DefaultView.RowFilter = "VENDOR_NAME" + " like '%" + "" + "%'";
                break;
        }
        gvVendor.EditIndex = -1;
        gvVendor.DataSource = dt.DefaultView;
        gvVendor.DataBind();
      
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (btnSave.Text == "Save")
        {
            int VendorID = VendorCtl.InsertVendor(txtVendorName.Text,txtAddress.Text,txtemail.Text,txtFax.Text
                ,txtContactPerson.Text, txtPhoneNo.Text, 0);
            if (VendorID > 0)
            {
                ClearAll();
                LoadVendor();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Save successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred.');", true);
            }
        }
        else
        {
            if (VendorCtl.UpdateVendor(VendorId, txtVendorName.Text, txtAddress.Text, txtemail.Text, txtFax.Text
                ,txtContactPerson.Text, txtPhoneNo.Text, 0, chkIsActive.Checked))
            {
                ClearAll();

                LoadVendor();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Update successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occurred.');", true);
            }
        }
    }


    protected void gvVendor_RowEditing(object sender, GridViewEditEventArgs e)
    {

        VendorId = int.Parse(gvVendor.Rows[e.NewEditIndex].Cells[0].Text);
        txtVendorName.Text = gvVendor.Rows[e.NewEditIndex].Cells[1].Text.Replace("&nbsp;", "");
        txtContactPerson.Text = gvVendor.Rows[e.NewEditIndex].Cells[2].Text.Replace("&nbsp;", "");
        txtPhoneNo.Text = gvVendor.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;", ""); 
        txtemail.Text = gvVendor.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
        txtAddress.Text = gvVendor.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
        txtFax.Text = gvVendor.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
        chkIsActive.Checked = bool.Parse(gvVendor.Rows[e.NewEditIndex].Cells[7].Text);
        for (int i = 0; i < gvVendor.Rows.Count; i++)
        {
            gvVendor.Rows[i].Cells[8].Enabled = false;
        }
        btnSave.Text = "Update";

    }
   
    private void ClearAll()
    {
        VendorId = Constants.IntNullValue;
        txtVendorName.Text = "";
        txtContactPerson.Text = ""; 
        txtPhoneNo.Text = ""; 
        txtemail.Text = ""; 
        txtAddress.Text = ""; 
        txtFax.Text = "";
        chkIsActive.Checked = true;

        btnSave.Text = "Save";
    }

    #endregion

    
}