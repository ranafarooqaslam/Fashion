using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Data;

public partial class Forms_masterPOS : MasterPage
{
    DataTable _purchaseSku=new DataTable();
    DataTable _dtFreeSku=new DataTable();

    public event EventHandler ContentCallEvent;
    readonly OrderEntryController _or = new OrderEntryController();

    readonly PhaysicalStockController _pcs = new PhaysicalStockController();
    private int onhold=0;


    //For Sync Model Maintain Stock Register
    private void CalculateStock()
    {
       _pcs.CalculateStockRegister(int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      
       // loadOnHoldInvoicNumber();
      //  CalculateStock();
    }

    #region Load Functions

    private bool FindCustomer()
    {
        if (((CheckBox)(ContentPlaceHolder1.FindControl("cbselectCustomer"))).Checked == true)
        {
            DataTable dtCustomer = (DataTable)this.Session["dtCustomer"];
            DataRow[] foundRows = dtCustomer.Select("CUSTOMER_CODE  = '" + ((TextBox)ContentPlaceHolder1.FindControl("txtOutletCode")).Text.Trim() + "'");
            if (foundRows.Length > 0)
            {
                this.Session.Add("CUSTOMER_ID", long.Parse(foundRows[0]["CUSTOMER_ID"].ToString()));
                return true;
            }
            return false;
        }
        return true;
    }
    public void loadOnHoldInvoicNumber()
    {

        DataTable dtOrder = _or.SelectPendingOrder(int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), 0, 0, 0, 0, Constants.Order_Pending_Id, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Convert.ToDateTime(this.Session["CurrentWorkDate"]));
        lblonHold.Text = Convert.ToString(dtOrder.Rows.Count);
    }
    private void LoadPendingOrder()
    {

        //DropDownList holdlist = (DropDownList)ContentPlaceHolder1.FindControl("ddonHoldList");
        //holdlist.Items.Clear();
        ((DropDownList)ContentPlaceHolder1.FindControl("ddonHoldList")).Items.Clear();
        DataTable dtOrder = _or.SelectPendingOrder(int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), 0, 0, 0, 0, Constants.Order_Pending_Id, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Convert.ToDateTime(this.Session["CurrentWorkDate"]));
        ((DropDownList)ContentPlaceHolder1.FindControl("ddonHoldList")).Items.Add(new clsListItems("Select To Unhold", Constants.LongNullValue.ToString()));
        lblonHold.Text = Convert.ToString(dtOrder.Rows.Count);
        clsWebFormUtil.FillDropDownList((DropDownList)ContentPlaceHolder1.FindControl("ddonHoldList"), dtOrder, 0, 1);
        this.Session.Add("dtOrder", dtOrder);
    }
    private void LoadGird()
    {
        _purchaseSku = (DataTable)this.Session["PurchaseSKU"];
        ((GridView)ContentPlaceHolder1.FindControl("GrdPurchase")).DataSource = _purchaseSku;
        ((GridView)ContentPlaceHolder1.FindControl("GrdPurchase")).DataBind();
    }
    private void LoadFreeGrid()
    {
        _dtFreeSku = (DataTable)this.Session["dtFreeSKU"];
        ((GridView)ContentPlaceHolder1.FindControl("GrdFreeSKU")).DataSource = _dtFreeSku;
        ((GridView)ContentPlaceHolder1.FindControl("GrdFreeSKU")).DataBind();
    }
    private void LoadEditInvoice(int pType)
    {
        OrderEntryController or = new OrderEntryController();
        DropDownList invoiceList = (DropDownList)ContentPlaceHolder1.FindControl("ddEditInvoice");
        invoiceList.Items.Clear();
        DataTable dtDoc = or.GetDocumentNo(Convert.ToDateTime(this.Session["CurrentWorkDate"]), Convert.ToInt32(Session["UserID"]), pType);
        ((DropDownList)ContentPlaceHolder1.FindControl("ddEditInvoice")).Items.Add(new clsListItems("Select TO Edit", Constants.LongNullValue.ToString()));
        clsWebFormUtil.FillDropDownList(((DropDownList)ContentPlaceHolder1.FindControl("ddEditInvoice")), dtDoc, "DocID", "DocNo");
        this.Session.Add("dtDoc", dtDoc);
    }

    #endregion

    #region Click Operation

    protected void btnNewCustomer_Click(object sender, EventArgs e)
    {
        //btnEditinvoice.Visible = false;
        //btnHold.Visible = false;
        //btnUnhold.Visible = false;
        //btnNewCustomer.Visible = false;
        //btnpricelookup.Visible = false;
        //if (((Button)(ContentPlaceHolder1.FindControl("btnToggleMode"))).Text == "SALE MODE")
        //{
        //    this.Session.Add("TogelBtnId", 1);//sale mode
        //}
        //else
        //{
        //    this.Session.Add("TogelBtnId", 2);//refund mode

        //}
         Server.Transfer("frmNewCustomerPOS.aspx");
    }
    protected void btnHold_Click(object sender, EventArgs e)
    {
        _purchaseSku = (DataTable)this.Session["PurchaseSKU"];
        _dtFreeSku = (DataTable)this.Session["dtFreeSKU"];

        if (_purchaseSku.Rows.Count > 0)
        {
            if (FindCustomer())
            {
                string ManualID = null;
                if (((Button)(ContentPlaceHolder1.FindControl("btnToggleMode"))).Text == "SALE MODE")
                {
                    ManualID = "1";//used for sale mode
                }
                else
                {
                    ManualID = "2";
                }
                DataControl DC = new DataControl();
                OrderEntryController mOrderController = new OrderEntryController();
                bool IsValidInsert = mOrderController.Add_Order(int.Parse(DC.chkNull_0(this.Session["DISTRIBUTOR_ID"].ToString())), ManualID, 0, 0, 0, long.Parse(this.Session["CUSTOMER_ID"].ToString()), 0, 0, 0, int.Parse(DC.chkNull_0(((DropDownList)(ContentPlaceHolder1.FindControl("DrpPayMode"))).SelectedValue.ToString())),
                            decimal.Parse(DC.chkNull_0(((TextBox)ContentPlaceHolder1.FindControl("txtGrossAmount")).Text)), decimal.Parse(DC.chkNull_0(((TextBox)ContentPlaceHolder1.FindControl("numtxtTotalExtraDiscnt")).Text)),
                            0, decimal.Parse(DC.chkNull_0(((TextBox)ContentPlaceHolder1.FindControl("numTxtTotalGST")).Text)), decimal.Parse(DC.chkNull_0(((TextBox)ContentPlaceHolder1.FindControl("numTxtTotlAmnt")).Text)),
                            0, Constants.Order_Pending_Id, _purchaseSku, _dtFreeSku, int.Parse(this.Session["UserId"].ToString()), DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), 0, 0);
                if (IsValidInsert)
                {
                    ((DropDownList)ContentPlaceHolder1.FindControl("ddonHoldList")).Visible = false;
                    this.ClearMasterALL();

                    this.Session.Remove("hfBillBookNo");


                    onhold++;


                }
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Nothing To Hold');", true);

        }

        loadOnHoldInvoicNumber();
        ((TextBox)ContentPlaceHolder1.FindControl("txtskuCode")).Focus();
    }
    protected void btnUnhold_Click(object sender, EventArgs e)
    {

        if (lblonHold.Text == "0")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Nothing To Unhold');", true);

        }
        else
        {
            ContentPlaceHolder1.FindControl("ddonHoldList").Visible = true;
            ((Label)(ContentPlaceHolder1.FindControl("lblUnhold"))).Visible = true;
            ((DropDownList)(ContentPlaceHolder1.FindControl("ddEditInvoice"))).Visible = false;
            ((DropDownList)(ContentPlaceHolder1.FindControl("ddsalesForce"))).Visible = false;
            ((Label)(ContentPlaceHolder1.FindControl("lblsaleforce"))).Visible = false;//
            this.LoadPendingOrder();
        }
        ((TextBox)ContentPlaceHolder1.FindControl("txtskuCode")).Focus();
    }
    protected void btnEditinvoice_Click(object sender, EventArgs e) ///Credit Note 
    {
        if (((Button)(ContentPlaceHolder1.FindControl("btnToggleMode"))).Text == "SALE MODE")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Credit Note Works only With Refund Mode');", true);
            return;
        }
        else
            if (((CheckBox)(ContentPlaceHolder1.FindControl("cbselectCustomer"))).Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please Select a customer by checking the customer check box');", true);
                return;
            }
            else if (((CheckBox)(ContentPlaceHolder1.FindControl("cbselectCustomer"))).Checked == true)
            {
                if (((TextBox)(ContentPlaceHolder1.FindControl("txtOutletCode"))).Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Please Select a customer from List');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Add Transaction to Credit Note ?');", true);
                    if (ContentCallEvent != null)
                        ContentCallEvent(this, EventArgs.Empty);

                }

            }

        ((TextBox)ContentPlaceHolder1.FindControl("txtskuCode")).Focus();

    }
    protected void btnpricelookup_Click(object sender, EventArgs e)
    {

        if (((TextBox)(ContentPlaceHolder1.FindControl("numtxtTotalExtraDiscnt"))).Text != "")
        {
            this.Session.Add("DiscountAmount", ((TextBox)(ContentPlaceHolder1.FindControl("numtxtTotalExtraDiscnt"))).Text);
        }
        if (((TextBox)(ContentPlaceHolder1.FindControl("txtAuthorisedBy"))).Text != "")
        {
            this.Session.Add("AuthorizedBy", ((TextBox)(ContentPlaceHolder1.FindControl("txtAuthorisedBy"))).Text);
        }
        if (((TextBox)(ContentPlaceHolder1.FindControl("txtCashReceived"))).Text != "")
        {
            this.Session.Add("CashRecives", ((TextBox)(ContentPlaceHolder1.FindControl("txtCashReceived"))).Text);
        }
        if (((TextBox)(ContentPlaceHolder1.FindControl("txtBalance"))).Text != "")
        {
            this.Session.Add("BalanceAmount", ((TextBox)(ContentPlaceHolder1.FindControl("txtBalance"))).Text);
        }
        if (((Button)(ContentPlaceHolder1.FindControl("btnToggleMode"))).Text == "SALE MODE")
        {
            this.Session.Add("TogelBtnId", 1);//sale mode
        }
        else
        {
            this.Session.Add("TogelBtnId", 2);//refund mode

        }
        string quntity = "-1";
        if (((TextBox)(ContentPlaceHolder1.FindControl("txtQuantity"))).Text == "1")
        {
            quntity = "1";
            this.Session.Add("quantity", quntity);
        }
        else
        {
            quntity = ((TextBox)(ContentPlaceHolder1.FindControl("txtQuantity"))).Text;
            this.Session.Add("quantity", quntity);
        }
       
        if (((DropDownList)(ContentPlaceHolder1.FindControl("DrpDiscount"))).SelectedValue == "0")
        {
            this.Session.Add("DrpDiscount", "0");
        }
        else
        {
             this.Session.Add("DrpDiscount", "1");
        }

        string discount = "0";
        if (((TextBox)(ContentPlaceHolder1.FindControl("txtDiscount"))).Text == "0")
        {
            discount = "0";

            this.Session.Add("discount", discount);
        }
        else
        {
            discount = ((TextBox)(ContentPlaceHolder1.FindControl("txtDiscount"))).Text;
            this.Session.Add("discount", discount);
        }

        if (((DropDownList)(ContentPlaceHolder1.FindControl("DrpPayMode"))).SelectedValue == "214")
        {
            this.Session.Add("paymode", 214);
        }
        else if (((DropDownList)(ContentPlaceHolder1.FindControl("DrpPayMode"))).SelectedValue == "215")
        {
            this.Session.Add("paymode", 215);
        }
        else
        {
            this.Session.Add("paymode", 217);
        }
        if (((DropDownList)(ContentPlaceHolder1.FindControl("ddsalesForce"))).SelectedValue != "")
        {
            this.Session.Add("ddsalesForce", ((DropDownList)(ContentPlaceHolder1.FindControl("ddsalesForce"))).SelectedValue);
        }

        this.Session.Add("CustName", ((TextBox)(ContentPlaceHolder1.FindControl("txtOutletName"))).Text);
        this.Session.Add("CustCode", ((TextBox)(ContentPlaceHolder1.FindControl("txtOutletCode"))).Text);

        Response.Redirect("frmProductSearch.aspx");
    }


    #endregion

    #region Clear

    private void ClearMasterALL()
    {
        this.EnableDisableController(true);
        this.Session.Remove("PurchaseSKU");
        this.Session.Remove("dtFreeSKU");
        ((LinkButton)(ContentPlaceHolder1.FindControl("btnSaveOrder"))).Enabled = false;
        Session.Remove("CustName");
        Session.Remove("CustCode");
        this.CreatTable();
        this.CreateFreeSKU();
         this.LoadGird();
        this.LoadFreeGrid();
        ((TextBox)ContentPlaceHolder1.FindControl("txtGrossAmount")).Text = "";
        ((TextBox)ContentPlaceHolder1.FindControl("numtxtTotalExtraDiscnt")).Text = "";
        ((TextBox)ContentPlaceHolder1.FindControl("numTxtTotalGST")).Text = "";
        ((TextBox)ContentPlaceHolder1.FindControl("numTxtTotlAmnt")).Text = "";
        ((TextBox)ContentPlaceHolder1.FindControl("txtBalance")).Text = "";
        ((TextBox)ContentPlaceHolder1.FindControl("txtCashReceived")).Text = "";

    }

    #endregion

    #region DataTable Creation

    private void CreatTable()
    {
        _purchaseSku = new DataTable();
        _purchaseSku.Columns.Add("SALE_ORDER_DETAIL_ID", typeof(long));
        _purchaseSku.Columns.Add("DistributorId", typeof(int));
        _purchaseSku.Columns.Add("SALE_ORDER_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_Code", typeof(string));
        _purchaseSku.Columns.Add("SKU_Name", typeof(string));
        _purchaseSku.Columns.Add("BATCH_NO", typeof(string));
        _purchaseSku.Columns.Add("UNIT_PRICE", typeof(decimal));
        _purchaseSku.Columns.Add("QUANTITY_UNIT", typeof(int));
        _purchaseSku.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("STANDARD_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("STANDARD_DISCOUNT_PER", typeof(decimal));
        _purchaseSku.Columns.Add("EXTRA_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("RETAIL_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("GST_RATE", typeof(decimal));
        _purchaseSku.Columns.Add("GST_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("TST_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("CLAIM_EXTRA_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("CLAIM_STANDARD_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("CLAIM_PER", typeof(decimal));
        _purchaseSku.Columns.Add("SED_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("NET_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("QUANTITY_CTN", typeof(decimal));
        _purchaseSku.Columns.Add("PACKSIZE", typeof(string));
        _purchaseSku.Columns.Add("IS_DELETED", typeof(bool));
        _purchaseSku.Columns.Add("SkuImage", typeof(string));
        _purchaseSku.Columns.Add("COLOR", typeof(string));
        _purchaseSku.Columns.Add("TRADE_PRICE", typeof(string));
        _purchaseSku.Columns.Add("AMOUNT_TEMP", typeof(int));
        _purchaseSku.Columns.Add("CHECK_DELETE", typeof(int));
        this.Session.Add("PurchaseSKU", _purchaseSku);
    }
    private void CreateFreeSKU()
    {
        _dtFreeSku = new DataTable();
        _dtFreeSku.Columns.Add("SKU_ID", typeof(int));
        _dtFreeSku.Columns.Add("SKU_Code", typeof(string));
        _dtFreeSku.Columns.Add("SKU_Name", typeof(string));
        _dtFreeSku.Columns.Add("UNIT_PRICE", typeof(decimal));
        _dtFreeSku.Columns.Add("Quantity", typeof(int));
        _dtFreeSku.Columns.Add("AMOUNT", typeof(decimal));
        _dtFreeSku.Columns.Add("GST_RATE", typeof(decimal));
        _dtFreeSku.Columns.Add("GST_AMOUNT", typeof(decimal));
        _dtFreeSku.Columns.Add("TST_AMOUNT", typeof(decimal));
        _dtFreeSku.Columns.Add("PROMOTION_ID", typeof(int));
        _dtFreeSku.Columns.Add("BASKET_ID", typeof(int));
        _dtFreeSku.Columns.Add("BASKET_DETAIL_ID", typeof(int));
        _dtFreeSku.Columns.Add("PROMOTION_OFFER_ID", typeof(int));
        this.Session.Add("dtFreeSKU", _dtFreeSku);
    }

    #endregion

    private void EnableDisableController(bool CValue)
    {

        if (CValue == true)
        {
            if (((TextBox)ContentPlaceHolder1.FindControl("txtGrossAmount")).Text != "")
            {
                if (decimal.Parse(((TextBox)ContentPlaceHolder1.FindControl("txtGrossAmount")).Text) > 0)
                {
                    ((TextBox)ContentPlaceHolder1.FindControl("txtOutletCode")).Enabled = true;
                    ((TextBox)ContentPlaceHolder1.FindControl("txtOutletName")).Enabled = true;
                }
            }
            else
            {
                ((TextBox)ContentPlaceHolder1.FindControl(" txtOutletCode")).Enabled = false;
                 ((TextBox)ContentPlaceHolder1.FindControl("txtOutletName")).Enabled = false;
            }
        }
    }

   
}
