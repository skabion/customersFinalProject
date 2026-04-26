<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Select.aspx.cs" Inherits="ArielProject.calculator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <br />
       
        <asp:Button ID="Button1" runat="server" OnClick="SelectUsers" Text="ShowDBInfo" Height="78px" Width="217px" />
        <asp:GridView ID="GV1" runat="server">
        </asp:GridView>
        <asp:DropDownList ID="ddlRegion" runat="server">
    <asp:ListItem Text="All Regions" Value="All Regions" />
    <asp:ListItem Text="Tzafon" Value="Tzafon" />
    <asp:ListItem Text="Merkaz" Value="Merkaz" />
    <asp:ListItem Text="Darom" Value="Darom" />
</asp:DropDownList>

    </form>
</body>
</html>
