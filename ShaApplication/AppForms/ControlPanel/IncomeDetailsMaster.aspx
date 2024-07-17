<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="IncomeDetailsMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.IncomeDetailsMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .error-border {
            border-color: red !important;
        }
    </style>
    <script>
        $(document).ready(function () {
            $("#save").click(function (e) { e.preventDefault(); })
            $(".grid_header_btn").click(function (e) { e.preventDefault(); })
        });
        function fnIncomeDetailRowClick(row) {
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
        function fnClientClick() {
            return true;
        }
        function makeReadOnly(event) {
            event.preventDefault();
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="custom_container">
        <div class="custom_inner_container">
            <div class="pagename_header_Container">
                <h1 class="page_head">Income Details</h1>
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_add grid_header_btn" id="addBtn" onserverclick="AddIncomeDetail_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_edit grid_header_btn" id="editBtn" onserverclick="EditIncomeDetail_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_delete grid_header_btn" id="deleteBtn" onserverclick="DeleteIncomeDetail_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="IncomeDetailsGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="IncomeDetailsGridView_SetDataRowId" OnSelectedIndexChanged="IncomeDetailsGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="IncomeId" Visible="false" />
                            <asp:BoundField DataField="InvoiceRefId" HeaderText="Invoice Reference Number" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="RecivableAccTypeName" HeaderText="Account Type" />
                            <asp:BoundField DataField="IncomeDescription" HeaderText="Income Description" />
                            <asp:BoundField DataField="CreditAmount" HeaderText="Credit Amount" />
                            <asp:BoundField DataField="CreditedDate" HeaderText="Credited Date" />
                            <asp:BoundField DataField="GstRate" HeaderText="GST" />
                            <asp:BoundField DataField="AmountTypeName" HeaderText="Amount Type" />
                            <asp:BoundField DataField="UserName" HeaderText="Receiver Name" />
                            <asp:BoundField DataField="PaymentTargetType" HeaderText="Target Description" />
                        </Columns>
                        <EmptyDataTemplate>No Record Found</EmptyDataTemplate>
                        <EmptyDataRowStyle />
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
                <span runat="server" id="total_target_amount" class="header_show_amount"></span>
                <asp:LinkButton ID="lnkClose" runat="server" OnClick="BtnCancel_Click" CssClass="popup_header_close_icon" CausesValidation="false">×</asp:LinkButton>
            </div>
            <div class="popup_body">
                <div class="div_container">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <asp:HiddenField runat="server" ID="IncomeID" />
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="IsInvoice" class="col-6 col-sm-6 col-md-6 col-lg-6">Is Invoice</label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:CheckBox runat="server" ID="IsInvoice" AutoPostBack="true" OnCheckedChanged="ShowInvoiceRefDropDownList" />
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="invoiceRef" class="col-6 col-sm-6 col-md-6 col-lg-6">Invoice Reference Number<asp:Label ID="InvoiceRefIdMandatoryLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Invoice Reference Number" autocomplete="off" class="form-control" ID="InvoiceRefId" MaxLength="250"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="InvoiceRefIdRfv" ControlToValidate="InvoiceRefId" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"
                                        ClientValidationFunction="fnValidateInputField"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator runat="server" ID="InvoiceRefIdRev" ControlToValidate="InvoiceRefId" ErrorMessage="Alpha Numeric,Special Characters & spaces only allow" Font-Size="12px" ForeColor="Red" Display="Dynamic" ValidationExpression="[A-Za-z0-9_-]+"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="isProject" class="col-6 col-sm-6 col-md-6 col-lg-6">Is Project</label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:CheckBox runat="server" class="" ID="IsProject" AutoPostBack="true" OnCheckedChanged="ShowProjectNameDropDownList"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="ProjectNameDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Project Name<asp:Label ID="ProjectNameLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="ProjectNameDropDownList" AutoPostBack="true" OnSelectedIndexChanged="ProjectDropDownOnChange" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="ProjectNameDropDownListRfv" ControlToValidate="ProjectNameDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="IsGST" class="col-6 col-sm-6 col-md-6 col-lg-6">Is GST</label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:CheckBox runat="server" class="" ID="IsGST" AutoPostBack="true" OnCheckedChanged="ShowGstRateDropDownList"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="GSTDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">GST<asp:Label ID="GstLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="GSTDropDownList" required="required"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="isEmployee" class="col-6 col-sm-6 col-md-6 col-lg-6">Is Employee</label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:CheckBox runat="server" class="" ID="IsEmployee" AutoPostBack="true" OnCheckedChanged="ShowEmployeeDropDownList"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="company" class="col-6 col-sm-6 col-md-6 col-lg-6">Employee Name<asp:Label ID="EmployeeLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="EmployeeDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="EmployeeDropDownListRfv" ControlToValidate="EmployeeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="company" class="col-6 col-sm-6 col-md-6 col-lg-6">Company<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="CompanyDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="CompanyDropDownListRfv" ControlToValidate="CompanyDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="AccTypeDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Account Type<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="AccTypeDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="AccTypeDropDownListRfv" ControlToValidate="AccTypeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="AmountTypeDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Amount Type<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="AmountTypeDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="AmountTypeDropDownListRfv" ControlToValidate="AmountTypeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="CreditAmount" class="col-6 col-sm-6 col-md-6 col-lg-6">Credit Amount<span class="mandatory">*</span></label>
                                <div class="col-6 row col-sm-6 col-md-6 col-lg-6" style="margin: 0px">
                                    <span class="form-control col col-sm-2 col-md-2 col-lg-2 currency">$</span>
                                    <asp:TextBox type="number" CssClass="form-control col col-sm-10 col-md-10 col-lg-10" runat="server" autocomplete="off" class="form-control" ID="CreditAmount" onblur="formatDecimal(this)" min="0" pattern="" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="CreditAmountRfv" ControlToValidate="CreditAmount" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="CreditAmountRev" ControlToValidate="CreditAmount" ErrorMessage="Enter a valid amount (e.g.,99.00)" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="CreditedDate" class="col-6 col-sm-6 col-md-6 col-lg-6">Credited Date<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" type="date" autocomplete="off" class="form-control" ID="CreditedDate" AutoPostBack="true" OnTextChanged="CreditedDate_TextChanged" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="CreditedDateRfv" ControlToValidate="CreditedDate" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="AdminDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Receiver Name<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="AdminDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="AdminDropDownListRfv" ControlToValidate="AdminDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="IncomeDescription" class="col-6 col-sm-6 col-md-6 col-lg-6">Description<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Description" autocomplete="off" class="form-control" ID="IncomeDescription" MaxLength="250" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="IncomeDescriptionRfv" ControlToValidate="IncomeDescription" ErrorMessage="Description is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="IncomeDescriptionRev" ControlToValidate="IncomeDescription" ErrorMessage="Alpha Numeric & special characters only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[A-Za-z0-9.,_\-&%@'\s]+$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="TargetDescriptionDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Target Description<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="TargetDescriptionDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="TargetDescriptionDropDownListRfv" ControlToValidate="TargetDescriptionDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Block">
                    <ContentTemplate>
                        <asp:Button type="button" runat="server" ID="save" CssClass="popup-btn popup-btn-save btn btn-primary px-4" Text="Save" OnClick="SaveIncomeDetails" OnClientClick="fnClientClick()" />
                        <asp:Button type="button" runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel btn btn-primary px-4" Text="Cancel" OnClick="BtnCancel_Click" CausesValidation="false" UseSubmitBehavior="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="save" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <%--<asp:Button runat="server" ID="save" CssClass="popup-btn popup-btn-save btn btn-primary px-4" Text="Save" OnClick="SaveIncomeDetails" ValidationGroup="myval"/>
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel btn btn-primary px-4" Text="Cancel" OnClick="BtnCancel_Click" UseSubmitBehavior="false"/>--%>
                <%--<button runat="server" type="button" class="popup-btn popup-btn-save btn btn-primary px-4" id="save" onserverclick="SaveIncomeDetails">Save</button>
                <asp:Button runat="server" type="button" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancel_Click" class="btn btn-primary px-4" UseSubmitBehavior="false" />--%>
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Record ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteIncome_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="BtnCancel_Click" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</asp:Content>

