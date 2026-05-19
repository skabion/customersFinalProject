using System;
using System.Collections.Generic;
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

        // טוען את כל הסטטיסטיקות והגרפים עבור מסעדה
        private void LoadStatistics(string restaurant)
        {
            // טוענים את כל ההזמנות פעם אחת ל-DataTable, ומשם מחשבים הכל
            DataTable allBookings = LoadAllBookings(restaurant);

            DateTime today = DateTime.Today;

            // ============ KPIs - מספרים סטטיסטיים ============
            // הוחלפו פעולות LINQ (Count, Where, Sum, Average) בלולאות for רגילות.

            int totalCount = allBookings.Rows.Count;

            // ספירת הזמנות עתידיות + סכום הסועדים בעבר + סכום כולל לחישוב ממוצע
            int upcomingCount = 0;
            int totalPastGuests = 0;
            int sumAllGuests = 0;

            for (int i = 0; i < allBookings.Rows.Count; i++)
            {
                DateTime date = DateTime.Parse(allBookings.Rows[i]["InvDate"].ToString());
                int guests = int.Parse(allBookings.Rows[i]["NumGuest"].ToString());

                if (date >= today)
                    upcomingCount++;
                else
                    totalPastGuests += guests;

                sumAllGuests += guests;
            }

            // חישוב ממוצע סועדים להזמנה (אם יש בכלל הזמנות)
            double avgGuests = 0;
            if (totalCount > 0)
            {
                avgGuests = (double)sumAllGuests / totalCount;
            }

            LblTotalCount.Text = totalCount.ToString();
            LblUpcomingCount.Text = upcomingCount.ToString();
            LblTotalGuests.Text = totalPastGuests.ToString();
            LblAvgGuests.Text = avgGuests.ToString("0.0");

            // ============ גרפים וטבלת הזמנות קרובות ============
            BindTableTypeChart(allBookings);
            BindTimeChart(allBookings);
            BindDayOfWeekChart(allBookings);
            BindUpcomingTable(allBookings, today);
        }

        // מביא את כל ההזמנות של המסעדה מהמסד.
        // הוחלף List<AdminBookingRow> ב-DataTable עם 6 עמודות.
        // הוחלף AppDomain.CurrentDomain.BaseDirectory ב-Server.MapPath.
        // הוסר בלוק using(...) והוחלפו פרמטרים בשרשור מחרוזות.
        private DataTable LoadAllBookings(string restaurant)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("InvDate");
            dt.Columns.Add("InvTime");
            dt.Columns.Add("NumGuest");
            dt.Columns.Add("TableType");
            dt.Columns.Add("Guest");
            dt.Columns.Add("PhoneNum");

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT InvDate, InvTime, NumGuest, TableType, Guest, PhoneNum " +
                         "FROM MyBooking WHERE Restaurant = '" + restaurant + "'";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                dt.Rows.Add(
                    Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd"),
                    reader["InvTime"].ToString(),
                    reader["NumGuest"].ToString(),
                    reader["TableType"].ToString(),
                    reader["Guest"].ToString(),
                    reader["PhoneNum"].ToString()
                );
            }
            con.Close();

            return dt;
        }

        // גרף 1: התפלגות לפי גודל שולחן (קטן/בינוני/גדול)
        // הוחלפו List<BarItem> + LINQ במערכים מקבילים + לולאות for.
        private void BindTableTypeChart(DataTable allBookings)
        {
            // 3 קטגוריות בסדר קבוע
            string[] order = { "Small", "Medium", "Large" };
            string[] labels = { "קטן (עד 2)", "בינוני (3-4)", "גדול (5+)" };
            int[] counts = new int[3];
            int[] percents = new int[3];

            // לכל קטגוריה - סופרים כמה הזמנות יש איתה
            for (int i = 0; i < order.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < allBookings.Rows.Count; j++)
                {
                    if (allBookings.Rows[j]["TableType"].ToString() == order[i])
                        count++;
                }
                counts[i] = count;
            }

            ApplyPercentages(counts, percents);

            // בודקים אם כל הספירות הן 0 (אין נתונים)
            bool allZero = true;
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > 0) allZero = false;
            }

            if (allZero)
            {
                LblTableTypesChart.Visible = false;
                PnlEmptyTable.Visible = true;
            }
            else
            {
                LblTableTypesChart.Text = BuildBarChartHtml(labels, counts, percents, "");
            }
        }

        // גרף 2: 8 השעות הפופולריות ביותר
        // הוחלף LINQ של GroupBy/OrderByDescending/Take בקוד פשוט יותר.
        private void BindTimeChart(DataTable allBookings)
        {
            // שלב 1: מעבר על ההזמנות ובניית רשימת שעות ייחודיות עם ספירה
            List<string> uniqueTimes = new List<string>();
            List<int> timeCounts = new List<int>();

            for (int i = 0; i < allBookings.Rows.Count; i++)
            {
                string time = allBookings.Rows[i]["InvTime"].ToString();

                // מחפשים אם השעה כבר נמצאת ברשימה
                int foundIdx = -1;
                for (int j = 0; j < uniqueTimes.Count; j++)
                {
                    if (uniqueTimes[j] == time)
                    {
                        foundIdx = j;
                    }
                }

                if (foundIdx == -1)
                {
                    // שעה חדשה - מוסיפים עם ספירה 1
                    uniqueTimes.Add(time);
                    timeCounts.Add(1);
                }
                else
                {
                    // שעה קיימת - מגדילים את הספירה
                    timeCounts[foundIdx] = timeCounts[foundIdx] + 1;
                }
            }

            // שלב 2: מיון בועות לפי ספירה (מהגדול לקטן). אם אותה ספירה - לפי שעה (מהקטנה לגדולה).
            int n = uniqueTimes.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    bool shouldSwap = false;
                    if (timeCounts[j] < timeCounts[j + 1])
                    {
                        shouldSwap = true;
                    }
                    else if (timeCounts[j] == timeCounts[j + 1])
                    {
                        if (uniqueTimes[j].CompareTo(uniqueTimes[j + 1]) > 0)
                            shouldSwap = true;
                    }

                    if (shouldSwap)
                    {
                        string tmpT = uniqueTimes[j];
                        uniqueTimes[j] = uniqueTimes[j + 1];
                        uniqueTimes[j + 1] = tmpT;

                        int tmpC = timeCounts[j];
                        timeCounts[j] = timeCounts[j + 1];
                        timeCounts[j + 1] = tmpC;
                    }
                }
            }

            // שלב 3: לוקחים רק את 8 הראשונים (אם יש פחות - לוקחים את כולם)
            int takeCount = uniqueTimes.Count;
            if (takeCount > 8) takeCount = 8;

            string[] labels = new string[takeCount];
            int[] counts = new int[takeCount];
            int[] percents = new int[takeCount];

            for (int i = 0; i < takeCount; i++)
            {
                labels[i] = uniqueTimes[i];
                counts[i] = timeCounts[i];
            }

            ApplyPercentages(counts, percents);

            if (takeCount == 0)
            {
                LblTimesChart.Visible = false;
                PnlEmptyTimes.Visible = true;
            }
            else
            {
                LblTimesChart.Text = BuildBarChartHtml(labels, counts, percents, "purple");
            }
        }

        // גרף 3: התפלגות לפי יום בשבוע (ראשון עד שבת)
        // הוחלף DayOfWeek enum במספרים שלמים (0=ראשון, 6=שבת).
        private void BindDayOfWeekChart(DataTable allBookings)
        {
            string[] labels = { "ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת" };
            int[] counts = new int[7];
            int[] percents = new int[7];

            for (int i = 0; i < allBookings.Rows.Count; i++)
            {
                DateTime date = DateTime.Parse(allBookings.Rows[i]["InvDate"].ToString());
                // (int) ממיר את DayOfWeek למספר: 0=ראשון, 1=שני, ..., 6=שבת
                int dayNum = (int)date.DayOfWeek;
                counts[dayNum] = counts[dayNum] + 1;
            }

            ApplyPercentages(counts, percents);

            LblDaysChart.Text = BuildBarChartHtml(labels, counts, percents, "green");
        }

        // טבלת 5 ההזמנות הקרובות.
        // הוחלף Repeater + LINQ ב-GridView + לולאת מיון בועות.
        private void BindUpcomingTable(DataTable allBookings, DateTime today)
        {
            // שלב 1: סינון רק הזמנות עתידיות (תאריך >= היום) למערכים מקבילים
            List<DateTime> dates = new List<DateTime>();
            List<string> times = new List<string>();
            List<string> guests = new List<string>();
            List<string> phones = new List<string>();
            List<string> nums = new List<string>();
            List<string> tableTypes = new List<string>();

            for (int i = 0; i < allBookings.Rows.Count; i++)
            {
                DateTime date = DateTime.Parse(allBookings.Rows[i]["InvDate"].ToString());
                if (date >= today)
                {
                    dates.Add(date);
                    times.Add(allBookings.Rows[i]["InvTime"].ToString());
                    guests.Add(allBookings.Rows[i]["Guest"].ToString());
                    phones.Add(allBookings.Rows[i]["PhoneNum"].ToString());
                    nums.Add(allBookings.Rows[i]["NumGuest"].ToString());
                    tableTypes.Add(allBookings.Rows[i]["TableType"].ToString());
                }
            }

            // שלב 2: מיון בועות לפי תאריך (מהקרוב לרחוק) ואז לפי שעה
            int n = dates.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    bool shouldSwap = false;
                    if (dates[j] > dates[j + 1])
                    {
                        shouldSwap = true;
                    }
                    else if (dates[j] == dates[j + 1])
                    {
                        if (times[j].CompareTo(times[j + 1]) > 0)
                            shouldSwap = true;
                    }

                    if (shouldSwap)
                    {
                        // החלפת כל ששת המערכים יחד
                        DateTime tmpD = dates[j]; dates[j] = dates[j + 1]; dates[j + 1] = tmpD;

                        string tmp;
                        tmp = times[j]; times[j] = times[j + 1]; times[j + 1] = tmp;
                        tmp = guests[j]; guests[j] = guests[j + 1]; guests[j + 1] = tmp;
                        tmp = phones[j]; phones[j] = phones[j + 1]; phones[j + 1] = tmp;
                        tmp = nums[j]; nums[j] = nums[j + 1]; nums[j + 1] = tmp;
                        tmp = tableTypes[j]; tableTypes[j] = tableTypes[j + 1]; tableTypes[j + 1] = tmp;
                    }
                }
            }

            // שלב 3: לוקחים רק את 5 הראשונים
            int takeCount = dates.Count;
            if (takeCount > 5) takeCount = 5;

            // בונים DataTable להצגה ב-GridView
            DataTable upcoming = new DataTable();
            upcoming.Columns.Add("תאריך");
            upcoming.Columns.Add("שעה");
            upcoming.Columns.Add("שם הסועד");
            upcoming.Columns.Add("טלפון");
            upcoming.Columns.Add("סועדים");
            upcoming.Columns.Add("שולחן");

            for (int i = 0; i < takeCount; i++)
            {
                upcoming.Rows.Add(
                    dates[i].ToString("dd/MM/yyyy"),
                    times[i],
                    guests[i],
                    phones[i],
                    nums[i],
                    TranslateTableType(tableTypes[i])
                );
            }

            if (takeCount == 0)
            {
                GridView1.Visible = false;
                PnlEmptyUpcoming.Visible = true;
            }
            else
            {
                GridView1.DataSource = upcoming;
                GridView1.DataBind();
            }
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

        // ממיר את שם סוג השולחן מאנגלית לעברית.
        // הוחלף switch ב-if/else if.
        private string TranslateTableType(string type)
        {
            if (type == "Small")
                return "קטן";
            else if (type == "Medium")
                return "בינוני";
            else if (type == "Large")
                return "גדול";
            else
                return type;
        }
    }
}
