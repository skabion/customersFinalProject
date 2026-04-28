using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArielProject
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            // 1. הגדרת החיבור לאקסס
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            con.Open();

            // 2. בניית השאילתה בשיטת שרשור מחרוזות
            string strsql = "SELECT * FROM MyUsers WHERE MyFullName = '" + TxtFullName.Text + "' AND MyPassword = '" + TxtPassword.Text + "'";
            OleDbCommand cmd = new OleDbCommand(strsql, con);
            OleDbDataReader dr = cmd.ExecuteReader();

            // 3. בדיקה אם חזרו נתונים
            if (dr.HasRows)
            {
                dr.Read(); // קריאת השורה שנמצאה
                Session["User"] = dr["MyFullName"].ToString();

                // השורה החדשה: שמירת הטלפון מהטבלה לתוך ה-Session
                // (וודא ששם העמודה ב-MyUsers הוא אכן PhoneNum)
                Session["Phone"] = dr["MyPhoneNumber"].ToString();

                Response.Redirect("HomePage.aspx");
            }
            else
            {
                // המשתמש לא נמצא - סוגרים את החיבור קודם
                con.Close();

                // מציגים הודעת שגיאה
                LblError.Text = "שם משתמש או סיסמה שגויים, נסה שוב.";
            }
        }
    }
}