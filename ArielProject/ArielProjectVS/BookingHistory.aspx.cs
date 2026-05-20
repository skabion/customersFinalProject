using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    public partial class BookingHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // בדיקת התחברות
            if (Session["User"] == null || Session["Phone"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            if (!IsPostBack)
            {
                LoadHistory();
            }
        }

        // טיפול בשינוי המיון ב-DropDown - טוענים מחדש את הטבלה ממוינת
        protected void DdlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHistory();
        }

        // טוען את היסטוריית ההזמנות של המשתמש ומציג אותן ב-GridView.
        // הכל בשאילתה אחת: INNER JOIN מצרף את פרטי המסעדה (FoodType + Region)
        // לכל הזמנה, IIF מתרגם את סוג השולחן לעברית, ו-ORDER BY ממיין לפי
        // הבחירה ב-DropDown - כולל לפי FoodType ו-Region.
        // הוסרו: שאילתה שנייה בלולאה (per-row), פונקציית BubbleSortByColumn
        // ופונקציית TranslateTableType - הכל מטופל ב-SQL.
        private void LoadHistory()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            // קביעת ORDER BY לפי בחירת המשתמש. כל 4 האפשרויות נעשות ב-SQL.
            string sortKey = DdlSort.SelectedValue;
            string orderBy;
            if (sortKey == "DateAsc")
                orderBy = "b.InvDate ASC, b.InvTime ASC";
            else if (sortKey == "FoodType")
                orderBy = "r.FoodType ASC, b.InvDate DESC";
            else if (sortKey == "Region")
                orderBy = "r.Region ASC, b.InvDate DESC";
            else
                orderBy = "b.InvDate DESC, b.InvTime DESC";

            // INNER JOIN מחבר את MyBooking ל-MyRestaurants לפי שם המסעדה.
            // b ו-r הם שמות קצרים (aliases) לטבלאות, כדי שלא נצטרך לכתוב שם מלא.
            // IIF מקונן בתוך ה-SELECT = if/else בתוך SQL.
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string sql = "SELECT b.Restaurant, b.InvDate, b.InvTime, b.NumGuest, " +
                         "IIF(b.TableType='Small','קטן (עד 2 סועדים)'," +
                         "IIF(b.TableType='Medium','בינוני (3-4 סועדים)','גדול (5+ סועדים)')) AS TableTypeHe, " +
                         "r.FoodType, r.Region " +
                         "FROM MyBooking AS b INNER JOIN MyRestaurants AS r " +
                         "ON b.Restaurant = r.Restaurants " +
                         "WHERE b.PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND b.InvDate < #" + today + "# " +
                         "ORDER BY " + orderBy;

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            // DataTable עם 7 עמודות בעברית - יוצג אוטומטית ב-GridView
            DataTable dt = new DataTable();
            dt.Columns.Add("מסעדה");
            dt.Columns.Add("תאריך");
            dt.Columns.Add("שעה");
            dt.Columns.Add("סועדים");
            dt.Columns.Add("סוג שולחן");
            dt.Columns.Add("סוג מטבח");
            dt.Columns.Add("אזור");

            while (reader.Read())
            {
                dt.Rows.Add(
                    reader["Restaurant"].ToString(),
                    Convert.ToDateTime(reader["InvDate"]).ToString("dd/MM/yyyy"),
                    reader["InvTime"].ToString(),
                    reader["NumGuest"].ToString(),
                    reader["TableTypeHe"].ToString(),
                    reader["FoodType"].ToString(),
                    reader["Region"].ToString()
                );
            }
            con.Close();

            // הצגת תוצאות או הודעת "אין הזמנות"
            if (dt.Rows.Count == 0)
            {
                GridView1.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                GridView1.Visible = true;
                PnlEmpty.Visible = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
    }
}
