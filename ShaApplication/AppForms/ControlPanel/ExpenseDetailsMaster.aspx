<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MainLayout.Master" AutoEventWireup="true" CodeBehind="ExpenseDetailsMaster.aspx.cs" Inherits="ShaApplication.AppForms.ControlPanel.ExpenseDetailsMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function fnExpenseDetailRowClick(row) {
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
                <h1 class="page_head">Expense Details</h1>
            </div>
        </div>
        <div class="div_container" style="width: 100%;">
            <div class="grid_container">
                <div class="tbl_head_div">
                    <div class="action_btn_container col col-sm-12">
                        <div class="grid_btn_headrow row">
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_add" onserverclick="AddExpense_Click">
                                <i class="fa fa-plus grid_haction_icon"></i><span class="grid_head_btn">Add</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_edit" id="editBtn" onserverclick="EditExpense_Click">
                                <i class="fa fa-edit grid_haction_icon"></i><span class="grid_head_btn">Edit</span>
                            </button>
                            <button runat="server" type="button" class="actions_btn grid_haction_btn btn_delete" id="deleteBtn" onserverclick="DeleteExpense_Click">
                                <i class="fa fa-archive  grid_haction_icon"></i><span class="grid_head_btn">Delete</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="grid_head">
                    <asp:GridView ID="ExpenseGridView" CssClass="grid_view" EmptyDataRowStyle-CssClass="center_text" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="ExpenseGridView_SetDataRowId" OnSelectedIndexChanged="ExpenseGridView_SelectedIndexChanged" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ExpenseId" Visible="false" />
                            <asp:BoundField DataField="InvoiceRefId" HeaderText="Invoice Reference Number" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Supplier Name" />
                            <asp:BoundField DataField="PayableAccTypeName" HeaderText="Account Type" />
                            <asp:BoundField DataField="ExpenseDescription" HeaderText="Description" />
                            <asp:BoundField DataField="DebitOverAllAmount" HeaderText="Debit Amount" />
                            <asp:BoundField DataField="DebitedDate" HeaderText="Debited Date" />
                            <asp:BoundField DataField="AmountTypeName" HeaderText="Amount Type" />
                        </Columns>
                        <EmptyDataTemplate>No Record Found</EmptyDataTemplate>
                        <EmptyDataRowStyle />
                    </asp:GridView>
                    <asp:HiddenField ID="SelectedRowIdHiddenField" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <div runat="server" id="popup_confirm_container" class="popup_outer_container">
        <div class="confirm_popup_inner_container">
            <p>Are You Sure You Want to Delete this Record ? </p>
            <div class="confirm_popup_action_container">
                <asp:Button runat="server" ID="OkBtn" CssClass="btn_ok" Text="Ok" OnClick="ConfirmDeleteExpense_Click" />
                <asp:Button runat="server" ID="CancelBtn" CssClass="btn_cancel" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
