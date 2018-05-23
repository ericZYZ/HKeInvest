using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using HKeInvestWebApplication.Models;
using System.Data;
using System.Data.SqlClient;
using HKeInvestWebApplication.Code_File;
using System.Net.Mail;
using System.Net.Mime;

namespace HKeInvestWebApplication.Account
{
    public partial class Register : Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }
            string sql = "SELECT a.userName FROM dbo.Client AS c, dbo.Account AS a WHERE c.accountNumber=a.accountNumber and RTRIM(c.firstName)='" + FirstName.Text.Trim() + "' and " +
                "RTRIM(c.lastName)='" + LastName.Text.Trim() + "' and " +
                "RTRIM(c.accountNumber)='" + AccountNumber.Text.Trim() + "' and " +
                "RTRIM(c.HKIDPassportNumber)='" + HKID.Text.Trim() + "' and " +
                "RTRIM(c.dateOfBirth)=CONVERT(date, '" + DateOfBirth.Text.Trim() + "', 103) and " +
                "RTRIM(c.email)='" + Email.Text.Trim() + "' and " +
                "c.isPrimary=(1)";

            DataTable account = myHKeInvestData.getData(sql);
            if (account.Rows.Count != 1)
            {
                ErrorMessage.Text = "user information doesn't match the account";
                return;
            }
            if (!string.IsNullOrWhiteSpace(account.Rows[0].Field<string>("userName")))
            {
                ErrorMessage.Text = "account already exists";
                return;
            }

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = UserName.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                result = manager.AddToRole(user.Id, "Client");
                if (result.Succeeded)
                {
                    var myTrans = myHKeInvestData.beginTransaction();
                    sql = "UPDATE dbo.Account SET userName='" + UserName.Text.Trim() + "' WHERE accountNumber='" + AccountNumber.Text.Trim() + "'";
                    myHKeInvestData.setData(sql, myTrans);
                    myHKeInvestData.commitTransaction(myTrans);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    string code = manager.GenerateEmailConfirmationToken(user.Id);
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    try
                    {
                        sendEmail(Email.Text.Trim(), callbackUrl);
                        signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    }
                    catch (Exception)
                    {
                        manager.Delete(user);
                        myTrans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE dbo.Account SET userName='' WHERE accountNumber='" + AccountNumber.Text.Trim() + "'";
                        myHKeInvestData.setData(sql, myTrans);
                        myHKeInvestData.commitTransaction(myTrans);

                    }
                    // manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                }
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

        private void sendEmail(string destination, string callbackUrl)
        {
            #region formatter
            string text = string.Format("Please click on this link to confirm your account: {0}", callbackUrl);
            string html = "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a><br/>";

            html += HttpUtility.HtmlEncode(@"Or copy the following link on the browser: " + callbackUrl);
            #endregion

            #region emailAccount
            string username = "comp3111_team104@cse.ust.hk";
            #endregion

            // Create an instance of MailMessage named mail.
            MailMessage mail = new MailMessage();
            // Set the sender (From), receiver (To), subject and message body fields of the mail message.
            mail.From = new MailAddress(username, "Team104 Newbee");
            mail.To.Add(destination);
            mail.Subject = "[HKeInvest] Confirm your account";
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Create an instance of SmtpClient named emailServer and set the mail server to use as "smtp.cse.ust.hk".
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");
            // Send the message.
            emailServer.Send(mail);
        }

        protected void cvAccountNumber_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            string accountNumber = AccountNumber.Text.Trim();
            string lastName = LastName.Text.Trim();
            lastName = lastName.ToUpper();
            int index = 2;
            if (accountNumber.Length != 10)
            {
                args.IsValid = false;
                return;
            }
            if (lastName.Length == 1)
            {
                if (accountNumber[0] != lastName[0] || accountNumber[1] != lastName[0])
                {

                    args.IsValid = false;
                    cvAccountNumber.ErrorMessage = "The account number does not match the client's last name";
                    return;
                }
            }
            else
            {
                if (accountNumber[0] != lastName[0] || accountNumber[1] != lastName[1])
                {
                    args.IsValid = false;
                    cvAccountNumber.ErrorMessage = "The account number does not match the client's last name";
                    return;
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