using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    // הוסרה המחלקה BookingItem עם properties (get; set;) -
    // היא היתה תכנות מונחה-עצמים שלא נלמד בתיכון.
    // במקומה משתמשים ב-DataTable רגיל.

    public partial class MyBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // בדיקת התחברות - אם אין משתמש או טלפון בסשן, חוזרים למסך התחברות
            if (Session["User"] == null || Session["Phone"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            // רק בטעינה הראשונה של הדף טוענים את ההזמנות (לא בכל postback)
            if (!IsPostBack)
            {
                LoadFutureBookings();
            }
        }

        // טוען מהמסד את ההזמנות העתידיות של המשתמש ומציג אותן בטבלה
        private void LoadFutureBookings()
        {
            // מחרוזת חיבור - הוחלף AppDomain.CurrentDomain.BaseDirectory
            // ב-Server.MapPath שמשמש בשאר הדפים. סגנון אחיד.
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            // השאילתה - הוחלפו פרמטרים (?) בשרשור מחרוזות,
            // אותו סגנון כמו בשאר הפרוייקט.
            // באקסס תאריך מוקף בסולמיות (#) במקום בגרשיים.
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string sql = "SELECT Restaurant, InvDate, InvTime, NumGuest, TableType " +
                         "FROM MyBooking " +
                         "WHERE PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND InvDate >= #" + today + "# " +
                         "ORDER BY InvDate, InvTime";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            // הוחלפה List<BookingItem> ב-DataTable - טבלה רגילה עם 5 עמודות.
            // ה-DataTable יוצג אוטומטית ב-GridView.
            // הוסר גם בלוק using(...) - חיבור רגיל שנסגר ידנית.
            DataTable dt = new DataTable();
            dt.Columns.Add("מסעדה");
            dt.Columns.Add("תאריך");
            dt.Columns.Add("שעה");
            dt.Columns.Add("סועדים");
            dt.Columns.Add("סוג שולחן");

            // לולאה שעוברת על כל השורות שחזרו מהמסד ומוסיפה אותן לטבלה
            while (reader.Read())
            {
                string restaurant = reader["Restaurant"].ToString();
                // ממירים את התאריך מהמסד לפורמט yyyy-MM-dd
                string date = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                string time = reader["InvTime"].ToString();
                string guests = reader["NumGuest"].ToString();
                // ממירים את סוג השולחן לעברית עם פונקציית עזר
                string tableType = TranslateTableType(reader["TableType"].ToString());

                dt.Rows.Add(restaurant, date, time, guests, tableType);
            }

            con.Close();

            // אם אין הזמנות עתידיות - מסתירים את הטבלה ומציגים את פאנל ה"ריק"
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

        // טיפול בלחיצה על "Select" ליד הזמנה -
        // מוציא את התאריך והשעה ומעביר את המשתמש לדף עריכת ההזמנה.
        // החלפנו את ה-Repeater עם הקישור "ערוך הזמנה" ב-GridView עם
        // כפתור Select אוטומטי (AutoGenerateSelectButton) - יותר פשוט.
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // אינדקסים של התאים בשורה הנבחרת:
            // Cells[0] = כפתור הבחירה (Select) שנוצר אוטומטית
            // Cells[1] = מסעדה
            // Cells[2] = תאריך
            // Cells[3] = שעה
            // Cells[4] = סועדים
            // Cells[5] = סוג שולחן
            string date = GridView1.SelectedRow.Cells[2].Text;
            string time = GridView1.SelectedRow.Cells[3].Text;
            Response.Redirect("Update.aspx?date=" + date + "&time=" + time);
        }

        // ממיר את שם סוג השולחן מאנגלית לתיאור עברי.
        // הוחלף switch בשרשרת if/else if כדי להישאר ברמת תיכון בסיסית.
        private string TranslateTableType(string type)
        {
            if (type == "Small")
                return "קטן (עד 2 סועדים)";
            else if (type == "Medium")
                return "בינוני (3-4 סועדים)";
            else if (type == "Large")
                return "גדול (5+ סועדים)";
            else
                return type;
        }
    }
}
