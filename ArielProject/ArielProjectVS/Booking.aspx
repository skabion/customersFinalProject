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

        /* עיצוב לכפתור של שעה פנויה */
        .available-time {
            background-color: white;
            color: black;
            border: 1px solid #ccc;
            padding: 10px 20px;
            margin: 5px 0;
            cursor: pointer;
            font-size: 16px;
            width: 100%;
            transition: 0.3s;
        }

        .available-time:hover {
            background-color: #f0f0f0;
        }

        /* עיצוב לכפתור של שעה תפוסה (אפור ולא לחיץ) */
        .unavailable-time {
            background-color: #f5f5f5;
            color: #b0b0b0;
            border: 1px solid #e0e0e0;
            padding: 10px 20px;
            margin: 5px 0;
            font-size: 16px;
            width: 100%;
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

            <div class="input-group">
                <label>תאריך:</label>
                <asp:TextBox ID="TxtDate" runat="server" CssClass="input-control" TextMode="Date"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidatorDate" runat="server" 
                ControlToValidate="TxtDate" 
                ErrorMessage="נדרש לבחור תאריך עתידי" 
                ForeColor="#c0392b" 
                Display="Dynamic" 
                Operator="GreaterThanEqual" 
                Type="Date"
                Font-Size="14px">
            </asp:CompareValidator>
            </div>

            <div class="input-group">
                <label>מספר סועדים:</label>
                <asp:TextBox ID="TxtGuests" runat="server" CssClass="input-control" TextMode="Number"></asp:TextBox>
            </div>

            <asp:Button ID="BtnCheckTimes" runat="server" Text="מצאו לי שולחן" 
                OnClick="BtnCheckTimes_Click" BackColor="Black" ForeColor="White" 
                Width="100%" Height="40px" Font-Bold="True" />

            <br /><br />

            <asp:Repeater ID="RepeaterTimes" runat="server" OnItemCommand="RepeaterTimes_ItemCommand">
                <ItemTemplate>
                    <div>
                        <asp:Button ID="BtnTime" runat="server" 
                            Text='<%# Eval("TimeStr") %>' 
                            CommandArgument='<%# Eval("TimeStr") %>' 
                            Enabled='<%# Convert.ToBoolean(Eval("IsAvailable")) %>'
                            CssClass='<%# Convert.ToBoolean(Eval("IsAvailable")) ? "available-time" : "unavailable-time" %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <br />
            <asp:Label ID="LblMsg" runat="server" Font-Bold="True"></asp:Label>
        </div>
    </form>
</body>
</html>