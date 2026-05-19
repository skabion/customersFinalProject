<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="ArielProject.Booking"%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>הזמנת מקום</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            direction: rtl;
            text-align: right;
            padding: 20px;
        }

        .booking-container {
            max-width: 400px;
            margin: 0 auto;
            border: 1px solid #ddd;
            padding: 20px;
            border-radius: 8px;
        }

        .input-group {
            margin-bottom: 15px;
        }

        .input-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .input-control {
            width: 100%;
            padding: 8px;
            box-sizing: border-box;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        /* עיצוב לטבלת השעות */
        .times-grid {
            border-collapse: collapse;
            margin-top: 10px;
        }

        .times-grid th, .times-grid td {
            border: 1px solid #ccc;
            padding: 8px 12px;
            text-align: center;
        }

        .times-grid th {
            background-color: #2c3e50;
            color: white;
        }

        .res-name {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
            text-align: center;
        }

        .timer-bar {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            background: #e74c3c;
            color: white;
            text-align: center;
            padding: 10px 20px;
            font-size: 18px;
            font-weight: bold;
            z-index: 9999;
            direction: rtl;
            box-shadow: 0 2px 6px rgba(0,0,0,0.3);
            transition: background 0.5s;
        }

        .timer-bar.warning {
            background: #e67e22;
        }

        .timer-bar.urgent {
            background: #c0392b;
            animation: pulse 0.8s infinite alternate;
        }

        @keyframes pulse {
            from { opacity: 1; }
            to   { opacity: 0.7; }
        }

        .timer-expired-overlay {
            display: none;
            position: fixed;
            top: 0; left: 0; right: 0; bottom: 0;
            background: rgba(0,0,0,0.75);
            z-index: 10000;
            justify-content: center;
            align-items: center;
        }

        .timer-expired-overlay.active {
            display: flex;
        }

        .timer-expired-box {
            background: white;
            border-radius: 12px;
            padding: 40px 30px;
            text-align: center;
            direction: rtl;
            max-width: 340px;
            width: 90%;
        }

        .timer-expired-box h2 {
            color: #c0392b;
            font-size: 26px;
            margin-bottom: 15px;
        }

        .timer-expired-box p {
            color: #555;
            font-size: 16px;
            margin-bottom: 25px;
        }

        .timer-expired-box a {
            display: inline-block;
            background: #2c3e50;
            color: white;
            padding: 12px 30px;
            border-radius: 6px;
            text-decoration: none;
            font-weight: bold;
            font-size: 15px;
        }
    </style>
</head>
<body style="padding-top: 52px;">

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
        var _bookingTimerInterval = null;
        var _bookingStorageKey = "bookingTimerStart";

        function clearBookingTimer() {
            if (_bookingTimerInterval) { clearInterval(_bookingTimerInterval); }
            sessionStorage.removeItem(_bookingStorageKey);
            var bar = document.getElementById("timerBar");
            if (bar) { bar.style.display = "none"; }
        }

        (function () {
            var DURATION = 3 * 60;

            var startTime = sessionStorage.getItem(_bookingStorageKey);
            if (!startTime) {
                startTime = Date.now();
                sessionStorage.setItem(_bookingStorageKey, startTime);
            } else {
                startTime = parseInt(startTime, 10);
            }

            function getRemaining() {
                var elapsed = Math.floor((Date.now() - startTime) / 1000);
                return Math.max(0, DURATION - elapsed);
            }

            function formatTime(secs) {
                var m = Math.floor(secs / 60);
                var s = secs % 60;
                return (m < 10 ? "0" : "") + m + ":" + (s < 10 ? "0" : "") + s;
            }

            function updateDisplay() {
                var remaining = getRemaining();
                document.getElementById("timerDisplay").textContent = formatTime(remaining);

                var bar = document.getElementById("timerBar");
                bar.className = "timer-bar";
                if (remaining <= 60) {
                    bar.className = "timer-bar urgent";
                } else if (remaining <= 90) {
                    bar.className = "timer-bar warning";
                }

                if (remaining === 0) {
                    clearInterval(_bookingTimerInterval);
                    sessionStorage.removeItem(_bookingStorageKey);
                    document.getElementById("timerExpiredOverlay").className = "timer-expired-overlay active";
                }
            }

            updateDisplay();
            _bookingTimerInterval = setInterval(updateDisplay, 1000);
        })();
    </script>

    <form id="form1" runat="server">
        <div class="booking-container">
            <div class="res-name">
                <asp:Label ID="LblResName" runat="server" Text="שם המסעדה"></asp:Label>
            </div>

            <%-- הסרנו את ה-CompareValidator כי הגדרת ערך ההשוואה הייתה
                 צריכה להיעשות מקוד C# (דבר שלא נלמד תמיד). במקום זאת
                 הבדיקה שהתאריך עתידי מתבצעת בקוד פונקציית BtnCheckTimes_Click. --%>
            <div class="input-group">
                <label>תאריך:</label>
                <asp:TextBox ID="TxtDate" runat="server" CssClass="input-control" TextMode="Date"></asp:TextBox>
            </div>

            <div class="input-group">
                <label>מספר סועדים:</label>
                <asp:TextBox ID="TxtGuests" runat="server" CssClass="input-control" TextMode="Number"></asp:TextBox>
            </div>

            <asp:Button ID="BtnCheckTimes" runat="server" Text="מצאו לי שולחן" 
                OnClick="BtnCheckTimes_Click" BackColor="Black" ForeColor="White" 
                Width="100%" Height="40px" Font-Bold="True" />

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

            <asp:Panel ID="TaxiPanel" runat="server" Visible="false"
                style="margin-top:20px; padding:15px; border:1px solid #2c3e50; border-radius:8px; background:#f9f9f9; text-align:center;">
                <asp:Label ID="LblTaxiQuestion" runat="server" Font-Bold="True" Font-Size="16px" ForeColor="#2c3e50"></asp:Label>
                <br /><br />
                <asp:Button ID="BtnTaxiYes" runat="server" Text="כן, הזמינו לי הסעה"
                    OnClick="BtnTaxiYes_Click"
                    BackColor="#27ae60" ForeColor="White" Width="48%" Height="40px" Font-Bold="True" />
                <asp:Button ID="BtnTaxiNo" runat="server" Text="לא תודה"
                    OnClick="BtnTaxiNo_Click"
                    BackColor="#7f8c8d" ForeColor="White" Width="48%" Height="40px" Font-Bold="True" />

                <asp:Panel ID="AddressPanel" runat="server" Visible="false"
                    style="margin-top:15px; padding:12px; border:1px dashed #2c3e50; border-radius:8px; background:white; text-align:right;">
                    <div style="font-weight:bold; text-align:center; margin-bottom:10px; color:#2c3e50;">
                        כתובת איסוף לנהג
                    </div>

                    <div class="input-group">
                        <label>עיר:</label>
                        <asp:TextBox ID="TxtCity" runat="server" CssClass="input-control" placeholder="לדוגמה: תל אביב - יפו"></asp:TextBox>
                    </div>

                    <div class="input-group">
                        <label>רחוב:</label>
                        <asp:TextBox ID="TxtStreet" runat="server" CssClass="input-control" placeholder="לדוגמה: דיזנגוף"></asp:TextBox>
                    </div>

                    <div class="input-group">
                        <label>מספר בית:</label>
                        <asp:TextBox ID="TxtHouseNum" runat="server" CssClass="input-control" TextMode="Number" placeholder="לדוגמה: 12"></asp:TextBox>
                    </div>

                    <asp:Button ID="BtnConfirmAddress" runat="server" Text="אישור כתובת והזמנה"
                        OnClick="BtnConfirmAddress_Click"
                        BackColor="#27ae60" ForeColor="White" Width="100%" Height="40px" Font-Bold="True" />

                    <br /><br />
                    <asp:Label ID="LblAddressError" runat="server" ForeColor="#c0392b" Font-Bold="True"></asp:Label>
                </asp:Panel>

                <br /><br />
                <asp:Label ID="LblTaxiResult" runat="server"></asp:Label>
            </asp:Panel>
        </div>
    </form>
</body>
</html>