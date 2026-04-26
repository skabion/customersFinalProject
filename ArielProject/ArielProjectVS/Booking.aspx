<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="ArielProject.Booking"%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>הזמנת מקום</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            direction: rtl;
            text-align: right;
            padding: 20px;
        }

        .booking-container {
            max-width: 400px;
            margin: 0 auto;
            border: 1px solid #ddd;
            padding: 20px;
            border-radius: 8px;
        }

        .input-group {
            margin-bottom: 15px;
        }

        .input-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .input-control {
            width: 100%;
            padding: 8px;
            box-sizing: border-box;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        /* עיצוב לכפתור של שעה פנויה */
        .available-time {
            background-color: white;
            color: black;
            border: 1px solid #ccc;
            padding: 10px 20px;
            margin: 5px 0;
            cursor: pointer;
            font-size: 16px;
            width: 100%;
            transition: 0.3s;
        }

        .available-time:hover {
            background-color: #f0f0f0;
        }

        /* עיצוב לכפתור של שעה תפוסה (אפור ולא לחיץ) */
        .unavailable-time {
            background-color: #f5f5f5;
            color: #b0b0b0;
            border: 1px solid #e0e0e0;
            padding: 10px 20px;
            margin: 5px 0;
            font-size: 16px;
            width: 100%;
        }

        .res-name {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="booking-container">
            <div class="res-name">
                <asp:Label ID="LblResName" runat="server" Text="שם המסעדה"></asp:Label>
            </div>

            <div class="input-group">
                <label>תאריך:</label>
                <asp:TextBox ID="TxtDate" runat="server" CssClass="input-control" TextMode="Date"></asp:TextBox>
            </div>

            <div class="input-group">
                <label>מספר סועדים:</label>
                <asp:TextBox ID="TxtGuests" runat="server" CssClass="input-control" TextMode="Number"></asp:TextBox>
            </div>

            <asp:Button ID="BtnCheckTimes" runat="server" Text="מצאו לי שולחן" 
                OnClick="BtnCheckTimes_Click" BackColor="Black" ForeColor="White" 
                Width="100%" Height="40px" Font-Bold="True" />

            <br /><br />

            <asp:Repeater ID="RepeaterTimes" runat="server" OnItemCommand="RepeaterTimes_ItemCommand">
                <ItemTemplate>
                    <div>
                        <asp:Button ID="BtnTime" runat="server" 
                            Text='<%# Eval("TimeStr") %>' 
                            CommandArgument='<%# Eval("TimeStr") %>' 
                            Enabled='<%# Convert.ToBoolean(Eval("IsAvailable")) %>'
                            CssClass='<%# Convert.ToBoolean(Eval("IsAvailable")) ? "available-time" : "unavailable-time" %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <br />
            <asp:Label ID="LblMsg" runat="server" Font-Bold="True"></asp:Label>
        </div>
    </form>
</body>
</html>