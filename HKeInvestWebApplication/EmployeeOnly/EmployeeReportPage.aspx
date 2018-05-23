<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeReportPage.aspx.cs" Inherits="HKeInvestWebApplication.EmployeeOnly.EmployeeReportPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <h2>Order status Report</h2>
            <div>
                <asp:Label runat="server" Text="Enter account number: "></asp:Label>
                <asp:TextBox ID="tbAccounNumber" runat="server" placeHolder="Account Number"></asp:TextBox>
                <asp:Button ID="btnAccountNumber" runat="server" Text="Search" OnClick="btnAccountNumber_OnClick" />
                <asp:Label ID="lbAccountNumberResult" runat="server" Visible="false"></asp:Label>
            </div>
            <div id="divReportContent" runat="server" visible="false">
                <div>
                    <h3>Active Order</h3>
                    <div>
                        <asp:GridView ID="OrderStatus" runat="server" OnSorting="OrderStatus_Sorting" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="referenceNumber" HeaderText="Reference Number" ReadOnly="True" SortExpression="referenceNumber" />
                                <asp:BoundField DataField="orderType" HeaderText="Buy/Sell" ReadOnly="True" SortExpression="orderType" />
                                <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True" SortExpression="securityType" />
                                <asp:BoundField DataField="code" HeaderText="Security Code" ReadOnly="True" SortExpression="code" />
                                <asp:BoundField DataField="name" HeaderText="Security Name" ReadOnly="True" SortExpression="name" />
                                <asp:BoundField DataField="orderDate" HeaderText="Order Date" ReadOnly="True" SortExpression="orderDate" />
                                <asp:BoundField DataField="orderStatus" HeaderText="Current Status" ReadOnly="True" />
                                <asp:BoundField DataField="dollarAmount" HeaderText="Buy Amount(HKD)" ReadOnly="True" />
                                <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Shares" ReadOnly="True" SortExpression="shares" />
                                <asp:BoundField DataField="limitOrderPrice" HeaderText="Highest Buying/Lowest Selling(HKD)" ShowHeader="False" />
                                <asp:BoundField DataField="stopOrderPrice" HeaderText="Stop Price(HKD)" ReadOnly="True" />
                                <asp:BoundField DataField="expiryDate" HeaderText="Expiry Day" ReadOnly="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <br />
                <div>
                    <h3>Order History</h3>


                    <div class="form-group">
                        <div>
                            <h4>Select the starting date</h4>
                            <asp:Calendar ID="FromDate" runat="server" OnSelectionChanged="FromDate_SelectionChanged"></asp:Calendar>
                            <asp:Button ID="btnClearFrom" runat="server" OnClick="btnClearFrom_OnClick" Text="Clear Date" />
                        </div>
                        <div>
                            <h4>Select the ending date</h4>
                            <asp:Calendar ID="ToDate" runat="server" OnSelectionChanged="ToDate_SelectionChanged"></asp:Calendar>
                            <asp:Button ID="btnClearTo" runat="server" OnClick="btnClearTo_OnClick" Text="Clear Date" />
                        </div>
                    </div>
                    <br />
                    <div>
                        <asp:DropDownList ID="ddlBuyOrSell" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBuyOrSell_OnSelectedIndexChanged">
                            <asp:ListItem Value="0">Buy or Sell</asp:ListItem>
                            <asp:ListItem Value="Buy">Buy</asp:ListItem>
                            <asp:ListItem Value="Sell">Sell</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged">
                            <asp:ListItem Value="0">Status</asp:ListItem>
                            <asp:ListItem Value="pending">Pending</asp:ListItem>
                            <asp:ListItem Value="partial">Partial</asp:ListItem>
                            <asp:ListItem Value="completed">Completed</asp:ListItem>
                            <asp:ListItem Value="cancelled">Cancelled</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecurityType_OnSelectedIndexChanged">
                            <asp:ListItem Value="0">Security type</asp:ListItem>
                            <asp:ListItem Value="bond">Bond</asp:ListItem>
                            <asp:ListItem Value="stock">Stock</asp:ListItem>
                            <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="tbCode" runat="server" placeholder="Security Code" />
                        <asp:Button ID="btnSpecific" runat="server" Text="Search" OnClick="btnSpecific_onClick" />
                    </div>
                    <div>
                        <br />
                        <asp:GridView ID="gvOrderHistory" runat="server" Visible="true" AutoGenerateColumns="false" OnSorting="gvOrderHistory_OnSorting" AllowSorting="true">
                            <Columns>
                                <asp:BoundField DataField="orderReferenceNumber" HeaderText="Order Reference Number" ReadOnly="true" />
                                <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="true" />
                                <asp:BoundField DataField="type" HeaderText="Security Type" ReadOnly="true" SortExpression="type" />
                                <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" />
                                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" SortExpression="name" />
                                <asp:BoundField DataField="subDate" HeaderText="Submission Date" ReadOnly="true" SortExpression="subDate" />
                                <asp:BoundField DataField="status" HeaderText="Status" ReadOnly="true" SortExpression="status" />
                                <asp:BoundField DataField="sharesExec" DataFormatString="{0:n2}" HeaderText="Shares Executed" ReadOnly="true" />
                                <asp:BoundField DataField="execDollar" DataFormatString="{0:n2}" HeaderText="Executed Dollars" ReadOnly="true" />
                                <asp:BoundField DataField="fee" DataFormatString="{0:n2}" HeaderText="Fee" ReadOnly="true" />
                                <asp:TemplateField HeaderText="View Link">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lvTransactionDetail" OnClick="lvTransactionDetail_Click" Text="Transaction Details" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />

                    <div id="divTransacionDetails" runat="server" visible="false">
                        <h4>Transaction Details</h4>
                        <asp:Button ID="btnHide" runat="server" Text="Hide" OnClick="btnHide_onClick" />
                        <asp:GridView ID="gvTransactionDetails" runat="server" Visible="false" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="orn" HeaderText="Order Refernce Number" ReadOnly="true" />
                                <asp:BoundField DataField="transactionNumber" HeaderText="Transaction Number" ReadOnly="true" />
                                <asp:BoundField DataField="dateExecuted" HeaderText="Date Executed" ReadOnly="true" />
                                <asp:BoundField DataField="sharesExecuted" DataFormatString="{0:n2}" HeaderText="Shares Executed" ReadOnly="true" />
                                <asp:BoundField DataField="priceExecuted" DataFormatString="{0:n2}" HeaderText="Price Executed" ReadOnly="true" />
                            </Columns>
                        </asp:GridView>
                    </div>


                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

