using System;
using System.Data;
using System.Data.OleDb;

namespace ArielProject
{
    public partial class Booking : System.Web.UI.Page
    {
        // משתנים של המחלקה - מחזיקים את שעות הפתיחה של המסעדה
        // משמשים במקום פרמטרים מסוג out
        int openHour;
        int openMinute;
        int closeHour;
        int closeMinute;

        protected void Page_Load(object sender, EventArgs e)
        {
            // אם המשתמש לא מחובר - חוזרים לדף ההתחברות
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            // רק בטעינה הראשונה של הדף
            if (!IsPostBack)
            {
                // מציגים את שם המשתמש המחובר בכותרת
                LblUserName.Text = Session["User"].ToString();

                // מציגים את שם המסעדה שהגיע בכתובת
                if (Request.QueryString["res"] != null)
                {
                    LblResName.Text = Request.QueryString["res"].ToString();
                }

                // הבדיקה שהתאריך עתידי הועברה לפונקציית BtnCheckTimes_Click,
                // במקום הגדרה דינמית של ValueToCompare על ה-Validator
                // (שלא תמיד נלמד בכיתה).
            }
        }

        // לחיצה על "מצאו לי שולחן"
        protected void BtnCheckTimes_Click(object sender, EventArgs e)
        {
            if (TxtDate.Text == "" || TxtGuests.Text == "")
            {
                LblMsg.Text = "נא להזין תאריך ומספר סועדים";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // בדיקה שהתאריך עתידי (החליף את CompareValidator).
            // משווים את התאריך שהוזן לתאריך של היום.
            DateTime chosen = DateTime.Parse(TxtDate.Text);
            if (chosen < DateTime.Today)
            {
                LblMsg.Text = "נדרש לבחור תאריך עתידי";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            ShowAvailableTimes();
        }

        // בונה את טבלת השעות ומציג אותה ב-GridView
        private void ShowAvailableTimes()
        {
            string res = LblResName.Text;
            string date = TxtDate.Text;
            int guests = int.Parse(TxtGuests.Text);

            // קביעת סוג השולחן לפי מספר הסועדים
            string tableType;
            string typeName;
            if (guests <= 2)
            {
                tableType = "SmallTables";
                typeName = "Small";
            }
            else if (guests <= 4)
            {
                tableType = "MediumTables";
                typeName = "Medium";
            }
            else
            {
                tableType = "LargeTables";
                typeName = "Large";
            }

            Session["SelectedType"] = typeName;

            // מתחברים למסד הנתונים ומקבלים את מספר השולחנות במסעדה
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sqlCap = "SELECT " + tableType + " FROM MyRestaurants WHERE Restaurants = '" + res + "'";
            OleDbCommand cmdCap = new OleDbCommand(sqlCap, con);
            con.Open();
            int totalTables = int.Parse(cmdCap.ExecuteScalar().ToString());
            con.Close();

            // קובעים את שעות הפתיחה של המסעדה לפי היום בשבוע
            // במקום להשתמש ב-enum בשם DayOfWeek (שלא נלמד),
            // אנחנו ממירים את היום בשבוע למספר שלם:
            // 0=ראשון, 1=שני, 2=שלישי, 3=רביעי, 4=חמישי, 5=שישי, 6=שבת
            DateTime selectedDate = DateTime.Parse(date);
            int dayNum = (int)selectedDate.DayOfWeek;
            GetOpeningHours(res, dayNum);

            // אפשר להזמין רק עד שעתיים לפני הסגירה
            int startMinutes = openHour * 60 + openMinute;
            int endMinutes = closeHour * 60 + closeMinute - 120;

            // אם המסעדה סגורה ביום הזה
            // (הוספתי סוגריים מפורשים כדי שיהיה ברור איזו פעולה נעשית קודם:
            // קודם בודקים את כל ה-AND שבסוגריים, ואחר כך משווים ל-OR.)
            if ((openHour == 0 && closeHour == 0) || (startMinutes > endMinutes))
            {
                LblMsg.Text = "המסעדה אינה מקבלת הזמנות בתאריך זה";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                GridView1.DataSource = null;
                GridView1.DataBind();
                GridView1.Visible = false;
                return;
            }

            // בונים טבלה עם השעות והסטטוס שלהן
            DataTable dt = new DataTable();
            dt.Columns.Add("שעה");
            dt.Columns.Add("סטטוס");

            int currentMinutes = startMinutes;
            while (currentMinutes <= endMinutes)
            {
                // ממירים את הדקות בחזרה לשעה:דקה
                int h = currentMinutes / 60;
                int m = currentMinutes % 60;
                if (h >= 24) h = h - 24;  // טיפול בשעות שעוברות חצות

                string timeStr = "";
                if (h < 10) timeStr = timeStr + "0";
                timeStr = timeStr + h + ":";
                if (m < 10) timeStr = timeStr + "0";
                timeStr = timeStr + m;

                bool isAvail = CheckSpecificTime(timeStr, res, date, totalTables, typeName);

                string statusText;
                if (isAvail) statusText = "פנוי";
                else statusText = "תפוס";

                dt.Rows.Add(timeStr, statusText);

                currentMinutes = currentMinutes + 30;
            }

            LblMsg.Text = "";
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Visible = true;
        }

        // קובע את שעות הפתיחה והסגירה לפי המסעדה והיום בשבוע.
        // השעות נשמרות בטבלת MyRestaurants ב-6 עמודות: WeekdayOpen, WeekdayClose,
        // FriOpen, FriClose, SatOpen, SatClose - כל אחת בפורמט "HH:MM".
        // שעות שעוברות חצות מיוצגות עם מספר גדול מ-24 (למשל 25:00 = 01:00 למחרת).
        // אם הפתיחה והסגירה שתיהן 00:00 פירושו שהמסעדה סגורה.
        // הפרמטר day הוא מספר שלם: 0=ראשון .. 5=שישי, 6=שבת.
        private void GetOpeningHours(string res, int day)
        {
            // ברירת מחדל - שעות גנריות אם המסעדה לא נמצאת בטבלה
            openHour = 18; openMinute = 0;
            closeHour = 23; closeMinute = 30;

            // בוחרים את זוג העמודות הנכון לפי היום:
            // יום 5 = שישי, יום 6 = שבת, וכל השאר (0-4 = ראשון-חמישי) = חול
            string openCol, closeCol;
            if (day == 5) { openCol = "FriOpen"; closeCol = "FriClose"; }
            else if (day == 6) { openCol = "SatOpen"; closeCol = "SatClose"; }
            else { openCol = "WeekdayOpen"; closeCol = "WeekdayClose"; }

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            // שאילתה ששולפת בדיוק את 2 הערכים שצריך - השעה הפותחת והסוגרת
            string sql = "SELECT " + openCol + ", " + closeCol +
                         " FROM MyRestaurants WHERE Restaurants = '" + res + "'";
            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // הערכים בפורמט "HH:MM" - מפצלים לפי ':' לקבלת שעה ודקה
                string[] openParts = reader[openCol].ToString().Split(':');
                openHour = int.Parse(openParts[0]);
                openMinute = int.Parse(openParts[1]);

                string[] closeParts = reader[closeCol].ToString().Split(':');
                closeHour = int.Parse(closeParts[0]);
                closeMinute = int.Parse(closeParts[1]);
            }
            con.Close();
        }

        // בודק האם שעה מסוימת פנויה - סופר כמה שולחנות תפוסים בטווח של שעתיים סביבה
        private bool CheckSpecificTime(string timeToCheck, string res, string date, int total, string type)
        {
            DateTime dt = DateTime.Parse(timeToCheck);
            string start = dt.AddHours(-2).ToString("HH:mm");
            string end = dt.AddHours(2).ToString("HH:mm");

            // אם הטווח עובר חצות - צריך תנאי OR במקום AND
            string timeCondition;
            if (String.Compare(start, end) > 0)
            {
                timeCondition = "(InvTime > '" + start + "' OR InvTime < '" + end + "')";
            }
            else
            {
                timeCondition = "(InvTime > '" + start + "' AND InvTime < '" + end + "')";
            }

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sqlCount = "SELECT COUNT(*) FROM MyBooking WHERE Restaurant='" + res +
                              "' AND InvDate=#" + date + "# AND " + timeCondition +
                              " AND TableType='" + type + "'";

            OleDbCommand cmd = new OleDbCommand(sqlCount, con);
            con.Open();
            int occupied = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();

            return occupied < total;
        }

        // המשתמש בחר שעה בטבלה
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // אינדקס 0 הוא כפתור הבחירה, אינדקס 1 השעה, אינדקס 2 הסטטוס
            string time = GridView1.SelectedRow.Cells[1].Text;
            string status = GridView1.SelectedRow.Cells[2].Text;

            if (status == "תפוס")
            {
                LblMsg.Text = "השעה תפוסה, נא לבחור שעה אחרת";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            SaveToDB(time);
        }

        // שמירת ההזמנה במסד הנתונים
        private void SaveToDB(string finalTime)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "INSERT INTO MyBooking (Guest, PhoneNum, InvDate, NumGuest, InvTime, Restaurant, TableType) " +
                         "VALUES ('" + Session["User"] + "', '" + Session["Phone"] + "', #" + TxtDate.Text +
                         "#, '" + TxtGuests.Text + "', '" + finalTime + "', '" + LblResName.Text +
                         "', '" + Session["SelectedType"] + "')";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // הצגת הודעת הצלחה והסתרת רשימת השעות
            LblMsg.Text = "ההזמנה לשעה " + finalTime + " בוצעה בהצלחה!";
            LblMsg.ForeColor = System.Drawing.Color.Green;
            GridView1.Visible = false;

            // הצגת הצעת ההסעה
            Session["BookedTime"] = finalTime;
            LblTaxiQuestion.Text = "מעוניין בהסעה בשעה " + finalTime + "?";
            TaxiPanel.Visible = true;

            // עצירת טיימר ההזמנה (קריאה ל-JavaScript)
            // הוחלף מ-Literal ל-Label כי Label נלמד בכיתה.
            // הטקסט שמוצב יוזרק לדף בתור HTML ולכן הסקריפט ירוץ.
            LblClearTimer.Text = "<script>clearBookingTimer();</script>";
        }

