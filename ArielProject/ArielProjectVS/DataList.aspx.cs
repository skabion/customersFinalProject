using System;
using System.Data.OleDb;
using System.Web.UI;
// הוסרו usings מיותרים: System.Collections.Generic, System.Data.SqlTypes,
// System.Drawing, System.Linq, System.Web, System.Web.UI.WebControls

namespace ArielProject
{
    public partial class DataList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // נורמליזציה: Server.MapPath עם שם הקובץ ישירות
                OleDbConnection con = new OleDbConnection();
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
                con.Open();

                string strsql = "SELECT * FROM MyRestaurants";

                OleDbCommand Cmd = new OleDbCommand(strsql, con);
                OleDbDataReader dr1 = Cmd.ExecuteReader();

                // תיקון באג: הוסרה הקריאה ל-dr1.Read() שהיתה כאן.
                // הקריאה הזו הזיזה את הסמן שורה אחת קדימה לפני ה-DataBind,
                // ולכן המסעדה הראשונה נדלגה. DataBind() כבר קורא את כל
                // השורות בעצמו - אין צורך לקרוא לפניו.
                DataList1.DataSource = dr1;
                DataList1.DataBind();

                con.Close();
            }
        }
    }
}
