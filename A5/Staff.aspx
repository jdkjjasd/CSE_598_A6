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
    </form>
</body>
</html>
