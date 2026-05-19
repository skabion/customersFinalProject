<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataList.aspx.cs" Inherits="ArielProject.DataList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Restaurants List</title>
</head>
<body>
    <form id="form1" runat="server">


        <%-- תוקנו 2 דברים:
             1. הוחלף תחביר DataBinder.Eval(Container.DataItem, "x") ב-Eval("x") פשוט יותר
             2. שם העמודה היה "MyRestaurants" (שם הטבלה, לא קיים כעמודה) - תוקן ל-"Restaurants" --%>
        <asp:DataList ID="DataList1" runat="server">
            <ItemTemplate>
                <%# Eval("Restaurants") %>
            </ItemTemplate>
        </asp:DataList>

    </form>
</body>
</html>
