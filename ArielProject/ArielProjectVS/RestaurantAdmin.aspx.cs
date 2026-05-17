using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web.UI;

namespace ArielProject
{
    // שורת הזמנה שנטענת מה-DB עבור חישובי הסטטיסטיקה
    public class AdminBookingRow
    {
        public DateTime InvDate { get; set; }
        public string InvTime { get; set; }
        public int NumGuest { get; set; }
        public string TableType { get; set; }
        public string Guest { get; set; }
        public string PhoneNum { get; set; }
    }

    // פריט עבור גרף בר (תווית + ספירה + רוחב באחוזים)
    public class BarItem
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public int Percent { get; set; }
    }

    // פריט עבור טבלת ההזמנות הקרובות
    public class UpcomingRow
    {
        public string DateStr { get; set; }
        public string InvTime { get; set; }
        public string Guest { get; set; }
        public string PhoneNum { get; set; }
        public string NumGuest { get; set; }
        public string TableType { get; set; }
    }

    public partial class RestaurantAdmin : System.Web.UI.Page
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

            // אימות 2: המשתמש חייב להיות מנהל מסעדה
            if (Session["RestaurantAdmin"] == null)
            {
                Response.Redirect("HomePage.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();
            string restaurant = Session["RestaurantAdmin"].ToString();
            LblRestaurantName.Text = restaurant;

            if (!IsPostBack)
            {
                LoadStatistics(restaurant);
            }
        }

        private void LoadStatistics(string restaurant)
        {
            // טוענים פעם אחת את כל ההזמנות של המסעדה, ומחשבים את הסטטיסטיקות בזיכרון
            List<AdminBookingRow> all = LoadAllBookings(restaurant);

            DateTime today = DateTime.Today;

            // ---- KPIs ----
            int totalCount = all.Count;
            int upcomingCount = all.Count(b => b.InvDate >= today);
            int totalPastGuests = all.Where(b => b.InvDate < today).Sum(b => b.NumGuest);
            double avgGuests = all.Count > 0 ? all.Average(b => b.NumGuest) : 0;

            LblTotalCount.Text = totalCount.ToString();
            LblUpcomingCount.Text = upcomingCount.ToString();
            LblTotalGuests.Text = totalPastGuests.ToString();
            LblAvgGuests.Text = avgGuests.ToString("0.0");

            // ---- גרף 1: התפלגות לפי גודל שולחן ----
            BindTableTypeChart(all);

            // ---- גרף 2: שעות פופולריות (Top 8) ----
            BindTimeChart(all);

            // ---- גרף 3: יום בשבוע (כל 7 הימים) ----
            BindDayOfWeekChart(all);

            // ---- טבלת 5 ההזמנות הקרובות ----
            BindUpcomingTable(all, today);
        }

        private List<AdminBookingRow> LoadAllBookings(string restaurant)
        {
            var list = new List<AdminBookingRow>();

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string sql = "SELECT InvDate, InvTime, NumGuest, TableType, Guest, PhoneNum " +
                             "FROM MyBooking WHERE Restaurant = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", restaurant);

                con.Open();
                OleDbDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new AdminBookingRow
                    {
                        InvDate = Convert.ToDateTime(r["InvDate"]),
                        InvTime = r["InvTime"].ToString(),
                        NumGuest = Convert.ToInt32(r["NumGuest"]),
                        TableType = r["TableType"].ToString(),
                        Guest = r["Guest"].ToString(),
                        PhoneNum = r["PhoneNum"].ToString()
                    });
                }
            }

            return list;
        }

        private void BindTableTypeChart(List<AdminBookingRow> all)
        {
            // משאירים סדר קבוע: קטן, בינוני, גדול
            string[] order = { "Small", "Medium", "Large" };
            string[] labels = { "קטן (עד 2)", "בינוני (3-4)", "גדול (5+)" };

            var bars = new List<BarItem>();
            for (int i = 0; i < order.Length; i++)
            {
                int count = all.Count(b => b.TableType == order[i]);
                bars.Add(new BarItem { Label = labels[i], Count = count });
            }

            ApplyPercentages(bars);

            if (bars.All(b => b.Count == 0))
            {
                RepeaterTableTypes.Visible = false;
                PnlEmptyTable.Visible = true;
            }
            else
            {
                RepeaterTableTypes.DataSource = bars;
                RepeaterTableTypes.DataBind();
            }
        }

        private void BindTimeChart(List<AdminBookingRow> all)
        {
            // 8 השעות הפופולריות ביותר
            var bars = all.GroupBy(b => b.InvTime)
                          .Select(g => new BarItem { Label = g.Key, Count = g.Count() })
                          .OrderByDescending(b => b.Count)
                          .ThenBy(b => b.Label)
                          .Take(8)
                          .ToList();

            ApplyPercentages(bars);

            if (bars.Count == 0)
            {
                RepeaterTimes.Visible = false;
                PnlEmptyTimes.Visible = true;
            }
            else
            {
                RepeaterTimes.DataSource = bars;
                RepeaterTimes.DataBind();
            }
        }

        private void BindDayOfWeekChart(List<AdminBookingRow> all)
        {
            // סדר ימים: ראשון -> שבת
            DayOfWeek[] order = { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday,
                                  DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };
            string[] labels = { "ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת" };

            var bars = new List<BarItem>();
            for (int i = 0; i < order.Length; i++)
            {
                int count = all.Count(b => b.InvDate.DayOfWeek == order[i]);
                bars.Add(new BarItem { Label = labels[i], Count = count });
            }

            ApplyPercentages(bars);

            RepeaterDays.DataSource = bars;
            RepeaterDays.DataBind();
        }

        private void BindUpcomingTable(List<AdminBookingRow> all, DateTime today)
        {
            var upcoming = all.Where(b => b.InvDate >= today)
                              .OrderBy(b => b.InvDate)
                              .ThenBy(b => b.InvTime)
                              .Take(5)
                              .Select(b => new UpcomingRow
                              {
                                  DateStr = b.InvDate.ToString("dd/MM/yyyy"),
                                  InvTime = b.InvTime,
                                  Guest = b.Guest,
                                  PhoneNum = b.PhoneNum,
                                  NumGuest = b.NumGuest.ToString(),
                                  TableType = TranslateTableType(b.TableType)
                              })
                              .ToList();

            if (upcoming.Count == 0)
            {
                RepeaterUpcoming.Visible = false;
                PnlEmptyUpcoming.Visible = true;
            }
            else
            {
                RepeaterUpcoming.DataSource = upcoming;
                RepeaterUpcoming.DataBind();
            }
        }

        // ממיר את ה-Count של כל בר ל-Percent רוחב יחסי, ביחס לערך המקסימלי
        private void ApplyPercentages(List<BarItem> bars)
        {
            if (bars.Count == 0) return;
            int max = bars.Max(b => b.Count);
            if (max == 0) return;

            foreach (var b in bars)
            {
                b.Percent = b.Count * 100 / max;
                // רוחב מינימלי כדי שלא יחתך לגמרי באחוזים נמוכים מאוד
                if (b.Count > 0 && b.Percent < 3) b.Percent = 3;
            }
        }

        private string TranslateTableType(string type)
        {
            switch (type)
            {
                case "Small": return "קטן";
                case "Medium": return "בינוני";
                case "Large": return "גדול";
                default: return type;
            }
        }
    }
}
