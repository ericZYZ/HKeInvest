<%@ Page Title="Buy Securities" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuySecurities.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.BuySecurities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <hr />
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label ID="lblAccountNumber" runat="server" Text="Account number: "></asp:Label>
        </div>
        <div class="form-group">
            <asp:Label ID="lblAccountBalance" runat="server" Text="Account balance: "></asp:Label>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ddlSecurityType" Text="Indicate the security type you want to buy: " CssClass="control-label"></asp:Label>
            <asp:DropDownList ID="ddlSecurityType" runat="server">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtSecurityCode" Text="Securty Code: " CssClass="control-label"></asp:Label>
            <asp:TextBox ID="txtSecurityCode" runat="server"></asp:TextBox>
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
        <h4>security information</h4>
        <asp:GridView runat="server"
            ID="gvSecurityInformation"
            AutoGenerateColumns="true">
            <EmptyDataTemplate>
                <p class="text-danger">
                    <asp:Literal runat="server" Text="No security"></asp:Literal>
                </p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div runat="server" visible="false" id="NormalBuy" class="form-horizontal">
        <hr />
        <h4><%: ddlSecurityType.SelectedValue %> buying details</h4>
        <asp:ValidationSummary runat="server" CssClass="text-danger" EnableClientScript="false" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtAmount" Text="Amount to be bought(in HKD)" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-3">
                <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="please indicate the amount to be bought" ControlToValidate="txtAmount" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvAmount" runat="server" ErrorMessage="Invalid amount" ControlToValidate="txtAmount" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvAmount_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="Buy_Click" Text="Buy" CssClass="btn btn-default" />
            </div>
        </div>
    </div>

    <div runat="server" visible="false" id="StockBuy" class="form-horizontal">
        <h4><%: ddlSecurityType.SelectedValue %> buying details</h4>
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

                <div class="form-group" runat="server" visible="false" id="HighPrice">
                    <asp:Label AssociatedControlID="txtHighPrice" runat="server" Text="Highest price: " CssClass="control-label col-md-3"></asp:Label>
                    <div class="col-md-3">
                        <asp:TextBox runat="server" ID="txtHighPrice"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Highest price is required" ControlToValidate="txtHighPrice" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvHighPrice" runat="server" ErrorMessage="" ControlToValidate="txtHighPrice" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvHighPrice_ServerValidate" Display="Dynamic" Text="*"></asp:CustomValidator>
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
                <asp:RequiredFieldValidator runat="server" ErrorMessage="please indicate the quantity to be bought" ControlToValidate="txtQuantity" CssClass="text-danger" EnableClientScript="False" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
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
                <asp:Button runat="server" OnClick="Buy_Click" Text="Buy" CssClass="btn btn-default" />
            </div>
        </div>
    </div>

</asp:Content>
