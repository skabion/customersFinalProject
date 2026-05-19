<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ArielProject.Login" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - התחברות</title>
    <%-- כל העיצוב מרוכז בקובץ Site.css --%>
    <link href="Site.css" rel="stylesheet" />
</head>
<body class="theme-auth theme-login login-flex">

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

            <%-- שונה: input-group → auth-input-group, icon → auth-icon
                 כדי להימנע מקונפליקט עם class באותו שם בדפים אחרים --%>
            <div class="auth-input-group">
                <span class="auth-icon">👤</span>
                <asp:TextBox ID="TxtFullName" runat="server" placeholder="שם מלא"></asp:TextBox>
            </div>

            <div class="auth-input-group">
                <span class="auth-icon">🔒</span>
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
