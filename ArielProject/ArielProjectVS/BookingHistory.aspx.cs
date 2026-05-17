using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    public class HistoryItem
    {
        public string Restaurant { get; set; }
        public string DateStr { get; set; }
        public string InvTime { get; set; }
        public string NumGuest { get; set; }
        public string TableType { get; set; }
        public string FoodType { get; set; }
        public string Region { get; set; }
    }

    public partial class BookingHistory : System.Web.UI.Page
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
                LoadHistory();
            }
        }

        protected void DdlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHistory();
        }

        private void LoadHistory()
        {
            List<HistoryItem> items = new List<HistoryItem>();

            // JOIN בין ההזמנות לטבלת המסעדות כדי לקבל גם FoodType ו-Region
            string orderBy = GetOrderByClause(DdlSort.SelectedValue);

            string sql =
                "SELECT b.Restaurant, b.InvDate, b.InvTime, b.NumGuest, b.TableType, r.FoodType, r.Region " +
                "FROM MyBooking AS b INNER JOIN MyRestaurants AS r ON b.Restaurant = r.Restaurants " +
                "WHERE b.PhoneNum = ? AND b.InvDate < ? " +
                orderBy;

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.Parameters.AddWithValue("?", Session["Phone"].ToString());
                cmd.Parameters.AddWithValue("?", DateTime.Today);

                con.Open();
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HistoryItem item = new HistoryItem();
                    item.Restaurant = reader["Restaurant"].ToString();
                    item.DateStr = Convert.ToDateTime(reader["InvDate"]).ToString("dd/MM/yyyy");
                    item.InvTime = reader["InvTime"].ToString();
                    item.NumGuest = reader["NumGuest"].ToString();
                    item.TableType = TranslateTableType(reader["TableType"].ToString());
                    item.FoodType = reader["FoodType"].ToString();
                    item.Region = reader["Region"].ToString();
                    items.Add(item);
                }
            }

            if (items.Count == 0)
            {
                RepeaterHistory.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                RepeaterHistory.Visible = true;
                PnlEmpty.Visible = false;
                RepeaterHistory.DataSource = items;
                RepeaterHistory.DataBind();
            }
        }

        private string GetOrderByClause(string sortKey)
        {
            // מיון משני קבוע לפי תאריך כדי לקבל סדר עקבי
            switch (sortKey)
            {
                case "DateAsc":
                    return "ORDER BY b.InvDate ASC, b.InvTime ASC";
                case "FoodType":
                    return "ORDER BY r.FoodType ASC, b.InvDate DESC";
                case "Region":
                    return "ORDER BY r.Region ASC, b.InvDate DESC";
                case "DateDesc":
                default:
                    return "ORDER BY b.InvDate DESC, b.InvTime DESC";
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
