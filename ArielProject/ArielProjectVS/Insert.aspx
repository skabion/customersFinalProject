<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="ArielProject.Insert" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>הרשמה</title>
</head>
<body>
    <div style="text-align:center">
        <form id="form1" runat="server">
            
            <asp:TextBox ID="SignUp_FullName" runat="server"></asp:TextBox> שם מלא
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="SignUp_FullName" 
                ErrorMessage="לא הוזן שם מלא" 
                ValidationGroup="SignUp" 
                Display="Dynamic" 
                CssClass="validator-msg" />
            <asp:RegularExpressionValidator ID="SignUp_FullName_RegularExpressionValidator" runat="server" 
                ControlToValidate="SignUp_FullName" 
                ValidationExpression="^[A-Z][a-zA-Z]*(\s+[A-Z][a-zA-Z]*)+$" 
                ErrorMessage="Invalid Name (Requires: English letters only, at least 2 words, each starting with a Capital letter)" 
                ValidationGroup="SignUp" 
                Display="Dynamic" 
                CssClass="validator-msg" />
            <p></p>

            <asp:TextBox ID="SignUp_Password" runat="server" TextMode="Password"></asp:TextBox> סיסמה
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="SignUp_Password" 
                ErrorMessage="לא הוזנה סיסמה" 
                ValidationGroup="SignUp" 
                Display="Dynamic" 
                CssClass="validator-msg" />
            <asp:RegularExpressionValidator ID="SignUp_Password_RegularExpressionValidator" runat="server" 
                ControlToValidate="SignUp_Password" 
                ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&])[A-Za-z\d!@#$%^&]{6,}$" 
                ErrorMessage="Password too weak (Requires: 1 Upper, 1 Lower, 1 Number, 1 Symbol, Min 6 chars)" 
                ValidationGroup="SignUp" 
                Display="Dynamic" 
                CssClass="validator-msg" />
            <p></p>

            <asp:TextBox ID="SignUp_Phone" runat="server"></asp:TextBox> טלפון
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="SignUp_Phone" 
                ErrorMessage="לא הוזן טלפון" 
                ValidationGroup="SignUp" 
                Display="Dynamic" 
                CssClass="validator-msg" />
            <asp:RegularExpressionValidator ID="SignUp_Phone_RegularExpressionValidator" runat="server" 
                ControlToValidate="SignUp_Phone" 
                ValidationExpression="^05[02345]\d{7}$" 
                ErrorMessage="Invalid phone number (Requires: 10 digits starting with 050, 052, 053, 054, or 055)" 
                ValidationGroup="SignUp" 
                Display="Dynamic" 
                CssClass="validator-msg" />
            <p>&nbsp;</p>

            <div style="text-align:center">
                <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownListArea_SelectedIndexChanged">
                </asp:DropDownList>
                אזור
            </div>
            
            <br /><br />

            <div style="text-align:center">
                העדפות:&nbsp; <br />
                <asp:CheckBox ID="CheckBoxVegan" runat="server" Text="Vegan" Style="direction:rtl;" />
                <asp:CheckBox ID="CheckBoxKosher" runat="server" Text="Kosher" Style="direction:rtl;" />
                <asp:CheckBox ID="CheckBoxVegetarian" runat="server" Text="Vegetarian" Style="direction:rtl;" />
            </div>
            
            <br /><br />

            אלרגיות:<br />
            <asp:CheckBox ID="CheckBoxGluten" runat="server" Text="Gluten" />
            <asp:CheckBox ID="CheckBoxPeanuts" runat="server" Text="Peanuts" />
            <asp:CheckBox ID="CheckBoxTreeNuts" runat="server" Text="TreeNuts" />
            <asp:CheckBox ID="CheckBoxFish" runat="server" Text="Fish" />
            <asp:CheckBox ID="CheckBoxSesame" runat="server" Text="Sesame" />
            <asp:CheckBox ID="CheckBoxMilk" runat="server" Text="Milk" />
    
            <p>
                <asp:Button ID="AddUser" runat="server" Text="הרשמה" OnClick="AddUser_Click" ValidationGroup="SignUp" />
            </p>
            
        </form> 
    </div>
</body>
</html>