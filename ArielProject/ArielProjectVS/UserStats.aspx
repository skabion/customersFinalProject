<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserStats.aspx.cs" Inherits="ArielProject.UserStats" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - נתוני משתמשים</title>
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
            <span class="hero-icon">👥</span>
            <h1>נתוני משתמשים</h1>
            <div class="admin-badge">⚙️ מצב מנהל מערכת</div>
        </div>

        <div class="gold-divider"></div>

        <div class="section-title">✦ &nbsp; סטטיסטיקה כללית &nbsp; ✦</div>

        <div class="kpi-row">
            <div class="kpi-card">
                <span class="kpi-icon">👤</span>
                <div class="kpi-label">סך כל המשתמשים</div>
                <div class="kpi-value"><asp:Label ID="LblTotalUsers" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="kpi-card">
                <span class="kpi-icon">🥗</span>
                <div class="kpi-label">צמחונים / טבעונים</div>
                <div class="kpi-value"><asp:Label ID="LblVegCount" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="kpi-card">
                <span class="kpi-icon">✡️</span>
                <div class="kpi-label">שומרי כשרות</div>
                <div class="kpi-value"><asp:Label ID="LblKosherCount" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="kpi-card">
                <span class="kpi-icon">⚠️</span>
                <div class="kpi-label">משתמשים עם אלרגיות</div>
                <div class="kpi-value"><asp:Label ID="LblAllergyCount" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="kpi-card">
                <span class="kpi-icon">👑</span>
                <div class="kpi-label">מנהלים במערכת</div>
                <div class="kpi-value"><asp:Label ID="LblAdminsCount" runat="server" Text="0"></asp:Label></div>
            </div>
        </div>

        <div class="section-title">✦ &nbsp; פילוחים וגרפים &nbsp; ✦</div>

        <div class="chart-grid">
            <div class="chart-card">
                <div class="chart-title">📍 התפלגות לפי אזור</div>
                <asp:Label ID="LblAreaChart" runat="server"></asp:Label>
            </div>

            <div class="chart-card">
                <div class="chart-title">🥗 העדפות תזונה</div>
                <asp:Label ID="LblDietChart" runat="server"></asp:Label>
            </div>

            <div class="chart-card">
                <div class="chart-title">⚠️ אלרגיות נפוצות</div>
                <asp:Label ID="LblAllergyChart" runat="server"></asp:Label>
            </div>
        </div>

        <div class="section-title">✦ &nbsp; חיפוש וסינון משתמשים &nbsp; ✦</div>

        <div class="filter-bar">
            <div class="filter-group">
                <label>אזור</label>
                <asp:DropDownList ID="DdlArea" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                    <asp:ListItem Value="" Text="כל האזורים" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Darom" Text="דרום"></asp:ListItem>
                    <asp:ListItem Value="Merkaz" Text="מרכז"></asp:ListItem>
                    <asp:ListItem Value="Tzafon" Text="צפון"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="filter-group">
                <label>העדפת תזונה</label>
                <asp:DropDownList ID="DdlDiet" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                    <asp:ListItem Value="" Text="הכל" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Vegetarian" Text="צמחוני"></asp:ListItem>
                    <asp:ListItem Value="Vegan" Text="טבעוני"></asp:ListItem>
                    <asp:ListItem Value="Kosher" Text="כשר"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="filter-group">
                <label>אלרגיה</label>
                <asp:DropDownList ID="DdlAllergy" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                    <asp:ListItem Value="" Text="הכל" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Gluten" Text="גלוטן"></asp:ListItem>
                    <asp:ListItem Value="Peanuts" Text="בוטנים"></asp:ListItem>
                    <asp:ListItem Value="TreeNuts" Text="אגוזים"></asp:ListItem>
                    <asp:ListItem Value="Fish" Text="דגים"></asp:ListItem>
                    <asp:ListItem Value="Sesame" Text="שומשום"></asp:ListItem>
                    <asp:ListItem Value="Milk" Text="חלב"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="filter-group">
                <label>תפקיד</label>
                <asp:DropDownList ID="DdlRole" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                    <asp:ListItem Value="" Text="הכל" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="User" Text="משתמש רגיל"></asp:ListItem>
                    <asp:ListItem Value="RestAdmin" Text="מנהל מסעדה"></asp:ListItem>
                    <asp:ListItem Value="Admin" Text="מנהל מערכת"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="filter-group">
                <label>חיפוש לפי שם</label>
                <asp:TextBox ID="TxtName" runat="server" CssClass="filter-input" placeholder="הקלד שם..."></asp:TextBox>
            </div>
            <asp:Button ID="BtnFilter" runat="server" Text="🔍 סנן" CssClass="filter-btn" OnClick="BtnFilter_Click" />
            <asp:Button ID="BtnClear" runat="server" Text="✕ נקה" CssClass="filter-btn-clear" OnClick="BtnClear_Click" />
        </div>

        <div class="results-info">
            מציג <b><asp:Label ID="LblShowing" runat="server" Text="0"></asp:Label></b>
            מתוך <b><asp:Label ID="LblTotal" runat="server" Text="0"></asp:Label></b>
            משתמשים
        </div>


        <div class="users-container">
            <asp:Label ID="LblUsersList" runat="server"></asp:Label>

            <asp:Panel ID="PnlEmpty" runat="server" Visible="false">
                <div class="empty-message">
                    🔍 לא נמצאו משתמשים שתואמים את הסינון - נסה לשנות את הקריטריונים
                </div>
            </asp:Panel>
        </div>

        <div class="nav-bar">
            <a href="RestaurantAdmin.aspx" class="nav-btn">← חזרה לדף מנהל</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
