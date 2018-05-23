using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;
using Microsoft.AspNet.Identity;
using System.Data;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class SellSecurities : System.Web.UI.Page
    {
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        HKeInvestData myHKeInvestData = new HKeInvestData();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userName = Context.User.Identity.GetUserName();
                string accountNumber = myHKeInvestCode.getUserAccountNumber(Session, userName);
                if (!string.IsNullOrWhiteSpace(accountNumber))
                {
                    lblAccountNumber.Text += accountNumber;
                }
            }
        }

        protected void Confirm_Click(object sender, EventArgs e)
        {
            if (ddlSecurityType.SelectedValue != "0")
            {
                string accountNumber = myHKeInvestCode.getUserAccountNumber(Session, Context.User.Identity.GetUserName());
                string securityType = ddlSecurityType.SelectedValue;
                ViewState["type"] = ddlSecurityType.SelectedValue;

                string sql = "SELECT type, code, name, shares, base, 0.00 as price, 0.00 as value FROM [SecurityHolding] WHERE accountNumber='" + accountNumber + "' and type='" + securityType + "'";
                DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
                if (dtSecurityHolding == null) { return; } // If the DataSet is null, a SQL error occurred.

                int dtRow = 0;
                foreach (DataRow row in dtSecurityHolding.Rows)
                {
                    string securityCode = row["code"].ToString();
                    decimal shares = Convert.ToDecimal(row["shares"]);
                    decimal price = myExternalFunctions.getSecuritiesPrice(securityType, securityCode);
                    decimal value = Math.Round(shares * price - (decimal).005, 2);
                    dtSecurityHolding.Rows[dtRow]["price"] = price;
                    dtSecurityHolding.Rows[dtRow]["value"] = value;
                    dtRow = dtRow + 1;
                }

                ErrorMessage.Text = "";
                SecurityInformation.Visible = true;
                gvSecurityInformation.DataSource = dtSecurityHolding;
                gvSecurityInformation.DataBind();
                StockSell.Visible = false;
                NormalSell.Visible = false;
            }
            else
            {
                ErrorMessage.Text = "please select a security type";
                SecurityInformation.Visible = false;
                NormalSell.Visible = false;
                StockSell.Visible = false;
                return;
            }
        }

        protected void Sell_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            if ((string)ViewState["type"] == "bond")
            {
                string code = (string)ViewState["code"];
                string shares = txtShares.Text.Trim();
                string referenceNumber = myExternalFunctions.submitBondSellOrder(code, shares);
                if (referenceNumber == null)
                {
                    // order failed to submit
                    // internal error
                    Response.Redirect("Result.aspx?result=Oops..An internal error occured");
                    return;
                }
                string accountNumber = myHKeInvestCode.getUserAccountNumber(Session, Context.User.Identity.GetUserName());
                string name = ((string)ViewState["name"]).Trim();
                myHKeInvestCode.createNewOrder(accountNumber, referenceNumber, name, code);
                myHKeInvestCode.createBondSellOrder(referenceNumber, shares);
            }
            else if ((string)ViewState["type"] == "stock")
            {
                string code = (string)ViewState["code"];
                string shares = txtQuantity.Text.Trim();
                string orderType = ddlStockType.SelectedValue;
                string expiryDate = ExpiryDate.Text.Trim();
                string allOrNone = AllOrNone.Checked ? "Y" : "N";
                string lowPrice = txtLowPrice.Text.Trim();
                string stopPrice = txtStopPrice.Text.Trim();

                if (orderType == "stop limit" && !validRelation(lowPrice, stopPrice))
                {
                    // HACK: manually set the validator's IsValid property
                    // without calling Validate() method to set Page.IsValid property
                    return;
                }

                string referenceNumber = myExternalFunctions.submitStockSellOrder(code, shares, orderType, expiryDate, allOrNone, lowPrice, stopPrice);
                if (referenceNumber == null)
                {
                    // order failed to submit
                    // internal error
                    Response.Redirect("Result.aspx?result=Oops..An internal error occured");
                    return;
                }
                string accountNumber = myHKeInvestCode.getUserAccountNumber(Session, Context.User.Identity.GetUserName());
                string name = ((string)ViewState["name"]).Trim();
                myHKeInvestCode.createNewOrder(accountNumber, referenceNumber, name, code);
                myHKeInvestCode.createStockSellOrder(referenceNumber, shares, orderType, expiryDate, allOrNone, lowPrice, stopPrice);
            }
            else if ((string)ViewState["type"] == "unit trust")
            {
                string code = (string)ViewState["code"];
                string shares = txtShares.Text.Trim();
                string referenceNumber = myExternalFunctions.submitUnitTrustSellOrder(code, shares);
                if (referenceNumber == null)
                {
                    // order failed to submit
                    // internal error
                    Response.Redirect("Result.aspx?result=Oops..An internal error occured");
                    return;
                }
                string accountNumber = myHKeInvestCode.getUserAccountNumber(Session, Context.User.Identity.GetUserName());
                string name = ((string)ViewState["name"]).Trim();
                myHKeInvestCode.createNewOrder(accountNumber, referenceNumber, name, code);
                myHKeInvestCode.createUnitTrustSellOrder(referenceNumber, shares);
            }

            Response.Redirect("Result.aspx?result=Sell Order Successfully Placed");
        }

        private bool validRelation(string lowPrice, string stopPrice)
        {
            decimal low = decimal.Parse(lowPrice);
            decimal stop = decimal.Parse(stopPrice);

            if (stop < low)
            {
                cvLowPrice.IsValid = false;
                cvLowPrice.ErrorMessage = "stop price must be >= limit price";
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void cvShares_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal shares;
            if (decimal.TryParse(txtShares.Text.Trim(), out shares))
            {
                if (shares <= 0)
                {
                    args.IsValid = false;
                    cvShares.ErrorMessage = "please enter a valid number";
                }
                else if (shares > 0 && Check_SellShares(shares))
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    cvShares.ErrorMessage = "insufficient shares held";
                }
            }
            else
            {
                args.IsValid = false;
                cvShares.ErrorMessage = "please enter a valid number";
            }
            return;
        }

        protected void cvQuantity_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal quantity;
            if (decimal.TryParse(txtQuantity.Text.Trim(), out quantity))
            {
                if (quantity <= 0)
                {
                    args.IsValid = false;
                    cvQuantity.ErrorMessage = "please enter a valid number";
                }
                else if (quantity > 0 && Check_SellShares(quantity))
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    cvQuantity.ErrorMessage = "insufficient shares held";
                }
            }
            else
            {
                args.IsValid = false;
                cvQuantity.ErrorMessage = "please enter a valid quantity";
            }
            return;
        }

        private bool Check_SellShares(decimal shares)
        {
            // get past sell status
            string accountNumber = myHKeInvestCode.getUserAccountNumber(Session, Context.User.Identity.GetUserName());
            string securityType = (string)ViewState["type"];
            DataTable dtSecurityHolding = myHKeInvestData.getData("SELECT shares FROM [SecurityHolding] WHERE accountNumber='" + accountNumber + "' and type='" + securityType + "'");
            decimal totalShares = dtSecurityHolding.Rows[0].Field<decimal>("shares");
            decimal pendingShares = 0;
            string tableName;
            if (securityType == "bond") { tableName = "BondOrderSell"; }
            else if (securityType == "stock") { tableName = "StockOrderSell"; }
            else { tableName = "UnitTrustOrderSell"; }
            string sql = string.Format("SELECT u.shares, o.orderStatus FROM [{0}] u, [Order] o WHERE u.orderReferenceNumber=o.orderReferenceNumber AND o.accountNumber='{1}'", tableName, accountNumber);
            DataTable dtOrder = myHKeInvestData.getData(sql);
            foreach (DataRow row in dtOrder.Rows)
            {
                if (row.Field<string>("orderStatus") != "completed" && row.Field<string>("orderStatus") != "cancelled")
                {
                    pendingShares += row.Field<decimal>("shares");
                }
            }
            return totalShares - pendingShares - shares >= 0;
        }

        protected void ddlStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlStockType.SelectedValue)
            {
                case "limit":
                    LowPrice.Visible = true;
                    StopPrice.Visible = false;
                    break;
                case "stop":
                    LowPrice.Visible = false;
                    StopPrice.Visible = true;
                    break;
                case "stop limit":
                    LowPrice.Visible = true;
                    StopPrice.Visible = true;
                    break;
                default:
                    LowPrice.Visible = false;
                    StopPrice.Visible = false;
                    break;
            }
        }

        protected void cvStockType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddlStockType.SelectedValue == "0")
            {
                args.IsValid = false;
                cvStockType.ErrorMessage = "please indicate your order type";
            }
            else
            {
                args.IsValid = true;
            }
            return;
        }

        protected void cvExpiryDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int expiry;
            if (int.TryParse(ExpiryDate.Text.Trim(), out expiry))
            {
                if (expiry >= 1 && expiry <= 7)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    cvExpiryDate.ErrorMessage = "expiry day can be up to seven days from the current date";
                }
            }
            else
            {
                args.IsValid = false;
                cvExpiryDate.ErrorMessage = "invalid number for expiry day";
            }
        }

        protected void cvLowPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal lowprice;
            if (decimal.TryParse(txtLowPrice.Text.Trim(), out lowprice))
            {
                if (lowprice > 0)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    cvLowPrice.ErrorMessage = "please enter a valid price for lowest price";
                }
            }
            else
            {
                args.IsValid = false;
                cvLowPrice.ErrorMessage = "Highest price is not a valid amount";
            }
        }

        protected void cvStopPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal stopprice;
            if (decimal.TryParse(txtStopPrice.Text.Trim(), out stopprice))
            {
                if (stopprice > 0)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    cvStopPrice.ErrorMessage = "please enter a valid price for stop price";
                }
            }
            else
            {
                args.IsValid = false;
                cvStopPrice.ErrorMessage = "Stop price is not a valid amount";
            }
        }

        protected void gvSecurityInformation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = (string)ViewState["type"];
            string code = (string)gvSecurityInformation.SelectedValue;
            ViewState["code"] = code.Trim();
            ViewState["name"] = gvSecurityInformation.SelectedRow.Cells[2].Text.Trim();
            if (type == "stock")
            {
                StockSell.Visible = true;
            }
            else
            {
                NormalSell.Visible = true;
            }
        }
    }
}