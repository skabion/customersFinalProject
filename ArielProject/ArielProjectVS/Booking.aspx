<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="ArielProject.Booking"%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - הזמנת מקום</title>
    <%-- כל העיצוב מרוכז בקובץ Site.css --%>
    <link href="Site.css" rel="stylesheet" />
</head>
<body class="theme-dark" style="padding-top: 52px;">

    <!-- Countdown Timer Bar -->
    <div class="timer-bar" id="timerBar">
        ⏱ זמן להזמנה: <span id="timerDisplay">03:00</span>
    </div>

    <!-- Expired Overlay -->
    <div class="timer-expired-overlay" id="timerExpiredOverlay">
        <div class="timer-expired-box">
            <h2>הזמן הסתיים ⏰</h2>
            <p>חלפו 3 דקות ממועד תחילת ההזמנה.<br />אנא חזור לקטלוג ובחר שוב.</p>
            <a href="Catalog.aspx">חזור לקטלוג</a>
        </div>
    </div>

    <script type="text/javascript">


        var bookingTimerInterval = null;
        var bookingStorageKey = "bookingTimerStart";

        // פונקציה שמופעלת מקוד C# (LblClearTimer) כשהזמנה הצליחה -
        // עוצרת את הטיימר ומסתירה את הסרגל
        function clearBookingTimer() {
            if (bookingTimerInterval) { clearInterval(bookingTimerInterval); }
            sessionStorage.removeItem(bookingStorageKey);
            var bar = document.getElementById("timerBar");
            if (bar) { bar.style.display = "none"; }
        }

        // משך הטיימר בשניות - 3 דקות = 180 שניות
        var DURATION = 3 * 60;

        // קוראים את זמן ההתחלה מ-sessionStorage. אם זו הפעם הראשונה - שומרים
        // את הזמן הנוכחי. אחרת ממירים את הערך השמור למספר.
        var startTime = sessionStorage.getItem(bookingStorageKey);
        if (!startTime) {
            startTime = Date.now();
            sessionStorage.setItem(bookingStorageKey, startTime);
        } else {
            startTime = parseInt(startTime);
        }

        // מחשבת כמה שניות נותרו (DURATION פחות הזמן שעבר)
        function getRemaining() {
            var elapsed = Math.floor((Date.now() - startTime) / 1000);
            var remaining = DURATION - elapsed;
            if (remaining < 0) remaining = 0;
            return remaining;
        }

        // ממירה מספר שניות לפורמט MM:SS עם אפס לפני אם צריך
        function formatTime(secs) {
            var m = Math.floor(secs / 60);
            var s = secs % 60;

            var mStr;
            if (m < 10) mStr = "0" + m;
            else mStr = "" + m;

            var sStr;
            if (s < 10) sStr = "0" + s;
            else sStr = "" + s;

            return mStr + ":" + sStr;
        }

        // הפונקציה הראשית של הטיימר - מתעדכנת כל שנייה
        function updateDisplay() {
            var remaining = getRemaining();
            document.getElementById("timerDisplay").textContent = formatTime(remaining);

            // אם הזמן נגמר - מציגים את חלון "הזמן הסתיים"
            if (remaining === 0) {
                clearInterval(bookingTimerInterval);
                sessionStorage.removeItem(bookingStorageKey);
                document.getElementById("timerExpiredOverlay").className = "timer-expired-overlay active";
            }
        }

        // מפעילים את הטיימר ומעדכנים את התצוגה כל שנייה (1000 מילישניות)
        updateDisplay();
        bookingTimerInterval = setInterval(updateDisplay, 1000);
    </script>

    <form id="form1" runat="server">

        <div class="header">
            <div class="logo">✦ EatIt ✦</div>
            <div class="greeting-area">
                שלום, <asp:Label ID="LblUserName" runat="server"></asp:Label>
            </div>
        </div>

        <div class="hero">
            <span class="hero-icon">🍽️</span>
            <h1>הזמנת מקום</h1>
            <div class="hero-subtitle">✦ &nbsp; בחר תאריך, שעה ומספר סועדים &nbsp; ✦</div>
        </div>

        <div class="gold-divider"></div>

        <div class="container">
            <div class="res-name">
                🍽️ <asp:Label ID="LblResName" runat="server" Text="שם המסעדה"></asp:Label>
            </div>

            <%-- שונה: input-group → form-input-group, input-control → booking-input --%>
            <div class="form-input-group">
                <label>תאריך:</label>
                <asp:TextBox ID="TxtDate" runat="server" CssClass="booking-input" TextMode="Date"></asp:TextBox>
            </div>

            <div class="form-input-group">
                <label>מספר סועדים:</label>
                <asp:TextBox ID="TxtGuests" runat="server" CssClass="booking-input" TextMode="Number"></asp:TextBox>
            </div>

            <asp:Button ID="BtnCheckTimes" runat="server" Text="🔍 מצאו לי שולחן"
                OnClick="BtnCheckTimes_Click" CssClass="btn-find-table" />

            <br /><br />

            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="True"
                AutoGenerateSelectButton="True"
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                Width="100%"
                CssClass="times-grid">
            </asp:GridView>

            <%-- פקד Label שמשמש להזרקת קוד JavaScript לעצירת הטיימר.
                 השתמשנו ב-Label במקום ב-Literal כי Label נלמד בכיתה. --%>
            <asp:Label ID="LblClearTimer" runat="server"></asp:Label>

            <br />
            <asp:Label ID="LblMsg" runat="server" Font-Bold="True"></asp:Label>

            <asp:Panel ID="TaxiPanel" runat="server" Visible="false" CssClass="taxi-panel">
                <asp:Label ID="LblTaxiQuestion" runat="server" CssClass="taxi-question"></asp:Label>

                <asp:Button ID="BtnTaxiYes" runat="server" Text="כן, הזמינו לי הסעה"
                    OnClick="BtnTaxiYes_Click" CssClass="btn-success" />
                <asp:Button ID="BtnTaxiNo" runat="server" Text="לא תודה"
                    OnClick="BtnTaxiNo_Click" CssClass="btn-secondary" />

                <asp:Panel ID="AddressPanel" runat="server" Visible="false" CssClass="address-panel">
                    <div class="address-title">📍 כתובת איסוף לנהג</div>

                    <div class="form-input-group">
                        <label>עיר:</label>
                        <asp:TextBox ID="TxtCity" runat="server" CssClass="booking-input" placeholder="לדוגמה: תל אביב - יפו"></asp:TextBox>
                    </div>

                    <div class="form-input-group">
                        <label>רחוב:</label>
                        <asp:TextBox ID="TxtStreet" runat="server" CssClass="booking-input" placeholder="לדוגמה: דיזנגוף"></asp:TextBox>
                    </div>

                    <div class="form-input-group">
                        <label>מספר בית:</label>
                        <asp:TextBox ID="TxtHouseNum" runat="server" CssClass="booking-input" TextMode="Number" placeholder="לדוגמה: 12"></asp:TextBox>
                    </div>

                    <asp:Button ID="BtnConfirmAddress" runat="server" Text="✓ אישור כתובת והזמנה"
                        OnClick="BtnConfirmAddress_Click" CssClass="btn-success-full" />

                    <br /><br />
                    <asp:Label ID="LblAddressError" runat="server" ForeColor="#ff8888" Font-Bold="True"></asp:Label>
                </asp:Panel>

                <asp:Label ID="LblTaxiResult" runat="server" CssClass="taxi-result"></asp:Label>
            </asp:Panel>
        </div>

        <div class="nav-bar">
            <a href="Catalog.aspx" class="nav-btn">← חזרה לקטלוג</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
