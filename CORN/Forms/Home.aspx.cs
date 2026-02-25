using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (this.Session["PurchaseSKU"] != null)
                {
                    Session.Remove("PurchaseSKU");
                }

                if (this.Session["dtFreeSKU"] != null)
                {
                    Session.Remove("dtFreeSKU");
                }
                if (this.Session["CustName"] != null)
                {
                    Session.Remove("CustName");
                }
                if (this.Session["CustCode"] != null)
                {
                    Session.Remove("CustCode");
                }

            }
            catch (Exception ex)
            {
            }
            
            if (Session["Message"] != null)
            {
                lblMessage.Text = Session["Message"].ToString();
                Session["Message"] = null;
            }
        }
    }
}