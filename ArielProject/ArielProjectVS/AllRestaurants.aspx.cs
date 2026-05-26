using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{

    public partial class AllRestaurants : System.Web.UI.Page
    {
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

        // טוען את כל המסעדות מהמסד ומציג אותן ב-GridView
        private void LoadRestaurants()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT Restaurants, Region, FoodType FROM MyRestaurants ORDER BY Restaurants";
            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("שם מסעדה");
            dt.Columns.Add("אזור");
            dt.Columns.Add("סוג מטבח");

            // קוראים כל שורה מהמסד ומוסיפים לטבלה.
            while (reader.Read())
            {
                string name = reader["Restaurants"].ToString();
                string region = reader["Region"].ToString();
                string foodType = reader["FoodType"].ToString();

                dt.Rows.Add(name, region, foodType);
            }
            con.Close();

            // אם אין מסעדות - מציגים פאנל ריק
            if (dt.Rows.Count == 0)
            {
                GridView1.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        // לחיצה על מסעדה - מעבירים את המנהל לדף הסטטיסטיקות של המסעדה.
        // מחליף את ה-Repeater עם הקישור Eval ב-GridView עם כפתור Select.
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cells[0] = כפתור הבחירה, Cells[1] = שם המסעדה
            string name = GridView1.SelectedRow.Cells[1].Text;

            // קידוד פשוט של רווחים לפורמט בטוח ל-URL (במקום HttpUtility.UrlEncode).
            // לדוגמה: "La Lush" יהפוך ל-"La%20Lush" כדי שה-URL לא יישבר.
            string encoded = name.Replace(" ", "%20");

            Response.Redirect("RestaurantAdmin.aspx?restaurant=" + encoded);
        }
    }
}
