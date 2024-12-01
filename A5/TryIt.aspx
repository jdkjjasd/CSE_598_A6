<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TryIt.aspx.cs" Inherits="A5.TryIt" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TryIt Page - Test Login Functionality</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>TryIt Interface for Testing Login Functionality</h2>
            <p>Welcome to the TryIt page. This page is designed to help you test the login functionality of our application. Below, you will find a form where you can enter your credentials to test the login feature.</p>
            <p>The current hardcoded user list is as follows:</p>
            <ul>
                <li><strong>Username:</strong> admin, <strong>Password:</strong> 123, <strong>Role:</strong> Staff</li>
                <li><strong>Username:</strong> user, <strong>Password:</strong> 123, <strong>Role:</strong> Member</li>
                <li><strong>Username:</strong> TA, <strong>Password:</strong> Cse445, <strong>Role:</strong> Staff</li>
                <li><strong>Username:</strong> member, <strong>Password:</strong> 123, <strong>Role:</strong> Member</li>
            </ul>
            <h2>Current Login User: <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label></h2>
            <p>Features you can test on this page:</p>
            <ul>
                <li>Generate a new random string to use for login verification.</li>
                <li>Login with the provided credentials and verify role-based redirection.</li>
                <li>Logout functionality to clear the current session.</li>
                <li>Weather forecast in the next 5 days for a specific ZIP code.</li>
            </ul>
            <asp:Label ID="lblUsername" runat="server" Text="Username:" AssociatedControlID="txtUsername"></asp:Label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <asp:Label ID="lblRString" runat="server" Text="Random String:" AssociatedControlID="txtRString"></asp:Label>
            <asp:TextBox ID="txtRString" runat="server"></asp:TextBox>
            <asp:Button ID="btnNewString" runat="server" Text="Generate New String" OnClick="btnNewString_Click" />
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
            <h2>Weather Service</h2>
            <p>Enter a Zip Code</p>
            <asp:TextBox ID="txtZipCode" runat="server" placeholder="ZIP Code"></asp:TextBox>
            <asp:Button ID="btnFetchWeather" runat="server" Text="Get Forecast" OnClick="btnFetchWeather_Click" />
            <br />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <asp:Label ID="lblWeatherForecast" runat="server" Text=""></asp:Label>

        </div>
    </form>
</body>
</html>