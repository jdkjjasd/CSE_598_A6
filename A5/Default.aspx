<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="A5.Default" %>
<%@ Register Src="~/LoginControl.ascx" TagPrefix="uc" TagName="LoginControl" %>



<!DOCTYPE html>
<html>
<head runat="server">
    <title>Default Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Welcome to Our Web Application</h1>
            <h2>Current Login User: <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label></h2>

            <h3>Please click the buttons below to access the related pages:</h3>
            <asp:Button ID="btnMember" runat="server" Text="Member" OnClick="btnMember_Click" />
            <asp:Button ID="btnStaff" runat="server" Text="Staff" OnClick="btnStaff_Click" />
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
            <asp:Button ID="btnTryit" runat="server" Text="Try It" OnClick="btnTryit_Click" />
        </div>
    </form>
</body>
</html>
