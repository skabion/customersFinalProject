<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantAdmin.aspx.cs" Inherits="ArielProject.RestaurantAdmin" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - דף מנהל מסעדה</title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }

        body {
            font-family: Arial, sans-serif;
            direction: rtl;
            min-height: 100vh;
            background: #0a0a1a;
            color: #f0e8d0;
            overflow-x: hidden;
        }

        body::before {
            content: '';
            position: fixed;
            top: 0; left: 0; right: 0; bottom: 0;
            background:
                radial-gradient(ellipse at 20% 20%, rgba(120, 60, 200, 0.25) 0%, transparent 50%),
                radial-gradient(ellipse at 80% 80%, rgba(180, 100, 30, 0.2) 0%, transparent 50%),
                radial-gradient(ellipse at 50% 50%, rgba(10, 40, 80, 0.8) 0%, transparent 80%),
                linear-gradient(135deg, #0d0d2b 0%, #1a0a2e 30%, #0d1f0d 60%, #1a1205 100%);
            z-index: -2;
        }

        body::after {
            content: '';
            position: fixed;
            top: 0; left: 0; right: 0; bottom: 0;
            background-image:
                radial-gradient(circle, rgba(212,175,55,0.15) 1px, transparent 1px),
                radial-gradient(circle, rgba(180,130,200,0.1) 1px, transparent 1px);
            background-size: 40px 40px, 70px 70px;
            background-position: 0 0, 20px 20px;
            z-index: -1;
            pointer-events: none;
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 18px 40px;
            background: linear-gradient(90deg,
                rgba(212,175,55,0.08) 0%,
                rgba(120,60,200,0.12) 50%,
                rgba(212,175,55,0.08) 100%);
            border-bottom: 1px solid rgba(212,175,55,0.3);
            backdrop-filter: blur(4px);
        }

        .logo {
            font-size: 28px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 14px rgba(212,175,55,0.5), 0 0 2px rgba(255,248,220,0.7);
            letter-spacing: 3px;
        }

        .greeting-area {
            font-size: 17px;
            color: #e8d5a3;
            text-shadow: 0 0 12px rgba(212,175,55,0.5);
        }

        .hero {
            text-align: center;
            padding: 45px 20px 18px;
        }

        .hero-icon {
            font-size: 52px;
            display: block;
            margin-bottom: 8px;
            animation: float 3s ease-in-out infinite;
        }

        @keyframes float {
            0%, 100% { transform: translateY(0); }
            50%       { transform: translateY(-6px); }
        }

        .hero h1 {
            font-size: 30px;
            letter-spacing: 3px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 18px rgba(212,175,55,0.55), 0 0 3px rgba(255,248,220,0.8);
            margin-bottom: 8px;
        }

        .restaurant-tag {
            display: inline-block;
            margin-top: 4px;
            padding: 8px 22px;
            background: linear-gradient(90deg, rgba(212,175,55,0.18), rgba(245,226,122,0.1));
            border: 1px solid rgba(212,175,55,0.5);
            border-radius: 50px;
            color: #fff8dc;
            font-size: 17px;
            font-weight: bold;
            letter-spacing: 2px;
        }

        .gold-divider {
            width: 200px;
            height: 2px;
            margin: 30px auto;
            background: linear-gradient(90deg, transparent, #d4af37, #f5e27a, #d4af37, transparent);
            border-radius: 2px;
        }

        .section-title {
            text-align: center;
            color: #f5e27a;
            font-size: 14px;
            letter-spacing: 5px;
            text-transform: uppercase;
            margin: 12px 0 22px;
            font-weight: bold;
        }

        /* --- KPI ROW --- */
        .kpi-row {
            display: flex;
            justify-content: center;
            gap: 18px;
            flex-wrap: wrap;
            padding: 0 30px 40px;
            max-width: 1100px;
            margin: 0 auto;
        }

        .kpi-card {
            flex: 1;
            min-width: 200px;
            max-width: 250px;
            background: linear-gradient(145deg,
                rgba(255,255,255,0.07) 0%,
                rgba(212,175,55,0.07) 50%,
                rgba(120,60,200,0.08) 100%);
            border: 1px solid rgba(212,175,55,0.3);
            border-radius: 14px;
            padding: 24px 18px;
            text-align: center;
            backdrop-filter: blur(6px);
            transition: transform 0.3s, box-shadow 0.3s;
        }

        .kpi-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 10px 30px rgba(212,175,55,0.18);
        }

        .kpi-icon {
            font-size: 32px;
            display: block;
            margin-bottom: 8px;
        }

        .kpi-label {
            font-size: 13px;
            color: rgba(212,175,55,0.7);
            letter-spacing: 1px;
            margin-bottom: 10px;
            min-height: 32px;
        }

        .kpi-value {
            font-size: 36px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 14px rgba(245,226,122,0.5);
        }

        /* --- CHART SECTION --- */
        .chart-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(360px, 1fr));
            gap: 22px;
            max-width: 1100px;
            margin: 0 auto;
            padding: 0 30px 40px;
        }

        .chart-card {
            background: linear-gradient(145deg,
                rgba(255,255,255,0.05) 0%,
                rgba(212,175,55,0.05) 50%,
                rgba(120,60,200,0.06) 100%);
            border: 1px solid rgba(212,175,55,0.25);
            border-radius: 14px;
            padding: 22px 24px;
            backdrop-filter: blur(6px);
        }

        .chart-title {
            font-size: 17px;
            color: #f5e27a;
            font-weight: bold;
            margin-bottom: 18px;
            border-bottom: 1px solid rgba(212,175,55,0.2);
            padding-bottom: 10px;
        }

        .bar-row {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 9px;
            font-size: 13px;
        }

        .bar-label {
            width: 95px;
            text-align: right;
            color: #e8d5a3;
            flex-shrink: 0;
        }

        .bar-track {
            flex: 1;
            height: 22px;
            background: rgba(0,0,0,0.35);
            border: 1px solid rgba(212,175,55,0.15);
            border-radius: 11px;
            overflow: hidden;
        }

        .bar-fill {
            height: 100%;
            background: linear-gradient(90deg, #c9954c, #d4af37, #f5e27a);
            border-radius: 11px;
            transition: width 0.6s ease;
            box-shadow: 0 0 10px rgba(212,175,55,0.35);
        }

        .bar-fill.purple {
            background: linear-gradient(90deg, #6e3aa7, #9e60d0, #c79ce8);
            box-shadow: 0 0 10px rgba(150,80,200,0.35);
        }

        .bar-fill.green {
            background: linear-gradient(90deg, #3b7a4a, #5fa066, #a6d8a6);
            box-shadow: 0 0 10px rgba(95,160,102,0.3);
        }

        .bar-value {
            min-width: 30px;
            text-align: left;
            font-weight: bold;
            color: #f5e27a;
            flex-shrink: 0;
        }

        .chart-empty {
            text-align: center;
            color: rgba(240,232,208,0.5);
            padding: 25px 10px;
            font-size: 14px;
        }

        /* --- UPCOMING TABLE --- */
        .upcoming-wrapper {
            max-width: 1100px;
            margin: 0 auto;
            padding: 0 30px 40px;
        }

        .upcoming-table {
            width: 100%;
            border-collapse: collapse;
            background: linear-gradient(145deg,
                rgba(255,255,255,0.05) 0%,
                rgba(212,175,55,0.04) 100%);
            border: 1px solid rgba(212,175,55,0.25);
            border-radius: 14px;
            overflow: hidden;
        }

        .upcoming-table th {
            background: rgba(212,175,55,0.15);
            color: #f5e27a;
            font-size: 14px;
            padding: 12px 14px;
            text-align: right;
            border-bottom: 1px solid rgba(212,175,55,0.3);
            letter-spacing: 1px;
        }

        .upcoming-table td {
            padding: 12px 14px;
            color: rgba(240,232,208,0.85);
            font-size: 14px;
            border-bottom: 1px solid rgba(212,175,55,0.1);
        }

        .upcoming-table tr:last-child td { border-bottom: none; }

        .upcoming-table tr:hover td {
            background: rgba(212,175,55,0.05);
        }

        .empty-message {
            text-align: center;
            padding: 30px 20px;
            background: rgba(255,255,255,0.03);
            border: 1px dashed rgba(212,175,55,0.3);
            border-radius: 14px;
            color: rgba(240,232,208,0.6);
            font-size: 14px;
        }

        .nav-bar {
            text-align: center;
            padding: 25px 20px 30px;
        }

        .nav-btn {
            display: inline-block;
            padding: 12px 28px;
            border-radius: 50px;
            font-size: 14px;
            font-weight: bold;
            text-decoration: none;
            transition: all 0.3s;
            letter-spacing: 1px;
            border: 1.5px solid rgba(212,175,55,0.55);
            color: #d4af37;
            background: transparent;
        }

        .nav-btn:hover {
            background: rgba(212,175,55,0.1);
            border-color: #d4af37;
            box-shadow: 0 4px 20px rgba(212,175,55,0.2);
            transform: translateY(-2px);
        }

        .footer {
            text-align: center;
            padding: 20px;
            font-size: 13px;
            color: rgba(212,175,55,0.3);
            border-top: 1px solid rgba(212,175,55,0.1);
        }
    </style>
</head>
<body>
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
            <div class="restaurant-tag">🍽️ <asp:Label ID="LblRestaurantName" runat="server"></asp:Label></div>
        </div>

        <div class="gold-divider"></div>

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

        <div class="nav-bar">
            <a href="HomePage.aspx" class="nav-btn">← חזרה לדף הבית</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
