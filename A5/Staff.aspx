<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Staff.aspx.cs" Inherits="A5.Staff" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Staff Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Staff Page</h1>
            <p>Welcome, staff <asp:Label ID="lblStaffName" runat="server" Text=""></asp:Label>!</p>
            <p>This is a page accessible only to authorized staff.</p>
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
    <asp:Panel ID="pnlAddAccount" runat="server" CssClass="add-account-panel">
    <h3>Add Account</h3>
    <table>
        <tr>
            <td><label for="txtNewUsername">Username:</label></td>
            <td><asp:TextBox ID="txtNewUsername" runat="server" CssClass="form-control" /></td>
        </tr>
        <tr>
            <td><label for="txtNewPassword">Password:</label></td>
            <td><asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" /></td>
        </tr>
        <tr>
            <td><label for="txtReEnterPassword">Re-enter Password:</label></td>
            <td><asp:TextBox ID="txtReEnterPassword" runat="server" CssClass="form-control" TextMode="Password" /></td>
        </tr>
        <tr>
            <td><label for="ddlRole">Role:</label></td>
            <td>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Select Role" Value="0" />
                    <asp:ListItem Text="Staff" Value="1" />
                    <asp:ListItem Text="Member" Value="2" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: right;">
                <asp:Button ID="btnAddAccount" runat="server" Text="Add Account" CssClass="btn btn-primary" OnClick="btnAddAccount_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
<asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" CssClass="table" 
    OnRowDeleting="gvAccounts_RowDeleting" DataKeyNames="Username" 
    OnRowCommand="gvAccounts_RowCommand">
    <Columns>
        <asp:BoundField DataField="username" HeaderText="Username" />
        <asp:BoundField DataField="enc_password" HeaderText="Encrypted Password" />
        <asp:BoundField DataField="role" HeaderText="Role" />


        <asp:TemplateField HeaderText="Memo Count">
            <ItemTemplate>
                <asp:Label ID="lblMemoCount" runat="server" Text='<%# Eval("memoCount") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="btnClearMemos" runat="server" Text="Clear Memos" CommandName="ClearMemos" CommandArgument='<%# Eval("username") %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</form>
</body>
</html>
