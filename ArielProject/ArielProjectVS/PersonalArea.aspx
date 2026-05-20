<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalArea.aspx.cs" Inherits="ArielProject.PersonalArea" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - אזור אישי</title>
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
            <span class="hero-icon">👤</span>
            <h1>האזור האישי שלך</h1>
            <div class="hero-subtitle">✦ &nbsp; ניהול ההזמנות והפרטים שלך &nbsp; ✦</div>
        </div>

        <div class="gold-divider"></div>

        <div class="options-grid">
            <a href="UpdateBookings.aspx" class="option-card">
                <span class="option-icon">📝</span>
                <h3>עריכת הזמנה קיימת</h3>
                <p>צפה בהזמנות העתידיות שלך, ערוך תאריך ושעה או בטל הזמנה</p>
            </a>
            <a href="BookingHistory.aspx" class="option-card">
                <span class="option-icon">📜</span>
                <h3>היסטוריית הזמנות</h3>
                <p>צפה בכל ההזמנות מהעבר שלך, מיין לפי תאריך, סוג מסעדה או אזור</p>
            </a>
        </div>

        <div class="nav-bar">
            <a href="HomePage.aspx" class="nav-btn">← חזרה לדף הבית</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
