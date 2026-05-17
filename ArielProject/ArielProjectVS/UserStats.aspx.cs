using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace ArielProject
{
    // שורת משתמש שנטענת מ-DB
    public class UserStatRow
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }              // ערך גולמי: Darom / Merkaz / Tzafon
        public string AreaHebrew { get; set; }
        public bool Vegetarian { get; set; }
        public bool Vegan { get; set; }
        public bool Kosher { get; set; }
        public bool Gluten { get; set; }
        public bool Peanuts { get; set; }
        public bool TreeNuts { get; set; }
        public bool Fish { get; set; }
        public bool Sesame { get; set; }
        public bool Milk { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsRestAdmin { get; set; }
        public string RestaurantName { get; set; }    // אם הוא מנהל מסעדה - שם המסעדה

        // HTML מוכן להזרקה ב-Repeater
        public string TagsHtml { get; set; }
        public string RoleTag { get; set; }

        // עוזרי סינון
        public bool HasDiet(string key)
        {
            switch (key)
            {
                case "Vegetarian": return Vegetarian;
                case "Vegan": return Vegan;
                case "Kosher": return Kosher;
                default: return false;
            }
        }

        public bool HasAllergy(string key)
        {
            switch (key)
            {
                case "Gluten": return Gluten;
                case "Peanuts": return Peanuts;
                case "TreeNuts": return TreeNuts;
                case "Fish": return Fish;
                case "Sesame": return Sesame;
                case "Milk": return Milk;
                default: return false;
            }
        }

        public bool MatchesRole(string key)
        {
            switch (key)
            {
                case "Admin": return IsAdmin;
                case "RestAdmin": return IsRestAdmin && !IsAdmin;
                case "User": return !IsAdmin && !IsRestAdmin;
                default: return true;
            }
        }

        public bool HasAnyAllergy
        {
            get { return Gluten || Peanuts || TreeNuts || Fish || Sesame || Milk; }
        }
    }

    public class StatBar
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public int Percent { get; set; }
    }

    public partial class UserStats : System.Web.UI.Page
    {
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\DBusers1.accdb";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            // רק מנהל מערכת רואה את הדף הזה
            if (Session["Admin"] == null)
            {
                Response.Redirect("HomePage.aspx");
                return;
            }

            LblUserName.Text = Session["User"].ToString();

            // טוענים את כל המשתמשים, מחשבים סטטיסטיקה כללית, ומציגים רשימה מסוננת
            List<UserStatRow> all = LoadAllUsers();
            BindKPIs(all);
            BindAreaChart(all);
            BindDietChart(all);
            BindAllergyChart(all);
            BindFilteredList(all);
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            // ה-Page_Load כבר טיפל בכל - אין צורך לעשות שום דבר נוסף
            // (נשמר כאן כדי שה-AutoPostBack של הדרופדאונס יקרא פונקציה תקינה)
        }

        protected void BtnFilter_Click(object sender, EventArgs e)
        {
            // Page_Load מטפל בכל - הכפתור רק מפעיל PostBack
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            // איפוס כל הפילטרים
            DdlArea.SelectedIndex = 0;
            DdlDiet.SelectedIndex = 0;
            DdlAllergy.SelectedIndex = 0;
            DdlRole.SelectedIndex = 0;
            TxtName.Text = "";

            // טוענים מחדש את הנתונים בלי סינון
            List<UserStatRow> all = LoadAllUsers();
            BindFilteredList(all);
        }

        // ===== טעינה מה-DB =====

        private List<UserStatRow> LoadAllUsers()
        {
            var list = new List<UserStatRow>();

            using (OleDbConnection con = new OleDbConnection(connStr))
            {
                string sql = "SELECT MyFullName, MyPhoneNumber, Area, " +
                             "Vegetarian, Vegan, Kosher, " +
                             "Gluten, Peanuts, TreeNuts, Fish, Sesame, Milk, " +
                             "RestaurantAdmin, Admin " +
                             "FROM MyUsers ORDER BY MyFullName";
                OleDbCommand cmd = new OleDbCommand(sql, con);

                con.Open();
                OleDbDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    string restAdminVal = r["RestaurantAdmin"] == DBNull.Value ? "" : r["RestaurantAdmin"].ToString().Trim();
                    string adminVal = r["Admin"] == DBNull.Value ? "" : r["Admin"].ToString().Trim();

                    var u = new UserStatRow
                    {
                        Name = r["MyFullName"].ToString(),
                        Phone = r["MyPhoneNumber"] == DBNull.Value ? "" : r["MyPhoneNumber"].ToString(),
                        Area = r["Area"] == DBNull.Value ? "" : r["Area"].ToString(),
                        Vegetarian = IsYes(r["Vegetarian"]),
                        Vegan = IsYes(r["Vegan"]),
                        Kosher = IsYes(r["Kosher"]),
                        Gluten = IsYes(r["Gluten"]),
                        Peanuts = IsYes(r["Peanuts"]),
                        TreeNuts = IsYes(r["TreeNuts"]),
                        Fish = IsYes(r["Fish"]),
                        Sesame = IsYes(r["Sesame"]),
                        Milk = IsYes(r["Milk"]),
                        IsAdmin = (adminVal == "כן"),
                        IsRestAdmin = (!string.IsNullOrEmpty(restAdminVal) && restAdminVal != "לא"),
                        RestaurantName = (!string.IsNullOrEmpty(restAdminVal) && restAdminVal != "לא") ? restAdminVal : ""
                    };

                    u.AreaHebrew = TranslateArea(u.Area);
                    u.TagsHtml = BuildTagsHtml(u);
                    u.RoleTag = BuildRoleTag(u);

                    list.Add(u);
                }
            }

            return list;
        }

        // המרה של ערך מ-DB ל-bool: "כן" => true, אחרת false
        private bool IsYes(object value)
        {
            if (value == DBNull.Value) return false;
            string s = value.ToString().Trim();
            return s == "כן";
        }

        private string TranslateArea(string area)
        {
            switch (area)
            {
                case "Darom": return "דרום";
                case "Merkaz": return "מרכז";
                case "Tzafon": return "צפון";
                default: return area;
            }
        }

        private string BuildTagsHtml(UserStatRow u)
        {
            var sb = new StringBuilder();
            if (u.Vegetarian) sb.Append("<span class='tag diet'>🥗 צמחוני</span>");
            if (u.Vegan) sb.Append("<span class='tag diet'>🌱 טבעוני</span>");
            if (u.Kosher) sb.Append("<span class='tag diet'>✡️ כשר</span>");
            if (u.Gluten) sb.Append("<span class='tag allergy'>🌾 גלוטן</span>");
            if (u.Peanuts) sb.Append("<span class='tag allergy'>🥜 בוטנים</span>");
            if (u.TreeNuts) sb.Append("<span class='tag allergy'>🌰 אגוזים</span>");
            if (u.Fish) sb.Append("<span class='tag allergy'>🐟 דגים</span>");
            if (u.Sesame) sb.Append("<span class='tag allergy'>🌿 שומשום</span>");
            if (u.Milk) sb.Append("<span class='tag allergy'>🥛 חלב</span>");
            return sb.ToString();
        }

        private string BuildRoleTag(UserStatRow u)
        {
            if (u.IsAdmin) return "<span class='tag role-admin'>⚙️ מנהל מערכת</span>";
            if (u.IsRestAdmin) return "<span class='tag role-rest'>🍽️ מנהל מסעדה: " + System.Web.HttpUtility.HtmlEncode(u.RestaurantName) + "</span>";
            return "";
        }

        // ===== KPIs וגרפים =====

        private void BindKPIs(List<UserStatRow> all)
        {
            LblTotalUsers.Text = all.Count.ToString();
            LblVegCount.Text = all.Count(u => u.Vegetarian || u.Vegan).ToString();
            LblKosherCount.Text = all.Count(u => u.Kosher).ToString();
            LblAllergyCount.Text = all.Count(u => u.HasAnyAllergy).ToString();
            LblAdminsCount.Text = all.Count(u => u.IsAdmin || u.IsRestAdmin).ToString();
        }

        private void BindAreaChart(List<UserStatRow> all)
        {
            string[] order = { "Darom", "Merkaz", "Tzafon" };
            string[] labels = { "דרום", "מרכז", "צפון" };

            var bars = new List<StatBar>();
            for (int i = 0; i < order.Length; i++)
            {
                int count = all.Count(u => u.Area == order[i]);
                bars.Add(new StatBar { Label = labels[i], Count = count });
            }
            ApplyPercentages(bars);

            RepeaterAreas.DataSource = bars;
            RepeaterAreas.DataBind();
        }

        private void BindDietChart(List<UserStatRow> all)
        {
            var bars = new List<StatBar>
            {
                new StatBar { Label = "🥗 צמחוני", Count = all.Count(u => u.Vegetarian) },
                new StatBar { Label = "🌱 טבעוני", Count = all.Count(u => u.Vegan) },
                new StatBar { Label = "✡️ כשר", Count = all.Count(u => u.Kosher) }
            };
            ApplyPercentages(bars);

            RepeaterDiets.DataSource = bars;
            RepeaterDiets.DataBind();
        }

        private void BindAllergyChart(List<UserStatRow> all)
        {
            var bars = new List<StatBar>
            {
                new StatBar { Label = "🌾 גלוטן", Count = all.Count(u => u.Gluten) },
                new StatBar { Label = "🥜 בוטנים", Count = all.Count(u => u.Peanuts) },
                new StatBar { Label = "🌰 אגוזים", Count = all.Count(u => u.TreeNuts) },
                new StatBar { Label = "🐟 דגים", Count = all.Count(u => u.Fish) },
                new StatBar { Label = "🌿 שומשום", Count = all.Count(u => u.Sesame) },
                new StatBar { Label = "🥛 חלב", Count = all.Count(u => u.Milk) }
            };
            // מציג ממיון יורד כדי שהאלרגיה הנפוצה ביותר תהיה בראש
            bars = bars.OrderByDescending(b => b.Count).ToList();
            ApplyPercentages(bars);

            RepeaterAllergies.DataSource = bars;
            RepeaterAllergies.DataBind();
        }

        private void ApplyPercentages(List<StatBar> bars)
        {
            if (bars.Count == 0) return;
            int max = bars.Max(b => b.Count);
            if (max == 0) return;
            foreach (var b in bars)
            {
                b.Percent = b.Count * 100 / max;
                if (b.Count > 0 && b.Percent < 3) b.Percent = 3;
            }
        }

        // ===== סינון רשימת המשתמשים =====

        private void BindFilteredList(List<UserStatRow> all)
        {
            string areaF = DdlArea.SelectedValue;
            string dietF = DdlDiet.SelectedValue;
            string allergyF = DdlAllergy.SelectedValue;
            string roleF = DdlRole.SelectedValue;
            string nameF = TxtName.Text == null ? "" : TxtName.Text.Trim();

            IEnumerable<UserStatRow> q = all;
            if (!string.IsNullOrEmpty(areaF)) q = q.Where(u => u.Area == areaF);
            if (!string.IsNullOrEmpty(dietF)) q = q.Where(u => u.HasDiet(dietF));
            if (!string.IsNullOrEmpty(allergyF)) q = q.Where(u => u.HasAllergy(allergyF));
            if (!string.IsNullOrEmpty(roleF)) q = q.Where(u => u.MatchesRole(roleF));
            if (!string.IsNullOrEmpty(nameF))
            {
                string lower = nameF.ToLower();
                q = q.Where(u => u.Name != null && u.Name.ToLower().Contains(lower));
            }

            var filtered = q.ToList();

            LblTotal.Text = all.Count.ToString();
            LblShowing.Text = filtered.Count.ToString();

            if (filtered.Count == 0)
            {
                RepeaterUsers.Visible = false;
                PnlEmpty.Visible = true;
            }
            else
            {
                RepeaterUsers.Visible = true;
                PnlEmpty.Visible = false;
                RepeaterUsers.DataSource = filtered;
                RepeaterUsers.DataBind();
            }
        }
    }
}
