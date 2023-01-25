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
    public partial class Buy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String userSearch = Request.QueryString["search"] ?? "";
            Debug.WriteLine(userSearch);
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();

                //if (userSearch.Length > 2)
                using (SqlCommand com = new SqlCommand("bookDetail", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ISBN", userSearch);
                    using (SqlDataAdapter da = new SqlDataAdapter(com))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        Repeater1.DataSource = dt;
                        Repeater1.DataBind();
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();
                BookImage.ImageUrl = "~/Content/images/books/" + userSearch + ".jpg";

                //if (userSearch.Length > 2)
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
    }
}