<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ArielProject.Login" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - התחברות</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        html, body {
            height: 100%;
            font-family: 'Segoe UI', Arial, sans-serif;
            direction: rtl;
            overflow: hidden;
        }

        body {
            background: linear-gradient(-45deg, #ff006e, #8338ec, #3a86ff, #06ffa5);
            background-size: 400% 400%;
            animation: gradientShift 15s ease infinite;
            display: flex;
            justify-content: center;
            align-items: center;
            position: relative;
        }

        @keyframes gradientShift {
            0%   { background-position: 0% 50%; }
            50%  { background-position: 100% 50%; }
            100% { background-position: 0% 50%; }
        }

        /* כדורי ניאון מטושטשים שצפים ברקע */
        .orb {
            position: absolute;
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
            top: 50%; left: 10%;
            animation: floatOrb 10s ease-in-out infinite;
        }

        @keyframes floatOrb {
            0%, 100% { transform: translate(0, 0) scale(1); }
            33%      { transform: translate(40px, -50px) scale(1.1); }
            66%      { transform: translate(-30px, 40px) scale(0.95); }
        }

        /* כרטיס ההתחברות - glassmorphism */
        .login-card {
            position: relative;
            z-index: 2;
            width: 420px;
            max-width: 90%;
            padding: 50px 40px 40px;
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
            0% {
                opacity: 0;
                transform: translateY(40px) scale(0.95);
            }
            100% {
                opacity: 1;
                transform: translateY(0) scale(1);
            }
        }

        /* לוגו */
        .brand {
            text-align: center;
            margin-bottom: 35px;
        }
        .brand-icon {
            display: inline-flex;
            justify-content: center;
            align-items: center;
            width: 72px;
            height: 72px;
            border-radius: 50%;
            background: linear-gradient(135deg, #ff006e, #8338ec);
            box-shadow: 0 10px 30px rgba(255, 0, 110, 0.5);
            font-size: 34px;
            margin-bottom: 14px;
            animation: pulseGlow 2.5s ease-in-out infinite;
        }
        @keyframes pulseGlow {
            0%, 100% { box-shadow: 0 10px 30px rgba(255, 0, 110, 0.5); }
            50%      { box-shadow: 0 10px 45px rgba(131, 56, 236, 0.8); }
        }
        .brand h1 {
            font-size: 28px;
            font-weight: 700;
            letter-spacing: 2px;
            background: linear-gradient(90deg, #ffffff, #ffd1f0);
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
            margin-bottom: 22px;
        }
        .input-group .icon {
            position: absolute;
            top: 50%;
            right: 16px;
            transform: translateY(-50%);
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

        /* כפתור התחברות */
        .login-btn {
            width: 100%;
            padding: 16px;
            margin-top: 10px;
            font-size: 16px;
            font-weight: 700;
            color: #ffffff;
            letter-spacing: 2px;
            background: linear-gradient(135deg, #ff006e 0%, #8338ec 50%, #3a86ff 100%);
            background-size: 200% 200%;
            border: none;
            border-radius: 14px;
            cursor: pointer;
            transition: all 0.4s;
            box-shadow: 0 8px 25px rgba(131, 56, 236, 0.45);
            font-family: 'Segoe UI', Arial, sans-serif;
            position: relative;
            overflow: hidden;
        }
        .login-btn:hover {
            background-position: 100% 100%;
            transform: translateY(-2px);
            box-shadow: 0 12px 35px rgba(255, 0, 110, 0.55);
        }
        .login-btn:active {
            transform: translateY(0);
        }

        /* הודעת שגיאה */
        .error-area {
            min-height: 22px;
            margin-top: 16px;
            text-align: center;
        }
        .error-area span {
            color: #ffe1e1 !important;
            background: rgba(255, 60, 90, 0.25);
            border: 1px solid rgba(255, 100, 130, 0.5);
            padding: 8px 14px;
            border-radius: 10px;
            font-size: 13px;
            display: inline-block;
        }
        .error-area span:empty {
            display: none;
        }

        /* קישורים תחתונים */
        .extra-links {
            margin-top: 24px;
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
            color: #ffd1f0;
        }

        .home-link {
            display: block;
            margin-top: 14px;
            color: rgba(255, 255, 255, 0.6);
            font-size: 12px;
            text-decoration: none;
            letter-spacing: 1px;
        }
        .home-link:hover {
            color: #ffffff;
        }
    </style>
</head>
<body>

    <!-- כדורי רקע מטושטשים -->
    <div class="orb orb-1"></div>
    <div class="orb orb-2"></div>
    <div class="orb orb-3"></div>

    <form id="form1" runat="server" defaultbutton="BtnLogin">
        <div class="login-card">

            <div class="brand">
                <div class="brand-icon">🍽️</div>
                <h1>EatIt</h1>
                <p>ברוכים השבים — התחברו לחשבון שלכם</p>
            </div>

            <div class="input-group">
                <span class="icon">👤</span>
                <asp:TextBox ID="TxtFullName" runat="server" placeholder="שם מלא"></asp:TextBox>
            </div>

            <div class="input-group">
                <span class="icon">🔒</span>
                <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" placeholder="סיסמה"></asp:TextBox>
            </div>

            <asp:Button ID="BtnLogin" runat="server" Text="התחבר" OnClick="BtnLogin_Click" CssClass="login-btn" />

            <div class="error-area">
                <asp:Label ID="LblError" runat="server"></asp:Label>
            </div>

            <div class="extra-links">
                אין לכם חשבון? <a href="Insert.aspx">הירשמו עכשיו</a>
            </div>

            <a href="HomePage.aspx" class="home-link">← חזרה לדף הבית</a>

        </div>
    </form>
</body>
</html>
