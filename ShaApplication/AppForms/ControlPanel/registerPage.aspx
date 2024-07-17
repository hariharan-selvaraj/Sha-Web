<%@ Page Title="Sha-Register" Language="C#" MasterPageFile="~/Master/LoginLayout.Master" AutoEventWireup="true" CodeBehind="registerPage.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.registerPage" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="login" runat="server">
    <div class="container register_padding">
        <div class="bg-light register_div">
            <h2 class="mb-4 register_head">Registration</h2>
            <div class="row center-align">
                <div class="col-lg-12 col-md-12 row center-align">
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="company" class="col col-sm-6 col-md-6 col-lg-6">Company<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:DropDownList runat="server" class="form-control" ID="companyDropDownList" required="required"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="companyDropDownListRfv" ControlToValidate="companyDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="passNo" class="col col-sm-6 col-md-6 col-lg-6">Passport No<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:TextBox runat="server" title="Passport Number" autocomplete="off" class="form-control" ID="passPortNo" MaxLength="8"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="passPortNoRfv" ControlToValidate="passPortNo" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="passPortNoRev" ControlToValidate="passPortNo" ErrorMessage="One uppercase letters followed by 7 digits" Font-Size="12px" ForeColor="Red" Display="Dynamic" ValidationExpression="^[A-Z]{1}[0-9]{7}"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 row center-align">
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="aName" class="col col-sm-6 col-md-6 col-lg-6">Admin Name<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:TextBox runat="server" title="Admin Name" autocomplete="off" class="form-control" ID="adminName" MaxLength="250" required="required"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="adminNameRfv" ControlToValidate="adminName" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="adminNameRev" ControlToValidate="adminName" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="icNo" class="col col-sm-6 col-md-6 col-lg-6">IC No<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:TextBox runat="server" title="Ic Number" autocomplete="off" class="form-control" ID="icNo" MaxLength="25" required="required"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="icNoRfv" ControlToValidate="icNo" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="icNoRev" ControlToValidate="icNo" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 row center-align">
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="phNo" class="col col-sm-6 col-md-6 col-lg-6">Phone No<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:TextBox runat="server" title="Phone Number" autocomplete="off" class="form-control" min="10" max="12" MaxLength="12" onkeypress="return /[0-9]/i.test(event.key)" ID="phNo"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="phNoRfv" ControlToValidate="phNo" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="phNoRev" ControlToValidate="phNo" ErrorMessage="This field allow 10 to 12 digit phone number." Font-Size="12px" ForeColor="Red" Display="Dynamic" ValidationExpression="^\d{10,12}$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="password" class="col col-sm-6 col-md-6 col-lg-6">Password<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:TextBox runat="server" TextMode="Password" title="Password" autocomplete="off" class="form-control" ID="password" MaxLength="250" required="required"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="passwordRfv" ControlToValidate="password" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="passwordRev" ControlToValidate="password" ErrorMessage="At least one uppercase, one lowercase, one special character, and at least one numeric digit." Font-Size="12px" ForeColor="Red" Display="Dynamic" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).+$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 row center-align">
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="address" class="col col-sm-6 col-md-6 col-lg-6">Address<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <textarea runat="server" title="Address" autocomplete="off" class="form-control" id="address" maxlength="2000" required="required"></textarea>
                            <asp:RequiredFieldValidator runat="server" ID="addressRfv" ControlToValidate="address" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="addressRev" ControlToValidate="address" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="cPassword" class="col col-sm-6 col-md-6 col-lg-6">Confirm Password<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:TextBox runat="server" TextMode="Password" title="Confirm Password" autocomplete="off" class="form-control" ID="cPassword" MaxLength="250" required="required"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 row center-align">
                    <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                        <label for="eMail" class="col col-sm-6 col-md-6 col-lg-6">Email Address<span class="mandatory">*</span></label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:TextBox runat="server" type="email" title="Email Address" autocomplete="off" class="form-control" ID="emailAddress" MaxLength="150" required="required"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="emailAddressRfv" ControlToValidate="emailAddress" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="emailAddressRev" ControlToValidate="emailAddress" ErrorMessage="Invalid Email Address." ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="form-group row col-12 col-sm-6 col-md-9 col-lg-6">
                        <label for="isAdmin" class="col col-sm-6 col-md-6 col-lg-6">Is Admin</label>
                        <div class="col col-sm-6 col-md-6 col-lg-6">
                            <asp:CheckBox runat="server" title="Is Admin" ID="isAdmin" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="text-align: right">
                <div class="col-lg-12 col-md-12 center-align register-div-btns">
                    <asp:Button runat="server" ID="register" Text="  Submit  " OnClick="SaveRegisterDetails" class="btn btn-primary px-3" />
                    <asp:Button runat="server" ID="clear" Text="  Clear  " OnClick="ClearRegisterDetails" class="btn btn-primary px-3" CausesValidation="false" UseSubmitBehavior="false" />
                </div>
                <div class="row error_msg_div center-align">
                    <asp:Label runat="server" ID="viewMessage" Visible="false" />
                </div>
            </div>
        </div>
    </div>
    <div runat="server" id="Success_Popup" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p runat="server" class="popup_header_title" id="NavSuccessPopupText"></p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="NavSuccessBtn" Text="Ok" CssClass="btn btn-success" OnClick="btnSuccess_login_Click" CausesValidation="false" UseSubmitBehavior="false"/>
            </div>
        </div>
    </div>
    <div runat="server" id="Failure_Popup" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p runat="server" class="popup_header_title" id="NavFailurePopupText"></p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="NavFailureBtn" Text="Ok" CssClass="btn btn-danger" OnClick="btn_Failure_Click" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('.reg_btn').hide();
            $('.back_btn').show();
        });
    </script>
</asp:Content>



