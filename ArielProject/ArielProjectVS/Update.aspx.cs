using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    // הוסרה המחלקה TimeSlotUpdate עם properties (get; set;) -
    // הוחלפה ב-DataTable עם 2 עמודות (שעה, סטטוס)

    public partial class Update : System.Web.UI.Page
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

            // קביעת תאריך מינימלי בשדה הטופס + הגדרת ערך השוואה ל-Validator.
            // השארנו את שימוש ב-txtDate.Attributes["min"] כי זה רק קובע HTML
            // attribute בצורה פשוטה (לא מתקדם).
            string todayStr = DateTime.Today.ToString("yyyy-MM-dd");
            txtDate.Attributes["min"] = todayStr;
            CompareValidatorDate.ValueToCompare = todayStr;

            if (!IsPostBack)
            {
                // קבלת התאריך והשעה של ההזמנה לעריכה מהכתובת (QueryString)
                string qDate = Request.QueryString["date"];
                string qTime = Request.QueryString["time"];

                // הוחלף string.IsNullOrEmpty בבדיקה ידנית של null או "".
                // null נוצר כשהפרמטר חסר בכתובת, "" כשהוא ריק.
                if (qDate == null || qTime == null || qDate == "" || qTime == "")
                {
                    pnlDetails.Visible = false;
                    lblMessage.Text = "לא נבחרה הזמנה לעריכה. חזור לדף ההזמנות.";
                    lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                    return;
                }

                LoadBooking(qDate, qTime);
            }
        }

        // טוען את פרטי ההזמנה הקיימת ומציג אותם בטופס
        private void LoadBooking(string date, string time)
        {
            // הוחלף AppDomain.CurrentDomain.BaseDirectory ב-Server.MapPath -
            // סגנון אחיד לכל הפרוייקט.
            // הוסר גם בלוק using(...) - חיבור רגיל שנסגר ידנית.
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection conn = new OleDbConnection(connStr);

            // הוחלפו פרמטרים (?) ושימוש ב-DateTime.ParseExact בשרשור מחרוזות.
            // התאריך כבר מגיע בפורמט yyyy-MM-dd מה-URL.
            // באקסס תאריך מוקף ב-# במקום ב-'.
            string sql = "SELECT * FROM MyBooking " +
                         "WHERE PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND InvDate = #" + date + "# " +
                         "AND InvTime = '" + time + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lblResName.Text = reader["Restaurant"].ToString();
                txtDate.Text = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                txtNumGuests.Text = reader["NumGuest"].ToString();

                // שומרים ב-Session את הפרטים המזהים של ההזמנה -
                // נצטרך אותם בהמשך כדי לעדכן או למחוק בדיוק את השורה הזו.
                Session["OldDate"] = txtDate.Text;
                Session["OldTime"] = reader["InvTime"].ToString();

                pnlDetails.Visible = true;
                lblMessage.Text = "ניתן לעדכן פרטים ולבחור שעה חדשה.";
                lblMessage.ForeColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                pnlDetails.Visible = false;
                lblMessage.Text = "ההזמנה לא נמצאה.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
            }

            conn.Close();
        }

        // לחיצה על "בדוק שעות פנויות" - בודק שהתאריך תקין ומציג שעות פנויות
        protected void btnCheckAvailability_Click(object sender, EventArgs e)
        {
            // בודקים שכל הוולידטורים בדף עברו (CompareValidator של תאריך)
            if (!Page.IsValid) return;

            // הוחלפה DateTime.TryParse עם פרמטר out ב-try/catch + DateTime.Parse רגיל.
            // out parameter לא נלמד בתיכון.
            DateTime chosen;
            try
            {
                chosen = DateTime.Parse(txtDate.Text);
            }
            catch
            {
                lblMessage.Text = "תאריך לא תקין.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            // בדיקת הגנת שרת - גם אם הוולידטור עוקף, אסור תאריך בעבר
            if (chosen.Date < DateTime.Today)
            {
                lblMessage.Text = "לא ניתן לעדכן הזמנה לתאריך שעבר.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            GenerateTimeSlots();
        }

        // בונה את טבלת השעות הפנויות ומציג אותה ב-GridView
        private void GenerateTimeSlots()
        {
            string res = lblResName.Text;
            string date = txtDate.Text;
            int guests = int.Parse(txtNumGuests.Text);

            // קביעת סוג השולחן לפי מספר הסועדים - פשוט if/else if
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

            // מתחברים למסד ושולפים את מספר השולחנות במסעדה.
            // הוסר בלוק using(...) - חיבור רגיל שנסגר ידנית.
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sqlCap = "SELECT " + tableType + " FROM MyRestaurants WHERE Restaurants = '" + res + "'";
            OleDbCommand cmdCap = new OleDbCommand(sqlCap, con);
            con.Open();
            int totalTables = int.Parse(cmdCap.ExecuteScalar().ToString());
            con.Close();

            // הוחלפה List<TimeSlotUpdate> ב-DataTable עם 2 עמודות.
            // ה-DataTable יוצג אוטומטית ב-GridView.
            DataTable dt = new DataTable();
            dt.Columns.Add("שעה");
            dt.Columns.Add("סטטוס");

            // לולאה שעוברת על השעות 18:00-23:30 בקפיצות של 30 דקות.
            // משתמשים ב-int במקום ב-DateTime - יותר פשוט.
            int startMinutes = 18 * 60;       // 18:00 - שעת פתיחה
            int endMinutes = 23 * 60 + 30;    // 23:30 - שעה אחרונה אפשרית

            int currentMinutes = startMinutes;
            while (currentMinutes <= endMinutes)
            {
                // המרת הדקות בחזרה לפורמט HH:mm
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

            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Visible = true;
        }

        // בודק אם שעה מסוימת פנויה - סופר כמה שולחנות תפוסים בטווח של שעתיים סביבה.
        // הוסר פרמטר OleDbConnection - הפונקציה פותחת חיבור משלה.
        private bool CheckSpecificTime(string timeToCheck, string res, string date, int total, string type)
        {
            DateTime dt = DateTime.Parse(timeToCheck);
            string start = dt.AddHours(-2).ToString("HH:mm");
            string end = dt.AddHours(2).ToString("HH:mm");

            // בדיקה אם הטווח עובר חצות - אם כן צריך תנאי OR במקום AND.
            // הוחלף string.Compare ב-CompareTo, והוסר האופרטור הטרנארי (?:).
            string timeCondition;
            if (start.CompareTo(end) > 0)
            {
                timeCondition = "(InvTime > '" + start + "' OR InvTime < '" + end + "')";
            }
            else
            {
                timeCondition = "(InvTime > '" + start + "' AND InvTime < '" + end + "')";
            }

            // הוחלף string.Format בשרשור מחרוזות פשוט.
            // השאילתה סופרת את כל ההזמנות בטווח חוץ מההזמנה הנוכחית
            // (זו שאנחנו עכשיו מעדכנים) - כדי שלא תיחשב כתפוסה בעצמה.
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

        // טיפול בלחיצה על "Select" ליד שעה - אם פנויה, מעדכן את ההזמנה.
        // הוחלף RepeaterTimes_ItemCommand ב-GridView1_SelectedIndexChanged.
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // אינדקסים: Cells[0]=כפתור, Cells[1]=שעה, Cells[2]=סטטוס
            string newTime = GridView1.SelectedRow.Cells[1].Text;
            string status = GridView1.SelectedRow.Cells[2].Text;

            if (status == "תפוס")
            {
                lblMessage.Text = "השעה תפוסה, נא לבחור שעה אחרת.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            UpdateInDB(newTime);
        }

        // מעדכן את שורת ההזמנה במסד הנתונים
        private void UpdateInDB(string newTime)
        {
            // הוסר בלוק using(...).
            // הוחלפו פרמטרים בשרשור מחרוזות.
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection conn = new OleDbConnection(connStr);

            string sql = "UPDATE MyBooking SET " +
                         "InvDate = #" + txtDate.Text + "#, " +
                         "InvTime = '" + newTime + "', " +
                         "NumGuest = '" + txtNumGuests.Text + "', " +
                         "TableType = '" + Session["SelectedTypeUpdate"].ToString() + "' " +
                         "WHERE PhoneNum = '" + Session["Phone"].ToString() + "' " +
                         "AND InvDate = #" + Session["OldDate"].ToString() + "# " +
                         "AND InvTime = '" + Session["OldTime"].ToString() + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            lblMessage.Text = "ההזמנה עודכנה בהצלחה לשעה " + newTime + "!";
            lblMessage.ForeColor = System.Drawing.Color.LightGreen;
            pnlDetails.Visible = false;
        }

        // לחיצה על "ביטול הזמנה" - מוחק את ההזמנה מהמסד
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // הוסר בלוק using(...).
            // הוחלפו פרמטרים בשרשור מחרוזות.
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

            lblMessage.Text = "ההזמנה בוטלה ונמחקה מהמערכת.";
            lblMessage.ForeColor = System.Drawing.Color.Orange;
            pnlDetails.Visible = false;
        }
    }
}
