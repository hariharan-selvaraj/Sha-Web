<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="CompanyMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.CompanyMaster" EnableViewState="true" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function companyRowClick(row) {
            if (row.classList.contains("selected_row")) {
                row.classList.remove("selected_row");
                fnDisableBtn();
            } else {
                let rows = document.querySelectorAll(".clickable_row");
                rows.forEach(function (r) {
                    r.classList.remove("selected_row");
                });
                row.classList.add("selected_row");
                let rowId = row.getAttribute("data-row-id");
                document.getElementById('<%= SelectedRowIdHiddenField.ClientID %>').value = rowId;
                fnEnableBtn();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="custom_container">
        <div class="custom_inner_container">
            <div class="pagename_header_Container">
                <h1 class="page_head">Company Master
                </h1>
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" class="actions_btn grid_haction_btn btn_add" onserverclick="AddCompany_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_edit" id="editBtn" onserverclick="EditCompany_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_delete" onserverclick="DeleteCompany_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="CompanyGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" ShowHeaderWhenEmpty="true" runat="server" OnRowDataBound="SetDataRowId" OnSelectedIndexChanged="CompanyGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="CompanyId" Visible="false" />
                            <asp:BoundField DataField="CompanyRefId" HeaderText="Company Reference Number" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                            <asp:BoundField DataField="PhoneNo" HeaderText="Phone Number" />
                            <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" />
                        </Columns>
                        <EmptyDataTemplate>No Record Found</EmptyDataTemplate>
                        <EmptyDataRowStyle />
                        <%--<PagerTemplate>
                            <div class="pager">
                                <asp:LinkButton ID="lnkFirst" runat="server" CommandName="Page" CommandArgument="First" Text="First" />
                                <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Page" CommandArgument="Prev" Text="Prev" />
                                <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand" OnItemDataBound="rptPages_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPage" runat="server" CommandName="Page" CommandArgument='<%# Container.DataItem %>' Text='<%# Container.DataItem %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:LinkButton ID="lnkNext" runat="server" CommandName="Page" CommandArgument="Next" Text="Next" />
                                <asp:LinkButton ID="lnkLast" runat="server" CommandName="Page" CommandArgument="Last" Text="Last" />
                            </div>
                        </PagerTemplate>--%>
                    </asp:GridView>
                    <asp:HiddenField ID="SelectedRowIdHiddenField" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <div runat="server" id="popup_container" class="popup_outer_container">
        <div class="popup_inner_container">
            <div class="popup_header">
                <p runat="server" class="popup_header_title" id="PopupHeaderText"></p>
                <asp:LinkButton ID="lnkClose" runat="server" OnClick="btnCancel_Click" CssClass="popup_header_close_icon" CausesValidation="false">×</asp:LinkButton>
            </div>
            <div class="popup_body">
                <div class="div_container">
                    <div class="row">
                        <asp:HiddenField runat="server" ID="companyId" />
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="cName" class="col col-sm-6 col-md-6 col-lg-6">Company Name<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Company Name" autocomplete="off" class="form-control" ID="companyName" MaxLength="250"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="companyNameRfv" ControlToValidate="companyName" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="companyNameRev" ControlToValidate="companyName" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="phNo" class="col col-sm-6 col-md-6 col-lg-6">Phone Number<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Phone Number" autocomplete="off" class="form-control" min="10" max="12" MaxLength="12" onkeypress="return /[0-9]/i.test(event.key)" ID="phNo"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="phNoRfv" ControlToValidate="phNo" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="phNoRev" ControlToValidate="phNo" ErrorMessage="This field allow 10 to 12 digit numbers." Font-Size="12px" ForeColor="Red" Display="Dynamic" ValidationExpression="^\d{10,12}$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="address" class="col col-sm-6 col-md-6 col-lg-6">Address<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <textarea runat="server" title="Address" autocomplete="off" class="form-control" id="address" maxlength="2000"></textarea>
                                    <asp:RequiredFieldValidator runat="server" ID="addressRfv" ControlToValidate="address" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="addressRev" ControlToValidate="address" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s,.'-]{3,100}$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="eMail" class="col col-sm-6 col-md-6 col-lg-6">Email Address<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" type="email" title="Email Address" autocomplete="off" class="form-control" ID="emailAddress" MaxLength="150" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="emailAddressRfv" ControlToValidate="emailAddress" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="emailAddressRev" ControlToValidate="emailAddress" ErrorMessage="Invalid Email Address." ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="cType" class="col col-sm-6 col-md-6 col-lg-6">Company Type<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="companyTypeDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="companyTypeDropDownListRfv" ControlToValidate="companyTypeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="cUen" class="col col-sm-6 col-md-6 col-lg-6">Company UEN<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Company UEN" autocomplete="off" class="form-control" ID="companyUen" MaxLength="12" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="companyUenRfv" ControlToValidate="companyUen" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="companyUenRev" ControlToValidate="companyUen" ErrorMessage="Alpha Numeric & Max Length 12 only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[a-zA-Z0-9]+"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-12 col-lg-12" id="left_align">
                                <label for="pCode" class="col-6 col-sm-6 col-md-3 col-lg-3">Postal Code<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-3 col-lg-3">
                                    <asp:TextBox runat="server" type="number" title="Postal Code" autocomplete="off" class="form-control" ID="postalCode" min="1" MaxLength="25" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="postalCodeRfv" ControlToValidate="postalCode" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="postalCodeRev" ControlToValidate="postalCode" ErrorMessage="This field allow 6 digit numbers only." ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="\d{6}"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" ID="save" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveCompanyDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="btnCancel_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Company ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteCompany_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
