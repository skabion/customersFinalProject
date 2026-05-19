using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    // הוסרה המחלקה HistoryItem עם properties - הוחלפה ב-DataTable

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

        // טוען את היסטוריית ההזמנות של המשתמש ומציג אותן ב-GridView
        private void LoadHistory()
        {
            // הוחלף AppDomain.CurrentDomain.BaseDirectory ב-Server.MapPath -
            // סגנון אחיד לכל הפרוייקט.
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            // קביעת סדר ה-ORDER BY בשאילתה (רק לפי תאריך).
            // עבור FoodType ו-Region נמיין בקוד C# בהמשך.
            string sortKey = DdlSort.SelectedValue;
            string orderBy;
            if (sortKey == "DateAsc")
                orderBy = " ORDER BY InvDate ASC, InvTime ASC";
            else
                orderBy = " ORDER BY InvDate DESC, InvTime DESC";

            // שאילתה 1: ההזמנות של המשתמש שתאריכן קטן מהיום (היסטוריה).
            // הוחלפו פרמטרים (?) בשרשור מחרוזות - אותו סגנון כמו בשאר הדפים.
            // הוסר ה-INNER JOIN - נביא את פרטי המסעדה בשאילתה נפרדת בהמשך.
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string sql = "SELECT Restaurant, InvDate, InvTime, NumGuest, TableType " +
                         "FROM MyBooking " +
                         "WHERE PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND InvDate < #" + today + "#" +
                         orderBy;

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            // הוחלפה List<HistoryItem> ב-DataTable - 7 עמודות.
            // FoodType ו-אזור יתחילו כריקות ונמלא אותן בשאילתה השנייה.
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
                string restName = reader["Restaurant"].ToString();
                string dateStr = Convert.ToDateTime(reader["InvDate"]).ToString("dd/MM/yyyy");
                string time = reader["InvTime"].ToString();
                string guestsStr = reader["NumGuest"].ToString();
                string tableType = TranslateTableType(reader["TableType"].ToString());

                // מוסיפים שורה עם שני התאים האחרונים ריקים (FoodType ו-אזור)
                dt.Rows.Add(restName, dateStr, time, guestsStr, tableType, "", "");
            }
            reader.Close();
            con.Close();

            // שאילתה 2: עבור כל הזמנה, מביאים את פרטי המסעדה (FoodType ו-Region)
            // החלפנו את ה-INNER JOIN בלולאה שמריצה שאילתה לכל הזמנה.
            // הוסר גם בלוק using(...) - חיבור רגיל שנסגר ידנית.
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string restName = dt.Rows[i]["מסעדה"].ToString();
                string sql2 = "SELECT FoodType, Region FROM MyRestaurants WHERE Restaurants = '" + restName + "'";
                OleDbCommand cmd2 = new OleDbCommand(sql2, con);
                con.Open();
                OleDbDataReader reader2 = cmd2.ExecuteReader();
                if (reader2.Read())
                {
                    dt.Rows[i]["סוג מטבח"] = reader2["FoodType"].ToString();
                    dt.Rows[i]["אזור"] = reader2["Region"].ToString();
                }
                reader2.Close();
                con.Close();
            }

            // מיון לפי סוג מטבח או אזור (אם נבחר) - באמצעות מיון בועות בקוד.
            // SQL לא יכול לעזור פה כי הוא לא רואה את העמודות האלה (אין JOIN).
            if (sortKey == "FoodType")
            {
                BubbleSortByColumn(dt, "סוג מטבח");
            }
            else if (sortKey == "Region")
            {
                BubbleSortByColumn(dt, "אזור");
            }

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

        // מיון בועות (Bubble Sort) על שורות ה-DataTable לפי עמודה נתונה.
        // עוברים על השורות בזוגות סמוכים ומחליפים אם הראשון גדול מהשני
        // (כלומר לא בסדר אלפביתי). חוזרים על זה N פעמים עד שהכל ממוין.
        private void BubbleSortByColumn(DataTable dt, string columnName)
        {
            int n = dt.Rows.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    string val1 = dt.Rows[j][columnName].ToString();
                    string val2 = dt.Rows[j + 1][columnName].ToString();

                    // CompareTo מחזיר מספר חיובי אם val1 גדול מ-val2 אלפביתית
                    if (val1.CompareTo(val2) > 0)
                    {
                        // החלפה: שומרים את כל הערכים של שורה j בעזר,
                        // ואז מחליפים בין השתיים. ItemArray הוא מערך עם כל
                        // ערכי התאים של השורה.
                        object[] temp = dt.Rows[j].ItemArray;
                        dt.Rows[j].ItemArray = dt.Rows[j + 1].ItemArray;
                        dt.Rows[j + 1].ItemArray = temp;
                    }
                }
            }
        }

        // ממיר את שם סוג השולחן מאנגלית לתיאור עברי.
        // הוחלף switch בשרשרת if/else if כדי להישאר ברמת תיכון.
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
