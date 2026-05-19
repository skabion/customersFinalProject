<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="ArielProject.Insert" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - הרשמה</title>
    <%-- כל העיצוב מרוכז בקובץ Site.css --%>
    <link href="Site.css" rel="stylesheet" />
</head>
<body class="theme-auth theme-signup signup-flex">

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

            <%-- הוחלפו כל ה-RegularExpressionValidator וה-RequiredFieldValidator
                 (שדרשו תחביר רגולרי מתקדם) בבדיקה ידנית בקוד C#.
                 כל שדה מקבל Label להצגת הודעת שגיאה (אם יש). --%>

            <!-- שם מלא -->
            <div class="auth-input-group">
                <span class="auth-icon">👤</span>
                <asp:TextBox ID="SignUp_FullName" runat="server" placeholder="שם מלא (באנגלית)"></asp:TextBox>
            </div>
            <asp:Label ID="LblNameError" runat="server" CssClass="validator-msg"></asp:Label>

            <!-- סיסמה -->
            <div class="auth-input-group">
                <span class="auth-icon">🔒</span>
                <asp:TextBox ID="SignUp_Password" runat="server" TextMode="Password" placeholder="סיסמה"></asp:TextBox>
            </div>
            <asp:Label ID="LblPasswordError" runat="server" CssClass="validator-msg"></asp:Label>

            <!-- טלפון -->
            <div class="auth-input-group">
                <span class="auth-icon">📱</span>
                <asp:TextBox ID="SignUp_Phone" runat="server" placeholder="טלפון (10 ספרות)"></asp:TextBox>
            </div>
            <asp:Label ID="LblPhoneError" runat="server" CssClass="validator-msg"></asp:Label>

            <!-- אזור -->
            <span class="section-title">📍 אזור מגורים</span>
            <div class="area-row">
                <%-- הוסרה התכונה OnSelectedIndexChanged כי ההאזנה ב-cs היתה ריקה ולא עשתה כלום --%>
                <asp:DropDownList ID="DropDownList1" runat="server">
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

            <%-- הוסר ValidationGroup="SignUp" כי אין יותר Validators
                 הבדיקה מתבצעת בקוד C# בפונקציה AddUser_Click --%>
            <asp:Button ID="AddUser" runat="server" Text="✨ הירשם עכשיו" OnClick="AddUser_Click" CssClass="signup-btn" />

            <div class="extra-links">
                כבר יש לכם חשבון? <a href="Login.aspx">התחברו כאן</a>
            </div>
            <a href="HomePage.aspx" class="home-link">← חזרה לדף הבית</a>

        </div>
    </form>
</body>
</html>
