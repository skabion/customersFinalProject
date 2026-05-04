using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArielProject
{
    // מחלקה קטנה שעוזרת לנו לשמור נתונים - נמצאת מחוץ למחלקת הדף
    public class TimeSlot
    {
        public string TimeStr { get; set; }
        public bool IsAvailable { get; set; }
    }

    // =========================================================
    // תחילת המחלקה של הדף - כל הפעולות חייבות להיות בתוך הסוגריים שלה!
    // =========================================================
    public partial class Booking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["res"] != null)
                {
                    LblResName.Text = Request.QueryString["res"].ToString();
                }
            }

            if (!IsPostBack)
            {
                // מגדיר את הערך להשוואה כתאריך של היום בפורמט שמתאים לשדה HTML5
                CompareValidatorDate.ValueToCompare = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        // ברגע שלוחצים "מצאו לי שולחן"
        protected void BtnCheckTimes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtDate.Text) || string.IsNullOrEmpty(TxtGuests.Text))
            {
                LblMsg.Text = "נא להזין תאריך ומספר סועדים";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            GenerateTimeSlots();
        }

        // כאן נמצאות השורות שעשו לך שגיאה - עכשיו הן בתוך המחלקה ולכן יזוהו
        private void GenerateTimeSlots()
        {
            string res = LblResName.Text;
            string date = TxtDate.Text;
            int guests = int.Parse(TxtGuests.Text);

            string tableType = "SmallTables";
            string typeName = "Small";
            if (guests > 2 && guests <= 4) { tableType = "MediumTables"; typeName = "Medium"; }
            else if (guests > 4) { tableType = "LargeTables"; typeName = "Large"; }

            Session["SelectedType"] = typeName;

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            OleDbConnection con = new OleDbConnection(connStr);

            string sqlCap = "SELECT " + tableType + " FROM MyRestaurants WHERE Restaurants = '" + res + "'";
            OleDbCommand cmdCap = new OleDbCommand(sqlCap, con);
            con.Open();
            object result = cmdCap.ExecuteScalar();
            int totalTables = result != null ? Convert.ToInt32(result) : 0;
            con.Close();

            List<TimeSlot> slots = new List<TimeSlot>();

            // קביעת שעות הפעילות לפי המסעדה והיום בשבוע
            DateTime selectedDate = DateTime.Parse(date);
            TimeSpan openSpan, closeSpan;
            GetOpeningHours(res, selectedDate.DayOfWeek, out openSpan, out closeSpan);

            // אפשר להזמין רק עד שעתיים לפני סגירת המסעדה
            DateTime startTime = DateTime.Today.Add(openSpan);
            DateTime endTime = DateTime.Today.Add(closeSpan).AddHours(-2);

            if (startTime > endTime)
            {
                LblMsg.Text = "המסעדה אינה מקבלת הזמנות בתאריך זה";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                RepeaterTimes.DataSource = null;
                RepeaterTimes.DataBind();
                RepeaterTimes.Visible = false;
                return;
            }

            while (startTime <= endTime)
            {
                string timeToCheck = startTime.ToString("HH:mm");
                bool isAvail = CheckSpecificTime(timeToCheck, res, date, totalTables, typeName, con);

                slots.Add(new TimeSlot { TimeStr = timeToCheck, IsAvailable = isAvail });

                startTime = startTime.AddMinutes(30);
            }

            LblMsg.Text = "";
            RepeaterTimes.DataSource = slots;
            RepeaterTimes.DataBind();
            RepeaterTimes.Visible = true;
        }

        // מחזירה את שעות הפעילות של המסעדה ביום מסוים בשבוע.
        // שעות שעוברות חצות מיוצגות עם ערך גדול מ-24 (למשל 25:00 = 01:00 למחרת).
        // TimeSpan.Zero לפתיחה ולסגירה => המסעדה סגורה ביום זה.
        private void GetOpeningHours(string res, DayOfWeek day, out TimeSpan open, out TimeSpan close)
        {
            // ברירת מחדל - שעות גנריות לכל מסעדה שעוד לא הוגדרה
            open = new TimeSpan(18, 0, 0);
            close = new TimeSpan(23, 30, 0);

            bool isFri = day == DayOfWeek.Friday;
            bool isSat = day == DayOfWeek.Saturday;

            switch (res)
            {
                case "Bobo":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-15:00 | שבת 20:00-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "La Lush":
                    // ראשון-חמישי 09:00-00:00 | שישי 08:00-16:00 | שבת 19:00-00:00
                    if (isFri) { open = new TimeSpan(8, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(24, 0, 0); }
                    else { open = new TimeSpan(9, 0, 0); close = new TimeSpan(24, 0, 0); }
                    break;

                case "Moses":
                    // ראשון-רביעי 12:00-00:00 | חמישי-שבת 12:00-01:00
                    if (day == DayOfWeek.Thursday || isFri || isSat)
                    { open = new TimeSpan(12, 0, 0); close = new TimeSpan(25, 0, 0); }
                    else
                    { open = new TimeSpan(12, 0, 0); close = new TimeSpan(24, 0, 0); }
                    break;

                case "Japanika":
                    // ראשון-חמישי 12:00-23:00 | שישי 12:00-15:00 | שבת 19:30-23:00
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 30, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Kagas":
                    // ראשון-חמישי 10:00-23:00 | שישי 09:00-16:00 | מוצ"ש 20:00-23:30
                    if (isFri) { open = new TimeSpan(9, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(10, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Vivino":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-16:00 | מוצ"ש 19:00-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Mahne Yuda":
                    // ראשון-חמישי 18:30-01:30 | שישי 12:00-15:30 | שבת סגור
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 30, 0); }
                    else if (isSat) { open = TimeSpan.Zero; close = TimeSpan.Zero; }
                    else { open = new TimeSpan(18, 30, 0); close = new TimeSpan(25, 30, 0); }
                    break;

                case "Oshi Oshi":
                    // ראשון-חמישי 12:00-23:00 | שישי 11:30-15:00 | מוצ"ש 20:00-23:00
                    if (isFri) { open = new TimeSpan(11, 30, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Biga":
                    // ראשון-חמישי 12:00-23:00 | שישי 12:00-15:00 | שבת 19:00-23:00
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Nafis":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-16:00 | מוצ"ש 19:30-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 30, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Zink":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-15:30 | מוצ"ש 19:00-00:00
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 30, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(24, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Max Brener":
                    // ראשון-חמישי 09:00-23:00 | שישי 09:00-16:00 | מוצ"ש 20:00-23:00
                    if (isFri) { open = new TimeSpan(9, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(9, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Segev":
                    // ראשון-חמישי 12:00-23:00 | שישי 12:00-15:00 | מוצ"ש 19:30-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 30, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Black":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-16:00 | מוצ"ש 20:00-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Kansai":
                    // ראשון-חמישי 12:00-23:00 | שישי 11:30-14:30 | מוצ"ש 20:30-23:00
                    if (isFri) { open = new TimeSpan(11, 30, 0); close = new TimeSpan(14, 30, 0); }
                    else if (isSat) { open = new TimeSpan(20, 30, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;
            }
        }

        private bool CheckSpecificTime(string timeToCheck, string res, string date, int total, string type, OleDbConnection con)
        {
            DateTime dt = DateTime.Parse(timeToCheck);
            string start = dt.AddHours(-2).ToString("HH:mm");
            string end = dt.AddHours(2).ToString("HH:mm");

            string timeCondition;
            if (string.Compare(start, end) > 0)
            {
                timeCondition = string.Format("(InvTime > '{0}' OR InvTime < '{1}')", start, end);
            }
            else
            {
                timeCondition = string.Format("(InvTime > '{0}' AND InvTime < '{1}')", start, end);
            }

            string sqlCount = string.Format(
                "SELECT COUNT(*) FROM MyBooking WHERE Restaurant='{0}' AND InvDate=#{1}# " +
                "AND {2} AND TableType='{3}'",
                res, date, timeCondition, type);

            OleDbCommand cmd = new OleDbCommand(sqlCount, con);
            con.Open();
            int occupied = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return occupied < total;
        }

        protected void RepeaterTimes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string finalTime = e.CommandArgument.ToString();
            SaveToDB(finalTime);
        }

        private void SaveToDB(string finalTime)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = string.Format("INSERT INTO MyBooking (Guest, PhoneNum, InvDate, NumGuest, InvTime, Restaurant, TableType) " +
             "VALUES ('{0}', '{1}', #{2}#, '{3}', '{4}', '{5}', '{6}')",
             Session["User"], Session["Phone"], TxtDate.Text,
             TxtGuests.Text, finalTime, LblResName.Text, Session["SelectedType"]);

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            LblMsg.Text = "ההזמנה לשעה " + finalTime + " בוצעה בהצלחה!";
            LblMsg.ForeColor = System.Drawing.Color.Green;

            RepeaterTimes.Visible = false;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "clearTimer", "clearBookingTimer();", true);
        }
    }
    // =========================================================
    // סוף המחלקה של הדף
    // =========================================================
}