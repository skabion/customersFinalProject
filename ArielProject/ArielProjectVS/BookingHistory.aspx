<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingHistory.aspx.cs" Inherits="ArielProject.BookingHistory" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - היסטוריית הזמנות</title>
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
            padding: 50px 20px 20px;
        }

        .hero-icon {
            font-size: 48px;
            display: block;
            margin-bottom: 10px;
        }

        .hero h1 {
            font-size: 34px;
            letter-spacing: 4px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 18px rgba(212,175,55,0.55), 0 0 3px rgba(255,248,220,0.8);
            margin-bottom: 12px;
        }

        .hero-subtitle {
            font-size: 13px;
            color: rgba(212,175,55,0.65);
            letter-spacing: 6px;
            text-transform: uppercase;
            margin-bottom: 30px;
        }

        .gold-divider {
            width: 200px;
            height: 2px;
            margin: 0 auto 30px;
            background: linear-gradient(90deg, transparent, #d4af37, #f5e27a, #d4af37, transparent);
            border-radius: 2px;
        }

        .sort-bar {
            max-width: 900px;
            margin: 0 auto 25px;
            padding: 0 30px;
            display: flex;
            align-items: center;
            justify-content: flex-end;
            gap: 14px;
            flex-wrap: wrap;
        }

        .sort-bar label {
            font-size: 14px;
            color: #e8d5a3;
            font-weight: bold;
        }

        .sort-select {
            padding: 9px 14px;
            background: rgba(0,0,0,0.4);
            border: 1px solid rgba(212,175,55,0.4);
            border-radius: 8px;
            color: #f0e8d0;
            font-size: 14px;
            font-family: Arial, sans-serif;
            cursor: pointer;
            min-width: 200px;
            transition: border-color 0.3s;
        }

        .sort-select:focus {
            outline: none;
            border-color: rgba(212,175,55,0.75);
        }

        .bookings-container {
            max-width: 900px;
            margin: 0 auto;
            padding: 0 30px 50px;
        }

        .booking-card {
            background: linear-gradient(145deg,
                rgba(255,255,255,0.04) 0%,
                rgba(212,175,55,0.04) 50%,
                rgba(120,60,200,0.05) 100%);
            border: 1px solid rgba(212,175,55,0.2);
            border-radius: 14px;
            padding: 22px 26px;
            margin-bottom: 16px;
            backdrop-filter: blur(6px);
            display: flex;
            align-items: center;
            gap: 22px;
            flex-wrap: wrap;
            opacity: 0.92;
        }

        .booking-card .booking-info {
            flex: 1;
            min-width: 250px;
        }

        .booking-restaurant {
            font-size: 21px;
            color: #f5e27a;
            font-weight: bold;
            margin-bottom: 6px;
        }

        .booking-tags {
            display: flex;
            gap: 8px;
            flex-wrap: wrap;
            margin-bottom: 10px;
        }

        .tag {
            display: inline-block;
            padding: 3px 12px;
            border-radius: 50px;
            font-size: 12px;
            background: rgba(212,175,55,0.12);
            border: 1px solid rgba(212,175,55,0.3);
            color: #e8d5a3;
        }

        .tag.region {
            background: rgba(120,60,200,0.15);
            border-color: rgba(180,130,220,0.35);
            color: #d6c1f0;
        }

        .booking-details {
            font-size: 13px;
            color: rgba(240,232,208,0.75);
            line-height: 1.9;
        }

        .booking-details span {
            display: inline-block;
            margin-left: 18px;
        }

        .booking-details .label {
            color: rgba(212,175,55,0.7);
            font-weight: bold;
            margin-left: 4px;
        }

        .past-badge {
            background: rgba(255,255,255,0.05);
            border: 1px solid rgba(212,175,55,0.25);
            color: rgba(240,232,208,0.55);
            font-size: 12px;
            padding: 6px 14px;
            border-radius: 50px;
            letter-spacing: 1px;
        }

        .empty-message {
            text-align: center;
            padding: 50px 20px;
            background: rgba(255,255,255,0.03);
            border: 1px dashed rgba(212,175,55,0.3);
            border-radius: 14px;
            color: rgba(240,232,208,0.7);
            font-size: 16px;
            line-height: 1.8;
        }

        .empty-message .empty-icon {
            font-size: 48px;
            display: block;
            margin-bottom: 15px;
        }

        .nav-bar {
            text-align: center;
            padding: 20px;
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
