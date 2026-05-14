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
        public string BookRide(string customerName, string restaurantName, string pickupTime)
        {
            try
            {
                Random rnd = new Random();
                int driverNumber = rnd.Next(1, 100);

                string path = Server.MapPath("~/TaxiDB1.accdb");
                string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path;

                using (OleDbConnection con = new OleDbConnection(connStr))
                {
                    // תיקון: הוספנו [] סביב השמות כדי למנוע שגיאות תחביר של אקסס
                    // ושימוש ב-Replace למקרה שיש גרש בשמות
                    string sql = string.Format(
                        "INSERT INTO [Taxis] ([CustomerName], [RestaurantName], [RideTime], [DriverNum]) " +
                        "VALUES ('{0}', '{1}', '{2}', {3})",
                        customerName.Replace("'", "''"),
                        restaurantName.Replace("'", "''"),
                        pickupTime,
                        driverNumber);

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();

                    // שליפת המזהה שנוצר
                    cmd.CommandText = "SELECT @@IDENTITY";
                    int newRideId = Convert.ToInt32(cmd.ExecuteScalar());

                    con.Close();

                    return "הנסיעה אושרה! מספר הזמנה: " + newRideId + " | נהג מספר " + driverNumber + " בדרך אליך.";
                }
            }
            catch (Exception ex)
            {
                return "שגיאה בספק המוניות: " + ex.Message;
            }
        }
    }
}