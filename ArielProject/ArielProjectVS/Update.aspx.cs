using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArielProject
{
    public class TimeSlotUpdate
    {
        public string TimeStr { get; set; }
        public bool IsAvailable { get; set; }
    }

    public partial class Update : System.Web.UI.Page
    {
        // מחרוזת התחברות גלובלית
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\DBusers1.accdb";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { pnlDetails.Visible = false; }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                // מחפשים את ההזמנה האחרונה לפי הטלפון
                string sql = "SELECT TOP 1 * FROM MyBooking WHERE PhoneNum = ? ORDER BY InvDate DESC, InvTime DESC";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.Parameters.AddWithValue("?", txtSearch.Text);

                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblResName.Text = reader["Restaurant"].ToString();
                    txtDate.Text = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                    txtNumGuests.Text = reader["NumGuest"].ToString();

                    // שומרים ב-Session את הפרטים המזהים כדי שנוכל לעדכן בדיוק את השורה הזו
                    Session["OldDate"] = txtDate.Text;
                    Session["OldTime"] = reader["InvTime"].ToString();
                    Session["CurrentPhone"] = txtSearch.Text;

                    pnlDetails.Visible = true;
                    lblMessage.Text = "הזמנה נמצאה! ניתן לעדכן פרטים ולבחור שעה חדשה.";
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    pnlDetails.Visible = false;
                    lblMessage.Text = "לא נמצאה הזמנה למספר זה.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnCheckAvailability_Click(object sender, EventArgs e)
        {
            GenerateTimeSlots();
        }

        private void GenerateTimeSlots()
        {
            string res = lblResName.Text;
            string date = txtDate.Text;
            int guests = int.Parse(txtNumGuests.Text);

            string tableType = "SmallTables";
            string typeName = "Small";
            if (guests > 2 && guests <= 4) { tableType = "MediumTables"; typeName = "Medium"; }
            else if (guests > 4) { tableType = "LargeTables"; typeName = "Large"; }

            Session["SelectedTypeUpdate"] = typeName;

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                con.Open();
                // שליפת כמות השולחנות
                string sqlCap = "SELECT " + tableType + " FROM MyRestaurants WHERE Restaurants = ?";
                OleDbCommand cmdCap = new OleDbCommand(sqlCap, con);
                cmdCap.Parameters.AddWithValue("?", res);
                int totalTables = Convert.ToInt32(cmdCap.ExecuteScalar());

                List<TimeSlotUpdate> slots = new List<TimeSlotUpdate>();
                DateTime startTime = DateTime.Parse("18:00");
                DateTime endTime = DateTime.Parse("23:30");

                while (startTime <= endTime)
                {
                    string timeToCheck = startTime.ToString("HH:mm");
                    bool isAvail = CheckSpecificTime(timeToCheck, res, date, totalTables, typeName, con);
                    slots.Add(new TimeSlotUpdate { TimeStr = timeToCheck, IsAvailable = isAvail });
                    startTime = startTime.AddMinutes(30);
                }

                RepeaterTimes.DataSource = slots;
                RepeaterTimes.DataBind();
            }
        }

        private bool CheckSpecificTime(string timeToCheck, string res, string date, int total, string type, OleDbConnection con)
        {
            DateTime dt = DateTime.Parse(timeToCheck);
            string start = dt.AddHours(-2).ToString("HH:mm");
            string end = dt.AddHours(2).ToString("HH:mm");

            // לוגיקה לבדיקת טווח שעות (כולל חציית חצות אם צריך)
            string timeCondition = string.Compare(start, end) > 0 ?
                "(InvTime > ? OR InvTime < ?)" : "(InvTime > ? AND InvTime < ?)";

            // שיפור: אנחנו סופרים את כל ההזמנות בטווח, *חוץ* מההזמנה שאנחנו כרגע מעדכנים
            string sqlCount = string.Format(
                "SELECT COUNT(*) FROM MyBooking WHERE Restaurant=? AND InvDate=? " +
                "AND {0} AND TableType=? AND NOT (PhoneNum=? AND InvDate=? AND InvTime=?)", timeCondition);

            OleDbCommand cmd = new OleDbCommand(sqlCount, con);
            cmd.Parameters.AddWithValue("?", res);
            cmd.Parameters.AddWithValue("?", date);
            cmd.Parameters.AddWithValue("?", start);
            cmd.Parameters.AddWithValue("?", end);
            cmd.Parameters.AddWithValue("?", type);
            cmd.Parameters.AddWithValue("?", Session["CurrentPhone"]);
            cmd.Parameters.AddWithValue("?", Session["OldDate"]);
            cmd.Parameters.AddWithValue("?", Session["OldTime"]);

            int occupied = Convert.ToInt32(cmd.ExecuteScalar());
            return occupied < total;
        }

        protected void RepeaterTimes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string newTime = e.CommandArgument.ToString();
            UpdateInDB(newTime);
        }

        private void UpdateInDB(string newTime)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string sql = "UPDATE MyBooking SET InvDate = ?, InvTime = ?, NumGuest = ?, TableType = ? " +
                             "WHERE PhoneNum = ? AND InvDate = ? AND InvTime = ?";

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.Parameters.AddWithValue("?", txtDate.Text);
                cmd.Parameters.AddWithValue("?", newTime);
                cmd.Parameters.AddWithValue("?", txtNumGuests.Text);
                cmd.Parameters.AddWithValue("?", Session["SelectedTypeUpdate"]);
                cmd.Parameters.AddWithValue("?", Session["CurrentPhone"]);
                cmd.Parameters.AddWithValue("?", Session["OldDate"]);
                cmd.Parameters.AddWithValue("?", Session["OldTime"]);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "ההזמנה עודכנה בהצלחה לשעה " + newTime + "!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                pnlDetails.Visible = false;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string sql = "DELETE FROM MyBooking WHERE PhoneNum = ? AND InvDate = ? AND InvTime = ?";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", Session["CurrentPhone"]);
                cmd.Parameters.AddWithValue("?", Session["OldDate"]);
                cmd.Parameters.AddWithValue("?", Session["OldTime"]);

                con.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "ההזמנה בוטלה ונמחקה מהמערכת.";
                lblMessage.ForeColor = System.Drawing.Color.Orange;
                pnlDetails.Visible = false;
            }
        }
    }
}