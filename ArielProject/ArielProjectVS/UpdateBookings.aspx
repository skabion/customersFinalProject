<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateBookings.aspx.cs" Inherits="ArielProject.UpdateBookings" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>EatIt - ניהול ההזמנות שלי</title>
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
            <span class="hero-icon">📋</span>
            <h1>ההזמנות העתידיות שלי</h1>
            <div class="hero-subtitle">✦ &nbsp; בחר הזמנה לעריכה &nbsp; ✦</div>
        </div>

        <div class="gold-divider"></div>

        <%-- הודעת סטטוס מעל שני המצבים - נראית גם במצב רשימה וגם במצב עריכה --%>
        <div style="text-align:center; margin-top:15px;">
            <asp:Label ID="LblMessage" runat="server" Font-Bold="true"></asp:Label>
        </div>

        <%-- ============ מצב 1: רשימת ההזמנות העתידיות ============
             לחיצה על "ערוך" ליד הזמנה מחליפה את הפאנל הזה לפאנל העריכה
             באותו דף - בלי redirect לדף אחר. --%>
        <asp:Panel ID="PnlList" runat="server">
            <div class="bookings-container">
                <asp:GridView ID="GridBookings" runat="server"
                    AutoGenerateColumns="True"
                    AutoGenerateSelectButton="True"
                    SelectText="✏️ ערוך"
                    OnSelectedIndexChanged="GridBookings_SelectedIndexChanged"
                    Width="100%"
                    CssClass="times-grid">
                </asp:GridView>

                <asp:Panel ID="PnlEmpty" runat="server" Visible="false">
                    <div class="empty-message">
                        <span class="empty-icon">📭</span>
                        <p>אין לך הזמנות עתידיות.</p>
                        <p><a href="Catalog.aspx">לחץ כאן להזמנת מסעדה</a></p>
                    </div>
                </asp:Panel>
            </div>
        </asp:Panel>

        <%-- ============ מצב 2: טופס עריכת הזמנה בודדת ============
             מוצג רק אחרי לחיצה על "ערוך" ברשימה. הקוד מאחורה מטפל
             בעדכון התאריך/השעה/הסועדים או במחיקה. --%>
        <asp:Panel ID="PnlEdit" runat="server" Visible="false">
            <div class="container">
                <h2>עריכת הזמנה</h2>
                <div class="subtitle">✦ &nbsp; עדכון פרטי ההזמנה &nbsp; ✦</div>

                <div class="res-header">
                    🍽️ <asp:Label ID="LblResName" runat="server"></asp:Label>
                </div>

                <div class="form-input-group">
                    <label>תאריך חדש:</label>
                    <asp:TextBox ID="TxtDate" runat="server" CssClass="update-input" TextMode="Date"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidatorDate" runat="server"
                        ControlToValidate="TxtDate"
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
                    <asp:TextBox ID="TxtNumGuests" runat="server" CssClass="update-input" TextMode="Number"></asp:TextBox>
                </div>

                <asp:Button ID="BtnCheckAvailability" runat="server" Text="🔍 בדוק שעות פנויות"
                    OnClick="BtnCheckAvailability_Click" CssClass="btn-main" />

                <span class="times-label">בחר שעה חדשה:</span>
                <asp:GridView ID="GridTimes" runat="server"
                    AutoGenerateColumns="True"
                    AutoGenerateSelectButton="True"
                    SelectText="בחר"
                    OnSelectedIndexChanged="GridTimes_SelectedIndexChanged"
                    Width="100%"
                    Visible="false"
                    CssClass="times-grid">
                </asp:GridView>

                <asp:Button ID="BtnDelete" runat="server" Text="❌ ביטול הזמנה (מחיקה)"
                    OnClick="BtnDelete_Click" CssClass="btn-delete"
                    OnClientClick="return confirm('האם אתה בטוח שברצונך לבטל את ההזמנה?');" />
            </div>
        </asp:Panel>

        <%-- ============ מצב 3: תשאול הסעה אחרי עדכון הזמנה ============
             מוצג אחרי שהזמנה עודכנה לשעה חדשה. שואל אם הלקוח רוצה הסעה
             למועד החדש. "לא" - מבטל את ההסעה הישנה אצל הספק.
             "כן" - מבטל את הישנה ומזמין חדשה לפי הכתובת והשעה החדשות. --%>
        <asp:Panel ID="PnlTaxi" runat="server" Visible="false">
            <div class="container">
                <asp:Panel ID="TaxiPanel" runat="server" CssClass="taxi-panel">
                    <asp:Label ID="LblTaxiQuestion" runat="server" CssClass="taxi-question"></asp:Label>

                    <asp:Button ID="BtnTaxiYes" runat="server" Text="כן, עדכנו לי הסעה"
                        OnClick="BtnTaxiYes_Click" CssClass="btn-success" />
                    <asp:Button ID="BtnTaxiNo" runat="server" Text="לא, בטלו את ההסעה"
                        OnClick="BtnTaxiNo_Click" CssClass="btn-secondary" />

                    <asp:Panel ID="AddressPanel" runat="server" Visible="false" CssClass="address-panel">
                        <div class="address-title">📍 כתובת איסוף לנהג</div>

                        <div class="form-input-group">
                            <label>עיר:</label>
                            <asp:TextBox ID="TxtCity" runat="server" CssClass="booking-input" list="cityList" placeholder="התחל להקליד שם עיר ובחר מהרשימה"></asp:TextBox>
                            <%-- datalist נותן שדה עם חיפוש תוך כדי הקלדה. הערים נטענות מהמסד
                                 דרך LitCities ב-Page_Load. עוזר למי שלא בטוח באיות או לערים עם כמה נוסחים. --%>
                            <datalist id="cityList"><asp:Literal ID="LitCities" runat="server"></asp:Literal></datalist>
                        </div>

                        <div class="form-input-group">
                            <label>רחוב:</label>
                            <asp:TextBox ID="TxtStreet" runat="server" CssClass="booking-input" placeholder="לדוגמה: דיזנגוף"></asp:TextBox>
                        </div>

                        <div class="form-input-group">
                            <label>מספר בית:</label>
                            <asp:TextBox ID="TxtHouseNum" runat="server" CssClass="booking-input" TextMode="Number" placeholder="לדוגמה: 12"></asp:TextBox>
                        </div>

                        <asp:Button ID="BtnConfirmAddress" runat="server" Text="✓ אישור כתובת ועדכון הסעה"
                            OnClick="BtnConfirmAddress_Click" CssClass="btn-success-full" />

                        <br /><br />
                        <asp:Label ID="LblAddressError" runat="server" ForeColor="#ff8888" Font-Bold="True"></asp:Label>
                    </asp:Panel>

                    <asp:Label ID="LblTaxiResult" runat="server" CssClass="taxi-result"></asp:Label>
                </asp:Panel>
            </div>
        </asp:Panel>

        <div class="nav-bar">
            <a href="PersonalArea.aspx" class="nav-btn">← חזרה לאזור האישי</a>
        </div>

        <div class="footer">✦ &nbsp; EatIt &copy; 2025 &nbsp; ✦</div>

    </form>
</body>
</html>
