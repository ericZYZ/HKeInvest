<%@ Page Title="Account Application" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountApplication.aspx.cs" Inherits="HKeInvestWebApplication.AccountApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-md-offset-0 col-md-12">
                <asp:Label runat="server" Text="Account Type: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-12">
                <asp:RadioButtonList ID="AccountType" runat="server" RepeatDirection="Vertical" OnSelectedIndexChanged="AccountType_SelectedIndexChanged" AutoPostBack="true" CausesValidation="false">
                    <asp:ListItem Text="Individual" Value="individual" />
                    <asp:ListItem Text="Tenants with Rights of Survivorship" Value="survivorship" />
                    <asp:ListItem Text="Joint Tenants in Common" Value="common" />
                </asp:RadioButtonList>
            </div>
        </div>
        <!--
        <asp:ValidationSummary runat="server" CssClass="text-danger" EnableClientScript="false" />
        -->
    </div>
    <div class="row">
        <div class="col-md-6">
            <div runat="server" visible="false" id="PrimaryAccountHolder" class="form-horizontal">
                <h4>Primary Account Holder</h4>
                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server" Text="Title: " CssClass="control-label"></asp:Label>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-8">
                        <asp:RadioButtonList ID="TitlePrimary" runat="server" RepeatColumns="4" Width="100%">
                            <asp:ListItem>Mr.</asp:ListItem>
                            <asp:ListItem>Mrs.</asp:ListItem>
                            <asp:ListItem>Ms.</asp:ListItem>
                            <asp:ListItem>Dr.</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select your title"
                            ControlToValidate="TitlePrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="FirstNamePrimary" runat="server" Text="FirstName" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-8">
                        <asp:TextBox ID="FirstNamePrimary" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="FirstName is required" ControlToValidate="FirstNamePrimary" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="LastNamePrimary" runat="server" Text="LastName" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="LastNamePrimary" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="LastName is required" ControlToValidate="LastNamePrimary" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="DateOfBirthPrimary" runat="server" Text="Date of Birth" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="DateOfBirthPrimary" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Date of Birth is required" ControlToValidate="DateOfBirthPrimary" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ErrorMessage="Date of birth is not valid" ControlToValidate="DateOfBirthPrimary" CssClass="text-danger" EnableClientScript="false" ForeColor="Red"
                            ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="EmailPrimary" runat="server" Text="Email" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="EmailPrimary" TextMode="Email" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ControlToValidate="EmailPrimary" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server" Text="Home address (cannot be a PO box):" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="BuildingPrimary" runat="server" Text="Building (if any): " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="BuildingPrimary" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="StreetPrimary" runat="server" Text="Street: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="StreetPrimary" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Street is required" ControlToValidate="StreetPrimary" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="DistrictPrimary" runat="server" Text="District: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="DistrictPrimary" runat="server" CssClass="form-control" MaxLength="19"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="District is required" ControlToValidate="DistrictPrimary" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="HomePhonePrimary" runat="server" Text="Home phone: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="HomePhonePrimary" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid phone number"
                            ControlToValidate="HomePhonePrimary"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="HomeFaxPrimary" runat="server" Text="Home fax: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="HomeFaxPrimary" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid fax number"
                            ControlToValidate="HomeFaxPrimary"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="BusinessPhonePrimary" runat="server" Text="Business phone: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="BusinessPhonePrimary" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid phone number"
                            ControlToValidate="BusinessPhonePrimary"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="MobilePhonePrimary" runat="server" Text="Mobile phone: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="MobilePhonePrimary" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid phone number"
                            ControlToValidate="MobilePhonePrimary"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-10">
                        <asp:CustomValidator
                            ID="cvPhonePrimary"
                            runat="server"
                            ErrorMessage="Please provide at least one phone number"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            OnServerValidate="cvPhonePrimary_ServerValidate">
                        </asp:CustomValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="CountryOfCitizenshipPrimary" runat="server" Text="Country of citizenship: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="CountryOfCitizenshipPrimary" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Country of citizenship is required"
                            ControlToValidate="CountryOfCitizenshipPrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="CountryOfLegalResidencePrimary" runat="server" Text="Country of legal residence: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="CountryOfLegalResidencePrimary" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Country of legal residence is required"
                            ControlToValidate="CountryOfLegalResidencePrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="HKIDPrimary" runat="server" Text="HKID/Passport#" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="HKIDPrimary" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="HKID/Passport# is required"
                            ControlToValidate="HKIDPrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="CountryOfIssuePrimary" runat="server" Text="Passport country of issue: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="CountryOfIssuePrimary" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Passport country of issue is required"
                            ControlToValidate="CountryOfIssuePrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>


                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server" Text="Employment status: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-8">
                        <asp:RadioButtonList
                            ID="EmploymentStatusPrimary" runat="server" RepeatColumns="3" Width="100%">
                            <asp:ListItem>Employed</asp:ListItem>
                            <asp:ListItem>Self-employed</asp:ListItem>
                            <asp:ListItem>Retired</asp:ListItem>
                            <asp:ListItem>Student</asp:ListItem>
                            <asp:ListItem>Not Employed</asp:ListItem>
                            <asp:ListItem>Homemaker</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select your employment status"
                            ControlToValidate="EmploymentStatusPrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div runat="server" id="EmploymentDetailsPrimary">
                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="OccupationPrimary" runat="server" Text="Specific occupation: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="OccupationPrimary" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="YearWithEmployerPrimary" runat="server" Text="Years with employer: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="YearWithEmployerPrimary" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid year"
                                ControlToValidate="YearWithEmployerPrimary"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{1,2})$">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="EmployerNamePrimary" runat="server" Text="Employer name: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="EmployerNamePrimary" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>

                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="EmployerPhonePrimary" runat="server" Text="Employer phone: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="EmployerPhonePrimary" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid phone number"
                                ControlToValidate="EmployerPhonePrimary"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{8,})$">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="NatureOfBusinessPrimary" runat="server" Text="Nature of business: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="NatureOfBusinessPrimary" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            <asp:CustomValidator
                                ID="cvEmploymentDetailsPrimary"
                                runat="server"
                                ErrorMessage="Please provide all employment information"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                OnServerValidate="cvEmploymentDetailsPrimary_ServerValidate">
                            </asp:CustomValidator>
                        </div>
                    </div>
                </div>


                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server"
                            Text="Are you employed by a registered securities broker/dealer, investment advisor, bank or other financial institution?"
                            CssClass="control-label">
                        </asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="EmployedByFinancialInstitutionPrimary" runat="server">
                            <asp:ListItem>No</asp:ListItem>
                            <asp:ListItem>Yes</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select an option"
                            ControlToValidate="EmployedByFinancialInstitutionPrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server"
                            Text="Are you a director, 10% shareholder or policy-making officer of a publicly traded company?"
                            CssClass="control-label">
                        </asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="PubliclyTradedCompanyPrimary" runat="server">
                            <asp:ListItem>No</asp:ListItem>
                            <asp:ListItem>Yes</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select an option"
                            ControlToValidate="PubliclyTradedCompanyPrimary"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>



            </div>
        </div>

        <div class="col-md-6">
            <div runat="server" visible="false" id="CoAccountHolder" class="form-horizontal">
                <h4>Co-Account Holder</h4>
                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server" Text="Title: " CssClass="control-label"></asp:Label>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-8">
                        <asp:RadioButtonList ID="TitleCoholder" runat="server" RepeatColumns="4" Width="100%">
                            <asp:ListItem>Mr.</asp:ListItem>
                            <asp:ListItem>Mrs.</asp:ListItem>
                            <asp:ListItem>Ms.</asp:ListItem>
                            <asp:ListItem>Dr.</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select your title"
                            ControlToValidate="TitleCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="FirstNameCoholder" runat="server" Text="FirstName" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-8">
                        <asp:TextBox ID="FirstNameCoholder" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="FirstName is required" ControlToValidate="FirstNameCoholder" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="LastNameCoholder" runat="server" Text="LastName" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="LastNameCoholder" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="LastName is required" ControlToValidate="LastNameCoholder" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="DateOfBirthCoholder" runat="server" Text="Date of Birth" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="DateOfBirthCoholder" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Date of Birth is required" ControlToValidate="DateOfBirthCoholder" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ErrorMessage="Date of birth is not valid" ControlToValidate="DateOfBirthCoholder" CssClass="text-danger" EnableClientScript="false" ForeColor="Red"
                            ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="EmailCoholder" runat="server" Text="Email" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="EmailCoholder" TextMode="Email" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ControlToValidate="EmailCoholder" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server" Text="Home address (cannot be a PO box):" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="BuildingCoholder" runat="server" Text="Building (if any): " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="BuildingCoholder" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="StreetCoholder" runat="server" Text="Street: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="StreetCoholder" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="Street is required" ControlToValidate="StreetCoholder" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="DistrictCoholder" runat="server" Text="District: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="DistrictCoholder" runat="server" CssClass="form-control" MaxLength="19"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="District is required" ControlToValidate="DistrictCoholder" CssClass="text-danger" EnableClientScript="False"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="HomePhoneCoholder" runat="server" Text="Home phone: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="HomePhoneCoholder" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid phone number"
                            ControlToValidate="HomePhoneCoholder"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="HomeFaxCoholder" runat="server" Text="Home fax: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="HomeFaxCoholder" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid fax number"
                            ControlToValidate="HomeFaxCoholder"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="BusinessPhoneCoholder" runat="server" Text="Business phone: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="BusinessPhoneCoholder" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid phone number"
                            ControlToValidate="BusinessPhoneCoholder"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="MobilePhoneCoholder" runat="server" Text="Mobile phone: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="MobilePhoneCoholder" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator
                            runat="server"
                            ErrorMessage="Please enter a valid phone number"
                            ControlToValidate="MobilePhoneCoholder"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            ForeColor="Red"
                            ValidationExpression="^(\d{8,})$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-10">
                        <asp:CustomValidator
                            ID="cvPhoneCoholder"
                            runat="server"
                            ErrorMessage="Please provide at least one phone number"
                            CssClass="text-danger"
                            EnableClientScript="false"
                            OnServerValidate="cvPhoneCoholder_ServerValidate">
                        </asp:CustomValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="CountryOfCitizenshipCoholder" runat="server" Text="Country of citizenship: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="CountryOfCitizenshipCoholder" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Country of citizenship is required"
                            ControlToValidate="CountryOfCitizenshipCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="CountryOfLegalResidenceCoholder" runat="server" Text="Country of legal residence: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="CountryOfLegalResidenceCoholder" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Country of legal residence is required"
                            ControlToValidate="CountryOfLegalResidenceCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="HKIDCoholder" runat="server" Text="HKID/Passport#" CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="HKIDCoholder" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="HKID/Passport# is required"
                            ControlToValidate="HKIDCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label AssociatedControlID="CountryOfIssueCoholder" runat="server" Text="Passport country of issue: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10">
                        <asp:TextBox ID="CountryOfIssueCoholder" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Passport country of issue is required"
                            ControlToValidate="CountryOfIssueCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>


                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server" Text="Employment status: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-8">
                        <asp:RadioButtonList
                            ID="EmploymentStatusCoholder" runat="server" RepeatColumns="3" Width="100%">
                            <asp:ListItem>Employed</asp:ListItem>
                            <asp:ListItem>Self-employed</asp:ListItem>
                            <asp:ListItem>Retired</asp:ListItem>
                            <asp:ListItem>Student</asp:ListItem>
                            <asp:ListItem>Not Employed</asp:ListItem>
                            <asp:ListItem>Homemaker</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select your employment status"
                            ControlToValidate="EmploymentStatusCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div runat="server" id="EmploymentDetailsCoholder">
                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="OccupationCoholder" runat="server" Text="Specific occupation: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="OccupationCoholder" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="YearWithEmployerCoholder" runat="server" Text="Years with employer: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="YearWithEmployerCoholder" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid year"
                                ControlToValidate="YearWithEmployerCoholder"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{1,2})$">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="EmployerNameCoholder" runat="server" Text="Employer name: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="EmployerNameCoholder" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>

                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="EmployerPhoneCoholder" runat="server" Text="Employer phone: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="EmployerPhoneCoholder" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ErrorMessage="Please enter a valid phone number"
                                ControlToValidate="EmployerPhoneCoholder"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                ForeColor="Red"
                                ValidationExpression="^(\d{8,})$">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-0 col-md-12">
                            <asp:Label AssociatedControlID="NatureOfBusinessCoholder" runat="server" Text="Nature of business: " CssClass="control-label"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:TextBox ID="NatureOfBusinessCoholder" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            <asp:CustomValidator
                                ID="cvEmploymentDetailsCoholder"
                                runat="server"
                                ErrorMessage="Please provide all employment information"
                                CssClass="text-danger"
                                EnableClientScript="false"
                                OnServerValidate="cvEmploymentDetailsCoholder_ServerValidate">
                            </asp:CustomValidator>
                        </div>
                    </div>
                </div>


                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server"
                            Text="Are you employed by a registered securities broker/dealer, investment advisor, bank or other financial institution?"
                            CssClass="control-label">
                        </asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="EmployedByFinancialInstitutionCoholder" runat="server">
                            <asp:ListItem>No</asp:ListItem>
                            <asp:ListItem>Yes</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select an option"
                            ControlToValidate="EmployedByFinancialInstitutionCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server"
                            Text="Are you a director, 10% shareholder or policy-making officer of a publicly traded company?"
                            CssClass="control-label">
                        </asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="PubliclyTradedCompanyCoholder" runat="server">
                            <asp:ListItem>No</asp:ListItem>
                            <asp:ListItem>Yes</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select an option"
                            ControlToValidate="PubliclyTradedCompanyCoholder"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

            </div>

        </div>
    </div>

    <div runat="server" visible="false" id="InvestmentProfile" class="form-horizontal">
        <h4>Investment Profile (For joint accounts, please include combined amounts) </h4>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="form-group">
                    <div class="col-md-offset-0 col-md-12">
                        <asp:Label runat="server" Text="primary source of funds: " CssClass="control-label"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-8">
                        <asp:RadioButtonList ID="PrimarySourceOfFunds" runat="server" OnSelectedIndexChanged="PrimarySourceOfFunds_SelectedIndexChanged" AutoPostBack="true" CausesValidation="false">
                            <asp:ListItem>salary/wages/savings</asp:ListItem>
                            <asp:ListItem>investment/capital gains</asp:ListItem>
                            <asp:ListItem>family/relatives/inheritance</asp:ListItem>
                            <asp:ListItem>Other (describe below)</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please select your primary source of funds"
                            ControlToValidate="PrimarySourceOfFunds"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div runat="server" visible="false" id="DivOtherFunds" class="col-md-6">
                        <asp:TextBox ID="OtherFunds" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            runat="server"
                            ErrorMessage="Please indicate your funds"
                            ControlToValidate="OtherFunds"
                            EnableClientScript="false"
                            CssClass="text-danger">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="form-group">
            <div class="col-md-offset-0 col-md-12">
                <asp:Label runat="server" Text="Investment objective for this account: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-8">
                <asp:RadioButtonList ID="InvestmentObjective" runat="server">
                    <asp:ListItem>capital preservation</asp:ListItem>
                    <asp:ListItem>income</asp:ListItem>
                    <asp:ListItem>growth</asp:ListItem>
                    <asp:ListItem>speculation</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator
                    runat="server"
                    ErrorMessage="Please select your investment objective"
                    ControlToValidate="InvestmentObjective"
                    EnableClientScript="false"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-0 col-md-12">
                <asp:Label runat="server" Text="Investment knowledge: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-8">
                <asp:RadioButtonList ID="InvestmentKnowledge" runat="server">
                    <asp:ListItem>none</asp:ListItem>
                    <asp:ListItem>limited</asp:ListItem>
                    <asp:ListItem>good</asp:ListItem>
                    <asp:ListItem>extensive</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator
                    runat="server"
                    ErrorMessage="Please select your investment knowledge"
                    ControlToValidate="InvestmentKnowledge"
                    EnableClientScript="false"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-0 col-md-12">
                <asp:Label runat="server" Text="Investment experience: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-8">
                <asp:RadioButtonList ID="InvestmentExperience" runat="server">
                    <asp:ListItem>none</asp:ListItem>
                    <asp:ListItem>limited</asp:ListItem>
                    <asp:ListItem>good</asp:ListItem>
                    <asp:ListItem>extensive</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator
                    runat="server"
                    ErrorMessage="Please select your investment experience"
                    ControlToValidate="InvestmentExperience"
                    EnableClientScript="false"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-0 col-md-12">
                <asp:Label runat="server" Text="Annual income: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-8">
                <asp:RadioButtonList ID="AnnualIncome" runat="server">
                    <asp:ListItem>under HK$20,000</asp:ListItem>
                    <asp:ListItem>HK$20,001 - HK$200,000</asp:ListItem>
                    <asp:ListItem>HK$200,001 - HK$2,000,000</asp:ListItem>
                    <asp:ListItem>more than HK$2,000,000</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator
                    runat="server"
                    ErrorMessage="Please select annual income"
                    ControlToValidate="AnnualIncome"
                    EnableClientScript="false"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>


        <div class="form-group">
            <div class="col-md-offset-0 col-md-12">
                <asp:Label runat="server" Text="Approximate liquid net worth: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-8">
                <asp:RadioButtonList ID="ApproxNetWorth" runat="server">
                    <asp:ListItem>under HK$100,000</asp:ListItem>
                    <asp:ListItem>HK$100,001 - HK$1,000,000</asp:ListItem>
                    <asp:ListItem>HK$1,000,001 - HK$10,000,000</asp:ListItem>
                    <asp:ListItem>more than HK$10,000,000</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator
                    runat="server"
                    ErrorMessage="Please select approximate liquid net worth"
                    ControlToValidate="ApproxNetWorth"
                    EnableClientScript="false"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-6">
                <asp:CheckBox Text="Yes, sweep my Free Credit Balance into the Fund." runat="server" />
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-0 col-md-12">
                <asp:Label runat="server" AssociatedControlID="FreeBalance" Text="Free balance: " CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-6">
                <asp:TextBox ID="FreeBalance" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator
                    runat="server"
                    ErrorMessage="Please indicate your free balance"
                    ControlToValidate="FreeBalance"
                    EnableClientScript="false"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator
                    runat="server"
                    ErrorMessage="Please enter a valid number"
                    ControlToValidate="FreeBalance"
                    CssClass="text-danger"
                    EnableClientScript="false"
                    ForeColor="Red"
                    ValidationExpression="^(\d{1,})$">
                </asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateAccount_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
