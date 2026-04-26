using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
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

            // שאילתה בסיסית
            string strsql = "SELECT * FROM MyRestaurants WHERE 1=1 ";

            // סינון אזור
            if (DdlRegion.SelectedValue != "הכל")
            {
                strsql += " AND Region = '" + DdlRegion.SelectedValue + "' ";
            }

            // סינון סוג מטבח
            if (DdlType.SelectedValue != "הכל")
            {
                strsql += " AND FoodType = '" + DdlType.SelectedValue + "' ";
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

            OleDbCommand cmd = new OleDbCommand(strsql, con);
            OleDbDataReader dr = cmd.ExecuteReader();

            DataListRestaurants.DataSource = dr;
            DataListRestaurants.DataBind();

            con.Close();
        }
    }
}