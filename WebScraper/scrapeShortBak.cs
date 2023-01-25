using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Threading;
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
        String campusNum = "58"; // campus code for cal poly pomona
        String url = "http://www.broncobookstore.com/textbooks_xml.asp?";
        String refrr = "http://www.broncobookstore.com/buy_courselisting.asp?";
        String termXPath = "//*[@name=\"selTerm\"]/option";
        String deptXPath = "/departments/department";
        String courseXPath = "/courses/course";
        String sectXPath = "/sections/section";
        String bookXPath = "//*[@class=\"book-desc\"]";
        String bookErrXPath = "//*[@class=\"error\"]";
        String bookHTML = "<html><head></head><body>{0}</body></html>";
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
        Func<Object,String> s = i => ""+i;//smaller toString

        protected void Page_Load(object sender, EventArgs e){}

        private void getTerms(String tUrl)
        {
            HtmlDocument doc = new HtmlWeb().Load(tUrl);
            foreach (var termNode in doc.DocumentNode.SelectNodes(termXPath))
            {
                String termID = termNode.Attributes["value"].Value.Split('|')[1];
                if (!termID.Equals("0") && termID != null)
                {
                    insertDB("TERM_T", new List<String>() { termID, "2014" }); // insert term info into term table
                    Debug.Write("\n**Found Term : " + termNode.InnerText);
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(_Get(f("control=campus&campus={0}&term={1}&t={2}", campusNum, termID, unxTime())));
                    foreach (XmlNode deptNode in xml.SelectNodes(deptXPath))
                    {
                        String deptID = deptNode.Attributes["id"].Value ?? s(badNumber++);
                        String deptName = deptNode.Attributes["name"].Value ?? badText;
                        String deptAbrev = deptNode.Attributes["abrev"].Value ?? badText;
                        insertDB("DEPARTMENT_T", new List<String>() { deptID, deptName, deptAbrev });
                        Debug.Write(f("\n**Found department : {0}\n", deptName));
                        insertDB("TERMDEPT_T", new List<string>() { termID, deptID }); // insert the relationship info between term and department
                        xml.LoadXml(_Get(f("control=department&dept={0}&term={1}&t={2}", deptID, termID, unxTime())));
                        foreach (XmlNode courseNode in xml.SelectNodes(courseXPath))
                        {
                            String courseID = courseNode.Attributes["id"].Value ?? s(badNumber++);
                            String courseName = courseNode.Attributes["name"].Value ?? badText;
                            insertDB("COURSE_T", new List<String>() { courseID, courseName }); // insert course information to course table
                            Debug.Write(f("*\nFound Course : {0}\n", courseName));
                            insertDB("DEPTCOURSE_T", new List<string>() { deptID, courseID }); // insert relationship info for department and course
                            xml.LoadXml(_Get(f("control=course&course={0}&term={1}&t={2}", courseID, termID, unxTime())));
                            foreach (XmlNode sectionNode in xml.SelectNodes(sectXPath))
                            {
                                String sectionID = sectionNode.Attributes["id"].Value ?? s(badNumber++);
                                String sectionName = sectionNode.Attributes["name"].Value ?? badText;
                                String sectionInst = sectionNode.Attributes["instructor"].Value ?? badText;
                                insertDB("SECTION_T", new List<string>() { sectionID, sectionName, sectionInst }); // insert section info into section table
                                Debug.Write(f("\n**Found section : {0}", sectionName));
                                insertDB("COURSESECT_T", new List<string>() { courseID, sectionID }); // insert relationship info for course and section
                                doc.LoadHtml(f(bookHTML, _Get(f("control=section&section={0}&t={1}", sectionID, unxTime()))));
                                var d = doc.DocumentNode.SelectNodes(bookXPath);
                                for (int i = 1; i <= (d == null ? 1 : d.Count); i++)
                                {
                                    List<String> bav = new List<String>();
                                    foreach (var x in bookAttrXpath)
                                    {
                                        var g = doc.DocumentNode.SelectSingleNode(f("({0})[{1}]", x.Value, i));
                                        bav.Add(g != null ? x.Key == "Price" ? g.InnerText.Split('$')[1] : g.InnerText
                                                : x.Key == "isbn" ? s(badISBN++) : x.Key == "Price" ? "" + badPrice : badText);
                                    }
                                    insertDB("BOOK_T", bav); // insert book info into book table
                                    insertDB("SECTBOOK_T", new List<string>() { sectionID, bav[0] }); // insert relationship between book and section
                                }
                            }
                        }
                    }
                } 
                else Debug.Write("ERROR No Terms Found,skipping");
            }
            Debug.Write("\n\n\nDONE !!!!!!\n\n\n");
        }

        private void insertDB(String tableName, List<String> param)  // take in the table name, and the values to be inserted
        {
            String v="@"+0;for(int i=1;i<param.Count;i++)v+=",@"+i;// add any other table attribute values separated by a comma
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["book_con"].ConnectionString))
                try
                {
                    if (con.State != ConnectionState.Open) con.Open(); // open connection
                    Debug.Write(f("\nInserting into {0}\n",tableName));
                    using (SqlCommand com = new SqlCommand(f("INSERT INTO {0} VALUES({1})\n;", tableName, v), con)) // using this sql command and our connection
                    {
                        for (int i = 0; i < param.Count; i++) com.Parameters.AddWithValue("@" + i, param[i]);
                        com.ExecuteNonQuery(); // execture the statement
                    }
                }
                catch (SqlException e)
                {
                    Debug.Write(e.Message.Contains("Cannot insert duplicate key")
                    ?"\n***Duplicate record\n"
                    :f("other exception in{0} {1}",tableName,e.Message));
                }
        }

        private String _Get(String param){actNatural();return webClient.DownloadString(url+param);}
        private long unxTime(){return(long)(DateTime.UtcNow.Subtract(new DateTime(1970,1,1))).TotalMilliseconds;}
        private void actNatural(){Thread.Sleep(rand.Next(300,800));}
        protected void Button1_Click1(object sender,EventArgs e){getTerms(refrr);}
        private String f(params Object[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
                a[0] =a[0].ToString().Replace("{" + i + "}", a[i + 1].ToString());
            return (String)a[0];
        }
    }
}