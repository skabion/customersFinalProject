using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    // הוסרו 3 המחלקות שהיו: AdminBookingRow, BarItem, UpcomingRow
    // הוחלפו ב-DataTable (לנתוני הזמנות + טבלת קרובות)
    // ובמערכים מקבילים (לנתוני גרפי בר).

    public partial class RestaurantAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // אימות 1: המשתמש חייב להיות מחובר
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            bool isSysAdmin = Session["Admin"] != null;
            bool isRestAdmin = Session["RestaurantAdmin"] != null;

            // אימות 2: רק מנהלים רואים את הדף הזה
            if (!isSysAdmin && !isRestAdmin)
            {
                Response.Redirect("HomePage.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            // קביעת מצב הדף:
            //   - מנהל מערכת עם ?restaurant=X => סטטיסטיקה למסעדה X
            //   - מנהל מערכת בלי QueryString  => תפריט מנהל
            //   - מנהל מסעדה                  => סטטיסטיקה למסעדה שלו
            string restaurantToShow = null;
            bool fromAdminList = false;

            if (isSysAdmin)
            {
                string qRest = Request.QueryString["restaurant"];
                // הוחלף string.IsNullOrEmpty בבדיקה ידנית של null או ""
                if (qRest != null && qRest != "")
                {
                    restaurantToShow = qRest;
                    fromAdminList = true;
                }
            }
            else if (isRestAdmin)
            {
                restaurantToShow = Session["RestaurantAdmin"].ToString();
            }

            if (restaurantToShow != null)
            {
                // מצב סטטיסטיקה
                PnlAdminMenu.Visible = false;
                PnlStats.Visible = true;
                PnlRestaurantTag.Visible = true;
                PnlAdminBadge.Visible = false;
                LblRestaurantName.Text = restaurantToShow;

                if (fromAdminList)
                {
                    BackLink.NavigateUrl = "AllRestaurants.aspx";
                    BackLink.Text = "← חזרה לרשימת המסעדות";
                }

                if (!IsPostBack)
                {
                    LoadStatistics(restaurantToShow);
                }

                // טבלת ההזמנות העתידיות נטענת מחדש בכל postback
                // כדי לאפשר שינוי מיון מה-DropDownList ללא צורך בלוגיקה נוספת
                BindUpcomingTable(restaurantToShow);
            }
            else
            {
                // מצב תפריט מנהל מערכת
                PnlStats.Visible = false;
                PnlAdminMenu.Visible = true;
                PnlRestaurantTag.Visible = false;
                PnlAdminBadge.Visible = true;
            }
        }

        // טוען את כל הסטטיסטיקות והגרפים עבור מסעדה.
        // כל פונקציה מריצה שאילתת SQL משלה - אין יותר טעינה כללית ל-DataTable
        // (הוסרה LoadAllBookings) ואין יותר ספירה/מיון/קיבוץ בקוד C#.
        private void LoadStatistics(string restaurant)
        {
            LoadKPIs(restaurant);
            BindTableTypeChart(restaurant);
            BindTimeChart(restaurant);
            BindDayOfWeekChart(restaurant);
        }

        // מחשב את 4 ה-KPIs בשאילתת SQL אחת עם פונקציות צבירה.
        // החליף לולאת for של 33 שורות שעברה על כל ההזמנות וחישבה ידנית.
        // IIF(cond, val1, val2) = if/else באקסס. SUM(IIF(...,1,0)) = ספירה מותנית.
        private void LoadKPIs(string restaurant)
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT COUNT(*) AS Total, " +
                         "SUM(IIF(InvDate >= #" + today + "#, 1, 0)) AS Upcoming, " +
                         "SUM(IIF(InvDate < #" + today + "#, NumGuest, 0)) AS PastGuests, " +
                         "AVG(NumGuest) AS AvgGuests " +
                         "FROM MyBooking WHERE Restaurant = '" + restaurant + "'";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            // אם אין הזמנות בכלל, SUM/AVG מחזירים NULL - ToString על NULL = "",
            // ואנחנו מציגים "0" במקום.
            if (reader.Read())
            {
                LblTotalCount.Text = reader["Total"].ToString();
                LblUpcomingCount.Text = NullToZero(reader["Upcoming"].ToString());
                LblTotalGuests.Text = NullToZero(reader["PastGuests"].ToString());

                string avg = reader["AvgGuests"].ToString();
                if (avg == "")
                    LblAvgGuests.Text = "0.0";
                else
                    LblAvgGuests.Text = double.Parse(avg).ToString("0.0");
            }
            con.Close();
        }

        // עזר: מחזיר "0" אם המחרוזת ריקה (NULL מ-SQL), אחרת מחזיר אותה
        private string NullToZero(string s)
        {
            if (s == "") return "0";
            return s;
        }

        // גרף 1: התפלגות לפי גודל שולחן (קטן/בינוני/גדול).
        // 3 שאילתות COUNT מאוחדות עם UNION ALL מחזירות בדיוק 3 שורות
        // בסדר הקבוע (Small, Medium, Large) - אין יותר לולאות ספירה בקוד.
        private void BindTableTypeChart(string restaurant)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT 'קטן (עד 2)' AS Lbl, COUNT(*) AS Cnt FROM MyBooking WHERE Restaurant='" + restaurant + "' AND TableType='Small' " +
                         "UNION ALL SELECT 'בינוני (3-4)', COUNT(*) FROM MyBooking WHERE Restaurant='" + restaurant + "' AND TableType='Medium' " +
                         "UNION ALL SELECT 'גדול (5+)', COUNT(*) FROM MyBooking WHERE Restaurant='" + restaurant + "' AND TableType='Large'";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            string[] labels = new string[3];
            int[] counts = new int[3];
            int[] percents = new int[3];

            int i = 0;
            while (reader.Read())
            {
                labels[i] = reader["Lbl"].ToString();
                counts[i] = int.Parse(reader["Cnt"].ToString());
                i++;
            }
            con.Close();

            // אם אין נתונים בכלל - 3 הספירות הן 0
            if (counts[0] == 0 && counts[1] == 0 && counts[2] == 0)
            {
                LblTableTypesChart.Visible = false;
                PnlEmptyTable.Visible = true;
            }
            else
            {
                ApplyPercentages(counts, percents);
                LblTableTypesChart.Text = BuildBarChartHtml(labels, counts, percents, "");
            }
        }

        // גרף 2: 8 השעות הפופולריות ביותר.

        private void BindTimeChart(string restaurant)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            // GROUP BY מקבץ הזמנות לפי שעה, COUNT סופר כמה הזמנות בכל קבוצה.
            // ORDER BY COUNT(*) DESC ממיין מהפופולרי ביותר לפחות פופולרי.
            // אם יש תיקו - InvTime ASC משמש כשובר שוויון.
            // TOP 8 מחזיר רק את 8 השורות הראשונות.
            string sql = "SELECT TOP 8 InvTime, COUNT(*) AS Cnt " +
                         "FROM MyBooking " +
                         "WHERE Restaurant = '" + restaurant + "' " +
                         "GROUP BY InvTime " +
                         "ORDER BY COUNT(*) DESC, InvTime ASC";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            // ה-DB החזיר תוצאות כבר ממוינות וחתוכות - אנחנו רק קוראים אותן.
            DataTable dt = new DataTable();
            dt.Columns.Add("InvTime");
            dt.Columns.Add("Cnt");

            while (reader.Read())
            {
                dt.Rows.Add(reader["InvTime"].ToString(), reader["Cnt"].ToString());
            }
            con.Close();

            // העברה למערכים בגודל מדויק (יכול להיות פחות מ-8 אם יש מעט שעות)
            int n = dt.Rows.Count;
            string[] labels = new string[n];
            int[] counts = new int[n];
            int[] percents = new int[n];

            for (int i = 0; i < n; i++)
            {
                labels[i] = dt.Rows[i]["InvTime"].ToString();
                counts[i] = int.Parse(dt.Rows[i]["Cnt"].ToString());
            }

            ApplyPercentages(counts, percents);

            if (n == 0)
            {
                LblTimesChart.Visible = false;
                PnlEmptyTimes.Visible = true;
            }
            else
            {
                LblTimesChart.Text = BuildBarChartHtml(labels, counts, percents, "purple");
            }
        }

        // גרף 3: התפלגות לפי יום בשבוע (ראשון עד שבת).
        // WEEKDAY() של אקסס מחזיר 1=ראשון .. 7=שבת. GROUP BY מקבץ את ההזמנות
        // לפי יום ו-COUNT סופר. ימים בלי הזמנות פשוט לא יחזרו - counts[] שלהם
        // נשארים 0 (ערך ברירת המחדל של מערך int חדש).
        private void BindDayOfWeekChart(string restaurant)
        {
            string[] labels = { "ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת" };
            int[] counts = new int[7];
            int[] percents = new int[7];

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT WEEKDAY(InvDate) AS DayNum, COUNT(*) AS Cnt " +
                         "FROM MyBooking WHERE Restaurant = '" + restaurant + "' " +
                         "GROUP BY WEEKDAY(InvDate)";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // מורידים 1 כדי להתאים לאינדקס במערך (0=ראשון .. 6=שבת)
                int idx = int.Parse(reader["DayNum"].ToString()) - 1;
                counts[idx] = int.Parse(reader["Cnt"].ToString());
            }
            con.Close();

            ApplyPercentages(counts, percents);
            LblDaysChart.Text = BuildBarChartHtml(labels, counts, percents, "green");
        }

        // טבלת כל ההזמנות העתידיות, ממוינות לפי בחירת המשתמש ב-DropDownList.
        // המיון נעשה ב-SQL (ORDER BY) - אין יותר לולאת מיון בועות בקוד.
        // הסינון "רק עתידיות" נעשה גם הוא ב-SQL (WHERE InvDate >= #today#).
        private void BindUpcomingTable(string restaurant)
        {
            // הכיוון "ASC" או "DESC" מגיע ישירות מה-Value של ה-DropDownList
            string direction = DdlSort.SelectedValue;

            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            // באקסס תאריך מוקף בסולמיות (#). אותו סגנון כמו בשאר הדפים בפרוייקט.
            // IIF מקונן בתוך ה-SQL מתרגם את סוג השולחן לעברית - חסך פונקציית עזר.
            // LEFT JOIN ל-MyUsers לפי שם הסועד (Guest = MyFullName, כפי שנשמר בהזמנה)
            // מביא את שדות האלרגיה של המשתמש. LEFT (ולא INNER) כדי שגם הזמנה שאין לה
            // משתמש תואם תוצג - השדות פשוט יחזרו NULL והעמודה תהיה ריקה.
            string sql = "SELECT b.InvDate, b.InvTime, b.Guest, b.PhoneNum, b.NumGuest, " +
                         "IIF(b.TableType='Small','קטן',IIF(b.TableType='Medium','בינוני','גדול')) AS TableTypeHe, " +
                         "u.Gluten, u.Peanuts, u.TreeNuts, u.Fish, u.Sesame, u.Milk " +
                         "FROM MyBooking AS b LEFT JOIN MyUsers AS u ON b.Guest = u.MyFullName " +
                         "WHERE b.Restaurant = '" + restaurant + "' " +
                         "AND b.InvDate >= #" + today + "# " +
                         "ORDER BY b.InvDate " + direction + ", b.InvTime " + direction;

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            DataTable upcoming = new DataTable();
            upcoming.Columns.Add("תאריך");
            upcoming.Columns.Add("שעה");
            upcoming.Columns.Add("שם הסועד");
            upcoming.Columns.Add("טלפון");
            upcoming.Columns.Add("סועדים");
            upcoming.Columns.Add("שולחן");
            upcoming.Columns.Add("אלרגיות");

            while (reader.Read())
            {
                // בונים מהשדות שהגיעו מ-MyUsers מחרוזת אלרגיות אחת לתא בטבלה
                string allergies = BuildAllergiesText(
                    reader["Gluten"].ToString(),
                    reader["Peanuts"].ToString(),
                    reader["TreeNuts"].ToString(),
                    reader["Fish"].ToString(),
                    reader["Sesame"].ToString(),
                    reader["Milk"].ToString()
                );

                upcoming.Rows.Add(
                    Convert.ToDateTime(reader["InvDate"]).ToString("dd/MM/yyyy"),
                    reader["InvTime"].ToString(),
                    reader["Guest"].ToString(),
                    reader["PhoneNum"].ToString(),
                    reader["NumGuest"].ToString(),
                    reader["TableTypeHe"].ToString(),
                    allergies
                );
            }
            con.Close();

            // איפוס מפורש של ה-Visible כדי שהמצב יתעדכן נכון גם בין postbacks
            if (upcoming.Rows.Count == 0)
            {
                GridView1.Visible = false;
                PnlEmptyUpcoming.Visible = true;
            }
            else
            {
                GridView1.Visible = true;
                PnlEmptyUpcoming.Visible = false;
                GridView1.DataSource = upcoming;
                GridView1.DataBind();
            }
        }

        // בונה מחרוזת אחת של האלרגיות שהסועד סימן בעת ההרשמה.
        // מקבל את 6 שדות האלרגיה מ-MyUsers ("כן"/"לא", או "" אם הסועד לא נמצא)
        // ומחזיר טקסט מופרד בפסיקים עם אמוג'ים - אותם אמוג'ים כמו בדף נתוני המשתמשים.
        // אם אין אף אלרגיה - מחזיר "—" כדי שהתא לא יישאר ריק לגמרי.
        private string BuildAllergiesText(string gluten, string peanuts, string treeNuts,
                                          string fish, string sesame, string milk)
        {
            string text = "";
            if (gluten == "כן") text = text + "🌾 גלוטן, ";
            if (peanuts == "כן") text = text + "🥜 בוטנים, ";
            if (treeNuts == "כן") text = text + "🌰 אגוזים, ";
            if (fish == "כן") text = text + "🐟 דגים, ";
            if (sesame == "כן") text = text + "🌿 שומשום, ";
            if (milk == "כן") text = text + "🥛 חלב, ";

            // אין אלרגיות - מציגים מקף במקום תא ריק
            if (text == "") return "—";

            // מסירים את ", " שנוסף בסוף האלרגיה האחרונה
            return text.Substring(0, text.Length - 2);
        }

        // ממיר את הספירות לאחוזים יחסית לערך המקסימלי.
        // הוחלפו LINQ של Max ו-foreach במערכים פשוטים עם לולאת for.
        private void ApplyPercentages(int[] counts, int[] percents)
        {
            // מציאת הערך המקסימלי בלולאה
            int max = 0;
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > max) max = counts[i];
            }

            // אם הכל אפס - לא יוצרים אחוזים
            if (max == 0) return;

            // חישוב אחוזים: כל ערך כיחס למקסימום, מוכפל ב-100
            for (int i = 0; i < counts.Length; i++)
            {
                percents[i] = counts[i] * 100 / max;
                // רוחב מינימלי כדי שהבר לא ייעלם לחלוטין באחוזים נמוכים
                if (counts[i] > 0 && percents[i] < 3) percents[i] = 3;
            }
        }

        // בונה את ה-HTML של גרף הברים על-ידי שרשור מחרוזות.
        // זה החליף את ה-Repeater שיצר את הברים אוטומטית עם templates.
        // colorClass: "" (זהב), "purple", "green" - לצביעת הברים בצבעים שונים.
        private string BuildBarChartHtml(string[] labels, int[] counts, int[] percents, string colorClass)
        {
            // מחליטים על המחלקה של ה-fill לפי הצבע המבוקש
            string fillClass = "bar-fill";
            if (colorClass != "")
                fillClass = fillClass + " " + colorClass;

            string html = "";
            for (int i = 0; i < labels.Length; i++)
            {
                html = html + "<div class='bar-row'>";
                html = html + "<div class='bar-label'>" + labels[i] + "</div>";
                html = html + "<div class='bar-track'>";
                html = html + "<div class='" + fillClass + "' style='width: " + percents[i] + "%;'></div>";
                html = html + "</div>";
                html = html + "<div class='bar-value'>" + counts[i] + "</div>";
                html = html + "</div>";
            }
            return html;
        }

    }
}
