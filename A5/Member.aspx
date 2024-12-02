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
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>



            <asp:TextBox ID="txtReminder" runat="server" Placeholder="Enter Reminder"></asp:TextBox>
            <asp:Button ID="btnAddMemo" runat="server" Text="Add Memo" OnClick="btnAddMemo_Click" />

            <h2>Get the Weather for a Zip Code</h2>
            <asp:TextBox ID="txtZipCode" runat="server" placeholder="ZIP Code"></asp:TextBox>
            <asp:Button ID="btnFetchWeather" runat="server" Text="Get Forecast" OnClick="btnFetchWeather_Click" />

            <br /><br />

            <asp:GridView ID="gvMemos" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="Key"
                OnRowCommand="gvMemos_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" />
                    <asp:BoundField DataField="Reminder" HeaderText="Reminder" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDeleteMemo" runat="server" Text="Delete" CommandName="DeleteMemo" CommandArgument='<%# Container.DataItemIndex %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <br />
            <asp:Label ID="lblWeatherForecast" runat="server" Text=""></asp:Label>
            <br />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
    </form>
</body>
</html>