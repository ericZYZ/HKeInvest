<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HKeInvestWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h2 style="text-align: center">Hong Kong Electronic Investments LLC</h2>
        <p class="lead" style="text-align: center">HKeInvest Online Security Portfolio Management System</p>
        <p class="lead">
            <asp:Image ID="Image1" runat="server" ImageUrl="http://magic.wizards.com/sites/mtg/files/images/featured/GP_HongKong.jpg" Height="300px" Width="900px" />
        </p>
        
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Client Access</h2>
            <p>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/SecuritySearching.aspx">Search Securities</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/ClientOnly/BuySecurities.aspx">Buy Securities</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/ClientOnly/SellSecurities.aspx">Sell Securities</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/ClientOnly/ClientSecurityHoldingsDetails.aspx">Security Holding Details</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/ClientOnly/ReportPage.aspx">Order History &amp; Status</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/ClientOnly/ProfitLoss.aspx">Track Profit/Loss</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/ClientOnly/FavoriteList.aspx">Your Favourite List</asp:HyperLink>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Employee Access</h2>
            <p>
                <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/EmployeeOnly/AccountApplication.aspx">Create an account for client</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/EmployeeOnly/SecurityHoldingDetails.aspx">View Client Security Holdings</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="~/EmployeeOnly/EmployeeReportPage.aspx">View Client Order History &amp; Status</asp:HyperLink>
            </p>
            
        </div>
        <div class="col-md-4">
            
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            To become a client please download application form from here:
            <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="~/Content/untitled.pdf">Application Form</asp:HyperLink>
            
        </div>
    </div>

</asp:Content>
