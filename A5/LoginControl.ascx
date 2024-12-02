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

<asp:Panel ID="pnlSignUp" runat="server">
    <h3>User Sign Up (Member Only)</h3>
    <asp:Label ID="lblSignUpUsername" runat="server" Text="Username:" AssociatedControlID="txtSignUpUsername"></asp:Label>
    <asp:TextBox ID="txtSignUpUsername" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lblSignUpPassword" runat="server" Text="Password:" AssociatedControlID="txtSignUpPassword"></asp:Label>
    <asp:TextBox ID="txtSignUpPassword" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Label ID="lblSignUpRePassword" runat="server" Text="Re-enter Password:" AssociatedControlID="txtSignUpRePassword"></asp:Label>
    <asp:TextBox ID="txtSignUpRePassword" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Label ID="lblRandomstr1" runat="server" Text="Random String"></asp:Label> <asp:Label ID="lblRString1" runat="server" Text="" AssociatedControlID="txtPassword"></asp:Label>
    <asp:TextBox ID="txtRString1" runat="server"></asp:TextBox>
    <br />     
    <asp:Button ID="btnNewString1" runat="server" Text="New String" OnClick="btnNewString_Click" />
    <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" />
    <asp:Label ID="lblSignUpMessage" runat="server" ForeColor="Green"></asp:Label>
</asp:Panel>