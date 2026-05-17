<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserStats.aspx.cs" Inherits="ArielProject.UserStats" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - נתוני משתמשים</title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }

        body {
            font-family: Arial, sans-serif;
            direction: rtl;
            min-height: 100vh;
            background: #0a0a1a;
            color: #f0e8d0;
            overflow-x: hidden;
        }

        body::before {
            content: '';
            position: fixed;
            top: 0; left: 0; right: 0; bottom: 0;
            background:
                radial-gradient(ellipse at 20% 20%, rgba(120, 60, 200, 0.25) 0%, transparent 50%),
                radial-gradient(ellipse at 80% 80%, rgba(180, 100, 30, 0.2) 0%, transparent 50%),
                radial-gradient(ellipse at 50% 50%, rgba(10, 40, 80, 0.8) 0%, transparent 80%),
                linear-gradient(135deg, #0d0d2b 0%, #1a0a2e 30%, #0d1f0d 60%, #1a1205 100%);
            z-index: -2;
        }

        body::after {
            content: '';
            position: fixed;
            top: 0; left: 0; right: 0; bottom: 0;
            background-image:
                radial-gradient(circle, rgba(212,175,55,0.15) 1px, transparent 1px),
                radial-gradient(circle, rgba(180,130,200,0.1) 1px, transparent 1px);
            background-size: 40px 40px, 70px 70px;
            background-position: 0 0, 20px 20px;
            z-index: -1;
            pointer-events: none;
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 18px 40px;
            background: linear-gradient(90deg,
                rgba(212,175,55,0.08) 0%,
                rgba(120,60,200,0.12) 50%,
                rgba(212,175,55,0.08) 100%);
            border-bottom: 1px solid rgba(212,175,55,0.3);
            backdrop-filter: blur(4px);
        }

        .logo {
            font-size: 28px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 14px rgba(212,175,55,0.5), 0 0 2px rgba(255,248,220,0.7);
            letter-spacing: 3px;
        }

        .greeting-area {
            font-size: 17px;
            color: #e8d5a3;
            text-shadow: 0 0 12px rgba(212,175,55,0.5);
        }

        .hero {
            text-align: center;
            padding: 45px 20px 18px;
        }

        .hero-icon {
            font-size: 50px;
            display: block;
            margin-bottom: 8px;
        }

        .hero h1 {
            font-size: 30px;
            letter-spacing: 3px;
            font-weight: bold;
            color: #f5e27a;
            text-shadow: 0 0 18px rgba(212,175,55,0.55), 0 0 3px rgba(255,248,220,0.8);
            margin-bottom: 8px;
        }

        .admin-badge {
            display: inline-block;
            margin-top: 4px;
            padding: 6px 18px;
            background: rgba(120,60,200,0.15);
            border: 1px solid rgba(180,130,220,0.4);
            border-radius: 50px;
            color: #d6c1f0;
            font-size: 13px;
            letter-spacing: 1px;
        }

        .gold-divider {
            width: 200px;
            height: 2px;
            margin: 24px auto;
            background: linear-gradient(90deg, transparent, #d4af37, #f5e27a, #d4af37, transparent);
            border-radius: 2px;
        }

        .section-title {
            text-align: center;
            color: #f5e27a;
            font-size: 14px;
            letter-spacing: 5px;
            text-transform: uppercase;
            margin: 12px 0 22px;
            font-weight: bold;
        }

        /* --- KPI --- */
        .kpi-row {
            display: flex;
            justify-content: center;
            gap: 18px;
            flex-wrap: wrap;
            padding: 0 30px 35px;
            max-width: 1150px;
            margin: 0 auto;
        }

        .kpi-card {
            flex: 1;
            min-width: 180px;
            max-width: 240px;
            background: linear-gradient(145deg,
                rgba(255,255,255,0.07) 0%,
                rgba(212,175,55,0.07) 50%,
                rgba(120,60,200,0.08) 100%);
            border: 1px solid rgba(212,175,55,0.3);
            border-radius: 14px;
            padding: 22px 16px;
            text-align: center;
            backdrop-filter: blur(6px);
            transition: transform 0.3s, box-shadow 0.3s;
        }

        .kpi-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 10px 30px rgba(212,175,55,0.18);
        }

        .kpi-icon { font-size: 30px; display: block; margin-bottom: 6px; }
        .kpi-label { font-size: 12px; color: rgba(212,175,55,0.7); letter-spacing: 1px; margin-bottom: 10px; min-height: 30px; }
        .kpi-value { font-size: 32px; font-weight: bold; color: #f5e27a; text-shadow: 0 0 14px rgba(245,226,122,0.5); }

        /* --- CHARTS --- */
        .chart-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(340px, 1fr));
            gap: 20px;
            max-width: 1150px;
            margin: 0 auto;
            padding: 0 30px 35px;
        }

        .chart-card {
            background: linear-gradient(145deg,
                rgba(255,255,255,0.05) 0%,
                rgba(212,175,55,0.05) 50%,
                rgba(120,60,200,0.06) 100%);
            border: 1px solid rgba(212,175,55,0.25);
            border-radius: 14px;
            padding: 20px 22px;
            backdrop-filter: blur(6px);
        }

        .chart-title {
            font-size: 16px;
            color: #f5e27a;
            font-weight: bold;
            margin-bottom: 16px;
            border-bottom: 1px solid rgba(212,175,55,0.2);
            padding-bottom: 8px;
        }

        .bar-row { display: flex; align-items: center; gap: 10px; margin-bottom: 8px; font-size: 13px; }
        .bar-label { width: 95px; text-align: right; color: #e8d5a3; flex-shrink: 0; }
        .bar-track { flex: 1; height: 20px; background: rgba(0,0,0,0.35); border: 1px solid rgba(212,175,55,0.15); border-radius: 10px; overflow: hidden; }
        .bar-fill {
            height: 100%;
            background: linear-gradient(90deg, #c9954c, #d4af37, #f5e27a);
            border-radius: 10px;
            transition: width 0.6s ease;
            box-shadow: 0 0 10px rgba(212,175,55,0.35);
        }
        .bar-fill.green { background: linear-gradient(90deg, #3b7a4a, #5fa066, #a6d8a6); box-shadow: 0 0 10px rgba(95,160,102,0.3); }
        .bar-fill.red { background: linear-gradient(90deg, #a0331e, #d65a3e, #f0a080); box-shadow: 0 0 10px rgba(210,90,60,0.3); }
        .bar-value { min-width: 30px; text-align: left; font-weight: bold; color: #f5e27a; flex-shrink: 0; }

        /* --- FILTER BAR --- */
        .filter-bar {
            max-width: 1150px;
            margin: 0 auto 12px;
            padding: 20px 24px;
            background: linear-gradient(145deg,
                rgba(255,255,255,0.05) 0%,
                rgba(212,175,55,0.06) 100%);
            border: 1px solid rgba(212,175,55,0.3);
            border-radius: 14px;
            display: flex;
            gap: 14px;
            align-items: flex-end;
            flex-wrap: wrap;
        }

        .filter-group {
            display: flex;
            flex-direction: column;
            gap: 4px;
        }

        .filter-group label {
            font-size: 12px;
            color: rgba(212,175,55,0.8);
            font-weight: bold;
            letter-spacing: 1px;
        }

        .filter-select, .filter-input {
            padding: 8px 12px;
            background: rgba(0,0,0,0.4);
            border: 1px solid rgba(212,175,55,0.35);
            border-radius: 8px;
            color: #f0e8d0;
            font-size: 13px;
            font-family: Arial, sans-serif;
            cursor: pointer;
            min-width: 140px;
            transition: border-color 0.3s;
        }

        .filter-select:focus, .filter-input:focus {
            outline: none;
            border-color: rgba(212,175,55,0.75);
        }

        .filter-btn {
            padding: 9px 22px;
            background: linear-gradient(135deg, #d4af37, #f5e27a, #c9954c);
            color: #1a0a2e;
            border: none;
            border-radius: 8px;
            font-size: 13px;
            font-weight: bold;
            font-family: Arial, sans-serif;
            cursor: pointer;
            letter-spacing: 1px;
            transition: all 0.3s;
        }

        .filter-btn:hover { box-shadow: 0 4px 18px rgba(212,175,55,0.45); filter: brightness(1.1); }

        .filter-btn-clear {
            padding: 9px 18px;
            background: transparent;
            color: #c79ce8;
            border: 1px solid rgba(180,130,220,0.4);
            border-radius: 8px;
            font-size: 13px;
            font-weight: bold;
            font-family: Arial, sans-serif;
            cursor: pointer;
            transition: all 0.3s;
        }

        .filter-btn-clear:hover { background: rgba(180,130,220,0.1); border-color: #c79ce8; }

        .results-info {
            max-width: 1150px;
            margin: 0 auto 16px;
            padding: 0 30px;
            color: rgba(212,175,55,0.85);
            font-size: 14px;
            letter-spacing: 1px;
            text-align: center;
        }

        .results-info b { color: #f5e27a; }

        /* --- USER LIST --- */
        .users-container {
            max-width: 1150px;
            margin: 0 auto;
            padding: 0 30px 30px;
        }

        .user-card {
            background: linear-gradient(145deg,
                rgba(255,255,255,0.05) 0%,
                rgba(212,175,55,0.04) 100%);
            border: 1px solid rgba(212,175,55,0.22);
            border-radius: 12px;
            padding: 18px 22px;
            margin-bottom: 12px;
            display: flex;
            gap: 18px;
            flex-wrap: wrap;
            align-items: center;
            transition: border-color 0.3s, box-shadow 0.3s;
        }

        .user-card:hover {
            border-color: rgba(212,175,55,0.5);
            box-shadow: 0 6px 22px rgba(212,175,55,0.13);
        }

        .user-icon {
            font-size: 32px;
            width: 50px;
            height: 50px;
            display: flex;
            align-items: center;
            justify-content: center;
            background: rgba(212,175,55,0.12);
            border: 1px solid rgba(212,175,55,0.35);
            border-radius: 50%;
            flex-shrink: 0;
        }

        .user-info { flex: 1; min-width: 220px; }

        .user-name-row {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 4px;
            flex-wrap: wrap;
        }

        .user-name {
            font-size: 17px;
            color: #f5e27a;
            font-weight: bold;
        }

        .user-meta {
            font-size: 13px;
            color: rgba(240,232,208,0.75);
            margin-bottom: 8px;
        }

        .user-meta span { margin-left: 16px; }
        .user-meta .label { color: rgba(212,175,55,0.6); }

        .tag-row { display: flex; gap: 6px; flex-wrap: wrap; }

        .tag {
            display: inline-block;
            padding: 3px 11px;
            border-radius: 50px;
            font-size: 11.5px;
            letter-spacing: 0.5px;
        }

        .tag.diet { background: rgba(95,160,102,0.15); border: 1px solid rgba(140,200,140,0.4); color: #a6d8a6; }
        .tag.allergy { background: rgba(210,90,60,0.15); border: 1px solid rgba(240,160,128,0.4); color: #f0a080; }
        .tag.role-admin { background: rgba(120,60,200,0.18); border: 1px solid rgba(180,130,220,0.5); color: #d6c1f0; font-weight: bold; }
        .tag.role-rest { background: rgba(212,175,55,0.18); border: 1px solid rgba(212,175,55,0.5); color: #f5e27a; font-weight: bold; }

        .empty-message {
            text-align: center;
            padding: 40px 20px;
            background: rgba(255,255,255,0.03);
            border: 1px dashed rgba(212,175,55,0.3);
            border-radius: 14px;
            color: rgba(240,232,208,0.6);
            font-size: 14px;
        }

        .nav-bar {
            text-align: center;
            padding: 22px 20px 28px;
        }

        .nav-btn {
            display: inline-block;
            padding: 12px 28px;
            border-radius: 50px;
            font-size: 14px;
            font-weight: bold;
            text-decoration: none;
            transition: all 0.3s;
            letter-spacing: 1px;
            border: 1.5px solid rgba(212,175,55,0.55);
            color: #d4af37;
            background: transparent;
        }

        .nav-btn:hover {
            background: rgba(212,175,55,0.1);
            border-color: #d4af37;
            box-shadow: 0 4px 20px rgba(212,175,55,0.2);
            transform: translateY(-2px);
        }

        .footer {
            text-align: center;
            padding: 20px;
            font-size: 13px;
            color: rgba(212,175,55,0.3);
            border-top: 1px solid rgba(212,175,55,0.1);
        }
    </style>
</head>
<body>
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
                <asp:Repeater ID="RepeaterAreas" runat="server">
                    <ItemTemplate>
                        <div class="bar-row">
                            <div class="bar-label"><%# Eval("Label") %></div>
                            <div class="bar-track">
                                <div class="bar-fill" style="width: <%# Eval("Percent") %>%;"></div>
                            </div>
                            <div class="bar-value"><%# Eval("Count") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="chart-card">
                <div class="chart-title">🥗 העדפות תזונה</div>
                <asp:Repeater ID="RepeaterDiets" runat="server">
                    <ItemTemplate>
                        <div class="bar-row">
                            <div class="bar-label"><%# Eval("Label") %></div>
                            <div class="bar-track">
                                <div class="bar-fill green" style="width: <%# Eval("Percent") %>%;"></div>
                            </div>
                            <div class="bar-value"><%# Eval("Count") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="chart-card">
                <div class="chart-title">⚠️ אלרגיות נפוצות</div>
                <asp:Repeater ID="RepeaterAllergies" runat="server">
                    <ItemTemplate>
                        <div class="bar-row">
                            <div class="bar-label"><%# Eval("Label") %></div>
                            <div class="bar-track">
                                <div class="bar-fill red" style="width: <%# Eval("Percent") %>%;"></div>
                            </div>
                            <div class="bar-value"><%# Eval("Count") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
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
            <asp:Repeater ID="RepeaterUsers" runat="server">
                <ItemTemplate>
                    <div class="user-card">
                        <div class="user-icon">👤</div>
                        <div class="user-info">
                            <div class="user-name-row">
                                <span class="user-name"><%# Eval("Name") %></span>
                                <%# Eval("RoleTag") %>
                            </div>
                            <div class="user-meta">
                                <span><span class="label">📞</span> <%# Eval("Phone") %></span>
                                <span><span class="label">📍</span> <%# Eval("AreaHebrew") %></span>
                            </div>
                            <div class="tag-row">
                                <%# Eval("TagsHtml") %>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

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
