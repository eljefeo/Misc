using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace freshSite2.Search
{
    
    public partial class SearchBooks : System.Web.UI.Page
    {
       
        TextBox searchBoxDefault1 = new TextBox();
        Button searchButtonDefault1;
        Button buy, sell;
        String userSearch = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //buy = new Button();
            //sell = new Button();
            //searchButtonDefault1 = new Button();
            //searchButtonDefault1.Click += new System.EventHandler(sendSearchDefault1);
            
            
            //searchBoxDefault1

            
          // if (!Page.IsPostBack)
            {
                

                userSearch = Request.QueryString["search"] ?? "";
               
               // if (String.IsNullOrWhiteSpace(userSearch))
                //{
                   // Label lab = new Label();
                    //lab.Width = 500;
                    //lab.Text = "Use the search box in the top right to find a book to buy or sell.";
                    //ph1.Controls.Add(lab);


                   /* TextBox tb = new TextBox();
                    tb.ID = "searchBoxDefault1";
                    tb.CssClass = "twitter-typeahead tt-query";
                    tb.Width = 300;
                    ph1.Controls.Add(tb);

                    searchButtonDefault1.Text = "Search";
                    searchButtonDefault1.ID = "searchButtonDefault1";
                    searchButtonDefault1.CssClass = "btn btn-success";
                    searchButtonDefault1.OnClientClick = "this.disabled = true; this.text = Searching...";
                    ph1.Controls.Add(searchButtonDefault1);*/

                }

                //else
                   doSearch(userSearch);

           // }
            //else
            {
            }
            

        }

        protected void showQuestion()
        {
            Label lab = new Label();
            lab.Text = "First, are you trying to:";

            sell.CssClass = "btn btn-success";
            sell.Text = "Sell a Book";
            sell.ID = "btnSell";
           
            sell.OnClientClick="this.disabled = true; this.value = 'Submitting...';";
            //sell.UseSubmitBehavior=false  ;
            buy.CssClass = "btn btn-success";
            //buy.OnClientClick = "goToBuy()";
            
            buy.Text = "Sell a Book";
            buy.ID = "btnSell";
            
 
        ph1.Controls.Add(new LiteralControl("<div class=\"jumbotron\"><div class=\"span12\">First, are you trying to:</div><div class=\"span12\">"));

        ph1.Controls.Add(sell);
        ph1.Controls.Add(new LiteralControl("</div><div class=\"span\">"));
   
            //ph1.Controls.Add(new LiteralControl("<Button runat=\"server\" Id=\"btnBuy\"  pnclick=\"goToBuy\" Text=\"Sell a book\" />"));
            
           // ph1.Controls.Add(lab);
           
        ph1.Controls.Add(buy);
        ph1.Controls.Add(new LiteralControl("</div></div>"));

        }

        protected void goToSell(object sender, EventArgs e)
        {

            ph1.Controls.Clear();

            ph1.Controls.Add(new LiteralControl(" YAY "));

           /* Button clickedButton = (Button)sender;
            clickedButton.Text = "...button clicked...";
            clickedButton.Enabled = false;

            Debug.WriteLine("gotosell");
            ph1.Controls.Clear();
            decision = true;
            //ph1.Controls.Add(new LiteralControl("<div class=\"jumbotron\"><div class=\"span12\">Great! To begin selling a bookwe first need to find it in the system.<br />Search below for the book you would like to sell.</div><div class=\"span12\">"));
            //String a = "<div class=\"input-group\"><asp:TextBox ID=\"searchBoxDefault1\" CssClass=\"twitter-typeahead tt-query\" PlaceHolder=\"Search by title or ISBN !\" runat=\"server\" Text=\"\" Width=\"300px\"></asp:TextBox><span class=\"input-group-btn\"><asp:Button ID=\"searchButtonDefault\" class=\"btn btn-success\" style=\"background-color: #FFE0B2; border-color: #000; color: #000;margin-left:5px;  margin-top: -3px; margin-bottom: 8px\" onclick =\"sendSearchDefault\" runat =\"server\" Text=\"Search Books\" /></span></div>";
            TextBox tb = new TextBox();
            tb.ID = "searchBoxDefault1";
            tb.CssClass = "twitter-typeahead tt-query";
            tb.Width = 300;
            ph1.Controls.Add(tb);
            // ID=\"searchButtonDefault\" class=\"btn btn-success\" style=\"background-color: #FFE0B2; border-color: #000; color: #000;margin-left:5px;  margin-top: -3px; margin-bottom: 8px\" onclick =\"sendSearchDefault\" runat =\"server\" Text=\"Search Books\" /></span></div>";
            searchBoxDefault1.ID = "searchButtonDefault1";
            searchBoxDefault1.CssClass = "btn btn-success";
            searchBoxDefault1.OnClientClick = "sendSearchDefault1";
            //ph1.Controls.Add(tb);
            ph1.Controls.Add(searchBoxDefault1);*/

        }
        protected void doSearch(string userSearch)
        {
            
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();
              
                if (userSearch.Length > 2)
                    using (SqlCommand com = new SqlCommand("searchWithAvailability", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@userSearch1", userSearch);
                    com.Parameters.AddWithValue("@userSearch2", userSearch);
                    using(SqlDataAdapter da = new SqlDataAdapter(com))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        Repeater1.DataSource = dt;
                        Repeater1.DataBind();
                    }
                }
            }
        }


        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //doSearch(e.CommandName);


            //ph1.Controls.Add(new LiteralControl("<div class=\"container\"><div class=\"row\"><div class=\"col-sm-4\">"));
            
            String[] urr = e.CommandName.Split('.');
            Response.Redirect(!urr[1].Equals("0") ? ("~/Search/Sell?search=" + urr[0]) : ("~/Search/Buy?search=" + urr[0]), false);
            return;


        }

        String isbnValue = "";

        protected String isbnReturn(String v)
        {
           
            isbnValue = v;
            return Int64.Parse(v) > 5000 ? v : "N/A";
        }

        protected String getIsbn(String v)
        {
            return isbnValue+"."+v;
        }

        protected String getIsbn()
        {
            return isbnValue+".d";
        }

        protected bool setVisible (String v)
        {
            return !String.IsNullOrEmpty(v);
        }

        protected void sendSearchDefault1(object sender, EventArgs e)
        {
            //search2 = searchBoxDefault1.Text;
            //Response.Redirect("~/Search/SearchBooks?search=" + searchBoxDefault1.Text + false);
            //return;
        }
    }
}