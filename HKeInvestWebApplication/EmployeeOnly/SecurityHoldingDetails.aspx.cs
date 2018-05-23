using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication
{
    public partial class SecurityHoldingDetails : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        string accountNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["currentBase"] = "HKD";
                ViewState["accountNumber"] = "";
                DataTable dtCurrency = myHKeInvestCode.getCurrencyData(Session);
                foreach (DataRow row in dtCurrency.Rows)
                {
                    ddlCurrency.Items.Add(row["currency"].ToString().Trim());
                    ddlOverviewCurrency.Items.Add(row["currency"].ToString().Trim());
                }
            }
            accountNumber = (string)ViewState["accountNumber"];
        }

        public bool accountExists(string accountNumber)
        {
            if (accountNumber == null)
            {
                return false;
            }

            string sql = "SELECT * FROM dbo.[Account] a WHERE a.accountNumber='" + accountNumber + "';";
            DataTable dt = myHKeInvestData.getData(sql);
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count != 1)
            {
                return false;
            }
            return true;
        }

        protected void btnAccountNumber_OnClick(object sender, EventArgs e)
        {
            divDetails.Visible = false;
            lbAccountNumberResult.Visible = false;
            string acNumber = tbAccounNumber.Text.Trim();
            if (acNumber==null||!accountExists(acNumber))
            {
                lbAccountNumberResult.Text = "Account not exists.";
                lbAccountNumberResult.Visible = true;
                tbAccounNumber.Text = "";
            }
            else
            {
                ViewState["accountNumber"] = acNumber;
                lblClientName.Text = "Client Name: " + myHKeInvestCode.getClientName(acNumber);
                accountNumber = acNumber;
                tbAccounNumber.Text = "";
                divDetails.Visible = true;
                // Initialize the overview
                initOverview();
            }
        }

        protected string toLowerType(string name)
        {
            if (name == null)
                return null;
            if (name.Equals("Stock"))
                return "stock";
            else if (name.Equals("Bond"))
                return "bond";
            else if (name.Equals("UnitTrust"))
                return "unit trust";
            else return null;
        }

        // Creating sql string for finding latest executed orders
        protected string makeSQL(string type, string bOrs)
        {
            string accountNumber = (string)ViewState["accountNumber"];
            string action = bOrs.ToLower();
            string sql = "SELECT t.executeDate,o.orderReferenceNumber,o.dateOfSubmission FROM dbo.[Order] o,dbo.[" + type + "Order" + bOrs + "] " + action + ",dbo.[Transaction] t WHERE o.accountNumber='" + accountNumber + "' AND o.orderStatus = 'completed' AND o.orderReferenceNumber=" + action + ".orderReferenceNumber AND " + action + ".orderReferenceNumber=t.orderReferenceNumber AND NOT EXISTS (SELECT * FROM dbo.[Order] o1,dbo.[" + type + "Order" + bOrs + "] " + action + "1,dbo.[Transaction] t1 WHERE o1.accountNumber='" + accountNumber + "' AND o1.orderReferenceNumber=" + action + "1.orderReferenceNumber AND " + action + "1.orderReferenceNumber=t1.orderReferenceNumber AND t1.executeDate>t.executeDate);";
            string targetORN = "";
            string targetDate = "";
            DataTable dt = myHKeInvestData.getData(sql);
            if (dt == null) return null;
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    targetORN = Convert.ToString(row["orderReferenceNumber"]).Trim();
                    targetDate = Convert.ToString(row["executeDate"]).Trim();
                }
            }
            sql = "SELECT '" + targetDate + "' as executeDate,'" + toLowerType(type) + "' as targetType, o.securityCode, t.executePrice,t.executeShares,o.dateOfSubmission FROM dbo.[Transaction] t, dbo.[Order] o WHERE o.orderReferenceNumber='" + targetORN + "' AND o.orderReferenceNumber=t.orderReferenceNumber;";
            return sql;
        }

        protected void findLatest(ref DataTable dt, ref DateTime latestDate, ref string latestSubDateString, ref decimal latestValue, string sql)
        {
            dt = myHKeInvestData.getData(sql);
            if (dt == null) return;    // sql error
            if (dt.Rows.Count != 0)
            {
                bool isSameTable = false;
                foreach (DataRow row in dt.Rows)
                {
                    DateTime date;
                    try
                    {
                        date = Convert.ToDateTime(row["executeDate"]);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Cannot convert date.");
                        return;
                    }
                    if (date.CompareTo(latestDate) >= 0)
                    {
                        if (!isSameTable)
                        {
                            resetVariables(ref latestDate, ref latestSubDateString, ref latestValue);
                            isSameTable = true;
                        }
                        latestDate = date;
                        string code = Convert.ToString(row["securityCode"]).Trim();
                        string type = Convert.ToString(row["targetType"]).Trim();
                        decimal latestExecPrice = Convert.ToDecimal(row["executePrice"]);
                        decimal latestExecShares = Convert.ToDecimal(row["executeShares"]);
                        latestValue += latestExecPrice * latestExecShares * getCurrencyRate(type, code);
                        DateTime d = new DateTime();
                        try
                        {
                            d = Convert.ToDateTime(row["dateOfSubmission"]);
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Cannot convert date.");
                            return;
                        }
                        latestSubDateString = d.ToString("G");
                    }
                }
            }
        }

        protected void resetVariables(ref DateTime latestDate, ref string latestSubDateString, ref decimal latestValue)
        {
            latestDate = new DateTime(1800, 1, 1);
            latestSubDateString = "";
            latestValue = 0;
        }

        protected void initOverview()
        {
            lblAccountNumber.Text = "Account number: " + (string)ViewState["accountNumber"];

            // Getting currency information
            string currentBase = (string)ViewState["currentBase"];
            decimal rate = myExternalFunctions.getCurrencyRate(currentBase);
            if (rate == -1)
            {
                return;
            }

            string accountNumber = (string)ViewState["accountNumber"];
            // variables to store the results
            decimal balance = 0;
            decimal totalValue = 0;
            decimal stockValue = 0;
            decimal bondValue = 0;
            decimal utValue = 0;

            // Create a table
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("types", typeof(string));
            dtResult.Columns.Add("value", typeof(decimal));
            dtResult.Columns.Add("lastDate", typeof(string));
            dtResult.Columns.Add("lastOrderValue", typeof(decimal));

            // Get the balance from the account
            string sql = "SELECT a.balance FROM dbo.[Account] a WHERE a.accountNumber = '" + accountNumber + "';";
            DataTable dt = myHKeInvestData.getData(sql);
            if (dt == null) return;    // sql error
            if (dt.Rows.Count != 0)
            {
                balance = Convert.ToDecimal(dt.Rows[0]["balance"]);
            }
            lblBalance.Text = Convert.ToString(balance).Trim() + " HKD";

            //Get the value of securities by types
            sql = "SELECT s.code,s.shares, s.[type] FROM dbo.[SecurityHolding] s WHERE s.accountNumber='" + accountNumber + "';";
            dt = myHKeInvestData.getData(sql);
            if (dt == null) return; //  sql error
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    decimal s = Convert.ToDecimal(row["shares"]);
                    string t = Convert.ToString(row["type"]).Trim();
                    string c = Convert.ToString(row["code"]).Trim();
                    if (t.Equals("stock"))
                        stockValue += s * myExternalFunctions.getSecuritiesPrice(t, c) * getCurrencyRate(t, c) / rate;
                    else if (t.Equals("bond"))
                        bondValue += s * myExternalFunctions.getSecuritiesPrice(t, c) * getCurrencyRate(t, c) / rate;
                    else if (t.Equals("unit trust"))
                        utValue += s * myExternalFunctions.getSecuritiesPrice(t, c) * getCurrencyRate(t, c) / rate;
                }
            }
            totalValue += stockValue + bondValue + utValue;
            totalValue = Math.Round(totalValue, 2);
            stockValue = Math.Round(stockValue, 2);
            bondValue = Math.Round(bondValue, 2);
            utValue = Math.Round(utValue, 2);
            lblTotalValue.Text = Convert.ToString(totalValue).Trim() + " " + currentBase;

            // Get the informtaion for individual types
            // type, value, submit date of last executed, value of last executed

            DateTime latestDate = new DateTime(1800, 1, 1);
            string latestSubDateString = "";
            decimal latestValue = 0;
            // Find the last executed buy order for stock
            sql = makeSQL("Stock", "Buy");
            findLatest(ref dt, ref latestDate, ref latestSubDateString, ref latestValue, sql);
            // Find the last executed sell order for stock
            sql = makeSQL("Stock", "Sell");
            findLatest(ref dt, ref latestDate, ref latestSubDateString, ref latestValue, sql);
            // Add the result into the dtResult
            dtResult.Rows.Add("Stock", Math.Round(stockValue, 2), latestSubDateString, Math.Round(latestValue / rate, 2));


            resetVariables(ref latestDate, ref latestSubDateString, ref latestValue);
            // Find the last executed buy order for bond
            sql = makeSQL("Bond", "Buy");
            findLatest(ref dt, ref latestDate, ref latestSubDateString, ref latestValue, sql);
            // Find the last exected sell order for bond
            sql = makeSQL("Bond", "Sell");
            findLatest(ref dt, ref latestDate, ref latestSubDateString, ref latestValue, sql);
            // Add the result into the dtResult
            dtResult.Rows.Add("Bond", Math.Round(bondValue, 2), latestSubDateString, Math.Round(latestValue / rate, 2));

            resetVariables(ref latestDate, ref latestSubDateString, ref latestValue);
            // Find the last executed buy order for unit trust
            sql = makeSQL("UnitTrust", "Buy");
            findLatest(ref dt, ref latestDate, ref latestSubDateString, ref latestValue, sql);
            // Find the last exected sell order for unit trust
            sql = makeSQL("UnitTrust", "Sell");
            findLatest(ref dt, ref latestDate, ref latestSubDateString, ref latestValue, sql);
            // Add the result into the dtResult
            dtResult.Rows.Add("Unit Trust", Math.Round(utValue, 2), latestSubDateString, Math.Round(latestValue / rate, 2));

            gvOverview.DataSource = dtResult;
            gvOverview.DataBind();
            gvOverview.Visible = true;

        }

        protected void ddlSecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset visbility of controls and initialize values.
            lblResultMessage.Visible = false;
            ddlCurrency.Visible = false;
            gvSecurityHolding.Visible = false;
            ddlCurrency.SelectedIndex = 0;
            string sql = "";

            // *******************************************************************
            // Set the account number and security type from the web page. *
            // *******************************************************************
            string accountNumber = (string)ViewState["accountNumber"]; // Set the account number from a web form control!
            string securityType = ddlSecurityType.SelectedValue; // Set the securityType from a web form control!

            /*
            // Check if an account number has been specified.
            if (accountNumber == "")
            {
                lblResultMessage.Text = "An error occur, we cannot find your account";
                lblResultMessage.Visible = true;
                ddlSecurityType.SelectedIndex = 0;
                return;
            }*/


            // No action when the first item in the DropDownList is selected.
            if (securityType == "0") { return; }

            /*
            // *****************************************************************************************
            // Construct the SQL statement to retrieve the first and last name of the client(s). *
            // *****************************************************************************************
            sql = "SELECT lastName, firstName FROM dbo.Client WHERE accountNumber='" + accountNumber + "'"; // Complete the SQL statement.

            DataTable dtClient = myHKeInvestData.getData(sql);
            if (dtClient == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned by the SQL statement, then display a message.
            if (dtClient.Rows.Count == 0)
            {
                lblResultMessage.Text = "Current account doesn't hold any security of this type.";
                lblResultMessage.Visible = true;
                gvSecurityHolding.Visible = false;
                return;
            }


            // Show the client name(s) on the web page.
            string clientName = "Client(s): ";
            int i = 1;
            foreach (DataRow row in dtClient.Rows)
            {
                clientName = clientName + row["lastName"] + ", " + row["firstName"];
                if (dtClient.Rows.Count != i)
                {
                    clientName = clientName + "and ";
                }
                i = i + 1;
            }
            */

            // *****************************************************************************************************************************
            //       Construct the SQL select statement to get the code, name, shares and base of the security holdings of a specific type *
            //       in an account. The select statement should also return three additonal columns -- price, value and convertedValue --  *
            //       whose values are not actually in the database, but are set to the constant 0.00 by the select statement. (HINT: see   *
            //       http://stackoverflow.com/questions/2504163/include-in-select-a-column-that-isnt-actually-in-the-database.)            *   
            // *****************************************************************************************************************************
            sql = "SELECT type, code, name, shares, base, 0.00 as price, 0.00 as value, 0.00 as convertedValue FROM dbo.SecurityHolding WHERE accountNumber='" + accountNumber + "' and type='" + securityType + "'"; // Complete the SQL statement.

            DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
            if (dtSecurityHolding == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned, then display a message that the account does not hold this type of security.
            if (dtSecurityHolding.Rows.Count == 0)
            {
                lblResultMessage.Text = "No " + securityType + "s held in this account.";
                lblResultMessage.Visible = true;
                gvSecurityHolding.Visible = false;
                return;
            }

            // For each security in the result, get its current price from an external system, calculate the total value
            // of the security and change the current price and total value columns of the security in the result.
            int dtRow = 0;
            foreach (DataRow row in dtSecurityHolding.Rows)
            {
                string securityCode = row["code"].ToString().Trim();
                decimal shares = Convert.ToDecimal(row["shares"]);
                decimal price = myExternalFunctions.getSecuritiesPrice(securityType, securityCode);
                decimal value = Math.Round(shares * price - (decimal).005, 2);
                dtSecurityHolding.Rows[dtRow]["price"] = price;
                dtSecurityHolding.Rows[dtRow]["value"] = value;
                dtRow = dtRow + 1;
            }

            // Set the initial sort expression and sort direction for sorting the GridView in ViewState.
            ViewState["SortExpression"] = "name";
            ViewState["SortDirection"] = "ASC";

            // Set the default sorting order of the gridview
            dtSecurityHolding.DefaultView.Sort = "name ASC";
            dtSecurityHolding.AcceptChanges();

            // Bind the GridView to the DataTable.
            gvSecurityHolding.DataSource = dtSecurityHolding;
            gvSecurityHolding.DataBind();

            // Set the visibility of controls and GridView data.
            gvSecurityHolding.Visible = true;
            ddlCurrency.Visible = true;
            gvSecurityHolding.Columns[myHKeInvestCode.getColumnIndexByName(gvSecurityHolding, "convertedValue")].Visible = false;
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the index value of the convertedValue column in the GridView using the helper method "getColumnIndexByName".
            int convertedValueIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityHolding, "convertedValue");

            // Get the currency to convert to from the ddlCurrency dropdownlist.
            // Hide the converted currency column if no currency is selected.
            string toCurrency = ddlCurrency.SelectedValue;
            if (toCurrency == "0")
            {
                gvSecurityHolding.Columns[convertedValueIndex].Visible = false;
                return;
            }

            // Make the convertedValue column visible and create a DataTable from the GridView.
            // Since a GridView cannot be updated directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            gvSecurityHolding.Columns[convertedValueIndex].Visible = true;
            DataTable dtSecurityHolding = myHKeInvestCode.unloadGridView(gvSecurityHolding);

            // ***********************************************************************************************************
            //       For each row in the DataTable, get the base currency of the security, convert the current value to  *
            //       the selected currency and assign the converted value to the convertedValue column in the DataTable. *
            // ***********************************************************************************************************
            DataTable dtCurrency = myHKeInvestCode.getCurrencyData(Session);
            foreach (DataRow row in dtSecurityHolding.Rows)
            {
                // Add your code here!
                string baseCurrency = row["base"].ToString();
                DataRow[] baseRateRow = dtCurrency.Select("currency = '" + baseCurrency + "'");
                DataRow[] toRateRow = dtCurrency.Select("currency = '" + toCurrency + "'");
                if (baseRateRow.Length == 1 && toRateRow.Length == 1)
                {
                    //decimal baseRate = baseRateRow[0].Field<decimal>("rate");
                    //decimal toRate = toRateRow[0].Field<decimal>("rate");
                    decimal value = row.Field<decimal>("value");
                    decimal convertedValue; // = value * baseRate / toRate;
                    convertedValue = myHKeInvestCode.convertCurrency(baseCurrency, baseRateRow[0]["rate"].ToString(), toCurrency, toRateRow[0]["rate"].ToString(), value);
                    row["convertedValue"] = convertedValue;
                }
            }

            // Change the header text of the convertedValue column to indicate the currency. 
            gvSecurityHolding.Columns[convertedValueIndex].HeaderText = "Value in " + toCurrency;

            // Bind the DataTable to the GridView.
            gvSecurityHolding.DataSource = dtSecurityHolding;
            gvSecurityHolding.DataBind();
        }

        protected void ddlOverviewCurrency_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Need to update: 
            // total value of security holdings, total value of security holdings for each type, value of last executed order
            int typeValueIndex = myHKeInvestCode.getColumnIndexByName(gvOverview, "value");
            int typeLastExecIndex = myHKeInvestCode.getColumnIndexByName(gvOverview, "lastOrderValue");
            string toCurrency = ddlOverviewCurrency.SelectedValue;
            if (toCurrency == "0")
            {
                ViewState["currentBase"] = "HKD";
            }
            else
            {
                ViewState["currentBase"] = toCurrency;
            }

            // Change the header text of the convertedValue column to indicate the currency. 
            gvOverview.Columns[typeValueIndex].HeaderText = "Value Held in " + (string)ViewState["currentBase"];
            gvOverview.Columns[typeLastExecIndex].HeaderText = "Value of Last Executed Order in " + (string)ViewState["currentBase"];

            initOverview();
        }

        protected void gvSecurityHolding_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Since a GridView cannot be sorted directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            // Create a DataTable from the GridView.
            DataTable dtSecurityHolding = myHKeInvestCode.unloadGridView(gvSecurityHolding);

            // Set the sort expression in ViewState for correct toggling of sort direction,
            // Sort the DataTable and bind it to the GridView.
            string sortExpression = e.SortExpression.ToLower();
            ViewState["SortExpression"] = sortExpression;
            dtSecurityHolding.DefaultView.Sort = sortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dtSecurityHolding.AcceptChanges();

            // Bind the DataTable to the GridView.
            gvSecurityHolding.DataSource = dtSecurityHolding.DefaultView;
            gvSecurityHolding.DataBind();
        }

        protected decimal getCurrencyRate(string type, string code)  // type: bond, unit trust
        {
            DataTable dt = myExternalFunctions.getSecuritiesByCode(type, code);
            if (dt == null || dt.Rows.Count == 0)
                return -1;  // error or not found
            string Base;
            if (type.Equals("stock"))
            {
                Base = "HKD";
            }
            else Base = Convert.ToString(dt.Rows[0]["base"]).Trim();
            return myExternalFunctions.getCurrencyRate(Base);
        }

    }


}