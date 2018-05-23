using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using System.Web.SessionState;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication.Code_File
{
    //**********************************************************
    //*  THE CODE IN THIS CLASS CAN BE MODIFIED AND ADDED TO.  *
    //**********************************************************
    public class HKeInvestCode
    {
        public void createStockSellOrder(string referenceNumber, string shares, string orderType, string expiryDay, string allOrNone, string lowPrice, string stopPrice)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            var trans = myHKeInvestData.beginTransaction();
            string sql = string.Format("INSERT INTO [StockOrderSell] VALUES ({0}, '{1}', {2}, '{3}',",
                shares,
                orderType,
                int.Parse(expiryDay),
                allOrNone);
            if (orderType == "limit")
            {
                sql += lowPrice + ", NULL, '" + referenceNumber + "')";
            }
            else if (orderType == "stop")
            {
                sql += "NULL, " + stopPrice + ", '" + referenceNumber + "')";
            }
            else if (orderType == "stop limit")
            {
                sql += lowPrice + "," + stopPrice + ", '" + referenceNumber + "')";
            }
            else
            {
                sql += "NULL, NULL, '" + referenceNumber + "')";
            }
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public void createUnitTrustSellOrder(string referenceNumber, string shares)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            var trans = myHKeInvestData.beginTransaction();
            string sql = string.Format("INSERT INTO [UnitTrustOrderSell] VALUES ({0}, '{1}')", decimal.Parse(shares), referenceNumber);
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public void createBondSellOrder(string referenceNumber, string shares)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            var trans = myHKeInvestData.beginTransaction();
            string sql = string.Format("INSERT INTO [BondOrderSell] VALUES ({0}, '{1}')", decimal.Parse(shares), referenceNumber);
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public void createStockBuyOrder(string referenceNumber, string shares, string orderType, string expiryDay, string allOrNone, string highPrice, string stopPrice)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            var trans = myHKeInvestData.beginTransaction();
            string sql = string.Format("INSERT INTO [StockOrderBuy] VALUES ({0}, '{1}', {2}, '{3}',",
                shares,
                orderType,
                int.Parse(expiryDay),
                allOrNone);
            if (orderType == "limit")
            {
                sql += highPrice + ", NULL, '" + referenceNumber + "')";
            }
            else if (orderType == "stop")
            {
                sql += "NULL, " + stopPrice + ", '" + referenceNumber + "')";
            }
            else if (orderType == "stop limit")
            {
                sql += highPrice + "," + stopPrice + ", '" + referenceNumber + "')";
            }
            else
            {
                sql += "NULL, NULL, '" + referenceNumber + "')";
            }
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public void createUnitTrustBuyOrder(string referenceNumber, string amount)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            var trans = myHKeInvestData.beginTransaction();
            string sql = string.Format("INSERT INTO [UnitTrustOrderBuy] VALUES ({0}, '{1}')", decimal.Parse(amount), referenceNumber);
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public void createBondBuyOrder(string referenceNumber, string amount)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            var trans = myHKeInvestData.beginTransaction();
            string sql = string.Format("INSERT INTO [BondOrderBuy] VALUES ({0}, '{1}')", decimal.Parse(amount), referenceNumber);
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public void createNewOrder(string accountNumber, string referenceNumber, string name, string code)
        {
            string status = "pending";
            decimal serviceFee = 0;
            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

            HKeInvestData myHKeInvestData = new HKeInvestData();
            var trans = myHKeInvestData.beginTransaction();
            string sql = string.Format("INSERT INTO [Order] VALUES ('{0}', '{1}', '{2}', {3}, '{4}', '{5}', '{6}')",
                referenceNumber,
                name,
                status,
                serviceFee,
                code,
                date,
                accountNumber);
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public string getUserAccountNumber(HttpSessionState session, string userName)
        {
            if (session["accountNumber"] == null)
            {
                HKeInvestData myHKeInvestData = new HKeInvestData();
                string sql = "SELECT accountNumber FROM dbo.Account WHERE userName='" + userName + "'";
                DataTable dtAccountNumber = myHKeInvestData.getData(sql);
                if (dtAccountNumber.Rows.Count == 1)
                {
                    session["accountNumber"] = dtAccountNumber.Rows[0].Field<string>("accountNumber");
                }
                else
                {
                    session["accountNumber"] = "";
                }
            }
            return (string)session["accountNumber"];
        }

        public string getClientName(string accountNumber)
        {
            if (accountNumber == null || accountNumber.Equals(""))
            {
                return "";
            }
            else
            {
                HKeInvestData myHKeInvestData = new HKeInvestData();
                string sql = "SELECT lastName,firstName FROM dbo.[Client] WHERE accountNumber='"+accountNumber+"';";
                DataTable dtName = myHKeInvestData.getData(sql);
                if (dtName.Rows.Count == 1)
                {
                    return dtName.Rows[0].Field<string>("firstName").Trim()+" "+ dtName.Rows[0].Field<string>("lastName").Trim();
                }
                else
                {
                    return "";
                }
            }
        }

        public DataTable getCurrencyData(HttpSessionState session)
        {
            if (session["currencyData"] == null)
            {
                ExternalFunctions myExternalFunctions = new ExternalFunctions();
                session["currencyData"] = myExternalFunctions.getCurrencyData();
            }
            return (DataTable)session["currencyData"];
        }

        public string getDataType(string value)
        {
            // Returns the data type of value. Tests for more types can be added if needed.
            if (value != null)
            {
                int n; decimal d; DateTime dt;
                if (int.TryParse(value, out n)) { return "System.Int32"; }
                else if (decimal.TryParse(value, out d)) { return "System.Decimal"; }
                else if (DateTime.TryParse(value, out dt)) { return "System.DateTime"; }
            }
            return "System.String";
        }

        public string getSortDirection(System.Web.UI.StateBag viewState, string sortExpression)
        {
            // If the GridView is sorted for the first time or sorting is being done on a new column, 
            // then set the sort direction to "ASC" in ViewState.
            if (viewState["SortDirection"] == null || viewState["SortExpression"].ToString() != sortExpression)
            {
                viewState["SortDirection"] = "ASC";
            }
            // Othewise if the same column is clicked for sorting more than once, then toggle its SortDirection.
            else if (viewState["SortDirection"].ToString() == "ASC")
            {
                viewState["SortDirection"] = "DESC";
            }
            else if (viewState["SortDirection"].ToString() == "DESC")
            {
                viewState["SortDirection"] = "ASC";
            }
            return viewState["SortDirection"].ToString();
        }

        public DataTable unloadGridViewWithLinkButton(GridView gv)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < gv.Columns.Count-1; i++)
            {
                dt.Columns.Add(((BoundField)gv.Columns[i]).DataField);
            }

            // For correct sorting, set the data type of each DataTable column based on the values in the GridView.
            gv.SelectedIndex = 0;
            for (int i = 0; i < gv.Columns.Count-1; i++)
            {
                dt.Columns[i].DataType = Type.GetType(getDataType(gv.SelectedRow.Cells[i].Text));
            }

            // Load the GridView data into the DataTable.
            foreach (GridViewRow row in gv.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < gv.Columns.Count-1; j++)
                {
                    dr[((BoundField)gv.Columns[j]).DataField.ToString().Trim()] = row.Cells[j].Text;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public DataTable unloadGridView(GridView gv)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                dt.Columns.Add(((BoundField)gv.Columns[i]).DataField);
            }

            // For correct sorting, set the data type of each DataTable column based on the values in the GridView.
            gv.SelectedIndex = 0;
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                dt.Columns[i].DataType = Type.GetType(getDataType(gv.SelectedRow.Cells[i].Text));
            }

            // Load the GridView data into the DataTable.
            foreach (GridViewRow row in gv.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < gv.Columns.Count; j++)
                {
                    dr[((BoundField)gv.Columns[j]).DataField.ToString().Trim()] = row.Cells[j].Text;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public int getColumnIndexByName(GridView gv, string columnName)
        {
            // Helper method to get GridView column index by a column's DataField name.
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (((BoundField)gv.Columns[i]).DataField.ToString().Trim() == columnName.Trim())
                { return i; }
            }
            MessageBox.Show("Column '" + columnName + "' was not found \n in the GridView '" + gv.ID.ToString() + "'.");
            return -1;
        }

        public decimal convertCurrency(string fromCurrency, string fromCurrencyRate, string toCurrency, string toCurrencyRate, decimal value)
        {
            if(fromCurrency == toCurrency)
            {
                return value;
            }
            else
            {
                return Math.Round(Convert.ToDecimal(fromCurrencyRate) / Convert.ToDecimal(toCurrencyRate) * value - (decimal).005, 2);
            }
        }
    }
}