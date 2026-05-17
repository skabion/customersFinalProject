<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="ArielProject.Update" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - עריכת הזמנה</title>
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

        .container {
            max-width: 560px;
            margin: 40px auto;
            padding: 36px 32px;
            background: linear-gradient(145deg,
                rgba(255,255,255,0.06) 0%,
                rgba(212,175,55,0.05) 50%,
                rgba(120,60,200,0.07) 100%);
            border: 1px solid rgba(212,175,55,0.3);
            border-radius: 16px;
            backdrop-filter: blur(6px);
            box-shadow: 0 12px 35px rgba(0,0,0,0.4);
        }

        h2 {
            text-align: center;
            color: #f5e27a;
            font-size: 28px;
            letter-spacing: 3px;
            margin-bottom: 8px;
            text-shadow: 0 0 14px rgba(212,175,55,0.5);
        }

        .subtitle {
            text-align: center;
            color: rgba(212,175,55,0.65);
            font-size: 12px;
            letter-spacing: 4px;
            text-transform: uppercase;
            margin-bottom: 30px;
        }

        .res-header {
            background: linear-gradient(90deg, rgba(212,175,55,0.15), rgba(212,175,55,0.05));
            border: 1px solid rgba(212,175,55,0.4);
            color: #f5e27a;
            padding: 14px 18px;
            border-radius: 10px;
            text-align: center;
            margin-bottom: 22px;
            font-size: 18px;
            font-weight: bold;
        }

        .input-group {
            margin-bottom: 16px;
        }

        label {
            display: block;
            font-weight: bold;
            margin-bottom: 6px;
            color: #e8d5a3;
            font-size: 14px;
        }

        .input-control {
            width: 100%;
            padding: 11px 14px;
            background: rgba(0,0,0,0.3);
            border: 1px solid rgba(212,175,55,0.3);
            border-radius: 8px;
            color: #f0e8d0;
            font-size: 14px;
            font-family: Arial, sans-serif;
            box-sizing: border-box;
            transition: border-color 0.3s;
        }

        .input-control:focus {
            outline: none;
            border-color: rgba(212,175,55,0.7);
            box-shadow: 0 0 12px rgba(212,175,55,0.15);
        }

        .available-time {
            background: rgba(255,255,255,0.05);
            color: #f0e8d0;
            border: 1px solid rgba(212,175,55,0.35);
            padding: 11px;
            margin: 6px 0;
            cursor: pointer;
            width: 100%;
            border-radius: 8px;
            transition: all 0.3s;
            font-family: Arial, sans-serif;
            font-size: 14px;
        }

        .available-time:hover {
            background: rgba(212,175,55,0.15);
            border-color: rgba(212,175,55,0.7);
            transform: translateY(-1px);
        }

        .unavailable-time {
            background: rgba(0,0,0,0.2);
            color: rgba(240,232,208,0.3);
            border: 1px solid rgba(212,175,55,0.1);
            padding: 11px;
            margin: 6px 0;
            width: 100%;
            border-radius: 8px;
            cursor: not-allowed;
            font-family: Arial, sans-serif;
            font-size: 14px;
        }

        .btn-main {
            background: linear-gradient(135deg, #d4af37, #f5e27a, #c9954c);
            color: #1a0a2e;
            border: none;
            padding: 13px;
            width: 100%;
            border-radius: 8px;
            cursor: pointer;
            font-size: 15px;
            font-weight: bold;
            font-family: Arial, sans-serif;
            letter-spacing: 1px;
            transition: all 0.3s;
            margin-top: 6px;
        }

        .btn-main:hover {
            box-shadow: 0 6px 22px rgba(212,175,55,0.5);
            filter: brightness(1.1);
        }

        .btn-delete {
            background: transparent;
            color: #ff8888;
            border: 1.5px solid rgba(255,100,100,0.5);
            padding: 11px;
            margin-top: 25px;
            border-radius: 8px;
            cursor: pointer;
            width: 100%;
            font-size: 14px;
            font-weight: bold;
            font-family: Arial, sans-serif;
            transition: all 0.3s;
        }

        .btn-delete:hover {
            background: rgba(255,80,80,0.15);
            border-color: #ff6666;
        }

        .times-label {
            display: block;
            margin: 20px 0 8px;
            color: #e8d5a3;
            font-weight: bold;
            font-size: 14px;
        }

        .nav-bar {
            text-align: center;
            padding: 10px 20px 30px;
        }

        .nav-btn {
            display: inline-block;
            padding: 11px 24px;
            border-radius: 50px;
            font-size: 13px;
            font-weight: bold;
            text-decoration: none;
            transition: all 0.3s;
            border: 1.5px solid rgba(212,175,55,0.55);
            color: #d4af37;
            background: transparent;
        }

        .nav-btn:hover {
            background: rgba(212,175,55,0.1);
            border-color: #d4af37;
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

        <div class="container">
            <h2>עריכת הזמנה</h2>
            <div class="subtitle">✦ &nbsp; עדכון פרטי ההזמנה &nbsp; ✦</div>

            <asp:Panel ID="pnlDetails" runat="server">
                <div class="res-header">
                    🍽️ <asp:Label ID="lblResName" runat="server"></asp:Label>
                </div>

                <div class="input-group">
                    <label>תאריך חדש:</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="input-control" TextMode="Date"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidatorDate" runat="server"
                        ControlToValidate="txtDate"
                        ErrorMessage="לא ניתן לבחור תאריך שעבר - נא לבחור תאריך עתידי"
                        ForeColor="#ff8888"
                        Display="Dynamic"
                        Operator="GreaterThanEqual"
                        Type="Date"
                        Font-Size="13px">
                    </asp:CompareValidator>
                </div>

                <div class="input-group">
                    <label>כמות אורחים מעודכנת:</label>
                    <asp:TextBox ID="txtNumGuests" runat="server" CssClass="input-control" TextMode="Number"></asp:TextBox>
                </div>

                <asp:Button ID="btnCheckAvailability" runat="server" Text="🔍 בדוק שעות פנויות"
                    OnClick="btnCheckAvailability_Click" CssClass="btn-main" />

                <span class="times-label">בחר שעה חדשה:</span>
                <asp:Repeater ID="RepeaterTimes" runat="server" OnItemCommand="RepeaterTimes_ItemCommand">
                    <ItemTemplate>
                        <asp:Button ID="BtnTime" runat="server"
                            Text='<%# Eval("TimeStr") %>'
                            CommandArgument='<%# Eval("TimeStr") %>'
                            Enabled='<%# Convert.ToBoolean(Eval("IsAvailable")) %>'
                            CssClass='<%# Convert.ToBoolean(Eval("IsAvailable")) ? "available-time" : "unavailable-time" %>' />
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Button ID="btnDelete" runat="server" Text="❌ ביטול הזמנה (מחיקה)" OnClick="btnDelete_Click"
                    CssClass="btn-delete" OnClientClick="return confirm('האם אתה בטוח שברצונך לבטל את ההזמנה?');" />
            </asp:Panel>

            <br />
            <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
        </div>

        <div class="nav-bar">
            <a href="MyBookings.aspx" class="nav-btn">← חזרה לרשימת ההזמנות</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
