<%@ Page Title="Sha Application" Language="C#" MasterPageFile="~/Master/LoginLayout.Master" AutoEventWireup="true" CodeBehind="loginPage.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.loginPage" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="login" runat="server">
    <div class="container-fluid login_padding">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-7 mb-5 mb-lg-0">
                    <p class="section-title pr-5"><span class="pr-2">For you</span></p>
                    <h1 class="mb-4">Work for yourself, not for  boss or for money</h1>
                    <p>
                        Whatever you do, work at it with all your heart, as working for the Lord, not for human masters, since you know that you will receive an inheritance from the God as a reward. 
                        <br />
                        Then you will get automatically all
                    </p>
                    <ul class="list-inline m-0">
                        <li class="py-2"><i class="fa fa-check text-success mr-3"></i>Opportunities </li>
                        <li class="py-2"><i class="fa fa-check text-success mr-3"></i>Recognition</li>
                        <li class="py-2"><i class="fa fa-check text-success mr-3"></i>Growth </li>

                    </ul>
                    <a href="#" class="btn btn-primary mt-4 py-2 px-4">Forgot your password?</a>
                </div>
                <div class="col-lg-5">
                    <div class="card border-0">
                        <div class="card-header bg-secondary text-center p-1">
                            <h1 class="text-white m-0">Login</h1>
                        </div>
                        <div class="card-body rounded-bottom bg-primary p-5">
                            <div class="form-group">
                                <asp:TextBox ID="txtUserName" runat="server" autocomplete="off" AutoCompleteType="None" required="required" class="form-control border-0 p-4" placeholder="IC Number"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="txtUserNameRfv" ControlToValidate="txtUserName" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                <%--<asp:RegularExpressionValidator runat="server" ID="txtUserNameRev" ControlToValidate="txtUserName" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[0-9A-Za-z\s]+"></asp:RegularExpressionValidator>--%>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtPassword" runat="server" autocomplete="off" TextMode="Password" class="form-control border-0 p-4" placeholder="Password" AutoCompleteType="None" required="required"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="txtPasswordRfv" ControlToValidate="txtPassword" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                <%--<asp:RegularExpressionValidator runat="server" ID="txtPasswordRev" ControlToValidate="txtPassword" ErrorMessage="At least one uppercase, one lowercase, one special character, and at least one numeric digit." ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).+$"></asp:RegularExpressionValidator>--%>
                            </div>
                            <div class="center-align">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" class="btn btn-secondary btn-block border-0 py-3" />
                            </div>
                            <div class="row error_msg_div center-align">
                                <asp:Label runat="server" ID="viewMessage" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
