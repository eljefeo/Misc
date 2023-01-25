using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace freshSite2
{
    public partial class bookDetail : System.Web.UI.Page
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

                //if (userSearch.Length > 2)
                using (SqlCommand com = new SqlCommand("getBook", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ISBN", userSearch);
                     using (SqlDataReader sdr = com.ExecuteReader())
                        if (sdr.Read())
                        {
                            lblisbn.Text = sdr["ISBN"].ToString();
                            lblTitle.Text = sdr["Title"].ToString();
                            lblAuthor.Text = sdr["Author"].ToString();
                            lblEdition.Text = sdr["Edition"].ToString();
                            lblPublisher.Text = sdr["Publisher"].ToString();
                        }
                    }
                }
            }

        

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}