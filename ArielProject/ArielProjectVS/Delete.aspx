<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="ArielProject.Delete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ניקוי הזמנות - ADMIN</title>
</head>
<body>
    <form id="form1" runat="server">
<div dir="rtl" style="text-align: center; margin-top: 50px;">
    <h2>ניהול מערכת: ניקוי היסטוריית הזמנות</h2>
    <p>פעולה זו תמחק לצמיתות את כל ההזמנות שתאריכן עבר.</p>
    
    <asp:Button ID="BtnDeleteHistory" runat="server" Text="מחק הזמנות ישנות" 
        OnClick="BtnDeleteHistory_Click" BackColor="Red" ForeColor="White" 
        OnClientClick="return confirm('האם אתה בטוח שברצונך למחוק את כל ההיסטוריה?');" />
    <br /><br />
    <asp:Label ID="LblStatus" runat="server" Text=""></asp:Label>
</div>
    </form>
</body>
</html>
