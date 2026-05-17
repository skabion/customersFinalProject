<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllRestaurants.aspx.cs" Inherits="ArielProject.AllRestaurants" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - כל המסעדות (מנהל מערכת)</title>
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
            font-size: 50px;
            display: block;
            margin-bottom: 10px;
        }

        .hero h1 {
            font-size: 32px;
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
            margin-bottom: 8px;
        }

        .admin-badge {
            display: inline-block;
            margin-top: 8px;
            padding: 6px 18px;
            background: rgba(120,60,200,0.15);
            border: 1px solid rgba(180,130,220,0.4);
            border-radius: 50px;
            color: #d6c1f0;
            font-size: 13px;
            letter-spacing: 1px;
        }

        .gold-divider {
            width: 200px;
            height: 2px;
            margin: 30px auto;
            background: linear-gradient(90deg, transparent, #d4af37, #f5e27a, #d4af37, transparent);
            border-radius: 2px;
        }

        .restaurants-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
            gap: 20px;
            max-width: 1100px;
            margin: 0 auto;
            padding: 0 30px 50px;
        }

        .restaurant-card {
            background: linear-gradient(145deg,
                rgba(255,255,255,0.06) 0%,
                rgba(212,175,55,0.06) 50%,
                rgba(120,60,200,0.08) 100%);
            border: 1px solid rgba(212,175,55,0.3);
            border-radius: 14px;
            padding: 26px 24px;
            text-align: center;
            backdrop-filter: blur(6px);
            text-decoration: none;
            color: inherit;
            display: block;
            transition: transform 0.3s, box-shadow 0.3s, border-color 0.3s;
        }

        .restaurant-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 14px 36px rgba(212,175,55,0.22);
            border-color: rgba(212,175,55,0.6);
            text-decoration: none;
        }

        .restaurant-icon {
            font-size: 36px;
            display: block;
            margin-bottom: 8px;
        }

        .restaurant-name {
            font-size: 20px;
            font-weight: bold;
            color: #f5e27a;
            margin-bottom: 12px;
            letter-spacing: 1px;
        }

        .restaurant-tags {
            display: flex;
            gap: 8px;
            justify-content: center;
            flex-wrap: wrap;
            margin-bottom: 14px;
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

        .view-stats-hint {
            font-size: 13px;
            color: rgba(212,175,55,0.7);
            font-weight: bold;
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
            <span class="hero-icon">🍽️</span>
            <h1>כל המסעדות במערכת</h1>
            <div class="hero-subtitle">✦ &nbsp; בחר מסעדה לצפייה בנתונים &nbsp; ✦</div>
            <div class="admin-badge">⚙️ מצב מנהל מערכת</div>
        </div>

        <div class="gold-divider"></div>

        <div class="restaurants-grid">
            <asp:Repeater ID="RepeaterRestaurants" runat="server">
                <ItemTemplate>
                    <a href='RestaurantAdmin.aspx?restaurant=<%# Eval("EncodedName") %>' class="restaurant-card">
                        <span class="restaurant-icon">🏛️</span>
                        <div class="restaurant-name"><%# Eval("Name") %></div>
                        <div class="restaurant-tags">
                            <span class="tag">🍴 <%# Eval("FoodType") %></span>
                            <span class="tag region">📍 <%# Eval("Region") %></span>
                        </div>
                        <div class="view-stats-hint">📊 לצפייה בנתונים ←</div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
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
