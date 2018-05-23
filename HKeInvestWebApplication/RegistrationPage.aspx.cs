using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication
{
    public partial class RegistrationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cvAccountNumber_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string accountNumber = AccountNumber.Text.Trim();
            string lastName = LastName.Text.Trim();
            lastName = lastName.ToUpper();
            int index = 0;
            if (accountNumber.Length == 0)
            {
                args.IsValid = false;
                return;
            }
            if (char.IsLetter(accountNumber, index))
            {
                if (accountNumber[index] != lastName[index])
                {
                    args.IsValid = false;
                    cvAccountNumber.ErrorMessage = "The account number does not match the client's last name";
                    return;
                }
                else
                {
                    ++index;
                }
            }
            else
            {
                args.IsValid = false;
                return;
            }
            if (char.IsLetter(accountNumber, index))
            {
                if (accountNumber[index] != lastName[index])
                {
                    args.IsValid = false;
                    cvAccountNumber.ErrorMessage = "The account number does not match the client's last name";
                    return;
                }
                else
                {
                    ++index;
                }
            }
            if (accountNumber.Length - index != 8)
            {
                args.IsValid = false;
                return;
            }
            for (; index < accountNumber.Length; ++index)
            {
                if (!char.IsDigit(accountNumber[index]))
                {
                    args.IsValid = false;
                    return;
                }
            }
            args.IsValid = true;
            return;
        }
    }
}