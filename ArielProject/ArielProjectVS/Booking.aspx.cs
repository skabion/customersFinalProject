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

            DateTime startTime = DateTime.Parse("18:00");
            DateTime endTime = DateTime.Parse("23:30");

            while (startTime <= endTime)
            {
                string timeToCheck = startTime.ToString("HH:mm");
                bool isAvail = CheckSpecificTime(timeToCheck, res, date, totalTables, typeName, con);

                slots.Add(new TimeSlot { TimeStr = timeToCheck, IsAvailable = isAvail });

                startTime = startTime.AddMinutes(30);
            }

            RepeaterTimes.DataSource = slots;
            RepeaterTimes.DataBind();
            RepeaterTimes.Visible = true;
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