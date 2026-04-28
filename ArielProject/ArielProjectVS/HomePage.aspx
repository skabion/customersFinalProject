<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="ArielProject.HomePage" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - מערכת הזמנות מסעדות</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@400;700&display=swap');

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: Arial, sans-serif;
            direction: rtl;
            min-height: 100vh;
            background: #0a0a1a;
            color: #f0e8d0;
            overflow-x: hidden;
        }

        /* רקע מלכותי עם שכבות */
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

        /* טקסטורה עדינה של נקודות זהב */
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

        /* --- HEADER --- */
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
            animation: shimmer 3s infinite ease-in-out;
            letter-spacing: 3px;
        }

        @keyframes shimmer {
            0%, 100% { color: #f5e27a; text-shadow: 0 0 14px rgba(212,175,55,0.5); }
            50%      { color: #fff8dc; text-shadow: 0 0 20px rgba(245,226,122,0.95); }
        }

        .greeting-area {
            font-size: 17px;
            color: #e8d5a3;
            text-shadow: 0 0 12px rgba(212,175,55,0.5);
        }

        /* --- HERO --- */
        .hero {
            text-align: center;
            padding: 80px 20px 50px;
            position: relative;
        }

        .hero-crown {
            font-size: 48px;
            display: block;
            margin-bottom: 10px;
            animation: float 3s ease-in-out infinite;
        }

        @keyframes float {
            0%, 100% { transform: translateY(0); }
            50%       { transform: translateY(-8px); }
        }

        .hero h1 {
            font-size: 42px;
            letter-spacing: 4px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 18px rgba(212,175,55,0.55), 0 0 3px rgba(255,248,220,0.8), 0 2px 0 rgba(0,0,0,0.4);
            margin-bottom: 12px;
        }

        .hero-subtitle {
            font-size: 15px;
            color: rgba(212,175,55,0.65);
            letter-spacing: 6px;
            text-transform: uppercase;
            margin-bottom: 50px;
        }

        /* --- CARDS --- */
        .cards-row {
            display: flex;
            justify-content: center;
            gap: 24px;
            flex-wrap: wrap;
            padding: 0 30px 60px;
        }

        .card {
            background: linear-gradient(145deg,
                rgba(255,255,255,0.07) 0%,
                rgba(212,175,55,0.06) 50%,
                rgba(120,60,200,0.08) 100%);
            border: 1px solid rgba(212,175,55,0.25);
            border-radius: 16px;
            padding: 30px 24px;
            max-width: 240px;
            text-align: center;
            backdrop-filter: blur(6px);
            transition: transform 0.3s, box-shadow 0.3s, border-color 0.3s;
            position: relative;
            overflow: hidden;
        }

        .card::before {
            content: '';
            position: absolute;
            top: -50%; left: -50%;
            width: 200%; height: 200%;
            background: radial-gradient(circle, rgba(212,175,55,0.08) 0%, transparent 60%);
            opacity: 0;
            transition: opacity 0.3s;
        }

        .card:hover::before { opacity: 1; }

        .card:hover {
            transform: translateY(-6px);
            box-shadow: 0 16px 40px rgba(212,175,55,0.2), 0 0 0 1px rgba(212,175,55,0.4);
            border-color: rgba(212,175,55,0.5);
        }

        .card-icon { font-size: 36px; margin-bottom: 12px; display: block; }

        .card h3 {
            font-size: 17px;
            color: #f5e27a;
            margin-bottom: 8px;
        }

        .card p {
            font-size: 13px;
            color: rgba(240,232,208,0.65);
            line-height: 1.7;
        }

        /* --- TEXT SECTION --- */
        .about-section {
            max-width: 820px;
            margin: 0 auto;
            padding: 0 30px 60px;
        }

        .about-section p {
            font-size: 16px;
            line-height: 2.1;
            color: rgba(240,232,208,0.8);
            margin-bottom: 22px;
            padding: 20px 24px;
            background: rgba(255,255,255,0.03);
            border-right: 3px solid rgba(212,175,55,0.5);
            border-radius: 0 10px 10px 0;
        }

        /* --- DIVIDER --- */
        .gold-divider {
            width: 200px;
            height: 2px;
            margin: 0 auto 50px;
            background: linear-gradient(90deg, transparent, #d4af37, #f5e27a, #d4af37, transparent);
            border-radius: 2px;
        }

        /* --- NAV BUTTONS --- */
        .nav-bar {
            text-align: center;
            padding: 30px 20px 50px;
            display: flex;
            justify-content: center;
            align-items: center;
            flex-wrap: wrap;
            gap: 16px;
        }

        .nav-btn {
            display: inline-block;
            padding: 13px 30px;
            border-radius: 50px;
            font-size: 15px;
            font-weight: bold;
            text-decoration: none;
            transition: all 0.3s;
            letter-spacing: 1px;
            cursor: pointer;
            border: none;
            font-family: Arial, sans-serif;
        }

        .nav-btn-gold {
            background: linear-gradient(135deg, #d4af37, #f5e27a, #c9954c);
            color: #1a0a2e;
            box-shadow: 0 4px 20px rgba(212,175,55,0.35);
        }

        .nav-btn-gold:hover {
            box-shadow: 0 6px 28px rgba(212,175,55,0.6);
            transform: translateY(-2px);
            filter: brightness(1.1);
        }

        .nav-btn-outline {
            background: transparent;
            color: #d4af37;
            border: 1.5px solid rgba(212,175,55,0.55);
        }

        .nav-btn-outline:hover {
            background: rgba(212,175,55,0.1);
            border-color: #d4af37;
            box-shadow: 0 4px 20px rgba(212,175,55,0.2);
            transform: translateY(-2px);
        }

        .nav-btn-danger {
            background: transparent;
            color: #ff8888;
            border: 1.5px solid rgba(255,100,100,0.4);
        }

        .nav-btn-danger:hover {
            background: rgba(255,80,80,0.1);
            border-color: #ff6666;
            transform: translateY(-2px);
        }

        /* --- FOOTER --- */
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

        <!-- HEADER -->
        <div class="header">
            <div class="logo">✦ EatIt ✦</div>
            <div class="greeting-area">
                <span id="greetingText"></span>
                <asp:Label ID="LblUserName" runat="server"></asp:Label>
            </div>
        </div>

        <!-- HERO -->
        <div class="hero">
            <span class="hero-crown">👑</span>
            <h1>ברוכים הבאים למערכת EatIt</h1>
            <div class="hero-subtitle">✦ &nbsp; חוויית ההזמנה המושלמת &nbsp; ✦</div>
        </div>

        <div class="gold-divider"></div>

        <!-- FEATURE CARDS -->
        <div class="cards-row">
            <div class="card">
                <span class="card-icon">🔍</span>
                <h3>סינון חכם</h3>
                <p>מצא את המסעדה המושלמת לפי אזור, סוג אוכל, כשרות והעדפות תזונתיות</p>
            </div>
            <div class="card">
                <span class="card-icon">📅</span>
                <h3>הזמנה מיידית</h3>
                <p>בחר תאריך, שעה וקבל אישור הזמנה בזמן אמת ללא המתנה</p>
            </div>
            <div class="card">
                <span class="card-icon">🍽️</span>
                <h3>15 מסעדות מובחרות</h3>
                <p>קולינריה ממיטב המסעדות בישראל — מהצפון ועד הדרום</p>
            </div>
            <div class="card">
                <span class="card-icon">⭐</span>
                <h3>חווית לקוח מלכותית</h3>
                <p>מערכת נוחה, מהירה ואינטואיטיבית לכל המשפחה</p>
            </div>
        </div>

        <div class="gold-divider"></div>

        <!-- ABOUT TEXT -->
        <div class="about-section">
            <p>
                לקוחות יקרים, כולנו מכירים את התסכול: רוצים לצאת לארוחה טובה עם המשפחה או החברים,
                אבל קשה למצוא מסעדה שמתאימה בדיוק לטעם שלכם, לאזור המגורים, לדרישות הכשרות
                או למגבלות התזונתיות. בדרך כלל מסתמכים על המלצה אקראית של מישהו, ולפעמים מתאכזבים מאוד מהתוצאה.
            </p>
            <p>
                גם אחרי שמצאתם מסעדה מתאימה, מתחילים הסיבוכים: צריך להתקשר, לבדוק זמינות,
                לוודא שיש שולחן בגודל הנכון למספר הסועדים, ולפעמים פשוט אין מקום בשעה שרציתם.
            </p>
            <p>
                לכן, בניתי מערכת מידע שמאפשרת לכם לסנן מסעדות לפי האזור, סוג האוכל, כשרות, תחליפי בשר
                והעדפות תזונתיות אישיות. לאחר בחירת המסעדה האידיאלית עבורכם, תוכלו להזמין מקום בקלות ובמהירות,
                לבחור תאריך ושעה, ולקבל אישור מיידי על ההזמנה.
            </p>
        </div>

        <!-- NAV BUTTONS -->
        <div class="nav-bar">
            <a href="Catalog.aspx" class="nav-btn nav-btn-gold">✦ להצגת מסעדות</a>
            <asp:HyperLink ID="LnkRegister" runat="server" NavigateUrl="Insert.aspx" CssClass="nav-btn nav-btn-outline">
                הרשמה לאתר
            </asp:HyperLink>
            <asp:HyperLink ID="LnkLogin" runat="server" NavigateUrl="Login.aspx" CssClass="nav-btn nav-btn-outline">
                יש לכם חשבון? התחברו
            </asp:HyperLink>
            <asp:LinkButton ID="BtnLogout" runat="server" OnClick="BtnLogout_Click" CssClass="nav-btn nav-btn-danger">
                התנתק
            </asp:LinkButton>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>

    <script type="text/javascript">
        (function () {
            var hour = new Date().getHours();
            var greeting;
            if (hour >= 5 && hour < 12) {
                greeting = "בוקר טוב,";
            } else if (hour >= 12 && hour < 17) {
                greeting = "צהריים טובים,";
            } else {
                greeting = "ערב טוב,";
            }
            document.getElementById("greetingText").textContent = greeting + " ";
        })();
    </script>
</body>
</html>
