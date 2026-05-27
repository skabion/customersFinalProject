// מחולל תיק הפרויקט - קורא את קוד המקור האמיתי ומייצר קובץ HTML שנפתח ב-Word.
const fs = require('fs');

const ROOT = 'c:\\Users\\Ariel\\Desktop\\customersFinalProject\\ArielProject\\ArielProjectVS\\';
const SUP  = 'c:\\Users\\Ariel\\Desktop\\customersFinalProject\\ArielProject\\Supplier\\';

function esc(s){ return String(s).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;'); }
function read(p){ try { return fs.readFileSync(p,'utf8'); } catch(e){ return null; } }

// בלוק קוד: כותרת עם שם הקובץ + תוכן הקובץ האמיתי, משוחרר תווים
function codeBlock(file, fullPath){
  const c = read(fullPath);
  if(c === null) return '<div class="note">[[הקובץ '+esc(file)+' לא נמצא בעת היצירה]]</div>';
  return '<div class="filelabel">'+esc(file)+'</div><pre class="code">'+esc(c)+'</pre>';
}

// ====== תרשימים (SVG) - גרסה ראשונית לציור מחדש ב-draw.io ======

function svgERD(){
  return `<svg class="diagram" viewBox="0 0 940 640" xmlns="http://www.w3.org/2000/svg">
  <defs><marker id="arrow" markerWidth="10" markerHeight="10" refX="8" refY="3" orient="auto"><path d="M0,0 L8,3 L0,6 Z" fill="#555"/></marker></defs>
  <!-- MyUsers -->
  <rect x="30" y="30" width="250" height="210" rx="6" fill="#fbeeee" stroke="#7a1f1f" stroke-width="2"/>
  <rect x="30" y="30" width="250" height="30" rx="6" fill="#7a1f1f"/>
  <text x="155" y="51" fill="#fff" font-size="15" text-anchor="middle" font-family="Arial">MyUsers</text>
  <text x="42" y="80" font-size="12" font-family="Consolas">UserID (PK)</text>
  <text x="42" y="100" font-size="12" font-family="Consolas">MyFullName</text>
  <text x="42" y="120" font-size="12" font-family="Consolas">MyPassword</text>
  <text x="42" y="140" font-size="12" font-family="Consolas">MyPhoneNumber</text>
  <text x="42" y="160" font-size="12" font-family="Consolas">Vegetarian / Vegan ...</text>
  <text x="42" y="180" font-size="12" font-family="Consolas">Area</text>
  <text x="42" y="200" font-size="12" font-family="Consolas">Admin</text>
  <text x="42" y="220" font-size="12" font-family="Consolas">RestaurantAdmin</text>
  <!-- MyRestaurants -->
  <rect x="660" y="30" width="250" height="190" rx="6" fill="#eef5fb" stroke="#1f4e7a" stroke-width="2"/>
  <rect x="660" y="30" width="250" height="30" rx="6" fill="#1f4e7a"/>
  <text x="785" y="51" fill="#fff" font-size="15" text-anchor="middle" font-family="Arial">MyRestaurants</text>
  <text x="672" y="80" font-size="12" font-family="Consolas">UserID (PK)</text>
  <text x="672" y="100" font-size="12" font-family="Consolas">Restaurants</text>
  <text x="672" y="120" font-size="12" font-family="Consolas">Region / FoodType</text>
  <text x="672" y="140" font-size="12" font-family="Consolas">Kosher / ReplacementMeals</text>
  <text x="672" y="160" font-size="12" font-family="Consolas">SmallTables</text>
  <text x="672" y="180" font-size="12" font-family="Consolas">MediumTables</text>
  <text x="672" y="200" font-size="12" font-family="Consolas">LargeTables</text>
  <!-- MyBooking -->
  <rect x="345" y="330" width="250" height="190" rx="6" fill="#eefbf0" stroke="#1f7a3d" stroke-width="2"/>
  <rect x="345" y="330" width="250" height="30" rx="6" fill="#1f7a3d"/>
  <text x="470" y="351" fill="#fff" font-size="15" text-anchor="middle" font-family="Arial">MyBooking</text>
  <text x="357" y="380" font-size="12" font-family="Consolas">UserID (PK)</text>
  <text x="357" y="400" font-size="12" font-family="Consolas">Guest</text>
  <text x="357" y="420" font-size="12" font-family="Consolas">PhoneNum</text>
  <text x="357" y="440" font-size="12" font-family="Consolas">Restaurant</text>
  <text x="357" y="460" font-size="12" font-family="Consolas">InvDate / InvTime</text>
  <text x="357" y="480" font-size="12" font-family="Consolas">NumGuest</text>
  <text x="357" y="500" font-size="12" font-family="Consolas">TableType</text>
  <!-- Cities -->
  <rect x="30" y="360" width="200" height="80" rx="6" fill="#f7f1fb" stroke="#5a1f7a" stroke-width="2"/>
  <rect x="30" y="360" width="200" height="30" rx="6" fill="#5a1f7a"/>
  <text x="130" y="381" fill="#fff" font-size="15" text-anchor="middle" font-family="Arial">Cities</text>
  <text x="42" y="410" font-size="12" font-family="Consolas">CityName</text>
  <text x="42" y="430" font-size="11" font-family="Arial" fill="#666">(לאימות כתובת איסוף)</text>
  <!-- Taxis (supplier, separate DB) -->
  <rect x="690" y="350" width="220" height="200" rx="6" fill="#fdf3e6" stroke="#a05a00" stroke-width="2" stroke-dasharray="6 4"/>
  <rect x="690" y="350" width="220" height="30" rx="6" fill="#a05a00"/>
  <text x="800" y="371" fill="#fff" font-size="14" text-anchor="middle" font-family="Arial">Taxis (ספק חיצוני)</text>
  <text x="702" y="400" font-size="12" font-family="Consolas">RideID (PK)</text>
  <text x="702" y="420" font-size="12" font-family="Consolas">CustomerName</text>
  <text x="702" y="440" font-size="12" font-family="Consolas">RestaurantName</text>
  <text x="702" y="460" font-size="12" font-family="Consolas">RideDate / RideTime</text>
  <text x="702" y="480" font-size="12" font-family="Consolas">DriverNum</text>
  <text x="702" y="500" font-size="12" font-family="Consolas">Adress</text>
  <text x="702" y="525" font-size="11" font-family="Arial" fill="#666">TaxiDB1.accdb (נפרד)</text>
  <!-- Relationships -->
  <line x1="170" y1="240" x2="380" y2="330" stroke="#555" stroke-width="1.5" marker-end="url(#arrow)"/>
  <text x="250" y="280" font-size="13" font-family="Arial">1 ── ∞</text>
  <line x1="720" y1="220" x2="560" y2="330" stroke="#555" stroke-width="1.5" marker-end="url(#arrow)"/>
  <text x="610" y="280" font-size="13" font-family="Arial">1 ── ∞</text>
  <line x1="595" y1="430" x2="690" y2="430" stroke="#a05a00" stroke-width="1.5" stroke-dasharray="5 3" marker-end="url(#arrow)"/>
  <text x="600" y="420" font-size="11" font-family="Arial" fill="#a05a00">קריאת Web Service</text>
  <line x1="230" y1="400" x2="345" y2="430" stroke="#5a1f7a" stroke-width="1.2" stroke-dasharray="4 3"/>
</svg>`;
}

function svgDFD(){
  return `<svg class="diagram" viewBox="0 0 940 560" xmlns="http://www.w3.org/2000/svg">
  <defs><marker id="ar2" markerWidth="10" markerHeight="10" refX="8" refY="3" orient="auto"><path d="M0,0 L8,3 L0,6 Z" fill="#444"/></marker></defs>
  <!-- process -->
  <circle cx="470" cy="280" r="95" fill="#fbeeee" stroke="#7a1f1f" stroke-width="2.5"/>
  <text x="470" y="272" text-anchor="middle" font-size="17" font-family="Arial" fill="#7a1f1f">0</text>
  <text x="470" y="296" text-anchor="middle" font-size="15" font-family="Arial" fill="#7a1f1f">מערכת EatIt</text>
  <!-- external entities -->
  <rect x="40" y="40" width="190" height="55" fill="#eef5fb" stroke="#1f4e7a" stroke-width="2"/>
  <text x="135" y="74" text-anchor="middle" font-size="14" font-family="Arial">לקוח / משתמש</text>
  <rect x="710" y="40" width="190" height="55" fill="#eef5fb" stroke="#1f4e7a" stroke-width="2"/>
  <text x="805" y="74" text-anchor="middle" font-size="14" font-family="Arial">מנהל מסעדה</text>
  <rect x="710" y="470" width="190" height="55" fill="#eef5fb" stroke="#1f4e7a" stroke-width="2"/>
  <text x="805" y="504" text-anchor="middle" font-size="14" font-family="Arial">מנהל מערכת</text>
  <rect x="40" y="470" width="190" height="55" fill="#fdf3e6" stroke="#a05a00" stroke-width="2"/>
  <text x="135" y="504" text-anchor="middle" font-size="14" font-family="Arial">ספק ההסעות</text>
  <!-- data stores -->
  <rect x="350" y="490" width="240" height="40" fill="#eefbf0" stroke="#1f7a3d" stroke-width="2"/>
  <text x="470" y="515" text-anchor="middle" font-size="13" font-family="Arial">D1 | DBusers1.accdb</text>
  <!-- arrows entity<->process -->
  <line x1="230" y1="70" x2="395" y2="225" stroke="#444" stroke-width="1.5" marker-end="url(#ar2)"/>
  <line x1="710" y1="70" x2="545" y2="225" stroke="#444" stroke-width="1.5" marker-end="url(#ar2)"/>
  <line x1="710" y1="497" x2="560" y2="330" stroke="#444" stroke-width="1.5" marker-end="url(#ar2)"/>
  <line x1="470" y1="375" x2="470" y2="488" stroke="#1f7a3d" stroke-width="1.5" marker-end="url(#ar2)"/>
  <line x1="470" y1="488" x2="470" y2="377" stroke="#1f7a3d" stroke-width="1.5" marker-end="url(#ar2)"/>
  <line x1="230" y1="497" x2="378" y2="330" stroke="#a05a00" stroke-width="1.5" marker-end="url(#ar2)"/>
  <text x="250" y="430" font-size="12" font-family="Arial" fill="#a05a00">BookRide / CancelRide</text>
  <text x="250" y="150" font-size="12" font-family="Arial">בקשות הזמנה</text>
  <text x="620" y="150" font-size="12" font-family="Arial">ניהול מסעדה</text>
</svg>`;
}

function svgUseCase(){
  return `<svg class="diagram" viewBox="0 0 940 620" xmlns="http://www.w3.org/2000/svg">
  <!-- system boundary -->
  <rect x="300" y="20" width="340" height="580" rx="10" fill="#fcfcfc" stroke="#999" stroke-width="2"/>
  <text x="470" y="45" text-anchor="middle" font-size="15" font-family="Arial" fill="#666">מערכת EatIt</text>
  ${['התחברות / הרשמה','חיפוש מסעדה (קטלוג)','הזמנת שולחן','הזמנת הסעה (Web Service)','עדכון / ביטול הזמנה','היסטוריית הזמנות','סטטיסטיקת מסעדה','ניהול נתוני משתמשים'].map((t,i)=>{
     const y = 90 + i*62;
     return `<ellipse cx="470" cy="${y}" rx="135" ry="24" fill="#fbeeee" stroke="#7a1f1f" stroke-width="1.5"/><text x="470" y="${y+5}" text-anchor="middle" font-size="13" font-family="Arial">${t}</text>`;
  }).join('')}
  <!-- actors -->
  ${actor(70,160,'לקוח')}
  ${actor(70,470,'מנהל מסעדה')}
  ${actor(820,160,'מנהל מערכת')}
  ${actor(820,470,'ספק ההסעות')}
  <line x1="120" y1="170" x2="335" y2="90" stroke="#777"/>
  <line x1="120" y1="170" x2="335" y2="152" stroke="#777"/>
  <line x1="120" y1="170" x2="335" y2="214" stroke="#777"/>
  <line x1="120" y1="470" x2="335" y2="400" stroke="#777"/>
  <line x1="120" y1="470" x2="335" y2="276" stroke="#777"/>
  <line x1="820" y1="170" x2="605" y2="462" stroke="#777"/>
  <line x1="820" y1="170" x2="605" y2="524" stroke="#777"/>
  <line x1="820" y1="470" x2="605" y2="214" stroke="#777"/>
</svg>`;
}
function actor(x,y,name){
  return `<circle cx="${x}" cy="${y-28}" r="12" fill="none" stroke="#333" stroke-width="2"/>
  <line x1="${x}" y1="${y-16}" x2="${x}" y2="${y+14}" stroke="#333" stroke-width="2"/>
  <line x1="${x-16}" y1="${y-4}" x2="${x+16}" y2="${y-4}" stroke="#333" stroke-width="2"/>
  <line x1="${x}" y1="${y+14}" x2="${x-13}" y2="${y+34}" stroke="#333" stroke-width="2"/>
  <line x1="${x}" y1="${y+14}" x2="${x+13}" y2="${y+34}" stroke="#333" stroke-width="2"/>
  <text x="${x}" y="${y+52}" text-anchor="middle" font-size="13" font-family="Arial">${name}</text>`;
}

function svgTree(){
  const box=(x,y,w,t,fill)=>`<rect x="${x}" y="${y}" width="${w}" height="38" rx="5" fill="${fill||'#eef5fb'}" stroke="#1f4e7a" stroke-width="1.5"/><text x="${x+w/2}" y="${y+24}" text-anchor="middle" font-size="13" font-family="Arial">${t}</text>`;
  return `<svg class="diagram" viewBox="0 0 940 470" xmlns="http://www.w3.org/2000/svg">
  ${box(370,20,200,'מערכת EatIt','#fbeeee')}
  ${box(60,120,210,'ניהול משתמשים')}
  ${box(365,120,210,'הזמנות')}
  ${box(680,120,210,'ניהול וסטטיסטיקה')}
  <line x1="470" y1="58" x2="165" y2="120" stroke="#777"/>
  <line x1="470" y1="58" x2="470" y2="120" stroke="#777"/>
  <line x1="470" y1="58" x2="785" y2="120" stroke="#777"/>
  ${box(40,210,110,'הרשמה','#fff')}
  ${box(160,210,110,'התחברות','#fff')}
  ${box(40,260,110,'התנתקות','#fff')}
  ${box(160,260,110,'אזור אישי','#fff')}
  <line x1="165" y1="158" x2="95" y2="210" stroke="#aaa"/>
  <line x1="165" y1="158" x2="215" y2="210" stroke="#aaa"/>
  ${box(330,210,130,'חיפוש מסעדה','#fff')}
  ${box(330,260,130,'בדיקת זמינות','#fff')}
  ${box(330,310,130,'הזמנת שולחן','#fff')}
  ${box(330,360,130,'הזמנת הסעה','#fff')}
  ${box(480,210,110,'עדכון','#fff')}
  ${box(480,260,110,'ביטול','#fff')}
  ${box(480,310,110,'היסטוריה','#fff')}
  <line x1="470" y1="158" x2="430" y2="210" stroke="#aaa"/>
  <line x1="470" y1="158" x2="520" y2="210" stroke="#aaa"/>
  ${box(670,210,140,'רשימת מסעדות','#fff')}
  ${box(670,260,140,'סטטיסטיקת מסעדה','#fff')}
  ${box(670,310,140,'נתוני משתמשים','#fff')}
  <line x1="785" y1="158" x2="740" y2="210" stroke="#aaa"/>
</svg>`;
}

// ====== טבלאות מסד הנתונים ======
function fieldsTable(rows){
  let h = '<table class="fields"><tr><th>שם השדה</th><th>סוג</th><th>תיאור</th></tr>';
  for(const r of rows){ h += '<tr><td class="fname">'+esc(r[0])+'</td><td>'+esc(r[1])+'</td><td>'+esc(r[2])+'</td></tr>'; }
  return h + '</table>';
}

// ====== מסכי האתר ======
const screens = [
  { file:'Login.aspx', cs:'Login.aspx.cs', title:'מסך התחברות (Login)', desc:
    `<p class="desc">מסך הכניסה למערכת. המשתמש מזין שם מלא וסיסמה, והמערכת מאמתת אותם מול טבלת <code>MyUsers</code>. בהתחברות מוצלחת נשמרים ב-<code>Session</code> פרטי המשתמש (שם, טלפון, והאם הוא מנהל מסעדה או מנהל מערכת), ומתבצעת הפניה לדף הבית. בכניסה כושלת מוצגת הודעת שגיאה.</p>
     <p class="desc"><b>סוג משתמש:</b> פתוח לכולם (אורח). <b>פקדים עיקריים:</b> שני TextBox (שם, סיסמה), Button, ו-Label לשגיאה. <b>אבטחה:</b> שאילתת האימות נכתבה מחדש עם <b>פרמטרים</b> (<code>OleDbParameter</code>) במקום שרשור מחרוזות, כדי למנוע SQL Injection (ראה פרק האתגרים).</p>` },
  { file:'insert.aspx', cs:'insert.aspx.cs', title:'מסך הרשמה (Insert)', desc:
    `<p class="desc">טופס פתיחת חשבון משתמש חדש. המשתמש מזין שם מלא, סיסמה וטלפון, בוחר אזור, ומסמן העדפות תזונה (צמחוני/טבעוני/כשר) ואלרגיות (גלוטן, בוטנים, אגוזים, דגים, שומשום, חלב). כל שדה עובר ולידציה בצד השרת (שם בן שתי מילים באנגלית, סיסמה חזקה, טלפון תקין) לפני ההכנסה לטבלת <code>MyUsers</code>.</p>
     <p class="desc"><b>סוג משתמש:</b> אורח. <b>פקדים עיקריים:</b> שלושה TextBox, DropDownList לאזור, תשעה CheckBox להעדפות/אלרגיות, ו-Button. <b>אבטחה:</b> שאילתת ה-INSERT נכתבה עם <b>13 פרמטרים</b> במקום שרשור מחרוזות.</p>` },
  { file:'HomePage.aspx', cs:'HomePage.aspx.cs', title:'דף הבית (HomePage)', desc:
    `<p class="desc">מסך הנחיתה הראשי. מציג ברכת פתיחה (משתנה לפי שעת היום בעזרת JavaScript) וכפתורי ניווט לכל חלקי המערכת. התצוגה משתנה לפי מצב ההתחברות: לאורח מוצגים כפתורי התחברות והרשמה, ולמשתמש מחובר מוצגים אזור אישי והתנתקות. כפתור "דף מנהל" מוצג רק אם ב-<code>Session</code> קיים סימון מנהל מסעדה או מנהל מערכת.</p>
     <p class="desc"><b>סוג משתמש:</b> כולם. <b>פקדים עיקריים:</b> Label לשם, קישורים, ו-LinkButton להתנתקות (<code>Session.Abandon()</code>). <b>גישה למסד:</b> אין — הדף עובד אך ורק מול ה-Session.</p>` },
  { file:'Catalog.aspx', cs:'Catalog.aspx.cs', title:'קטלוג וחיפוש מסעדות (Catalog)', desc:
    `<p class="desc">מסך חיפוש המסעדות. המשתמש בוחר אזור וסוג מטבח, ויכול לסמן סינון לפי כשרות או קיום מנות חלופיות לאלרגיים. בלחיצה על "חיפוש" נשלפות המסעדות המתאימות מטבלת <code>MyRestaurants</code> ומוצגות ככרטיסיות ב-DataList. כל כרטיס מוביל להזמנת שולחן באותה מסעדה.</p>
     <p class="desc"><b>סוג משתמש:</b> אורח. <b>פקדים עיקריים:</b> שני DropDownList, שני CheckBox, Button, ו-DataList בעל חמש עמודות. <b>לוגיקה:</b> בניית שאילתת <code>SELECT</code> דינמית עם תנאים לפי הבחירות. <b>אבטחה:</b> כאן עדיין נעשה שרשור מחרוזות (הערכים מגיעים מרשימות סגורות, אך זו נקודה לשיפור עתידי).</p>` },
  { file:'Booking.aspx', cs:'Booking.aspx.cs', title:'הזמנת שולחן + הזמנת הסעה (Booking)', desc:
    `<p class="desc">מסך ההזמנה המרכזי. המשתמש בוחר תאריך ומספר סועדים, והמערכת מחשבת את סוג השולחן הדרוש (קטן/בינוני/גדול) ומציגה טבלת שעות פנויות. בדיקת הזמינות סופרת כמה שולחנות מאותו סוג כבר תפוסים בחלון של שעתיים סביב כל שעה, ומשווה למספר השולחנות במסעדה. לאחר בחירת שעה נשמרת ההזמנה בטבלת <code>MyBooking</code>, ומוצג <b>תשאול הסעה</b>: אם המשתמש מעוניין, הוא מזין כתובת איסוף והמערכת קוראת ל-Web Service של ספק ההסעות (ראה פרק שירותי רשת). הדף כולל גם טיימר ספירה-לאחור של 3 דקות להשלמת ההזמנה.</p>
     <p class="desc"><b>סוג משתמש:</b> משתמש רשום (בדיקת <code>Session["User"]</code>). <b>פקדים עיקריים:</b> TextBox לתאריך ולמספר סועדים, GridView לשעות, ו-Panel לתשאול ההסעה ולכתובת.</p>` },
  { file:'UpdateBookings.aspx', cs:'UpdateBookings.aspx.cs', title:'ניהול ועדכון הזמנות (UpdateBookings)', desc:
    `<p class="desc">דף משולב לניהול ההזמנות העתידיות של המשתמש. במצב הרשימה מוצגות ההזמנות העתידיות; בלחיצה על "ערוך" עוברים לטופס עריכה שבו ניתן לבחור תאריך, מספר סועדים ושעה חדשים, או לבטל את ההזמנה. לאחר עדכון מוצג <b>תשאול הסעה למועד החדש</b>: בחירת "לא" מבטלת את ההסעה הישנה אצל הספק, ובחירת "כן" מבטלת את הישנה ומזמינה הסעה חדשה לתאריך, לשעה ולכתובת המעודכנים. גם מחיקת הזמנה מבטלת את ההסעה הנלווית אצל הספק — כדי שלא תיוותר "מונית יתומה" (ראה פרק שירותי רשת ופרק האתגרים).</p>
     <p class="desc"><b>סוג משתמש:</b> משתמש רשום. <b>פקדים עיקריים:</b> שני GridView (רשימה ושעות), TextBox, CompareValidator לתאריך, ו-Panel לתשאול ההסעה.</p>` },
  { file:'BookingHistory.aspx', cs:'BookingHistory.aspx.cs', title:'היסטוריית הזמנות (BookingHistory)', desc:
    `<p class="desc">מציג למשתמש את הזמנות העבר שלו (תאריך שכבר חלף). ניתן למיין את הרשימה לפי תאריך, סוג מטבח או אזור, ולמחוק את כל ההיסטוריה בלחיצה אחת (לאחר אישור). השליפה מבצעת <code>INNER JOIN</code> בין <code>MyBooking</code> ל-<code>MyRestaurants</code> כדי להציג גם את סוג המטבח והאזור.</p>
     <p class="desc"><b>סוג משתמש:</b> משתמש רשום (בדיקת <code>Session["User"]</code> ו-<code>Session["Phone"]</code>). <b>פקדים עיקריים:</b> DropDownList למיון (AutoPostBack), Button למחיקה, GridView, ו-Panel ל"אין היסטוריה".</p>` },
  { file:'PersonalArea.aspx', cs:'PersonalArea.aspx.cs', title:'אזור אישי (PersonalArea)', desc:
    `<p class="desc">דף תפריט קצר למשתמש הרשום, ובו שני שערים: עריכת/ביטול הזמנה (UpdateBookings) והיסטוריית ההזמנות (BookingHistory). הדף מציג את שם המשתמש מתוך ה-Session.</p>
     <p class="desc"><b>סוג משתמש:</b> משתמש רשום (בדיקת <code>Session["User"]</code>, אחרת הפניה ל-Login). <b>גישה למסד:</b> אין.</p>` },
  { file:'AllRestaurants.aspx', cs:'AllRestaurants.aspx.cs', title:'כל המסעדות — מנהל מערכת (AllRestaurants)', desc:
    `<p class="desc">מסך למנהל המערכת המציג טבלה של כל המסעדות (שם, אזור, סוג מטבח) ממוינת לפי שם. לחיצה על מסעדה מעבירה לדף הסטטיסטיקות שלה. המיון מתבצע ב-SQL (<code>ORDER BY</code>).</p>
     <p class="desc"><b>סוג משתמש:</b> מנהל מערכת בלבד (בדיקת <code>Session["User"]</code> ואז <code>Session["Admin"]</code>). <b>פקדים עיקריים:</b> GridView עם כפתור בחירה, ו-Panel ל"אין מסעדות".</p>` },
  { file:'RestaurantAdmin.aspx', cs:'RestaurantAdmin.aspx.cs', title:'לוח בקרה למנהל מסעדה (RestaurantAdmin)', desc:
    `<p class="desc">לוח בקרה וסטטיסטיקה למסעדה. מציג מדדים (סך ההזמנות, הזמנות עתידיות, סך הסועדים וממוצע הסועדים), שלושה גרפי-עמודות (התפלגות לפי גודל שולחן, השעות הפופולריות, והימים העמוסים בשבוע), וטבלת הזמנות עתידיות הניתנת למיון. כל החישובים (COUNT, SUM, AVG, GROUP BY) מתבצעים ב-SQL. מנהל מסעדה רואה את המסעדה שלו; מנהל מערכת מגיע דרך AllRestaurants עם פרמטר שם המסעדה.</p>
     <p class="desc"><b>סוג משתמש:</b> מנהל מסעדה או מנהל מערכת. <b>פקדים עיקריים:</b> Label רבים (KPIs וגרפים מוזרקים כ-HTML), DropDownList למיון, ו-GridView. <b>אבטחה:</b> שם המסעדה מוכנס לשאילתות בשרשור — נקודה לשיפור עתידי.</p>` },
  { file:'UserStats.aspx', cs:'UserStats.aspx.cs', title:'נתוני משתמשים — מנהל מערכת (UserStats)', desc:
    `<p class="desc">מסך אנליטיקה על כלל המשתמשים: מדדים (סך משתמשים, צמחונים/טבעונים, שומרי כשרות, בעלי אלרגיות, מנהלים), שלושה גרפי-עמודות (אזור, העדפות תזונה, אלרגיות), ופאנל סינון/חיפוש משתמשים עם כרטיסיות תוצאה. חלק מהחישובים נעשים בקוד C# בלולאות, וחלק (גרף האלרגיות) בשאילתת <code>UNION ALL</code>.</p>
     <p class="desc"><b>סוג משתמש:</b> מנהל מערכת בלבד. <b>פקדים עיקריים:</b> Label רבים, ארבעה DropDownList, TextBox לחיפוש, שני Button, ו-Panel ל"לא נמצאו".</p>` },
];

let screensHtml = '';
for(const s of screens){
  screensHtml += '<h2>'+esc(s.title)+'</h2>' + s.desc;
  screensHtml += codeBlock(s.file, ROOT + s.file);
  if(s.cs) screensHtml += codeBlock(s.cs, ROOT + s.cs);
}

// ====== הרכבת המסמך ======
const html = '﻿<!DOCTYPE html>\n<html dir="rtl" lang="he"><head><meta charset="utf-8"/>\n<title>תיק פרויקט — EatIt</title>\n<style>\n'+
`@page { size: A4; margin: 2cm; }
body { font-family: 'David','Arial',sans-serif; direction: rtl; font-size: 12pt; line-height:1.6; color:#222; margin:0 28px; }
h1.chapter { page-break-before: always; color:#7a1f1f; border-bottom:3px solid #c9a24b; padding-bottom:6px; font-size:22pt; }
h2 { color:#7a1f1f; font-size:15pt; margin-top:22px; border-right:5px solid #c9a24b; padding-right:8px; }
h3 { color:#333; font-size:13pt; }
p.desc { text-align: justify; }
code { font-family:Consolas,monospace; background:#f0f0f0; padding:0 3px; direction:ltr; display:inline-block; }
table.fields { border-collapse: collapse; width:100%; margin:10px 0; }
table.fields th, table.fields td { border:1px solid #999; padding:5px 8px; font-size:11pt; vertical-align:top; }
table.fields th { background:#7a1f1f; color:#fff; }
table.fields td.fname { font-family:Consolas,monospace; direction:ltr; text-align:left; white-space:nowrap; color:#06419a; }
.filelabel { background:#2d2d2d; color:#fff; font-family:Consolas,monospace; direction:ltr; padding:4px 10px; font-size:10pt; margin-top:16px; }
pre.code { direction:ltr; text-align:left; background:#f6f6f6; border:1px solid #ccc; border-top:none; padding:10px; font-family:Consolas,'Courier New',monospace; font-size:9pt; line-height:1.35; white-space:pre-wrap; word-wrap:break-word; margin-top:0; }
.note { background:#fff7e6; border-right:4px solid #c9a24b; padding:8px 12px; margin:12px 0; }
.ph { background:#fff3a0; color:#7a5b00; font-weight:bold; padding:1px 5px; border-radius:3px; }
.cover { text-align:center; padding-top:70px; page-break-after:always; }
.cover .school { font-size:17pt; margin-bottom:6px; }
.cover .alt { font-size:13pt; color:#666; }
.cover .title { font-size:40pt; color:#7a1f1f; font-weight:bold; margin:46px 0 10px; }
.cover .subtitle { font-size:16pt; color:#444; }
.cover .meta { font-size:14pt; line-height:2.3; margin-top:60px; }
svg.diagram { display:block; margin:16px auto; max-width:100%; height:auto; }
ul.toc { font-size:13pt; line-height:1.9; list-style:none; }
ul.toc li.c { font-weight:bold; margin-top:6px; }
ul.toc li.s { padding-right:22px; font-weight:normal; }
.diagcap { text-align:center; font-size:11pt; color:#666; margin-bottom:18px; }
`+
'\n</style></head><body>\n'+

// ---------- שער ----------
`<div class="cover">
  <div class="school"><span class="ph">[[שם בית הספר]]</span></div>
  <div class="alt">חלופת "שירותי רשת" — תכנון ותכנות מערכות</div>
  <div class="title">EatIt</div>
  <div class="subtitle">מערכת לאיתור והזמנת מקום במסעדות, עם שירות הסעות חיצוני</div>
  <div class="meta">
    שם התלמיד: <span class="ph">[[שם מלא]]</span><br/>
    תעודת זהות: <span class="ph">[[ת"ז]]</span><br/>
    כיתה: <span class="ph">[[כיתה]]</span><br/>
    שם המנחה: <span class="ph">[[שם המורה]]</span><br/>
    תאריך הגשה: <span class="ph">[[תאריך]]</span>
  </div>
</div>`+

// ---------- תוכן עניינים ----------
`<h1 class="chapter" style="page-break-before:avoid">תוכן עניינים</h1>
<div class="note">לאחר פתיחת הקובץ ב-Word ניתן ליצור תוכן עניינים אוטומטי עם מספרי עמודים: <b>References → Table of Contents</b> (הכותרות כבר מוגדרות כ-Heading 1/2). להלן רשימת הפרקים:</div>
<ul class="toc">
  <li class="c">1. מבוא</li>
  <li class="s">1.1 הרקע לפרויקט · 1.2 מטרות המערכת · 1.3 תיאור המערכת · 1.4 גבולות המערכת לכל משתמש</li>
  <li class="s">1.5 סביבת הפיתוח ושפות · 1.6 שכבות המערכת · 1.7 אתגרים מרכזיים · 1.8 חידושים</li>
  <li class="c">2. ניתוח מערכת ותרשימים (ERD, DFD, Use-Case, עץ תהליכים)</li>
  <li class="c">3. בסיס הנתונים</li>
  <li class="c">4. שירותי רשת — ספק ההסעות (Web Service)</li>
  <li class="c">5. צד לקוח — מסכי האתר</li>
  <li class="c">6. רפלקציה — סיכום אישי</li>
  <li class="c">7. ביבליוגרפיה</li>
  <li class="c">8. נספחים</li>
</ul>`+

// ---------- 1. מבוא ----------
`<h1 class="chapter">1. מבוא</h1>
<h2>1.1 הרקע לפרויקט</h2>
<p class="desc"><b>שם הפרויקט:</b> EatIt — מערכת אינטרנטית לאיתור מסעדות ולהזמנת מקום בהן.</p>
<p class="desc"><b>תיאור:</b> המערכת מאפשרת למשתמש לחפש מסעדה לפי אזור וסוג מטבח, לסנן לפי כשרות והתאמה לאלרגיות, לבדוק זמינות שולחנות בתאריך ובשעה רצויים, ולהזמין מקום. בנוסף, ולמען נוחות הלקוח, המערכת מתממשקת ל<b>ספק הסעות חיצוני</b> (Web Service) ומאפשרת להזמין מונית לאיסוף עד למסעדה — וגם לעדכן או לבטל את ההסעה כשההזמנה משתנה. למנהלי המסעדות ולמנהל המערכת קיימים דפי ניהול וסטטיסטיקה.</p>
<p class="desc"><b>קהל היעד:</b> סועדים המחפשים מסעדה ומעוניינים בהזמנת מקום והסעה בקלות; בעלי מסעדות הרוצים לעקוב אחר ההזמנות והעומסים.</p>
<p class="desc"><b>הסיבות לבחירת הנושא:</b> <span class="ph">[[הסבר אישי קצר: למה בחרת דווקא מערכת הזמנת מסעדות והסעות — מה עורר אצלך את הרעיון]]</span></p>

<h2>1.2 מטרות המערכת</h2>
<p class="desc"><b>מטרת על:</b> לאפשר למשתמש למצוא מסעדה מתאימה ולהזמין בה מקום (ובמידת הצורך גם הסעה) בתהליך פשוט, מהיר וברור.</p>
<p class="desc"><b>מטרות נלוות:</b></p>
<p class="desc">• התאמת ההצעות להעדפות התזונה והאלרגיות של המשתמש.<br/>• מניעת הזמנות כפולות על אותו שולחן באמצעות בדיקת זמינות בזמן אמת.<br/>• מתן ערך מוסף ללקוח דרך שילוב שירות הסעות חיצוני.<br/>• מתן כלי ניהול וסטטיסטיקה לבעלי המסעדות ולמנהל המערכת.</p>

<h2>1.3 תיאור המערכת</h2>
<p class="desc">המערכת בנויה כאתר אינטרנט. משתמש חדש נרשם ובוחר את העדפותיו, מתחבר, ומגיע לדף הבית. משם הוא עובר לקטלוג, מסנן ובוחר מסעדה, ומבצע הזמנה הכוללת בחירת תאריך, מספר סועדים ושעה פנויה. לאחר ההזמנה הוא יכול להזמין הסעה. באזור האישי הוא יכול לצפות בהזמנות, לעדכן או לבטל אותן, ולראות היסטוריה. בעל מסעדה רואה לוח בקרה עם נתוני ההזמנות שלו, ומנהל המערכת רואה את כל המסעדות ונתוני כלל המשתמשים.</p>

<h2>1.4 גבולות המערכת לכל סוג משתמש</h2>
<p class="desc"><b>אורח (לא מחובר):</b> יכול לצפות בדף הבית, להירשם, להתחבר ולעיין בקטלוג המסעדות. אינו יכול להזמין.</p>
<p class="desc"><b>משתמש רשום:</b> כל מה שמותר לאורח, ובנוסף: הזמנת שולחן, הזמנת/עדכון/ביטול הסעה, צפייה ועריכה של ההזמנות שלו, וצפייה בהיסטוריה.</p>
<p class="desc"><b>מנהל מסעדה:</b> צופה בלוח הבקרה והסטטיסטיקה של המסעדה שבניהולו בלבד.</p>
<p class="desc"><b>מנהל מערכת:</b> צופה ברשימת כל המסעדות, בסטטיסטיקה של כל מסעדה, ובנתוני כלל המשתמשים.</p>

<h2>1.5 סביבת הפיתוח ושפות התכנות</h2>
<p class="desc"><b>סביבת פיתוח:</b> Microsoft Visual Studio. <b>טכנולוגיה:</b> ASP.NET Web Forms על גבי .NET Framework 4.7.2.</p>
<p class="desc"><b>שפות:</b> C# (לוגיקת צד השרת), HTML (מבנה הדפים, קובצי .aspx), CSS (עיצוב — הקובץ Site.css), JavaScript (טיימר ההזמנה וברכת שעת היום), ו-SQL (שאילתות מול מסד הנתונים).</p>

<h2>1.6 שכבות המערכת והפלטפורמה</h2>
<p class="desc">המערכת בנויה בשלוש שכבות לוגיות:</p>
<p class="desc"><b>1. שכבת הנתונים:</b> מסד נתונים Microsoft Access (<code>DBusers1.accdb</code>), בגישה דרך ספק <code>Microsoft.ACE.OLEDB.12.0</code> (OleDb).<br/>
<b>2. שכבת הלוגיקה והתצוגה:</b> דפי ASP.NET Web Forms (קובצי .aspx + code-behind ב-C#), הרצים בשרת והמגישים HTML לדפדפן.<br/>
<b>3. שכבת שירות חיצוני:</b> ספק ההסעות — Web Service נפרד (ASMX) עם מסד נתונים משלו (<code>TaxiDB1.accdb</code>), שהמערכת קוראת לו דרך Connected Service.</p>

<h2>1.7 אתגרים מרכזיים (בעיה ← פתרון)</h2>
<p class="desc"><b>בעיה 1 — חשיפה ל-SQL Injection:</b> שאילתות ההתחברות וההרשמה נבנו בשרשור מחרוזות עם קלט המשתמש, מה שמאפשר הזרקת SQL (למשל הקלדת <code>' OR '1'='1</code> בסיסמה). <b>פתרון:</b> השאילתות נכתבו מחדש עם <b>פרמטרים</b> (<code>OleDbParameter</code> וסימני <code>?</code>), כך שהקלט מטופל כנתון בלבד ולא כקוד.</p>
<p class="desc"><b>בעיה 2 — "מונית יתומה" בעדכון/ביטול הזמנה:</b> כאשר לקוח עדכן או ביטל הזמנה, ההסעה שכבר הוזמנה אצל הספק נשארה רשומה עם השעה והכתובת הישנות. <b>פתרון:</b> הוספת פעולת <code>CancelRide</code> לספק, ותשאול הסעה בעת עדכון: ביטול ההסעה הישנה ויצירת חדשה למועד המעודכן. כדי לזהות נסיעה במדויק נוספה לטבלת הספק עמודת תאריך (<code>RideDate</code>), כך שהזיהוי נעשה לפי תאריך + שעה ולא לפי שעה בלבד.</p>
<p class="desc"><b>בעיה 3 — מניעת הזמנות כפולות על שולחן:</b> צריך לוודא שלא יוזמנו יותר שולחנות מהקיים. <b>פתרון:</b> אלגוריתם בדיקת זמינות הסופר את השולחנות התפוסים מאותו סוג בחלון של שעתיים סביב השעה המבוקשת, ומשווה למלאי השולחנות במסעדה.</p>
<p class="desc"><b>בעיה 4 — ממשק לפי סוג משתמש:</b> אותם דפים צריכים להתנהג שונה לאורח, למשתמש רשום ולמנהלים. <b>פתרון:</b> שמירת תפקיד המשתמש ב-<code>Session</code> בעת ההתחברות (<code>Admin</code>, <code>RestaurantAdmin</code>), ובדיקתו בתחילת כל דף מוגן ובהצגת/הסתרת פקדים.</p>

<h2>1.8 חידושים, התאמות ועדכונים</h2>
<p class="desc">• <b>שילוב Web Service חיצוני</b> אמיתי (ספק הסעות עם מסד נתונים נפרד), כולל הזמנה, עדכון וביטול מסונכרנים.<br/>• <b>לוחות בקרה וסטטיסטיקה</b> עם גרפים שנבנים דינמית, למנהלי מסעדות ולמנהל המערכת.<br/>• <b>בדיקת זמינות בזמן אמת</b> לפי חלון זמן וסוג שולחן.<br/>• <b>טיימר הזמנה</b> של 3 דקות המונע "תפיסת" שעה לזמן בלתי מוגבל.</p>`+

// ---------- 2. ניתוח / תרשימים ----------
`<h1 class="chapter">2. ניתוח מערכת ותרשימים</h1>
<div class="note">התרשימים הבאים הם <b>גרסה ראשונית</b> שנוצרה אוטומטית. מומלץ לצייר אותם מחדש בכלי ייעודי (למשל <b>draw.io</b>) לעיצוב נקי יותר לפני ההגשה.</div>
<h2>2.1 תרשים ישויות-קשרים (ERD)</h2>
`+ svgERD() + `<div class="diagcap">תרשים 1: ישויות המערכת והקשרים ביניהן. הקו המקווקו מסמן קשר חוצה-מסדים אל הספק החיצוני.</div>
<h2>2.2 תרשים זרימת נתונים — רמה 0 (DFD-0)</h2>
`+ svgDFD() + `<div class="diagcap">תרשים 2: זרימת הנתונים בין הגורמים החיצוניים, המערכת, מסד הנתונים וספק ההסעות.</div>
<h2>2.3 תרשים תרחישי שימוש (Use-Case)</h2>
`+ svgUseCase() + `<div class="diagcap">תרשים 3: תרחישי השימוש המרכזיים והשחקנים במערכת.</div>
<h2>2.4 עץ תהליכים</h2>
`+ svgTree() + `<div class="diagcap">תרשים 4: פירוק היררכי של פעולות המערכת.</div>`+

// ---------- 3. בסיס הנתונים ----------
`<h1 class="chapter">3. בסיס הנתונים</h1>
<p class="desc">מסד הנתונים הראשי הוא <code>DBusers1.accdb</code> (Microsoft Access), ובו ארבע טבלאות. הגישה אליו נעשית בעזרת ספק <code>Microsoft.ACE.OLEDB.12.0</code>. (לטבלת ספק ההסעות, <code>Taxis</code>, ראו פרק 4.)</p>

<h2>3.1 טבלת MyUsers — משתמשים</h2>
<p class="desc">מכילה את פרטי כל המשתמשים הרשומים, כולל העדפות התזונה והאלרגיות, והרשאות הניהול.</p>
`+ fieldsTable([
  ['UserID','מספר שלם','מזהה רץ ייחודי של המשתמש (מפתח ראשי)'],
  ['MyFullName','טקסט','שם מלא (משמש גם להתחברות)'],
  ['MyPassword','טקסט','סיסמת המשתמש'],
  ['MyPhoneNumber','טקסט','מספר טלפון — מזהה את הזמנות המשתמש'],
  ['Vegetarian','טקסט','האם צמחוני ("כן"/"לא")'],
  ['Vegan','טקסט','האם טבעוני ("כן"/"לא")'],
  ['Kosher','טקסט','האם שומר כשרות ("כן"/"לא")'],
  ['Gluten','טקסט','אלרגיה לגלוטן ("כן"/"לא")'],
  ['Peanuts','טקסט','אלרגיה לבוטנים ("כן"/"לא")'],
  ['TreeNuts','טקסט','אלרגיה לאגוזי עץ ("כן"/"לא")'],
  ['Fish','טקסט','אלרגיה לדגים ("כן"/"לא")'],
  ['Sesame','טקסט','אלרגיה לשומשום ("כן"/"לא")'],
  ['Milk','טקסט','אלרגיה לחלב ("כן"/"לא")'],
  ['Area','טקסט','אזור מגורים (דרום/מרכז/צפון)'],
  ['Admin','טקסט','"כן" אם המשתמש הוא מנהל מערכת'],
  ['RestaurantAdmin','טקסט','שם המסעדה שהמשתמש מנהל, או ריק אם אינו מנהל מסעדה'],
]) + `

<h2>3.2 טבלת MyRestaurants — מסעדות</h2>
<p class="desc">מכילה את פרטי המסעדות במערכת ואת מספר השולחנות מכל סוג (לצורך בדיקת הזמינות).</p>
`+ fieldsTable([
  ['UserID','מספר שלם','מזהה רץ ייחודי של המסעדה (מפתח ראשי)'],
  ['Restaurants','טקסט','שם המסעדה'],
  ['Region','טקסט','אזור גאוגרפי (דרום/מרכז/צפון)'],
  ['FoodType','טקסט','סוג המטבח (איטלקי, אסייתי וכו\')'],
  ['Kosher','טקסט','האם המסעדה כשרה ("כן"/"לא")'],
  ['ReplacementMeals','טקסט','האם קיימות מנות חלופיות לאלרגיים ("כן"/"לא")'],
  ['SmallTables','טקסט','מספר שולחנות קטנים (עד 2 סועדים)'],
  ['MediumTables','טקסט','מספר שולחנות בינוניים (3–4 סועדים)'],
  ['LargeTables','טקסט','מספר שולחנות גדולים (5+ סועדים)'],
]) + `

<h2>3.3 טבלת MyBooking — הזמנות</h2>
<p class="desc">מכילה שורה לכל הזמנת שולחן. ההזמנות מזוהות לפי מספר הטלפון של המשתמש.</p>
`+ fieldsTable([
  ['UserID','מספר שלם','מזהה רץ ייחודי של ההזמנה (מפתח ראשי)'],
  ['Guest','טקסט','שם המזמין'],
  ['PhoneNum','טקסט','טלפון המזמין — מקשר להזמנות המשתמש'],
  ['Restaurant','טקסט','שם המסעדה שהוזמנה'],
  ['InvDate','תאריך','תאריך ההזמנה'],
  ['InvTime','טקסט','שעת ההזמנה (HH:MM)'],
  ['NumGuest','טקסט','מספר הסועדים'],
  ['TableType','טקסט','סוג השולחן (Small/Medium/Large)'],
  ['Time','טקסט','שדה ישן שאינו בשימוש (נשאר ממבנה קודם של הטבלה)'],
]) + `

<h2>3.4 טבלת Cities — ערים</h2>
<p class="desc">רשימת שמות הערים החוקיות, המשמשת לאימות כתובת האיסוף בעת הזמנת הסעה.</p>
`+ fieldsTable([
  ['CityName','טקסט','שם עיר/יישוב מוכר במערכת'],
]) +

// ---------- 4. שירותי רשת ----------
`<h1 class="chapter">4. שירותי רשת — ספק ההסעות</h1>
<p class="desc">לב חלופת "שירותי רשת" בפרויקט הוא ה<b>ספק החיצוני</b>: שירות הסעות עצמאי, הממומש כפרויקט נפרד עם <b>מסד נתונים משלו</b> (<code>TaxiDB1.accdb</code>) ונגיש דרך <b>Web Service</b> מסוג ASMX (<code>WebService1.asmx</code>). מערכת המסעדות (EatIt) היא ה"לקוח" של השירות, וקוראת לו דרך Connected Service (מחלקת הפרוקסי <code>WebService1SoapClient</code>).</p>
<h2>4.1 תיאור השירות והקשר בין המערכות</h2>
<p class="desc">כאשר לקוח מסיים להזמין שולחן ומעוניין בהסעה, מערכת המסעדות שולחת לספק את שם הלקוח, שם המסעדה, התאריך, השעה והכתובת. הספק רושם את הנסיעה אצלו במסד הנפרד, מקצה מספר נהג, ומחזיר אישור. כאשר הלקוח מעדכן או מבטל את ההזמנה, המערכת קוראת לפעולת הביטול אצל הספק כדי לשמור על סנכרון בין שתי המערכות. הפרדה זו (שני מסדי נתונים, קריאות דרך Web Service) היא שמדמה אינטגרציה אמיתית בין שני ארגונים שונים.</p>
<h2>4.2 הפעולה BookRide — הזמנת הסעה</h2>
<p class="desc">מקבלת שם לקוח, שם מסעדה, תאריך, שעה וכתובת, מכניסה שורה חדשה לטבלת <code>Taxis</code> עם מספר נהג אקראי, ומחזירה הודעת אישור הכוללת את מספר ההזמנה ומספר הנהג.</p>
<h2>4.3 הפעולה CancelRide — ביטול הסעה</h2>
<p class="desc">מקבלת שם לקוח, שם מסעדה, תאריך ושעה, ומוחקת מטבלת <code>Taxis</code> את הנסיעה המתאימה. הזיהוי לפי תאריך + שעה (ולא שעה בלבד) מונע בלבול בין שתי הזמנות לאותה מסעדה ושעה בתאריכים שונים. הפעולה משמשת בעת עדכון או ביטול של הזמנה.</p>
<h2>4.4 קוד שירות הרשת (צד הספק)</h2>`+
codeBlock('Supplier / WebService1.asmx.cs', SUP + 'WebService1.asmx.cs') + `
<h2>4.5 טבלת הספק — Taxis (במסד TaxiDB1.accdb)</h2>
`+ fieldsTable([
  ['RideID','מספר שלם','מזהה רץ ייחודי של הנסיעה (מפתח ראשי)'],
  ['CustomerName','טקסט','שם הלקוח שהזמין'],
  ['RestaurantName','טקסט','שם המסעדה (יעד הנסיעה)'],
  ['RideDate','טקסט','תאריך הנסיעה (yyyy-MM-dd) — נוסף לזיהוי מדויק'],
  ['RideTime','טקסט','שעת האיסוף (HH:MM)'],
  ['DriverNum','מספר שלם','מספר הנהג שהוקצה (אקראי)'],
  ['Adress','טקסט','כתובת האיסוף'],
]) +

// ---------- 5. מסכים ----------
`<h1 class="chapter">5. צד לקוח — מסכי האתר</h1>
<p class="desc">לכל מסך מובאים תיאור קצר, קוד התצוגה (<code>.aspx</code>) וקוד צד השרת (<code>.aspx.cs</code>). <span class="ph">[[מומלץ להוסיף מתחת לכל תיאור צילום מסך של הדף כפי שהוא רץ באתר]]</span></p>`+
screensHtml +

// ---------- 6. רפלקציה ----------
`<h1 class="chapter">6. רפלקציה — סיכום אישי</h1>
<div class="note">החלק הזה צריך להיכתב <b>בלשונך ובמילותיך שלך</b> (מצופה לפחות חצי עמוד עד עמוד). להלן שלד עם נקודות לכתיבה:</div>
<p class="desc"><span class="ph">[[תהליך העבודה: איך התקדמת, מה היה קל ומה היה קשה]]</span></p>
<p class="desc"><span class="ph">[[מה למדת מבחינה טכנית — למשל: עבודה עם Web Service, מסד נתונים, מניעת SQL Injection]]</span></p>
<p class="desc"><span class="ph">[[בעיה אחת שנתקעת בה ואיך פתרת אותה]]</span></p>
<p class="desc"><span class="ph">[[מה היית עושה אחרת או משפר אם היה לך עוד זמן]]</span></p>
<p class="desc"><span class="ph">[[תובנה אישית / תודות]]</span></p>`+

// ---------- 7. ביבליוגרפיה ----------
`<h1 class="chapter">7. ביבליוגרפיה</h1>
<div class="note">לפי הנחיות משרד החינוך יש לרשום מקורות בכללי <b>APA</b>, ולכל קטע קוד שיובא ממקור חיצוני (GitHub / StackOverflow / כלי AI) לציין קישור ציבורי. עדכן/השלם את הרשימה לפי המקורות שהשתמשת בהם בפועל:</div>
<p class="desc">• Microsoft. <i>ASP.NET Web Forms Documentation</i>. <span class="ph">[[קישור]]</span><br/>
• W3Schools. <i>HTML, CSS, SQL Tutorials</i>. https://www.w3schools.com<br/>
• Stack Overflow. <span class="ph">[[קישורים ספציפיים אם השתמשת]]</span><br/>
• <span class="ph">[[מקורות נוספים]]</span></p>`+

// ---------- 8. נספחים ----------
`<h1 class="chapter">8. נספחים</h1>
<h2>נספח א\' — קובץ העיצוב (Site.css)</h2>`+
codeBlock('Site.css', ROOT + 'Site.css') + `
<h2>נספח ב\' — קובץ ההגדרות (Web.config)</h2>`+
codeBlock('Web.config', ROOT + 'Web.config') +

'</body></html>';

const OUT = 'c:\\Users\\Ariel\\Desktop\\customersFinalProject\\תיק_פרויקט_EatIt.html';
fs.writeFileSync(OUT, html, 'utf8');
console.log('Created: ' + OUT);
console.log('Length (chars): ' + html.length);
