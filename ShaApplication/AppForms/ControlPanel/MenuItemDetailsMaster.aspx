<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="MenuItemDetailsMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.MenuItemDetailsMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function fnMenuItemDetailRowClick(row) {
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
                <h1 class="page_head">Menu Item Details</h1>
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" class="actions_btn grid_haction_btn btn_add" onserverclick="AddMenuItem_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_edit" id="editBtn" onserverclick="EditMenuItem_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_delete" id="deleteBtn" onserverclick="DeleteMenuItem_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="MenuItemGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="MenuItemGridGridView_SetDataRowId" OnSelectedIndexChanged="MenuItemGridGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="MenuItemId" Visible="false" />
                            <asp:BoundField DataField="ModuleName" HeaderText="Module Name" />
                            <asp:BoundField DataField="MenuItemName" HeaderText="MenuItem Name" />
                            <asp:BoundField DataField="MenuItemDescription" HeaderText="Description" />
                            <asp:BoundField DataField="TaskURL" HeaderText="URL" />
                            <asp:BoundField DataField="MenuItemIcon" HeaderText="Icon" />
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
                        <asp:HiddenField runat="server" ID="MenuItemId" />
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <label for="ModuleNameDropDownList" class="col col-sm-6 col-md-6 col-lg-6">Module Name<span class="mandatory">*</span></label>
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                        <asp:DropDownList runat="server" class="form-control" ID="ModuleNameDropDownList" required="required"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="ModuleNameDropDownListRfv" ControlToValidate="ModuleNameDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <label for="MenuItemName" class="col col-sm-6 col-md-6 col-lg-6">MenuItem Name<span class="mandatory">*</span></label>
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                        <asp:TextBox runat="server" title="MenuItem Name" autocomplete="off" class="form-control" ID="MenuItemName" MaxLength="250" required="required"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="MenuItemNameRfv" ControlToValidate="MenuItemName" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ID="MenuItemNameRev" ControlToValidate="MenuItemName" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <label for="TaskURL" class="col col-sm-6 col-md-6 col-lg-6">Task URL<span class="mandatory">*</span></label>
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                        <asp:TextBox runat="server" title="Task URL" autocomplete="off" class="form-control" ID="TaskURL" MaxLength="250" required="required"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="TaskURLRfv" ControlToValidate="TaskURL" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ID="TaskURLRev" ControlToValidate="TaskURL" ErrorMessage="Enter a valid url (e.g., Index.aspx)" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]+\.aspx$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <label for="MenuItemDescription" class="col col-sm-6 col-md-6 col-lg-6">Description<span class="mandatory">*</span></label>
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                        <asp:TextBox runat="server" title="Description" autocomplete="off" class="form-control" ID="MenuItemDescription" MaxLength="1000" required="required"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="MenuItemDescriptionRfv" ControlToValidate="MenuItemDescription" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ID="MenuItemDescriptionRev" ControlToValidate="MenuItemDescription" ErrorMessage="Alpha Numeric & special characters only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[A-Za-z0-9.,_\-&%@'\s]+$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <label for="MenuItemIcon" class="col col-sm-6 col-md-6 col-lg-6">Menu Icon<span class="mandatory">*</span></label>
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                        <asp:TextBox runat="server" title="Code" autocomplete="off" class="form-control" ID="MenuItemIcon" MaxLength="50" required="required"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="MenuItemIconRfv" ControlToValidate="MenuItemIcon" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ID="MenuItemIconRev" ControlToValidate="MenuItemIcon" ErrorMessage="Alpha Numeric,Special Characters & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z\s-]+"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" ID="save" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveMenuItemDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancel_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this MenuItem ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteMenuItem_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="BtnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
