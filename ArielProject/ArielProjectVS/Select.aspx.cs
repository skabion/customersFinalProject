using System;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
// הוסרו using System.Collections.Generic, System.Linq, System.Web - לא היו בשימוש

namespace ArielProject
{
    // הערה: שם המחלקה calculator הוא שריד מתבנית ישנה (כנראה העתקה)
    // והוא משמש את Inherits ב-Select.aspx ("ArielProject.calculator").
    // השארתי כדי לא לפגוע ב-aspx.
    public partial class calculator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SelectUsers(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            con.Open();

            string selectedRegion = ddlRegion.SelectedValue;  // האזור שנבחר בתפריט

            // הוחלפה שאילתה עם פרמטר (@Region + cmd.Parameters.AddWithValue)
            // בשרשור מחרוזות פשוט - אותו סגנון כמו בשאר הדפים בפרוייקט.
            string strsql;
            if (selectedRegion == "All Regions")
            {
                // אם נבחר "כל האזורים" - מציגים את כל המסעדות ממוינות לפי אזור
                strsql = "SELECT * FROM MyRestaurants ORDER BY Region ASC, Restaurants ASC";
            }
            else
            {
                // אחרת - מסננים רק את האזור שנבחר
                strsql = "SELECT * FROM MyRestaurants WHERE Region = '" + selectedRegion + "' ORDER BY Restaurants ASC";
            }

            OleDbCommand cmd = new OleDbCommand(strsql, con);

            OleDbDataReader Dr = cmd.ExecuteReader();
            GV1.DataSource = Dr;
            GV1.DataBind();

            con.Close();
        }
    }
}
