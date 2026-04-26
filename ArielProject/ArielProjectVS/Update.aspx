<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="ArielProject.Update" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>עדכון הזמנה - Makeover</title>
    <style>
        body { font-family: 'Segoe UI', Arial; direction: rtl; text-align: right; background-color: #f4f4f4; padding: 20px; }
        .container { max-width: 500px; margin: 0 auto; background: white; padding: 30px; border-radius: 15px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); }
        h2 { text-align: center; color: #333; }
        .search-section { border-bottom: 2px solid #eee; margin-bottom: 20px; padding-bottom: 20px; text-align: center; }
        .res-header { background: #000; color: #fff; padding: 10px; border-radius: 8px; text-align: center; margin-bottom: 20px; font-size: 1.2em; }
        .input-group { margin-bottom: 15px; }
        label { display: block; font-weight: bold; margin-bottom: 5px; }
        .input-control { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 5px; box-sizing: border-box; }
        
        /* עיצוב כפתורי השעות מה-Booking */
        .available-time { background-color: white; color: black; border: 1px solid #ccc; padding: 10px; margin: 5px 0; cursor: pointer; width: 100%; border-radius: 5px; transition: 0.3s; }
        .available-time:hover { background-color: #e9e9e9; }
        .unavailable-time { background-color: #f5f5f5; color: #b0b0b0; border: 1px solid #e0e0e0; padding: 10px; margin: 5px 0; width: 100%; border-radius: 5px; cursor: not-allowed; }
        
        .btn-main { background: #000; color: #fff; border: none; padding: 12px; width: 100%; border-radius: 5px; cursor: pointer; font-size: 16px; font-weight: bold; }
        .btn-delete { background: #ff4d4d; color: white; border: none; padding: 8px; margin-top: 20px; border-radius: 5px; cursor: pointer; width: 100%; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>ניהול הזמנה קיימת</h2>

            <div class="search-section">
                <label>הזן מספר טלפון לחיפוש:</label>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="input-control" placeholder="למשל: 0501234567"></asp:TextBox>
                <br /><br />
                <asp:Button ID="btnSearch" runat="server" Text="חפש הזמנה" OnClick="btnSearch_Click" CssClass="btn-main" />
            </div>

            <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                <div class="res-header">
                    מסעדה: <asp:Label ID="lblResName" runat="server"></asp:Label>
                </div>

                <div class="input-group">
                    <label>תאריך חדש:</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="input-control" TextMode="Date"></asp:TextBox>
                </div>

                <div class="input-group">
                    <label>כמות אורחים מעודכנת:</label>
                    <asp:TextBox ID="txtNumGuests" runat="server" CssClass="input-control" TextMode="Number"></asp:TextBox>
                </div>

                <asp:Button ID="btnCheckAvailability" runat="server" Text="בדוק שעות פנויות לעדכון" 
                    OnClick="btnCheckAvailability_Click" CssClass="btn-main" BackColor="#4CAF50" />

                <br /><br />
                <label>בחר שעה חדשה:</label>
                <asp:Repeater ID="RepeaterTimes" runat="server" OnItemCommand="RepeaterTimes_ItemCommand">
                    <ItemTemplate>
                        <asp:Button ID="BtnTime" runat="server" 
                            Text='<%# Eval("TimeStr") %>' 
                            CommandArgument='<%# Eval("TimeStr") %>' 
                            Enabled='<%# Convert.ToBoolean(Eval("IsAvailable")) %>'
                            CssClass='<%# Convert.ToBoolean(Eval("IsAvailable")) ? "available-time" : "unavailable-time" %>' />
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Button ID="btnDelete" runat="server" Text="ביטול הזמנה (מחיקה)" OnClick="btnDelete_Click" 
                    CssClass="btn-delete" OnClientClick="return confirm('האם אתה בטוח שברצונך לבטל את ההזמנה?');" />
            </asp:Panel>

            <br />
            <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </form>
</body>
</html>