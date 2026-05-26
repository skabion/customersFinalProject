using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    // דף משולב: גם רשימת ההזמנות העתידיות (PnlList) וגם טופס עריכת הזמנה
    // בודדת (PnlEdit). שני המצבים מתחלפים על אותו דף - בלי redirect.
    // איחד את הדפים MyBookings.aspx ו-Update.aspx שהיו קודם נפרדים.
    public partial class UpdateBookings : System.Web.UI.Page
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

            // קביעת תאריך מינימלי + ערך השוואה ל-Validator (גם בכל postback)
            string todayStr = DateTime.Today.ToString("yyyy-MM-dd");
            TxtDate.Attributes["min"] = todayStr;
            CompareValidatorDate.ValueToCompare = todayStr;

            if (!IsPostBack)
            {
                LoadFutureBookings();
            }
        }

        // ============================================================
        // מצב רשימה (PnlList): ההזמנות העתידיות של המשתמש
        // ============================================================

        // טוען את ההזמנות העתידיות של המשתמש מהמסד ומציג אותן ב-GridBookings
        private void LoadFutureBookings()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

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

            DataTable dt = new DataTable();
            dt.Columns.Add("מסעדה");
            dt.Columns.Add("תאריך");
            dt.Columns.Add("שעה");
            dt.Columns.Add("סועדים");
            dt.Columns.Add("סוג שולחן");

            while (reader.Read())
            {
                string restaurant = reader["Restaurant"].ToString();
                string date = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                string time = reader["InvTime"].ToString();
                string guests = reader["NumGuest"].ToString();
                string tableType = TranslateTableType(reader["TableType"].ToString());

                dt.Rows.Add(restaurant, date, time, guests, tableType);
            }
            con.Close();

            if (dt.Rows.Count == 0)
            {
                GridBookings.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                GridBookings.Visible = true;
                PnlEmpty.Visible = false;
                GridBookings.DataSource = dt;
                GridBookings.DataBind();
            }
        }

        // לחיצה על "ערוך" ליד הזמנה - מחליפים את הפאנל לטופס העריכה
        // וטוענים את פרטי ההזמנה הנבחרת. במקום Redirect לדף נפרד.
        protected void GridBookings_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cells[0]=כפתור, Cells[1]=מסעדה, Cells[2]=תאריך, Cells[3]=שעה
            string date = GridBookings.SelectedRow.Cells[2].Text;
            string time = GridBookings.SelectedRow.Cells[3].Text;

            // מעבר ממצב רשימה למצב עריכה
            PnlList.Visible = false;
            PnlEdit.Visible = true;

            LoadBooking(date, time);
        }

        // ============================================================
        // מצב עריכה (PnlEdit): טופס לעריכת הזמנה בודדת
        // ============================================================

        // טוען את פרטי ההזמנה הקיימת ומציג אותם בטופס
        private void LoadBooking(string date, string time)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection conn = new OleDbConnection(connStr);

            string sql = "SELECT * FROM MyBooking " +
                         "WHERE PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND InvDate = #" + date + "# " +
                         "AND InvTime = '" + time + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                LblResName.Text = reader["Restaurant"].ToString();
                TxtDate.Text = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                TxtNumGuests.Text = reader["NumGuest"].ToString();

                // שומרים ב-Session את מזהי ההזמנה המקורית -
                // נצטרך אותם בעדכון/מחיקה כדי למקם את השורה הנכונה במסד.
                Session["OldDate"] = TxtDate.Text;
                Session["OldTime"] = reader["InvTime"].ToString();

                LblMessage.Text = "ניתן לעדכן פרטים ולבחור שעה חדשה.";
                LblMessage.ForeColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                PnlEdit.Visible = false;
                PnlList.Visible = true;
                LblMessage.Text = "ההזמנה לא נמצאה.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
            }
            conn.Close();
        }

        // לחיצה על "בדוק שעות פנויות" - מאמתים תאריך ובונים את טבלת השעות
        protected void BtnCheckAvailability_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            DateTime chosen;
            try
            {
                chosen = DateTime.Parse(TxtDate.Text);
            }
            catch
            {
                LblMessage.Text = "תאריך לא תקין.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            // הגנה נוספת בצד השרת - גם אם ה-Validator עוקף, אסור תאריך עבר
            if (chosen.Date < DateTime.Today)
            {
                LblMessage.Text = "לא ניתן לעדכן הזמנה לתאריך שעבר.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            GenerateTimeSlots();
        }

        // בונה את טבלת השעות הפנויות ומציג אותה ב-GridTimes
        private void GenerateTimeSlots()
        {
            string res = LblResName.Text;
            string date = TxtDate.Text;
            int guests = int.Parse(TxtNumGuests.Text);

            // קביעת סוג השולחן לפי מספר הסועדים
            string tableType;
            string typeName;
            if (guests <= 2)
            {
                tableType = "SmallTables";
                typeName = "Small";
            }
            else if (guests <= 4)
            {
                tableType = "MediumTables";
                typeName = "Medium";
            }
            else
            {
                tableType = "LargeTables";
                typeName = "Large";
            }

            Session["SelectedTypeUpdate"] = typeName;

            // שולפים מהמסד את מספר השולחנות מהסוג המבוקש
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sqlCap = "SELECT " + tableType + " FROM MyRestaurants WHERE Restaurants = '" + res + "'";
            OleDbCommand cmdCap = new OleDbCommand(sqlCap, con);
            con.Open();
            int totalTables = int.Parse(cmdCap.ExecuteScalar().ToString());
            con.Close();

            DataTable dt = new DataTable();
            dt.Columns.Add("שעה");
            dt.Columns.Add("סטטוס");

            // קובעים את שעות הפתיחה לפי היום בשבוע (זהה לכל המסעדות).
            // 1=ראשון .. 5=חמישי: 9:00-23:00, 6=שישי: 8:00-16:00, 7=שבת: 19:30-23:30
            // מוסיפים 1 ל-DayOfWeek כי הוא מחזיר 0=ראשון, ואנחנו רוצים 1=ראשון.
            DateTime selectedDate = DateTime.Parse(date);
            int dayNum = (int)selectedDate.DayOfWeek + 1;

            int openHour, openMinute, closeHour, closeMinute;
            if (dayNum == 6)
            {
                openHour = 8; openMinute = 0;
                closeHour = 16; closeMinute = 0;
            }
            else if (dayNum == 7)
            {
                openHour = 19; openMinute = 30;
                closeHour = 23; closeMinute = 30;
            }
            else
            {
                openHour = 9; openMinute = 0;
                closeHour = 23; closeMinute = 0;
            }

            // אפשר להזמין רק עד שעתיים לפני הסגירה
            int startMinutes = openHour * 60 + openMinute;
            int endMinutes = closeHour * 60 + closeMinute - 120;

            int currentMinutes = startMinutes;
            while (currentMinutes <= endMinutes)
            {
                int h = currentMinutes / 60;
                int m = currentMinutes % 60;

                string timeStr = "";
                if (h < 10) timeStr = timeStr + "0";
                timeStr = timeStr + h + ":";
                if (m < 10) timeStr = timeStr + "0";
                timeStr = timeStr + m;

                bool isAvail = CheckSpecificTime(timeStr, res, date, totalTables, typeName);

                string statusText;
                if (isAvail) statusText = "פנוי";
                else statusText = "תפוס";

                dt.Rows.Add(timeStr, statusText);

                currentMinutes = currentMinutes + 30;
            }

            GridTimes.DataSource = dt;
            GridTimes.DataBind();
            GridTimes.Visible = true;
        }

        // בודק אם שעה ספציפית פנויה - סופר תפוסים בטווח 2+- שעות.
        // לא סופר את ההזמנה הנוכחית כדי שלא תיחשב כתפוסה ע"י עצמה.
        private bool CheckSpecificTime(string timeToCheck, string res, string date, int total, string type)
        {
            DateTime dt = DateTime.Parse(timeToCheck);
            string start = dt.AddHours(-2).ToString("HH:mm");
            string end = dt.AddHours(2).ToString("HH:mm");

            // אם הטווח חוצה חצות - תנאי OR במקום AND
            string timeCondition;
            if (start.CompareTo(end) > 0)
                timeCondition = "(InvTime > '" + start + "' OR InvTime < '" + end + "')";
            else
                timeCondition = "(InvTime > '" + start + "' AND InvTime < '" + end + "')";

            string sqlCount = "SELECT COUNT(*) FROM MyBooking " +
                              "WHERE Restaurant = '" + res + "' " +
                              "AND InvDate = #" + date + "# " +
                              "AND " + timeCondition + " " +
                              "AND TableType = '" + type + "' " +
                              "AND NOT (PhoneNum = '" + Session["Phone"].ToString() + "' " +
                              "AND InvDate = #" + Session["OldDate"].ToString() + "# " +
                              "AND InvTime = '" + Session["OldTime"].ToString() + "')";

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);
            OleDbCommand cmd = new OleDbCommand(sqlCount, con);
            con.Open();
            int occupied = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();

            return occupied < total;
        }

        // לחיצה על שעה בטבלה - אם פנויה, מעדכן את ההזמנה
        protected void GridTimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cells[0]=כפתור, Cells[1]=שעה, Cells[2]=סטטוס
            string newTime = GridTimes.SelectedRow.Cells[1].Text;
            string status = GridTimes.SelectedRow.Cells[2].Text;

            if (status == "תפוס")
            {
                LblMessage.Text = "השעה תפוסה, נא לבחור שעה אחרת.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            UpdateInDB(newTime);
        }

        // מעדכן את שורת ההזמנה במסד הנתונים
        private void UpdateInDB(string newTime)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection conn = new OleDbConnection(connStr);

            string sql = "UPDATE MyBooking SET " +
                         "InvDate = #" + TxtDate.Text + "#, " +
                         "InvTime = '" + newTime + "', " +
                         "NumGuest = '" + TxtNumGuests.Text + "', " +
                         "TableType = '" + Session["SelectedTypeUpdate"].ToString() + "' " +
                         "WHERE PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND InvDate = #" + Session["OldDate"].ToString() + "# " +
                         "AND InvTime = '" + Session["OldTime"].ToString() + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            LblMessage.Text = "ההזמנה עודכנה בהצלחה לשעה " + newTime + "!";
            LblMessage.ForeColor = System.Drawing.Color.LightGreen;

            // חזרה לרשימה עם הנתונים המעודכנים
            PnlEdit.Visible = false;
            PnlList.Visible = true;
            LoadFutureBookings();
        }

        // לחיצה על "ביטול הזמנה" - מוחק את ההזמנה מהמסד
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "DELETE FROM MyBooking " +
                         "WHERE PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND InvDate = #" + Session["OldDate"].ToString() + "# " +
                         "AND InvTime = '" + Session["OldTime"].ToString() + "'";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            LblMessage.Text = "ההזמנה בוטלה ונמחקה מהמערכת.";
            LblMessage.ForeColor = System.Drawing.Color.Orange;

            // חזרה לרשימה (בלי ההזמנה שנמחקה)
            PnlEdit.Visible = false;
            PnlList.Visible = true;
            LoadFutureBookings();
        }

        // ============================================================
        // עזר
        // ============================================================

        // ממיר את שם סוג השולחן מאנגלית לתיאור עברי
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
