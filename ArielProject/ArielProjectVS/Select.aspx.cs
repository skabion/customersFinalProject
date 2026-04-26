using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;


namespace ArielProject
{
    public partial class calculator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SelectUsers(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            con.Open();

            string selectedRegion = ddlRegion.SelectedValue; // בחר את האזור שנבחר בתפריט

            string strsql;
            if (selectedRegion == "All Regions") // אם נבחר "כל האזורים", מיינת לפי אזור
            {
                strsql = "SELECT * FROM MyRestaurants ORDER BY Region ASC, Restaurants ASC";
            }
            else
            {
                strsql = "SELECT * FROM MyRestaurants WHERE Region = @Region ORDER BY Restaurants ASC";
            }

            OleDbCommand cmd = new OleDbCommand(strsql, con);
            cmd.Parameters.AddWithValue("@Region", selectedRegion); // הוסף את האזור כפרמטר

            OleDbDataReader Dr = cmd.ExecuteReader();
            GV1.DataSource = Dr;
            GV1.DataBind();

            con.Close();
        }


    }
}