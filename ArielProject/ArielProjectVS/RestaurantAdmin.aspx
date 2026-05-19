<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantAdmin.aspx.cs" Inherits="ArielProject.RestaurantAdmin" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - דף מנהל מסעדה</title>
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
            <span class="hero-icon">👑</span>
            <h1>דף מנהל מסעדה</h1>
            <asp:Panel ID="PnlRestaurantTag" runat="server">
                <div class="restaurant-tag">🍽️ <asp:Label ID="LblRestaurantName" runat="server"></asp:Label></div>
            </asp:Panel>
            <asp:Panel ID="PnlAdminBadge" runat="server" Visible="false">
                <div class="restaurant-tag">⚙️ מנהל מערכת</div>
            </asp:Panel>
        </div>

        <div class="gold-divider"></div>

        <asp:Panel ID="PnlStats" runat="server">
        <div class="section-title">✦ &nbsp; סטטיסטיקה כללית &nbsp; ✦</div>

        <div class="kpi-row">
            <div class="kpi-card">
                <span class="kpi-icon">📊</span>
                <div class="kpi-label">סך כל ההזמנות</div>
                <div class="kpi-value"><asp:Label ID="LblTotalCount" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="kpi-card">
                <span class="kpi-icon">📅</span>
                <div class="kpi-label">הזמנות עתידיות</div>
                <div class="kpi-value"><asp:Label ID="LblUpcomingCount" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="kpi-card">
                <span class="kpi-icon">👥</span>
                <div class="kpi-label">סך סועדים שהתארחו</div>
                <div class="kpi-value"><asp:Label ID="LblTotalGuests" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="kpi-card">
                <span class="kpi-icon">📈</span>
                <div class="kpi-label">ממוצע סועדים להזמנה</div>
                <div class="kpi-value"><asp:Label ID="LblAvgGuests" runat="server" Text="0"></asp:Label></div>
            </div>
        </div>

        <div class="section-title">✦ &nbsp; פילוחים וגרפים &nbsp; ✦</div>

        <div class="chart-grid">
            <div class="chart-card">
                <div class="chart-title">🪑 התפלגות לפי גודל שולחן</div>
                <asp:Repeater ID="RepeaterTableTypes" runat="server">
                    <ItemTemplate>
                        <div class="bar-row">
                            <div class="bar-label"><%# Eval("Label") %></div>
                            <div class="bar-track">
                                <div class="bar-fill" style="width: <%# Eval("Percent") %>%;"></div>
                            </div>
                            <div class="bar-value"><%# Eval("Count") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="PnlEmptyTable" runat="server" Visible="false" CssClass="chart-empty">
                    אין נתונים להצגה
                </asp:Panel>
            </div>

            <div class="chart-card">
                <div class="chart-title">⏰ השעות הפופולריות ביותר</div>
                <asp:Repeater ID="RepeaterTimes" runat="server">
                    <ItemTemplate>
                        <div class="bar-row">
                            <div class="bar-label"><%# Eval("Label") %></div>
                            <div class="bar-track">
                                <div class="bar-fill purple" style="width: <%# Eval("Percent") %>%;"></div>
                            </div>
                            <div class="bar-value"><%# Eval("Count") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="PnlEmptyTimes" runat="server" Visible="false" CssClass="chart-empty">
                    אין נתונים להצגה
                </asp:Panel>
            </div>

            <div class="chart-card">
                <div class="chart-title">📆 הזמנות לפי יום בשבוע</div>
                <asp:Repeater ID="RepeaterDays" runat="server">
                    <ItemTemplate>
                        <div class="bar-row">
                            <div class="bar-label"><%# Eval("Label") %></div>
                            <div class="bar-track">
                                <div class="bar-fill green" style="width: <%# Eval("Percent") %>%;"></div>
                            </div>
                            <div class="bar-value"><%# Eval("Count") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="section-title">✦ &nbsp; 5 ההזמנות הקרובות &nbsp; ✦</div>

        <div class="upcoming-wrapper">
            <asp:Repeater ID="RepeaterUpcoming" runat="server">
                <HeaderTemplate>
                    <table class="upcoming-table">
                        <tr>
                            <th>תאריך</th>
                            <th>שעה</th>
                            <th>שם הסועד</th>
                            <th>טלפון</th>
                            <th>סועדים</th>
                            <th>שולחן</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("DateStr") %></td>
                        <td><%# Eval("InvTime") %></td>
                        <td><%# Eval("Guest") %></td>
                        <td><%# Eval("PhoneNum") %></td>
                        <td><%# Eval("NumGuest") %></td>
                        <td><%# Eval("TableType") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Panel ID="PnlEmptyUpcoming" runat="server" Visible="false">
                <div class="empty-message">📭 אין הזמנות עתידיות במסעדה כרגע</div>
            </asp:Panel>
        </div>
        </asp:Panel>

        <asp:Panel ID="PnlAdminMenu" runat="server" Visible="false">
            <div class="admin-menu-card">
                <h3>ברוך הבא, מנהל מערכת</h3>
                <p>בחר באפשרות הרצויה לניהול וצפייה בנתוני המערכת</p>
                <div class="admin-actions">
                    <asp:HyperLink ID="LnkAllRestaurants" runat="server" NavigateUrl="AllRestaurants.aspx" CssClass="admin-action-btn">
                        📊 הצגת נתונים על כל המסעדות
                    </asp:HyperLink>
                    <asp:HyperLink ID="LnkUserStats" runat="server" NavigateUrl="UserStats.aspx" CssClass="admin-action-btn">
                        👥 נתוני משתמשים
                    </asp:HyperLink>
                </div>
            </div>
        </asp:Panel>

        <div class="nav-bar">
            <asp:HyperLink ID="BackLink" runat="server" CssClass="nav-btn" NavigateUrl="HomePage.aspx" Text="← חזרה לדף הבית"></asp:HyperLink>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
