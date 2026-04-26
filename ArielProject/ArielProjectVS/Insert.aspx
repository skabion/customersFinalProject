<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="ArielProject.Insert" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
            <div style="text-align:center">

    <form id="form1" runat="server">
        
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="לא הוזן שם מלא"></asp:RequiredFieldValidator>

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>שם מלא

        <p>

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2" ErrorMessage="לא הוזנה סיסמה"></asp:RequiredFieldValidator>

            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox> סיסמה
        </p>

        <p>

            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox3" ErrorMessage="לא הוזן טלפון"></asp:RequiredFieldValidator>

    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox> טלפון
        </p>
        <p>
            &nbsp;</p>
       

        <div style="text-align:center">

            <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownListArea_SelectedIndexChanged">
            </asp:DropDownList>
        אזור</div>
            
            
        <br />
        <br />


            <div style="text-align:center">
            העדפות:&nbsp; <br />
           <asp:CheckBox ID="CheckBoxVegan" runat="server" Text="Vegan" Style="direction:rtl;" />
            <asp:CheckBox ID="CheckBoxKosher" runat="server" Text="Kosher" Style="direction:rtl;" />
            <asp:CheckBox ID="CheckBoxVegetarian" runat="server" Text="Vegetarian" Style="direction:rtl;" />
            </div>
            

            <br />
            <br />


            אלרגיות:<br />
            <asp:CheckBox ID="CheckBoxGluten" runat="server" Text="Gluten" />
            <asp:CheckBox ID="CheckBoxPeanuts" runat="server" Text="Peanuts" />
            <asp:CheckBox ID="CheckBoxTreeNuts" runat="server" Text="TreeNuts" />
            <asp:CheckBox ID="CheckBoxFish" runat="server" Text="Fish" />
            <asp:CheckBox ID="CheckBoxSesame" runat="server" Text="Sesame" />
            <asp:CheckBox ID="CheckBoxMilk" runat="server" Text="Milk" />
   
        <p>
            <asp:Button ID="AddUser" runat="server" Text="הרשמה" OnClick="AddUser_Click" />
            </p>
            
    </form> 

            </div>
</body>
</html>