        // הלקוח לחץ "כן, הזמינו לי הסעה"
        protected void BtnTaxiYes_Click(object sender, EventArgs e)
        {
            BtnTaxiYes.Visible = false;
            BtnTaxiNo.Visible = false;
            LblTaxiQuestion.Text = "אנא הזן את כתובת האיסוף";
            AddressPanel.Visible = true;
        }

        // הלקוח לחץ "לא תודה"
        protected void BtnTaxiNo_Click(object sender, EventArgs e)
        {
            LblTaxiResult.Text = "תודה! נשמח לראותך במסעדה.";
            LblTaxiResult.ForeColor = System.Drawing.Color.DarkGreen;
            BtnTaxiYes.Visible = false;
            BtnTaxiNo.Visible = false;
            LblTaxiQuestion.Visible = false;
        }

        // הלקוח אישר את הכתובת - בדיקה ואז קריאה לספק ההסעה
        protected void BtnConfirmAddress_Click(object sender, EventArgs e)
        {
            string city = TxtCity.Text.Trim();
            string street = TxtStreet.Text.Trim();
            string house = TxtHouseNum.Text.Trim();

            // בדיקה בסיסית - כל השדות חייבים להיות מלאים
            if (city == "" || street == "" || house == "")
            {
                LblAddressError.Text = "יש למלא עיר, רחוב ומספר בית.";
                return;
            }

            // בדיקה שמספר הבית מכיל רק ספרות (במקום try/catch על int.Parse).
            // עוברים תו-תו על המחרוזת ומוודאים שכל תו נמצא בין '0' ל-'9'.
            // נשמר רק אם נלמד char והשוואת תווים – זו דרך בסיסית בלי try/catch.
            bool isAllDigits = true;
            for (int i = 0; i < house.Length; i++)
            {
                if (house[i] < '0' || house[i] > '9')
                {
                    isAllDigits = false;
                }
            }
            if (!isAllDigits)
            {
                LblAddressError.Text = "מספר בית חייב להיות מספר.";
                return;
            }
            int houseNum = int.Parse(house);
            if (houseNum <= 0)
            {
                LblAddressError.Text = "מספר בית חייב להיות מספר חיובי.";
                return;
            }

            // בדיקה שהעיר קיימת בטבלת הערים שבמסד הנתונים
            string connStrCity = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection conCity = new OleDbConnection(connStrCity);
            string sqlCity = "SELECT COUNT(*) FROM Cities WHERE CityName = '" + city + "'";
            OleDbCommand cmdCity = new OleDbCommand(sqlCity, conCity);
            conCity.Open();
            int cityCount = int.Parse(cmdCity.ExecuteScalar().ToString());
            conCity.Close();

            if (cityCount == 0)
            {
                LblAddressError.Text = "העיר \"" + city + "\" לא נמצאה במאגר היישובים.";
                return;
            }

            // הכתובת תקינה - מזמינים את ההסעה
            LblAddressError.Text = "";
            string finalTime = "";
            if (Session["BookedTime"] != null)
            {
                finalTime = Session["BookedTime"].ToString();
            }
            string fullAddress = street + " " + houseNum + ", " + city;

            try
            {
                TaxiServiceAPI.WebService1SoapClient taxi = new TaxiServiceAPI.WebService1SoapClient();
                string taxiResponse = taxi.BookRide(Session["User"].ToString(), LblResName.Text, finalTime, fullAddress);

                LblTaxiResult.Text = "<b>הודעה מחברת ההסעות:</b><br/>" + taxiResponse;
                LblTaxiResult.ForeColor = System.Drawing.Color.DarkGreen;
            }
            catch
            {
                LblTaxiResult.Text = "שגיאה בחיבור לחברת ההסעות. נסה שוב מאוחר יותר.";
                LblTaxiResult.ForeColor = System.Drawing.Color.Red;
            }

            // אחרי הזמנה מוצלחת - מסתירים את כל פאנל הכתובת והשאלה
            AddressPanel.Visible = false;
            LblTaxiQuestion.Visible = false;
        }

    }
}
