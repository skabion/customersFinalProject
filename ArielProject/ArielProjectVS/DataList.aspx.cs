using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArielProject
{
    public partial class DataList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
                con.Open();

               string strsql = "SELECT * FROM MyRestaurants ";

                OleDbCommand Cmd = new OleDbCommand(strsql, con);
                OleDbDataReader dr1 = Cmd.ExecuteReader();
                dr1.Read();
                DataList1.DataSource = dr1;
                DataList1.DataBind();

                con.Close();

            }
        }
    }
}