<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="ArielProject.Catalog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - קטלוג מסעדות</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            direction: rtl;
            min-height: 100vh;
            background:
                radial-gradient(ellipse at 20% 20%, rgba(120,60,200,0.25) 0%, transparent 50%),
                radial-gradient(ellipse at 80% 80%, rgba(180,100,30,0.2) 0%, transparent 50%),
                linear-gradient(135deg, #0d0d2b 0%, #1a0a2e 30%, #0d1f0d 60%, #1a1205 100%);
            background-attachment: fixed;
            color: #f0e8d0;
        }

        /* --- HEADER --- */
        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 18px 40px;
            background: rgba(212,175,55,0.06);
            border-bottom: 1px solid rgba(212,175,55,0.3);
        }
        .logo {
            font-size: 26px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 12px rgba(212,175,55,0.45), 0 0 2px rgba(255,248,220,0.7);
            animation: shimmer 3s infinite ease-in-out;
            letter-spacing: 3px;
        }
        @keyframes shimmer {
            0%, 100% { color: #f5e27a; text-shadow: 0 0 12px rgba(212,175,55,0.45); }
            50%      { color: #fff8dc; text-shadow: 0 0 18px rgba(245,226,122,0.9); }
        }
        .header-home {
            font-size: 14px;
            color: rgba(212,175,55,0.7);
            text-decoration: none;
            border: 1px solid rgba(212,175,55,0.3);
            padding: 6px 16px;
            border-radius: 20px;
            transition: all 0.3s;
        }
        .header-home:hover {
            color: #d4af37;
            border-color: #d4af37;
            background: rgba(212,175,55,0.08);
        }

        /* --- SEARCH --- */
        h2 {
            color: #f5e27a;
            text-shadow: 0 0 10px rgba(212,175,55,0.4);
            letter-spacing: 3px;
            font-size: 28px;
            margin-bottom: 25px;
        }
        .search-area {
            text-align: center;
            margin: 30px auto 40px;
            max-width: 700px;
            padding: 25px 30px;
            background: rgba(255,255,255,0.04);
            border: 1px solid rgba(212,175,55,0.25);
            border-radius: 14px;
            color: #e8d5a3;
            line-height: 2;
        }
        .search-area select {
            background: rgba(10,10,26,0.85);
            color: #f0e8d0;
            border: 1px solid rgba(212,175,55,0.4);
            border-radius: 6px;
            padding: 6px 12px;
            font-size: 14px;
            margin-right: 6px;
            outline: none;
        }
        .search-area select:hover { border-color: #d4af37; }
        .search-area input[type=checkbox] { accent-color: #d4af37; margin: 0 8px; }
        .search-area input[type=submit] {
            margin-top: 12px;
            padding: 11px 36px;
            background: linear-gradient(135deg, #d4af37, #f5e27a, #c9954c);
            color: #1a0a2e;
            font-weight: bold;
            font-size: 15px;
            border: none;
            border-radius: 50px;
            cursor: pointer;
            letter-spacing: 1px;
            box-shadow: 0 4px 18px rgba(212,175,55,0.35);
            transition: all 0.3s;
        }
        .search-area input[type=submit]:hover {
            box-shadow: 0 6px 26px rgba(212,175,55,0.6);
            transform: translateY(-2px);
            filter: brightness(1.1);
        }

        /* --- RESTAURANT CARDS --- */
        .rest-card {
            border: 1px solid rgba(212,175,55,0.25) !important;
            border-radius: 14px !important;
            padding: 22px 18px !important;
            margin: 10px !important;
            width: 220px !important;
            text-align: center !important;
            background: linear-gradient(145deg,
                rgba(255,255,255,0.06) 0%,
                rgba(212,175,55,0.05) 50%,
                rgba(120,60,200,0.07) 100%) !important;
            box-shadow: 0 4px 14px rgba(0,0,0,0.3) !important;
            transition: transform 0.3s, box-shadow 0.3s, border-color 0.3s;
            display: inline-block;
            vertical-align: top;
        }
        .rest-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 14px 36px rgba(212,175,55,0.2) !important;
            border-color: rgba(212,175,55,0.55) !important;
        }
        .rest-card h3 {
            color: #f5e27a !important;
            font-size: 17px;
            margin: 0 0 10px;
            letter-spacing: 1px;
        }
        .rest-card p {
            font-size: 13px;
            color: rgba(240,232,208,0.75);
            margin: 4px 0;
        }
        .rest-card p b {
            color: rgba(212,175,55,0.9);
        }
        .book-btn {
            display: inline-block !important;
            margin-top: 12px !important;
            padding: 9px 18px !important;
            background: linear-gradient(135deg, #d4af37, #f5e27a, #c9954c) !important;
            color: #1a0a2e !important;
            text-decoration: none !important;
            border-radius: 50px !important;
            font-weight: bold !important;
            font-size: 13px !important;
            box-shadow: 0 3px 12px rgba(212,175,55,0.3);
            transition: all 0.3s;
        }
        .book-btn:hover {
            box-shadow: 0 5px 20px rgba(212,175,55,0.55);
            filter: brightness(1.1);
            transform: translateY(-1px);
        }
        .show-more-btn {
            display: inline-block;
            margin-top: 8px;
            padding: 7px 16px;
            background: transparent;
            color: #d4af37;
            border: 1px solid rgba(212,175,55,0.45);
            border-radius: 50px;
            font-size: 13px;
            font-family: Arial, sans-serif;
            cursor: pointer;
            transition: all 0.3s;
        }
        .show-more-btn:hover {
            background: rgba(212,175,55,0.1);
            border-color: #d4af37;
        }

        /* --- MODAL --- */
        .info-modal-overlay {
            display: none;
            position: fixed;
            top: 0; left: 0;
            width: 100%; height: 100%;
            background-color: rgba(0,0,0,0.75);
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }
        .info-modal-overlay.active { display: flex; }
        .info-modal-box {
            background: linear-gradient(145deg, #0f0f2a, #1a0d30, #0d1a0d);
            border: 1px solid rgba(212,175,55,0.4);
            border-radius: 16px;
            padding: 30px 35px;
            width: 90%; max-width: 550px;
            max-height: 85vh; overflow-y: auto;
            box-shadow: 0 20px 60px rgba(0,0,0,0.6);
            direction: rtl; text-align: right;
            position: relative;
            color: #f0e8d0;
        }
        .info-modal-close {
            position: absolute; top: 12px; left: 15px;
            background: none; border: none;
            font-size: 26px; cursor: pointer;
            color: rgba(212,175,55,0.6); font-weight: bold;
        }
        .info-modal-close:hover { color: #d4af37; }
        .info-modal-title {
            color: #f5e27a;
            margin-top: 0; margin-bottom: 18px;
            font-size: 26px;
            border-bottom: 1px solid rgba(212,175,55,0.25);
            padding-bottom: 10px;
            letter-spacing: 2px;
        }
        .info-modal-section {
            margin: 14px 0;
            line-height: 1.7;
            color: rgba(240,232,208,0.85);
            font-size: 14px;
        }
        .info-modal-section b { color: #d4af37; }
        .info-modal-dishes {
            list-style: none;
            padding-right: 0;
            margin: 8px 0;
        }
        .info-modal-dishes li {
            padding: 4px 0;
            color: rgba(240,232,208,0.8);
            font-size: 14px;
        }
        .info-modal-dishes li::before {
            content: '✦ ';
            color: rgba(212,175,55,0.6);
            font-size: 11px;
        }
        .info-modal-rating {
            background: rgba(212,175,55,0.08);
            border: 1px solid rgba(212,175,55,0.2);
            padding: 10px 14px;
            border-radius: 8px;
            margin: 12px 0;
            color: #e8d5a3;
        }
        .last-booking {
            background: rgba(180,100,0,0.18);
            padding: 9px 14px;
            border-right: 3px solid #d4af37;
            border-radius: 0 6px 6px 0;
            margin-top: 10px;
            font-weight: bold;
            color: #f5e27a;
        }
        .info-modal-map {
            margin-top: 10px;
            border: 1px solid rgba(212,175,55,0.3);
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 14px rgba(0,0,0,0.35);
        }
        .info-modal-map iframe {
            display: block;
            width: 100%;
            height: 220px;
            border: 0;
        }
        .info-modal-book-btn {
            display: block; width: 100%; text-align: center;
            margin-top: 22px; padding: 13px 20px;
            background: linear-gradient(135deg, #d4af37, #f5e27a, #c9954c);
            color: #1a0a2e !important;
            text-decoration: none; border-radius: 50px;
            font-weight: bold; font-size: 15px; box-sizing: border-box;
            box-shadow: 0 4px 18px rgba(212,175,55,0.35);
            letter-spacing: 1px;
            transition: all 0.3s;
        }
        .info-modal-book-btn:hover {
            box-shadow: 0 6px 28px rgba(212,175,55,0.6);
            filter: brightness(1.1);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="page-header">
            <div class="logo">✦ EatIt ✦</div>
            <a href="HomePage.aspx" class="header-home">← דף הבית</a>
        </div>

<div dir="rtl" style="text-align: center; margin-top: 30px;">
    <h2>חיפוש מסעדות</h2>

    <div class="search-area">
    בחר אזור:
    <asp:DropDownList ID="DdlRegion" runat="server">
        <asp:ListItem Text="הכל" Value="הכל"></asp:ListItem>
        <asp:ListItem Text="צפון" Value="Tzafon"></asp:ListItem>
        <asp:ListItem Text="מרכז" Value="Merkaz"></asp:ListItem>
        <asp:ListItem Text="דרום" Value="Darom"></asp:ListItem>
    </asp:DropDownList>
    <br /><br />

    סוג מטבח:
    <asp:DropDownList ID="DdlType" runat="server">
        <asp:ListItem Text="הכל" Value="הכל"></asp:ListItem>
        <asp:ListItem Text="איטלקי" Value="Italian"></asp:ListItem>
        <asp:ListItem Text="בשרי" Value="Meat"></asp:ListItem>
        <asp:ListItem Text="אסייתי" Value="Asian"></asp:ListItem>
        <asp:ListItem Text="בראסרי" Value="Brasserie"></asp:ListItem>
        <asp:ListItem Text="קינוחים" Value="Deserts"></asp:ListItem>
    </asp:DropDownList>
    <br /><br />
    <asp:CheckBox ID="ChkKosher" runat="server" Text="הצג מסעדות כשרות בלבד" />
    <asp:CheckBox ID="ChkReplacementMeals" runat="server" Text="תחלופה לאלרגנים בלבד" /><br />
    <asp:Button ID="BtnSearch" runat="server" Text="חפש מסעדות" OnClick="BtnSearch_Click" />
    </div>
    <br /><br />

<asp:DataList ID="DataListRestaurants" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
    <ItemTemplate>
        <div class="rest-card">
            <h3><%# Eval("Restaurants") %></h3>
            <p><b>אזור:</b> <%# Eval("Region") %></p>
            <p><b>סוג מטבח:</b> <%# Eval("FoodType") %></p>
            <p><b>כשרות:</b> <%# Eval("Kosher") %></p>
            <p><b>תחלופה לאלרגנים:</b> <%# Eval("ReplacementMeals") %></p>
            <a href='Booking.aspx?res=<%# Eval("Restaurants") %>' class="book-btn">הזמן שולחן</a>
            <br />
            <button type="button" class="show-more-btn" data-restaurant='<%# Eval("Restaurants") %>' onclick="showRestaurantInfo(this.getAttribute('data-restaurant'))">הצג עוד</button>
        </div>
    </ItemTemplate>
</asp:DataList>
    <br />
</div>

<!-- Modal window -->
<div id="restaurantInfoModal" class="info-modal-overlay" onclick="overlayClick(event)">
    <div class="info-modal-box">
        <button type="button" class="info-modal-close" onclick="closeRestaurantInfo()">&times;</button>
        <h2 class="info-modal-title" id="modalTitle"></h2>
        <div class="info-modal-section" id="modalAbout"></div>
        <div class="info-modal-section">📍 <b>כתובת:</b> <span id="modalAddress"></span></div>
        <div class="info-modal-map" id="modalMapContainer" style="display:none;">
            <iframe id="modalMapFrame" src="" loading="lazy" allowfullscreen referrerpolicy="no-referrer-when-downgrade"></iframe>
        </div>
        <div class="info-modal-section">
            ⏰ <b>שעות פעילות:</b><br />
            <span id="modalHours"></span>
            <div class="last-booking">🪑 ניתן להזמין מקום עד שעה <span id="modalLastBooking"></span></div>
        </div>
        <div class="info-modal-section">
            🍽️ <b>מנות מובחרות:</b>
            <ul class="info-modal-dishes" id="modalDishes"></ul>
        </div>
        <div class="info-modal-rating" id="modalRating"></div>
        <a id="modalBookBtn" class="info-modal-book-btn" href="#">הזמן שולחן במסעדה</a>
    </div>
</div>

<!-- Hidden restaurant data (HTML, not JS strings) -->
<div id="rest-data" style="display:none">

    <div id="rd-Bobo">
        <span class="ri-about">מסעדה איטלקית כשרה (בד"ץ בית יוסף) חדישה וצבעונית, מעוצבת בסגנון מודרני וחם. המטבח מתמחה בפסטות טריות בעבודת יד, דגים טריים, ופיצות נפוליטניות אפויות בתנור חם עם בצק אוורי וטופינגים יצירתיים.</span>
        <span class="ri-addr">רחוב חיים ויצמן 58, חולון</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:30<br />שישי: 12:00 - 15:00<br />שבת: 20:00 - 23:30</div>
        <span class="ri-last">21:30</span>
        <ul class="ri-dishes">
            <li>פסטות טריות בעבודת יד</li>
            <li>פיצות נפוליטניות בתנור חם</li>
            <li>דגים טריים על הפלאנצ'ה</li>
            <li>אנטיפסטי איטלקי</li>
            <li>קינוחים איטלקיים קלאסיים</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 5/5 | שירות 5/5 | אווירה 5/5</span>
    </div>

    <div id="rd-La-Lush">
        <span class="ri-about">בראסרי אירופאי עם נגיעות צרפתיות-איטלקיות. אווירה מיוחדת ומזמינה לארוחה ולישיבה בכל שעות היום. תפריט עשיר עם חומרי גלם איכותיים, תנור טאבון מרכזי ובר יינות איכותי.</span>
        <span class="ri-addr">רחוב סוקולוב 80, בת ים</span>
        <div class="ri-hours">ראשון-חמישי: 09:00 - 00:00<br />שישי: 08:00 - 16:00<br />שבת: 19:00 - 00:00</div>
        <span class="ri-last">22:00</span>
        <ul class="ri-dishes">
            <li>ארוחות בוקר בראסרי עשירות</li>
            <li>פסטות איטלקיות טריות</li>
            <li>דגים ופירות ים</li>
            <li>סלטים אירופאיים</li>
            <li>קינוחי בית בעבודת יד</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4.5/5 | אווירה 5/5</span>
    </div>

    <div id="rd-Moses">
        <span class="ri-about">מסעדת המבורגרים פרימיום שבה כל הבשר נטחן טרי מדי יום במקום. מסעדת בשרים אמריקאית קלאסית עם אווירה משפחתית, המבורגרים עסיסיים, ומנות בשריות עשירות.</span>
        <span class="ri-addr">רחוב הברזל 30, רמת החייל, תל אביב</span>
        <div class="ri-hours">ראשון-רביעי: 12:00 - 00:00<br />חמישי: 12:00 - 01:00<br />שישי-שבת: 12:00 - 01:00</div>
        <span class="ri-last">22:00</span>
        <ul class="ri-dishes">
            <li>המבורגר בקר 220 גרם טחון טרי</li>
            <li>המבורגר Moses Classic</li>
            <li>כנפיים פיקנטיות</li>
            <li>צ'יפס בלגי עם רטבי בית</li>
            <li>מילקשייקים סמיכים</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4/5 | אווירה 4.5/5</span>
    </div>

    <div id="rd-Japanika">
        <span class="ri-about">רשת מסעדות אסייתית בעלת סניפים רבים בחיפה ובצפון. מתמחה בסושי איכותי, מנות יפניות ותאילנדיות, וקעריות מוקפצות. אווירה תוססת ומחירים נגישים.</span>
        <span class="ri-addr">דרך ישראל בר יהודה 111, חיפה</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:00<br />שישי: 12:00 - 15:00<br />שבת: 19:30 - 23:00</div>
        <span class="ri-last">21:00</span>
        <ul class="ri-dishes">
            <li>מגשי סושי מגוונים</li>
            <li>אורז מוקפץ עם ירקות וחלבון</li>
            <li>אטריות אודון בווק</li>
            <li>טמפורה ירקות ושרימפס</li>
            <li>מיסו סופ קלאסי</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4/5 | שירות 4/5 | אווירה 4/5</span>
    </div>

    <div id="rd-Kagas">
        <span class="ri-about">בראסרי מודרני בלב באר שבע עם אווירה אירופאית חמה. מטבח עשיר המשלב מנות בית קלאסיות עם טוויסט מקומי. מקום מצוין לארוחות עסקיות, דייטים, ומפגשי משפחה.</span>
        <span class="ri-addr">רחוב הנשיאים 4, באר שבע</span>
        <div class="ri-hours">ראשון-חמישי: 10:00 - 23:00<br />שישי: 09:00 - 16:00<br />מוצ"ש: 20:00 - 23:30</div>
        <span class="ri-last">21:00</span>
        <ul class="ri-dishes">
            <li>סטייק אנטריקוט עם תוספות</li>
            <li>פסטה ביתית טרייה</li>
            <li>סלטי בראסרי עשירים</li>
            <li>דגים על הפלאנצ'ה</li>
            <li>קינוחי שוקולד וקרם</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4/5 | אווירה 4.5/5</span>
    </div>

    <div id="rd-Vivino">
        <span class="ri-about">שגרירות הקולינריה האיטלקית של חיפה. מסעדה משתרעת על 850 מ"ר עם מעדנייה איטלקית סמוכה ואולם אירועים. הכל נעשה במקום - מהבצק ועד הגלידה. תפריט עשיר המבוסס על חומרי גלם איטלקיים מקוריים.</span>
        <span class="ri-addr">כיכר אליזבט 1, הרובע האיטלקי, חיפה</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:30<br />שישי: 12:00 - 16:00<br />מוצ"ש: 19:00 - 23:30</div>
        <span class="ri-last">21:30</span>
        <ul class="ri-dishes">
            <li>פסטות טריות בעבודת יד</li>
            <li>פיצות בתנור עצים</li>
            <li>דגים ופירות ים</li>
            <li>אנטיפסטי איטלקי</li>
            <li>טירמיסו וקינוחים איטלקיים</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 5/5 | שירות 4.5/5 | אווירה 5/5</span>
    </div>

    <div id="rd-Mahne-Yuda">
        <span class="ri-about">מסעדת השף האייקונית של אסף גרניט ואורי נבון בלב שוק מחנה יהודה בירושלים. שילוב ייחודי של בישול ישראלי מודרני עם דגש על חומרי גלם משובחים מהשוק. אווירה תוססת ובלתי נשכחת.</span>
        <span class="ri-addr">רחוב בית יעקב 10, שוק מחנה יהודה, ירושלים</span>
        <div class="ri-hours">ראשון-חמישי: 18:30 - 01:30<br />שישי: 12:00 - 15:30<br />שבת: סגור</div>
        <span class="ri-last">23:30</span>
        <ul class="ri-dishes">
            <li>פולנטה Mahane Yuda המפורסמת</li>
            <li>כבד אווז עם פירות</li>
            <li>דגים טריים מהיום</li>
            <li>אסאדו עם תפוחי אדמה צלויים</li>
            <li>קינוחי שוק יצירתיים</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 5/5 | שירות 5/5 | אווירה 5/5</span>
    </div>

    <div id="rd-Oshi-Oshi">
        <span class="ri-about">רשת הסושי הגדולה והמוצלחת בישראל. הסניף בבאר שבע כשר על ידי הרבנות הראשית ומציע סושי באיכות גבוהה, חומרי גלם טריים, וניקיון מוקפד. אווירה צעירה ושירות מקצועי.</span>
        <span class="ri-addr">רחוב יצחק רגר 2, קניון הנגב, באר שבע</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:00<br />שישי: 11:30 - 15:00<br />מוצ"ש: 20:00 - 23:00</div>
        <span class="ri-last">21:00</span>
        <ul class="ri-dishes">
            <li>מגשי סושי משובחים</li>
            <li>מנות פוקה (Poke Bowls)</li>
            <li>סשימי טרי</li>
            <li>מאקי רולים מיוחדים</li>
            <li>אדממה ומיסו סופ</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4.5/5 | אווירה 4/5</span>
    </div>

    <div id="rd-Biga">
        <span class="ri-about">פסטה בר איטלקי קטן וחמים בבאר שבע, המתמחה בפסטות ופיצות שנעשות במקום מחומרי גלם טריים. אווירה אינטימית ומחירים סבירים. אהוב במיוחד על תושבי העיר.</span>
        <span class="ri-addr">רחוב התעשייה 51, באר שבע</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:00<br />שישי: 12:00 - 15:00<br />שבת: 19:00 - 23:00</div>
        <span class="ri-last">21:00</span>
        <ul class="ri-dishes">
            <li>פיצה עם זיתים וגבינת עיזים</li>
            <li>פסטה ברוטב שמנת וערמונים</li>
            <li>פסטה ברוטב רוזה</li>
            <li>סלט קפרזה</li>
            <li>ברוסקטות ביתיות</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4.5/5 | אווירה 4/5</span>
    </div>

    <div id="rd-Nafis">
        <span class="ri-about">מסעדה ים תיכונית עם תפריט עשיר ומגוון, המשלב טעמים ישראליים, איטלקיים, אמריקאיים ומזרחיים תחת קורת גג אחת. אווירה מזמינה לכל המשפחה.</span>
        <span class="ri-addr">רחוב סוקולוב 31, רעננה</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:30<br />שישי: 12:00 - 16:00<br />מוצ"ש: 19:30 - 23:30</div>
        <span class="ri-last">21:30</span>
        <ul class="ri-dishes">
            <li>מבחר סלטים ים תיכוניים</li>
            <li>סטייק אנטריקוט</li>
            <li>פסטה ופיצה איטלקית</li>
            <li>מנות אסייתיות (סושי ומוקפצים)</li>
            <li>קינוחי בית</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4/5 | שירות 4/5 | אווירה 4.5/5</span>
    </div>

    <div id="rd-Zink">
        <span class="ri-about">מסעדה איטלקית-אמריקאית מאז 2003, ידועה במנות הבשר המיושנות והפסטות הטריות. תנור אבן מיוחד לדגים ובר חי תוסס. אטרקציה קולינרית במחוז המרכז.</span>
        <span class="ri-addr">רחוב עתיר ידע 6, יהוד-מונוסון</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:30<br />שישי: 12:00 - 15:30<br />מוצ"ש: 19:00 - 00:00</div>
        <span class="ri-last">21:30</span>
        <ul class="ri-dishes">
            <li>סטייק פילה במלפלפלים חריפים</li>
            <li>שרימפס ברוטב חמאת עגבניות</li>
            <li>שניצל עגל מנתח Cinta</li>
            <li>המבורגר בית טחון טרי</li>
            <li>פסטות בעבודת יד</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 5/5 | שירות 4.5/5 | אווירה 5/5</span>
    </div>

    <div id="rd-Max-Brener">
        <span class="ri-about">בר שוקולד וקינוחים בינלאומי. חוויה קולינרית מתוקה עם ריח שוקולד בלתי נשכח. אטרקציה גדולה למשפחות, זוגות, ולחובבי שוקולד אמיתיים. בעלת עשרות סניפים בעולם.</span>
        <span class="ri-addr">מתחם הקסטרא, רחוב פלימן 8, חיפה</span>
        <div class="ri-hours">ראשון-חמישי: 09:00 - 23:00<br />שישי: 09:00 - 16:00<br />מוצ"ש: 20:00 - 23:00</div>
        <span class="ri-last">21:00</span>
        <ul class="ri-dishes">
            <li>סופלה שוקולד פיור</li>
            <li>וופלים בלגיים עם תוספות שוקולד</li>
            <li>קרפים מתוקים</li>
            <li>שייק שוקולד עשיר</li>
            <li>שוקו חם בכוס פינוקים</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4/5 | אווירה 5/5</span>
    </div>

    <div id="rd-Segev">
        <span class="ri-about">מסעדת השף של משה שגב בבאר שבע - מטבח טרי וססגוני המשלב טעמים ישראליים מודרניים עם השפעות איטלקיות. השף שגב מציע חוויה קולינרית מעודנת בלב הדרום.</span>
        <span class="ri-addr">רחוב ברוך קטינקא 2, מתחם ישפרו, באר שבע</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:00<br />שישי: 12:00 - 15:00<br />מוצ"ש: 19:30 - 23:30</div>
        <span class="ri-last">21:00</span>
        <ul class="ri-dishes">
            <li>פסטה ראביולי ביתי</li>
            <li>סלטים ים תיכוניים</li>
            <li>המבורגר שגב מיוחד</li>
            <li>מנות שף משתנות לפי עונה</li>
            <li>קינוחי שוקולד אומנותיים</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4.5/5 | אווירה 4.5/5</span>
    </div>

    <div id="rd-Black">
        <span class="ri-about">מסעדת המבורגרים פרימיום שמשלבת בשרים איכותיים עם חוויית בראסרי מודרנית. תפריט עשיר הכולל גם אופציות צמחוניות וטבעוניות. אווירה אורבנית ושירות אדיב.</span>
        <span class="ri-addr">קניון עזריאלי, רחוב מנחם בגין 132, תל אביב</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:30<br />שישי: 12:00 - 16:00<br />מוצ"ש: 20:00 - 23:30</div>
        <span class="ri-last">21:30</span>
        <ul class="ri-dishes">
            <li>המבורגר Black Signature</li>
            <li>סטייק אנטריקוט מיושן</li>
            <li>כריכי בשר עשירים</li>
            <li>סלט סיזר עם עוף</li>
            <li>קינוחי בית</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4/5 | אווירה 4.5/5</span>
    </div>

    <div id="rd-Kansai">
        <span class="ri-about">מסעדת סושי כשרה (בהשגחת הרבנות הראשית תל אביב) המביאה את יפן המודרנית. השף טל אלמלח יצר תפריט יפני חדשני המשלב טכניקות מתקדמות עם חומרי גלם טריים, סושי מוקפד וקעריות צ'יראשי.</span>
        <span class="ri-addr">רחוב יגאל אלון 94, תל אביב</span>
        <div class="ri-hours">ראשון-חמישי: 12:00 - 23:00<br />שישי: 11:30 - 14:30<br />מוצ"ש: 20:30 - 23:00</div>
        <span class="ri-last">21:00</span>
        <ul class="ri-dishes">
            <li>מגשי סושי שף</li>
            <li>צ'יראשי טרי</li>
            <li>מוקפצי ווק</li>
            <li>וג'י רול טבעוני</li>
            <li>טמפורה קלילה</li>
        </ul>
        <span class="ri-rating">⭐ דירוג: אוכל 4.5/5 | שירות 4.5/5 | אווירה 4/5</span>
    </div>

</div>
<!-- End hidden restaurant data -->

<script type="text/javascript">
    function showRestaurantInfo(name) {
        var id = "rd-" + name.replace(/ /g, "-");
        var d = document.getElementById(id);
        if (!d) { return; }

        document.getElementById("modalTitle").innerText = name;
        document.getElementById("modalAbout").innerText = d.querySelector(".ri-about").innerText;
        document.getElementById("modalAddress").innerText = d.querySelector(".ri-addr").innerText;
        document.getElementById("modalHours").innerHTML = d.querySelector(".ri-hours").innerHTML;
        document.getElementById("modalLastBooking").innerText = d.querySelector(".ri-last").innerText;
        document.getElementById("modalDishes").innerHTML = d.querySelector(".ri-dishes").innerHTML;
        document.getElementById("modalRating").innerText = d.querySelector(".ri-rating").innerText;
        document.getElementById("modalBookBtn").href = "Booking.aspx?res=" + encodeURIComponent(name);

        var address = d.querySelector(".ri-addr").innerText.trim();
        var mapContainer = document.getElementById("modalMapContainer");
        var mapFrame = document.getElementById("modalMapFrame");
        if (address !== "") {
            mapFrame.src = "https://maps.google.com/maps?q=" + encodeURIComponent(address) + "&t=m&z=16&output=embed&iwloc=near";
            mapContainer.style.display = "block";
        } else {
            mapFrame.src = "";
            mapContainer.style.display = "none";
        }

        document.getElementById("restaurantInfoModal").classList.add("active");
    }

    function closeRestaurantInfo() {
        document.getElementById("restaurantInfoModal").classList.remove("active");
    }

    function overlayClick(event) {
        if (event.target.id === "restaurantInfoModal") {
            closeRestaurantInfo();
        }
    }

    document.addEventListener("keydown", function(e) {
        if (e.key === "Escape") { closeRestaurantInfo(); }
    });
</script>

    </form>
</body>
</html>
