<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Member.aspx.cs" Inherits="A5.Member" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Member Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Member Page</h1>
            <p>Welcome, <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>!</p>
            <p>This is a page accessible only to registered members.</p>
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
    </form>
</body>
</html>