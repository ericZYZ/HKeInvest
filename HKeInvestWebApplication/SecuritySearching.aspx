<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecuritySearching.aspx.cs" Inherits="HKeInvestWebApplication.SecuritySearching" %>

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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="securityType" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Security Type is required" ValidationGroup="requireType" Display="Dynamic"></asp:RequiredFieldValidator>
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
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="securityPartialName" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security Partial Name cannot contains special symbols " ValidationExpression="^[a-zA-Z0-9.\s]*$" ValidationGroup="requireName"></asp:RegularExpressionValidator>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="securityPartialName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Security Partial Name is required" ValidationGroup="requireName" Display="Dynamic"></asp:RequiredFieldValidator>
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
                            <asp:TextBox ID="securityCode" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="securityCode" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security Code can only contains digits" ValidationExpression="^[0-9]*$" ValidationGroup="requireCode"></asp:RegularExpressionValidator>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="securityCode" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Security Code is required" ValidationGroup="requireCode" Display="Dynamic"></asp:RequiredFieldValidator>
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
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
