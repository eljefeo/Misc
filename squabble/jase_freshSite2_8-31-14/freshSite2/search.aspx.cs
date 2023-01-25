using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using freshSite2.Models;

namespace freshSite2
{
    public partial class search : Page
    {

        SqlDataAdapter adapter;
        DataTable univs = new DataTable();
        DataTable terms = new DataTable();
        DataTable depts = new DataTable();
        DataTable crs = new DataTable();
        DataTable sects = new DataTable();

        //DataTable teach = new DataTable();
        DataTable bks = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            string userSearch = Request.QueryString["search"] ?? "nothing";
            //Button btn = new Button();
           // btn.Command += new CommandEventHandler(this.doList);

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                if (userSearch != "")
                    using (SqlCommand com = new SqlCommand("SELECT * FROM BOOK_T WHERE Title LIKE '%" + userSearch + "%'", con))
                    {
                        try
                        {
                            con.Open();

                            Label ltt = new Label();
                            Label laa = new Label();
                            Label lii = new Label();
                            Label lee = new Label();
                            Label lpp = new Label();

                            pnlResults.Controls.Add(new LiteralControl(" <div class=\"col-md-10\">"));

                            ltt.Text = "Title";
                            laa.Text = "Author";
                            lii.Text = "ISBN";
                            lee.Text = "Edition";
                            lpp.Text = "Publisher";

                            pnlResults.Controls.Add(new LiteralControl("<b> <div class=\"col-md-2\">"));
                            pnlResults.Controls.Add(ltt);
                            pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                            pnlResults.Controls.Add(laa);
                            pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                            pnlResults.Controls.Add(lii);
                            pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                            pnlResults.Controls.Add(lee);
                            pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                            pnlResults.Controls.Add(lpp);
                            pnlResults.Controls.Add(new LiteralControl(" </b></div>"));


                            pnlResults.Controls.Add(new LiteralControl(" </div>"));

                            pnlResults.Controls.Add(new LiteralControl("<br /><br /><br /><br />"));

                            int counter = 0;
                            using (SqlDataReader sdr = com.ExecuteReader())
                                while (sdr.Read())
                                {
                                    counter++;
                                    Label lt = new Label();
                                    Label la = new Label();
                                    Label li = new Label();
                                    Label le = new Label();
                                    Label lp = new Label();
                                    Button btn = new Button
                                    {
                                        ID = "listBook" + counter,
                                        CommandArgument = sdr["ISBN"].ToString(),
                                        Text = "LIST THIs MOTHERFUCKER"
                                    };
                                    btn.Attributes.Add("runat", "server");
                                    btn.Click += new System.EventHandler(listBook);


                                    lt.Text = sdr["Title"].ToString();
                                    la.Text = sdr["Author"].ToString();
                                    li.Text = sdr["ISBN"].ToString();
                                    le.Text = sdr["Edition"].ToString();
                                    lp.Text = sdr["Publisher"].ToString();



                                    pnlResults.Controls.Add(new LiteralControl(" <div class=\"col-md-12\"><div class=\"col-md-2\">"));
                                    pnlResults.Controls.Add(lt);
                                    pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                                    pnlResults.Controls.Add(la);
                                    pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                                    pnlResults.Controls.Add(li);
                                    pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                                    pnlResults.Controls.Add(le);
                                    pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                                    pnlResults.Controls.Add(lp);
                                    pnlResults.Controls.Add(new LiteralControl(" </div> <div class=\"col-md-2\">"));
                                    pnlResults.Controls.Add(btn);
                                    pnlResults.Controls.Add(new LiteralControl(" </div>"));


                                    pnlResults.Controls.Add(new LiteralControl(" </div>"));
                                    pnlResults.Controls.Add(new LiteralControl("<br /><br /><br />"));

                                }


                        }
                        catch (SqlException ee)
                        {
                            Debug.WriteLine("caught here");
                            Debug.WriteLine(ee.Message);
                        }


                        //    data.Add((int)sdr["ID"], (string)sdr["Name"], (string)sdr["Email"], (string)sdr["PhoneNo"]);

                    }


            }

