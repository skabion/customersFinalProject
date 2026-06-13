<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddRestaurant.aspx.cs" Inherits="ArielProject.AddRestaurant" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - הוספת מסעדה</title>
    <%-- כל העיצוב מרוכז בקובץ Site.css --%>
    <link href="Site.css" rel="stylesheet" />
</head>
<body class="theme-dark">
    <form id="form1" runat="server">

        <div class="header">
            <div class="logo">✦ EatIt ✦</div>
            <div class="greeting-area">
                שלום, <asp:Label ID="LblUserName" runat="server"></asp:Label>
            </div>
        </div>

        <div class="hero">
            <span class="hero-icon">➕</span>
            <h1>הוספת מסעדה חדשה</h1>
            <div class="hero-subtitle">✦ &nbsp; הוספת מסעדה לקטלוג &nbsp; ✦</div>
            <div class="admin-badge">⚙️ מצב מנהל מערכת</div>
        </div>

        <div class="gold-divider"></div>

        <div class="container">
            <h2>פרטי המסעדה</h2>
            <div class="subtitle">✦ &nbsp; מלא את כל השדות &nbsp; ✦</div>

            <%-- שם המסעדה - המזהה של המסעדה בכל המערכת --%>
            <div class="form-input-group">
                <label>שם המסעדה (באנגלית):</label>
                <asp:TextBox ID="TxtName" runat="server" CssClass="update-input" placeholder="לדוגמה: Bella Roma"></asp:TextBox>
            </div>

            <%-- אזור - אותם ערכים כמו בקטלוג --%>
            <div class="form-input-group">
                <label>אזור:</label>
                <asp:DropDownList ID="DdlRegion" runat="server" CssClass="update-input">
                    <asp:ListItem Text="צפון" Value="Tzafon"></asp:ListItem>
                    <asp:ListItem Text="מרכז" Value="Merkaz"></asp:ListItem>
                    <asp:ListItem Text="דרום" Value="Darom"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- סוג מטבח - אותם ערכים כמו בקטלוג --%>
            <div class="form-input-group">
                <label>סוג מטבח:</label>
                <asp:DropDownList ID="DdlType" runat="server" CssClass="update-input">
                    <asp:ListItem Text="איטלקי" Value="Italian"></asp:ListItem>
                    <asp:ListItem Text="בשרי" Value="Meat"></asp:ListItem>
                    <asp:ListItem Text="אסייתי" Value="Asian"></asp:ListItem>
                    <asp:ListItem Text="בראסרי" Value="Brasserie"></asp:ListItem>
                    <asp:ListItem Text="קינוחים" Value="Deserts"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- כשרות + תחלופה לאלרגנים - צ'קבוקסים (מסומן = "כן") --%>
            <div class="form-input-group">
                <label>מאפיינים:</label>
                <div style="color:#f0e8d0; accent-color:#d4af37; line-height:2;">
                    <asp:CheckBox ID="ChkKosher" runat="server" Text="מסעדה כשרה" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="ChkReplacement" runat="server" Text="תחלופה לאלרגנים" />
                </div>
            </div>

            <%-- מספר השולחנות לפי גודל - נשמרים כטקסט אך חייבים להיות מספרים שלמים --%>
            <div class="form-input-group">
                <label>מספר שולחנות קטנים (עד 2 סועדים):</label>
                <asp:TextBox ID="TxtSmall" runat="server" CssClass="update-input" TextMode="Number" placeholder="לדוגמה: 5"></asp:TextBox>
            </div>

            <div class="form-input-group">
                <label>מספר שולחנות בינוניים (3-4 סועדים):</label>
                <asp:TextBox ID="TxtMedium" runat="server" CssClass="update-input" TextMode="Number" placeholder="לדוגמה: 4"></asp:TextBox>
            </div>

            <div class="form-input-group">
                <label>מספר שולחנות גדולים (5+ סועדים):</label>
                <asp:TextBox ID="TxtLarge" runat="server" CssClass="update-input" TextMode="Number" placeholder="לדוגמה: 2"></asp:TextBox>
            </div>

            <asp:Button ID="BtnAdd" runat="server" Text="➕ הוסף מסעדה לקטלוג"
                OnClick="BtnAdd_Click" CssClass="btn-main" />

            <%-- הודעת שגיאה (אדום) והודעת הצלחה (ירוק) --%>
            <div style="text-align:center; margin-top:18px;">
                <asp:Label ID="LblError" runat="server" ForeColor="#ff8888" Font-Bold="True"></asp:Label>
                <asp:Label ID="LblSuccess" runat="server" ForeColor="#8be08b" Font-Bold="True"></asp:Label>
            </div>
        </div>

        <div class="nav-bar">
            <a href="RestaurantAdmin.aspx" class="nav-btn">← חזרה לדף מנהל</a>
            <a href="Catalog.aspx" class="nav-btn">🍽️ צפייה בקטלוג</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
