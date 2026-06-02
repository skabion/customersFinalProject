using System;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArielProject
{
    public partial class Catalog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            con.Open();

            // בונים את השאילתה דינמית לפי הפילטרים הנבחרים, ומוסיפים פרמטר
            // לכל סינון משתמש (Region/FoodType) - כך הערך מועבר בנפרד ולא משורשר.
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;

            string strsql = "SELECT * FROM MyRestaurants WHERE 1=1 ";

            // סינון אזור
            if (DdlRegion.SelectedValue != "הכל")
            {
                strsql += " AND Region = ? ";
                cmd.Parameters.AddWithValue("?", DdlRegion.SelectedValue);
            }

            // סינון סוג מטבח
            if (DdlType.SelectedValue != "הכל")
            {
                strsql += " AND FoodType = ? ";
                cmd.Parameters.AddWithValue("?", DdlType.SelectedValue);
            }

            // סינון כשרות
            if (ChkKosher.Checked)
            {
                strsql += " AND Kosher = 'כן' ";
            }

            if (ChkReplacementMeals.Checked)
            {
                strsql += " AND ReplacementMeals = 'כן' ";
            }

            cmd.CommandText = strsql;
            OleDbDataReader dr = cmd.ExecuteReader();

            DataListRestaurants.DataSource = dr;
            DataListRestaurants.DataBind();

            con.Close();
        }
    }
}