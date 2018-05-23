using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class ProfitLoss : System.Web.UI.Page
    {
        string accountNumber;
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExteernalFunction = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userName = Context.User.Identity.GetUserName();
                string sql = "SELECT accountNumber FROM dbo.Account WHERE userName='" + userName + "'";
                DataTable dtAccountNumber = myHKeInvestData.getData(sql);
                if (dtAccountNumber.Rows.Count == 1)
                {
                    ViewState["accountNumber"] = dtAccountNumber.Rows[0].Field<string>("accountNumber");
                }
                else
                {
                    accountNumber = "";
                }
                gvTrackGroup.Visible = false;
                gvTrackIndividual.Visible = false;
            }
            accountNumber = (string)ViewState["accountNumber"];
        }

        protected void TrackType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvTrackGroup.Visible = false;
            gvTrackIndividual.Visible = false;
            ddlSecurityType.ClearSelection();
            if (TrackType.SelectedValue == "individual")
            {
                Input_individual.Visible = true;
                ddlSecurityType.Visible = true;
                btnTrack.Visible = true;
                tbSecurityCode.Visible = true;
            }
            else if (TrackType.SelectedValue == "givenType")
            {
                Input_individual.Visible = true;
                ddlSecurityType.Visible = true;
                btnTrack.Visible = false;
                tbSecurityCode.Visible = false;
            }
            else
            {
                Input_individual.Visible = false;
                getAllTracking();
            }
        }

        protected void ddlSecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TrackType.SelectedValue == "givenType" && ddlSecurityType.SelectedValue != "0")
            {
                getGivenType();
            }
        }

        protected void getBuySellInfo(ref decimal spent, ref decimal gain, ref decimal fee, string dbType)
        {
            decimal rate = 1;
            string type = toLowerType(dbType);
            // calculate the dollars spent and fees for buying
            string sql = "SELECT o.securityCode, o.serviceFee, t.executeShares, t.executePrice FROM dbo.[Order] o, dbo.[Transaction] t, dbo.[" + dbType + "OrderBuy] buy WHERE o.accountNumber = '" + accountNumber + "' AND o.orderReferenceNumber=t.orderReferenceNumber AND o.orderReferenceNumber = buy.orderReferenceNumber;";
            DataTable dtBuy = myHKeInvestData.getData(sql);
            if (dtBuy == null) return;  // sql error
            if (dtBuy.Rows.Count != 0)
            {
                foreach (DataRow row in dtBuy.Rows)
                {
                    string code = Convert.ToString(row["securityCode"]).Trim();
                    rate = getCurrencyRate(type, code);
                    fee += Convert.ToDecimal(row["serviceFee"]);
                    spent += rate * Convert.ToDecimal(row["executeShares"]) * Convert.ToDecimal(row["executePrice"]);
                }
            }

            // Calculating the dollars gained and fees for selling
            sql = "SELECT o.securityCode,o.serviceFee, t.executeShares, t.executePrice FROM dbo.[Order] o, dbo.[Transaction] t, dbo.[" + dbType + "OrderSell] sell WHERE o.accountNumber = '" + accountNumber + "' AND o.orderReferenceNumber=t.orderReferenceNumber AND o.orderReferenceNumber = sell.orderReferenceNumber;";
            DataTable dtSell = myHKeInvestData.getData(sql);
            if (dtSell == null) return;  // sql error
            if (dtSell.Rows.Count != 0)
            {
                foreach (DataRow row in dtSell.Rows)
                {
                    string code = Convert.ToString(row["securityCode"]).Trim();
                    rate = getCurrencyRate(type, code);
                    fee += Convert.ToDecimal(row["serviceFee"]);
                    gain += Convert.ToDecimal(row["executeShares"]) * Convert.ToDecimal(row["executePrice"]);
                }
            }
        }

        protected void getGroupHoldingInfo(ref decimal curValue, string type)
        {
            decimal rate = 1;
            // Retrieve the current holding of the given type of security
            string sql = "SELECT s.shares,s.code FROM dbo.[SecurityHolding] s WHERE s.accountNumber='" + accountNumber + "' AND s.type='" + type + "';";
            DataTable dtCurShare = myHKeInvestData.getData(sql);
            if (dtCurShare == null) return; //sql error
            if (dtCurShare.Rows.Count != 0)
            {
                foreach (DataRow row in dtCurShare.Rows)
                {
                    string code = Convert.ToString(row["code"]).Trim();
                    decimal s = Convert.ToDecimal(row["shares"]);
                    rate = getCurrencyRate(type, code);
                    curValue += getCurrencyRate(type, code.Trim()) * myExteernalFunction.getSecuritiesPrice(type, code.Trim()) * s;
                }
            }
        }

        protected void groupRefineAndBind(ref decimal spent, ref decimal gain, ref decimal fee, ref decimal curValue, ref decimal profitLoss)
        {
            // Calculate profit/loss
            profitLoss = Math.Round(curValue - spent + gain - fee);
            decimal percentage = (spent == 0) ? 0 : Math.Round(profitLoss / spent, 4);

            // Format all the results
            curValue = Math.Round(curValue, 2);
            spent = Math.Round(spent, 2);
            gain = Math.Round(gain, 2);
            fee = Math.Round(fee, 2);

            //  Create a table for the result
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("spent", typeof(string));
            dtResult.Columns.Add("gain", typeof(string));
            dtResult.Columns.Add("fee", typeof(string));
            dtResult.Columns.Add("profitLoss", typeof(string));

            dtResult.Rows.Add(spent, gain, fee, profitLoss.ToString() + "(" + percentage.ToString("P") + ")");

            // Bind the gridview to the datatable
            gvTrackGroup.DataSource = dtResult;
            gvTrackGroup.DataBind();

            // Setup the visibility of the grid view
            gvTrackGroup.Visible = true;
        }

        protected void getGivenType()
        {
            // total spent, total gain, fee, profit/loss
            string dbType = ddlSecurityType.SelectedValue;
            if (dbType == null) return;
            if (dbType.Equals("0"))
            {
                gvTrackGroup.Visible = false;
                return;
            }
            string type = toLowerType(dbType);

            // variables to store the results
            decimal spent = 0;
            decimal gain = 0;
            decimal fee = 0;
            decimal curValue = 0;
            decimal profitLoss = 0;

            getBuySellInfo(ref spent, ref gain, ref fee, dbType);
            getGroupHoldingInfo(ref curValue, type);
            groupRefineAndBind(ref spent, ref gain, ref fee, ref curValue, ref profitLoss);
        }

        private void getAllTracking()
        {
            string[] dbTypes = { "Stock", "Bond", "UnitTrust" };

            // variables to store the results
            decimal spent = 0;
            decimal gain = 0;
            decimal fee = 0;
            decimal curValue = 0;
            decimal profitLoss = 0;

            foreach (string dbType in dbTypes)
            {
                getBuySellInfo(ref spent,ref gain,ref fee,dbType);
                getGroupHoldingInfo(ref curValue, toLowerType(dbType));
            }

            groupRefineAndBind(ref spent, ref gain, ref fee, ref curValue, ref profitLoss);
        }

        protected void TrackIndividualSecurity_Click(object sender, EventArgs e)
        {
            string dbType = ddlSecurityType.SelectedValue.Trim();
            string code = tbSecurityCode.Text.Trim();
            string securityName = "";
            if (dbType == "0" || code.Equals(""))
            {
                gvTrackIndividual.Visible = false;
                return;
            }
            string type = toLowerType(dbType);

            string sql;

            // variables to store the results
            decimal totalBuyDollars = 0;
            decimal totalSellDollars = 0;
            decimal shareHeld = 0;
            decimal currentValue = 0;
            decimal profit_loss = 0;
            decimal fee = 0;
            decimal rate = 1;

            sql = "SELECT o.serviceFee, o.securityName, t.executeShares, t.executePrice FROM dbo.[Order] o, dbo.[Transaction] t, dbo.[" + dbType + "OrderBuy] buy WHERE o.accountNumber='" + accountNumber + "' AND o.securityCode =" + code + " AND o.orderReferenceNumber=buy.orderReferenceNumber AND buy.orderReferenceNumber = t.orderReferenceNumber;";
            DataTable dtBuyDollar = myHKeInvestData.getData(sql);
            if (dtBuyDollar == null) return;    // sql error occurs
            if (dtBuyDollar.Rows.Count != 0)
            {
                rate = getCurrencyRate(type, code);
                foreach (DataRow row in dtBuyDollar.Rows)
                {
                    securityName = Convert.ToString(row["securityName"]);
                    decimal executeShares = Convert.ToDecimal(row["executeShares"]);
                    decimal executePrice = Convert.ToDecimal(row["executePrice"]);
                    totalBuyDollars += executeShares * executePrice * rate;
                    fee += Convert.ToDecimal(row["serviceFee"]);
                }
            }

            //Now need to compute the total sell dollars
            sql = "SELECT o.serviceFee, t.executeShares, t.executePrice FROM dbo.[Order] o,dbo.[" + dbType + "OrderSell] s, dbo.[Transaction] t WHERE o.accountNumber='" + accountNumber + "' AND o.securityCode='" + code + "' AND o.orderReferenceNumber=s.orderReferenceNumber AND s.orderReferenceNumber=t.orderReferenceNumber;";
            DataTable dtSellDollar = myHKeInvestData.getData(sql);
            if (dtSellDollar == null) return;   // sql error occurs
            if (dtSellDollar.Rows.Count != 0)
            {
                foreach (DataRow row in dtSellDollar.Rows)
                {
                    decimal executeShares = Convert.ToDecimal(row["executeShares"]);
                    decimal executePrice = Convert.ToDecimal(row["executePrice"]);
                    totalSellDollars += executeShares * executePrice * rate;
                    fee += Convert.ToDecimal(row["serviceFee"]);
                }
            }

            // Retrieve the current shares of security, and get the current price per share
            sql = "SELECT shares, name FROM dbo.[SecurityHolding] s WHERE s.accountNumber='" + accountNumber + "' AND s.code='" + code + "' AND s.[type]='" + type + "';";
            DataTable dtShares = myHKeInvestData.getData(sql);
            if (dtShares == null) return;   // sql error occurs
            if (dtShares.Rows.Count != 0)
            {
                foreach (DataRow row in dtShares.Rows)
                {
                    shareHeld += Convert.ToDecimal(row["shares"]);
                    securityName = Convert.ToString(row["name"]).Trim();
                }
            }
            currentValue = rate * shareHeld * myExteernalFunction.getSecuritiesPrice(type, code);

            // Calculate the profit/loss
            profit_loss = Math.Round(currentValue - totalBuyDollars + totalSellDollars - fee, 2);
            decimal percentage = (totalBuyDollars == 0) ? 0 : Math.Round(profit_loss / totalBuyDollars, 4);

            // Format the results
            shareHeld = Math.Round(shareHeld, 2);
            totalBuyDollars = Math.Round(totalBuyDollars, 2);
            totalSellDollars = Math.Round(totalSellDollars, 2);
            fee = Math.Round(fee, 2);

            // security type, security code, security name, shares held, total dollar amount for buying, total dollar amount from selling, total fees paid and profit/loss
            DataTable dtTrack = new DataTable();
            dtTrack.Columns.Add("type", typeof(string));
            dtTrack.Columns.Add("code", typeof(string));
            dtTrack.Columns.Add("name", typeof(string));
            dtTrack.Columns.Add("shares", typeof(string));
            dtTrack.Columns.Add("spent", typeof(string));
            dtTrack.Columns.Add("gain", typeof(string));
            dtTrack.Columns.Add("fee", typeof(string));
            dtTrack.Columns.Add("profitLoss", typeof(string));
            dtTrack.Rows.Add(type, code, securityName, shareHeld, totalBuyDollars, totalSellDollars, fee, profit_loss.ToString() + "(" + percentage.ToString("P") + ")");

            // Bind the gridview to the datatable
            gvTrackIndividual.DataSource = dtTrack;
            gvTrackIndividual.DataBind();

            //Set the visibility of gridview
            gvTrackIndividual.Visible = true;
        }

        // input type names are standard "stock", "bond",or "unit trust"
        protected decimal getCurrencyRate(string type, string code)
        {
            DataTable dt = myExteernalFunction.getSecuritiesByCode(type, code);
            if (dt == null || dt.Rows.Count == 0)
                return -1;  // error or not found
            string Base;
            if (type.Equals("stock"))
            {
                Base = "HKD";
            }
            else
            {
                Base = Convert.ToString(dt.Rows[0]["base"]).Trim();
            }
            return myExteernalFunction.getCurrencyRate(Base);
        }

        // convert type names to "stock","bond", or "unit trust"
        protected string toLowerType(String name)
        {
            if (name == null)
                return null;
            name = name.ToLower();
            if (name.Contains("stock"))
                return "stock";
            else if (name.Contains("bond"))
                return "bond";
            else if (name.Contains("unit"))
                return "unit trust";
            else return null;
        }

    }


}