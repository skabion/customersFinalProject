using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    public class BookingItem
    {
        public string Restaurant { get; set; }
        public string DateStr { get; set; }
        public string InvTime { get; set; }
        public string NumGuest { get; set; }
        public string TableType { get; set; }
    }

    public partial class MyBookings : System.Web.UI.Page
    {
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\DBusers1.accdb";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null || Session["Phone"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            if (!IsPostBack)
            {
                LoadFutureBookings();
            }
        }

        private void LoadFutureBookings()
        {
            List<BookingItem> bookings = new List<BookingItem>();

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string sql = "SELECT Restaurant, InvDate, InvTime, NumGuest, TableType " +
                             "FROM MyBooking " +
                             "WHERE PhoneNum = ? AND InvDate >= ? " +
                             "ORDER BY InvDate, InvTime";

                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", Session["Phone"].ToString());
                cmd.Parameters.AddWithValue("?", DateTime.Today);

                con.Open();
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BookingItem item = new BookingItem();
                    item.Restaurant = reader["Restaurant"].ToString();
                    item.DateStr = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                    item.InvTime = reader["InvTime"].ToString();
                    item.NumGuest = reader["NumGuest"].ToString();
                    item.TableType = TranslateTableType(reader["TableType"].ToString());
                    bookings.Add(item);
                }
            }

            if (bookings.Count == 0)
            {
                RepeaterBookings.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                RepeaterBookings.DataSource = bookings;
                RepeaterBookings.DataBind();
            }
        }

        private string TranslateTableType(string type)
        {
            switch (type)
            {
                case "Small": return "קטן (עד 2 סועדים)";
                case "Medium": return "בינוני (3-4 סועדים)";
                case "Large": return "גדול (5+ סועדים)";
                default: return type;
            }
        }
    }
}
