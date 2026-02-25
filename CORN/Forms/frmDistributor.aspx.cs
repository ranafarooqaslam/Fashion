using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.IO;
using System.Web;
public partial class Forms_frmDistributor : System.Web.UI.Page
{
    DistributorController mController = new DistributorController();
    private static int DistributorId;
    private static int CompanyId;

    /// <summary>
    /// Page_Load Function Populates All Combos and Grids On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.GetDistributorType();
            this.LoadGird();
            this.LoadCompany();
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
        }
    }

    /// <summary>
    /// Loads Companies To Comapny Combo
    /// </summary>
    private void LoadCompany()
    {
        CompanyController mCompany = new CompanyController();
        DataTable dt = mCompany.SelectCompany(Constants.IntNullValue, Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(DrpCompanyName, dt, 0, 1, true);
    }

    /// <summary>
    /// Loads Location Types To LocationType Combo
    /// </summary>
    private void GetDistributorType()
    {
        DataTable dt = mController.SelectDistributorTypeInfo(Constants.IntNullValue);
        clsWebFormUtil.FillDropDownList(ddDistributorType, dt, 0, 2);
    }

    /// <summary>
    /// Loads Locations To Location Grid
    /// </summary>
    private void LoadGird()
    {
        if (ddDistributorType.Items.Count > 0)
        {
            DataTable dtDistributor = mController.SelectAllDistributors(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue);

            switch (ddSearchType.SelectedIndex)
            {
                case 1:
                    dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 2:
                    dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 3:
                    dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 4:
                    dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                case 5:
                    dtDistributor.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'";
                    break;
                default:
                    dtDistributor.DefaultView.RowFilter = "Distributor_name" + " like '%" + "" + "%'";
                    break;
            }
          
            GridDistributor.DataSource = dtDistributor;
            GridDistributor.Columns[0].Visible = true;
            GridDistributor.Columns[1].Visible = true;
            GridDistributor.Columns[2].Visible = true;
            GridDistributor.Columns[3].Visible = true;
            GridDistributor.Columns[11].Visible = true;
            GridDistributor.EditIndex = -1;
            GridDistributor.DataBind();
            GridDistributor.Columns[0].Visible = false;
            GridDistributor.Columns[1].Visible = false;
            GridDistributor.Columns[2].Visible = false;
            GridDistributor.Columns[3].Visible = false;
            GridDistributor.Columns[11].Visible = false;
           
        }

    }

    /// <summary>
    /// Sets/UnSets Focus To GST No. TextBox
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void cbRegistered_CheckedChanged(object sender, EventArgs e)
    {
        if (cbRegistered.Checked == true)
        {
            txtgstno.Enabled = true;
            txtgstno.Focus();
        }
        else
        {
            txtgstno.Text = "";
            txtgstno.Enabled = false;
        }
    }

    /// <summary>
    /// Saves Or Updates A Location
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string fileName = null;
        //if (fuPic.HasFile)
        //{
        //    Session["haspic"] = 1;
        //    string path = Server.MapPath("~/Pics");
        //    string fExtension = "";
        //    FileInfo oFileInfo = new FileInfo(fuPic.PostedFile.FileName);
        //    fExtension = Path.GetExtension(fuPic.FileName);
        //    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "pic" + fExtension;
        //    string fullFileName = path + "\\" + fileName;
        //    Session.Add("Pic", fileName);
        //    hfPic.Value = fileName;
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    fuPic.PostedFile.SaveAs(fullFileName);
        //}
        try
        {
            if (fuPic.HasFile)
            {
                fileName = Path.GetExtension(fuPic.FileName);
            }

        }
        catch (Exception rre) { }
        if (btnSave.Text == "Save")
        {
          string DistributorId=  mController.InsertDistributor(int.Parse(DrpCompanyName.SelectedValue.ToString()), Constants.IntNullValue, !chkIsActive.Checked, System.DateTime.Now, System.DateTime.Now, Constants.IntNullValue, Constants.IntNullValue
                , Constants.IntNullValue, Constants.IntNullValue, int.Parse(ddDistributorType.SelectedValue.ToString()), txtcontactperson.Text, txtPhoneNo.Text, txtgstno.Text,
                txtpassword.Text, txtAddress1.Text, txtAddress2.Text, txtDistributorCode.Text, txtDistributorName.Text, null, cbRegistered.Checked, 1, int.Parse(this.Session["UserId"].ToString()),fileName,chbPromotionON.Checked);
            Upload_image(DistributorId, "");
        }
        else
        {
            mController.UpdateDistributor(int.Parse(DrpCompanyName.SelectedValue.ToString()), Constants.IntNullValue, !chkIsActive.Checked, Constants.DateNullValue, System.DateTime.Now, Constants.IntNullValue, Constants.IntNullValue
            , Constants.IntNullValue, Constants.IntNullValue, int.Parse(ddDistributorType.SelectedValue.ToString()), txtcontactperson.Text, txtPhoneNo.Text, txtgstno.Text,
            txtpassword.Text, txtAddress1.Text, txtAddress2.Text, DistributorId, txtDistributorCode.Text, txtDistributorName.Text, null, cbRegistered.Checked, 1, int.Parse(this.Session["UserId"].ToString()),fileName, chbPromotionON.Checked);
            Upload_image(DistributorId.ToString(), "");
        }
        lblErrorMsg.Text = "";
        ClearAll();
        this.LoadGird();
    }
    protected void Upload_image(string distributorId, string updateid)
    {
        string msg = "";
        string fExtension = "";
        if (fuPic.HasFile)
        {
            // if (fuImageSku.PostedFile.ContentType == "image/jpeg")

            {
                if (fuPic.PostedFile.ContentLength < 11102400)
                {
                    try
                    {
                        string filename = Path.GetFileName(fuPic.FileName);
                        fExtension = Path.GetExtension(fuPic.FileName);
                        fuPic.SaveAs(Server.MapPath("~/Pics/") + distributorId + fExtension);
                        //fuImageSku.Text = "Upload status: File uploaded!";
                        // imgSKU.ImageUrl = Server.MapPath("~/SkuImages/") + skuid + ".jpeg";
                    }
                    catch (Exception ex)
                    {
                        msg = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                    }
                }
                else
                {
                    msg = "Upload status: The file has to be less than 100 kb!";
                }

            }
            //else
            //{
            //    msg = "Upload status: Only JPEG files are accepted!";
            //}

        }
        else
        {

            // FileInfo TheFile = new FileInfo(MapPath("~/SkuImages/temp.jpeg"));
            if (updateid != "")
            {
                try
                {
                    FileInfo editfile = new FileInfo(MapPath("~/Pics/" + updateid + ".jpeg"));
                    if (editfile.Exists)
                    {
                        File.Delete(MapPath("~/Pics/" + updateid + ".jpeg"));

                    }
                }
                catch (Exception e)
                {
                    string sss = e.StackTrace;
                }
            }
            // File.Copy(MapPath("~/SkuImages/temp.jpeg"), MapPath("~/SkuImages/" + skuid + ".jpeg"));
            //if (TheFile.Exists)
            //{
            //    File.Delete(MapPath("~/SkuImages/temp.jpeg"));

            //} 
        }

    }
    /// <summary>
    /// Clears All The Fields Through ClearAll() Function.
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    /// <summary>
    /// Loads Locations To Location Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        this.LoadGird();
    }

    /// <summary>
    /// Sets PageIndex Of Location Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void GridDistributor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GridDistributor.PageIndex = e.NewPageIndex;
        this.LoadGird();
    }

    /// <summary>
    /// Actives/DeActives A Location.
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GridDistributor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DistributorId = int.Parse(GridDistributor.Rows[e.RowIndex].Cells[0].Text);
        CompanyId = int.Parse(GridDistributor.Rows[e.RowIndex].Cells[11].Text);
        bool IsRegister = bool.Parse(GridDistributor.Rows[e.RowIndex].Cells[1].Text);
        mController.UpdateDistributor(CompanyId, Constants.IntNullValue, true, Constants.DateNullValue, System.DateTime.Now, Constants.IntNullValue, Constants.IntNullValue
               , Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, null, null, null, null, null, null, DistributorId, null,
               null, null, true, 1, int.Parse(this.Session["UserId"].ToString()),null,true);
        this.LoadGird();
    }

    /// <summary>
    /// Sets Location Data For Edit. This Function Runs When An Existing Location Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GridDistributor_RowEditing(object sender, GridViewEditEventArgs e)
    {
       // Session.Add("Pic", "");
        //hfPic.Value = "";
        DistributorId = int.Parse(GridDistributor.Rows[e.NewEditIndex].Cells[0].Text);
        DrpCompanyName.SelectedValue = GridDistributor.Rows[e.NewEditIndex].Cells[11].Text;
        cbRegistered.Checked = bool.Parse(GridDistributor.Rows[e.NewEditIndex].Cells[1].Text);
        ddDistributorType.SelectedValue = GridDistributor.Rows[e.NewEditIndex].Cells[2].Text;
        txtAddress2.Text = GridDistributor.Rows[e.NewEditIndex].Cells[3].Text;
        txtDistributorCode.Text = GridDistributor.Rows[e.NewEditIndex].Cells[4].Text;
        txtDistributorName.Text = GridDistributor.Rows[e.NewEditIndex].Cells[5].Text;
        txtAddress1.Text = GridDistributor.Rows[e.NewEditIndex].Cells[7].Text;
        txtcontactperson.Text = GridDistributor.Rows[e.NewEditIndex].Cells[8].Text;
        txtPhoneNo.Text = GridDistributor.Rows[e.NewEditIndex].Cells[9].Text;
        if (GridDistributor.Rows[e.NewEditIndex].Cells[13].Text == "Active")
        {
            chkIsActive.Checked = true;
        }
        else
        {
            chkIsActive.Checked = false;
        }
        if (GridDistributor.Rows[e.NewEditIndex].Cells[10].Text == "&nbsp;")
        {
            txtgstno.Text = "";
        }
        else
        {
            txtgstno.Text = GridDistributor.Rows[e.NewEditIndex].Cells[10].Text;
        }
        chbPromotionON.Checked = bool.Parse(GridDistributor.Rows[e.NewEditIndex].Cells[14].Text);
        // hfPic.Value = GridDistributor.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
        string picExtension= GridDistributor.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");


        try
        {
            imgSKU.ImageUrl = "~/Pics/" + DistributorId + picExtension;
        }
        catch (Exception ee)
        {
            string ex = ee.Message;
        }
        for (int i = 0; i < GridDistributor.Rows.Count; i++)
        {
            GridDistributor.Rows[i].Cells[15].Enabled = false;
        }
        btnSave.Text = "Update";

    }    

    /// <summary>
    /// Clears All The Fields.
    /// </summary>
    private void ClearAll()
    {
        txtDistributorName.Text = "";
        txtDistributorCode.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtcontactperson.Text = "";
        txtPhoneNo.Text = "";
        txtpassword.Text = "";
        txtgstno.Text = ""; 
        btnSave.Text = "Save";
        cbRegistered.Checked = false;
        for (int i = 0; i < GridDistributor.Rows.Count; i++)
        {
            GridDistributor.Rows[i].Cells[14].Enabled = true;
        }
        try
        {
            imgSKU.ImageUrl = "../images/no-image.jpg";
        }
        catch (Exception ee)
        {
            string ex = ee.Message;
        }
    }
}
