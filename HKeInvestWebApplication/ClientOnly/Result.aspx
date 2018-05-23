<%@ Page Title="Result" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.Result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 runat="server" id="title">.</h2>
    <hr />
    <asp:Button ID="btnBack" runat="server" Text="Buy Security" CssClass="btn btn-default" Visible="false"
        OnClick="btnBack_Click"></asp:Button>

    <asp:Button ID="btnBack1" runat="server" Text="Sell Security" CssClass="btn btn-default" Visible="false"
        OnClick="btnBack1_Click"></asp:Button>

</asp:Content>
