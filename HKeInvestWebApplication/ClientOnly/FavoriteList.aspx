<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FavoriteList.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.FavoriteList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Searching</h2>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>


                <div class="form-horizontal">

                    <div class="form-group" id="method" runat="server" visible="true">
                        <h4>
                            <asp:Label ID="Label1" runat="server" Text="Search Method" Style="font-weight: 700; text-decoration: underline;" AssociatedControlID="searchMethod"></asp:Label>
                        </h4>
                        <div class="col-md-4">
                            <asp:RadioButtonList ID="searchMethod" runat="server" AutoPostBack="True" OnSelectedIndexChanged="searchMethod_SelectedIndexChanged">
                                <asp:ListItem>Security Type</asp:ListItem>
                                <asp:ListItem>Security Type &amp; Security Partial Name</asp:ListItem>
                                <asp:ListItem>Security Type &amp; Security Code</asp:ListItem>
                            </asp:RadioButtonList>

                        </div>
                    </div>




                    <div class="form-group" id="type" runat="server" visible="false">
                        <h4>
                            <asp:Label ID="Label2" runat="server" Text="Security Type" Style="text-decoration: underline" AssociatedControlID="securityType"></asp:Label>
                        </h4>

                        <div class="col-md-4">
                            <asp:RadioButtonList ID="securityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="securityType_SelectedIndexChanged">
                                <asp:ListItem>Bond</asp:ListItem>
                                <asp:ListItem>Unit Trust</asp:ListItem>
                                <asp:ListItem>Stock</asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="securityType" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Security Type is required" ValidationGroup="requireType"></asp:RequiredFieldValidator>
                        </div>


                        <div class="col-md-4">
                            <asp:Button ID="search1" runat="server" Text="Search" CssClass="btn button-default" OnClick="search1_Click" ValidationGroup="requireType" />
                        </div>


                    </div>



                    <div class="form-group" id="name" runat="server" visible="false">

                        <h4>
                            <asp:Label ID="Label3" runat="server" Text="Security Partial Name" Style="text-decoration: underline" AssociatedControlID="securityPartialName"></asp:Label>
                        </h4>
                        <div class="col-md-4">
                            <asp:TextBox ID="securityPartialName" runat="server" CssClass="form-control"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="securityPartialName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Security Partial Name is required" ValidationGroup="requireName"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-4">
                            <asp:Button ID="search2" runat="server" Text="Search" CssClass="btn button-default" OnClick="search2_Click" ValidationGroup="requireName" />
                        </div>
                    </div>




                    <div class="form-group" id="code" runat="server" visible="false">
                        <h4>
                            <asp:Label ID="Label4" runat="server" Text="Security Code" Style="text-decoration: underline" AssociatedControlID="securityCode"></asp:Label>
                        </h4>
                        <div class="col-md-4">
                            <asp:TextBox ID="securityCode" runat="server" CssClass="form-control"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="securityCode" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Security Code is required" ValidationGroup="requireCode"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-4">
                            <asp:Button ID="search3" runat="server" Text="Search" CssClass="btn button-default" OnClick="search3_Click" ValidationGroup="requireCode" />
                        </div>

                    </div>



                    <div class="form-group" id="result" runat="server" visible="false">
                        <asp:Label ID="Label5" runat="server" Text="*Please indicate sorting column" ForeColor="#CC3300"></asp:Label>
                        <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                            <asp:ListItem>Sort by Name (Default)</asp:ListItem>
                            <asp:ListItem>Sort by Code</asp:ListItem>
                        </asp:DropDownList>
                        <div>
                            <asp:GridView ID="gvSecurityDetails" runat="server" OnRowDataBound="gvSecurityDetails_RowDataBound">
                            </asp:GridView>
                        </div>
                    </div>
                    <div>
                        <div>
                            <h3>Favorite Security</h3>
                            <asp:RadioButtonList ID="rblFavoriteOperation" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblFavoriteOperation_OnSelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Display/Delete" Value="display"></asp:ListItem>
                                <asp:ListItem Text="Add" Value="add"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div runat="server" id="divAdd" visible="false">
                            <asp:DropDownList ID="ddlFavoriteType" runat="server" AutoPostBack="false">
                                <asp:ListItem Value="0">Security Type</asp:ListItem>
                                <asp:ListItem Value="bond">Bond</asp:ListItem>
                                <asp:ListItem Value="stock">Stock</asp:ListItem>
                                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="tbSecurityCode" runat="server" placeholder="Security Code" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_onClick" />
                            <asp:Label ID="lblAddErrorMessage" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div runat="server" id="divDisplay" visible="false">
                            <div runat="server" id="divDisplayStock" visible="false">
                                <h4>Stock</h4>
                                <asp:GridView ID="gvStockFavorite" runat="server" Visible="false" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" />
                                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" />
                                        <asp:BoundField DataField="close" HeaderText="Close" ReadOnly="true" />
                                        <asp:BoundField DataField="changePercent" HeaderText="Change Percent" ReadOnly="true" />
                                        <asp:BoundField DataField="changeDollar" HeaderText="Change Dollar" ReadOnly="true" />
                                        <asp:BoundField DataField="volume" HeaderText="Volume" ReadOnly="true" />
                                        <asp:BoundField DataField="high" HeaderText="High" ReadOnly="true" />
                                        <asp:BoundField DataField="low" HeaderText="Low" ReadOnly="true" />
                                        <asp:BoundField DataField="peRatio" HeaderText="PE Ratio" ReadOnly="true" />
                                        <asp:BoundField DataField="yield" HeaderText="Yield" ReadOnly="true" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lbDeleteStock" OnClick="lbDeleteStock_OnClick" Text="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div runat="server" id="divDisplayBond" visible="false">
                                <h4>Bond</h4>
                                <asp:GridView ID="gvBondFavorite" runat="server" Visible="false" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" />
                                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" />
                                        <asp:BoundField DataField="launchDate" HeaderText="Launch Date" ReadOnly="true" />
                                        <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="true" />
                                        <asp:BoundField DataField="size" HeaderText="Size" ReadOnly="true" />
                                        <asp:BoundField DataField="price" HeaderText="Price" ReadOnly="true" />
                                        <asp:BoundField DataField="sixMonths" HeaderText="Six Months" ReadOnly="true" />
                                        <asp:BoundField DataField="oneYear" HeaderText="One Year" ReadOnly="true" />
                                        <asp:BoundField DataField="threeYears" HeaderText="Three Years" ReadOnly="true" />
                                        <asp:BoundField DataField="sinceLaunch" HeaderText="Since Launch" ReadOnly="true" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lbDeleteBond" OnClick="lbDeleteBond_OnClick" Text="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div runat="server" id="divDisplayUT" visible="false">
                                <h4>Unit Trust</h4>
                                <asp:GridView ID="gvUTFavorite" runat="server" Visible="false" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" />
                                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" />
                                        <asp:BoundField DataField="launchDate" HeaderText="Launch Date" ReadOnly="true" />
                                        <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="true" />
                                        <asp:BoundField DataField="size" HeaderText="Size" ReadOnly="true" />
                                        <asp:BoundField DataField="price" HeaderText="Price" ReadOnly="true" />
                                        <asp:BoundField DataField="riskReturn" HeaderText="Risk Return" ReadOnly="true" />
                                        <asp:BoundField DataField="sixMonths" HeaderText="Six Months" ReadOnly="true" />
                                        <asp:BoundField DataField="oneYear" HeaderText="One Year" ReadOnly="true" />
                                        <asp:BoundField DataField="threeYears" HeaderText="Three Years" ReadOnly="true" />
                                        <asp:BoundField DataField="sinceLaunch" HeaderText="Since Launch" ReadOnly="true" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lbDeleteUT" OnClick="lbDeleteUT_OnClick" Text="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
