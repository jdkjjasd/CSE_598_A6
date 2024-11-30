<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="A5.Login" %>
<%@ Register Src="~/LoginControl.ascx" TagPrefix="uc" TagName="LoginControl" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>log in page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>please log in</h2>
            <uc:LoginControl ID="LoginControl1" runat="server" />
        </div>
    </form>
</body>
</html>