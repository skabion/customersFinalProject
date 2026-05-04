<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="ArielProject.Insert" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - הרשמה</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        html, body {
            min-height: 100%;
            font-family: 'Segoe UI', Arial, sans-serif;
            direction: rtl;
        }

        body {
            background: linear-gradient(-45deg, #ff006e, #8338ec, #3a86ff, #06ffa5);
            background-size: 400% 400%;
            animation: gradientShift 15s ease infinite;
            display: flex;
            justify-content: center;
            align-items: flex-start;
            padding: 40px 20px;
            position: relative;
            overflow-x: hidden;
        }

        @keyframes gradientShift {
            0%   { background-position: 0% 50%; }
            50%  { background-position: 100% 50%; }
            100% { background-position: 0% 50%; }
        }

        /* כדורי ניאון מטושטשים שצפים ברקע */
        .orb {
            position: fixed;
            border-radius: 50%;
            filter: blur(70px);
            opacity: 0.6;
            pointer-events: none;
            z-index: 0;
        }
        .orb-1 {
            width: 380px; height: 380px;
            background: #ff006e;
            top: -100px; right: -100px;
            animation: floatOrb 12s ease-in-out infinite;
        }
        .orb-2 {
            width: 320px; height: 320px;
            background: #06ffa5;
            bottom: -80px; left: -80px;
            animation: floatOrb 14s ease-in-out infinite reverse;
        }
        .orb-3 {
            width: 260px; height: 260px;
            background: #3a86ff;
            top: 50%; left: 5%;
            animation: floatOrb 10s ease-in-out infinite;
        }

        @keyframes floatOrb {
            0%, 100% { transform: translate(0, 0) scale(1); }
            33%      { transform: translate(40px, -50px) scale(1.1); }
            66%      { transform: translate(-30px, 40px) scale(0.95); }
        }

        /* כרטיס ההרשמה - glassmorphism */
        .signup-card {
            position: relative;
            z-index: 2;
            width: 520px;
            max-width: 95%;
            padding: 45px 40px 35px;
            background: rgba(255, 255, 255, 0.12);
            backdrop-filter: blur(22px);
            -webkit-backdrop-filter: blur(22px);
            border: 1px solid rgba(255, 255, 255, 0.25);
            border-radius: 24px;
            box-shadow:
                0 25px 60px rgba(0, 0, 0, 0.35),
                inset 0 1px 0 rgba(255, 255, 255, 0.4);
            color: #ffffff;
            animation: cardFloat 0.8s cubic-bezier(0.34, 1.56, 0.64, 1);
        }

        @keyframes cardFloat {
            0%   { opacity: 0; transform: translateY(40px) scale(0.95); }
            100% { opacity: 1; transform: translateY(0) scale(1); }
        }

        /* לוגו */
        .brand {
            text-align: center;
            margin-bottom: 30px;
        }
        .brand-icon {
            display: inline-flex;
            justify-content: center;
            align-items: center;
            width: 72px;
            height: 72px;
            border-radius: 50%;
            background: linear-gradient(135deg, #06ffa5, #3a86ff);
            box-shadow: 0 10px 30px rgba(6, 255, 165, 0.5);
            font-size: 34px;
            margin-bottom: 14px;
            animation: pulseGlow 2.5s ease-in-out infinite;
        }
        @keyframes pulseGlow {
            0%, 100% { box-shadow: 0 10px 30px rgba(6, 255, 165, 0.5); }
            50%      { box-shadow: 0 10px 45px rgba(58, 134, 255, 0.8); }
        }
        .brand h1 {
            font-size: 26px;
            font-weight: 700;
            letter-spacing: 2px;
            background: linear-gradient(90deg, #ffffff, #d1ffe6);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            margin-bottom: 6px;
        }
        .brand p {
            font-size: 14px;
            color: rgba(255, 255, 255, 0.75);
            letter-spacing: 1px;
        }

        /* שדות קלט */
        .input-group {
            position: relative;
            margin-bottom: 16px;
        }
        .input-group .icon {
            position: absolute;
            top: 16px;
            right: 16px;
            font-size: 18px;
            color: rgba(255, 255, 255, 0.7);
            pointer-events: none;
            z-index: 2;
        }
        .input-group input[type="text"],
        .input-group input[type="password"] {
            width: 100%;
            padding: 16px 48px 16px 18px;
            font-size: 15px;
            color: #ffffff;
            background: rgba(255, 255, 255, 0.1);
            border: 1.5px solid rgba(255, 255, 255, 0.25);
            border-radius: 14px;
            outline: none;
            transition: all 0.3s;
            font-family: 'Segoe UI', Arial, sans-serif;
            direction: rtl;
        }
        .input-group input::placeholder {
            color: rgba(255, 255, 255, 0.55);
        }
        .input-group input:focus {
            background: rgba(255, 255, 255, 0.18);
            border-color: #ffffff;
            box-shadow: 0 0 0 4px rgba(255, 255, 255, 0.15);
        }

        /* הודעות validator */
        .validator-msg {
            display: block;
            margin: 6px 6px 10px;
            color: #ffe1e1 !important;
            background: rgba(255, 60, 90, 0.25);
            border: 1px solid rgba(255, 100, 130, 0.5);
            padding: 6px 12px;
            border-radius: 8px;
            font-size: 12px;
            text-align: right;
        }

        /* קבוצות */
        .section-title {
            display: block;
            font-size: 13px;
            color: rgba(255, 255, 255, 0.85);
            letter-spacing: 1px;
            margin: 18px 4px 10px;
            font-weight: 600;
        }

        /* Dropdown - אזור */
        .area-row {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 8px;
        }
        .area-row select {
            flex: 1;
            padding: 14px 18px;
            font-size: 15px;
            color: #ffffff;
            background: rgba(255, 255, 255, 0.1);
            border: 1.5px solid rgba(255, 255, 255, 0.25);
            border-radius: 14px;
            outline: none;
            font-family: 'Segoe UI', Arial, sans-serif;
            cursor: pointer;
            direction: rtl;
        }
        .area-row select option {
            background: #2a1a4a;
            color: #ffffff;
        }
        .area-row select:focus {
            background: rgba(255, 255, 255, 0.18);
            border-color: #ffffff;
        }
        .area-row .area-label {
            font-size: 14px;
            color: rgba(255, 255, 255, 0.85);
            font-weight: 600;
        }

        /* CheckBoxes - מעטפת */
        .checkbox-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 8px;
            margin-bottom: 10px;
        }
        .checkbox-grid.allergies {
            grid-template-columns: repeat(3, 1fr);
        }

        /* asp:CheckBox מורנדר כ-span שעוטף input+label */
        .checkbox-grid > span {
            display: flex;
            align-items: center;
            gap: 8px;
            padding: 10px 12px;
            background: rgba(255, 255, 255, 0.08);
            border: 1.5px solid rgba(255, 255, 255, 0.2);
            border-radius: 12px;
            transition: all 0.3s;
            cursor: pointer;
        }
        .checkbox-grid > span:hover {
            background: rgba(255, 255, 255, 0.15);
            border-color: rgba(255, 255, 255, 0.5);
        }
        .checkbox-grid input[type="checkbox"] {
            width: 18px;
            height: 18px;
            cursor: pointer;
            accent-color: #06ffa5;
            margin: 0;
        }
        .checkbox-grid label {
            font-size: 14px;
            color: #ffffff;
            cursor: pointer;
            margin: 0;
        }

        /* כפתור הרשמה */
        .signup-btn {
            width: 100%;
            padding: 16px;
            margin-top: 20px;
            font-size: 16px;
            font-weight: 700;
            color: #ffffff;
            letter-spacing: 2px;
            background: linear-gradient(135deg, #06ffa5 0%, #3a86ff 50%, #8338ec 100%);
            background-size: 200% 200%;
            border: none;
            border-radius: 14px;
            cursor: pointer;
            transition: all 0.4s;
            box-shadow: 0 8px 25px rgba(58, 134, 255, 0.45);
            font-family: 'Segoe UI', Arial, sans-serif;
        }
        .signup-btn:hover {
            background-position: 100% 100%;
            transform: translateY(-2px);
            box-shadow: 0 12px 35px rgba(6, 255, 165, 0.55);
        }
        .signup-btn:active {
            transform: translateY(0);
        }

        /* קישורים תחתונים */
        .extra-links {
            margin-top: 22px;
            text-align: center;
            font-size: 13px;
            color: rgba(255, 255, 255, 0.75);
        }
        .extra-links a {
            color: #ffffff;
            font-weight: 600;
            text-decoration: none;
            border-bottom: 1px dashed rgba(255, 255, 255, 0.5);
            padding-bottom: 1px;
            transition: all 0.3s;
        }
        .extra-links a:hover {
            border-bottom-color: #ffffff;
            color: #d1ffe6;
        }
        .home-link {
            display: block;
            margin-top: 12px;
            color: rgba(255, 255, 255, 0.6);
            font-size: 12px;
            text-decoration: none;
            letter-spacing: 1px;
        }
        .home-link:hover {
            color: #ffffff;
        }

        @media (max-width: 540px) {
            .checkbox-grid { grid-template-columns: repeat(2, 1fr); }
            .signup-card { padding: 35px 22px 25px; }
        }
    </style>
</head>
<body>

    <!-- כדורי רקע מטושטשים -->
    <div class="orb orb-1"></div>
    <div class="orb orb-2"></div>
    <div class="orb orb-3"></div>

    <form id="form1" runat="server" defaultbutton="AddUser">
        <div class="signup-card">

            <div class="brand">
                <div class="brand-icon">✨</div>
                <h1>הצטרפו אלינו</h1>
                <p>פתחו חשבון חדש ב-EatIt</p>
            </div>

            <!-- שם מלא -->
            <div class="input-group">
                <span class="icon">👤</span>
                <asp:TextBox ID="SignUp_FullName" runat="server" placeholder="שם מלא (באנגלית)"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                ControlToValidate="SignUp_FullName"
                ErrorMessage="לא הוזן שם מלא"
                ValidationGroup="SignUp"
                Display="Dynamic"
                CssClass="validator-msg" />
            <asp:RegularExpressionValidator ID="SignUp_FullName_RegularExpressionValidator" runat="server"
                ControlToValidate="SignUp_FullName"
                ValidationExpression="^[A-Z][a-zA-Z]*(\s+[A-Z][a-zA-Z]*)+$"
                ErrorMessage="שם לא תקין: אותיות אנגלית בלבד, לפחות 2 מילים, כל אחת מתחילה באות גדולה"
                ValidationGroup="SignUp"
                Display="Dynamic"
                CssClass="validator-msg" />

            <!-- סיסמה -->
            <div class="input-group">
                <span class="icon">🔒</span>
                <asp:TextBox ID="SignUp_Password" runat="server" TextMode="Password" placeholder="סיסמה"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                ControlToValidate="SignUp_Password"
                ErrorMessage="לא הוזנה סיסמה"
                ValidationGroup="SignUp"
                Display="Dynamic"
                CssClass="validator-msg" />
            <asp:RegularExpressionValidator ID="SignUp_Password_RegularExpressionValidator" runat="server"
                ControlToValidate="SignUp_Password"
                ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&])[A-Za-z\d!@#$%^&]{6,}$"
                ErrorMessage="סיסמה חלשה מדי: דרושים אות גדולה, אות קטנה, ספרה, תו מיוחד, ולפחות 6 תווים"
                ValidationGroup="SignUp"
                Display="Dynamic"
                CssClass="validator-msg" />

            <!-- טלפון -->
            <div class="input-group">
                <span class="icon">📱</span>
                <asp:TextBox ID="SignUp_Phone" runat="server" placeholder="טלפון (10 ספרות)"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                ControlToValidate="SignUp_Phone"
                ErrorMessage="לא הוזן טלפון"
                ValidationGroup="SignUp"
                Display="Dynamic"
                CssClass="validator-msg" />
            <asp:RegularExpressionValidator ID="SignUp_Phone_RegularExpressionValidator" runat="server"
                ControlToValidate="SignUp_Phone"
                ValidationExpression="^05[02345]\d{7}$"
                ErrorMessage="מספר טלפון לא תקין: 10 ספרות שמתחילות ב-050, 052, 053, 054 או 055"
                ValidationGroup="SignUp"
                Display="Dynamic"
                CssClass="validator-msg" />

            <!-- אזור -->
            <span class="section-title">📍 אזור מגורים</span>
            <div class="area-row">
                <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownListArea_SelectedIndexChanged">
                </asp:DropDownList>
            </div>

            <!-- העדפות -->
            <span class="section-title">🥗 העדפות תזונתיות</span>
            <div class="checkbox-grid">
                <asp:CheckBox ID="CheckBoxVegan" runat="server" Text="טבעוני" />
                <asp:CheckBox ID="CheckBoxKosher" runat="server" Text="כשר" />
                <asp:CheckBox ID="CheckBoxVegetarian" runat="server" Text="צמחוני" />
            </div>

            <!-- אלרגיות -->
            <span class="section-title">⚠️ אלרגיות</span>
            <div class="checkbox-grid allergies">
                <asp:CheckBox ID="CheckBoxGluten" runat="server" Text="גלוטן" />
                <asp:CheckBox ID="CheckBoxPeanuts" runat="server" Text="בוטנים" />
                <asp:CheckBox ID="CheckBoxTreeNuts" runat="server" Text="אגוזים" />
                <asp:CheckBox ID="CheckBoxFish" runat="server" Text="דגים" />
                <asp:CheckBox ID="CheckBoxSesame" runat="server" Text="שומשום" />
                <asp:CheckBox ID="CheckBoxMilk" runat="server" Text="חלב" />
            </div>

            <asp:Button ID="AddUser" runat="server" Text="✨ הירשם עכשיו" OnClick="AddUser_Click" ValidationGroup="SignUp" CssClass="signup-btn" />

            <div class="extra-links">
                כבר יש לכם חשבון? <a href="Login.aspx">התחברו כאן</a>
            </div>
            <a href="HomePage.aspx" class="home-link">← חזרה לדף הבית</a>

        </div>
    </form>
</body>
</html>
