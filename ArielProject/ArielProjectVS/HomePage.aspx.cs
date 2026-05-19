using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
// הוסרו using System.Collections.Generic ו-using System.Linq - לא היו בשימוש

namespace ArielProject
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLoggedIn = Session["User"] != null;

            if (isLoggedIn)
                LblUserName.Text = Session["User"].ToString();
            else
                LblUserName.Text = "אורח";

            LnkRegister.Visible = !isLoggedIn;
            LnkLogin.Visible = !isLoggedIn;
            LnkPersonalArea.Visible = isLoggedIn;
            // כפתור "דף מנהל מסעדה" - מופיע גם למנהל מסעדה וגם למנהל מערכת
            LnkRestaurantAdmin.Visible = isLoggedIn && (Session["RestaurantAdmin"] != null || Session["Admin"] != null);
            BtnLogout.Visible = isLoggedIn;
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("HomePage.aspx");
        }
    }
}