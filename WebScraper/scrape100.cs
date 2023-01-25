using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Xml;
using System.Linq;

namespace scrapeAfterDelete
{
    public partial class scrape : System.Web.UI.Page
    {
        WebClient webClient = new WebClient(); // does get requests
        int badISBN = 200;// use these bad values when missing
        int badNumber = 616161;
        double badPrice = 123.45;
        Random rand = new Random();
        String bookHTML = "<html><head></head><body>{0}</body></html>";//use this to format incomplete html
        Dictionary<String, String> bookAtt_XPath = new Dictionary<String, String>()//xpaths to our book attributes
        {     {"i","//*[@class=\"isbn\"]"},{"t","//*[@class=\"book-title\"]"},
              {"a","//*[@class=\"book-meta book-author\"]"},{"e","//*[@class=\"book-meta book-edition\"]"},
              {"p","//*[@class=\"book-meta book-publisher\"]"},{"b","//*[@class=\"book-meta book-binding\"]"},
              {"r","//*[@class=\"book-price-list\"]"}};

        protected void Page_Load(object sender, EventArgs e)  // fuck it, put it in the page load
        {
            Func<Object, String> s = i => "" + i;//little ToString
            HtmlDocument doc = new HtmlWeb().Load("http://www.broncobookstore.com/buy_courselisting.asp?");
            XmlDocument xml = new XmlDocument(); //department, course, and section info come as xml, term and book are html
            foreach (var termNode in doc.DocumentNode.SelectNodes("//*[@name=\"selTerm\"]/option"))
            {
                String termID = termNode.Attributes["value"].Value.Split('|')[1] ?? s(0); // term looks like this  265|"someothershit"
                if (!termID.Equals("0") && termID != null)//they put the first entry as 0|0 for whateverTheFuck reason
                {
                    insertDB("TERM_T", new List<String>() { termID, "2014" }); // insert term info into term table
                    xml.LoadXml(_Get(f("control=campus&campus=58&term={0}&t={1}", termID, unxTime()))); // 58 is cal poly campus code
                    foreach (XmlNode deptNode in xml.SelectNodes("/departments/department"))//cycle through departements
                    {
                        List<String>deptAtts=getAtts(new List<string>{"id","name","abrev"},s(++badNumber),deptNode);//get attributes
                        insertDB("DEPARTMENT_T", new List<String>() { deptAtts[0], deptAtts[1], deptAtts[2] });//insert solo first
                        insertDB("TERMDEPT_T", new List<string>() { termID, deptAtts[0] });//insert relationship between entities 
                        xml.LoadXml(_Get(f("control=department&dept={0}&term={1}&t={2}", deptAtts[0], termID, unxTime())));//load up courses
                        foreach (XmlNode courseNode in xml.SelectNodes("/courses/course"))//cycle through courses
                        {
                            List<String> courseAtts = getAtts(new List<string> { "id", "name" }, s(++badNumber), courseNode);//attributes
                            insertDB("COURSE_T", new List<String>() { courseAtts[0], courseAtts[1] });//solo table
                            insertDB("DEPTCOURSE_T", new List<string>() { deptAtts[0], courseAtts[0] });//relationship table
                            xml.LoadXml(_Get(f("control=course&course={0}&term={1}&t={2}", courseAtts[0], termID, unxTime())));//load sect
                            foreach (XmlNode sectionNode in xml.SelectNodes("/sections/section"))//cycle through sections
                            {
                                List<String> secAtts = getAtts(new List<string> { "id", "name", "instructor" }, s(++badNumber), sectionNode);
                                insertDB("SECTION_T", new List<string>() { secAtts[0], secAtts[1], secAtts[2] });//solo
                                insertDB("COURSESECT_T", new List<string>() { courseAtts[0], secAtts[0] });//relation
                                doc.LoadHtml(f(bookHTML, _Get(f("control=section&section={0}&t={1}", secAtts[0], unxTime()))));//load page
                                var d = doc.DocumentNode.SelectNodes("//*[@class=\"book-desc\"]");//grab all books on page
                                for (int i = 1; i <= (d == null ? 1 : d.Count); i++) // cycle through each book on page
                                {
                                    ++badNumber;//use for random bad data
                                    List<String> bav = new List<String>(); // bav = book attribute value
                                    foreach (var x in bookAtt_XPath)//cycle through xpaths to the book attibutes 
                                    {
                                        var g = doc.DocumentNode.SelectSingleNode(f("({0})[{1}]", x.Value, i));//arrays of each attribute node
                                        bav.Add(g != null ? x.Key == "Price" ? g.InnerText.Split('$')[1] : g.InnerText//gnarly ternary 
                                               : x.Key == "isbn" ? s(badISBN++) : x.Key == "Price" ? "" + badPrice : s(badNumber));
                                    }
                                    insertDB("BOOK_T", bav); // insert book info into book table
                                    insertDB("SECTBOOK_T", new List<string>() { secAtts[0], bav[0] }); // insert relationship between book and section
                }}}}}//close loops and ifs
                else Debug.Write("Term error....skipping");//they put the first value as blank, or zero, fucking assholes
            }
            Debug.Write("\n\n\nDONE !!!!!!\n\n\n"); //yep, done
        }
        private void insertDB(String tableName, List<String> param)// take in the table name, and the values to be inserted
        {
            String v="@"+0;for(int i=1;i<param.Count;i++)v+=",@"+i;// add table attribute values separated by a comma
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["book_con"].ConnectionString))
                try
                {
                    con.Open(); // you guessed it,
                    foreach (String aa in param) Debug.WriteLine("Inserting into "+tableName+" : \t"+aa); //show on screen whats up
                    using (SqlCommand com = new SqlCommand(f("INSERT INTO {0} VALUES({1})\n;", tableName, v), con)) // sql command,connection
                    {
                        for(int i=0;i<param.Count;i++)com.Parameters.AddWithValue("@"+i,param[i]); //parameterized query
                        com.ExecuteNonQuery(); // execute the insert statement
                    }
                }
                catch (SqlException e)// duplicate entry
                {Debug.Write(e.Message.Contains("Cannot insert duplicate key")?"\n***Duplicate entry\n":f("***other : "+tableName));}
        }
        private List<String>getAtts(List<String> a,String b,XmlNode c)//take string parameters, find values in xmlnode attributes, if null use badnumber
        {List<String>f=new List<string>();a.ForEach(x=>f.Add(c.Attributes[x].Value??b));return f;}
        private String _Get(String p){actNatural();return webClient.DownloadString("http://www.broncobookstore.com/textbooks_xml.asp?"+p);}//get
        private long unxTime() { return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds; }//unix time ftw thank you 1970
        private void actNatural() { Thread.Sleep(rand.Next(300, 800)); }//chill for a sec, dont piss off the server, cause this shit looks like they got c-grade CS students to make it
        private String f(params Object[]a)// tired of writing String.Format(blahblahbla...causeImLazyAsFuck.. now just f(....
        {for(int i=0;i<a.Length-1;i++)a[0]=((String)a[0]).Replace("{"+i+"}",a[i+1].ToString());return (String)a[0];}
    }
}