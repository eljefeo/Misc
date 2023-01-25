using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace freshSite2.Search
{
    public partial class Sell : System.Web.UI.Page
    {

       
        String userSearch;
        protected void Page_Load(object sender, EventArgs e)
        {
            dropQuality.Items.Add("Shitty");
            dropQuality.Items.Add("Not Shitty, Not Fuckin Sweet");
            dropQuality.Items.Add("Fuckin Sweet");
            
            userSearch = Request.QueryString["search"] ?? "";
            BookImage.ImageUrl = "~/Content/images/books/" + userSearch + ".jpg";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("getBook", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ISBN", userSearch);
                    using (SqlDataReader sdr = com.ExecuteReader())
                        if (sdr.Read())
                        {
                            lblIsbn1.Text = sdr["ISBN"].ToString();
                            lblTitle1.Text = sdr["Title"].ToString();
                            lblAuthor1.Text = sdr["Author"].ToString();
                            lblEdition1.Text = sdr["Edition"].ToString();
                            lblPublisher1.Text = sdr["Publisher"].ToString();
                        }
                }
            }
        }



        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
           
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("ListBook", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UserId", Context.User.Identity.GetUserId());
                    com.Parameters.AddWithValue("@ISBN", lblIsbn1.Text);
                    com.Parameters.AddWithValue("@InitialPrice", tbPrice.Text);
                    com.Parameters.AddWithValue("@Quality", dropQuality.SelectedValue);
                    com.Parameters.AddWithValue("@BuySell", 1);
                    com.Parameters.AddWithValue("@Notes", tbNotes.Text);
                    com.ExecuteNonQuery();
                }
            }
        
        }
    }
}