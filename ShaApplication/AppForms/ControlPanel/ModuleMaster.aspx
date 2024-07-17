<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="ModuleMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.ModuleMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function fnModuleMasterRowClick(row) {
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
                <h1 class="page_head">Module Master</h1>
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" class="actions_btn grid_haction_btn btn_add" onserverclick="AddModule_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_edit" id="editBtn" onserverclick="EditModule_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_delete" id="deleteBtn" onserverclick="DeleteModule_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="ModuleMasterGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="ModuleMasterGridGridView_SetDataRowId" OnSelectedIndexChanged="ModuleMasterGridGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ModuleId" Visible="false" />
                            <asp:BoundField DataField="ModuleName" HeaderText="Module Name" />
                            <asp:BoundField DataField="ModuleDescription" HeaderText="Description" />
                            <asp:BoundField DataField="ModuleIcon" HeaderText="Menu Icon" />
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
                        <asp:HiddenField runat="server" ID="ModuleMasterId" />
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="ModuleName" class="col col-sm-6 col-md-6 col-lg-6">Module Name<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Module Name" autocomplete="off" class="form-control" ID="ModuleName" MaxLength="250" pattern="[A-Za-z0-9\s]+" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="ModuleNameRfv" ControlToValidate="ModuleName" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="ModuleNameRev" ControlToValidate="ModuleName" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="ModuleMasterDescription" class="col col-sm-6 col-md-6 col-lg-6">Description<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Description" autocomplete="off" class="form-control" ID="ModuleMasterDescription" MaxLength="250" pattern="[A-Za-z0-9\s]+" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="ModuleMasterDescriptionRfv" ControlToValidate="ModuleMasterDescription" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="ModuleMasterDescriptionRev" ControlToValidate="ModuleMasterDescription" ErrorMessage="Alpha Numeric & special characters only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[A-Za-z0-9.,_\-&%@'\s]+$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="ModuleIcon" class="col col-sm-6 col-md-6 col-lg-6">Module Icon<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Module Icon" autocomplete="off" class="form-control" ID="ModuleIcon" MaxLength="50" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="ModuleIconRfv" ControlToValidate="ModuleIcon" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="ModuleIconRev" ControlToValidate="ModuleIcon" ErrorMessage="Alpha Numeric,Special Characters & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z\s-]+"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" ID="save" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveModuleMasterDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancel_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Module ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteModule_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="BtnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
