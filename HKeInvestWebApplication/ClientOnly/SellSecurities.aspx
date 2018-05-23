<%@ Page Title="Sell Securities" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SellSecurities.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.SellSecurities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <hr />
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label ID="lblAccountNumber" runat="server" Text="Account number: "></asp:Label>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ddlSecurityType" Text="Indicate the security type you want to sell: " CssClass="control-label"></asp:Label>
            <asp:DropDownList ID="ddlSecurityType" runat="server">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <div class="col-md-3">
                <asp:Button runat="server" OnClick="Confirm_Click" CausesValidation="false" Text="confirm" CssClass="btn btn-default" />
            </div>
        </div>
    </div>

    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal" id="SecurityInformation" runat="server" visible="false">
        <h4>security holdings</h4>
        <asp:GridView runat="server"
            ID="gvSecurityInformation"
            AutoGenerateColumns="false"
            DataKeyNames="code"
            OnSelectedIndexChanged="gvSecurityInformation_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="true" />
                <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" />
                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" />
                <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Shares" ReadOnly="True" />
                <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" />
                <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Price" ReadOnly="True" />
                <asp:BoundField DataField="value" DataFormatString="{0:n2}" HeaderText="Value" ReadOnly="True" />
                <asp:ButtonField Text="Select" HeaderText="Sell" CommandName="Select" />
            </Columns>
            <EmptyDataTemplate>
                <p class="text-danger">
                    <asp:Literal runat="server" Text="No security held"></asp:Literal>
                </p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div runat="server" visible="false" id="NormalSell" class="form-horizontal">
        <hr />
        <h4><%: ddlSecurityType.SelectedValue %> selling details</h4>
        <asp:ValidationSummary runat="server" CssClass="text-danger" EnableClientScript="false" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtShares" Text="Shares to sell" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-3">
                <asp:TextBox ID="txtShares" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="please indicate the shares to sell" ControlToValidate="txtShares" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvShares" runat="server" ErrorMessage="Invalid shares" ControlToValidate="txtShares" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvShares_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="Sell_Click" Text="Sell" CssClass="btn btn-default" />
            </div>
        </div>
    </div>

    <div runat="server" visible="false" id="StockSell" class="form-horizontal">
        <h4><%: ddlSecurityType.SelectedValue %> selling details</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" EnableClientScript="false" />

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="form-group">
                    <asp:Label AssociatedControlID="ddlStockType" runat="server" Text="stock type" CssClass="control-label col-md-3"></asp:Label>
                    <div class="col-md-3">
                        <asp:DropDownList runat="server" ID="ddlStockType" AutoPostBack="true" OnSelectedIndexChanged="ddlStockType_SelectedIndexChanged">
                            <asp:ListItem Value="0">--order type--</asp:ListItem>
                            <asp:ListItem Value="market">market order</asp:ListItem>
                            <asp:ListItem Value="limit">limit order</asp:ListItem>
                            <asp:ListItem Value="stop">stop order</asp:ListItem>
                            <asp:ListItem Value="stop limit">stop limit order</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:CustomValidator runat="server" ID="cvStockType" ErrorMessage="" ControlToValidate="ddlStockType" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvStockType_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
                </div>

                <div class="form-group" runat="server" visible="false" id="LowPrice">
                    <asp:Label AssociatedControlID="txtLowPrice" runat="server" Text="Lowest price: " CssClass="control-label col-md-3"></asp:Label>
                    <div class="col-md-3">
                        <asp:TextBox runat="server" ID="txtLowPrice"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Lowest price is required" ControlToValidate="txtLowPrice" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvLowPrice" runat="server" ErrorMessage="" ControlToValidate="txtLowPrice" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvLowPrice_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
                    </div>
                </div>

                <div class="form-group" runat="server" visible="false" id="StopPrice">
                    <asp:Label AssociatedControlID="txtStopPrice" runat="server" Text="Stop price: " CssClass="control-label col-md-3"></asp:Label>
                    <div class="col-md-3">
                        <asp:TextBox runat="server" ID="txtStopPrice"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Stop price is required" ControlToValidate="txtStopPrice" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvStopPrice" runat="server" ErrorMessage="" ControlToValidate="txtStopPrice" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvStopPrice_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtQuantity" Text="quantity of shares" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-3">
                <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="please indicate the quantity to sell" ControlToValidate="txtQuantity" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvQuantity" runat="server" ErrorMessage="" ControlToValidate="txtQuantity" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvQuantity_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ExpiryDate" Text="expiry day(1-7)" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="ExpiryDate" runat="server" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Expiry day is required" ControlToValidate="ExpiryDate" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvExpiryDate" runat="server" ErrorMessage="" ControlToValidate="ExpiryDate" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvExpiryDate_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="AllOrNone" Text="all or none" CssClass="control-label col-md-3"></asp:Label>
            <asp:CheckBox ID="AllOrNone" runat="server" />
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="Sell_Click" Text="Sell" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
