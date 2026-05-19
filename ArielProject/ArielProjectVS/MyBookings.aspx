<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBookings.aspx.cs" Inherits="ArielProject.MyBookings" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - ההזמנות שלי</title>
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
            <span class="hero-icon">📋</span>
            <h1>ההזמנות העתידיות שלי</h1>
            <div class="hero-subtitle">✦ &nbsp; בחר הזמנה לעריכה &nbsp; ✦</div>
        </div>

        <div class="gold-divider"></div>

        <div class="bookings-container">
            <%-- הוחלף ה-Repeater עם <ItemTemplate> ו-Eval() (לא נלמדים בתיכון)
                 ב-GridView פשוט עם AutoGenerateColumns שמציג את כל עמודות
                 ה-DataTable באופן אוטומטי. כפתור הבחירה (Select) מוסיף קישור
                 לכל שורה כדי לערוך את ההזמנה. --%>
            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="True"
                AutoGenerateSelectButton="True"
                SelectText="✏️ ערוך"
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                Width="100%"
                CssClass="times-grid">
            </asp:GridView>

            <asp:Panel ID="PnlEmpty" runat="server" Visible="false">
                <div class="empty-message">
                    <span class="empty-icon">📭</span>
                    <p>אין לך הזמנות עתידיות.</p>
                    <p><a href="Catalog.aspx">לחץ כאן להזמנת מסעדה</a></p>
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
