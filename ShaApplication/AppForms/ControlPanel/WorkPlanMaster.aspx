<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="WorkPlanMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.WorkPlanMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="custom_container">
        <div class="custom_inner_container">
            <div class="pagename_header_Container">
                <h1 class="page_head">Work Plan Master</h1>
            </div>
        </div>
        <div class="div_container">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_add" onserverclick="AddWorkPlan_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_edit" id="editBtn" onserverclick="EditWorkPlan_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_delete" id="deleteBtn" onserverclick="DeleteWorkPlan_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="WorkPlanGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="WorkPlanGridView_SetDataRowId" OnSelectedIndexChanged="WorkPlanGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Work_Plan_Id" Visible="false" />
                            <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                            <asp:BoundField DataField="PlannedDate" HeaderText="Planned Date" />
                            <asp:BoundField DataField="WorkDescription" HeaderText="Description/Location" />
                            <asp:BoundField DataField="Phase" HeaderText="Phase" />
                            <asp:BoundField DataField="TeamSupplyName" HeaderText="Team Head Name" />
                            <asp:BoundField DataField="WorkersCount" HeaderText="Workers Count" />
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
                        <asp:HiddenField runat="server" ID="Work_Plan_Id" />
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="ProjectNameDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Project Name<asp:Label ID="ProjectNameLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="ProjectNameDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="ProjectNameDropDownListRfv" ControlToValidate="ProjectNameDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="PlannedDate" class="col-6 col-sm-6 col-md-6 col-lg-6">Planned Date<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" type="date" autocomplete="off" class="form-control" ID="PlannedDate" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="PlannedDateRfv" ControlToValidate="PlannedDate" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="WorkDescription" class="col-6 col-sm-6 col-md-6 col-lg-6">Description/Location<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Work Description" autocomplete="off" class="form-control" ID="WorkDescription" MaxLength="250" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="WorkDescriptionRfv" ControlToValidate="WorkDescription" ErrorMessage="Description is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="WorkDescriptionRev" ControlToValidate="WorkDescription" ErrorMessage="Alpha Numeric & special characters only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[A-Za-z0-9.,_\-&%@'\s]+$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="TeamHeadNameDropDownList" class="col col-sm-6 col-md-6 col-lg-6">Team Head Name<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="TeamHeadNameDropDownList" AutoPostBack="true" OnSelectedIndexChanged="TeamDropDownList_SelectedIndexChanged" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="TeamHeadNameDropDownListRfv" ControlToValidate="TeamHeadNameDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div runat="server" visible="false" id="Workers_chkBox_Container" class="col-lg-12 col-md-12 row menu_item_chkBox_Container center-align" style="margin: 0px;">
                                <div class="row mi_check_box_div">
                                    <p runat="server" class="popup_header_title" style="padding: 0; margin: 5px 0;">Team Workers<span class="mandatory">*</span></p>
                                    <asp:CheckBoxList ID="chkBoxList" RepeatColumns="3" RepeatLayout="Table" RepeatDirection="Vertical" runat="server"></asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="WorkersCount" class="col col-sm-6 col-md-6 col-lg-6">Workers Count<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" title="Workers Count" autocomplete="off" class="form-control" ID="WorkersCount" MaxLength="250" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="WorkersCountRfv" ControlToValidate="WorkersCount" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="WorkersCountRev" ControlToValidate="WorkersCount" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" ID="save" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveWorkPlanDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancel_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Record ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteWorkPlan_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="BtnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
