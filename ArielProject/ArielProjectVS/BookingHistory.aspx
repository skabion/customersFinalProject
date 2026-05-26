<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingHistory.aspx.cs" Inherits="ArielProject.BookingHistory" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - היסטוריית הזמנות</title>
    <%-- כל העיצוב מרוכז בקובץ Site.css --%>
    <link href="Site.css" rel="stylesheet" />
</head>
<body class="theme-dark">
    <form id="form1" runat="server">

        <div class="header">
            <div class="logo">✦ EatIt ✦</div>
            <div class="greeting-area">
                שלום, <asp:Label ID="LblUserName" runat="server"></asp:Label>
            </div>
        </div>

        <div class="hero">
            <span class="hero-icon">📜</span>
            <h1>היסטוריית ההזמנות שלי</h1>
            <div class="hero-subtitle">✦ &nbsp; הזמנות מהעבר &nbsp; ✦</div>
        </div>

        <div class="gold-divider"></div>

        <div class="sort-bar">

            <label>מיון לפי:</label>
            <asp:DropDownList ID="DdlSort" runat="server" CssClass="sort-select"
                AutoPostBack="true" OnSelectedIndexChanged="DdlSort_SelectedIndexChanged">
                <asp:ListItem Value="DateDesc" Text="תאריך - חדש לישן" Selected="True"></asp:ListItem>
                <asp:ListItem Value="DateAsc" Text="תאריך - ישן לחדש"></asp:ListItem>
                <asp:ListItem Value="FoodType" Text="סוג מסעדה"></asp:ListItem>
                <asp:ListItem Value="Region" Text="אזור מסעדה"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <%-- כפתור מחיקת כל ההיסטוריה. OnClientClick מציג חלון אישור ב-JS
             לפני שהפעולה מגיעה לשרת - כדי שלא ימחק בטעות. --%>
        <div style="text-align:center; margin: 10px 0;">
            <asp:Button ID="BtnClear" runat="server" Text="🗑️ ניקוי היסטוריית הזמנות"
                OnClick="BtnClear_Click" CssClass="btn-delete"
                OnClientClick="return confirm('האם אתה בטוח שברצונך למחוק את כל ההיסטוריה? פעולה זו אינה הפיכה.');" />
        </div>

        <div class="bookings-container">

            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="True"
                Width="100%"
                CssClass="times-grid">
            </asp:GridView>

            <asp:Panel ID="PnlEmpty" runat="server" Visible="false">
                <div class="empty-message">
                    <span class="empty-icon">📭</span>
                    <p>אין לך עדיין הזמנות בהיסטוריה.</p>
                    <p>הזמנות שתאריכן עבר יופיעו כאן.</p>
                </div>
            </asp:Panel>
        </div>

        <div class="nav-bar">
            <a href="PersonalArea.aspx" class="nav-btn">← חזרה לאזור האישי</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
