using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication.EmployeeOnly
{
    public partial class EmployeeReportPage : System.Web.UI.Page
    {
        string accountNumber;
        string bosChoice = null;
        string typeChoice = null;
        string codeChoice = null;
        string statusChoice = null;
        DataTable dtOrderHistory;
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunction = new ExternalFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            gvTransactionDetails.Visible = false;
            divTransacionDetails.Visible = false;

            if (!IsPostBack)
            {
                ViewState["accountNumber"] = "";
                divReportContent.Visible = false;

                // Initializing the calendars
                DateTime today = DateTime.Today;
                FromDate.SelectedDate = today;
                ToDate.SelectedDate = today;
                ViewState["fromDate"] = today;
                ViewState["toDate"] = today;

                // Initializing view states for variables
                ViewState["bosChoice"] = null;
                ViewState["typeChoice"] = null;
                ViewState["codeChoice"] = null;
                ViewState["statusChoice"] = null;
                ViewState["dtOrderHistory"] = null;
            }

            // Loading the variables for convenience
            accountNumber = (string)ViewState["accountNumber"];
            bosChoice = (string)ViewState["bosChoice"];
            typeChoice = (string)ViewState["typeChoice"];
            codeChoice = (string)ViewState["codeChoice"];
            statusChoice = (string)ViewState["statusChoice"];
            dtOrderHistory = (DataTable)ViewState["dtOrderHistory"];
        }

        protected void btnAccountNumber_OnClick(object sender, EventArgs e)
        {
            divReportContent.Visible = false;
            lbAccountNumberResult.Visible = false;
            string acNumber = tbAccounNumber.Text.Trim();
            if (acNumber == null || accountExists(acNumber))
            {
                ViewState["accountNumber"] = acNumber;
                accountNumber = acNumber;
                tbAccounNumber.Text = "";
                divReportContent.Visible = true;
                
                // Initializing grid views
                initGVOrderStatus();
                setGVOrderStatus();
                initGVHistory();
                setGVHistory();
                ViewState["dtOrderHistory"] = dtOrderHistory;
            }
            else
            {
                lbAccountNumberResult.Text = "Account not exists.";
                lbAccountNumberResult.Visible = true;
                tbAccounNumber.Text = "";
            }
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


        protected void setGVHistory()
        {
            // Strings for additional criteria
            string codeCrit = " ", statusCrit = " ";
            if (codeChoice != null)
                codeCrit = " o.securityCode='" + codeChoice + "' AND ";
            if (statusChoice != null)
                statusCrit = " o.orderStatus='" + statusChoice + "' AND ";

            // Reset all the rows in dtOrderHistory
            dtOrderHistory.Rows.Clear();

            // Restriction on the action(buy/sell) if there is any
            List<string> BAS = new List<string>();
            if (bosChoice != null)
                BAS.Add(bosChoice);
            else
            {
                BAS.Add("Buy");
                BAS.Add("Sell");
            }

            // Restriction on the type if there is any
            List<string> dbTypes = new List<string>();
            if (typeChoice != null)
                dbTypes.Add(toDBType(typeChoice));
            else
            {
                dbTypes.Add("Stock"); dbTypes.Add("Bond"); dbTypes.Add("UnitTrust");
            } // Securities types string for query

            // Construct the basic sql string
            string sql = "";
            foreach (string action in BAS)
            {
                foreach (string dbType in dbTypes)
                {
                    // For a given action (buy/sell) and a given type of securities
                    DateTime from = (DateTime)ViewState["fromDate"];
                    DateTime to = (DateTime)ViewState["toDate"];
                    from = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
                    to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);
                    sql = "SELECT o.orderReferenceNumber,o.securityCode,o.securityName,o.dateOfSubmission,o.orderStatus,o.serviceFee FROM dbo.[Order] o, dbo.[" + dbType + "Order" + action + "] " + action + " WHERE" + codeCrit + statusCrit + " o.accountNumber='" + accountNumber + "' AND o.orderReferenceNumber=" + action + ".orderReferenceNumber " +
                   "and o.dateOfSubmission >= '" + from + "' and o.dateOfSubmission <= '" + to + "'";


                    DataTable dt = myHKeInvestData.getData(sql);
                    if (dt == null) return;    // sql error
                    if (dt.Rows.Count != 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            // For a given orderReferenceNumber, find its transactions

                            decimal sharesExecuted = 0;
                            decimal dollarsExecuted = 0;
                            string orn = Convert.ToString(row["orderReferenceNumber"]).Trim();
                            sql = "SELECT t.executeShares,t.executePrice FROM dbo.[Transaction] t,dbo.[Order] o WHERE o.accountNumber='" + accountNumber + "' AND t.orderReferenceNumber='" + orn + "' AND o.orderReferenceNumber=t.orderReferenceNumber;";
                            DataTable dt1 = myHKeInvestData.getData(sql);
                            if (dt == null) return;    // sql error
                            if (dt.Rows.Count != 0)
                            {
                                foreach (DataRow r in dt1.Rows)
                                {
                                    sharesExecuted += Convert.ToDecimal(r["executeShares"]);
                                    string code = Convert.ToString(row["securityCode"]).Trim();
                                    dollarsExecuted += Convert.ToDecimal(r["executePrice"]) * Convert.ToDecimal(r["executeShares"]) * getCurrencyRate(dbType, code);
                                }
                            }
                            string c = Convert.ToString(row["securityCode"]).Trim();
                            string n = Convert.ToString(row["securityName"]).Trim();
                            string sub = Convert.ToString(row["dateOfSubmission"]).Trim();
                            string st = Convert.ToString(row["orderStatus"]).Trim();
                            decimal f = Convert.ToDecimal(row["serviceFee"]);
                            dtOrderHistory.Rows.Add(orn, action, toLowerType(dbType), c, n, sub, st, sharesExecuted, dollarsExecuted, f);
                        }
                    }
                }
            }
            
            // Set the default sorting order of the gridview
            dtOrderHistory.DefaultView.Sort = "subDate DESC";
            dtOrderHistory.AcceptChanges();

            gvOrderHistory.DataSource = dtOrderHistory;
            gvOrderHistory.DataBind();
            gvOrderHistory.Visible = true;
        }

        protected void initGVHistory()
        {
            dtOrderHistory = new DataTable();
            dtOrderHistory.Columns.Add("orderReferenceNumber", typeof(string));
            dtOrderHistory.Columns.Add("buyOrSell", typeof(string));
            dtOrderHistory.Columns.Add("type", typeof(string));
            dtOrderHistory.Columns.Add("code", typeof(string));
            dtOrderHistory.Columns.Add("name", typeof(string));
            dtOrderHistory.Columns.Add("subDate", typeof(string));
            dtOrderHistory.Columns.Add("status", typeof(string));
            dtOrderHistory.Columns.Add("sharesExec", typeof(decimal));
            dtOrderHistory.Columns.Add("execDollar", typeof(decimal));
            dtOrderHistory.Columns.Add("fee", typeof(decimal));
        }

        protected void lvTransactionDetail_Click(object sender, EventArgs e)
        {
            // Get the text of lvTransactionDetail
            GridViewRow gRow = (GridViewRow)((LinkButton)sender).NamingContainer;

            // Get the order reference number
            string orn = gRow.Cells[0].Text.Trim();

            // Query the database to get transaction information
            string sql = "SELECT '" + orn + "' AS orn,t.transactionNumber, t.executeDate as dateExecuted,t.executeShares as sharesExecuted,t.executePrice as priceExecuted FROM dbo.[Transaction] t WHERE t.orderReferenceNumber='" + orn + "';";
            DataTable dt1 = myHKeInvestData.getData(sql);
            if (dt1 == null) return;    // sql error
            if (dt1.Rows.Count == 0)
                return;
            else
            {
                gvTransactionDetails.DataSource = dt1;
                gvTransactionDetails.DataBind();
                divTransacionDetails.Visible = true;
                gvTransactionDetails.Visible = true;
            }
        }

        protected void ddlBuyOrSell_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBuyOrSell.SelectedValue.Equals("0"))
            {
                bosChoice = null;
                ViewState["bosChoice"] = bosChoice;
            }
            else {
                bosChoice = ddlBuyOrSell.SelectedValue.Trim();
                ViewState["bosChoice"] = bosChoice;
            }
            setGVHistory();
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue.Equals("0"))
            {
                statusChoice = null;
                ViewState["statusChoice"] = statusChoice;
            }
            else
            {
                statusChoice = ddlStatus.SelectedValue.Trim();
                ViewState["statusChoice"] = statusChoice;
            }
            setGVHistory();
        }

        protected void ddlSecurityType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSecurityType.SelectedValue.Trim().Equals("0"))
            {
                typeChoice = null;
                ViewState["typeChoice"] = typeChoice;
            }
            else
            {
                typeChoice = ddlSecurityType.SelectedValue.Trim();
                ViewState["typeChoice"] = typeChoice;
            }
        }

        protected void btnSpecific_onClick(object sender, EventArgs e)
        {
            if (tbCode.Text.Trim().Equals(""))
            {
                codeChoice = null;
                ViewState["codeChoice"] = codeChoice;
            }
            else
            {
                codeChoice = tbCode.Text.Trim();
                ViewState["codeChoice"] = codeChoice;
            }
            setGVHistory();
        }

        protected void btnHide_onClick(object sender, EventArgs e)
        {
            gvTransactionDetails.Visible = false;
            divTransacionDetails.Visible = false;
        }

        // Given a orderReferenceNumber, find out which kind of order it is (ie. buy/sell,stock/bond/unit trust)
        private string[] getOrderTypes(string referNumber)
        {
            if (referNumber != null)
            {
                string[] dbs = { "BondOrderBuy", "BondOrderSell", "UnittrustOrderBuy", "UnittrustOrderSell", "StockOrderBuy", "StockOrderSell" };
                foreach (string db in dbs)
                {
                    string sql1 = "select * from dbo.[" + db + "] bb where bb.orderReferenceNumber = '" + referNumber + "' ";
                    DataTable dt = myHKeInvestData.getData(sql1);
                    if (dt == null) continue;
                    if (dt.Rows.Count != 0)
                    {
                        if (db.Equals("BondOrderBuy"))
                        {
                            string[] result = { "Buy", "Bond" };
                            return result;
                        }
                        else if (db.Equals("BondOrderSell"))
                        {
                            string[] result = { "Sell", "Bond" };
                            return result;
                        }
                        else if (db.Equals("UnittrustOrderBuy"))
                        {
                            string[] result = { "Buy", "Unittrust" };
                            return result;
                        }
                        else if (db.Equals("UnittrustOrderSell"))
                        {
                            string[] result = { "Sell", "Unittrust" };
                            return result;
                        }
                        else if (db.Equals("StockOrderBuy"))
                        {
                            string[] result = { "Buy", "Stock" };
                            return result;
                        }
                        else if (db.Equals("StockOrderSell"))
                        {
                            string[] result = { "Sell", "Stock" };
                            return result;
                        }
                    }
                }
            }

            string[] re = { "notExist", "notExist" };
            return re;
        }

        protected void OrderStatus_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Since a GridView cannot be sorted directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            // Create a DataTable from the GridView.
            DataTable dt_order_status = myHKeInvestCode.unloadGridView(OrderStatus);

            // Set the sort expression in ViewState for correct toggling of sort direction,
            // Sort the DataTable and bind it to the GridView.
            ViewState["SortExpression"] = e.SortExpression;
            dt_order_status.DefaultView.Sort = e.SortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dt_order_status.AcceptChanges();

            // Bind the DataTable to the GridView.
            OrderStatus.DataSource = dt_order_status.DefaultView;
            OrderStatus.DataBind();
        }

        protected void gvOrderHistory_OnSorting(object sender, GridViewSortEventArgs e)
        {
            // Since a GridView cannot be sorted directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            // Create a DataTable from the GridView.
            DataTable dt_order_history = myHKeInvestCode.unloadGridViewWithLinkButton(gvOrderHistory);

            // Set the sort expression in ViewState for correct toggling of sort direction,
            // Sort the DataTable and bind it to the GridView.
            ViewState["SortExpression"] = e.SortExpression;
            dt_order_history.DefaultView.Sort = e.SortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dt_order_history.AcceptChanges();

            // Bind the DataTable to the GridView.
            gvOrderHistory.DataSource = dt_order_history.DefaultView;
            gvOrderHistory.DataBind();
        }

        private void initGVOrderStatus()
        {
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "referenceNumber")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "orderType")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "securityType")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "code")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "name")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "orderDate")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "orderStatus")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "dollarAmount")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "shares")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "limitOrderPrice")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "stopOrderPrice")].Visible = true;
            OrderStatus.Columns[myHKeInvestCode.getColumnIndexByName(OrderStatus, "expiryDate")].Visible = true;
        }

        private string toDBType(string type)
        {
            if (type == null)
            {
                return null;
            }
            type = type.ToLower();
            if (type.Contains("bond"))
            {
                return "Bond";
            }
            else if (type.Contains("stock"))
            {
                return "Stock";
            }
            else if (type.Contains("unit"))
            {
                return "UnitTrust";
            }
            else
            {
                return null;
            }
        }

        private string toLowerType(string type)
        {
            if (type == null)
            {
                return null;
            }
            type = type.ToLower();
            if (type.Contains("bond"))
            {
                return "bond";
            }
            else if (type.Contains("stock"))
            {
                return "stock";
            }
            else if (type.Contains("unit"))
            {
                return "unit trust";
            }
            else
            {
                return null;
            }
        }

        protected void setGVOrderStatus()
        {
            string sql_all_order = string.Format("SELECT o.orderReferenceNumber, o.dateOfSubmission ,o.securityCode, o.orderStatus, o.securityName from dbo.[Order] o WHERE o.accountNumber = '{0}' AND o.orderStatus<>'{1}' AND o.orderStatus<>'{2}'; ",
                accountNumber,
                "completed",
                "cancelled"
                );
            DataTable dtAllOrders = myHKeInvestData.getData(sql_all_order);

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("referenceNumber", typeof(string));
            dtResult.Columns.Add("orderDate", typeof(string));
            dtResult.Columns.Add("code", typeof(string));
            dtResult.Columns.Add("name", typeof(string));
            dtResult.Columns.Add("orderStatus", typeof(string));

            foreach (DataRow row in dtAllOrders.Rows)
            {
                string rn = Convert.ToString(row["orderReferenceNumber"]);
                string od = Convert.ToString(row["dateOfSubmission"]).Trim();
                string sc = Convert.ToString(row["securityCode"]);
                string st = Convert.ToString(row["orderStatus"]);
                string sn = Convert.ToString(row["securityName"]);
                dtResult.Rows.Add(rn, od, sc, sn, st);
            }
            // after add all the raw info, start to get orderType and securityType first;
            dtResult.Columns.Add("orderType", typeof(string));
            dtResult.Columns.Add("securityType", typeof(string));
            for (int i = 0; i < dtAllOrders.Rows.Count; i++)
            {
                string referNumber = Convert.ToString(dtAllOrders.Rows[i]["orderReferenceNumber"]);
                string[] types = getOrderTypes(referNumber);
                if (types[0].Equals("notExist") || types[1].Equals("notExist")) continue;
                dtResult.Rows[i]["orderType"] = types[0];
                dtResult.Rows[i]["securityType"] = types[1];
            }

            // after get ordertype and security type, for bond and unittrust get buy amount /sell shares, for stock get 4 other things
            dtResult.Columns.Add("dollarAmount", typeof(string));
            dtResult.Columns.Add("shares", typeof(string));
            dtResult.Columns.Add("limitOrderPrice", typeof(string));
            dtResult.Columns.Add("stopOrderPrice", typeof(string));
            dtResult.Columns.Add("expiryDate", typeof(string));
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                string referNumber = Convert.ToString(dtResult.Rows[i]["referenceNumber"]);
                string ot = Convert.ToString(dtResult.Rows[i]["orderType"]);
                string st = Convert.ToString(dtResult.Rows[i]["securityType"]);
                string dbname = st + "Order" + ot;
                if (st.Equals("Bond") || st.Equals("Unittrust"))
                {
                    if (ot.Equals("Buy"))
                    {
                        string sql_get_amount = "select b.amount from dbo.[" + dbname + "] b where b.orderReferenceNumber = '" + referNumber + "' ";
                        DataTable dt_buy = myHKeInvestData.getData(sql_get_amount);
                        if (dt_buy == null) continue;
                        if (dt_buy.Rows.Count == 1)
                        {
                            string code = Convert.ToString(dtResult.Rows[i]["code"]);
                            if (st.Equals("Unittrust")) st = "unit trust";
                            dtResult.Rows[i]["dollarAmount"] = Convert.ToDecimal(dt_buy.Rows[0]["amount"]) * getCurrencyRate(st.ToLower(), code);
                        }
                    }
                    else if (ot.Equals("Sell"))
                    {
                        string sql_get_shares = "select s.shares from dbo.[" + dbname + "] s where s.orderReferenceNumber = '" + referNumber + "' ";
                        DataTable dt_sell = myHKeInvestData.getData(sql_get_shares);
                        if (dt_sell == null) continue;
                        if (dt_sell.Rows.Count == 1)
                        {
                            dtResult.Rows[i]["shares"] = Convert.ToDecimal(dt_sell.Rows[0]["shares"]);
                        }
                    }
                }
                else if (st.Equals("Stock"))
                {
                    if (ot.Equals("Buy"))
                    {
                        string sql_get_buy = "select b.expiryDay, b.shares, b.stockOrderType, b.allOrNone, b.limitPrice, b.stopPrice from dbo.StockOrderBuy b where b.orderReferenceNumber = '" + referNumber + "' ";
                        DataTable dt_buy = myHKeInvestData.getData(sql_get_buy);
                        if (dt_buy == null) continue;
                        if (dt_buy.Rows.Count == 1)
                        {
                            dtResult.Rows[i]["shares"] = Convert.ToString(dt_buy.Rows[0]["shares"]);
                            dtResult.Rows[i]["expiryDate"] = Convert.ToInt16(dt_buy.Rows[0]["expiryDay"]);
                            dtResult.Rows[i]["limitOrderPrice"] = (dt_buy.Rows[0]["limitPrice"] == DBNull.Value) ? 0 : Convert.ToDecimal(dt_buy.Rows[0]["limitPrice"]);
                            dtResult.Rows[i]["stopOrderPrice"] = (dt_buy.Rows[0]["stopPrice"] == DBNull.Value) ? 0 : Convert.ToDecimal(dt_buy.Rows[0]["stopPrice"]);
                        }
                    }
                    else if (ot.Equals("Sell"))
                    {
                        string sql_get_sell = "select s.expiryDay, s.shares, s.stockOrderType, s.allOrNone, s.limitPrice, s.stopPrice from dbo.StockOrderSell s where s.orderReferenceNumber = '" + referNumber + "' ";
                        DataTable dt_sell = myHKeInvestData.getData(sql_get_sell);
                        if (dt_sell == null) continue;
                        if (dt_sell.Rows.Count == 1)
                        {
                            dtResult.Rows[i]["shares"] = Convert.ToString(dt_sell.Rows[0]["shares"]);
                            dtResult.Rows[i]["expiryDate"] = Convert.ToInt16(dt_sell.Rows[0]["expiryDay"]);
                            dtResult.Rows[i]["limitOrderPrice"] = (dt_sell.Rows[0]["limitPrice"] == DBNull.Value) ? 0 : Convert.ToDecimal(dt_sell.Rows[0]["limitPrice"]);
                            dtResult.Rows[i]["stopOrderPrice"] = (dt_sell.Rows[0]["stopPrice"] == DBNull.Value) ? 0 : Convert.ToDecimal(dt_sell.Rows[0]["stopPrice"]);
                        }
                    }
                }
            }

            // Set the default sorting order of the gridview
            dtResult.DefaultView.Sort = "orderDate DESC";
            dtResult.AcceptChanges();

            // get all the information required to show the order details, start to bind
            OrderStatus.DataSource = dtResult;
            OrderStatus.DataBind();
            OrderStatus.Visible = true;
        }

        protected decimal getCurrencyRate(string type, string code)
        {
            type = toLowerType(type);
            DataTable dt = myExternalFunction.getSecuritiesByCode(type, code);
            if (dt == null || dt.Rows.Count == 0)
            {
                return -1;
            }
            string Base;
            if (type.Equals("stock"))
            {
                Base = "HKD";
            }
            else {
                Base = Convert.ToString(dt.Rows[0]["base"]).Trim();
            }
            return myExternalFunction.getCurrencyRate(Base);
        }

        protected void FromDate_SelectionChanged(object sender, EventArgs e)
        {
            ViewState["fromDate"] = FromDate.SelectedDate;
            setGVHistory();
        }

        protected void ToDate_SelectionChanged(object sender, EventArgs e)
        {
            ViewState["toDate"] = ToDate.SelectedDate;
            setGVHistory();
        }

        protected void btnClearFrom_OnClick(object sender, EventArgs e)
        {
            FromDate.SelectedDates.Clear();
            ViewState["fromDate"] = (new DateTime(2000, 1, 1));
            setGVHistory();
        }

        protected void btnClearTo_OnClick(object sender, EventArgs e)
        {
            ToDate.SelectedDates.Clear();
            ViewState["toDate"] = (new DateTime(2050, 1, 1));
            setGVHistory();
        }


    }
}