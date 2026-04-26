<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ArielProject.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div dir="rtl" style="text-align: center; margin-top: 50px;">
    <h2>התחברות למערכת</h2>
    שם מלא: <asp:TextBox ID="TxtFullName" runat="server"></asp:TextBox>
    <br /><br />
    סיסמה: <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password"></asp:TextBox>
    <br /><br />
    <asp:Button ID="BtnLogin" runat="server" Text="התחבר" OnClick="BtnLogin_Click" />
    <br /><br />
    <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
</div>
    </form>
</body>
</html>
