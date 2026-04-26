<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataList.aspx.cs" Inherits="ArielProject.DataList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Restaurants List</title>
</head>
<body>
    <form id="form1" runat="server">


        <asp:DataList ID="DataList1" runat="server">
            <ItemTemplate>
<%# DataBinder.Eval(Container.DataItem, "MyRestaurants") %>
            </ItemTemplate>
        </asp:DataList>

    </form>
</body>
</html>
