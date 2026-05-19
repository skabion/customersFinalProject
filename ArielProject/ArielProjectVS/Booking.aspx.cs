using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ArielProject
{
    // מחלקה קטנה שעוזרת לנו לשמור נתונים - נמצאת מחוץ למחלקת הדף
    public class TimeSlot
    {
        public string TimeStr { get; set; }
        public bool IsAvailable { get; set; }
    }

    // =========================================================
    // תחילת המחלקה של הדף - כל הפעולות חייבות להיות בתוך הסוגריים שלה!
    // =========================================================
    public partial class Booking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["res"] != null)
                {
                    LblResName.Text = Request.QueryString["res"].ToString();
                }
            }

            if (!IsPostBack)
            {
                // מגדיר את הערך להשוואה כתאריך של היום בפורמט שמתאים לשדה HTML5
                CompareValidatorDate.ValueToCompare = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        // ברגע שלוחצים "מצאו לי שולחן"
        protected void BtnCheckTimes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtDate.Text) || string.IsNullOrEmpty(TxtGuests.Text))
            {
                LblMsg.Text = "נא להזין תאריך ומספר סועדים";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            GenerateTimeSlots();
        }

        // כאן נמצאות השורות שעשו לך שגיאה - עכשיו הן בתוך המחלקה ולכן יזוהו
        private void GenerateTimeSlots()
        {
            string res = LblResName.Text;
            string date = TxtDate.Text;
            int guests = int.Parse(TxtGuests.Text);

            string tableType = "SmallTables";
            string typeName = "Small";
            if (guests > 2 && guests <= 4) { tableType = "MediumTables"; typeName = "Medium"; }
            else if (guests > 4) { tableType = "LargeTables"; typeName = "Large"; }

            Session["SelectedType"] = typeName;

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            OleDbConnection con = new OleDbConnection(connStr);

            string sqlCap = "SELECT " + tableType + " FROM MyRestaurants WHERE Restaurants = '" + res + "'";
            OleDbCommand cmdCap = new OleDbCommand(sqlCap, con);
            con.Open();
            object result = cmdCap.ExecuteScalar();
            int totalTables = result != null ? Convert.ToInt32(result) : 0;
            con.Close();

            List<TimeSlot> slots = new List<TimeSlot>();

            // קביעת שעות הפעילות לפי המסעדה והיום בשבוע
            DateTime selectedDate = DateTime.Parse(date);
            TimeSpan openSpan, closeSpan;
            GetOpeningHours(res, selectedDate.DayOfWeek, out openSpan, out closeSpan);

            // אפשר להזמין רק עד שעתיים לפני סגירת המסעדה
            DateTime startTime = DateTime.Today.Add(openSpan);
            DateTime endTime = DateTime.Today.Add(closeSpan).AddHours(-2);

            if (startTime > endTime)
            {
                LblMsg.Text = "המסעדה אינה מקבלת הזמנות בתאריך זה";
                LblMsg.ForeColor = System.Drawing.Color.Red;
                RepeaterTimes.DataSource = null;
                RepeaterTimes.DataBind();
                RepeaterTimes.Visible = false;
                return;
            }

            while (startTime <= endTime)
            {
                string timeToCheck = startTime.ToString("HH:mm");
                bool isAvail = CheckSpecificTime(timeToCheck, res, date, totalTables, typeName, con);

                slots.Add(new TimeSlot { TimeStr = timeToCheck, IsAvailable = isAvail });

                startTime = startTime.AddMinutes(30);
            }

            LblMsg.Text = "";
            RepeaterTimes.DataSource = slots;
            RepeaterTimes.DataBind();
            RepeaterTimes.Visible = true;
        }

        // מחזירה את שעות הפעילות של המסעדה ביום מסוים בשבוע.
        // שעות שעוברות חצות מיוצגות עם ערך גדול מ-24 (למשל 25:00 = 01:00 למחרת).
        // TimeSpan.Zero לפתיחה ולסגירה => המסעדה סגורה ביום זה.
        private void GetOpeningHours(string res, DayOfWeek day, out TimeSpan open, out TimeSpan close)
        {
            // ברירת מחדל - שעות גנריות לכל מסעדה שעוד לא הוגדרה
            open = new TimeSpan(18, 0, 0);
            close = new TimeSpan(23, 30, 0);

            bool isFri = day == DayOfWeek.Friday;
            bool isSat = day == DayOfWeek.Saturday;

            switch (res)
            {
                case "Bobo":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-15:00 | שבת 20:00-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "La Lush":
                    // ראשון-חמישי 09:00-00:00 | שישי 08:00-16:00 | שבת 19:00-00:00
                    if (isFri) { open = new TimeSpan(8, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(24, 0, 0); }
                    else { open = new TimeSpan(9, 0, 0); close = new TimeSpan(24, 0, 0); }
                    break;

                case "Moses":
                    // ראשון-רביעי 12:00-00:00 | חמישי-שבת 12:00-01:00
                    if (day == DayOfWeek.Thursday || isFri || isSat)
                    { open = new TimeSpan(12, 0, 0); close = new TimeSpan(25, 0, 0); }
                    else
                    { open = new TimeSpan(12, 0, 0); close = new TimeSpan(24, 0, 0); }
                    break;

                case "Japanika":
                    // ראשון-חמישי 12:00-23:00 | שישי 12:00-15:00 | שבת 19:30-23:00
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 30, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Kagas":
                    // ראשון-חמישי 10:00-23:00 | שישי 09:00-16:00 | מוצ"ש 20:00-23:30
                    if (isFri) { open = new TimeSpan(9, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(10, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Vivino":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-16:00 | מוצ"ש 19:00-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Mahne Yuda":
                    // ראשון-חמישי 18:30-01:30 | שישי 12:00-15:30 | שבת סגור
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 30, 0); }
                    else if (isSat) { open = TimeSpan.Zero; close = TimeSpan.Zero; }
                    else { open = new TimeSpan(18, 30, 0); close = new TimeSpan(25, 30, 0); }
                    break;

                case "Oshi Oshi":
                    // ראשון-חמישי 12:00-23:00 | שישי 11:30-15:00 | מוצ"ש 20:00-23:00
                    if (isFri) { open = new TimeSpan(11, 30, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Biga":
                    // ראשון-חמישי 12:00-23:00 | שישי 12:00-15:00 | שבת 19:00-23:00
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Nafis":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-16:00 | מוצ"ש 19:30-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 30, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Zink":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-15:30 | מוצ"ש 19:00-00:00
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 30, 0); }
                    else if (isSat) { open = new TimeSpan(19, 0, 0); close = new TimeSpan(24, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Max Brener":
                    // ראשון-חמישי 09:00-23:00 | שישי 09:00-16:00 | מוצ"ש 20:00-23:00
                    if (isFri) { open = new TimeSpan(9, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(9, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Segev":
                    // ראשון-חמישי 12:00-23:00 | שישי 12:00-15:00 | מוצ"ש 19:30-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(15, 0, 0); }
                    else if (isSat) { open = new TimeSpan(19, 30, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;

                case "Black":
                    // ראשון-חמישי 12:00-23:30 | שישי 12:00-16:00 | מוצ"ש 20:00-23:30
                    if (isFri) { open = new TimeSpan(12, 0, 0); close = new TimeSpan(16, 0, 0); }
                    else if (isSat) { open = new TimeSpan(20, 0, 0); close = new TimeSpan(23, 30, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 30, 0); }
                    break;

                case "Kansai":
                    // ראשון-חמישי 12:00-23:00 | שישי 11:30-14:30 | מוצ"ש 20:30-23:00
                    if (isFri) { open = new TimeSpan(11, 30, 0); close = new TimeSpan(14, 30, 0); }
                    else if (isSat) { open = new TimeSpan(20, 30, 0); close = new TimeSpan(23, 0, 0); }
                    else { open = new TimeSpan(12, 0, 0); close = new TimeSpan(23, 0, 0); }
                    break;
            }
        }

        private bool CheckSpecificTime(string timeToCheck, string res, string date, int total, string type, OleDbConnection con)
        {
            DateTime dt = DateTime.Parse(timeToCheck);
            string start = dt.AddHours(-2).ToString("HH:mm");
            string end = dt.AddHours(2).ToString("HH:mm");

            string timeCondition;
            if (string.Compare(start, end) > 0)
            {
                timeCondition = string.Format("(InvTime > '{0}' OR InvTime < '{1}')", start, end);
            }
            else
            {
                timeCondition = string.Format("(InvTime > '{0}' AND InvTime < '{1}')", start, end);
            }

            string sqlCount = string.Format(
                "SELECT COUNT(*) FROM MyBooking WHERE Restaurant='{0}' AND InvDate=#{1}# " +
                "AND {2} AND TableType='{3}'",
                res, date, timeCondition, type);

            OleDbCommand cmd = new OleDbCommand(sqlCount, con);
            con.Open();
            int occupied = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return occupied < total;
        }

        protected void RepeaterTimes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string finalTime = e.CommandArgument.ToString();
            SaveToDB(finalTime);
        }

        private void SaveToDB(string finalTime)
        {
            // 1. שמירת ההזמנה במסד הנתונים המקומי
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("") + "\\DBusers1.accdb";
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = string.Format("INSERT INTO MyBooking (Guest, PhoneNum, InvDate, NumGuest, InvTime, Restaurant, TableType) " +
             "VALUES ('{0}', '{1}', #{2}#, '{3}', '{4}', '{5}', '{6}')",
             Session["User"], Session["Phone"], TxtDate.Text,
             TxtGuests.Text, finalTime, LblResName.Text, Session["SelectedType"]);

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // 2. הצגת הודעת הצלחה והסתרת רשימת השעות
            LblMsg.Text = "ההזמנה לשעה " + finalTime + " בוצעה בהצלחה!";
            LblMsg.ForeColor = System.Drawing.Color.Green;
            RepeaterTimes.Visible = false;

            // 3. הצגת הצעת ההסעה - רק עכשיו שואלים את הלקוח אם הוא רוצה הסעה
            ViewState["BookedTime"] = finalTime;
            LblTaxiQuestion.Text = "מעוניין בהסעה בשעה " + finalTime + "?";
            TaxiPanel.Visible = true;

            // עצירת טיימר ההזמנה
            Page.ClientScript.RegisterStartupScript(this.GetType(), "clearTimer", "clearBookingTimer();", true);
        }

        // הלקוח לחץ "כן, הזמינו לי הסעה" - לא מזמינים עדיין, שואלים קודם כתובת
        protected void BtnTaxiYes_Click(object sender, EventArgs e)
        {
            BtnTaxiYes.Visible = false;
            BtnTaxiNo.Visible = false;
            LblTaxiQuestion.Text = "אנא הזן את כתובת האיסוף";
            AddressPanel.Visible = true;
        }

        // הלקוח לחץ "לא תודה" - לא פונים לספק
        protected void BtnTaxiNo_Click(object sender, EventArgs e)
        {
            LblTaxiResult.Text = "תודה! נשמח לראותך במסעדה.";
            LblTaxiResult.ForeColor = System.Drawing.Color.DarkGreen;
            BtnTaxiYes.Visible = false;
            BtnTaxiNo.Visible = false;
            LblTaxiQuestion.Visible = false;
        }

        // הלקוח אישר את הכתובת - ולידציה ואז קריאה לספק
        protected void BtnConfirmAddress_Click(object sender, EventArgs e)
        {
            string city = (TxtCity.Text ?? "").Trim();
            string street = (TxtStreet.Text ?? "").Trim();
            string house = (TxtHouseNum.Text ?? "").Trim();

            // בדיקה בסיסית - כל השדות חייבים להיות מלאים
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(street) || string.IsNullOrEmpty(house))
            {
                LblAddressError.Text = "יש למלא עיר, רחוב ומספר בית.";
                return;
            }

            int houseNum;
            if (!int.TryParse(house, out houseNum) || houseNum <= 0)
            {
                LblAddressError.Text = "מספר בית חייב להיות מספר חיובי.";
                return;
            }

            // ולידציה אמיתית מול נתוני משרד הפנים (data.gov.il)
            string validationError;
            if (!ValidateCityAndStreet(city, street, out validationError))
            {
                LblAddressError.Text = validationError;
                return;
            }

            // הכתובת תקינה - מזמינים את ההסעה
            LblAddressError.Text = "";
            string finalTime = ViewState["BookedTime"] != null ? ViewState["BookedTime"].ToString() : "";
            string fullAddress = street + " " + houseNum + ", " + city;

            try
            {
                TaxiServiceAPI.WebService1SoapClient taxi = new TaxiServiceAPI.WebService1SoapClient();
                string taxiResponse = taxi.BookRide(Session["User"].ToString(), LblResName.Text, finalTime, fullAddress);

                LblTaxiResult.Text = "<b>הודעה מחברת ההסעות:</b><br/>" + taxiResponse;
                LblTaxiResult.ForeColor = System.Drawing.Color.DarkGreen;
            }
            catch (Exception)
            {
                LblTaxiResult.Text = "שגיאה בחיבור לחברת ההסעות. נסה שוב מאוחר יותר.";
                LblTaxiResult.ForeColor = System.Drawing.Color.Red;
            }

            // אחרי הזמנה מוצלחת - מסתירים את כל פאנל הכתובת והשאלה
            AddressPanel.Visible = false;
            LblTaxiQuestion.Visible = false;
        }

        // ולידציה אמיתית של עיר ורחוב מול data.gov.il
        // resource_id-ים מצוטטים מהקטלוג הפתוח של משרד הפנים
        private bool ValidateCityAndStreet(string city, string street, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol =
                    System.Net.SecurityProtocolType.Tls12;

                if (!CityExists(city))
                {
                    errorMessage = "העיר \"" + city + "\" לא נמצאה במאגר היישובים בישראל.";
                    return false;
                }

                if (!StreetExistsInCity(city, street))
                {
                    errorMessage = "הרחוב \"" + street + "\" לא קיים בעיר \"" + city + "\".";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "לא ניתן לאמת את הכתובת כרגע (שירות החיפוש לא זמין). נסה שוב מאוחר יותר. (" + ex.Message + ")";
                return false;
            }
        }

        private bool CityExists(string city)
        {
            // קטלוג היישובים (משרד הפנים)
            string resourceId = "5c78e9fa-c2e2-4771-93ff-7f400a12f7ba";
            System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>> records =
                QueryGovApi(resourceId, city, 50);

            foreach (var rec in records)
            {
                if (rec.ContainsKey("שם_ישוב"))
                {
                    string val = (rec["שם_ישוב"] ?? "").ToString().Trim();
                    if (NamesMatch(val, city))
                        return true;
                }
            }
            return false;
        }

        private bool StreetExistsInCity(string city, string street)
        {
            // קטלוג הרחובות בישראל (משרד הפנים)
            string resourceId = "9ad3862c-8391-4b2f-84a4-2d4c68625f4b";
            System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>> records =
                QueryGovApi(resourceId, street, 300);

            foreach (var rec in records)
            {
                string cityVal = rec.ContainsKey("שם_ישוב") ? (rec["שם_ישוב"] ?? "").ToString().Trim() : "";
                string streetVal = rec.ContainsKey("שם_רחוב") ? (rec["שם_רחוב"] ?? "").ToString().Trim() : "";

                if (NamesMatch(cityVal, city) && NamesMatch(streetVal, street))
                    return true;
            }
            return false;
        }

        // השוואת שמות גמישה - מתעלמת מסדר רווחים ומקאף
        private bool NamesMatch(string fromApi, string fromUser)
        {
            if (string.IsNullOrEmpty(fromApi) || string.IsNullOrEmpty(fromUser)) return false;
            string a = fromApi.Replace("-", " ").Replace("\"", "").Trim();
            string b = fromUser.Replace("-", " ").Replace("\"", "").Trim();
            while (a.Contains("  ")) a = a.Replace("  ", " ");
            while (b.Contains("  ")) b = b.Replace("  ", " ");
            if (a.Equals(b, StringComparison.OrdinalIgnoreCase)) return true;
            // מאפשר התאמה חלקית - "תל אביב" מתאים ל"תל אביב יפו"
            if (a.Contains(b) || b.Contains(a)) return true;
            return false;
        }

        // קריאה בסיסית ל-data.gov.il - מחזיר את רשימת הרשומות
        private System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>
            QueryGovApi(string resourceId, string query, int limit)
        {
            string url = "https://data.gov.il/api/3/action/datastore_search?resource_id="
                + resourceId
                + "&q=" + System.Net.WebUtility.UrlEncode(query)
                + "&limit=" + limit;

            var list = new System.Collections.Generic.List<
                System.Collections.Generic.Dictionary<string, object>>();

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers.Add("User-Agent", "ArielRestaurantProject/1.0");
                string json = client.DownloadString(url);

                System.Web.Script.Serialization.JavaScriptSerializer ser =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                ser.MaxJsonLength = int.MaxValue;

                // DeserializeObject מחזיר Dictionary לאובייקטים ו-object[] למערכים
                var data = ser.DeserializeObject(json) as System.Collections.Generic.Dictionary<string, object>;
                if (data == null || !data.ContainsKey("result")) return list;

                var result = data["result"] as System.Collections.Generic.Dictionary<string, object>;
                if (result == null || !result.ContainsKey("records")) return list;

                var records = result["records"] as object[];
                if (records == null) return list;

                foreach (var rec in records)
                {
                    var dict = rec as System.Collections.Generic.Dictionary<string, object>;
                    if (dict != null) list.Add(dict);
                }
                return list;
            }
        }
    }
    // =========================================================
    // סוף המחלקה של הדף
    // =========================================================
}