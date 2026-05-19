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

            <%-- שונה: input-group → auth-input-group, icon → auth-icon --%>
            <!-- שם מלא -->
            <div class="auth-input-group">
                <span class="auth-icon">👤</span>
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
            <div class="auth-input-group">
                <span class="auth-icon">🔒</span>
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
            <div class="auth-input-group">
                <span class="auth-icon">📱</span>
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
