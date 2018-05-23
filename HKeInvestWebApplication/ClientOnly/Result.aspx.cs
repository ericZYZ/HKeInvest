using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class Result : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string result = Request.QueryString["result"];
                if (!string.IsNullOrWhiteSpace(result))
                {
                    btnBack.Visible = true;
                    btnBack1.Visible = true;
                    title.InnerText = result;
                }
                else
                {
                    title.InnerText = ".";
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("BuySecurities.aspx");
        }

        protected void btnBack1_Click(object sender, EventArgs e)
        {
            Response.Redirect("SellSecurities.aspx");
        }
    }
}