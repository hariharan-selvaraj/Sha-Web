<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="PaymentTargetMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.PaymentTargetMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function fnPaymentTargetDetailRowClick(row) {
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
                <h1 class="page_head">Payment Target Master</h1>
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_add" onserverclick="AddPaymentTarget_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_edit" id="editBtn" onserverclick="EditPaymentTarget_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_delete" id="deleteBtn" onserverclick="DeletePaymentTarget_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="PaymentTargetGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="PaymentTargetGridView_SetDataRowId" OnSelectedIndexChanged="PaymentTargetGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="PaymentTargetId" Visible="false" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="PaymentTargetType" HeaderText="Type" />
                            <asp:BoundField DataField="PaymentTargetFromDate" HeaderText="From Date" />
                            <asp:BoundField DataField="PaymentTargetToDate" HeaderText="To Date" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" />
                            <asp:BoundField DataField="PaymentTargetDescription" HeaderText="Description" />
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
                <asp:LinkButton ID="lnkClose" runat="server" OnClick="BtnCancel_Click" CssClass="popup_header_close_icon" CausesValidation="false">×</asp:LinkButton>
            </div>
            <div class="popup_body">
                <div class="div_container">
                    <div class="row">
                        <asp:HiddenField runat="server" ID="PaymentTargetId" />
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="ProjectDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Project Name<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="ProjectDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="ProjectDropDownListRfv" ControlToValidate="ProjectDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="PaymentTargetType" class="col col-sm-6 col-md-6 col-lg-6">Type<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="TargetTypeDropDownList" required="required">
                                        <asp:ListItem Text="-Select TargetType-" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                        <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="TargetTypeDropDownListRfv" ControlToValidate="TargetTypeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="PaymentTargetDate" class="col col-sm-6 col-md-6 col-lg-6">From Date<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" type="date" autocomplete="off" class="form-control" ID="PaymentTargetFromDate" AutoPostBack="true" OnTextChanged="TargetFromDate_TextChanged" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="PaymentTargetFromDateRfv" ControlToValidate="PaymentTargetFromDate" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="PaymentTargetDate" class="col col-sm-6 col-md-6 col-lg-6">To Date<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" type="date" autocomplete="off" class="form-control" ID="PaymentTargetToDate" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="PaymentTargetToDateRfv" ControlToValidate="PaymentTargetToDate" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="Amount" class="col col-sm-6 col-md-6 col-lg-6">Amount<span class="mandatory">*</span></label>
                                <div class="row col-sm-6 col-md-6 col-lg-6" style="margin: 0px">
                                    <span class="form-control col col-sm-2 col-md-2 col-lg-2 currency">$</span>
                                    <asp:TextBox type="number" CssClass="form-control col col-sm-10 col-md-10 col-lg-10" runat="server" autocomplete="off" class="form-control" ID="Amount" min="0" onblur="formatDecimal(this)" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="AmountRfv" ControlToValidate="Amount" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="AmountRev" ControlToValidate="Amount" ErrorMessage="Enter a valid amount (e.g.,99.00)" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="PaymentTargetDescription" class="col col-sm-6 col-md-6 col-lg-6">Target Description<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Description" autocomplete="off" class="form-control" ID="PaymentTargetDescription" MaxLength="1000" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="PaymentTargetDescriptionRfv" ControlToValidate="PaymentTargetDescription" ErrorMessage="This field is mandatory." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="PaymentTargetDescriptionRev" ControlToValidate="PaymentTargetDescription" ErrorMessage="Alpha Numeric & special characters only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[A-Za-z0-9.,_\-&%@'\s]+$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" ID="save" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SavePaymentTargetDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancel_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Record ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeletePaymentTarget_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="BtnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
