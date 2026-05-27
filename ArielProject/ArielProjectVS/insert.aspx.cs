using System;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
// הוסרו using System.Collections.Generic ו-using System.Linq - לא היו בשימוש בקובץ

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
                string strsql = "SELECT Area FROM MyUsers ORDER BY Area Desc";
                OleDbCommand cmd = new OleDbCommand(strsql, con);
                OleDbDataReader dr = cmd.ExecuteReader();
                DropDownList1.DataSource = dr;
                DropDownList1.DataTextField = "Area";
                DropDownList1.DataBind();
                con.Close();
            }
        }

        protected void AddUser_Click(object sender, EventArgs e)
        {

            // מנקים הודעות שגיאה קודמות
            LblNameError.Text = "";
            LblPasswordError.Text = "";
            LblPhoneError.Text = "";

            // בודקים כל שדה. כל פונקציה מחזירה "" אם תקין,
            // או הודעת שגיאה אם לא תקין.
            string nameError = ValidateName(SignUp_FullName.Text);
            string passwordError = ValidatePassword(SignUp_Password.Text);
            string phoneError = ValidatePhone(SignUp_Phone.Text);

            bool hasError = false;
            if (nameError != "")
            {
                LblNameError.Text = nameError;
                hasError = true;
            }
            if (passwordError != "")
            {
                LblPasswordError.Text = passwordError;
                hasError = true;
            }
            if (phoneError != "")
            {
                LblPhoneError.Text = phoneError;
                hasError = true;
            }

            // אם יש לפחות שגיאה אחת - לא ממשיכים להכניס למסד
            if (hasError) return;

            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            con.Open();

            // העדפות תזונתיות - לכל צ'קבוקס בודקים אם הוא מסומן.
            // אם כן, שומרים "כן" במשתנה, אחרת "לא".
            string vegetarian;
            if (CheckBoxVegetarian.Checked)
                vegetarian = "כן";
            else
                vegetarian = "לא";

            string vegan;
            if (CheckBoxVegan.Checked)
                vegan = "כן";
            else
                vegan = "לא";

            string kosher;
            if (CheckBoxKosher.Checked)
                kosher = "כן";
            else
                kosher = "לא";

            // אלרגיות - אותה לוגיקה, לכל צ'קבוקס של אלרגיה
            string gluten;
            if (CheckBoxGluten.Checked)
                gluten = "כן";
            else
                gluten = "לא";

            string peanuts;
            if (CheckBoxPeanuts.Checked)
                peanuts = "כן";
            else
                peanuts = "לא";

            string treeNuts;
            if (CheckBoxTreeNuts.Checked)
                treeNuts = "כן";
            else
                treeNuts = "לא";

            string fish;
            if (CheckBoxFish.Checked)
                fish = "כן";
            else
                fish = "לא";

            string sesame;
            if (CheckBoxSesame.Checked)
                sesame = "כן";
            else
                sesame = "לא";

            string milk;
            if (CheckBoxMilk.Checked)
                milk = "כן";
            else
                milk = "לא";

            // אזור
            string area = DropDownList1.SelectedItem.Text;  // Darom / Merkaz / Tzafon

            // בניית השאילתה עם פרמטרים (מונע SQL Injection).
            // כל ערך מיוצג ע"י ? ומועבר בנפרד דרך Parameters - הקלט אף פעם לא הופך לקוד SQL.
            // ב-OleDb הסדר חשוב: הפרמטרים מתאימים לפי הסדר של ה-? בשאילתה.
            string strsql =
                "INSERT INTO MyUsers " +
                "(MyFullName, MyPassword, MyPhoneNumber, " +
                "Vegetarian, Vegan, Kosher, " +
                "Gluten, Peanuts, TreeNuts, Fish, Sesame, Milk, " +
                "Area) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            OleDbCommand cmd = new OleDbCommand(strsql, con);
            cmd.Parameters.AddWithValue("@MyFullName", SignUp_FullName.Text);
            cmd.Parameters.AddWithValue("@MyPassword", SignUp_Password.Text);
            cmd.Parameters.AddWithValue("@MyPhoneNumber", SignUp_Phone.Text);
            cmd.Parameters.AddWithValue("@Vegetarian", vegetarian);
            cmd.Parameters.AddWithValue("@Vegan", vegan);
            cmd.Parameters.AddWithValue("@Kosher", kosher);
            cmd.Parameters.AddWithValue("@Gluten", gluten);
            cmd.Parameters.AddWithValue("@Peanuts", peanuts);
            cmd.Parameters.AddWithValue("@TreeNuts", treeNuts);
            cmd.Parameters.AddWithValue("@Fish", fish);
            cmd.Parameters.AddWithValue("@Sesame", sesame);
            cmd.Parameters.AddWithValue("@Milk", milk);
            cmd.Parameters.AddWithValue("@Area", area);

            int y = cmd.ExecuteNonQuery();
            con.Close();

            if (y > 0)
            {
                Response.Redirect("Login.aspx");
            }
        }

        // בודק שם מלא: לפחות 2 מילים באנגלית, כל אחת מתחילה באות גדולה
        private string ValidateName(string name)
        {
            // בדיקה ראשונה - לא ריק
            if (name == "")
                return "לא הוזן שם מלא";

            int wordCount = 0;       // ספירת מילים שמצאנו
            bool prevSpace = true;   // האם התו הקודם היה רווח (או תחילת המחרוזת)

            // עוברים על כל תו במחרוזת
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (c == ' ')
                {
                    // רווח - סימן לסוף מילה (התו הבא יהיה תחילת מילה חדשה)
                    prevSpace = true;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // אות גדולה - תקין בכל מקום במילה.
                    // אם זה התו הראשון של מילה חדשה, מגדילים את מונה המילים.
                    if (prevSpace) wordCount++;
                    prevSpace = false;
                }
                else if (c >= 'a' && c <= 'z')
                {
                    // אות קטנה - תקין רק באמצע מילה, לא בהתחלה.
                    if (prevSpace)
                        return "כל מילה בשם חייבת להתחיל באות גדולה";
                    prevSpace = false;
                }
                else
                {
                    // תו שאינו אות אנגלית או רווח - לא תקין
                    return "השם יכול להכיל רק אותיות אנגלית ורווחים";
                }
            }

            // צריך לפחות 2 מילים
            if (wordCount < 2)
                return "השם חייב להכיל לפחות 2 מילים";

            return "";
        }

        // בודק סיסמה: לפחות 6 תווים, עם אות גדולה, אות קטנה, ספרה ותו מיוחד
        private string ValidatePassword(string pw)
        {
            // בדיקה ראשונה - לא ריק
            if (pw == "")
                return "לא הוזנה סיסמה";

            // בדיקת אורך מינימלי
            if (pw.Length < 6)
                return "הסיסמה חייבת להיות לפחות 6 תווים";

            // 4 דגלים - האם הופיעו: אות גדולה, אות קטנה, ספרה, תו מיוחד
            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            // עוברים תו-תו ובודקים לאיזו קטגוריה הוא שייך
            for (int i = 0; i < pw.Length; i++)
            {
                char c = pw[i];

                if (c >= 'A' && c <= 'Z')
                    hasUpper = true;
                else if (c >= 'a' && c <= 'z')
                    hasLower = true;
                else if (c >= '0' && c <= '9')
                    hasDigit = true;
                else if (c == '!' || c == '@' || c == '#' || c == '$' ||
                         c == '%' || c == '^' || c == '&')
                    hasSpecial = true;
                else
                    return "הסיסמה יכולה להכיל רק אותיות, ספרות ותווים מיוחדים: !@#$%^&";
            }

            // בודקים שמצאנו את כל 4 הסוגים
            if (!hasUpper)
                return "הסיסמה חייבת להכיל לפחות אות גדולה אחת";
            if (!hasLower)
                return "הסיסמה חייבת להכיל לפחות אות קטנה אחת";
            if (!hasDigit)
                return "הסיסמה חייבת להכיל לפחות ספרה אחת";
            if (!hasSpecial)
                return "הסיסמה חייבת להכיל לפחות תו מיוחד (!@#$%^&)";

            return "";
        }

        // בודק מספר טלפון: 10 ספרות שמתחילות ב-050/052/053/054/055
        private string ValidatePhone(string phone)
        {
            // בדיקה ראשונה - לא ריק
            if (phone == "")
                return "לא הוזן טלפון";

            // אורך חייב להיות בדיוק 10
            if (phone.Length != 10)
                return "מספר טלפון חייב להיות בדיוק 10 ספרות";

            // שני התווים הראשונים חייבים להיות "05"
            if (phone[0] != '0' || phone[1] != '5')
                return "מספר טלפון חייב להתחיל ב-05";

            // התו השלישי חייב להיות 0, 2, 3, 4 או 5
            char third = phone[2];
            if (third != '0' && third != '2' && third != '3' && third != '4' && third != '5')
                return "מספר טלפון חייב להתחיל ב-050, 052, 053, 054 או 055";

            // בודקים שכל שאר התווים (מתו 3 והלאה) הם ספרות
            for (int i = 3; i < phone.Length; i++)
            {
                if (phone[i] < '0' || phone[i] > '9')
                    return "מספר טלפון יכול להכיל ספרות בלבד";
            }

            return "";
        }
    }
}
