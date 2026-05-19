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
            <asp:Repeater ID="RepeaterBookings" runat="server">
                <ItemTemplate>
                    <div class="booking-card">
                        <div class="booking-info">
                            <div class="booking-restaurant">🍽️ <%# Eval("Restaurant") %></div>
                            <div class="booking-details">
                                <span><span class="label">תאריך:</span> <%# Eval("DateStr") %></span>
                                <span><span class="label">שעה:</span> <%# Eval("InvTime") %></span>
                                <span><span class="label">סועדים:</span> <%# Eval("NumGuest") %></span>
                                <span><span class="label">סוג שולחן:</span> <%# Eval("TableType") %></span>
                            </div>
                        </div>
                        <div class="booking-actions">
                            <a href='<%# "Update.aspx?date=" + Eval("DateStr") + "&time=" + Eval("InvTime") %>' class="btn-edit">✏️ ערוך הזמנה</a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

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
