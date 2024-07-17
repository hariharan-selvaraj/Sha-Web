<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="TimeSheetMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.TimeSheetMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="custom_container">
        <div class="custom_inner_container">
            <div class="pagename_header_Container">
                <h1 class="page_head">Time Sheet Master</h1>
            </div>
        </div>
        <div class="row p-2" style="background-color: #f0f8ff; width: 97%; margin-top: 1%;">
            <div class="admin_Name_dd_container col col-sm-4" style="padding: 0;">
                <div class="row col-12 col-sm-12 col-md-12 col-lg-12">
                    <label for="UserDropDownList" class="col col-sm-5 col-md-5 col-lg-5 center-align" style="padding: 0; margin: 0;">User<span class="form-element-mandatory">&nbsp;*</span></label>
                    <div class="col col-sm-7 col-md-7 col-lg-7" style="padding: 0;">
                        <asp:DropDownList runat="server" class="form-control" ID="UserDropDownList"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12 col-sm-3 center-align">
                <label for="FromDate" class="form-element-label col-5 col-sm-5">FromDate<span class="form-element-mandatory">&nbsp;*</span></label>
                <asp:TextBox runat="server" type="date" autocomplete="off" class="form-control col-7 col-sm-7" ID="FromDate" AutoPostBack="true" OnTextChanged="FromDate_TextChanged">> required="required"></asp:TextBox>
            </div>
            <div class="col-12 col-sm-3 center-align">
                <label for="ToDate" class="form-element-label col-4 col-sm-4">ToDate<span class="form-element-mandatory">&nbsp;*</span></label>
                <asp:TextBox runat="server" type="date" autocomplete="off" class="form-control col-8 col-sm-8" ID="ToDate">required="required"></asp:TextBox>
            </div>
            <div class="col-12 col-sm-2 center-align" style="gap: 5%;">
                <asp:Button runat="server" ID="SearchBtn" CssClass="col-6 col-sm-6 btn btn-success" Text="Search" OnClick="SearchTimeSheetDetails" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
                <asp:Button runat="server" ID="ClearBtn" CssClass="col-6 col-sm-6 btn btn-secondary" Text="Clear" OnClick="BtnClear_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <%--<div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" class="actions_btn grid_haction_btn btn_add" onserverclick="AddTimeSheet_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_edit" id="editBtn" onserverclick="EditTimeSheet_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" class="actions_btn grid_haction_btn btn_delete" id="deleteBtn" onserverclick="DeleteTimeSheet_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>--%>
                <div class="grid_head">
                    <asp:GridView ID="TimeSheetGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="TimeSheetGridView_SetDataRowId" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Date" HeaderText="Date" />
                            <asp:BoundField DataField="Day" HeaderText="Day" />
                            <asp:BoundField DataField="MonthlyWorkedHrs" HeaderText="Monthly Worked Hours" />
                            <asp:BoundField DataField="HolidayType" Visible="false" />
                            <asp:BoundField DataField="WorkedDays" HeaderText="Worked Days" />
                            <asp:BoundField DataField="InTime" HeaderText="InTime" />
                            <asp:BoundField DataField="OutTime" HeaderText="OutTime" />
                            <asp:BoundField DataField="WorkedOfficeHrs" HeaderText="Worked Office Hours" />
                            <%--<asp:TemplateField HeaderText="InTime Image Path">
                                <ItemTemplate>
                                    <asp:LinkButton ID="InTimeImgPath" runat="server" Text="Download" OnClick="btnDownload_OnClick"
                                        CommandArgument='<%# Eval("InTimeImgPath") %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OutTime Image Path">
                                <ItemTemplate>
                                    <asp:LinkButton ID="OutTimeImgPath" runat="server" Text="Download" OnClick="btnDownload_OnClick"
                                        CommandArgument='<%# Eval("OutTimeImgPath") %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="InTime Image Path">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlInTimeImgPath" runat="server" NavigateUrl='<%# "DownloadHandler.ashx?file=" + Eval("InTimeImgPath") %>' Text='Download' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OutTime Image Path">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlOutTimeImgPath" runat="server" NavigateUrl='<%# "DownloadHandler.ashx?file=" + Eval("OutTimeImgPath") %>' Text='Download' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                            <asp:BoundField DataField="ProjectType" HeaderText="Project Type" />
                        </Columns>
                        <EmptyDataTemplate>No Record Found</EmptyDataTemplate>
                        <EmptyDataRowStyle />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <%--<div id="imagePopup" class="popup" style="display: none;">
        <div class="popup-content">
            <span class="close" onclick="closeImagePopup()">&times;</span>
            <img id="popupImage" src="" alt="Popup Image" />
        </div>
    </div>--%>
    <%--<div runat="server" id="popup_container" class="popup_outer_container">
        <div class="popup_inner_container">
            <div class="popup_header">
                <p runat="server" class="popup_header_title" id="PopupHeaderText"></p>
                <asp:LinkButton ID="lnkClose" runat="server" OnClick="BtnCancel_Click" CssClass="popup_header_close_icon" CausesValidation="false">×</asp:LinkButton>
            </div>
            <div class="popup_body">
                <div class="div_container">
                    <div class="row">
                        <asp:HiddenField runat="server" ID="TimeSheetId" />
                        <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                            <div class="form-group row col-12 col-sm-6 col-md-6 col-lg-6">
                                <label for="PayAccTypeName" class="col col-sm-6 col-md-6 col-lg-6">User Name<span class="mandatory">*</span></label>
                                <div class="col col-sm-6 col-md-6 col-lg-6">
                                    <asp:TextBox runat="server" autocomplete="off" class="form-control" ID="UserName" MaxLength="250" required="required"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="PayAccTypeNameRfv" ControlToValidate="UserName" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" ID="PayAccTypeNameRev" ControlToValidate="UserName" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="ProjectNameDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Project Name<asp:Label ID="ProjectNameLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="ProjectNameDropDownList" required="required"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="PhotoTypeDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Photo Type<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:DropDownList runat="server" class="form-control" ID="PhotoTypeDropDownList" required="required"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="PhotoTypeDropDownListRfv" ControlToValidate="PhotoTypeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                                <label for="ImgUpload" class="col-6 col-sm-6 col-md-6 col-lg-6">Image Upload<span class="mandatory">*</span></label>
                                <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                    <asp:FileUpload type="file" runat="server" AutoPostBack="true" class="form-control" ID="ImgUpload"></asp:FileUpload>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" ID="save" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveTimeSheetDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancel_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>--%>
    <%--<div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Record ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteTimeSheet_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="BtnCancel_Click" />
            </div>
        </div>
    </div>--%>
</asp:Content>
