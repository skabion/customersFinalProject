<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="ArielProject.Catalog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div dir="rtl" style="text-align: center; margin-top: 30px;">
    <h2>חיפוש מסעדות</h2>
    
    בחר אזור: 
    <asp:DropDownList ID="DdlRegion" runat="server">
        <asp:ListItem Text="הכל" Value="הכל"></asp:ListItem>
        <asp:ListItem Text="צפון" Value="Tzafon"></asp:ListItem>
        <asp:ListItem Text="מרכז" Value="Merkaz"></asp:ListItem>
        <asp:ListItem Text="דרום" Value="Darom"></asp:ListItem>
    </asp:DropDownList>
    <br /><br />
    
    סוג מטבח:
    <asp:DropDownList ID="DdlType" runat="server">
        <asp:ListItem Text="הכל" Value="הכל"></asp:ListItem>
        <asp:ListItem Text="איטלקי" Value="Italian"></asp:ListItem>
        <asp:ListItem Text="בשרי" Value="Meat"></asp:ListItem>
        <asp:ListItem Text="אסייתי" Value="Asian"></asp:ListItem>
        <asp:ListItem Text="בראסרי" Value="Brasserie"></asp:ListItem>
        <asp:ListItem Text="קינוחים" Value="Deserts"></asp:ListItem>
    </asp:DropDownList>
    <br /><br />
    <asp:CheckBox ID="ChkKosher" runat="server" Text="הצג מסעדות כשרות בלבד" />
    <asp:CheckBox ID="ChkReplacementMeals" runat="server" Text="תחלופה לאלרגנים בלבד" /><br />
    <asp:Button ID="BtnSearch" runat="server" Text="חפש מסעדות" OnClick="BtnSearch_Click" />
    <br /><br />

<asp:DataList ID="DataListRestaurants" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
    <ItemTemplate>
        <div style="border: 1px solid #cccccc; border-radius: 8px; padding: 15px; margin: 10px; width: 250px; text-align: center; background-color: #f9f9f9; box-shadow: 2px 2px 5px rgba(0,0,0,0.1);">
            
            <h3 style="color: #333;"><%# Eval("Restaurants") %></h3> 
            
            <p><b>אזור:</b> <%# Eval("Region") %></p>
            <p><b>סוג מטבח:</b> <%# Eval("FoodType") %></p>
            <p><b>כשרות:</b> <%# Eval("Kosher") %></p>
            <p><b>תחלופה לאלרגנים:</b> <%# Eval("ReplacementMeals") %></p>
            <a href='Booking.aspx?res=<%# Eval("Restaurants") %>' style="display: inline-block; padding: 8px 15px; background-color: #5D7B9D; color: white; text-decoration: none; border-radius: 5px; font-weight: bold;">הזמן שולחן</a>

        </div>
    </ItemTemplate>
</asp:DataList>
    <br />
</div>
    </form>
</body>
</html>
