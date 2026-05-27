using System;
using System.Web.Services;
using System.Data.OleDb;
using System.Web;

namespace Supplier
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string BookRide(string customerName, string restaurantName, string rideDate, string pickupTime, string address)
        {
            try
            {
                Random rnd = new Random();
                int driverNumber = rnd.Next(1, 100);

                string path = Server.MapPath("~/TaxiDB1.accdb");
                string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path;

                using (OleDbConnection con = new OleDbConnection(connStr))
                {
                    // שומרים גם את התאריך (RideDate) כדי שאפשר יהיה לזהות במדויק
                    // איזו נסיעה לבטל - לפי תאריך + שעה ולא רק לפי שעה.
                    // [] סביב שמות + Replace למניעת שגיאות תחביר וגרשים
                    string sql = string.Format(
                        "INSERT INTO [Taxis] ([CustomerName], [RestaurantName], [RideDate], [RideTime], [DriverNum], [Adress]) " +
                        "VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}')",
                        customerName.Replace("'", "''"),
                        restaurantName.Replace("'", "''"),
                        rideDate,
                        pickupTime,
                        driverNumber,
                        (address ?? "").Replace("'", "''"));

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT @@IDENTITY";
                    int newRideId = Convert.ToInt32(cmd.ExecuteScalar());

                    con.Close();

                    return "הנסיעה אושרה! מספר הזמנה: " + newRideId
                        + " | נהג מספר " + driverNumber + " בדרך אל "
                        + (string.IsNullOrEmpty(address) ? "כתובתך" : address) + ".";
                }
            }
            catch (Exception ex)
            {
                return "שגיאה בספק המוניות: " + ex.Message;
            }
        }

        // ביטול נסיעה קיימת.
        // מקבל את שם הלקוח, שם המסעדה, התאריך והשעה של הנסיעה שצריך לבטל,
        // ומוחק את השורה/ות המתאימות מטבלת Taxis.
        // התאריך + השעה יחד מזהים נסיעה אחת במדויק (כך לא מבלבלים בין
        // שתי הזמנות לאותה מסעדה ושעה בתאריכים שונים).
        // משמש כשלקוח מעדכן או מבטל הזמנה במערכת המסעדות.
        [WebMethod]
        public string CancelRide(string customerName, string restaurantName, string rideDate, string rideTime)
        {
            try
            {
                string path = Server.MapPath("~/TaxiDB1.accdb");
                string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path;

                using (OleDbConnection con = new OleDbConnection(connStr))
                {
                    // שאילתה עם פרמטרים (?) - הקלט עובר בנפרד ולא מצורף למחרוזת,
                    // כך נמנעת בעיית SQL Injection.
                    string sql = "DELETE FROM [Taxis] " +
                                 "WHERE [CustomerName] = ? AND [RestaurantName] = ? AND [RideDate] = ? AND [RideTime] = ?";

                    OleDbCommand cmd = new OleDbCommand(sql, con);

                    // ב-OleDb הפרמטרים מתאימים לפי הסדר של ה-? בשאילתה
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);
                    cmd.Parameters.AddWithValue("@RestaurantName", restaurantName);
                    cmd.Parameters.AddWithValue("@RideDate", rideDate);
                    cmd.Parameters.AddWithValue("@RideTime", rideTime);

                    con.Open();
                    int rowsDeleted = cmd.ExecuteNonQuery();
                    con.Close();

                    // מחזירים הודעה לפי כמה נסיעות נמחקו בפועל
                    if (rowsDeleted > 0)
                        return "ההסעה הקודמת בוטלה בהצלחה.";
                    else
                        return "לא נמצאה הסעה קודמת לביטול.";
                }
            }
            catch (Exception ex)
            {
                return "שגיאה בביטול ההסעה: " + ex.Message;
            }
        }
    }
}
