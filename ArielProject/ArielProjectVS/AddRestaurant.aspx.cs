using System;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{

    public partial class AddRestaurant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // אימות 1: המשתמש חייב להיות מחובר
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // אימות 2: רק מנהל מערכת רשאי להוסיף מסעדות (לא מנהל מסעדה רגיל)
            if (Session["Admin"] == null)
            {
                Response.Redirect("HomePage.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();
        }

        // לחיצה על "הוסף מסעדה לקטלוג"
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            // מנקים הודעות קודמות
            LblError.Text = "";
            LblSuccess.Text = "";

            string name = TxtName.Text.Trim();

            // בדיקה 1: שם המסעדה לא ריק
            if (name == "")
            {
                LblError.Text = "נא להזין שם מסעדה.";
                return;
            }

            // בדיקה 2: שלושת שדות השולחנות חייבים להיות מספרים שלמים אי-שליליים.
            // העמודות נשמרות כטקסט במסד, אבל דף ההזמנה עושה int.Parse עליהן -
            // ערך ריק או לא-מספרי יפיל את דף ההזמנה, לכן מוודאים כאן.
            if (!IsNonNegativeInt(TxtSmall.Text) ||
                !IsNonNegativeInt(TxtMedium.Text) ||
                !IsNonNegativeInt(TxtLarge.Text))
            {
                LblError.Text = "מספר השולחנות בכל שדה חייב להיות מספר שלם (0 ומעלה).";
                return;
            }

            int small = int.Parse(TxtSmall.Text);
            int medium = int.Parse(TxtMedium.Text);
            int large = int.Parse(TxtLarge.Text);

            // בדיקה 3: חייב להיות לפחות שולחן אחד, אחרת אי אפשר יהיה להזמין במסעדה
            if (small + medium + large == 0)
            {
                LblError.Text = "המסעדה חייבת לכלול לפחות שולחן אחד מסוג כלשהו.";
                return;
            }

            // בדיקה 4: שלא קיימת כבר מסעדה בשם הזה.
            // אקסס משווה טקסט ללא תלות ברישיות, כך ש-"bobo" ייחשב זהה ל-"Bobo".
            if (RestaurantExists(name))
            {
                LblError.Text = "כבר קיימת מסעדה בשם \"" + name + "\".";
                return;
            }

            // הכל תקין - מכניסים את המסעדה למסד
            InsertRestaurant(name, small, medium, large);

            // הצלחה - מנקים את הטופס ומציגים הודעה.
            // מרגע זה המסעדה מופיעה אוטומטית בקטלוג וניתנת להזמנה.
            LblSuccess.Text = "המסעדה \"" + name + "\" נוספה לקטלוג בהצלחה!";
            ClearForm();
        }

        // עזר: מחזיר true אם המחרוזת היא מספר שלם אי-שלילי (0 ומעלה).
        // int.TryParse מטפל בריק/לא-מספרי/חריגה מהטווח בלי לזרוק שגיאה.
        private bool IsNonNegativeInt(string s)
        {
            int val;
            if (!int.TryParse(s, out val))
                return false;
            return val >= 0;
        }

        // בודק אם כבר קיימת מסעדה עם השם הזה - שאילתת COUNT מפרמטרת
        private bool RestaurantExists(string name)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT COUNT(*) FROM MyRestaurants WHERE Restaurants = ?";
            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.Parameters.AddWithValue("?", name);
            con.Open();
            int count = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();

            return count > 0;
        }

        // מכניס שורת מסעדה חדשה לטבלת MyRestaurants
        private void InsertRestaurant(string name, int small, int medium, int large)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);
            con.Open();

            // מחשבים מזהה רץ חדש (MAX+1) כדי לשמור רצף עם השורות הקיימות.
            // אם הטבלה ריקה MAX מחזיר NULL, ואז מתחילים מ-1.
            OleDbCommand cmdMax = new OleDbCommand("SELECT MAX(UserID) FROM MyRestaurants", con);
            object maxObj = cmdMax.ExecuteScalar();
            int nextId = 1;
            if (maxObj != null && maxObj != DBNull.Value)
                nextId = int.Parse(maxObj.ToString()) + 1;

            // כשרות ותחלופה נשמרים כ"כן"/"לא" - בדיוק כפי שהקטלוג מסנן לפיהם
            string kosher;
            if (ChkKosher.Checked)
                kosher = "כן";
            else
                kosher = "לא";

            string replacement;
            if (ChkReplacement.Checked)
                replacement = "כן";
            else
                replacement = "לא";

            // INSERT מפרמטר - כל ערך מיוצג ע"י ? ומועבר בנפרד, כך שהקלט
            // אף פעם לא הופך לקוד SQL (מונע SQL Injection). הסדר חשוב ב-OleDb.
            string sql = "INSERT INTO MyRestaurants " +
                         "(UserID, Restaurants, Region, FoodType, Kosher, ReplacementMeals, " +
                         "SmallTables, MediumTables, LargeTables) " +
                         "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.Parameters.AddWithValue("?", nextId);
            cmd.Parameters.AddWithValue("?", name);
            cmd.Parameters.AddWithValue("?", DdlRegion.SelectedValue);
            cmd.Parameters.AddWithValue("?", DdlType.SelectedValue);
            cmd.Parameters.AddWithValue("?", kosher);
            cmd.Parameters.AddWithValue("?", replacement);
            // עמודות השולחנות הן טקסט במסד, לכן שומרים אותן כמחרוזת
            cmd.Parameters.AddWithValue("?", small.ToString());
            cmd.Parameters.AddWithValue("?", medium.ToString());
            cmd.Parameters.AddWithValue("?", large.ToString());
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // מאפס את הטופס אחרי הוספה מוצלחת, כדי שאפשר יהיה להוסיף מסעדה נוספת
        private void ClearForm()
        {
            TxtName.Text = "";
            TxtSmall.Text = "";
            TxtMedium.Text = "";
            TxtLarge.Text = "";
            ChkKosher.Checked = false;
            ChkReplacement.Checked = false;
            DdlRegion.SelectedIndex = 0;
            DdlType.SelectedIndex = 0;
        }
    }
}
