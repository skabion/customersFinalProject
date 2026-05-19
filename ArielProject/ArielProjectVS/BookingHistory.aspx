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
            <label for="<%= DdlSort.ClientID %>">מיון לפי:</label>
            <asp:DropDownList ID="DdlSort" runat="server" CssClass="sort-select"
                AutoPostBack="true" OnSelectedIndexChanged="DdlSort_SelectedIndexChanged">
                <asp:ListItem Value="DateDesc" Text="תאריך - חדש לישן" Selected="True"></asp:ListItem>
                <asp:ListItem Value="DateAsc" Text="תאריך - ישן לחדש"></asp:ListItem>
                <asp:ListItem Value="FoodType" Text="סוג מסעדה"></asp:ListItem>
                <asp:ListItem Value="Region" Text="אזור מסעדה"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="bookings-container">
            <asp:Repeater ID="RepeaterHistory" runat="server">
                <ItemTemplate>
                    <div class="booking-card">
                        <div class="booking-info">
                            <div class="booking-restaurant">🍽️ <%# Eval("Restaurant") %></div>
                            <div class="booking-tags">
                                <span class="tag">🍴 <%# Eval("FoodType") %></span>
                                <span class="tag region">📍 <%# Eval("Region") %></span>
                            </div>
                            <div class="booking-details">
                                <span><span class="label">תאריך:</span> <%# Eval("DateStr") %></span>
                                <span><span class="label">שעה:</span> <%# Eval("InvTime") %></span>
                                <span><span class="label">סועדים:</span> <%# Eval("NumGuest") %></span>
                                <span><span class="label">סוג שולחן:</span> <%# Eval("TableType") %></span>
                            </div>
                        </div>
                        <div class="past-badge">✓ הזמנה הסתיימה</div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

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
