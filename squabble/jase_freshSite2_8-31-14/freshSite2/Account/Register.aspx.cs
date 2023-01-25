using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using freshSite2.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace freshSite2.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
           

            if (tbEmail.Text.Contains("@csupomona.edu") && tbEmail.Text.Length > 15)
            {// check email for unique
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    con.Open();
                    String email = tbEmail.Text;
                    bool vEmail = true;
                    using (SqlCommand com = new SqlCommand("select Email from USERDETAIL_T", con))
                    {
                        using (SqlDataReader sdr = com.ExecuteReader())
                            while (sdr.Read())
                                if(email.Equals(sdr["Email"].ToString()))
                                    vEmail=false;
                    }

                    if(vEmail)
                    {
                        if(Regex.IsMatch(tbFirstName.Text, "^[a-zA-Z]+$") || tbFirstName.Text.Equals(""))
                        {
                            if(Regex.IsMatch(tbLastName.Text, "^[a-zA-Z]+$") || tbLastName.Text.Equals(""))
                            {
                                var manager = new UserManager();
                                var user = new ApplicationUser() { UserName = UserName.Text };
                                IdentityResult result = manager.Create(user, Password.Text);
                                if (result.Succeeded)
                                {
                                    IdentityHelper.SignIn(manager, user, isPersistent: false);

                                        String id = "";
                                        using (SqlCommand com = new SqlCommand("getId", con))
                                        {
                                            com.CommandType = CommandType.StoredProcedure;
                                            com.Parameters.AddWithValue("@UserName", UserName.Text);
                                            using (SqlDataReader sdr = com.ExecuteReader())
                                                if (sdr.Read())
                                                    id = sdr["Id"].ToString();
                                        }

                                        using (SqlCommand com = new SqlCommand("NewUserDetail", con))
                                        {
                                            Debug.WriteLine("ID " + Context.User.Identity.GetUserId());
                                            com.CommandType = CommandType.StoredProcedure;
                                            com.Parameters.AddWithValue("@UserId", id);
                                            com.Parameters.AddWithValue("@Email", tbEmail.Text);
                                            com.Parameters.AddWithValue("@FirstName", tbFirstName.Text);
                                            com.Parameters.AddWithValue("@LastName", tbLastName.Text);
                                            com.ExecuteNonQuery();
                                        }
                                   
                                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                                }
                                else
                                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                            }
                            else 
                                ErrorMessage.Text = "Invalid Last Name";
                        }
                        else
                           ErrorMessage.Text = "Invalid First Name";
                    }
                    else
                        ErrorMessage.Text = "That Email is already in use";
                }
        }
            else
            ErrorMessage.Text = "Email must be a Cal Poly Email";
        }


        private bool isValidEmail(String email)
        {
            return false;
        }
    }
}


/*

protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = new UserManager();
            var user = new ApplicationUser() { UserName = UserName.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

*/