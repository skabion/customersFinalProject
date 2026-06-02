using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{

    public partial class UpdateBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // בדיקת התחברות
            if (Session["User"] == null || Session["Phone"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            // קביעת תאריך מינימלי + ערך השוואה ל-Validator (גם בכל postback)
            string todayStr = DateTime.Today.ToString("yyyy-MM-dd");
            TxtDate.Attributes["min"] = todayStr;
            CompareValidatorDate.ValueToCompare = todayStr;

            if (!IsPostBack)
            {
                LoadFutureBookings();
            }
        }

        // ============================================================
        // מצב רשימה (PnlList): ההזמנות העתידיות של המשתמש
        // ============================================================

        // טוען את ההזמנות העתידיות של המשתמש מהמסד ומציג אותן ב-GridBookings
        private void LoadFutureBookings()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT Restaurant, InvDate, InvTime, NumGuest, TableType " +
                         "FROM MyBooking " +
                         "WHERE PhoneNum = ? AND InvDate >= ? " +
                         "ORDER BY InvDate, InvTime";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.Parameters.AddWithValue("?", Session["Phone"].ToString());
            cmd.Parameters.AddWithValue("?", DateTime.Today);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("מסעדה");
            dt.Columns.Add("תאריך");
            dt.Columns.Add("שעה");
            dt.Columns.Add("סועדים");
            dt.Columns.Add("סוג שולחן");

            while (reader.Read())
            {
                string restaurant = reader["Restaurant"].ToString();
                string date = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                string time = reader["InvTime"].ToString();
                string guests = reader["NumGuest"].ToString();
                string tableType = TranslateTableType(reader["TableType"].ToString());

                dt.Rows.Add(restaurant, date, time, guests, tableType);
            }
            con.Close();

            if (dt.Rows.Count == 0)
            {
                GridBookings.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                GridBookings.Visible = true;
                PnlEmpty.Visible = false;
                GridBookings.DataSource = dt;
                GridBookings.DataBind();
            }
        }

        // לחיצה על "ערוך" ליד הזמנה - מחליפים את הפאנל לטופס העריכה
        // וטוענים את פרטי ההזמנה הנבחרת. במקום Redirect לדף נפרד.
        protected void GridBookings_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cells[0]=כפתור, Cells[1]=מסעדה, Cells[2]=תאריך, Cells[3]=שעה
            string date = GridBookings.SelectedRow.Cells[2].Text;
            string time = GridBookings.SelectedRow.Cells[3].Text;

            // מעבר ממצב רשימה למצב עריכה
            PnlList.Visible = false;
            PnlEdit.Visible = true;

            LoadBooking(date, time);
        }

        // ============================================================
        // מצב עריכה (PnlEdit): טופס לעריכת הזמנה בודדת
        // ============================================================

        // טוען את פרטי ההזמנה הקיימת ומציג אותם בטופס
        private void LoadBooking(string date, string time)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection conn = new OleDbConnection(connStr);

            string sql = "SELECT * FROM MyBooking " +
                         "WHERE PhoneNum = ? AND InvDate = ? AND InvTime = ?";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.Parameters.AddWithValue("?", Session["Phone"].ToString());
            cmd.Parameters.AddWithValue("?", DateTime.Parse(date));
            cmd.Parameters.AddWithValue("?", time);
            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                LblResName.Text = reader["Restaurant"].ToString();
                TxtDate.Text = Convert.ToDateTime(reader["InvDate"]).ToString("yyyy-MM-dd");
                TxtNumGuests.Text = reader["NumGuest"].ToString();

                // שומרים ב-Session את מזהי ההזמנה המקורית -
                // נצטרך אותם בעדכון/מחיקה כדי למקם את השורה הנכונה במסד.
                Session["OldDate"] = TxtDate.Text;
                Session["OldTime"] = reader["InvTime"].ToString();

                LblMessage.Text = "ניתן לעדכן פרטים ולבחור שעה חדשה.";
                LblMessage.ForeColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                PnlEdit.Visible = false;
                PnlList.Visible = true;
                LblMessage.Text = "ההזמנה לא נמצאה.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
            }
            conn.Close();
        }

        // לחיצה על "בדוק שעות פנויות" - מאמתים תאריך ובונים את טבלת השעות
        protected void BtnCheckAvailability_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            DateTime chosen;
            try
            {
                chosen = DateTime.Parse(TxtDate.Text);
            }
            catch
            {
                LblMessage.Text = "תאריך לא תקין.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            // הגנה נוספת בצד השרת - גם אם ה-Validator עוקף, אסור תאריך עבר
            if (chosen.Date < DateTime.Today)
            {
                LblMessage.Text = "לא ניתן לעדכן הזמנה לתאריך שעבר.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            GenerateTimeSlots();
        }

        // בונה את טבלת השעות הפנויות ומציג אותה ב-GridTimes
        private void GenerateTimeSlots()
        {
            string res = LblResName.Text;
            string date = TxtDate.Text;
            int guests = int.Parse(TxtNumGuests.Text);

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

            Session["SelectedTypeUpdate"] = typeName;

            // שולפים מהמסד את מספר השולחנות מהסוג המבוקש
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sqlCap = "SELECT " + tableType + " FROM MyRestaurants WHERE Restaurants = ?";
            OleDbCommand cmdCap = new OleDbCommand(sqlCap, con);
            cmdCap.Parameters.AddWithValue("?", res);
            con.Open();
            int totalTables = int.Parse(cmdCap.ExecuteScalar().ToString());
            con.Close();

            DataTable dt = new DataTable();
            dt.Columns.Add("שעה");
            dt.Columns.Add("סטטוס");

            // קובעים את שעות הפתיחה לפי היום בשבוע (זהה לכל המסעדות).
            // 1=ראשון .. 5=חמישי: 9:00-23:00, 6=שישי: 8:00-16:00, 7=שבת: 19:30-23:30
            // מוסיפים 1 ל-DayOfWeek כי הוא מחזיר 0=ראשון, ואנחנו רוצים 1=ראשון.
            DateTime selectedDate = DateTime.Parse(date);
            int dayNum = (int)selectedDate.DayOfWeek + 1;

            int openHour, openMinute, closeHour, closeMinute;
            if (dayNum == 6)
            {
                openHour = 8; openMinute = 0;
                closeHour = 16; closeMinute = 0;
            }
            else if (dayNum == 7)
            {
                openHour = 19; openMinute = 30;
                closeHour = 23; closeMinute = 30;
            }
            else
            {
                openHour = 9; openMinute = 0;
                closeHour = 23; closeMinute = 0;
            }

            // אפשר להזמין רק עד שעתיים לפני הסגירה
            int startMinutes = openHour * 60 + openMinute;
            int endMinutes = closeHour * 60 + closeMinute - 120;

            int currentMinutes = startMinutes;
            while (currentMinutes <= endMinutes)
            {
                int h = currentMinutes / 60;
                int m = currentMinutes % 60;

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

            GridTimes.DataSource = dt;
            GridTimes.DataBind();
            GridTimes.Visible = true;
        }

        // בודק אם שעה ספציפית פנויה - סופר תפוסים בטווח 2+- שעות.
        // לא סופר את ההזמנה הנוכחית כדי שלא תיחשב כתפוסה ע"י עצמה.
        private bool CheckSpecificTime(string timeToCheck, string res, string date, int total, string type)
        {
            DateTime dt = DateTime.Parse(timeToCheck);
            string start = dt.AddHours(-2).ToString("HH:mm");
            string end = dt.AddHours(2).ToString("HH:mm");

            // אם הטווח חוצה חצות - תנאי OR במקום AND
            string timeCondition;
            if (start.CompareTo(end) > 0)
                timeCondition = "(InvTime > ? OR InvTime < ?)";
            else
                timeCondition = "(InvTime > ? AND InvTime < ?)";

            string sqlCount = "SELECT COUNT(*) FROM MyBooking " +
                              "WHERE Restaurant = ? " +
                              "AND InvDate = ? " +
                              "AND " + timeCondition + " " +
                              "AND TableType = ? " +
                              "AND NOT (PhoneNum = ? " +
                              "AND InvDate = ? " +
                              "AND InvTime = ?)";

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);
            OleDbCommand cmd = new OleDbCommand(sqlCount, con);
            cmd.Parameters.AddWithValue("?", res);
            cmd.Parameters.AddWithValue("?", DateTime.Parse(date));
            cmd.Parameters.AddWithValue("?", start);
            cmd.Parameters.AddWithValue("?", end);
            cmd.Parameters.AddWithValue("?", type);
            cmd.Parameters.AddWithValue("?", Session["Phone"].ToString());
            cmd.Parameters.AddWithValue("?", DateTime.Parse(Session["OldDate"].ToString()));
            cmd.Parameters.AddWithValue("?", Session["OldTime"].ToString());
            con.Open();
            int occupied = int.Parse(cmd.ExecuteScalar().ToString());
            con.Close();

            return occupied < total;
        }

        // לחיצה על שעה בטבלה - אם פנויה, מעדכן את ההזמנה
        protected void GridTimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cells[0]=כפתור, Cells[1]=שעה, Cells[2]=סטטוס
            string newTime = GridTimes.SelectedRow.Cells[1].Text;
            string status = GridTimes.SelectedRow.Cells[2].Text;

            if (status == "תפוס")
            {
                LblMessage.Text = "השעה תפוסה, נא לבחור שעה אחרת.";
                LblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            UpdateInDB(newTime);
        }

        // מעדכן את שורת ההזמנה במסד הנתונים
        private void UpdateInDB(string newTime)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection conn = new OleDbConnection(connStr);

            string sql = "UPDATE MyBooking SET " +
                         "InvDate = ?, InvTime = ?, NumGuest = ?, TableType = ? " +
                         "WHERE PhoneNum = ? AND InvDate = ? AND InvTime = ?";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.Parameters.AddWithValue("?", DateTime.Parse(TxtDate.Text));
            cmd.Parameters.AddWithValue("?", newTime);
            cmd.Parameters.AddWithValue("?", TxtNumGuests.Text);
            cmd.Parameters.AddWithValue("?", Session["SelectedTypeUpdate"].ToString());
            cmd.Parameters.AddWithValue("?", Session["Phone"].ToString());
            cmd.Parameters.AddWithValue("?", DateTime.Parse(Session["OldDate"].ToString()));
            cmd.Parameters.AddWithValue("?", Session["OldTime"].ToString());
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            // שומרים את התאריך והשעה החדשים - נצטרך אותם אם הלקוח יזמין הסעה
            // חדשה למועד המעודכן.
            Session["NewDate"] = TxtDate.Text;
            Session["NewTime"] = newTime;

            LblMessage.Text = "ההזמנה עודכנה בהצלחה לשעה " + newTime + "!";
            LblMessage.ForeColor = System.Drawing.Color.LightGreen;

            // במקום לחזור ישר לרשימה - עוברים לתשאול ההסעה.
            // צריך לשאול כי ההסעה הישנה (אם הוזמנה) עדיין רשומה אצל הספק
            // עם השעה הישנה, וצריך לבטל או לעדכן אותה.
            PnlEdit.Visible = false;
            PnlTaxi.Visible = true;

            // איפוס הפאנל למצב התחלתי (חשוב אם מעדכנים כמה הזמנות באותו ביקור)
            AddressPanel.Visible = false;
            BtnTaxiYes.Visible = true;
            BtnTaxiNo.Visible = true;
            LblTaxiResult.Text = "";
            LblTaxiQuestion.Visible = true;
            LblTaxiQuestion.Text = "ההזמנה עודכנה לשעה " + newTime + ". האם ברצונך הסעה למועד החדש?";
        }

        // ============================================================
        // מצב הסעה (PnlTaxi): תשאול הסעה אחרי עדכון ההזמנה
        // ============================================================

        // הלקוח לחץ "כן, עדכנו לי הסעה" - מבקשים ממנו את כתובת האיסוף
        protected void BtnTaxiYes_Click(object sender, EventArgs e)
        {
            BtnTaxiYes.Visible = false;
            BtnTaxiNo.Visible = false;
            LblTaxiQuestion.Text = "אנא הזן את כתובת האיסוף";
            AddressPanel.Visible = true;
        }

        // הלקוח לחץ "לא, בטלו את ההסעה" - מבטלים את ההסעה הישנה אצל הספק
        protected void BtnTaxiNo_Click(object sender, EventArgs e)
        {
            // מבטלים את ההסעה הישנה (לפי השעה המקורית). אם לא הייתה הסעה -
            // הספק פשוט יחזיר שלא נמצאה נסיעה לביטול, וזה בסדר.
            string taxiMsg = CancelOriginalRide();

            LblMessage.Text = "ההזמנה עודכנה. " + taxiMsg;
            LblMessage.ForeColor = System.Drawing.Color.LightGreen;

            BackToList();
        }

        // הלקוח אישר את כתובת האיסוף - בודקים את הכתובת, מבטלים את ההסעה
        // הישנה ומזמינים הסעה חדשה לשעה ולכתובת החדשות.
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

            // מספר הבית חייב להיות ספרות בלבד
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
            string sqlCity = "SELECT COUNT(*) FROM Cities WHERE CityName = ?";
            OleDbCommand cmdCity = new OleDbCommand(sqlCity, conCity);
            cmdCity.Parameters.AddWithValue("@CityName", city);
            conCity.Open();
            int cityCount = int.Parse(cmdCity.ExecuteScalar().ToString());
            conCity.Close();

            if (cityCount == 0)
            {
                LblAddressError.Text = "העיר \"" + city + "\" לא נמצאה במאגר היישובים.";
                return;
            }

            LblAddressError.Text = "";

            // קוראים את התאריך והשעה החדשים ששמרנו ב-Session
            string newDate = "";
            if (Session["NewDate"] != null)
            {
                newDate = Session["NewDate"].ToString();
            }
            string newTime = "";
            if (Session["NewTime"] != null)
            {
                newTime = Session["NewTime"].ToString();
            }
            string fullAddress = street + " " + houseNum + ", " + city;

            // שלב א': מבטלים את ההסעה הישנה (לפי התאריך והשעה המקוריים).
            // שלב ב': מזמינים הסעה חדשה לתאריך, לשעה ולכתובת החדשים.
            string taxiResult;
            try
            {
                TaxiServiceAPI.WebService1SoapClient taxi = new TaxiServiceAPI.WebService1SoapClient();
                taxi.CancelRide(Session["User"].ToString(), LblResName.Text,
                                Session["OldDate"].ToString(), Session["OldTime"].ToString());
                taxiResult = taxi.BookRide(Session["User"].ToString(), LblResName.Text,
                                           newDate, newTime, fullAddress);
            }
            catch
            {
                taxiResult = "שגיאה בחיבור לחברת ההסעות. נסה שוב מאוחר יותר.";
            }

            LblMessage.Text = "ההזמנה וההסעה עודכנו. " + taxiResult;
            LblMessage.ForeColor = System.Drawing.Color.LightGreen;

            BackToList();
        }

        // מבטל אצל ספק ההסעות את ההסעה המקורית, לפי שם הלקוח, המסעדה,
        // התאריך הישן (Session["OldDate"]) והשעה הישנה (Session["OldTime"]).
        // מחזיר את הודעת הספק.
        private string CancelOriginalRide()
        {
            try
            {
                TaxiServiceAPI.WebService1SoapClient taxi = new TaxiServiceAPI.WebService1SoapClient();
                return taxi.CancelRide(Session["User"].ToString(), LblResName.Text,
                                       Session["OldDate"].ToString(), Session["OldTime"].ToString());
            }
            catch
            {
                return "לא ניתן היה לעדכן את ספק ההסעות כעת.";
            }
        }

        // חוזר ממצב ההסעה אל רשימת ההזמנות המעודכנת
        private void BackToList()
        {
            PnlTaxi.Visible = false;
            PnlEdit.Visible = false;
            PnlList.Visible = true;
            LoadFutureBookings();
        }

        // לחיצה על "ביטול הזמנה" - מוחק את ההזמנה מהמסד
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "DELETE FROM MyBooking " +
                         "WHERE PhoneNum = ? AND InvDate = ? AND InvTime = ?";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            cmd.Parameters.AddWithValue("?", Session["Phone"].ToString());
            cmd.Parameters.AddWithValue("?", DateTime.Parse(Session["OldDate"].ToString()));
            cmd.Parameters.AddWithValue("?", Session["OldTime"].ToString());
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // אם הוזמנה הסעה להזמנה הזו - צריך לבטל אותה גם אצל הספק,
            // אחרת המונית תישאר רשומה לתאריך ולשעה של הזמנה שכבר לא קיימת.
            string taxiMsg = CancelOriginalRide();

            LblMessage.Text = "ההזמנה בוטלה ונמחקה מהמערכת. " + taxiMsg;
            LblMessage.ForeColor = System.Drawing.Color.Orange;

            // חזרה לרשימה (בלי ההזמנה שנמחקה)
            PnlEdit.Visible = false;
            PnlList.Visible = true;
            LoadFutureBookings();
        }

        // ============================================================
        // עזר
        // ============================================================

        // ממיר את שם סוג השולחן מאנגלית לתיאור עברי
        private string TranslateTableType(string type)
        {
            if (type == "Small")
                return "קטן (עד 2 סועדים)";
            else if (type == "Medium")
                return "בינוני (3-4 סועדים)";
            else if (type == "Large")
                return "גדול (5+ סועדים)";
            else
                return type;
        }
    }
}
