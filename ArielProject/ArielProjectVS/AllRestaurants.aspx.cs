using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;

namespace ArielProject
{
    public class RestaurantCard
    {
        public string Name { get; set; }
        public string EncodedName { get; set; }   // לכתובת ה-URL (כדי שרווחים יקודדו)
        public string Region { get; set; }
        public string FoodType { get; set; }
    }

    public partial class AllRestaurants : System.Web.UI.Page
    {
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\DBusers1.accdb";

        protected void Page_Load(object sender, EventArgs e)
        {
            // אימות 1: המשתמש חייב להיות מחובר
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // אימות 2: רק מנהל מערכת רואה את הדף הזה
            if (Session["Admin"] == null)
            {
                Response.Redirect("HomePage.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            if (!IsPostBack)
            {
                LoadRestaurants();
            }
        }

        private void LoadRestaurants()
        {
            var list = new List<RestaurantCard>();

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string sql = "SELECT Restaurants, Region, FoodType FROM MyRestaurants ORDER BY Restaurants";
                OleDbCommand cmd = new OleDbCommand(sql, con);

                con.Open();
                OleDbDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    string name = r["Restaurants"].ToString();
                    list.Add(new RestaurantCard
                    {
                        Name = name,
                        EncodedName = HttpUtility.UrlEncode(name),
                        Region = r["Region"].ToString(),
                        FoodType = r["FoodType"].ToString()
                    });
                }
            }

            if (list.Count == 0)
            {
                RepeaterRestaurants.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                RepeaterRestaurants.DataSource = list;
                RepeaterRestaurants.DataBind();
            }
        }
    }
}
