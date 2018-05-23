using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;

namespace HKeInvestWebApplication
{
    public partial class AccountApplication : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountType.SelectedIndex == 0)
            {
                CoAccountHolder.Visible = false;
                PrimaryAccountHolder.Visible = true;
                InvestmentProfile.Visible = true;
            }
            else
            {
                CoAccountHolder.Visible = true;
                PrimaryAccountHolder.Visible = true;
                InvestmentProfile.Visible = true;
            }
        }

        protected void PrimarySourceOfFunds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PrimarySourceOfFunds.SelectedIndex == 3)
            {
                DivOtherFunds.Visible = true;
            }
            else
            {
                DivOtherFunds.Visible = false;
            }
        }

        protected void cvPhonePrimary_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(HomePhonePrimary.Text) &&
                string.IsNullOrEmpty(BusinessPhonePrimary.Text) &&
                string.IsNullOrEmpty(MobilePhonePrimary.Text))
            {
                args.IsValid = false;
            }
        }

        protected void cvPhoneCoholder_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(HomePhoneCoholder.Text) &&
                string.IsNullOrEmpty(BusinessPhoneCoholder.Text) &&
                string.IsNullOrEmpty(MobilePhoneCoholder.Text))
            {
                args.IsValid = false;
            }
        }

        protected void cvEmploymentDetailsPrimary_ServerValidate(object source, ServerValidateEventArgs args)
        {
            TextBox occupation = OccupationPrimary;
            TextBox years = YearWithEmployerPrimary;
            TextBox name = EmployerNamePrimary;
            TextBox phone = EmployerPhonePrimary;
            TextBox nature = NatureOfBusinessPrimary;
            int selectedIndex = EmploymentStatusPrimary.SelectedIndex;
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

        protected void cvEmploymentDetailsCoholder_ServerValidate(object source, ServerValidateEventArgs args)
        {
            TextBox occupation = OccupationCoholder;
            TextBox years = YearWithEmployerCoholder;
            TextBox name = EmployerNameCoholder;
            TextBox phone = EmployerPhoneCoholder;
            TextBox nature = NatureOfBusinessCoholder;
            int selectedIndex = EmploymentStatusCoholder.SelectedIndex;
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

        protected void CreateAccount_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }
            string sql = "";
            // 1. generate a unique account number
            string lastName = LastNamePrimary.Text.Trim().ToUpper();
            if (lastName.Length == 1)
            {
                lastName = lastName + lastName;
            }
            else
            {
                lastName = string.Concat(lastName[0], lastName[1]);
            }
            sql = string.Format("SELECT COUNT(*) FROM dbo.Account WHERE accountNumber like '{0}%'", lastName);

            decimal newNumber = myHKeInvestData.getAggregateValue(sql) + 1;
            string newAccountNumber = lastName + newNumber.ToString("00000000");
            string accountType = AccountType.SelectedValue;
            var myTrans = myHKeInvestData.beginTransaction();

            sql = InsertAccount(newAccountNumber, accountType);
            myHKeInvestData.setData(sql, myTrans);
            myHKeInvestData.commitTransaction(myTrans);

            // 2. insert client information into client table
            // 2.1 insert primary account holder's information
            InsertPrimaryAccountHolder(newAccountNumber, true);
            // 2.2 insert co-account holder's information (if any)
            if (accountType != "individual")
            {
                InsertCoAccountHolder(newAccountNumber, false);
            }
            Response.Redirect("../Default.aspx");
        }

        private string InsertAccount(string accountNumber, string accountType)
        {
            string funds;
            if (PrimarySourceOfFunds.SelectedIndex == 3)
            {
                funds = OtherFunds.Text.Trim();
            }
            else
            {
                funds = PrimarySourceOfFunds.SelectedItem.Text.Trim();
            }
            string objective = InvestmentObjective.SelectedItem.Text.Trim();
            string knowledge = InvestmentKnowledge.SelectedItem.Text.Trim();
            string experience = InvestmentExperience.SelectedItem.Text.Trim();
            string income = AnnualIncome.SelectedItem.Text.Trim();
            string netWorth = ApproxNetWorth.SelectedItem.Text.Trim();
            decimal balance = decimal.Parse(FreeBalance.Text.Trim());

            return string.Format("INSERT INTO dbo.Account(accountNumber, accountType, balance, sourceOfFund, investmentObjective, investmentKnowledge, investmentExperience, annualIncome, liquidNetWorth) VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                accountNumber,
                accountType,
                balance,
                funds,
                objective,
                knowledge,
                experience,
                income,
                netWorth);
        }

        private void InsertPrimaryAccountHolder(string accountNumber, bool isPrimary)
        {
            string title = TitlePrimary.Text.Trim();
            string firstName = FirstNamePrimary.Text.Trim();
            string lastName = LastNamePrimary.Text.Trim();
            string dob = DateOfBirthPrimary.Text.Trim();
            string email = EmailPrimary.Text.Trim();
            string building = BuildingPrimary.Text.Trim();
            string street = StreetPrimary.Text.Trim();
            string district = DistrictPrimary.Text.Trim();
            string homePhone = HomePhonePrimary.Text.Trim();
            string homeFax = HomeFaxPrimary.Text.Trim();
            string businessPhone = BusinessPhonePrimary.Text.Trim();
            string mobilePhone = MobilePhonePrimary.Text.Trim();
            string citizenship = CountryOfCitizenshipPrimary.Text.Trim();
            string residence = CountryOfLegalResidencePrimary.Text.Trim();
            string HKID = HKIDPrimary.Text.Trim();
            string issueCountry = CountryOfIssuePrimary.Text.Trim();
            string employmentStatus = EmploymentStatusPrimary.SelectedItem.Text.Trim();
            string occupation = OccupationPrimary.Text.Trim();
            string yearsWithEmployer = YearWithEmployerPrimary.Text.Trim();
            string employerName = EmployerNamePrimary.Text.Trim();
            string employerPhone = EmployerPhonePrimary.Text.Trim();
            string businessNature = NatureOfBusinessPrimary.Text.Trim();
            string isFinancial = EmployedByFinancialInstitutionPrimary.Text.Trim();
            string isTraded = PubliclyTradedCompanyPrimary.Text.Trim();

            string sql = string.Format(
                "INSERT INTO dbo.Client VALUES ('{0}', '{1}', '{2}', CONVERT(date, '{3}', 103), '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', {23}, {24}, {25})",
                title,
                firstName,
                lastName,
                dob,
                email,
                HKID,
                accountNumber,
                building,
                street,
                district,
                homePhone,
                homeFax,
                businessPhone,
                mobilePhone,
                citizenship,
                residence,
                issueCountry,
                employmentStatus,
                occupation,
                yearsWithEmployer,
                employerName,
                employerPhone,
                businessNature,
                isFinancial == "Yes" ? "(1)" : "(0)",
                isTraded == "Yes" ? "(1)" : "(0)",
                isPrimary ? "(1)" : "(0)");

            var myTrans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, myTrans);
            myHKeInvestData.commitTransaction(myTrans);
        }

        private void InsertCoAccountHolder(string accountNumber, bool isPrimary)
        {
            string title = TitleCoholder.Text.Trim();
            string firstName = FirstNameCoholder.Text.Trim();
            string lastName = LastNameCoholder.Text.Trim();
            string dob = DateOfBirthCoholder.Text.Trim();
            string email = EmailCoholder.Text.Trim();
            string building = BuildingCoholder.Text.Trim();
            string street = StreetCoholder.Text.Trim();
            string district = DistrictCoholder.Text.Trim();
            string homePhone = HomePhoneCoholder.Text.Trim();
            string homeFax = HomeFaxCoholder.Text.Trim();
            string businessPhone = BusinessPhoneCoholder.Text.Trim();
            string mobilePhone = MobilePhoneCoholder.Text.Trim();
            string citizenship = CountryOfCitizenshipCoholder.Text.Trim();
            string residence = CountryOfLegalResidenceCoholder.Text.Trim();
            string HKID = HKIDCoholder.Text.Trim();
            string issueCountry = CountryOfIssueCoholder.Text.Trim();
            string employmentStatus = EmploymentStatusCoholder.SelectedItem.Text.Trim();
            string occupation = OccupationCoholder.Text.Trim();
            string yearsWithEmployer = YearWithEmployerCoholder.Text.Trim();
            string employerName = EmployerNameCoholder.Text.Trim();
            string employerPhone = EmployerPhoneCoholder.Text.Trim();
            string businessNature = NatureOfBusinessCoholder.Text.Trim();
            string isFinancial = EmployedByFinancialInstitutionCoholder.Text.Trim();
            string isTraded = PubliclyTradedCompanyCoholder.Text.Trim();

            string sql = string.Format(
                "INSERT INTO dbo.Client VALUES ('{0}', '{1}', '{2}', CONVERT(date, '{3}', 103), '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', {23}, {24}, {25})",
                title,
                firstName,
                lastName,
                dob,
                email,
                HKID,
                accountNumber,
                building,
                street,
                district,
                homePhone,
                homeFax,
                businessPhone,
                mobilePhone,
                citizenship,
                residence,
                issueCountry,
                employmentStatus,
                occupation,
                yearsWithEmployer,
                employerName,
                employerPhone,
                businessNature,
                isFinancial == "Yes" ? "(1)" : "(0)",
                isTraded == "Yes" ? "(1)" : "(0)",
                isPrimary ? "(1)" : "(0)");

            var myTrans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, myTrans);
            myHKeInvestData.commitTransaction(myTrans);
        }
    }
}