using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace scrapeAfterDelete
{
    public partial class scrape : System.Web.UI.Page
    {
        WebClient webClient = new WebClient();
        int badISBN = 200;
        int badNumber = 616161;
        double badPrice = 123.45;
        String badText = "Not Available";
        Random rand = new Random();
        string campusNum = "58"; // campus code for cal poly pomona
        string url = "http://www.broncobookstore.com/textbooks_xml.asp?";
        string refrr = "http://www.broncobookstore.com/buy_courselisting.asp?";
        List<KeyValuePair<string, string>> bookAttrXpath = new List<KeyValuePair<string, string>>() 
        { 
            new KeyValuePair<string, string>("isbn", "//*[@class=\"isbn\"]"),
            new KeyValuePair<string, string>("Title", "//*[@class=\"book-title\"]"),
            new KeyValuePair<string, string>("Author", "//*[@class=\"book-meta book-author\"]"),
            new KeyValuePair<string, string>("Edition", "//*[@class=\"book-meta book-edition\"]"),
            new KeyValuePair<string, string>("Publisher", "//*[@class=\"book-meta book-publisher\"]"),
            new KeyValuePair<string, string>("Binding", "//*[@class=\"book-meta book-binding\"]"),
            new KeyValuePair<string, string>("Price", "//*[@class=\"book-price-list\"]")
        };

        protected void Page_Load(object sender, EventArgs e){}

        private void getTerms(String tUrl)
        {
            HtmlDocument doc = new HtmlWeb().Load(tUrl);
            foreach (var termNode in doc.DocumentNode.SelectNodes("//*[@name=\"selTerm\"]/option"))
            {
                String termID = termNode.Attributes["value"].Value.Split('|')[1];
                if (!termID.Equals("0") && termID != null)
                {
                    insertDB("TERM_T", new List<String>() { termID, "2014" }); // insert term info into term table
                    Debug.Write("\n**Found Term : " + termNode.InnerText);
                    getDepartments(termID); // run department list
                }
                else Debug.Write("ERROR No Terms Found,skipping");
            }
            Debug.Write("\n\n\nDONE !!!!!!\n\n\n");
        }


        private void getDepartments(String termID)
        {
            actNatural();
            String param = f("control=campus&campus={0}&term={1}&t={2}", campusNum, termID, unxTime());
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(_Get(param));
            foreach (XmlNode deptNode in xml.SelectNodes("/departments/department"))
            {
                String deptID = deptNode.Attributes["id"].Value;
                String deptName = deptNode.Attributes["name"].Value;
                String deptAbrev = deptNode.Attributes["abrev"].Value;
                insertDB("DEPARTMENT_T", new List<String>()
                    {
                        deptID!=null?deptID:""+badNumber++,
                        deptName!=null?deptName:badText,
                        deptAbrev!=null?deptAbrev:badText
                    });
                Debug.Write(f("\n**Found department : {0}\n", deptName));
                insertDB("TERMDEPT_T", new List<string>() { termID, deptID }); // insert the relationship info between term and department
                getCourses(termID, deptID); //run course list for department
            }
        }

        private void getCourses(String termID, String deptID)
        {
            actNatural();
            String param = f("control=department&dept={0}&term={1}&t={2}", deptID, termID, unxTime());
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(_Get(param));
            foreach (XmlNode courseNode in xml.SelectNodes("/courses/course"))
            {
                String courseID = courseNode.Attributes["id"].Value;
                String courseName = courseNode.Attributes["name"].Value;
                insertDB("COURSE_T", new List<String>() 
                { 
                    courseID!=null?courseID:""+badNumber++,
                    courseName!=null?courseName:badText
                }); // insert course information to course table
                Debug.Write(f("*\nFound Course : {0}\n", courseName));
                insertDB("DEPTCOURSE_T", new List<string>() { deptID, courseID }); // insert relationship info for department and course
                getSections(termID, courseID); // run section list
            }
        }
        //
        private void getSections(String termID, String courseID)
        {
            //Func<String, String, String> doVal = (a, b) => a != null ? a : b;
            actNatural();
            String param = f("control=course&course={0}&term={1}&t={2}", courseID, termID, unxTime());
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(_Get(param));
            foreach (XmlNode sectionNode in xml.SelectNodes("/sections/section"))
            {
                
                String sectionID = sectionNode.Attributes["id"].Value;
                String sectionName = sectionNode.Attributes["name"].Value;
                String sectionInst = sectionNode.Attributes["instructor"].Value;
                insertDB("SECTION_T", new List<string>() 
                { 
                    sectionID!=null?sectionID:""+badNumber++,
                    sectionName!=null?sectionName:badText,
                    sectionInst!=null?sectionInst:badText
                }); // insert section info into section table
                Debug.Write(f("\n**Found section : {0}", sectionName));
                insertDB("COURSESECT_T", new List<string>() { courseID, sectionID }); // insert relationship info for course and section
                getBooks(sectionID); // run book list
            }
        }

        private void getBooks(String sectionID)
        {
            actNatural();
            String param = f("control=section&section={0}&t={1}", sectionID, unxTime());
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(f("<html><head></head><body>{0}</body></html>", _Get(param)));
            if (doc.DocumentNode.SelectNodes("//*[@class=\"error\"]") == null)
                for (int i = 1; i <= doc.DocumentNode.SelectNodes("//*[@class=\"book-desc\"]").Count; i++)
                {
                    List<String> bav = new List<String>();
                    foreach (var x in bookAttrXpath)
                    {
                        var g = doc.DocumentNode.SelectSingleNode(f("({0})[{1}]", x.Value, i));
                        bav.Add(g!=null?x.Key=="Price"?g.InnerText.Split('$')[1]:g.InnerText
                                :x.Key=="isbn"?""+badISBN++:x.Key=="Price"?""+badPrice:badText);
                    }
                    insertDB("BOOK_T", bav); // insert book info into book table
                    insertDB("SECTBOOK_T", new List<string>() { sectionID, bav[0] }); // insert relationship between book and section
                }
            else
            {
                insertDB("BOOK_T", new List<String>(){""+badISBN,badText,
                    badText,badText,badText,badText,""+badPrice});
                insertDB("SECTBOOK_T", new List<string>() { sectionID, "" + badISBN++ });
            }
        }

        private void insertDB(String tableName, List<String> param)  // take in the table name, and the values to be inserted
        {
            String v = "@" + 0;
            for (int i = 1; i < param.Count; i++) v += ",@" + i;// add any other table attribute values separated by a comma
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["book_con"].ConnectionString))
                try
                {
                    if (con.State != ConnectionState.Open) con.Open(); // open connection
                    Debug.Write("\nInserting into " + tableName);
                    using (SqlCommand com = new SqlCommand(f("INSERT INTO {0} VALUES({1});", tableName, v), con)) // using this sql command and our connection
                    {
                        for (int i = 0; i < param.Count; i++) com.Parameters.AddWithValue("@" + i, param[i]);
                        com.ExecuteNonQuery(); // execture the statement
                    }
                }
                catch (SqlException e)
                {
                    if (e.Message.Contains("Cannot insert duplicate key"))
                        Debug.Write("\n***Duplicate record\n");
                    else
                    {
                        Debug.Write("other Exception: " + e.Message);
                        Debug.WriteLine("\tTable name " + tableName);
                        //Debug.WriteLine(p.Aggregate((a,b)=>a+="\t"+b));
                        foreach (var a in param)
                            Debug.WriteLine("\tPARAMETER Val : " + a);
                    }
                }
        }

        private String _Get(String param)
        { /*using (var wb = new WebClient())*/return webClient.DownloadString(url + param); }
        private long unxTime()
        { return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds; }
        private void actNatural() { Thread.Sleep(rand.Next(300, 800)); }
        protected void Button1_Click1(object sender, EventArgs e) { getTerms(refrr); }

        //String format 
        private String f(params Object[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
                a[0] =a[0].ToString().Replace("{" + i + "}", a[i + 1].ToString());
            return (String)a[0];
        }
    }
}