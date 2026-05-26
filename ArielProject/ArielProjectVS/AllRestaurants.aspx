<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllRestaurants.aspx.cs" Inherits="ArielProject.AllRestaurants" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - כל המסעדות (מנהל מערכת)</title>
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
            <span class="hero-icon">🍽️</span>
            <h1>כל המסעדות במערכת</h1>
            <div class="hero-subtitle">✦ &nbsp; בחר מסעדה לצפייה בנתונים &nbsp; ✦</div>
            <div class="admin-badge">⚙️ מצב מנהל מערכת</div>
        </div>

        <div class="gold-divider"></div>

        <div class="restaurants-grid">

            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="True"
                AutoGenerateSelectButton="True"
                SelectText="📊 לצפייה בנתונים"
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                Width="100%"
                CssClass="times-grid">
            </asp:GridView>
        </div>

        <asp:Panel ID="PnlEmpty" runat="server" Visible="false">
            <div style="max-width: 600px; margin: 0 auto; padding: 0 30px 40px;">
                <div class="empty-message">
                    📭 אין מסעדות רשומות במערכת
                </div>
            </div>
        </asp:Panel>

        <div class="nav-bar">
            <a href="RestaurantAdmin.aspx" class="nav-btn">← חזרה לדף מנהל</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
