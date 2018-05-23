<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfitLoss.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ProfitLoss" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Profit/loss Tracking</h2>
    <h5>All amount in HKD</h5>
    <div class="form-horizontal">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>
                    <asp:RadioButtonList ID="TrackType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="TrackType_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Individual security" Value="individual" />
                        <asp:ListItem Text="Securities of a type" Value="givenType" />
                        <asp:ListItem Text="All securities" Value="all" />
                    </asp:RadioButtonList>
                </div>
                <br />
                <div id="Input_individual" visible="false" runat="server">
                    <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                        <asp:ListItem Value="0">Security Type</asp:ListItem>
                        <asp:ListItem Value="Bond">Bond</asp:ListItem>
                        <asp:ListItem Value="Stock">Stock</asp:ListItem>
                        <asp:ListItem Value="UnitTrust">Unit Trust</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbSecurityCode" runat="server" placeholder="Security Code" />
                    <asp:Button ID="btnTrack" runat="server" OnClick="TrackIndividualSecurity_Click" Text="Track" />
                </div>
                <br />
                <div>
                    <asp:GridView ID="gvTrackGroup" runat="server" Visible="false" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="spent" HeaderText="Spent" ReadOnly="True" />
                            <asp:BoundField DataField="gain" HeaderText="Gain" ReadOnly="True" />
                            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True" />
                            <asp:BoundField DataField="profitLoss" HeaderText="Profit/Loss" ReadOnly="True" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <asp:GridView ID="gvTrackIndividual" runat="server" Visible="false" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="true" />
                            <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" />
                            <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" />
                            <asp:BoundField DataField="shares" HeaderText="Shares" ReadOnly="True" />
                            <asp:BoundField DataField="spent" HeaderText="Spent" ReadOnly="True" />
                            <asp:BoundField DataField="gain" HeaderText="Gain" ReadOnly="True" />
                            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True" />
                            <asp:BoundField DataField="profitLoss" HeaderText="Profit/Loss" ReadOnly="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
