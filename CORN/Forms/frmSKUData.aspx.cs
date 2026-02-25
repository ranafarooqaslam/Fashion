using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.IO;

/// <summary>
/// From To Add, Edit, Delete SKU
/// </summary>
public partial class Forms_frmSKUData : Page
{
    readonly SkuController _mController = new SkuController();
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    private  DataTable _mDt,_mSkuDt;
    private string FileExtension;
    private string image_file_path;

    private bool CheckDublicateSku()
    {
        _mSkuDt = (DataTable)Session["m_SKUDt"];

        DataRow[] foundRows = _mSkuDt.Select("SKU_CODE  = '" + txtbarcode.Text + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }
        
            return false;
        
    }
    private bool CheckDublicateSku2()
    {
        _mSkuDt = (DataTable)Session["m_SKUDt"];

        var foundRows = _mSkuDt.Select("SKU_CODE ='" + txtbarcode.Text + "' AND SKU_ID <> '" + hfSkuId.Value + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }
        return false;
    }
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            LoadSKUCompany();
            LoadSKUDivisions();
            LoadSKUCategory();
            LoadSKUSubCategory();
            LoadSKUBrand();
            LoadTag();
            LoadData();
          
            LoadGrid();
            LoadCountry();
            Page.Form.Enctype = "multipart/form-data";
        }
    }

    private void LoadSKUCompany()
    {
        _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUPrincipal, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(ddskuPrincipal, _mDt, 0, 3, true);
    }
    protected void ddskuPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
     //LoadSKUDivisions();
      //  LoadSKUCategory();
      //  LoadSKUBrand();
       // LoadTag();
      //  LoadData();
      //  LoadGrid();
      
    }

    private void LoadSKUDivisions()
    {
       
        // /   if (ddskuPrincipal.Items.Count > 0)/
            {
                _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUDivision, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()));
                clsWebFormUtil.FillDropDownList(ddskudivision, _mDt, 0, 3, true);
            }
        
        //else
       // {
       //     ddskudivision.Items.Clear();   
      //  }
    }
    protected void ddskudivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadSKUCategory();
       // LoadSKUBrand();
      //  LoadTag();
       // LoadData();
       // LoadGrid();
        
    }

    private void LoadSKUCategory()
    {
        _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(ddskucategory, _mDt, 0, 3, true);
    }
    protected void ddskucategory_SelectedIndexChanged(object sender, EventArgs e)
    {
       //LoadSKUBrand();
      // LoadTag();
      //  LoadData();
      //  LoadGrid();
        LoadSKUSubCategory();

    }

    private void LoadSKUSubCategory()
    {
        ddskuSubCategory.Items.Clear(); 
        if (ddskucategory.Items.Count   > 0)
        {
            _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUSubCategory, Constants.IntNullValue, int.Parse(ddskucategory.SelectedValue), null, null, true, int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(ddskuSubCategory, _mDt, 0, 3, true);
        }
    }

    private void LoadSKUBrand()
    {

            //if (ddskucategory.Items.Count   > 0)
            {
                _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUBrand, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()));
                clsWebFormUtil.FillDropDownList(ddskuBrand, _mDt, 0, 3, true);
            }
        
    }
    protected void ddskubrand_selectedIndexChanged(object sender, EventArgs e)
    {
        //  LoadTag();
        //  LoadData();

    }

    private void LoadTag()
    {
       
        // if (ddskuBrand.Items.Count > 0)
        {
            DataTable dt = _mHerController.SelectSkuHierarchy(Constants.SKUTAG, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(ddskuTag, dt, 0, 3, true);
        }
        //else 
        //{
        //    ddskuTag.Items.Clear();
        //}
    }
    protected void ddskuTag_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadData();

    }

    protected void LoadCountry()
    {

        DataTable dt = _mController.SelectSkuCountry();
        clsWebFormUtil.FillDropDownList(drpCOuntry, dt, 0, 1, true);


    }
    
    private void LoadData()
    {

        _mSkuDt = _mController.SelectSkuInfo2(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()), Constants.IntNullValue);
        Session.Add("m_SKUDt", _mSkuDt);
    }
    private void LoadGrid()
    {
        _mSkuDt = (DataTable)Session["m_SKUDt"];

        switch (ddSearchType.SelectedIndex)
        {
            
            case 1:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            case 2:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            case 3:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            case 4:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            case 5:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            case 6:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            case 7:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            case 8:
                _mSkuDt.DefaultView.RowFilter = ddSearchType.SelectedValue + " like '%" + txtSeach.Text + "%'";
                break;
            default:
                _mSkuDt.DefaultView.RowFilter = "SKU_CODE" + " like '%" + "" + "%'";
                break; 
        }
        grdSKUData.EditIndex = -1;
        grdSKUData.DataSource = _mSkuDt.DefaultView;   
        grdSKUData.DataBind();
       }

    protected void grdSKUData_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            try
            {
                ddskuPrincipal.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[0].Text;
            }
            catch (Exception)
            {
            }
            try
            {
                LoadSKUDivisions();
            }
            catch (Exception)
            {
            }
            try
            {
                ddskudivision.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[1].Text;
            }
            catch (Exception)
            {
            }
            try
            {
                LoadSKUCategory();
            }
            catch (Exception)
            {
            }
            try
            {   
                ddskucategory.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[2].Text;                
            }
            catch (Exception)
            {
            }
            try
            {
                LoadSKUSubCategory();
            }
            catch (Exception)
            {
            }
            if (grdSKUData.Rows[e.NewEditIndex].Cells[2].Text != "0")
            {
                ddskuSubCategory.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[21].Text;
            }
            try
            {
                LoadSKUBrand();
            }
            catch (Exception)
            {
            }
            try
            {
                ddskuBrand.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[3].Text;
            }
            catch (Exception)
            {
            }
            try
            {
                ddskuTag.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[4].Text;
            }
            catch (Exception ex)
            {
            }
            hfSkuId.Value = grdSKUData.Rows[e.NewEditIndex].Cells[5].Text;
            txtskucode.Text = grdSKUData.Rows[e.NewEditIndex].Cells[12].Text;
            txtbarcode.Text = grdSKUData.Rows[e.NewEditIndex].Cells[11].Text;
            txtskuname.Text = grdSKUData.Rows[e.NewEditIndex].Cells[13].Text;
            txtpacksize.Text = grdSKUData.Rows[e.NewEditIndex].Cells[14].Text;
            txtcolor.Text = grdSKUData.Rows[e.NewEditIndex].Cells[15].Text;
            try
            {
                DrpSKUTaxType.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[16].Text;
            }
            catch (Exception)
            {
            }
            try
            {
                drpCOuntry.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[17].Text;
            }
            catch (Exception)
            {
            }
            try
            {
                drpSeason.SelectedValue = grdSKUData.Rows[e.NewEditIndex].Cells[18].Text;
            }
            catch (Exception)
            {
            }
            txtSKU.Text = grdSKUData.Rows[e.NewEditIndex].Cells[19].Text.Replace("&nbsp;", "");
            txtYear.Text = grdSKUData.Rows[e.NewEditIndex].Cells[20].Text.Replace("&nbsp;", "");
            string extension = grdSKUData.Rows[e.NewEditIndex].Cells[22].Text.Replace("&nbsp;", "");
            FileExtension = extension;
            int showONpos = int.Parse(grdSKUData.Rows[e.NewEditIndex].Cells[23].Text.Replace("&nbsp;", ""));
            if (showONpos == 1)
            {
                chbSHowOnPOS.Checked = true;
            }
            else { chbSHowOnPOS.Checked = false; }
            imgSKU.Visible = true;
            imgSKU.ImageUrl = "~/SkuImages/" + hfSkuId.Value + extension;


            if (!string.IsNullOrEmpty(Server.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[24].Text)))
            {
                drpMaterial.SelectedIndex = drpMaterial.Items.IndexOf(
                    drpMaterial.Items.FindByText(
                        Server.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[24].Text)));
            }
            if (!string.IsNullOrEmpty(Server.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[25].Text)))
            {
                drpFit.SelectedIndex = drpFit.Items.IndexOf(
                drpFit.Items.FindByText(
                    Server.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[25].Text)));
            }

            txtWeight.Text = Server.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[26].Text);
            txtKarat.Text = Server.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[27].Text);
            txtMakeCharge.Text = Server.HtmlDecode(grdSKUData.Rows[e.NewEditIndex].Cells[28].Text);
        }
        catch (Exception ee)
        {
            string ex = ee.Message;
        }

        for (int i = 0; i < grdSKUData.Rows.Count; i++)
        {
            grdSKUData.Rows[i].Cells[29].Enabled = false;
            grdSKUData.Rows[i].Cells[30].Enabled = false;
        }
        btnSave.Text = "Update";
    }
    protected void grdSKUData_RowDeleting(object sender, GridViewDeleteEventArgs e)
   {

      // bool IsExemted = bool.Parse(grdSKUData.Rows[e.RowIndex].Cells[16].Text);
       string result = _mController.UpdateSKUS(false, false, Constants.CharNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue,
       Constants.DecimalNullValue, Constants.DecimalNullValue, Constants.ShortNullValue, int.Parse(grdSKUData.Rows[e.RowIndex].Cells[5].Text), null, null, null, null, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
       LoadData();
       LoadGrid();
   }

    protected void btnSave_Click(object sender, EventArgs e)
    {
       const bool isExemted = true;
        string fExtension = "";
       char gstOn = 'E';
       if (txtpacksize.Text.Length <= 0)
       {
           lblErrorMsg.Text = "Must Enter SKU Packsize";
           return; 
       }
       if (txtskucode.Text.Length <= 0)
       {
           lblErrorMsg.Text = "Must Enter SKU Code";
           return;
       }
       if (txtskuname.Text.Length <= 0)
       {
           lblErrorMsg.Text = "Must Enter SKU Name";
           return;
       }
        try
        {
            if (fuImageSku.HasFile)
            {
                fExtension = Path.GetExtension(fuImageSku.FileName);
                FileExtension = fExtension;
            }
            

        }
        catch (Exception rre) { }

        int showOnPos = 0;
        if (chbSHowOnPOS.Checked==true)
        {
            showOnPos = 1;
        }
        if (btnSave.Text == "Save")
           {
                if (CheckDublicateSku())
            {

                string skuid =_mController.InsertSKUS2(isExemted, true,
                   char.Parse(DrpSKUTaxType.SelectedValue.ToString()),
                   int.Parse(ddskuPrincipal.SelectedValue.ToString()), int.Parse(ddskudivision.SelectedValue.ToString()),
                   int.Parse(ddskucategory.SelectedValue.ToString()), int.Parse(ddskuSubCategory.SelectedValue.ToString()),
                   int.Parse(ddskuBrand.SelectedValue.ToString()), Constants.IntNullValue, 0, 0,
                   (txtcolor.Text).ToString(),
                   txtbarcode.Text.ToUpper(), txtskuname.Text, fExtension, txtpacksize.Text,
                   int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()),
                   txtskucode.Text, txtcolor.Text, int.Parse(ddskuTag.SelectedValue), drpCOuntry.SelectedValue,
                   drpSeason.SelectedValue, txtYear.Text, txtSKU.Text,showOnPos,drpMaterial.SelectedItem.Text,
                   drpFit.SelectedItem.Text, txtWeight.Text, txtKarat.Text, txtMakeCharge.Text);
                Upload_image(skuid, "");
                CLearAll();
                
                //  imgSKU.Visible = false;

                LoadData(); 
           }
                 else
       {
           ScriptManager.RegisterStartupScript(this, GetType(), "msg",
               "alert('SKU already exist of that Barcode.');", true);
       }
       }
       else if (btnSave.Text == "Update")
       {
           if (CheckDublicateSku2())
            {
                Upload_image(hfSkuId.Value, hfSkuId.Value);
                _mController.UpdateSKUS2(isExemted, true, char.Parse(DrpSKUTaxType.SelectedValue.ToString()),
                   int.Parse(ddskuPrincipal.SelectedValue.ToString()), int.Parse(ddskudivision.SelectedValue.ToString()),
                   int.Parse(ddskucategory.SelectedValue.ToString()), int.Parse(ddskuSubCategory.SelectedValue.ToString()), 
                   int.Parse(ddskuBrand.SelectedValue.ToString()),Constants.IntNullValue, 0
                   , 0, null,Convert.ToInt32(hfSkuId.Value), txtbarcode.Text.ToUpper(), txtskuname.Text, FileExtension, txtpacksize.Text,
                   int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()),
                   txtskucode.Text, txtcolor.Text, int.Parse(ddskuTag.SelectedValue), drpCOuntry.SelectedValue,
                   drpSeason.SelectedValue, txtYear.Text, txtSKU.Text,showOnPos, drpMaterial.SelectedItem.Text,
                   drpFit.SelectedItem.Text, txtWeight.Text, txtKarat.Text, txtMakeCharge.Text);

               CLearAll();
              // imgSKU.Visible = false;
              
               LoadData(); 
           }
           else
           {
               ScriptManager.RegisterStartupScript(this, GetType(), "msg",
               "alert('SKU already exist of that Barcode.');", true);
           }
       }
    
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }
 
    
    protected void Upload_image(string skuid,string updateid)
    {
        string msg = "";
        string fExtension = "";
        if (fuImageSku.HasFile)
        {
            // if (fuImageSku.PostedFile.ContentType == "image/jpeg")
           
            {
                if (fuImageSku.PostedFile.ContentLength < 11102400)
                {
                    try
                    {
                        string filename = Path.GetFileName(fuImageSku.FileName);
                         fExtension = Path.GetExtension(fuImageSku.FileName);
                        fuImageSku.SaveAs(Server.MapPath("~/SkuImages/") + skuid + fExtension);
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
                    FileInfo editfile = new FileInfo(MapPath("~/SkuImages/" + updateid + ".jpeg"));
                    if (editfile.Exists)
                    {
                        File.Delete(MapPath("~/SkuImages/" + updateid + ".jpeg"));

                    }
                }
                catch (Exception e)
                {
                    string sss=e.StackTrace;
                }
            }
           // File.Copy(MapPath("~/SkuImages/temp.jpeg"), MapPath("~/SkuImages/" + skuid + ".jpeg"));
            //if (TheFile.Exists)
            //{
            //    File.Delete(MapPath("~/SkuImages/temp.jpeg"));
                
            //} 
        }
       
    }
    //protected void btnUploadImage_Click(object sender, EventArgs e)
    //{
    //    //File.Delete(MapPath("~/SkuImages/temp.jpeg"));
    //    string filename = Path.GetFileName(fuImageSku.FileName);
    //    if (filename != "")
    //    {
    //         fuImageSku.SaveAs(Server.MapPath("~/SkuImages/") + "temp" + ".jpeg");
    //        //fuImageSku.Text = "Upload status: File uploaded!";
    //       // imgSKU.Visible = true;
    //        imgSKU.ImageUrl = "~/SkuImages/" + "temp.jpeg";

    //    }
       
    //}

    private void CLearAll()
    {
        txtpacksize.Text = "";
        txtskucode.Text = "";
        txtcolor.Text = "";
        txtskuname.Text = "";
        txtbarcode.Text = "";
        txtSKU.Text = "";
        txtYear.Text = "";
        txtMakeCharge.Text = "";
        txtWeight.Text = "";
        txtKarat.Text = "";

        btnSave.Text = "Save";
        try
        {
            imgSKU.ImageUrl = "../images/no-image.jpg";
        }
        catch (Exception ee)
        {
            string ex = ee.Message;
        }
        LoadData();
        LoadGrid();

    }
   
}
