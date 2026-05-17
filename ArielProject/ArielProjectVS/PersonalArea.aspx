<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalArea.aspx.cs" Inherits="ArielProject.PersonalArea" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - אזור אישי</title>
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
            padding: 60px 20px 30px;
        }

        .hero-icon {
            font-size: 56px;
            display: block;
            margin-bottom: 10px;
        }

        .hero h1 {
            font-size: 38px;
            letter-spacing: 4px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 18px rgba(212,175,55,0.55), 0 0 3px rgba(255,248,220,0.8);
            margin-bottom: 12px;
        }

        .hero-subtitle {
            font-size: 14px;
            color: rgba(212,175,55,0.65);
            letter-spacing: 6px;
            text-transform: uppercase;
            margin-bottom: 30px;
        }

        .gold-divider {
            width: 200px;
            height: 2px;
            margin: 0 auto 40px;
            background: linear-gradient(90deg, transparent, #d4af37, #f5e27a, #d4af37, transparent);
            border-radius: 2px;
        }

        .options-grid {
            display: flex;
            justify-content: center;
            gap: 24px;
            flex-wrap: wrap;
            padding: 0 30px 50px;
            max-width: 1000px;
            margin: 0 auto;
        }

        .option-card {
            background: linear-gradient(145deg,
                rgba(255,255,255,0.07) 0%,
                rgba(212,175,55,0.06) 50%,
                rgba(120,60,200,0.08) 100%);
            border: 1px solid rgba(212,175,55,0.25);
            border-radius: 16px;
            padding: 36px 28px;
            width: 260px;
            text-align: center;
            backdrop-filter: blur(6px);
            transition: transform 0.3s, box-shadow 0.3s, border-color 0.3s;
            text-decoration: none;
            color: inherit;
            display: block;
        }

        .option-card:hover {
            transform: translateY(-6px);
            box-shadow: 0 16px 40px rgba(212,175,55,0.2), 0 0 0 1px rgba(212,175,55,0.4);
            border-color: rgba(212,175,55,0.6);
            text-decoration: none;
        }

        .option-icon {
            font-size: 42px;
            margin-bottom: 14px;
            display: block;
        }

        .option-card h3 {
            font-size: 18px;
            color: #f5e27a;
            margin-bottom: 10px;
        }

        .option-card p {
            font-size: 13px;
            color: rgba(240,232,208,0.7);
            line-height: 1.7;
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
            <span class="hero-icon">👤</span>
            <h1>האזור האישי שלך</h1>
            <div class="hero-subtitle">✦ &nbsp; ניהול ההזמנות והפרטים שלך &nbsp; ✦</div>
        </div>

        <div class="gold-divider"></div>

        <div class="options-grid">
            <a href="MyBookings.aspx" class="option-card">
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
