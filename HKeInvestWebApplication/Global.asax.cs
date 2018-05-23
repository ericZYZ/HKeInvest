using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Net.Mail;
using System.Net.Mime;

namespace HKeInvestWebApplication
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Thread bgThread = new Thread(PeriodicTask);
            bgThread.IsBackground = true;
            bgThread.Start();
        }

        private void PeriodicTask()
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();
            do
            {
                DataTable dtOrders = myHKeInvestData.getData("SELECT * FROM [Order]");

                foreach (DataRow order in dtOrders.Rows)
                {
                    string code = order.Field<string>("securityCode");
                    string referenceNumber = order.Field<string>("orderReferenceNumber");
                    string oldStatus = order.Field<string>("orderStatus").Trim();
                    string status = myExternalFunctions.getOrderStatus(referenceNumber).Trim();
                    if (status == "partial" || oldStatus != status)
                    {
                        DataTable dtTransaction = myExternalFunctions.getOrderTransaction(referenceNumber);
                        // update the local transaction table, get the new transactions in dtChanges
                        DataTable dtChanges = Sync_TransactionTable(dtTransaction, referenceNumber);
                        if (dtChanges != null || oldStatus != status)
                        {

                            string type, buyOrSell;
                            DataTable dtOrderDetails = getOrderDetails(referenceNumber, out type, out buyOrSell);
                            if (dtOrderDetails == null)
                            {
                                // cannot find the order details, internal error
                                continue;
                            }

                            string accountNumber = order.Field<string>("accountNumber");
                            DataTable dtAccount = myHKeInvestData.getData("SELECT balance FROM [Account] WHERE accountNumber='" + accountNumber + "'");
                            if (dtAccount.Rows.Count != 1)
                            {
                                // cannot find the account balance, internal error
                                continue;
                            }
                            decimal balance = dtAccount.Rows[0].Field<decimal>("balance");

                            // calculate the total executed price for dtChanges not all transactions
                            decimal totalPrice, totalShares;
                            string securityBase;
                            if (dtChanges != null)
                            {
                                Calculate_totalPrice(dtChanges, type, code, out totalPrice, out totalShares, out securityBase);

                                // update account balance and security holdings
                                balance = Update_AccountBalance(accountNumber, balance, 0, totalPrice, buyOrSell);
                                Update_SecurityHolding(dtOrderDetails, type, accountNumber, totalShares, securityBase, buyOrSell);
                            }


                            if (oldStatus != status)
                            {
                                if (status == "completed" || status == "cancelled")
                                {
                                    // order finished execution
                                    decimal assets = Get_Assets(accountNumber, balance);
                                    // calculate service fee
                                    Calculate_totalPrice(dtTransaction, type, code, out totalPrice, out totalShares, out securityBase);
                                    decimal serviceFee = Calculate_ServiceFee(totalPrice, assets, type, buyOrSell, dtOrderDetails);
                                    // update order status
                                    Update_OrderStatus(referenceNumber, status, serviceFee);
                                    // update account balance and security holdings
                                    Update_AccountBalance(accountNumber, balance, serviceFee, 0, buyOrSell);
                                    // send invoice to client
                                    Send_Invoice(referenceNumber, accountNumber, buyOrSell, code, type, dtOrderDetails, serviceFee, totalPrice, totalShares, dtTransaction);
                                }
                                else
                                {
                                    Update_OrderStatus(referenceNumber, status, 0);
                                }
                            }
                        }

                    }
                }

                // check the status of alerts
                Check_AlertStatus();

                Thread.Sleep(20000);
            } while (true);

            throw new NotImplementedException();
        }

        private decimal Update_AccountBalance(string accountNumber, decimal balance, decimal serviceFee, decimal totalPrice, string buyOrSell)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            decimal newBalance = balance - serviceFee;
            if (buyOrSell == "buy")
            {
                newBalance = newBalance - totalPrice;
            }
            else if (buyOrSell == "sell")
            {
                newBalance = newBalance + totalPrice;
            }
            string sql = string.Format("UPDATE [Account] SET balance={0} WHERE accountNumber='{1}'", newBalance, accountNumber);
            var trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);

            return newBalance;
        }

        private void Update_OrderStatus(string referenceNumber, string status, decimal serviceFee)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            string sql = string.Format("UPDATE [Order] SET orderStatus='{0}', serviceFee={1} WHERE orderReferenceNumber='{2}'", status, serviceFee, referenceNumber);
            var trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        private void Send_Invoice(string referenceNumber, string accountNumber, string buyOrSell, string code, string type,
            DataTable dtOrderDetails, decimal serviceFee, decimal totalPrice, decimal totalShares, DataTable dtTransaction)
        {
            // fetch the primary account holder email
            HKeInvestData myHKeInvestData = new HKeInvestData();
            DataTable dtClient = myHKeInvestData.getData(string.Format("SELECT lastName, email FROM [Client] WHERE accountNumber='{0}' AND isPrimary=(1)", accountNumber));
            if (dtClient.Rows.Count != 1) { return; }
            string destination = dtClient.Rows[0].Field<string>("email");
            string lastName = dtClient.Rows[0].Field<string>("lastName");
            #region construct the invoice
            string text = string.Format("Hi, {11}<br/>One of your order has been processd<br/>order reference number: {0}<br/>" +
                "account number: {1}<br/>" +
                "buy or sell: {2}<br/>" +
                "security code: {3}<br/>" +
                "security name: {4}<br/>" +
                "stock order type: {5}<br/>" +
                "date of submission: {6}<br/>" +
                "total number of shares {7}: {8}<br/>" +
                "total executed HKD amount: {9}<br/>" +
                "fee charged: {10}<br/>",
                referenceNumber,
                accountNumber,
                buyOrSell,
                code,
                dtOrderDetails.Rows[0].Field<string>("securityName"),
                type == "stock" ? dtOrderDetails.Rows[0].Field<string>("stockOrderType") : "N/A",
                dtOrderDetails.Rows[0].Field<DateTime>("dateOfSubmission"),
                buyOrSell == "buy" ? "bought" : "sold", totalShares,
                totalPrice,
                serviceFee,
                lastName);
            string transactionDetails = "transaction details:<br/>";
            foreach (DataRow transaction in dtTransaction.Rows)
            {
                string transactionNumber = transaction.Field<int>("transactionNumber").ToString("00000000");
                DateTime executeDate = transaction.Field<DateTime>("executeDate");
                decimal executeShares = transaction.Field<decimal>("executeShares");
                decimal executePrice = transaction.Field<decimal>("executePrice");
                transactionDetails += string.Format("transaction number:{0} execute date:{1} execute shares:{2} execute price:{3}<br/>", transactionNumber, executeDate, executeShares, executePrice);
            }
            text += transactionDetails;
            string html = text;
            #endregion

            #region emailAccount
            string username = "comp3111_team104@cse.ust.hk";
            #endregion

            // Create an instance of MailMessage named mail.
            MailMessage mail = new MailMessage();
            // Set the sender (From), receiver (To), subject and message body fields of the mail message.
            mail.From = new MailAddress(username, "Team104 Newbee");
            mail.To.Add(destination);
            mail.Subject = "[HKeInvest] Security Invoice";
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Create an instance of SmtpClient named emailServer and set the mail server to use as "smtp.cse.ust.hk".
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");
            // Send the message.
            emailServer.Send(mail);
            return;
        }

        private void Update_SecurityHolding(DataTable dtOrderDetails, string type, string accountNumber, decimal totalShares, string securityBase, string buyOrSell)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            string sql = Get_UpdateSql(dtOrderDetails, type, accountNumber, totalShares, securityBase, buyOrSell);
            if (sql == null) { return; }
            var trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        private string Get_UpdateSql(DataTable dtOrderDetails, string type, string accountNumber, decimal totalShares, string securityBase, string buyOrSell)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            string code = dtOrderDetails.Rows[0].Field<string>("securityCode");
            // find if the security is in the account
            string sql = string.Format("SELECT shares FROM [SecurityHolding] WHERE type='{0}' and code='{1}' and accountNumber='{2}'", type, code, accountNumber);
            DataTable dt = myHKeInvestData.getData(sql);
            if (dt.Rows.Count == 0)
            {
                if (buyOrSell == "sell") { return null; }
                // new security, return insert statement
                return string.Format("INSERT INTO [SecurityHolding] VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}')",
                    accountNumber,
                    type,
                    code,
                    dtOrderDetails.Rows[0].Field<string>("securityName"),
                    totalShares,
                    securityBase);
            }
            else
            {
                decimal newShares;
                // already hold this security generate update 
                if (buyOrSell == "buy")
                {
                    newShares = totalShares + dt.Rows[0].Field<decimal>("shares");
                    return string.Format("UPDATE [SecurityHolding] SET shares={0} WHERE accountNumber='{1}' and type='{2}' and code='{3}'", newShares, accountNumber, type, code);
                }
                else if (buyOrSell == "sell")
                {
                    newShares = dt.Rows[0].Field<decimal>("shares") - totalShares;
                    if (newShares == 0)
                    {
                        // sold out delete the record;
                        return string.Format("DELETE FROM [SecurityHolding] WHERE accountNumber='{0}' and type='{1}' and code='{2}'", accountNumber, type, code);
                    }
                    else if (newShares > 0)
                    {
                        return string.Format("UPDATE [SecurityHolding] SET shares={0} WHERE accountNumber='{1}' and type='{2}' and code='{3}'", newShares, accountNumber, type, code);

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private void Calculate_totalPrice(DataTable dtTransaction, string type, string code, out decimal totalPrice, out decimal totalShares, out string securityBase)
        {
            ExternalFunctions myExternalFunctions = new ExternalFunctions();
            securityBase = "HKD";
            totalShares = 0;
            totalPrice = 0;
            if (type != "stock")
            {
                DataTable dt = myExternalFunctions.getSecuritiesByCode(type, code);
                securityBase = dt.Rows[0].Field<string>("base");
            }
            decimal exchangeRate = myExternalFunctions.getCurrencyRate(securityBase);

            foreach (DataRow row in dtTransaction.Rows)
            {
                decimal price = row.Field<decimal>("executePrice");
                decimal shares = row.Field<decimal>("executeShares");
                totalShares += shares;
                // need to convert the base for price
                totalPrice = totalPrice + (price * shares);
            }
            totalPrice *= exchangeRate;
            totalPrice = Math.Round(totalPrice, 2);
            return;
        }

        private decimal Calculate_ServiceFee(decimal total, decimal assets, string type, string buyOrSell, DataTable dtOrderDetails)
        {
            decimal fee = 0;
            if (type == "unit trust" || type == "bond")
            {
                if (buyOrSell == "buy")
                {
                    if (assets < 500000)
                    {
                        fee = total * (decimal)0.05;
                    }
                    else
                    {
                        fee = total * (decimal)0.03;
                    }
                }
                else if (buyOrSell == "sell")
                {
                    if (assets < 500000)
                    {
                        fee = 100;
                    }
                    else
                    {
                        fee = 50;
                    }
                }
            }
            else if (type == "stock")
            {
                string stockType = dtOrderDetails.Rows[0].Field<string>("stockOrderType");

                if (assets < 1000000)
                {
                    fee = 150;
                    switch (stockType)
                    {
                        case "market":
                            if (fee < total * (decimal)0.04) { fee = total * (decimal)0.04; }
                            break;
                        case "limit":
                        case "stop":
                            if (fee < total * (decimal)0.06) { fee = total * (decimal)0.06; }
                            break;
                        case "stop limit":
                            if (fee < total * (decimal)0.08) { fee = total * (decimal)0.08; }
                            break;
                    }
                }
                else
                {
                    fee = 100;
                    switch (stockType)
                    {
                        case "market":
                            if (fee < total * (decimal)0.02) { fee = total * (decimal)0.02; }
                            break;
                        case "limit":
                        case "stop":
                            if (fee < total * (decimal)0.04) { fee = total * (decimal)0.04; }
                            break;
                        case "stop limit":
                            if (fee < total * (decimal)0.06) { fee = total * (decimal)0.06; }
                            break;
                    }
                }
            }
            return fee;
        }

        private decimal Get_Assets(string accountNumber, decimal balance)
        {
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            HKeInvestData myHKeInvestData = new HKeInvestData();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();

            DataTable dtCurrency = myExternalFunctions.getCurrencyData();
            DataTable dt = myHKeInvestData.getData("SELECT type, code, shares, base FROM [SecurityHolding] WHERE accountNumber='" + accountNumber + "'");
            decimal ret = balance;
            foreach (DataRow row in dt.Rows)
            {
                string securityCode = row["code"].ToString();
                string securityType = row["type"].ToString();
                string securityBase = row["base"].ToString();
                decimal shares = Convert.ToDecimal(row["shares"]);
                decimal price = myExternalFunctions.getSecuritiesPrice(securityType, securityCode);
                decimal value = Math.Round(shares * price - (decimal).005, 2);
                DataRow[] baseRateRow = dtCurrency.Select("currency = '" + securityBase + "'");
                DataRow[] toRateRow = dtCurrency.Select("currency = 'HKD'");
                if (baseRateRow.Length == 1 && toRateRow.Length == 1)
                {
                    value = myHKeInvestCode.convertCurrency(securityBase, baseRateRow[0]["rate"].ToString(), "HKD", toRateRow[0]["rate"].ToString(), value);
                }
                ret += value;
            }
            return ret;
        }

        private DataTable getOrderDetails(string referenceNumber, out string type, out string buyOrSell)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            DataTable dt = myHKeInvestData.getData("SELECT * FROM [Order] o, [UnitTrustOrderBuy] u WHERE o.orderReferenceNumber=u.orderReferenceNumber and o.orderReferenceNumber='" + referenceNumber + "'");
            if (dt.Rows.Count == 1)
            {
                type = "unit trust";
                buyOrSell = "buy";
                return dt;
            }
            dt = myHKeInvestData.getData("SELECT * FROM [Order] o, [UnitTrustOrderSell] u WHERE o.orderReferenceNumber=u.orderReferenceNumber and o.orderReferenceNumber='" + referenceNumber + "'");
            if (dt.Rows.Count == 1)
            {
                type = "unit trust";
                buyOrSell = "sell";
                return dt;
            }
            dt = myHKeInvestData.getData("SELECT * FROM [Order] o, [StockOrderBuy] u WHERE o.orderReferenceNumber=u.orderReferenceNumber and o.orderReferenceNumber='" + referenceNumber + "'");
            if (dt.Rows.Count == 1)
            {
                type = "stock";
                buyOrSell = "buy";
                return dt;
            }
            dt = myHKeInvestData.getData("SELECT * FROM [Order] o, [StockOrderSell] u WHERE o.orderReferenceNumber=u.orderReferenceNumber and o.orderReferenceNumber='" + referenceNumber + "'");
            if (dt.Rows.Count == 1)
            {
                type = "stock";
                buyOrSell = "sell";
                return dt;
            }
            dt = myHKeInvestData.getData("SELECT * FROM [Order] o, [BondOrderBuy] u WHERE o.orderReferenceNumber=u.orderReferenceNumber and o.orderReferenceNumber='" + referenceNumber + "'");
            if (dt.Rows.Count == 1)
            {
                type = "bond";
                buyOrSell = "buy";
                return dt;
            }
            dt = myHKeInvestData.getData("SELECT * FROM [Order] o, [BondOrderSell] u WHERE o.orderReferenceNumber=u.orderReferenceNumber and o.orderReferenceNumber='" + referenceNumber + "'");
            if (dt.Rows.Count == 1)
            {
                type = "bond";
                buyOrSell = "sell";
                return dt;
            }
            type = "";
            buyOrSell = "";
            return null;
        }

        private DataTable Sync_TransactionTable(DataTable dtTransaction, string orderReferenceNumber)
        {
            if (dtTransaction == null) { return null; }
            // clone the table, and convert the column type
            DataTable dtCloned = new DataTable();
            var primaryKey = dtCloned.Columns.Add("transactionNumber", typeof(string));
            dtCloned.Columns.Add("orderReferenceNumber", typeof(string));
            dtCloned.Columns.Add("executeDate", typeof(DateTime));
            dtCloned.Columns.Add("executeShares", typeof(decimal));
            dtCloned.Columns.Add("executePrice", typeof(decimal));
            dtCloned.PrimaryKey = new DataColumn[] { primaryKey };
            foreach (DataRow transaction in dtTransaction.Rows)
            {
                DateTime executeDate = transaction.Field<DateTime>("executeDate");
                string transactionNumber = transaction.Field<int>("transactionNumber").ToString("00000000");
                string referenceNumber = transaction.Field<int>("referenceNumber").ToString("00000000");
                decimal executeShares = transaction.Field<decimal>("executeShares");
                decimal executePrice = transaction.Field<decimal>("executePrice");

                DataRow newRow = dtCloned.NewRow();
                newRow["transactionNumber"] = transactionNumber;
                newRow["orderReferenceNumber"] = referenceNumber;
                newRow["executeDate"] = executeDate;
                newRow["executeShares"] = executeShares;
                newRow["executePrice"] = executePrice;
                dtCloned.Rows.Add(newRow);
            }

            HKeInvestData myHKeInvestData = new HKeInvestData();
            DataTable dtLast = myHKeInvestData.getData("SELECT * FROM [Transaction] WHERE [orderReferenceNumber]='" + orderReferenceNumber.Trim() + "'");
            dtLast.AcceptChanges();
            dtLast.Merge(dtCloned);
            DataTable dtChanges = dtLast.GetChanges(DataRowState.Added);
            if (dtChanges == null) { return null; }
            foreach (DataRow transaction in dtChanges.Rows)
            {
                DateTime executeDate = transaction.Field<DateTime>("executeDate");
                string transactionNumber = transaction.Field<string>("transactionNumber");
                string referenceNumber = transaction.Field<string>("orderReferenceNumber");
                decimal executeShares = transaction.Field<decimal>("executeShares");
                decimal executePrice = transaction.Field<decimal>("executePrice");
                string date = executeDate.ToString("MM/dd/yyyy hh:mm:ss tt");

                string sql = string.Format("INSERT INTO [Transaction] VALUES ('{0}', '{1}', '{2}', {3}, {4})",
                    transactionNumber,
                    referenceNumber,
                    date,
                    executeShares,
                    executePrice);
                var trans = myHKeInvestData.beginTransaction();
                myHKeInvestData.setData(sql, trans);
                myHKeInvestData.commitTransaction(trans);
            }

            return dtChanges;
        }

        private void Check_AlertStatus()
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();
            DataTable dtAlert = myHKeInvestData.getData("SELECT * FROM [Alert]");
            foreach (DataRow alert in dtAlert.Rows)
            {
                string accountNumber = alert.Field<string>("accountNumber");
                string code = alert.Field<string>("code");
                string type = alert.Field<string>("type");
                string highOrLow = alert.Field<string>("highOrLow");
                decimal value = alert.Field<decimal>("value");
                string isSameSide = alert.Field<string>("isSameSide");
                decimal currPrice = myExternalFunctions.getSecuritiesPrice(type, code);

                if (((highOrLow == "high" && currPrice >= value) || (highOrLow == "low" && currPrice <= value)) && isSameSide == "no")
                {
                    // send notification to the client and cancel the alert.
                    string sql = string.Format("DELETE FROM [Alert] WHERE accountNumber='{0}' AND code='{1}' AND type='{2}' AND highOrLow='{3}'",
                        accountNumber,
                        code,
                        type,
                        highOrLow);
                    var trans = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);
                    Send_Notification(accountNumber, type, code, highOrLow, currPrice);
                }
                else if (isSameSide == "yes" && ((highOrLow == "high" && currPrice < value) || (highOrLow == "low" && currPrice > value)))
                {
                    string sql = string.Format("UPDATE [Alert] SET isSameSide='no' WHERE  accountNumber='{0}' AND code='{1}' AND type='{2}' AND highOrLow='{3}'",
                        accountNumber,
                        code,
                        type,
                        highOrLow);
                    var trans = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);
                }
            }
        }

        private void Send_Notification(string accountNumber, string type, string code, string highOrLow, decimal currPrice)
        {
            // fetch the primary account holder email
            HKeInvestData myHKeInvestData = new HKeInvestData();
            DataTable dtClient = myHKeInvestData.getData(string.Format("SELECT lastName, email FROM [Client] WHERE accountNumber='{0}' AND isPrimary=(1)", accountNumber));
            if (dtClient.Rows.Count != 1) { return; }
            string destination = dtClient.Rows[0].Field<string>("email");
            string lastName = dtClient.Rows[0].Field<string>("lastName");
            // fetch the name of the security
            DataTable dtSecurity = myHKeInvestData.getData(string.Format("SELECT name FROM [SecurityHolding] WHERE accountNumber='{0}' AND type='{1}' AND code='{2}'", accountNumber, type, code));
            if (dtSecurity.Rows.Count != 1) { return; }
            string securityName = dtSecurity.Rows[0].Field<string>("name");

            #region construct notification message
            string text = string.Format(@"Hi, {0}<br/>One of your alert(s) has been triggered:<br/>{1} {2} {3}, {4} price reached<br/>the price that triggered the alert: {5}<br/>",
                lastName, type, code, securityName, highOrLow, currPrice);
            string html = text;
            #endregion

            #region emailAccount
            string username = "comp3111_team104@cse.ust.hk";
            #endregion

            // Create an instance of MailMessage named mail.
            MailMessage mail = new MailMessage();
            // Set the sender (From), receiver (To), subject and message body fields of the mail message.
            mail.From = new MailAddress(username, "Team104 Newbee");
            mail.To.Add(destination);
            mail.Subject = "[HKeInvest] Alert Notification";
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Create an instance of SmtpClient named emailServer and set the mail server to use as "smtp.cse.ust.hk".
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");
            // Send the message.
            emailServer.Send(mail);
            return;
        }
    }
}