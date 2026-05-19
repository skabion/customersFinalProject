<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="ArielProject.Update" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - עריכת הזמנה</title>
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

        <div class="container">
            <h2>עריכת הזמנה</h2>
            <div class="subtitle">✦ &nbsp; עדכון פרטי ההזמנה &nbsp; ✦</div>

            <asp:Panel ID="pnlDetails" runat="server">
                <div class="res-header">
                    🍽️ <asp:Label ID="lblResName" runat="server"></asp:Label>
                </div>

                <%-- שונה: input-group → form-input-group, input-control → update-input
                     כדי להימנע מקונפליקט עם class באותו שם בדפים אחרים --%>
                <div class="form-input-group">
                    <label>תאריך חדש:</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="update-input" TextMode="Date"></asp:TextBox>
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

                <div class="form-input-group">
                    <label>כמות אורחים מעודכנת:</label>
                    <asp:TextBox ID="txtNumGuests" runat="server" CssClass="update-input" TextMode="Number"></asp:TextBox>
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
