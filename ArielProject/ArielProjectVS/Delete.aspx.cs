using System;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnDeleteHistory_Click(object sender, EventArgs e)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            OleDbConnection con = new OleDbConnection(connStr);

            // 1. נשתמש בפורמט חודש/יום/שנה כי אקסס "מדבר" אמריקאית בשאילתות
            string today = DateTime.Now.ToString("MM/dd/yyyy");

            // 2. נחליף את הגרשיים (') בסולמיות (#)
            string strsql = "DELETE FROM MyBooking WHERE InvDate < #" + today + "#";

            OleDbCommand cmd = new OleDbCommand(strsql, con);

            con.Open();
            int rows = cmd.ExecuteNonQuery();
            con.Close();

            LblStatus.Text = "נמחקו " + rows + " הזמנות.";
        }

    }
}
