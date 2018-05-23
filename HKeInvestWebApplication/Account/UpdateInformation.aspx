<%@ Page Title="Update Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateInformation.aspx.cs" Inherits="HKeInvestWebApplication.Account.UpdateInformation" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-md-6">
                <asp:Label ID="lblAccountNumber" runat="server" Text="Account number: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-6">
                <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" MaxLength="10" Visible="false"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-6">
                <asp:Button ID="accountSearchButton" OnClick="accountSearch_Click" runat="server" Text="Search" CssClass="btn btn-default" Visible="false" />
            </div>
        </div>
    </div>

    <div class="form-horizontal">
        <asp:GridView runat="server"
            ID="gvClientInformation"
            DataSourceID="SqlDataSource1"
            AutoGenerateColumns="false"
            DataKeyNames="isPrimary"
            OnSelectedIndexChanged="gvClientInformation_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="firstName" ReadOnly="true" HeaderText="First Name" />
                <asp:BoundField DataField="lastName" ReadOnly="true" HeaderText="Last Name" />
                <asp:BoundField DataField="dateOfBirth" ReadOnly="true" DataFormatString="{0:d}" HeaderText="Date of Birth" />
                <asp:BoundField DataField="isPrimary" ReadOnly="true" HeaderText="Primary Account Holder" />
                <asp:ButtonField Text="Details" HeaderText="Show Details" CommandName="Select" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Label runat="server" Text="Please specify a valid account number."></asp:Label>
            </EmptyDataTemplate>
        </asp:GridView>

        <div class="form-group">
            <div class="col-md-12">
                <asp:Label ID="lblResult" runat="server" CssClass="control-label" Visible="false" Text="Your information has been updated!"></asp:Label>
            </div>
        </div>
    </div>

    <asp:DetailsView runat="server"
        ID="dvAccountInformation"
        DataSourceID="AccountInformationSqlDataSource"
        AutoGenerateRows="false"
        AutoGenerateEditButton="true"
        OnItemUpdating="dvAccountInformation_ItemUpdating"
        OnItemUpdated="dvAccountInformation_ItemUpdated"
        OnModeChanged="dvAccountInformation_ModeChanged">
        <Fields>
            <asp:TemplateField HeaderText="Primary source of fund">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("sourceOfFund") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="SourceOfFund" Text='<%# Bind("sourceOfFund") %>' ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            <asp:RadioButtonList runat="server" OnSelectedIndexChanged="SourceOfFund_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem>salary/wages/savings</asp:ListItem>
                                <asp:ListItem>investment/capital gains</asp:ListItem>
                                <asp:ListItem>family/relatives/inheritance</asp:ListItem>
                                <asp:ListItem>Other (describe below)</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Investment Objective">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("investmentObjective") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="InvestmentObjective" Text='<%# Bind("investmentObjective") %>' ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="InvestmentObjective_SelectedIndexChanged">
                                <asp:ListItem>capital preservation</asp:ListItem>
                                <asp:ListItem>income</asp:ListItem>
                                <asp:ListItem>growth</asp:ListItem>
                                <asp:ListItem>speculation</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Investment Knowledge">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("investmentKnowledge") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="InvestmentKnowledge" Text='<%# Bind("investmentKnowledge") %>' ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="InvestmentKnowledge_SelectedIndexChanged">
                                <asp:ListItem>none</asp:ListItem>
                                <asp:ListItem>limited</asp:ListItem>
                                <asp:ListItem>good</asp:ListItem>
                                <asp:ListItem>extensive</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Investment Experience">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("investmentExperience") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="InvestmentExperience" Text='<%# Bind("investmentExperience") %>' ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="InvestmentExperience_SelectedIndexChanged">
                                <asp:ListItem>none</asp:ListItem>
                                <asp:ListItem>limited</asp:ListItem>
                                <asp:ListItem>good</asp:ListItem>
                                <asp:ListItem>extensive</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Annual Income">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("annualIncome") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="AnnualIncome" Text='<%# Bind("annualIncome") %>' ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="AnnualIncome_SelectedIndexChanged">
                                <asp:ListItem>under HK$20,000</asp:ListItem>
                                <asp:ListItem>HK$20,001 - HK$200,000</asp:ListItem>
                                <asp:ListItem>HK$200,001 - HK$2,000,000</asp:ListItem>
                                <asp:ListItem>more than HK$2,000,000</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Net Worth">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("liquidNetWorth") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="LiquidNetWorth" Text='<%# Bind("liquidNetWorth") %>' ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="LiquidNetWorth_SelectedIndexChanged">
                                <asp:ListItem>under HK$100,000</asp:ListItem>
                                <asp:ListItem>HK$100,001 - HK$1,000,000</asp:ListItem>
                                <asp:ListItem>HK$1,000,001 - HK$10,000,000</asp:ListItem>
                                <asp:ListItem>more than HK$10,000,000</asp:ListItem>
                            </asp:RadioButtonList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>

    <div class="form-horizontal">
        <asp:FormView
            ID="fvClientInformation"
            DataSourceID="ClientInformationSqlDataSource"
            runat="server"
            RenderOuterTable="false"
            OnItemUpdating="fvClientInformation_ItemUpdating"
            OnItemUpdated="fvClientInformation_ItemUpdated"
            OnModeChanged="fvClientInformation_ModeChanged">
            <ItemTemplate>
                <table>
                    <tr>
                        <td>title</td>
                        <td><%# Eval("title") %></td>
                    </tr>
                    <tr>
                        <td>First Name</td>
                        <td><%# Eval("firstName") %></td>
                    </tr>
                    <tr>
                        <td>Last Name</td>
                        <td><%# Eval("lastName") %></td>
                    </tr>
                    <tr>
                        <td>Date of Birth</td>
                        <td><%# Eval("dateOfBirth", "{0:d}") %></td>
                    </tr>
                    <tr>
                        <td>email</td>
                        <td><%# Eval("email") %></td>
                    </tr>
                    <tr>
                        <td>HKID/Passport No.</td>
                        <td><%# Eval("HKIDPassportNumber") %></td>
                    </tr>
                    <tr>
                        <td>Building</td>
                        <td><%# Eval("homeAddrBuilding") %></td>
                    </tr>
                    <tr>
                        <td>Street</td>
                        <td><%# Eval("homeAddrStreet") %></td>
                    </tr>
                    <tr>
                        <td>District</td>
                        <td><%# Eval("homeAddrDistrict") %></td>
                    </tr>
                    <tr>
                        <td>Home Phone</td>
                        <td><%# Eval("homePhone") %></td>
                    </tr>
                    <tr>
                        <td>Home Fax</td>
                        <td><%# Eval("homeFax") %></td>
                    </tr>
                    <tr>
                        <td>Business Phone</td>
                        <td><%# Eval("businessPhone") %></td>
                    </tr>
                    <tr>
                        <td>Mobile Phone</td>
                        <td><%# Eval("mobilePhone") %></td>
                    </tr>
                    <tr>
                        <td>Country of Citizenship</td>
                        <td><%# Eval("countryOfCitizenship") %></td>
                    </tr>
                    <tr>
                        <td>Country of Legal Residence</td>
                        <td><%# Eval("countryOfLegalResidence") %></td>
                    </tr>
                    <tr>
                        <td>Passport Issue Country</td>
                        <td><%# Eval("passportCountryOfIssue") %></td>
                    </tr>
                    <tr>
                        <td>Employment Status</td>
                        <td><%# Eval("employmentStatus") %></td>
                    </tr>
                    <tr>
                        <td>Occupation</td>
                        <td><%# Eval("specificOccupation") %></td>
                    </tr>
                    <tr>
                        <td>Years with Employer</td>
                        <td><%# Eval("yearsWithEmployer") %></td>
                    </tr>
                    <tr>
                        <td>Employer Name</td>
                        <td><%# Eval("employerName") %></td>
                    </tr>
                    <tr>
                        <td>Employer Phone</td>
                        <td><%# Eval("employerPhone") %></td>
                    </tr>
                    <tr>
                        <td>Nature of Business</td>
                        <td><%# Eval("natureOfBusiness") %></td>
                    </tr>
                    <tr>
                        <td>In Financial Institution</td>
                        <td><%# Eval("isInFinancialInstitution") %></td>
                    </tr>
                    <tr>
                        <td>In Publicly Traded Company</td>
                        <td><%# Eval("isPubliclyTradedCompany") %></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton Text="Edit" CommandName="Edit" runat="server"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>

            <EditItemTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Title: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="Title" runat="server" RepeatColumns="4" Width="100%"
                                OnDataBinding="Title_DataBinding">
                                <asp:ListItem>Mr.</asp:ListItem>
                                <asp:ListItem>Mrs.</asp:ListItem>
                                <asp:ListItem>Ms.</asp:ListItem>
                                <asp:ListItem>Dr.</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="FirstName" runat="server" Text="FirstName" CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="FirstName" runat="server" CssClass="form-control" MaxLength="35" Text='<%# Bind("firstName") %>'
                                ReadOnly='<%# !Context.User.IsInRole("Employee") %>' onFocus="this.select()"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="FirstName is required" ControlToValidate="FirstName" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="LastName" runat="server" Text="LastName" CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="LastName" runat="server" CssClass="form-control" MaxLength="35" Text='<%# Bind("lastName") %>'
                                ReadOnly='<%# !Context.User.IsInRole("Employee") %>' onFocus="this.select()"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="LastName is required" ControlToValidate="LastName" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="DateOfBirth" runat="server" Text="Date of Birth" CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="DateOfBirth" runat="server" CssClass="form-control" MaxLength="10" Text='<%# Bind("dateOfBirth", "{0:d}") %>'
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="Email" runat="server" Text="Email" CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Email" TextMode="Email" runat="server" CssClass="form-control" MaxLength="30" Text='<%# Bind("email") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ControlToValidate="Email" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="Building" runat="server" Text="Building: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Building" runat="server" CssClass="form-control" MaxLength="50" Text='<%# Bind("homeAddrBuilding") %>'
                                onFocus="this.select()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="Street" runat="server" Text="Street: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Street" runat="server" CssClass="form-control" MaxLength="35" Text='<%# Bind("homeAddrStreet") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="Street is required" ControlToValidate="Street" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="District" runat="server" Text="District: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="District" runat="server" CssClass="form-control" MaxLength="19" Text='<%# Bind("homeAddrDistrict") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="District is required" ControlToValidate="District" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="HomePhone" runat="server" Text="Home phone: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="HomePhone" runat="server" CssClass="form-control" MaxLength="8" Text='<%# Bind("homePhone") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid phone number"
                                ControlToValidate="HomePhone"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{8,})$">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="HomeFax" runat="server" Text="Home fax: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="HomeFax" runat="server" CssClass="form-control" MaxLength="8" Text='<%# Bind("homeFax") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid fax number"
                                ControlToValidate="HomeFax"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{8,})$">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="BusinessPhone" runat="server" Text="Business phone: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessPhone" runat="server" CssClass="form-control" MaxLength="8" Text='<%# Bind("businessPhone") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid phone number"
                                ControlToValidate="BusinessPhone"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{8,})$">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="MobilePhone" runat="server" Text="Mobile phone: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="MobilePhone" runat="server" CssClass="form-control" MaxLength="8" Text='<%# Bind("mobilePhone") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid phone number"
                                ControlToValidate="MobilePhone"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{8,})$">
                            </asp:RegularExpressionValidator>
                            <asp:CustomValidator
                                ID="cvPhone"
                                runat="server"
                                ErrorMessage="Please provide at least one phone number"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                OnServerValidate="cvPhone_ServerValidate">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="CountryOfCitizenship" runat="server" Text="Country of citizenship: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="CountryOfCitizenship" runat="server" CssClass="form-control" MaxLength="70" Text='<%# Bind("countryOfCitizenship") %>'
                                ReadOnly='<%# !Context.User.IsInRole("Employee") %>' onFocus="this.select()"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                runat="server"
                                ErrorMessage="Country of citizenship is required"
                                ControlToValidate="CountryOfCitizenship"
                                EnableClientScript="false"
                                CssClass="text-danger">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="CountryOfLegalResidence" runat="server" Text="Country of legal residence: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="CountryOfLegalResidence" runat="server" CssClass="form-control" MaxLength="70" Text='<%# Bind("countryOfLegalResidence") %>'
                                ReadOnly='<%# !Context.User.IsInRole("Employee") %>' onFocus="this.select()"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                runat="server"
                                ErrorMessage="Country of legal residence is required"
                                ControlToValidate="CountryOfLegalResidence"
                                EnableClientScript="false"
                                CssClass="text-danger">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="HKID" runat="server" Text="HKID/Passport#" CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="HKID" runat="server" CssClass="form-control" MaxLength="8" Text='<%# Bind("HKIDPassportNumber") %>'
                                ReadOnly="true"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                runat="server"
                                ErrorMessage="HKID/Passport# is required"
                                ControlToValidate="HKID"
                                EnableClientScript="false"
                                CssClass="text-danger">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="CountryOfIssue" runat="server" Text="Passport country of issue: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="CountryOfIssue" runat="server" CssClass="form-control" MaxLength="70" Text='<%# Bind("passportCountryOfIssue") %>'
                                ReadOnly='<%# !Context.User.IsInRole("Employee") %>' onFocus="this.select()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Employment status: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList
                                ID="EmploymentStatus" runat="server" RepeatColumns="3" Width="100%"
                                OnDataBinding="EmploymentStatus_DataBinding">
                                <asp:ListItem>Employed</asp:ListItem>
                                <asp:ListItem>Self-employed</asp:ListItem>
                                <asp:ListItem>Retired</asp:ListItem>
                                <asp:ListItem>Student</asp:ListItem>
                                <asp:ListItem>Not Employed</asp:ListItem>
                                <asp:ListItem>Homemaker</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="Occupation" runat="server" Text="Specific occupation: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Occupation" runat="server" CssClass="form-control" MaxLength="20" Text='<%# Bind("specificOccupation") %>'
                                onFocus="this.select()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="YearWithEmployer" runat="server" Text="Years with employer: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="YearWithEmployer" runat="server" CssClass="form-control" MaxLength="2" Text='<%# Bind("yearsWithEmployer") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid year"
                                ControlToValidate="YearWithEmployer"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{1,2})$">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="EmployerName" runat="server" Text="Employer name: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="EmployerName" runat="server" CssClass="form-control" MaxLength="25" Text='<%# Bind("employerName") %>'
                                onFocus="this.select()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="EmployerPhone" runat="server" Text="Employer phone: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="EmployerPhone" runat="server" CssClass="form-control" MaxLength="8" Text='<%# Bind("employerPhone") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid phone number"
                                ControlToValidate="EmployerPhone"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{8,})$">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label AssociatedControlID="NatureOfBusiness" runat="server" Text="Nature of business: " CssClass="control-label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="NatureOfBusiness" runat="server" CssClass="form-control" MaxLength="20" Text='<%# Bind("natureOfBusiness") %>'
                                onFocus="this.select()"></asp:TextBox>
                            <asp:CustomValidator
                                ID="cvEmploymentDetails"
                                runat="server"
                                ErrorMessage="Please provide all employment information"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                OnServerValidate="cvEmploymentDetails_ServerValidate">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server"
                                Text="Are you in financial institution?"
                                CssClass="control-label">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="EmployedByFinancialInstitution" runat="server" RepeatColumns="2" Width="100%"
                                OnDataBinding="EmployedByFinancialInstitution_DataBinding">
                                <asp:ListItem>No</asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server"
                                Text="Are you in a publicly traded company?"
                                CssClass="control-label">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="PubliclyTradedCompany" runat="server" RepeatColumns="2" Width="100%"
                                OnDataBinding="PubliclyTradedCompany_DataBinding">
                                <asp:ListItem>No</asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton
                                Text="Update"
                                CommandName="Update"
                                runat="server" />
                            &nbsp;
                            <asp:LinkButton
                                Text="Cancel"
                                CommandName="Cancel"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </EditItemTemplate>
        </asp:FormView>
    </div>

    <asp:SqlDataSource ID="AccountInformationSqlDataSource"
        SelectCommand="SELECT * FROM Account WHERE accountNumber=@accountNumber"
        UpdateCommand="UPDATE Account SET sourceOfFund=@sourceOfFund, investmentObjective=@investmentObjective, investmentKnowledge=@investmentKnowledge, investmentExperience=@investmentExperience, annualIncome=@annualIncome, liquidNetWorth=@liquidNetWorth WHERE accountNumber=@accountNumber"
        ConnectionString="<%$ ConnectionStrings:HKeInvestConnectionString %>"
        runat="server">
        <SelectParameters>
            <asp:Parameter Name="accountNumber" Type="string" DefaultValue="0000000000" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="accountNumber" Type="string" DefaultValue="0000000000" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource1"
        SelectCommand="SELECT firstName, lastName, dateofBirth, isPrimary FROM Client WHERE accountNumber=@accountNumber"
        ConnectionString="<%$ ConnectionStrings:HKeInvestConnectionString %>"
        runat="server">
        <SelectParameters>
            <asp:Parameter Name="accountNumber" Type="string" DefaultValue="0000000000" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="ClientInformationSqlDataSource"
        SelectCommand="SELECT RTRIM(title) AS title, RTRIM(firstName) AS firstName, RTRIM(lastName) AS lastName, RTRIM(dateOfBirth) AS dateOfBirth, RTRIM(email) AS email, RTRIM(homeAddrBuilding) AS homeAddrBuilding, RTRIM(homeAddrStreet) AS homeAddrStreet, RTRIM(homeAddrDistrict) AS homeAddrDistrict, RTRIM(homePhone) AS homePhone, RTRIM(homeFax) AS homeFax, RTRIM(businessPhone) AS businessPhone, RTRIM(mobilePhone) AS mobilePhone, RTRIM(countryOfCitizenship) AS countryOfCitizenship, RTRIM(countryOfLegalResidence) AS countryOfLegalResidence, RTRIM(HKIDPassportNumber) AS HKIDPassportNumber, RTRIM(passportCountryOfIssue) AS passportCountryOfIssue, RTRIM(employmentStatus) AS employmentStatus, RTRIM(specificOccupation) AS specificOccupation, RTRIM(yearsWithEmployer) AS yearsWithEmployer, RTRIM(employerName) AS employerName, RTRIM(employerPhone) AS employerPhone, RTRIM(natureOfBusiness) AS natureOfBusiness, RTRIM(isInFinancialInstitution) AS isInFinancialInstitution, RTRIM(isPubliclyTradedCompany) AS isPubliclyTradedCompany FROM Client WHERE accountNumber=@accountNumber AND isPrimary=@isPrimary"
        UpdateCommand="UPDATE [Client] SET title=@title, firstName=@firstName, lastName=@lastName, dateOfBirth=@dateOfBirth, email=@email, homeAddrBuilding=@homeAddrBuilding, homeAddrStreet=@homeAddrStreet, homeAddrDistrict=@homeAddrDistrict, homePhone=@homePhone, homeFax=@homeFax, businessPhone=@businessPhone, mobilePhone=@mobilePhone, countryOfCitizenship=@countryOfCitizenship, countryOfLegalResidence=@countryOfLegalResidence, HKIDPassportNumber=@HKIDPassportNumber, passportCountryOfIssue=@passportCountryOfIssue, employmentStatus=@employmentStatus, specificOccupation=@specificOccupation, yearsWithEmployer=@yearsWithEmployer, employerName=@employerName, employerPhone=@employerPhone, natureOfBusiness=@natureOfBusiness, isInFinancialInstitution=@isInFinancialInstitution, isPubliclyTradedCompany=@isPubliclyTradedCompany WHERE accountNumber=@accountNumber AND isPrimary=@isPrimary"
        ConnectionString="<%$ ConnectionStrings:HKeInvestConnectionString %>"
        runat="server">
        <SelectParameters>
            <asp:Parameter Name="accountNumber" Type="string" DefaultValue="0000000000" />
            <asp:Parameter Name="isPrimary" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="accountNumber" Type="string" DefaultValue="0000000000" />
            <asp:Parameter Name="isPrimary" Type="Boolean" DefaultValue="True" />
            <asp:Parameter Name="title" Type="string" DefaultValue="Mr." />
            <asp:Parameter Name="employmentStatus" Type="string" DefaultValue="Student" />
            <asp:Parameter Name="isInFinancialInstitution" Type="Boolean" DefaultValue="True" />
            <asp:Parameter Name="isPubliclyTradedCompany" Type="Boolean" DefaultValue="True" />
        </UpdateParameters>
    </asp:SqlDataSource>

</asp:Content>
