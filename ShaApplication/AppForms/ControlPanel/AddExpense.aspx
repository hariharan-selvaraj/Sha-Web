<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="AddExpense.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.AddExpense" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(".grid_header_btn").click(function (e) { e.preventDefault(); })
        function fnExpenseDetailsRowClick(row) {
            let rows = document.querySelectorAll(".clickable_row");
            rows.forEach(function (row) {
                row.classList.remove("selected_row");
            });
            row.classList.add("selected_row");
            let rowId = row.getAttribute("data-row-id");
            let Id = row.getAttribute("data-id");
            document.getElementById('<%= SelectedDetailsRowIdHiddenField.ClientID %>').value = rowId;
            document.getElementById('<%= SelectedDetailsIdHiddenField.ClientID %>').value = Id;
            fnEnableBtn();
        }
        function triggerUpload(e) {
            e.preventDefault();
            var fileUpload = document.getElementById('<%= Expensefileupload.ClientID %>');
            var uploadFileNameDiv = document.getElementById('<%= upload_fileName_div.ClientID %>');
            var expenseFileDiv = document.getElementById('<%= Expense_file_div.ClientID %>');
            if ((expenseFileDiv != null && expenseFileDiv != null) && uploadFileNameDiv.innerText.trim() !== "") {
                alert('You Can Add Only One File.');
                return;
            }
            if (fileUpload.files.length > 0) {
                var file = fileUpload.files[0];
                var fileSize = (file.size / 1024.0).toFixed(2) + " Kb";
                uploadFileNameDiv.innerText = file.name + " (" + fileSize + ")";
                expenseFileDiv.style.display = 'flex';
            }
        }
        function fnDeleteUploadFile(e) {
            debugger;
            e.preventDefault();
            var fileUpload = document.getElementById('<%= Expensefileupload.ClientID %>');
            var uploadFileNameDiv = document.getElementById('<%= upload_fileName_div.ClientID %>');
            var expenseFileDiv = document.getElementById('<%= Expense_file_div.ClientID %>');
            var fileUploadBtn = document.getElementById('<%= FileUploadBtn.ClientID %>');
            if (uploadFileNameDiv.innerText.trim() === "") {
                alert('No file found.');
                return;
            }
            fileUpload.value = "";
            uploadFileNameDiv.innerText = "";
            expenseFileDiv.style.display = 'none';
            fileUpload.style.display = 'block';
            fileUploadBtn.style.display = 'block';
        }
        function showImagePopup() {
            document.getElementById('<%= ImageViewPopUp.ClientID %>').style.display = 'block';
        }
        function closePopup() {
            document.getElementById('<%= ImageViewPopUp.ClientID %>').style.display = 'none';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="popup_header" style="text-align: center">
        <p runat="server" id="Header_text" class="popup_header_title"></p>
    </div>
    <div class="div_container scroll_container">
        <div class="row" style="background-color: aliceblue; padding: 10px;">
            <div class="col-lg-12 col-md-12 row center-align" style="margin: 0px;">
                <asp:HiddenField runat="server" ID="ExpenseID" />
                <!-- Is Invoice Checkbox -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="IsInvoiceCheckbox" class="col-6 col-sm-6 col-md-6 col-lg-6">Is Invoice</label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:CheckBox runat="server" ID="IsInvoiceCheckbox" AutoPostBack="true" OnCheckedChanged="FnDisabledInvoiceRefId"></asp:CheckBox>
                    </div>
                </div>
                <!-- Invoice Reference Number -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="InvoiceRefNo" class="col-6 col-sm-6 col-md-6 col-lg-6">Invoice Reference Number<asp:Label ID="InvoiceRefIdMandatoryLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:TextBox runat="server" class="form-control" AutoPostBack="true" autocomplete="off" ID="InvoiceRefNo"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="InvoiceRefNoRev" ControlToValidate="InvoiceRefNo" ErrorMessage="Alpha Numeric,Special Characters & spaces only allow" Font-Size="12px" ForeColor="Red" Display="Dynamic" ValidationExpression="[A-Za-z0-9_-]+"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <!-- Is GST Checkbox -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="IsGSTCheckbox" class="col-6 col-sm-6 col-md-6 col-lg-6">Is GST</label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:CheckBox runat="server" ID="IsGSTCheckbox" AutoPostBack="true" OnCheckedChanged="FnShowGSTDropDownList"></asp:CheckBox>
                    </div>
                </div>
                <!-- GST Dropdown List -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="GSTDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">GST<asp:Label ID="GstMandatoryLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:DropDownList runat="server" class="form-control" AutoPostBack="true" ID="GSTDropDownList"></asp:DropDownList>
                    </div>
                </div>
                <!-- Is Emplyee CheckBox -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="isEmployee" class="col-6 col-sm-6 col-md-6 col-lg-6">Is Employee</label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:CheckBox runat="server" class="" ID="IsEmployee" AutoPostBack="true" OnCheckedChanged="ShowEmployeeDropDownList"></asp:CheckBox>
                    </div>
                </div>
                <!-- Emplyee Dropdown List -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="company" class="col-6 col-sm-6 col-md-6 col-lg-6">Employee Name<asp:Label ID="EmployeeLbl" runat="server" Text="" CssClass="mandatory">*</asp:Label></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:DropDownList runat="server" class="form-control" ID="EmployeeDropDownList" required="required"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="EmployeeDropDownListRfv" ControlToValidate="EmployeeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!-- Supplier Dropdown List -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="SupplierDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Supplier Name<span class="mandatory">*</span></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:DropDownList runat="server" class="form-control" AutoPostBack="true" ID="SupplierDownList"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="SupplierDownListRfv" ControlToValidate="SupplierDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!-- Expense Description -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="ExpenseDescription" class="col-6 col-sm-6 col-md-6 col-lg-6">Description<span class="mandatory">*</span></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:TextBox runat="server" title="Description" AutoPostBack="true" autocomplete="off" class="form-control" ID="ExpenseDescription" MaxLength="250"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="ExpenseDescriptionRfv" ControlToValidate="ExpenseDescription" ErrorMessage="Description is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="ExpenseDescriptionRev" ControlToValidate="ExpenseDescription" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <!-- Debit Amount -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="DebitAmount" class="col-6 col-sm-6 col-md-6 col-lg-6">Debit Amount<span class="mandatory">*</span></label>
                    <div class="row col-6 col-sm-6 col-md-6 col-lg-6" style="margin: 0px;">
                        <span class="form-control col-2 col-sm-2 col-md-2 col-lg-2 currency">$</span>
                        <asp:TextBox runat="server" type="number" AutoPostBack="true" autocomplete="off" class="form-control col-10 col-sm-10 col-md-10 col-lg-10" ID="DebitAmount" onblur="formatDecimal(this)"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="DebitAmountRfv" ControlToValidate="DebitAmount" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="DebitAmountRev" ControlToValidate="DebitAmount" ErrorMessage="Enter a valid amount (e.g.,99.00)" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <!-- Debited Date -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="DebitedDate" class="col-6 col-sm-6 col-md-6 col-lg-6">Debited Date<span class="mandatory">*</span></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:TextBox runat="server" AutoPostBack="true" type="date" autocomplete="off" class="form-control" ID="DebitedDate"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="DebitedDateRfv" ControlToValidate="DebitedDate" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!-- Account Type Dropdown List -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="AccountTypeDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Account Type<span class="mandatory">*</span></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:DropDownList runat="server" required="required" AutoPostBack="true" class="form-control" ID="AccountTypeDropDownList"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="AccountTypeDropDownListRfv" ControlToValidate="AccountTypeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!-- Amount Type Dropdown List -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="AmountTypeDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Amount Type<span class="mandatory">*</span></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:DropDownList runat="server" required="required" AutoPostBack="true" class="form-control" ID="AmountTypeDropDownList"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="AmountTypeDropDownListRfv" ControlToValidate="AmountTypeDropDownList" InitialValue="0" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!-- Paid By Dropdown List -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="PaidByDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Paid By<span class="mandatory">*</span></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:DropDownList runat="server" required="required" AutoPostBack="true" class="form-control" ID="PaidByDropDownList"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="PaidByDropDownListRfv" ControlToValidate="PaidByDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!-- Received By Dropdown List -->
                <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                    <label for="ReceivedByDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Received By<span class="mandatory">*</span></label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:DropDownList runat="server" required="required" AutoPostBack="true" class="form-control" ID="ReceivedByDropDownList"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="ReceivedByDropDownListRfv" ControlToValidate="ReceivedByDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!-- File Upload -->
                <div class="form-group row col-12 col-sm-12 col-md-12 col-lg-12" id="left_align">
                    <label for="Expensefileupload" class="col-6 col-sm-6 col-md-3 col-lg-3">File Upload</label>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                        <asp:UpdatePanel ID="UpdatePanel5" class="row UpdatePanelRow" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <asp:FileUpload runat="server" class="form-control" Width="42%" ID="Expensefileupload"></asp:FileUpload>
                                <asp:Button ID="FileUploadBtn" class="upload_btn" runat="server" Text="Upload" Width="25%" OnClick="FileUploadBtn_Click" CausesValidation="false" UseSubmitBehavior="false" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="FileUploadBtn" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="row col-12 col-sm-12 col-md-12 col-lg-12" runat="server" id="Expense_file_div">
                        <div class="col-4 col-sm-4 col-md-4 col-lg-4 upload_fileName_div center-align" runat="server" id="upload_fileName_div" style="border: 1px solid black; border-radius: 5px;">
                        </div>
                        <div class="col-1 col-sm-1 col-md-1 col-lg-1 center-align">
                            <button id="DeleteUploadFileBtn" runat="server" onserverclick="ClearUploadFile" class="popup_header_close_icon center-align upload_btn" causesvalidation="false" usesubmitbehavior="false">×</button>
                            <button runat="server" type="button" onserverclick="ViewUploadFile" id="view_file_div" style="border: none;">
                                <i class="fa-solid fa-eye grid_haction_icon"></i>
                            </button>
                        </div>
                    </div>
                    <div class="ImageViewPopUp" runat="server" id="ImageViewPopUp" style="display: none;">
                        <div class="popup-content">
                            <span class="close" onclick="closePopup()">&times;</span>
                            <%--<asp:ImageButton runat="server" ID="popupImage" src="" alt="Image" Style="width: 100%; height: 96%;" />--%>
                            <asp:Image ID="popupImage" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="custom_container">
            <div class="div_container" style="width: 100%;">
                <div class="grid_container">
                    <div class="tbl_head_div">
                        <div class="action_btn_container col-12 col-sm-12">
                            <div class="grid_btn_headrow row">
                                <button runat="server" type="button" class="actions_btn grid_haction_btn btn_add grid_header_btn" id="addBtn" onserverclick="AddExpenseDetails">
                                    <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                                </button>
                                <button runat="server" type="button" class="actions_btn grid_haction_btn btn_edit grid_header_btn" id="editBtn" onserverclick="EditExpenseDetails">
                                    <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                                </button>
                                <button runat="server" type="button" class="actions_btn grid_haction_btn btn_delete grid_header_btn" id="deleteBtn" onserverclick="DeleteExpenseDetails">
                                    <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="grid_head">
                        <asp:GridView ID="ExpenseDetailsGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="ExpenseDetailsGridView_SetDataRowId" OnSelectedIndexChanged="ExpenseDetailsGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="DetailsRowId" Visible="false" />
                                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                                <asp:BoundField DataField="TargetDescription" HeaderText="Target Description" />
                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                <asp:BoundField DataField="GST" HeaderText="GST" />
                            </Columns>
                            <EmptyDataTemplate>No Record Found</EmptyDataTemplate>
                            <EmptyDataRowStyle />
                        </asp:GridView>
                        <asp:HiddenField ID="SelectedDetailsRowIdHiddenField" runat="server" />
                        <asp:HiddenField ID="SelectedDetailsIdHiddenField" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="popup_footer">
        <asp:Button runat="server" type="button" ID="save" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveExpenseHeader" class="btn btn-primary px-4" />
        <asp:Button runat="server" type="button" ID="cancel" CssClass="popup-btn popup-btn-cancel" Text="Back" OnClick="btnBack_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
    </div>
    <div runat="server" id="popup_container" class="popup_outer_container">
        <div class="popup_inner_container">
            <div class="popup_header">
                <p runat="server" id="Details_text" class="popup_header_title"></p>
                <span runat="server" id="total_target_amount" class="header_show_amount"></span>
                <span runat="server" id="balance_target_amount" class="header_show_amount"></span>
                <asp:LinkButton ID="lnkClose" runat="server" OnClick="BtnCancelExpensePopup_Click" CssClass="popup_header_close_icon" CausesValidation="false" UseSubmitBehavior="false">×</asp:LinkButton>
            </div>
            <div class="popup_body">
                <div class="div_container">
                    <div class="row">
                        <asp:HiddenField runat="server" ID="ExpenseDetailsId" />
                        <asp:HiddenField runat="server" ID="ExpenseDetailsRowId" />

                        <!-- Company Name Dropdown List -->
                        <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                            <label for="CompanyDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Company Name<span class="mandatory">*</span></label>
                            <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                <asp:DropDownList runat="server" class="form-control" AutoPostBack="true" ID="CompanyDropDownList"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="CompanyDropDownListRfv" ControlToValidate="CompanyDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Project Name Dropdown List -->
                        <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                            <label for="ProjectNameDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Project Name<span class="mandatory">*</span></label>
                            <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                <asp:DropDownList runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="GetTargetDetails" ID="ProjectNameDropDownList"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="ProjectNameDropDownListRfv" ControlToValidate="ProjectNameDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Target Description Dropdown List -->
                        <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                            <label for="TargetDescriptionDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">Target Description<span class="mandatory">*</span></label>
                            <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                <asp:DropDownList runat="server" required="required" AutoPostBack="true" class="form-control" ID="TargetDescriptionDropDownList"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="TargetDescriptionDropDownListRfv" ControlToValidate="TargetDescriptionDropDownList" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Description Textbox -->
                        <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                            <label for="Description" class="col-6 col-sm-6 col-md-6 col-lg-6">Description<span class="mandatory">*</span></label>
                            <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                <asp:TextBox runat="server" title="Description" AutoPostBack="true" autocomplete="off" class="form-control" ID="Description" MaxLength="250"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="DescriptionRfv" ControlToValidate="Description" ErrorMessage="Description is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="DescriptionRev" ControlToValidate="Description" ErrorMessage="Alpha Numeric & spaces only allow" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="[A-Za-z0-9\s]+"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <!-- Amount Textbox -->
                        <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                            <label for="DebitAmount" class="col-6 col-sm-6 col-md-6 col-lg-6">Amount<span class="mandatory">*</span></label>
                            <div class="row col-6 col-sm-6 col-md-6 col-lg-6" style="margin: 0px;">
                                <span class="form-control col-2 col-sm-2 col-md-2 col-lg-2 currency">$</span>
                                <asp:TextBox type="number" CssClass="form-control col-10 col-sm-10 col-md-10 col-lg-10" AutoPostBack="true" runat="server" OnTextChanged="ShowBalanceTargetAmount" autocomplete="off" class="form-control" ID="Amount" onblur="formatDecimal(this)"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="ShowBalanceTargetAmountRfv" ControlToValidate="Amount" ErrorMessage="This field is mandatory." ForeColor="Red" Font-Size="12px" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="ShowBalanceTargetAmountRev" ControlToValidate="Amount" ErrorMessage="Enter a valid amount (e.g.,99.00)" ForeColor="Red" Font-Size="12px" Display="Dynamic" ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <!-- Details GST Dropdown List -->
                        <div class="form-group row col-12 col-sm-12 col-md-6 col-lg-6">
                            <label for="ExpenseGSTDropDownList" class="col-6 col-sm-6 col-md-6 col-lg-6">GST</label>
                            <div class="col-6 col-sm-6 col-md-6 col-lg-6">
                                <asp:DropDownList runat="server" class="form-control" AutoPostBack="true" ID="ExpenseGSTDropDownList"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="popup_footer">
                <asp:Button runat="server" type="button" ID="SaveExpensePopup" CssClass="popup-btn popup-btn-save" Text="Save" OnClick="SaveExpenseDetails" class="btn btn-primary px-4" />
                <asp:Button runat="server" type="button" ID="CancelExpensePopup" CssClass="popup-btn popup-btn-cancel" Text="Cancel" OnClick="BtnCancelExpensePopup_Click" class="btn btn-primary px-4" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Record ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" type="button" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteExpenseDetails" />
                <asp:Button runat="server" type="button" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="btnCancelDeleteExpenseDetails" CausesValidation="false" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</asp:Content>
