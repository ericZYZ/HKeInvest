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
    public partial class SecuritySearching : System.Web.UI.Page
    {

        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        ExternalData myExternalData = new ExternalData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState.Add("SortExpression", "name");
                ViewState.Add("SortDirection", "ASC");
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
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
            if (type == "Bond") {
                
            }
            else if (type == "Unit Trust") {

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
                    if ((RequiredFieldValidator2.IsValid==true)&& (RequiredFieldValidator3.IsValid == true)&&(RegularExpressionValidator1.IsValid==true)&& (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
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
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
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
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
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
            else if(type == "Stock") {
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
                dtBond = null;
                if ((RequiredFieldValidator2.IsValid == true) && (RegularExpressionValidator2.IsValid == true))
                { dtBond = myExternalData.getData("select * from [Bond] where [name] like '%" + securityName.Trim() + "%' order by [name]"); }
                if (dtBond == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtBond;
                    gvSecurityDetails.DataBind();
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
                }
            
            }
            else if (type == "Unit Trust")
            {

                DataTable dtUnitTrust = new DataTable();
                dtUnitTrust=null;
                if ((RequiredFieldValidator2.IsValid == true) && (RegularExpressionValidator2.IsValid == true))
                { dtUnitTrust = myExternalData.getData("select * from [UnitTrust] where [name] like '%" + securityName.Trim() + "%' order by [name]"); }
                if (dtUnitTrust == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtUnitTrust;
                    gvSecurityDetails.DataBind();
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
                }
            

            }
            else if (type == "Stock")
            {

                DataTable dtStock = new DataTable();
                dtStock = null;
                if ((RequiredFieldValidator2.IsValid == true) && (RegularExpressionValidator2.IsValid == true))
                { dtStock = myExternalData.getData("select * from [Stock] where [name] like '%" + securityName.Trim() + "%' order by [name]"); }
                if (dtStock == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtStock;
                    gvSecurityDetails.DataBind();
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
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
                dtBond = null;
                if ((RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true))
                { dtBond = myExternalData.getData("select * from [Bond] where [code] = '" + Code.Trim() + "' order by [name]"); }
                if (dtBond == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtBond;
                    gvSecurityDetails.DataBind();
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
                }
            
            }
            else if (type == "Unit Trust")
            {

                DataTable dtUnitTrust = new DataTable();
                dtUnitTrust = null;
                if ((RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true))
                { dtUnitTrust = myExternalData.getData("select * from [UnitTrust] where [code] = '" + Code.Trim() + "' order by [name]"); }
                if (dtUnitTrust == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtUnitTrust;
                    gvSecurityDetails.DataBind();
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
                }
            

            }
            else if (type == "Stock")
            {

                DataTable dtStock = new DataTable();
                dtStock = null;
                if ((RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true))
                { dtStock = myExternalData.getData("select * from [Stock] where [code] = '" + Code.Trim() + "' order by [name]"); }
                if (dtStock == null) { result.Visible = false; return; }
                else
                {
                    gvSecurityDetails.DataSource = dtStock;
                    gvSecurityDetails.DataBind();
                    if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                    else { result.Visible = false; }
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

            if (selected == "Sort by Code") {
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
                        }
                    

                    }


                }
                else if (method == "Security Type & Security Partial Name") {

                    if (type == "Bond")
                    {

                        DataTable dtBond = myExternalFunctions.getSecuritiesByName("bond", securityName);
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            if (securityName != "") { if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; } }
                            else { result.Visible = false; }
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
                            if (securityName != "") { if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; } }
                            else { result.Visible = false; }
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
                            if (securityName != "") { if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; } }
                            else { result.Visible = false; }
                        }
                    

                    }
                }
                else if (method == "Security Type & Security Code") {

                    if (type == "Bond")
                    {

                        DataTable dtBond = myExternalFunctions.getSecuritiesByCode("bond", Code);
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            if (Code != "") { if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; } }
                            else { result.Visible = false; }
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
                            if (Code != "") { if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; } }
                            else { result.Visible = false; }
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
                            if (Code != "") { if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; } }
                            else { result.Visible = false; }
                        }
                    
                    }
                }


                }
            else if (selected == "Sort by Name (Default)")
            {

                if (method == "Security Type") {

                    if (type == "Bond")
                    {
                        DataTable dtBond = new DataTable();
                        dtBond = myExternalData.getData("select * from [Bond] order by [name]");
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
                        }
                    

                    }


                }
                else if (method == "Security Type & Security Code") {

                    if (type == "Bond")
                    {
                        DataTable dtBond = new DataTable();
                        dtBond = myExternalData.getData("select * from [Bond] where [code] = '" + Code.Trim() + "' order by [name]");
                        if (dtBond == null) { result.Visible = false; return; }
                        else
                        {
                            gvSecurityDetails.DataSource = dtBond;
                            gvSecurityDetails.DataBind();
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
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
                            if ((RequiredFieldValidator2.IsValid == true) && (RequiredFieldValidator3.IsValid == true) && (RegularExpressionValidator1.IsValid == true) && (RegularExpressionValidator2.IsValid == true)) { result.Visible = true; }
                            else { result.Visible = false; }
                        }
                    

                    }


                }
            }
        }


    }
}