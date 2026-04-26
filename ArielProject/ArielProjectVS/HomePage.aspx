<%@ Page Title="דף הבית" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="ArielProject.HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- כל מה שכתוב כאן ייכנס לתוך ה"חור" שקראנו לו MainContent ב-Master Page --%>
    <div style="text-align:center; padding: 50px;">
        <h1>ברוכים הבאים לאתר המסעדות שלי!</h1>
        <p>כאן תוכלו למצוא את המסעדות הטובות ביותר ולהזמין מקום בקליק.</p>
        
        <br />
        <a href="Catalog.aspx" style="padding:10px 20px; background-color:#333; color:white; text-decoration:none; border-radius:5px;">
            לצפייה בקטלוג המסעדות
        </a>
    </div>
</asp:Content>