using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;
using Microsoft.AspNet.Identity;
using System.Data;

namespace HKeInvestWebApplication.Account
{
    public partial class UpdateInformation : System.Web.UI.Page
    {
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        // HKeInvestData myHKeInvestData = new HKeInvestData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.IsInRole("Client"))
                {
                    string userName = Context.User.Identity.GetUserName();
                    string accountNumber = myHKeInvestCode.getUserAccountNumber(Session, userName);
                    lblAccountNumber.Text += accountNumber;
                    Bind_ClientInformation(accountNumber);
                }
                else if (Context.User.IsInRole("Employee"))
                {
                    txtAccountNumber.Visible = true;
                    accountSearchButton.Visible = true;
                }
                else
                {
                    // unidentified role
                    return;
                }
            }
        }

        private void Bind_ClientInformation(string accountNumber)
        {
            ViewState["accountNumber"] = accountNumber;

            SqlDataSource1.SelectParameters["accountNumber"].DefaultValue = accountNumber;
            gvClientInformation.DataBind();

            AccountInformationSqlDataSource.SelectParameters["accountNumber"].DefaultValue = accountNumber;
            dvAccountInformation.DataBind();

            ClientInformationSqlDataSource.SelectParameters["accountNumber"].DefaultValue = "0000000000";
            fvClientInformation.DataBind();
        }

        protected void accountSearch_Click(object sender, EventArgs e)
        {
            lblResult.Visible = false;
            Bind_ClientInformation(txtAccountNumber.Text.Trim());
        }

        protected void gvClientInformation_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblResult.Visible = false;

            bool isPrimary = (bool)gvClientInformation.SelectedValue;
            ViewState["isPrimary"] = isPrimary;
            string accountNumber = (string)ViewState["accountNumber"];

            ClientInformationSqlDataSource.SelectParameters["accountNumber"].DefaultValue = accountNumber;
            ClientInformationSqlDataSource.SelectParameters["isPrimary"].DefaultValue = isPrimary ? "True" : "False";
            fvClientInformation.DataBind();
        }

        protected void cvEmploymentDetails_ServerValidate(object source, ServerValidateEventArgs args)
        {
            TextBox occupation = (TextBox)fvClientInformation.FindControl("Occupation");
            TextBox years = (TextBox)fvClientInformation.FindControl("YearWithEmployer");
            TextBox name = (TextBox)fvClientInformation.FindControl("EmployerName");
            TextBox phone = (TextBox)fvClientInformation.FindControl("EmployerPhone");
            TextBox nature = (TextBox)fvClientInformation.FindControl("NatureOfBusiness");
            int selectedIndex = ((RadioButtonList)fvClientInformation.FindControl("EmploymentStatus")).SelectedIndex;
            if (selectedIndex == 0)
            {
                // all employment information should be provided
                if (string.IsNullOrWhiteSpace(occupation.Text) ||
                    string.IsNullOrWhiteSpace(years.Text) ||
                    string.IsNullOrWhiteSpace(name.Text) ||
                    string.IsNullOrWhiteSpace(phone.Text) ||
                    string.IsNullOrWhiteSpace(nature.Text))
                {
                    args.IsValid = false;
                }
            }
        }

        protected void cvPhone_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            TextBox HomePhone = (TextBox)fvClientInformation.FindControl("HomePhone");
            TextBox BusinessPhone = (TextBox)fvClientInformation.FindControl("BusinessPhone");
            TextBox MobilePhone = (TextBox)fvClientInformation.FindControl("MobilePhone");

            if (string.IsNullOrWhiteSpace(HomePhone.Text) &&
                string.IsNullOrWhiteSpace(BusinessPhone.Text) &&
                string.IsNullOrWhiteSpace(MobilePhone.Text))
            {
                args.IsValid = false;
            }
        }

        protected void Title_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rlist = (RadioButtonList)sender;
            DataView dv = (DataView)ClientInformationSqlDataSource.Select(DataSourceSelectArguments.Empty);
            switch (((string)dv[0]["title"]).Trim())
            {
                case "Mr.": rlist.SelectedIndex = 0; break;
                case "Mrs.": rlist.SelectedIndex = 1; break;
                case "Ms.": rlist.SelectedIndex = 2; break;
                case "Dr.": rlist.SelectedIndex = 3; break;
                default: break;
            }
        }

        protected void EmploymentStatus_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rlist = (RadioButtonList)sender;
            DataView dv = (DataView)ClientInformationSqlDataSource.Select(DataSourceSelectArguments.Empty);
            switch (((string)dv[0]["employmentStatus"]).Trim())
            {
                case "Employed": rlist.SelectedIndex = 0; break;
                case "Self-employed": rlist.SelectedIndex = 1; break;
                case "Retired": rlist.SelectedIndex = 2; break;
                case "Student": rlist.SelectedIndex = 3; break;
                case "Not Employed": rlist.SelectedIndex = 4; break;
                case "Homemaker": rlist.SelectedIndex = 5; break;
                default: break;
            }
        }

        protected void EmployedByFinancialInstitution_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rlist = (RadioButtonList)sender;
            DataView dv = (DataView)ClientInformationSqlDataSource.Select(DataSourceSelectArguments.Empty);
            switch (((string)dv[0]["isInFinancialInstitution"]))
            {
                case "0": rlist.SelectedIndex = 0; break;
                case "1": rlist.SelectedIndex = 1; break;
                default: break;
            }
        }

        protected void PubliclyTradedCompany_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rlist = (RadioButtonList)sender;
            DataView dv = (DataView)ClientInformationSqlDataSource.Select(DataSourceSelectArguments.Empty);
            switch (((string)dv[0]["isPubliclyTradedCompany"]))
            {
                case "0": rlist.SelectedIndex = 0; break;
                case "1": rlist.SelectedIndex = 1; break;
                default: break;
            }
        }

        protected void fvClientInformation_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            bool isPrimary = (bool)ViewState["isPrimary"];
            string accountNumber = (string)ViewState["accountNumber"];

            string isinfinancial = ((RadioButtonList)fvClientInformation.FindControl("EmployedByFinancialInstitution")).SelectedValue.Trim() == "Yes" ? "True" : "False";
            string isinpublicly = ((RadioButtonList)fvClientInformation.FindControl("PubliclyTradedCompany")).SelectedValue.Trim() == "Yes" ? "True" : "False";

            ClientInformationSqlDataSource.UpdateParameters["accountNumber"].DefaultValue = accountNumber;
            ClientInformationSqlDataSource.UpdateParameters["isPrimary"].DefaultValue = isPrimary ? "True" : "False";
            ClientInformationSqlDataSource.UpdateParameters["title"].DefaultValue = ((RadioButtonList)fvClientInformation.FindControl("Title")).SelectedValue.Trim();
            ClientInformationSqlDataSource.UpdateParameters["employmentStatus"].DefaultValue = ((RadioButtonList)fvClientInformation.FindControl("EmploymentStatus")).SelectedValue.Trim();
            ClientInformationSqlDataSource.UpdateParameters["isInFinancialInstitution"].DefaultValue = isinfinancial;
            ClientInformationSqlDataSource.UpdateParameters["isPubliclyTradedCompany"].DefaultValue = isinpublicly;
        }

        protected void fvClientInformation_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            lblResult.Visible = true;
            fvClientInformation.DataBind();
            gvClientInformation.DataBind();
        }

        protected void fvClientInformation_ModeChanged(object sender, EventArgs e)
        {
            if (fvClientInformation.CurrentMode == FormViewMode.Edit)
            {
                lblResult.Visible = false;
            }
        }

        protected void SourceOfFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = ((RadioButtonList)sender).SelectedValue;
            if (selectedValue.Trim() == "Other (describe below)")
            {
                ((TextBox)dvAccountInformation.FindControl("SourceOfFund")).ReadOnly = false;
            }
            else
            {
                ((TextBox)dvAccountInformation.FindControl("SourceOfFund")).ReadOnly = true;
                ((TextBox)dvAccountInformation.FindControl("SourceOfFund")).Text = selectedValue;
            }
        }

        protected void InvestmentObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((TextBox)dvAccountInformation.FindControl("InvestmentObjective")).Text = ((RadioButtonList)sender).SelectedValue;
        }

        protected void InvestmentKnowledge_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((TextBox)dvAccountInformation.FindControl("InvestmentKnowledge")).Text = ((RadioButtonList)sender).SelectedValue;
        }

        protected void InvestmentExperience_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((TextBox)dvAccountInformation.FindControl("InvestmentExperience")).Text = ((RadioButtonList)sender).SelectedValue;
        }

        protected void AnnualIncome_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((TextBox)dvAccountInformation.FindControl("AnnualIncome")).Text = ((RadioButtonList)sender).SelectedValue;
        }

        protected void LiquidNetWorth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((TextBox)dvAccountInformation.FindControl("LiquidNetWorth")).Text = ((RadioButtonList)sender).SelectedValue;
        }

        protected void dvAccountInformation_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            lblResult.Visible = true;
            dvAccountInformation.DataBind();
        }

        protected void dvAccountInformation_ModeChanged(object sender, EventArgs e)
        {
            if (dvAccountInformation.CurrentMode == DetailsViewMode.Edit)
            {

                lblResult.Visible = false;
            }
        }

        protected void dvAccountInformation_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            AccountInformationSqlDataSource.UpdateParameters["accountNumber"].DefaultValue = (string)ViewState["accountNumber"];
        }
    }
}