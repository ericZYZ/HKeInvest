<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrationPage.aspx.cs" Inherits="HKeInvestWebApplication.RegistrationPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Create a new client login account</h4>

    <div class="form-horizontal">
        <asp:ValidationSummary runat="server" CssClass="text-danger" DisplayMode="BulletList" EnableClientScript="false" ShowSummary="true"/>
        
        <div class="form-group">
            <asp:Label AssociatedControlID="FirstName" runat="server" Text="FirstName" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="FirstName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="FirstName is required" ControlToValidate="FirstName" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
            </div>

            <asp:Label AssociatedControlID="LastName" runat="server" Text="LastName" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="LastName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="LastName is required" ControlToValidate="LastName" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
            </div>

        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="AccountNumber" runat="server" Text="Account#" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="AccountNumber" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Account number is required" ControlToValidate="AccountNumber" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvAccountNumber" runat="server" ErrorMessage="Invalid account number" ControlToValidate="AccountNumber" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvAccountNumber_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
            </div>

            <asp:Label AssociatedControlID="HKID" runat="server" Text="HKID/Passport#" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="HKID" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="HKID/Passport# is required" ControlToValidate="HKID" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
            </div>

        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="DateOfBirth" runat="server" Text="Date of Birth" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="DateOfBirth" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Date of Birth is required" ControlToValidate="DateOfBirth" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="Date of birth is not valid" ControlToValidate="DateOfBirth" CssClass="text-danger" EnableClientScript="false" ForeColor="Red"
                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
            </div>

            <asp:Label AssociatedControlID="Email" runat="server" Text="Email" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Email" TextMode="Email" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ControlToValidate="Email" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
            </div>

            <hr />
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="UserName" runat="server" Text="User Name" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="UserName" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="User Name is required" ControlToValidate="Username" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="User Name must contain at least 6 characters" ControlToValidate="UserName" EnableClientScript="false" CssClass="text-danger" ForeColor="Red"
                    ValidationExpression="^.{6,}$" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="User Name must contain only letters and digits" ControlToValidate="UserName" EnableClientScript="false" CssClass="text-danger" ForeColor="Red"
                    ValidationExpression="^\w+$" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
            </div>

        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="Password" runat="server" Text="Password" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Password" TextMode="Password" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Password is required" ControlToValidate="Password" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="Password must contain at least 8 characters" ControlToValidate="Password" EnableClientScript="false" CssClass="text-danger" ForeColor="Red"
                    ValidationExpression="^.{8,}$" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="Password must contain at least 2 nonalphanumeric characters" ControlToValidate="Password" EnableClientScript="false" CssClass="text-danger" ForeColor="Red"
                    ValidationExpression=".*(\W).*(\W)" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
            </div>

            <asp:Label AssociatedControlID="ConfirmPassword" runat="server" Text="Confirm Password" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="ConfirmPassword" TextMode="Password" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Confirm Password is required" ControlToValidate="ConfirmPassword" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:CompareValidator runat="server" ErrorMessage="Password and Confirm Password do not match" ControlToCompare="Password" ControlToValidate="ConfirmPassword" CssClass="text-danger" EnableClientScript="false" Display="Dynamic" Text="*"></asp:CompareValidator>
            </div>

        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="Register" runat="server" Text="Register" CssClass="btn button-default" />
            </div>
        </div>
    </div>
</asp:Content>
