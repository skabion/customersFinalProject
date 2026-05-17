using System;
using System.Web.UI;

namespace ArielProject
{
    public partial class PersonalArea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();
        }
    }
}
