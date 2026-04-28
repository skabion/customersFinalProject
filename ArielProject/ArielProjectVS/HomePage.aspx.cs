using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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