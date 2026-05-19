<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="ArielProject.HomePage" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - מערכת הזמנות מסעדות</title>
    <%-- כל העיצוב מרוכז בקובץ Site.css --%>
    <link href="Site.css" rel="stylesheet" />
</head>
<body class="theme-dark">
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
            <asp:HyperLink ID="LnkPersonalArea" runat="server" NavigateUrl="PersonalArea.aspx" CssClass="nav-btn nav-btn-gold">
                ✦ אזור אישי
            </asp:HyperLink>
            <asp:HyperLink ID="LnkRestaurantAdmin" runat="server" NavigateUrl="RestaurantAdmin.aspx" CssClass="nav-btn nav-btn-gold">
                👑 דף מנהל מסעדה
            </asp:HyperLink>
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