            if (!IsPostBack)
            {
               

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {

                  


                    try
                    {
                        adapter = new SqlDataAdapter("SELECT UniversityID, UniversityName FROM UNIVERSITY_T", con);
                        adapter.Fill(univs);
                        UniversityList.DataSource = univs;
                        UniversityList.DataTextField = "UniversityName";
                        UniversityList.DataValueField = "UniversityID";
                        UniversityList.DataBind();

                        /*
                        
                         */
                    }
                    catch (SqlException sqlE)
                    {

                    }

                    loadResults();

                }

                UniversityList.Items.Insert(0, new ListItem("<Select University>", "0"));
                TermList.Items.Insert(0, new ListItem("<Select Term>", "0"));
                DepartmentList.Items.Insert(0, new ListItem("<Select Department>", "0"));
                CourseList.Items.Insert(0, new ListItem("<Select Course>", "0"));
                SectionList.Items.Insert(0, new ListItem("<Select Section>", "0"));
                BookList.Items.Insert(0, new ListItem("<Select Book>", "0"));
            }

        }

       

        protected void UniversityList_SelectedIndexChanged(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                if (UniversityList.SelectedIndex > 0)
                {
                    TermList.Enabled = true;
                    DepartmentList.Enabled = true;

                    String com = String.Format("select d.DeptID,d.DeptName" +
                                    " from DEPARTMENT_T d, UNIVERSITY_T u, UNIVDEPT_T ud" +
                                    " where ud.UniversityID = {0}" +
                                    " and ud.DeptID = d.DeptID", UniversityList.SelectedValue);

                    adapter = new SqlDataAdapter(com, con);
                    adapter.Fill(depts);
                    DepartmentList.DataSource = depts;
                    DepartmentList.DataTextField = "DeptName";
                    DepartmentList.DataValueField = "DeptID";
                    DepartmentList.DataBind();

                    adapter = new SqlDataAdapter("select TermID,TermDescription from TERM_T", con);
                    adapter.Fill(terms);
                    TermList.DataSource = terms;
                    TermList.DataTextField = "TermDescription";
                    TermList.DataValueField = "TermID";
                    TermList.DataBind();


                }
            }
        }

        protected void TermList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                if (UniversityList.SelectedIndex > 0)
                {
                    CourseList.Enabled = true;
                    String com = String.Format("SELECT c.CourseID, c.CourseName FROM COURSE_T c ,DEPTCOURSE_T dc" +
                                " where c.CourseID = dc.CourseID and dc.DeptID = {0}", DepartmentList.SelectedValue);
                    adapter = new SqlDataAdapter(com, con);
                    adapter.Fill(crs);
                    CourseList.DataSource = crs;
                    CourseList.DataTextField = "CourseName";
                    CourseList.DataValueField = "CourseID";
                    CourseList.DataBind();
                }

                else CourseList.Enabled = false;

            }

        }

        protected void CourseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                if (UniversityList.SelectedIndex > 0)
                {
                    SectionList.Enabled = true;
                    String com = String.Format("SELECT s.SectionID, s.SectionName FROM SECTION_T s ,COURSESECT_T cs" +
                                " where s.SectionID = cs.SectionID and cs.CourseID = {0}", CourseList.SelectedValue);
                    adapter = new SqlDataAdapter(com, con);
                    adapter.Fill(sects);
                    SectionList.DataSource = sects;
                    SectionList.DataTextField = "SectionName";
                    SectionList.DataValueField = "SectionID";
                    SectionList.DataBind();
                }

                else SectionList.Enabled = false;

            }
        }

        protected void SectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BookList.Items.Clear();

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                if (UniversityList.SelectedIndex > 0)
                {
                    BookList.Enabled = true;
                    String com = String.Format("SELECT b.ISBN, b.Title FROM BOOK_T b , SECTBOOK_T sb" +
                                " where b.ISBN = sb.ISBN and sb.SectionID = {0}", SectionList.SelectedValue);
                    adapter = new SqlDataAdapter(com, con);
                    adapter.Fill(bks);
                    BookList.DataSource = bks;
                    BookList.DataTextField = "Title";
                    BookList.DataValueField = "ISBN";
                    BookList.DataBind();

                }

                else BookList.Enabled = false;

            }
        }


        protected void BookList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void loadResults()
        {

        }


        protected void doList(object sender, EventArgs e)
        {
            Button btnn = (Button)sender;
            String arg = btnn.CommandArgument;
            Debug.WriteLine("got to listed " + arg);
        }

        protected void listBook(object sender, EventArgs e)
        {
            Button btnn = (Button)sender;
            String userId = Context.User.Identity.GetUserId();
            String isbnn = btnn.CommandArgument;
            Debug.WriteLine("got to listed " + userId + " " + isbnn);

           using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

               using (SqlCommand com = new SqlCommand("INSERT  INTO USERBOOK_T(UserId,ISBN,BuySell) VALUES('"+userId+"','"+isbnn+"',1)", con))
                    {
                        con.Open();
                      com.ExecuteNonQuery();
                    }
            }

            
        }


         protected void doNext(object sender, EventArgs e)
        {

        }



    }
}