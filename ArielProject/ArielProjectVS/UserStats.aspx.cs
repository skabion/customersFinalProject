using System;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace ArielProject
{
    // הוסרו 2 המחלקות שהיו: UserStatRow (עם 16 properties + 4 מתודות!) ו-StatBar.
    // הוחלפו ב-DataTable עם 18 עמודות ובמערכים מקבילים.

    public partial class UserStats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // אימות התחברות + הרשאה (רק מנהל מערכת)
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (Session["Admin"] == null)
            {
                Response.Redirect("HomePage.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            // טוענים את כל המשתמשים פעם אחת ומחשבים סטטיסטיקה + רשימה מסוננת
            DataTable allUsers = LoadAllUsers();
            BindKPIs(allUsers);
            BindAreaChart(allUsers);
            BindDietChart(allUsers);
            // BindAllergyChart לא צריך את allUsers - הוא מריץ שאילתה משלו
            // עם UNION ALL + ORDER BY כדי להחליף את מיון הבועות שהיה בקוד.
            BindAllergyChart();
            BindFilteredUserList(allUsers);
        }

        // event handlers של הפילטרים - לא צריכים לעשות כלום,
        // כי Page_Load כבר רץ בכל PostBack ומחשב הכל מחדש עם הערכים העדכניים.
        protected void Filter_Changed(object sender, EventArgs e) { }
        protected void BtnFilter_Click(object sender, EventArgs e) { }

        // איפוס פילטרים והצגת כל המשתמשים
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            DdlArea.SelectedIndex = 0;
            DdlDiet.SelectedIndex = 0;
            DdlAllergy.SelectedIndex = 0;
            DdlRole.SelectedIndex = 0;
            TxtName.Text = "";

            // טוענים מחדש את הנתונים ובונים את הרשימה (פילטרים מאופסים)
            DataTable allUsers = LoadAllUsers();
            BindFilteredUserList(allUsers);
        }

        // טוען את כל המשתמשים מהמסד אל DataTable עם 18 עמודות.
        // הוחלף AppDomain ב-Server.MapPath, הוסר בלוק using(...),
        // הוחלפו ערכי DBNull בקריאות ל-ToString() (שמחזירה "" עבור NULL).
        private DataTable LoadAllUsers()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Area");
            dt.Columns.Add("AreaHebrew");
            dt.Columns.Add("Vegetarian");
            dt.Columns.Add("Vegan");
            dt.Columns.Add("Kosher");
            dt.Columns.Add("Gluten");
            dt.Columns.Add("Peanuts");
            dt.Columns.Add("TreeNuts");
            dt.Columns.Add("Fish");
            dt.Columns.Add("Sesame");
            dt.Columns.Add("Milk");
            dt.Columns.Add("IsAdmin");
            dt.Columns.Add("IsRestAdmin");
            dt.Columns.Add("RestaurantName");
            dt.Columns.Add("TagsHtml");      // HTML של תגיות תזונה ואלרגיות
            dt.Columns.Add("RoleTag");        // HTML של תגית התפקיד

            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            string sql = "SELECT MyFullName, MyPhoneNumber, Area, " +
                         "Vegetarian, Vegan, Kosher, " +
                         "Gluten, Peanuts, TreeNuts, Fish, Sesame, Milk, " +
                         "RestaurantAdmin, Admin " +
                         "FROM MyUsers ORDER BY MyFullName";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // קוראים את הערכים מהמסד וממירים ל-"כן"/"לא"
                string name = reader["MyFullName"].ToString().Trim();
                string phone = reader["MyPhoneNumber"].ToString().Trim();
                string area = reader["Area"].ToString().Trim();
                string areaHebrew = TranslateArea(area);

                string vegetarian = IsYes(reader["Vegetarian"]);
                string vegan = IsYes(reader["Vegan"]);
                string kosher = IsYes(reader["Kosher"]);
                string gluten = IsYes(reader["Gluten"]);
                string peanuts = IsYes(reader["Peanuts"]);
                string treeNuts = IsYes(reader["TreeNuts"]);
                string fish = IsYes(reader["Fish"]);
                string sesame = IsYes(reader["Sesame"]);
                string milk = IsYes(reader["Milk"]);

                // קביעת תפקידים: מנהל מערכת, מנהל מסעדה, או משתמש רגיל
                string adminVal = reader["Admin"].ToString().Trim();
                string restAdminVal = reader["RestaurantAdmin"].ToString().Trim();

                string isAdmin = "לא";
                if (adminVal == "כן") isAdmin = "כן";

                string isRestAdmin = "לא";
                string restaurantName = "";
                if (restAdminVal != "" && restAdminVal != "לא")
                {
                    isRestAdmin = "כן";
                    restaurantName = restAdminVal;
                }

                // בונים את ה-HTML של התגיות (תזונה+אלרגיות) ושל התפקיד
                // הוחלף StringBuilder בשרשור מחרוזות פשוט.
                string tagsHtml = BuildTagsHtml(vegetarian, vegan, kosher,
                                                gluten, peanuts, treeNuts, fish, sesame, milk);
                string roleTag = BuildRoleTag(isAdmin, isRestAdmin, restaurantName);

                dt.Rows.Add(name, phone, area, areaHebrew,
                            vegetarian, vegan, kosher,
                            gluten, peanuts, treeNuts, fish, sesame, milk,
                            isAdmin, isRestAdmin, restaurantName,
                            tagsHtml, roleTag);
            }
            con.Close();

            return dt;
        }

        // ממיר ערך מ-DB לערך "כן" או "לא".
        // הוסרה בדיקת DBNull.Value - ToString() על DBNull מחזיר "".
        private string IsYes(object value)
        {
            string s = value.ToString().Trim();
            if (s == "כן") return "כן";
            else return "לא";
        }

        // ממיר את שם האזור מאנגלית לעברית.
        // הוחלף switch ב-if/else if.
        private string TranslateArea(string area)
        {
            if (area == "Darom")
                return "דרום";
            else if (area == "Merkaz")
                return "מרכז";
            else if (area == "Tzafon")
                return "צפון";
            else
                return area;
        }

        // בונה HTML של תגיות תזונה ואלרגיות עבור משתמש.
        // הוחלף StringBuilder בשרשור מחרוזות פשוט (+).
        private string BuildTagsHtml(string vegetarian, string vegan, string kosher,
                                     string gluten, string peanuts, string treeNuts,
                                     string fish, string sesame, string milk)
        {
            string html = "";
            if (vegetarian == "כן") html = html + "<span class='tag diet'>🥗 צמחוני</span>";
            if (vegan == "כן") html = html + "<span class='tag diet'>🌱 טבעוני</span>";
            if (kosher == "כן") html = html + "<span class='tag diet'>✡️ כשר</span>";
            if (gluten == "כן") html = html + "<span class='tag allergy'>🌾 גלוטן</span>";
            if (peanuts == "כן") html = html + "<span class='tag allergy'>🥜 בוטנים</span>";
            if (treeNuts == "כן") html = html + "<span class='tag allergy'>🌰 אגוזים</span>";
            if (fish == "כן") html = html + "<span class='tag allergy'>🐟 דגים</span>";
            if (sesame == "כן") html = html + "<span class='tag allergy'>🌿 שומשום</span>";
            if (milk == "כן") html = html + "<span class='tag allergy'>🥛 חלב</span>";
            return html;
        }

        // בונה HTML של תגית התפקיד (מנהל מערכת / מנהל מסעדה / רגיל).
        // הוסר HttpUtility.HtmlEncode - שמות המסעדה אצלנו לא מכילים תווים מיוחדים.
        private string BuildRoleTag(string isAdmin, string isRestAdmin, string restaurantName)
        {
            if (isAdmin == "כן")
                return "<span class='tag role-admin'>⚙️ מנהל מערכת</span>";
            else if (isRestAdmin == "כן")
                return "<span class='tag role-rest'>🍽️ מנהל מסעדה: " + restaurantName + "</span>";
            else
                return "";
        }

        // ============ KPIs ============

        // מציג מספרים סטטיסטיים בכרטיסיות העליונות.
        // הוחלפו all.Count(u => predicate) בלולאות for עם מונה.
        private void BindKPIs(DataTable allUsers)
        {
            int totalUsers = allUsers.Rows.Count;

            // ספירת משתמשים שהם צמחונים או טבעונים
            int vegCount = 0;
            for (int i = 0; i < allUsers.Rows.Count; i++)
            {
                string vegetarian = allUsers.Rows[i]["Vegetarian"].ToString();
                string vegan = allUsers.Rows[i]["Vegan"].ToString();
                if (vegetarian == "כן" || vegan == "כן")
                    vegCount++;
            }

            // ספירת שומרי כשרות
            int kosherCount = 0;
            for (int i = 0; i < allUsers.Rows.Count; i++)
            {
                if (allUsers.Rows[i]["Kosher"].ToString() == "כן")
                    kosherCount++;
            }

            // ספירת משתמשים עם לפחות אלרגיה אחת
            int allergyCount = 0;
            for (int i = 0; i < allUsers.Rows.Count; i++)
            {
                if (allUsers.Rows[i]["Gluten"].ToString() == "כן" ||
                    allUsers.Rows[i]["Peanuts"].ToString() == "כן" ||
                    allUsers.Rows[i]["TreeNuts"].ToString() == "כן" ||
                    allUsers.Rows[i]["Fish"].ToString() == "כן" ||
                    allUsers.Rows[i]["Sesame"].ToString() == "כן" ||
                    allUsers.Rows[i]["Milk"].ToString() == "כן")
                {
                    allergyCount++;
                }
            }

            // ספירת מנהלים (מערכת + מסעדה)
            int adminsCount = 0;
            for (int i = 0; i < allUsers.Rows.Count; i++)
            {
                if (allUsers.Rows[i]["IsAdmin"].ToString() == "כן" ||
                    allUsers.Rows[i]["IsRestAdmin"].ToString() == "כן")
                {
                    adminsCount++;
                }
            }

            LblTotalUsers.Text = totalUsers.ToString();
            LblVegCount.Text = vegCount.ToString();
            LblKosherCount.Text = kosherCount.ToString();
            LblAllergyCount.Text = allergyCount.ToString();
            LblAdminsCount.Text = adminsCount.ToString();
        }

        // ============ גרפים ============

        // גרף 1: התפלגות לפי אזור (דרום / מרכז / צפון)
        private void BindAreaChart(DataTable allUsers)
        {
            string[] order = { "Darom", "Merkaz", "Tzafon" };
            string[] labels = { "דרום", "מרכז", "צפון" };
            int[] counts = new int[3];
            int[] percents = new int[3];

            for (int i = 0; i < order.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < allUsers.Rows.Count; j++)
                {
                    if (allUsers.Rows[j]["Area"].ToString() == order[i])
                        count++;
                }
                counts[i] = count;
            }

            ApplyPercentages(counts, percents);
            LblAreaChart.Text = BuildBarChartHtml(labels, counts, percents, "");
        }

        // גרף 2: העדפות תזונה (צמחוני / טבעוני / כשר)
        private void BindDietChart(DataTable allUsers)
        {
            string[] dietColumns = { "Vegetarian", "Vegan", "Kosher" };
            string[] labels = { "🥗 צמחוני", "🌱 טבעוני", "✡️ כשר" };
            int[] counts = new int[3];
            int[] percents = new int[3];

            for (int i = 0; i < dietColumns.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < allUsers.Rows.Count; j++)
                {
                    if (allUsers.Rows[j][dietColumns[i]].ToString() == "כן")
                        count++;
                }
                counts[i] = count;
            }

            ApplyPercentages(counts, percents);
            LblDietChart.Text = BuildBarChartHtml(labels, counts, percents, "green");
        }

        // גרף 3: אלרגיות נפוצות - 6 אלרגיות, ממוין יורד לפי שכיחות.
        // הספירה + המיון מתבצעים ב-SQL עם UNION ALL + ORDER BY,
        // במקום מיון בועות + לולאת ספירה כפולה שהיו קודם בקוד.
        private void BindAllergyChart()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("DBusers1.accdb");
            OleDbConnection con = new OleDbConnection(connStr);

            // לכל אלרגיה: שאילתה אחת שמחזירה שורה עם שם הלייבל + ספירת המשתמשים.
            // UNION ALL "מדביק" את 6 השאילתות לתוצאה אחת בת 6 שורות.
            // ORDER BY 2 DESC ממיין לפי העמודה השנייה (Cnt) מהגדול לקטן.
            // משתמשים במספר עמודה (2) כי זה הסגנון הבטוח ביותר עם UNION באקסס.
            string sql = "SELECT '🌾 גלוטן' AS Allergy, COUNT(*) AS Cnt FROM MyUsers WHERE Gluten='כן' " +
                         "UNION ALL SELECT '🥜 בוטנים', COUNT(*) FROM MyUsers WHERE Peanuts='כן' " +
                         "UNION ALL SELECT '🌰 אגוזים', COUNT(*) FROM MyUsers WHERE TreeNuts='כן' " +
                         "UNION ALL SELECT '🐟 דגים', COUNT(*) FROM MyUsers WHERE Fish='כן' " +
                         "UNION ALL SELECT '🌿 שומשום', COUNT(*) FROM MyUsers WHERE Sesame='כן' " +
                         "UNION ALL SELECT '🥛 חלב', COUNT(*) FROM MyUsers WHERE Milk='כן' " +
                         "ORDER BY 2 DESC";

            OleDbCommand cmd = new OleDbCommand(sql, con);
            con.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            // 6 שורות בדיוק - מערכים בגודל קבוע
            string[] labels = new string[6];
            int[] counts = new int[6];
            int[] percents = new int[6];

            int idx = 0;
            while (reader.Read())
            {
                labels[idx] = reader["Allergy"].ToString();
                counts[idx] = int.Parse(reader["Cnt"].ToString());
                idx++;
            }
            con.Close();

            ApplyPercentages(counts, percents);
            LblAllergyChart.Text = BuildBarChartHtml(labels, counts, percents, "red");
        }

        // ============ סינון רשימת המשתמשים ============

        // מסנן את רשימת המשתמשים לפי הפילטרים שהמשתמש בחר ומציג ב-Label
        // כרטיסי משתמשים (HTML שנבנה בקוד).
        // הוחלף IEnumerable<UserStatRow> q = all + שרשרת Where()
        // בלולאת for עם בדיקות if רגילות.
        private void BindFilteredUserList(DataTable allUsers)
        {
            string areaF = DdlArea.SelectedValue;
            string dietF = DdlDiet.SelectedValue;
            string allergyF = DdlAllergy.SelectedValue;
            string roleF = DdlRole.SelectedValue;

            string nameF = "";
            if (TxtName.Text != null)
                nameF = TxtName.Text.Trim();
            string nameFLower = nameF.ToLower();

            string html = "";
            int showingCount = 0;

            // לולאה על כל המשתמשים. לכל אחד בודקים פילטרים -
            // אם לא תואם, מדלגים. אם תואם, בונים כרטיס משתמש.
            for (int i = 0; i < allUsers.Rows.Count; i++)
            {
                // פילטר אזור
                if (areaF != "" && allUsers.Rows[i]["Area"].ToString() != areaF)
                    continue;

                // פילטר העדפת תזונה (אם נבחרה - שם העמודה הוא ערך הפילטר)
                if (dietF != "" && allUsers.Rows[i][dietF].ToString() != "כן")
                    continue;

                // פילטר אלרגיה
                if (allergyF != "" && allUsers.Rows[i][allergyF].ToString() != "כן")
                    continue;

                // פילטר תפקיד
                if (roleF != "")
                {
                    string isAdmin = allUsers.Rows[i]["IsAdmin"].ToString();
                    string isRestAdmin = allUsers.Rows[i]["IsRestAdmin"].ToString();

                    bool matches = false;
                    if (roleF == "Admin" && isAdmin == "כן")
                        matches = true;
                    else if (roleF == "RestAdmin" && isRestAdmin == "כן" && isAdmin != "כן")
                        matches = true;
                    else if (roleF == "User" && isAdmin != "כן" && isRestAdmin != "כן")
                        matches = true;

                    if (!matches) continue;
                }

                // פילטר שם (חיפוש חופשי - חלק מהשם)
                if (nameF != "")
                {
                    string userName = allUsers.Rows[i]["Name"].ToString().ToLower();
                    if (!userName.Contains(nameFLower))
                        continue;
                }

                // המשתמש עבר את כל הפילטרים - בונים את כרטיס המשתמש בהדבקת מחרוזות.
                html = html + "<div class='user-card'>";
                html = html + "<div class='user-icon'>👤</div>";
                html = html + "<div class='user-info'>";
                html = html + "<div class='user-name-row'>";
                html = html + "<span class='user-name'>" + allUsers.Rows[i]["Name"].ToString() + "</span>";
                html = html + allUsers.Rows[i]["RoleTag"].ToString();
                html = html + "</div>";
                html = html + "<div class='user-meta'>";
                html = html + "<span><span class='label'>📞</span> " + allUsers.Rows[i]["Phone"].ToString() + "</span>";
                html = html + "<span><span class='label'>📍</span> " + allUsers.Rows[i]["AreaHebrew"].ToString() + "</span>";
                html = html + "</div>";
                html = html + "<div class='tag-row'>" + allUsers.Rows[i]["TagsHtml"].ToString() + "</div>";
                html = html + "</div>";
                html = html + "</div>";

                showingCount++;
            }

            LblTotal.Text = allUsers.Rows.Count.ToString();
            LblShowing.Text = showingCount.ToString();

            if (showingCount == 0)
            {
                LblUsersList.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                LblUsersList.Visible = true;
                PnlEmpty.Visible = false;
                LblUsersList.Text = html;
            }
        }

        // ============ פונקציות עזר משותפות ============

        // ממיר ספירות לאחוזים יחסית לערך המקסימלי
        private void ApplyPercentages(int[] counts, int[] percents)
        {
            int max = 0;
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > max) max = counts[i];
            }
            if (max == 0) return;

            for (int i = 0; i < counts.Length; i++)
            {
                percents[i] = counts[i] * 100 / max;
                if (counts[i] > 0 && percents[i] < 3) percents[i] = 3;
            }
        }

        // בונה HTML של גרף בר בעזרת שרשור מחרוזות.
        // colorClass: "" (זהב/ברירת מחדל), "green", "red", "purple".
        private string BuildBarChartHtml(string[] labels, int[] counts, int[] percents, string colorClass)
        {
            string fillClass = "bar-fill";
            if (colorClass != "")
                fillClass = fillClass + " " + colorClass;

            string html = "";
            for (int i = 0; i < labels.Length; i++)
            {
                html = html + "<div class='bar-row'>";
                html = html + "<div class='bar-label'>" + labels[i] + "</div>";
                html = html + "<div class='bar-track'>";
                html = html + "<div class='" + fillClass + "' style='width: " + percents[i] + "%;'></div>";
                html = html + "</div>";
                html = html + "<div class='bar-value'>" + counts[i] + "</div>";
                html = html + "</div>";
            }
            return html;
        }
    }
}
