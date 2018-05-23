<%@ Page Title="Security Holding Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityHoldingDetails.aspx.cs" Inherits="HKeInvestWebApplication.SecurityHoldingDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Holding Details</h2>
    <div>
        <h3>Account Overview</h3>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>
                    <asp:Label runat="server" Text="Enter account number: "></asp:Label>
                    <asp:TextBox ID="tbAccounNumber" runat="server" placeHolder="Account Number"></asp:TextBox>
                    <asp:Button ID="btnAccountNumber" runat="server" Text="Search" OnClick="btnAccountNumber_OnClick" />
                    <asp:Label ID="lbAccountNumberResult" runat="server" Visible="false"></asp:Label>
                </div>
                <div id="divDetails" runat="server" visible="false">
                    <br />
                    <div>
                        <asp:DropDownList ID="ddlOverviewCurrency" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOverviewCurrency_OnSelectedIndexChanged">
                            <asp:ListItem Value="0">Currency</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:Label ID="lblAccountNumber" runat="server" Text="Account number: "></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblClientName" runat="server" Text="Client Name: "></asp:Label>
                    </div>

                    <div>
                        <asp:Label runat="server" Visible="true" Text="Total value of securities holdings: "></asp:Label>
                        <asp:Label ID="lblTotalValue" runat="server" Visible="true"></asp:Label>
                        <br />
                        <asp:Label runat="server" Visible="true" Text="Free balance:"></asp:Label>
                        <asp:Label ID="lblBalance" runat="server" Visible="true"></asp:Label>
                    </div>
                    <div>
                        <asp:GridView ID="gvOverview" runat="server" Visible="false" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="types" HeaderText="Type" ReadOnly="True" />
                                <asp:BoundField DataField="value" HeaderText="Value Held" ReadOnly="True" />
                                <asp:BoundField DataField="lastDate" HeaderText="Date of Last Executed Order" ReadOnly="True" />
                                <asp:BoundField DataField="lastOrderValue" HeaderText="Value of Last Executed Order" ReadOnly="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <h3>Details in Types</h3>

                    <div>
                        <div>
                            <asp:Label ID="lblResultMessage" runat="server"></asp:Label>
                        </div>
                        <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                            <asp:ListItem Value="0">Security type</asp:ListItem>
                            <asp:ListItem Value="bond">Bond</asp:ListItem>
                            <asp:ListItem Value="stock">Stock</asp:ListItem>
                            <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                            <asp:ListItem Value="0">Currency</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <br />
                    <div>
                        <asp:GridView ID="gvSecurityHolding" runat="server" Visible="False" AutoGenerateColumns="False" AllowSorting="true" OnSorting="gvSecurityHolding_Sorting">
                            <Columns>
                                <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="True" />
                                <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                                <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Shares" ReadOnly="True" SortExpression="shares" />
                                <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" />
                                <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Price" ReadOnly="True" />
                                <asp:BoundField DataField="value" DataFormatString="{0:n2}" HeaderText="Value" ReadOnly="True" SortExpression="value" />
                                <asp:BoundField DataField="convertedValue" DataFormatString="{0:n2}" HeaderText="Value in" ReadOnly="True"/>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
