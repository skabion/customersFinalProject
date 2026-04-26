using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArielProject
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
                con.Open();
                string strsql = "SELECT Ezor FROM HlpTbl ORDER BY Ezor Desc";
                OleDbCommand cmd = new OleDbCommand(strsql, con);
                OleDbDataReader dr = cmd.ExecuteReader();
                DropDownList1.DataSource = dr;
                DropDownList1.DataTextField = "Ezor";
                DropDownList1.DataBind();
                con.Close();
            }
        }

        protected void AddUser_Click(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            con.Open();

            // העדפות
            string vegetarian = CheckBoxVegetarian.Checked ? "כן" : "לא";
            string vegan = CheckBoxVegan.Checked ? "כן" : "לא";
            string kosher = CheckBoxKosher.Checked ? "כן" : "לא";

            // אלרגיות
            string gluten = CheckBoxGluten.Checked ? "כן" : "לא";
            string peanuts = CheckBoxPeanuts.Checked ? "כן" : "לא";
            string treeNuts = CheckBoxTreeNuts.Checked ? "כן" : "לא";
            string fish = CheckBoxFish.Checked ? "כן" : "לא";
            string sesame = CheckBoxSesame.Checked ? "כן" : "לא";
            string milk = CheckBoxMilk.Checked ? "כן" : "לא";

            // אזור
            string area = DropDownList1.SelectedItem.Text;  // Darom / Merkaz / Tzafon

            // שים לב: כאן משתמשים בדיוק בשמות העמודות מהטבלה שלך!
            string strsql =
                "INSERT INTO MyUsers " +
                "(MyFullName, MyPassword, MyPhoneNumber, " +
                "Vegetarian, Vegan, Kosher, " +
                "Gluten, Peanuts, TreeNuts, Fish, Sesame, Milk, " +
                "Area) " +
                "VALUES (" +
                "'" + TextBox1.Text + "'," +      // MyFullName
                "'" + TextBox2.Text + "'," +      // MyPassword
                "'" + TextBox3.Text + "'," +      // MyPhoneNumber
                "'" + vegetarian + "'," +
                "'" + vegan + "'," +
                "'" + kosher + "'," +
                "'" + gluten + "'," +
                "'" + peanuts + "'," +
                "'" + treeNuts + "'," +
                "'" + fish + "'," +
                "'" + sesame + "'," +
                "'" + milk + "'," +
                "'" + area + "'" +
                ")";

            OleDbCommand cmd = new OleDbCommand(strsql, con);
            int y = 0;
            y = cmd.ExecuteNonQuery();
            Response.Write(y);
            con.Close();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownListArea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
