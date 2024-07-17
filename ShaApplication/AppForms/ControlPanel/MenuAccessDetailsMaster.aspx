<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="MenuAccessDetailsMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.MenuAccessDetailsMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="custom_container">
        <div class="custom_inner_container">
            <div class="pagename_header_Container">
                <h1 class="page_head">Menu Access Details</h1>
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="admin_Name_dd_container col col-sm-5" style="padding: 0;">
                        <div class="row col-12 col-sm-6 col-md-6 col-lg-6">
                            <label for="AdminNameDropDownList" class="col col-sm-3 col-md-3 col-lg-3 center-align" style="padding: 0; margin: 0;">User</label>
                            <div class="col col-sm-9 col-md-9 col-lg-9" style="padding: 0;">
                                <asp:DropDownList runat="server" class="form-control" ID="AdminNameDropDownList" AutoPostBack="True" OnSelectedIndexChanged="AdminDropDownOnChange"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="action_btn_container col col-sm-7">
                        <div class="grid_btn_headrow row">
                            <button runat="server" class="actions_btn grid_haction_btn btn_add" style="width: 25% !important" onserverclick="AddEditMenuAccess_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Enable Access</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="MenuAccessGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="MenuAccessId" Visible="false" />
                            <asp:BoundField DataField="AdminName" HeaderText="Admin Name" />
                            <asp:BoundField DataField="ModuleName" HeaderText="Module Name" />
                            <asp:BoundField DataField="MenuItemName" HeaderText="MenuItem Name" />
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
                        <asp:HiddenField runat="server" ID="MenuAccessId" />
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <label for="adminName" class="col col-sm-6 col-md-6 col-lg-6">User Name<span class="mandatory">*</span></label>
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                        <asp:DropDownList runat="server" class="form-control" ID="AdminDropDownList" AutoPostBack="true" OnSelectedIndexChanged="AdminDropDownList_SelectedIndexChanged" required="required"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="AdminDropDownListRfv" ControlToValidate="AdminDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                    <label for="ModuleNameDropDownList" class="col col-sm-6 col-md-6 col-lg-6">Module Name<span class="mandatory">*</span></label>
                                    <div class="col col-sm-6 col-md-6 col-lg-6">
                                        <asp:DropDownList runat="server" class="form-control" ID="ModuleNameDropDownList" AutoPostBack="true" OnSelectedIndexChanged="ModuleDropDownList_SelectedIndexChanged" required="required"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="ModuleNameDropDownListRfv" ControlToValidate="ModuleNameDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 row menu_item_chkBox_Container center-align" style="margin: 0px;">
                            <div class="row mi_check_box_div">
                                <p runat="server" class="popup_header_title" style="padding: 0; margin: 5px 0;">Menu Items<span class="mandatory">*</span></p>
                                <asp:CheckBoxList ID="chkBoxList" RepeatColumns="3" RepeatLayout="Table" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" ID="save" AutoPostBack="true" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveMenuAccessDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancel_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</asp:Content>
