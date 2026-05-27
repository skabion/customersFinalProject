using System;
using System.Data.OleDb;
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

            // 2. בניית השאילתה עם פרמטרים (מונע SQL Injection).
            // במקום לשרשר את הקלט לתוך המחרוזת, שמים ? ומעבירים את הערך בנפרד.
            string strsql = "SELECT * FROM MyUsers WHERE MyFullName = ? AND MyPassword = ?";
            OleDbCommand cmd = new OleDbCommand(strsql, con);
            cmd.Parameters.AddWithValue("@MyFullName", TxtFullName.Text);
            cmd.Parameters.AddWithValue("@MyPassword", TxtPassword.Text);
            OleDbDataReader dr = cmd.ExecuteReader();

            // 3. בדיקה אם חזרו נתונים
            if (dr.HasRows)
            {
                dr.Read(); // קריאת השורה שנמצאה
                Session["User"] = dr["MyFullName"].ToString();

                // שמירת הטלפון מהטבלה לתוך ה-Session
                Session["Phone"] = dr["MyPhoneNumber"].ToString();

                // אם המשתמש הוא מנהל מסעדה - שומרים את שם המסעדה ב-Session.
                // הערך בעמודה הוא "לא" עבור מי שאינו מנהל, אחרת שם המסעדה.

                string restAdmin = dr["RestaurantAdmin"].ToString().Trim();
                if (restAdmin != "")
                {
                    Session["RestaurantAdmin"] = restAdmin;
                }

                // אם המשתמש הוא מנהל מערכת (Admin = "כן") - שומרים אותו ב-Session.
                string adminFlag = dr["Admin"].ToString().Trim();
                if (adminFlag == "כן")
                {
                    Session["Admin"] = true;
                }

                // סוגרים את החיבור גם כשההתחברות מצליחה (תוקן - קודם זה היה רק ב-else)
                con.Close();

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