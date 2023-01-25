using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

using Microsoft.AspNet.Identity;

namespace freshSite2
{

    public partial class MyAccount : System.Web.UI.Page
    {
        //this counter will be used for the accordians so they all have different id's and dont start opening other fuckers
        int COUNTER = 0;
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

            loadBuying();
            loadSelling();

        }

        // this sets up the accordians for the users books that they are selling
        protected void loadSelling()
        {
            //FOR COMMENTS SEE loadBuying() BELOW, they are the same thing pretty much

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                String command = "SELECT * FROM USERBOOK_T WHERE UserId = '" + Context.User.Identity.GetUserId() + "' and BuySell = 1";
                using (SqlCommand com = new SqlCommand(command, con))
                {
                    try
                    {
                        con.Open();
                        pnlSelling.Controls.Add(new LiteralControl("<div class='panel-group' id='accordion1'>"));

                        using (SqlDataReader sdr = com.ExecuteReader())
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    COUNTER++;

                                    String longText = "<div class='panel panel-default'>"
                                        + "<div class='panel-heading'><h4 class='panel-title img-rounded'><a data-toggle='collapse' "
                                        + "data-parent='#accordion1' href='#collapse" + COUNTER + "'>";
                                    pnlSelling.Controls.Add(new LiteralControl(longText));

                                    Label lt = new Label();
                                    lt.Text = sdr["ISBN"].ToString();
                                    pnlSelling.Controls.Add(lt);

                                    pnlSelling.Controls.Add(new LiteralControl("</a></h4></div><div id='collapse" + COUNTER + "' class='panel-collapse collapse'><div class='panel-body'>"));

                                    Label la = new Label();
                                    la.Text = "MONSTER BLUNTS";
                                    pnlSelling.Controls.Add(la);


                                    pnlSelling.Controls.Add(new LiteralControl("</div></div></div>"));
                                }//end while
                            }
                            else
                            {
                                //should put a search bar here or something
                                String longText = "<div class='panel panel-default'>"
                                        + "<div class='panel-heading'><h4 class='panel-title img-rounded'><a data-toggle='collapse' "
                                        + "data-parent='#accordion' href='#collapse" + ++COUNTER + "'>";
                                pnlSelling.Controls.Add(new LiteralControl(longText));

                                Label lt = new Label();
                                lt.Text = "Go List something, douche";
                                pnlSelling.Controls.Add(lt);

                                pnlSelling.Controls.Add(new LiteralControl("</a></h4></div><div id='collapse" + COUNTER + "' class='panel-collapse collapse'><div class='panel-body'>"));

                                Label la = new Label();
                                la.Text = "bag";
                                pnlSelling.Controls.Add(la);
                                pnlBuying.Controls.Add(new LiteralControl("</div></div></div>"));
                            }

                        pnlSelling.Controls.Add(new LiteralControl("</div>"));
                    }
                    catch (SqlException ee)
                    {
                        Debug.WriteLine("caught here in selling");
                        Debug.WriteLine(ee.Message);
                    }
                }
            }
        }

        // setup the accordians for the books the user is buying 
        protected void loadBuying()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                String command = "SELECT * FROM USERBOOK_T WHERE UserId = '" + Context.User.Identity.GetUserId() + "' and BuySell = 0";
                using (SqlCommand com = new SqlCommand(command, con))
                {
                    try
                    {
                        con.Open();
                        //this starts like the whole accordian thing
                        pnlBuying.Controls.Add(new LiteralControl("<div class='panel-group' id='accordion'>"));

                        using (SqlDataReader sdr = com.ExecuteReader())
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    COUNTER++;
                                    //start the begining of the html for the accordian for each record found in the DB
                                    String longText = "<div class='panel panel-default'>"
                                        + "<div class='panel-heading'><h4 class='panel-title img-rounded'><a data-toggle='collapse' "
                                        + "data-parent='#accordion' href='#collapse" + COUNTER + "'>";
                                    pnlBuying.Controls.Add(new LiteralControl(longText));

                                    //add the information for book in the title of the accordian here
                                    Label lt = new Label();
                                    lt.Text = sdr["ISBN"].ToString();
                                    pnlBuying.Controls.Add(lt);

                                    //end the begining of the accordian here, and start the div for the inside of the accordian
                                    pnlBuying.Controls.Add(new LiteralControl("</a></h4></div><div id='collapse" + COUNTER + "' class='panel-collapse collapse'><div class='panel-body'>"));

                                    //info for the inside here
                                    Label la = new Label();
                                    la.Text = "MONSTER BLUNTS";
                                    pnlBuying.Controls.Add(la);

                                    //end the accordian
                                    pnlBuying.Controls.Add(new LiteralControl("</div></div></div>"));

                                }//end while
                            }

                        //we use the next chunk if there are no records for the user in this area, like if they have
                        // nothing listed to buy or sell or whatever, just at least put something
                            else
                        {
                        //should put as search bar here or something
                            String longText = "<div class='panel panel-default'>"
                                    + "<div class='panel-heading'><h4 class='panel-title img-rounded'><a data-toggle='collapse' "
                                    + "data-parent='#accordion' href='#collapse" + ++COUNTER + "'>";
                            pnlBuying.Controls.Add(new LiteralControl(longText));

                            Label lt = new Label();
                            lt.Text = "Nothing here, go try and buy somethin, this shit aint free bitch";
                            pnlBuying.Controls.Add(lt);

                            pnlBuying.Controls.Add(new LiteralControl("</a></h4></div><div id='collapse" + COUNTER + "' class='panel-collapse collapse'><div class='panel-body'>"));

                            Label la = new Label();
                            la.Text = "Buy something";
                            pnlBuying.Controls.Add(la);


                            pnlBuying.Controls.Add(new LiteralControl("</div></div></div>"));
                        }
                        //this ends like the whole thing or something
                        pnlBuying.Controls.Add(new LiteralControl("</div>"));
                    }
                    catch (SqlException ee) {Debug.WriteLine("caught here in buying : " + ee.Message);}
                }
            }
        }




    }
}