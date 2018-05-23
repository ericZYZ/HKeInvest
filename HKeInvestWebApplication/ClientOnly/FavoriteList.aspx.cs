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

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class FavoriteList : System.Web.UI.Page
    {
        string accountNumber;
        bool hasLoadAccountNumber = false;
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        ExternalData myExternalData = new ExternalData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userName = Context.User.Identity.GetUserName();
                string sql = "SELECT accountNumber FROM dbo.Account WHERE userName='" + userName + "'";
                DataTable dtAccountNumber = myHKeInvestData.getData(sql);
                if (dtAccountNumber.Rows.Count == 1)
                {
                    ViewState["accountNumber"] = dtAccountNumber.Rows[0].Field<string>("accountNumber").Trim();
                }
                else
                {
                    ViewState["accountNumber"] = "";
                }
                ViewState.Add("SortExpression", "name");
                ViewState.Add("SortDirection", "ASC");
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
            }
            if (!hasLoadAccountNumber)
            {
                hasLoadAccountNumber = true;
                accountNumber = (string)ViewState["accountNumber"];
            }
        }


        protected void searchMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

            securityPartialName.Text = String.Empty;
            securityCode.Text = String.Empty;
            string method = searchMethod.SelectedValue;
            if (method == "Security Type")
            {
                result.Visible = false;
                name.Visible = false;
                code.Visible = false;
                type.Visible = true;

                search1.Visible = true;
                search2.Visible = false;
                search3.Visible = false;
            }
            else if (method == "Security Type & Security Partial Name")
            {
                result.Visible = false;
                name.Visible = true;
                code.Visible = false;
                type.Visible = true;

                search1.Visible = false;
                search2.Visible = true;
                search3.Visible = false;

            }
            else {
                result.Visible = false;
                name.Visible = false;
                code.Visible = true;
                type.Visible = true;

                search1.Visible = false;
                search2.Visible = false;
                search3.Visible = true;
            }


        }

        protected void securityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable result = myHKeInvestData.getData(sql);

            string type = securityType.SelectedValue;
            string sql = "";
            if (type == "Bond")
            {

            }
            else if (type == "Unit Trust")
            {

            }
            else {

            }
        }

        protected void search1_Click(object sender, EventArgs e)
        {
            ddlSort.SelectedValue = "Sort by Name (Default)";
            string type = securityType.SelectedValue;
            //string method = searchMethod.SelectedValue;

            if (type == "Bond")
            {
                DataTable dtBond = new DataTable();
                dtBond = myExternalData.getData("select * from [Bond] order by [name]");
                if (dtBond == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtBond;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }
            }
            else if (type == "Unit Trust")
            {

                DataTable dtUnitTrust = new DataTable();
                dtUnitTrust = myExternalData.getData("select * from [UnitTrust] order by [name]");
                if (dtUnitTrust == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtUnitTrust;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }

            }
            else if (type == "Stock")
            {

                DataTable dtStock = new DataTable();
                dtStock = myExternalData.getData("select * from [Stock] order by [name]");
                if (dtStock == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtStock;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }

            }
        }

        protected void gvSecurityDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            string type = securityType.SelectedValue;
            if (type == "Bond")
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = "Code";
                    e.Row.Cells[1].Text = "Name";
                    e.Row.Cells[2].Text = "Launch Date";
                    e.Row.Cells[3].Text = "Base Currency";
                    e.Row.Cells[4].Text = "Size";
                    e.Row.Cells[5].Text = "Price";
                    e.Row.Cells[6].Text = "Annual Growth in Last 6 Months";
                    e.Row.Cells[7].Text = "Annual Growth in Last 1 Year";
                    e.Row.Cells[8].Text = "Annual Growth in Last 3 Years";
                    e.Row.Cells[9].Text = "Annual Growth since Launch";
                }
            }
            else if (type == "Unit Trust")
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = "Code";
                    e.Row.Cells[1].Text = "Name";
                    e.Row.Cells[2].Text = "Launch Date";
                    e.Row.Cells[3].Text = "Base Currency";
                    e.Row.Cells[4].Text = "Size";
                    e.Row.Cells[5].Text = "Price";
                    e.Row.Cells[6].Text = "Risk/Return Ratio";
                    e.Row.Cells[7].Text = "Annual Growth in Last 6 Months";
                    e.Row.Cells[8].Text = "Annual Growth in Last 1 Year";
                    e.Row.Cells[9].Text = "Annual Growth in Last 3 Years";
                    e.Row.Cells[10].Text = "Annual Growth since Launch";
                }

            }
            else if (type == "Stock")
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = "Code";
                    e.Row.Cells[1].Text = "Name";
                    e.Row.Cells[2].Text = "Closing Price in HKD";
                    e.Row.Cells[3].Text = "Percentage Change in Price Last Trading Day";
                    e.Row.Cells[4].Text = "Price Change Last Trading Day";
                    e.Row.Cells[5].Text = "Volume of Shares Traded Last Trading Day";
                    e.Row.Cells[6].Text = "Highest Price during Preceding 52 Weeks";
                    e.Row.Cells[7].Text = "Lowest Price during Preceding 52 Weeks";
                    e.Row.Cells[8].Text = "P/E Ratio";
                    e.Row.Cells[9].Text = "Yield";

                }
            }
        }

        protected void search2_Click(object sender, EventArgs e)
        {
            ddlSort.SelectedValue = "Sort by Name (Default)";
            string type = securityType.SelectedValue;
            string securityName = securityPartialName.Text;

            if (type == "Bond")
            {
                DataTable dtBond = new DataTable();
                dtBond = myExternalData.getData("select * from [Bond] where [name] like '%" + securityName.Trim() + "%' order by [name]");
                if (dtBond == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtBond;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }
            }
            else if (type == "Unit Trust")
            {

                DataTable dtUnitTrust = new DataTable();
                dtUnitTrust = myExternalData.getData("select * from [UnitTrust] where [name] like '%" + securityName.Trim() + "%' order by [name]");
                if (dtUnitTrust == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtUnitTrust;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }

            }
            else if (type == "Stock")
            {

                DataTable dtStock = new DataTable();
                dtStock = myExternalData.getData("select * from [Stock] where [name] like '%" + securityName.Trim() + "%' order by [name]");
                if (dtStock == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtStock;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }

            }

        }

        protected void search3_Click(object sender, EventArgs e)
        {
            ddlSort.SelectedValue = "Sort by Name (Default)";
            string type = securityType.SelectedValue;
            string Code = securityCode.Text;

            if (type == "Bond")
            {
                DataTable dtBond = new DataTable();
                dtBond = myExternalData.getData("select * from [Bond] where [code] = '" + Code.Trim() + "' order by [name]");
                if (dtBond == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtBond;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }
            }
            else if (type == "Unit Trust")
            {

                DataTable dtUnitTrust = new DataTable();
                dtUnitTrust = myExternalData.getData("select * from [UnitTrust] where [code] = '" + Code.Trim() + "' order by [name]");
                if (dtUnitTrust == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtUnitTrust;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }

            }
            else if (type == "Stock")
            {

                DataTable dtStock = new DataTable();
                dtStock = myExternalData.getData("select * from [Stock] where [code] = '" + Code.Trim() + "' order by [name]");
                if (dtStock == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtStock;
                    gvSecurityDetails.DataBind();
                    result.Visible = true;
                }

            }
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddlSort.SelectedValue;

            string type = securityType.SelectedValue;
            string method = searchMethod.SelectedValue;
            string securityName = securityPartialName.Text;
            string Code = securityCode.Text;

            if (selected == "Sort by Code")
            {
                if (method == "Security Type")
                {
                    if (type == "Bond")
                    {

                        DataTable dtBond = myExternalFunctions.getSecuritiesData("bond");
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            ViewState["SortExpression"] = "name";
                            ViewState["SortDirection"] = "ASC";

                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }
                    }
                    else if (type == "Unit Trust")
                    {

                        DataTable dtUnitTrust = myExternalFunctions.getSecuritiesData("unit trust");
                        if (dtUnitTrust == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtUnitTrust;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }
                    else if (type == "Stock")
                    {

                        DataTable dtStock = myExternalFunctions.getSecuritiesData("stock");
                        if (dtStock == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtStock;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }


                }
                else if (method == "Security Type & Security Partial Name")
                {

                    if (type == "Bond")
                    {

                        DataTable dtBond = myExternalFunctions.getSecuritiesByName("bond", securityName);
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            if (securityName != "") result.Visible = true;
                        }

                    }
                    else if (type == "Unit Trust")
                    {

                        DataTable dtUnitTrust = myExternalFunctions.getSecuritiesByName("unit trust", securityName);
                        if (dtUnitTrust == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtUnitTrust;
                            gvSecurityDetails.DataBind();
                            if (securityName != "") result.Visible = true;
                        }

                    }
                    else if (type == "Stock")
                    {
                        DataTable dtStock = myExternalFunctions.getSecuritiesByName("stock", securityName);
                        if (dtStock == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtStock;
                            gvSecurityDetails.DataBind();
                            if (securityName != "") result.Visible = true;
                        }

                    }
                }
                else if (method == "Security Type & Security Code")
                {

                    if (type == "Bond")
                    {

                        DataTable dtBond = myExternalFunctions.getSecuritiesByCode("bond", Code);
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            if (Code != "") result.Visible = true;
                        }
                    }
                    else if (type == "Unit Trust")
                    {
                        DataTable dtUnitTrust = myExternalFunctions.getSecuritiesByCode("unit trust", Code);
                        if (dtUnitTrust == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtUnitTrust;
                            gvSecurityDetails.DataBind();
                            if (Code != "") result.Visible = true;
                        }
                    }
                    else if (type == "Stock")
                    {
                        DataTable dtStock = myExternalFunctions.getSecuritiesByCode("stock", Code);
                        if (dtStock == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtStock;
                            gvSecurityDetails.DataBind();
                            if (Code != "") result.Visible = true;
                        }
                    }
                }


            }
            else if (selected == "Sort by Name (Default)")
            {

                if (method == "Security Type")
                {

                    if (type == "Bond")
                    {
                        DataTable dtBond = new DataTable();
                        dtBond = myExternalData.getData("select * from [Bond] order by [name]");
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }
                    }
                    else if (type == "Unit Trust")
                    {

                        DataTable dtUnitTrust = new DataTable();
                        dtUnitTrust = myExternalData.getData("select * from [UnitTrust] order by [name]");
                        if (dtUnitTrust == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtUnitTrust;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }
                    else if (type == "Stock")
                    {

                        DataTable dtStock = new DataTable();
                        dtStock = myExternalData.getData("select * from [Stock] order by [name]");
                        if (dtStock == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtStock;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }
                }
                else if (method == "Security Type & Security Partial Name")
                {
                    if (type == "Bond")
                    {
                        DataTable dtBond = new DataTable();
                        dtBond = myExternalData.getData("select * from [Bond] where [name] like '%" + securityName.Trim() + "%' order by [name]");
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }
                    }
                    else if (type == "Unit Trust")
                    {

                        DataTable dtUnitTrust = new DataTable();
                        dtUnitTrust = myExternalData.getData("select * from [UnitTrust] where [name] like '%" + securityName.Trim() + "%' order by [name]");
                        if (dtUnitTrust == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtUnitTrust;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }
                    else if (type == "Stock")
                    {

                        DataTable dtStock = new DataTable();
                        dtStock = myExternalData.getData("select * from [Stock] where [name] like '%" + securityName.Trim() + "%' order by [name]");
                        if (dtStock == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtStock;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }


                }
                else if (method == "Security Type & Security Code")
                {

                    if (type == "Bond")
                    {
                        DataTable dtBond = new DataTable();
                        dtBond = myExternalData.getData("select * from [Bond] where [code] = '" + Code.Trim() + "' order by [name]");
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }
                    }
                    else if (type == "Unit Trust")
                    {

                        DataTable dtUnitTrust = new DataTable();
                        dtUnitTrust = myExternalData.getData("select * from [UnitTrust] where [code] = '" + Code.Trim() + "' order by [name]");
                        if (dtUnitTrust == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtUnitTrust;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }
                    else if (type == "Stock")
                    {

                        DataTable dtStock = new DataTable();
                        dtStock = myExternalData.getData("select * from [Stock] where [code] = '" + Code.Trim() + "' order by [name]");
                        if (dtStock == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtStock;
                            gvSecurityDetails.DataBind();
                            result.Visible = true;
                        }

                    }


                }
            }
        }

        protected void rblFavoriteOperation_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblAddErrorMessage.Visible = false;
            if (rblFavoriteOperation.SelectedValue.Equals("add"))
            {
                divDisplay.Visible = false;
                divAdd.Visible = true;
            }
            else if (rblFavoriteOperation.SelectedValue.Equals("display"))
            {
                displayList();
            }
        }

        protected void displayList()
        {
            divAdd.Visible = false;
            divDisplay.Visible = true;
            // Setting up the gridviews
            bindGVByType("stock");
            bindGVByType("bond");
            bindGVByType("unit trust");
        }

        protected void bindGVByType(string type)    // type = "stock", "bond", "unit trust"
        {
            // initializing variables
            GridView gv;
            if (type.Equals("stock"))
            {
                gv = gvStockFavorite;
            }
            else if (type.Equals("bond"))
            {
                gv = gvBondFavorite;
            }
            else if (type.Equals("unit trust"))
            {
                gv = gvUTFavorite;
            }
            else
            {
                return;
            }

            string sql = "SELECT f.code FROM dbo.[FavoriteSecurities] f WHERE f.accountNumber='" + accountNumber + "' AND f.[type]='" + type + "';";
            DataTable dt = myHKeInvestData.getData(sql);
            DataTable dtResult=null;
            if (dt == null) return; // sql error
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string code = Convert.ToString(row["code"]).Trim();
                    DataTable dtIntermediate = myExternalFunctions.getSecuritiesByCode(type, code);
                    if (dtIntermediate != null)
                    {
                        if (dtResult == null)
                        {
                            dtResult = dtIntermediate;
                        }
                        else {
                            foreach (DataRow r in dtIntermediate.Rows)
                            {
                                dtResult.ImportRow(r);
                            }
                        }
                    }
                }
            }
            if (dtResult == null)
            {
                gv.Visible = false;
                if (type.Equals("stock"))
                {
                    divDisplayStock.Visible = false;
                }
                else if (type.Equals("bond"))
                {
                    divDisplayBond.Visible = false;
                }
                else if (type.Equals("unit trust"))
                {
                    divDisplayUT.Visible = false;
                }
                return;
            }
            else
            {
                gv.DataSource = dtResult;
                gv.DataBind();
                gv.Visible = true;
                if (type.Equals("stock"))
                {
                    divDisplayStock.Visible = true;
                }
                else if (type.Equals("bond"))
                {
                    divDisplayBond.Visible = true;
                }
                else if (type.Equals("unit trust"))
                {
                    divDisplayUT.Visible = true;
                }
                return;
            }
        }

        protected void btnAdd_onClick(object sender, EventArgs e)
        {
            lblAddErrorMessage.Visible = false;
            // Check if the input is valid
            if (tbSecurityCode.Text.Trim().Equals("") || ddlFavoriteType.SelectedValue.Equals("0"))
            {
                lblAddErrorMessage.Text = "Invalid Input.";
                lblAddErrorMessage.Visible = true;
                return;
            }
            string code = tbSecurityCode.Text.Trim();
            string type = ddlFavoriteType.SelectedValue.Trim();
            DataTable dtTest = myExternalFunctions.getSecuritiesByCode(type, code);
            // Check if such security exists
            if (dtTest == null)
            {
                lblAddErrorMessage.Text = "No such security.";
                lblAddErrorMessage.Visible = true;
                return;
            }
            else
            {
                string sql;
                // Check if it is already exists in the database
                sql = "SELECT * FROM dbo.[FavoriteSecurities] f WHERE f.accountNumber='" + accountNumber + "' AND f.[type]='" + type + "' AND f.code ='" + code + "';";
                DataTable d = myHKeInvestData.getData(sql);
                if (d == null) return;   // sql error
                if (d.Rows.Count != 0)
                {
                    lblAddErrorMessage.Text = "Security already added.";
                    lblAddErrorMessage.Visible = true;
                    return;
                }
                // Now we are safe to add the record
                // construct the sql for inserting record
                sql = string.Format("INSERT INTO dbo.[FavoriteSecurities] VALUES	('{0}','{1}','{2}');", accountNumber, type, code);
                var myTrans = myHKeInvestData.beginTransaction();
                myHKeInvestData.setData(sql, myTrans);
                myHKeInvestData.commitTransaction(myTrans);
                lblAddErrorMessage.Text = "Successfully added.";
                lblAddErrorMessage.Visible = true;
            }
        }

        protected void lbDeleteStock_OnClick(object sender, EventArgs e)
        {
            GridViewRow gRow = (GridViewRow)((LinkButton)sender).NamingContainer;
            string code = gRow.Cells[0].Text.Trim();
            string type = "stock";
            deleteListItem(type, code);
            displayList();
        }

        protected void lbDeleteBond_OnClick(object sender, EventArgs e)
        {
            GridViewRow gRow = (GridViewRow)((LinkButton)sender).NamingContainer;
            string code = gRow.Cells[0].Text.Trim();
            string type = "bond";
            deleteListItem(type, code);
            displayList();
        }

        protected void lbDeleteUT_OnClick(object sender, EventArgs e)
        {
            GridViewRow gRow = (GridViewRow)((LinkButton)sender).NamingContainer;
            string code = gRow.Cells[0].Text.Trim();
            string type = "unit trust";
            deleteListItem(type, code);
            displayList();
        }

        protected void deleteListItem(string type,string code)
        {
            string sql = string.Format("DELETE FROM dbo.[FavoriteSecurities] WHERE accountNumber='{0}' AND code='{1}' AND [type]='{2}';",
                accountNumber,
                code,
                type
                );
            var myTrans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, myTrans);
            myHKeInvestData.commitTransaction(myTrans);
        }
    }
}