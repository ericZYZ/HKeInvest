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
    public partial class BuySecurities : System.Web.UI.Page
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
                    DataTable dtAccountInformation = myHKeInvestData.getData("SELECT balance FROM [Account] WHERE [accountNumber]='" + accountNumber + "'");
                    decimal balance = (decimal)dtAccountInformation.Rows[0]["balance"];
                    lblAccountBalance.Text += balance.ToString();
                    ViewState["accountBalance"] = balance;
                }
            }
        }
        
        protected void Confirm_Click(object sender, EventArgs e)
        {
            if (ddlSecurityType.SelectedValue != "0")
            {
                if (string.IsNullOrWhiteSpace(txtSecurityCode.Text))
                {
                    ErrorMessage.Text = "please type in the security code";
                    SecurityInformation.Visible = false;
                    NormalBuy.Visible = false;
                    StockBuy.Visible = false;
                    return;
                }
                else
                {
                    DataTable securityinfo = myExternalFunctions.getSecuritiesByCode(ddlSecurityType.SelectedValue, txtSecurityCode.Text.Trim());
                    if (securityinfo == null)
                    {
                        ErrorMessage.Text = "invalid security code";
                        SecurityInformation.Visible = false;
                        NormalBuy.Visible = false;
                        StockBuy.Visible = false;
                        return;
                    }

                    ErrorMessage.Text = "";

                    ViewState["code"] = txtSecurityCode.Text.Trim();
                    ViewState["type"] = ddlSecurityType.SelectedValue;

                    SecurityInformation.Visible = true;
                    ViewState["name"] = securityinfo.Rows[0]["name"];
                    gvSecurityInformation.DataSource = securityinfo;
                    gvSecurityInformation.DataBind();

                    if (ddlSecurityType.SelectedValue == "stock")
                    {
                        NormalBuy.Visible = false;
                        StockBuy.Visible = true;
                    }
                    else
                    {
                        NormalBuy.Visible = true;
                        StockBuy.Visible = false;
                    }
                }
            }
            else
            {
                ErrorMessage.Text = "please select a security type";
                SecurityInformation.Visible = false;
                NormalBuy.Visible = false;
                StockBuy.Visible = false;
                return;
            }
        }

        protected void Buy_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            if ((string)ViewState["type"] == "bond")
            {
                string code = (string)ViewState["code"];
                string amount = txtAmount.Text.Trim();
                string referenceNumber = myExternalFunctions.submitBondBuyOrder(code, amount);
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
                myHKeInvestCode.createBondBuyOrder(referenceNumber, amount);
            }
            else if ((string)ViewState["type"] == "stock")
            {
                string code = (string)ViewState["code"];
                string shares = txtQuantity.Text.Trim();
                string orderType = ddlStockType.SelectedValue;
                string expiryDate = ExpiryDate.Text.Trim();
                string allOrNone = AllOrNone.Checked ? "Y" : "N";
                string highPrice = txtHighPrice.Text.Trim();
                string stopPrice = txtStopPrice.Text.Trim();

                if (orderType == "stop limit" && !validRelation(highPrice, stopPrice))
                {
                    // HACK: manually set the validator's IsValid property
                    // without calling Validate() method to set Page.IsValid property
                    return;
                }

                string referenceNumber = myExternalFunctions.submitStockBuyOrder(code, shares, orderType, expiryDate, allOrNone, highPrice, stopPrice);
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
                myHKeInvestCode.createStockBuyOrder(referenceNumber, shares, orderType, expiryDate, allOrNone, highPrice, stopPrice);
            }
            else if ((string)ViewState["type"] == "unit trust")
            {
                string code = (string)ViewState["code"];
                string amount = txtAmount.Text.Trim();
                string referenceNumber = myExternalFunctions.submitUnitTrustBuyOrder(code, amount);
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
                myHKeInvestCode.createUnitTrustBuyOrder(referenceNumber, amount);
            }

            Response.Redirect("Result.aspx?result=Buy Order Successfully Placed");
        }

        private bool validRelation(string highPrice, string stopPrice)
        {
            decimal high = decimal.Parse(highPrice);
            decimal stop = decimal.Parse(stopPrice);

            if (stop > high)
            {
                cvHighPrice.IsValid = false;
                cvHighPrice.ErrorMessage = "stop price must be <= limit price";
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void cvAmount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal amount;
            if (decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                if (amount <= (decimal)ViewState["accountBalance"]
                    && amount > 0)
                {
                    args.IsValid = true;
                }
                else if (amount <= 0)
                {
                    args.IsValid = false;
                    cvAmount.ErrorMessage = "please enter a valid number";
                }
                else
                {
                    args.IsValid = false;
                    cvAmount.ErrorMessage = "insufficient balance";
                }
            }
            else
            {
                args.IsValid = false;
                cvAmount.ErrorMessage = "please enter a valid number";
            }
            return;
        }

        protected void cvQuantity_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal quantity;
            if (decimal.TryParse(txtQuantity.Text.Trim(), out quantity))
            {
                if (quantity > 0 && (quantity % 100 == 0))
                {
                        args.IsValid = true;
                }
                else if (quantity <= 0)
                {
                    args.IsValid = false;
                    cvQuantity.ErrorMessage = "please enter a valid number";
                }
                else
                {
                    args.IsValid = false;
                    cvQuantity.ErrorMessage = "quantity must be a multiple of 100";
                }
            }
            else
            {
                args.IsValid = false;
                cvQuantity.ErrorMessage = "please enter a valid quantity";
            }
            return;
        }

        protected void ddlStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlStockType.SelectedValue)
            {
                case "limit":
                    HighPrice.Visible = true;
                    StopPrice.Visible = false;
                    break;
                case "stop":
                    HighPrice.Visible = false;
                    StopPrice.Visible = true;
                    break;
                case "stop limit":
                    HighPrice.Visible = true;
                    StopPrice.Visible = true;
                    break;
                default:
                    HighPrice.Visible = false;
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

        protected void cvHighPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal highprice;
            if (decimal.TryParse(txtHighPrice.Text.Trim(), out highprice))
            {
                if (highprice > 0)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    cvHighPrice.ErrorMessage = "please enter a valid price for highest price";
                }
            }
            else
            {
                args.IsValid = false;
                cvHighPrice.ErrorMessage = "Highest price is not a valid amount";
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
    }
}