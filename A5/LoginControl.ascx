<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginControl.ascx.cs" Inherits="A5.LoginControl" %>
<asp:Panel ID="pnlLogin" runat="server">
    <h3>User Login</h3>
    <asp:Label ID="lblUsername" runat="server" Text="Username:" AssociatedControlID="txtUsername"></asp:Label>
    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword"></asp:Label>
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Label ID="lblRandomstr" runat="server" Text="Random String"></asp:Label> <asp:Label ID="lblRString" runat="server" Text="" AssociatedControlID="txtPassword"></asp:Label>
    <asp:TextBox ID="txtRString" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="btnNewString" runat="server" Text="New String" OnClick="btnNewString_Click" />
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
</asp:Panel>