using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace freshSite2
{
    public class typeAhead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string prefixText = context.Request.QueryString["q"];

            context.Response.ContentType = "application/json";

            List<string> countries = new List<string>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string cmdText = "typeahead"; // change to add with value parameter
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@search", prefixText);
                    try
                    {
                        con.Open();
                        List<string> obj = new List<string>();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                /*try
                                {
                                    isb = Convert.ToInt64(dr["ISBN"].ToString().Trim());
                                }
                                catch (Exception e)
                                {

                                }
                                title = dr["Title"].ToString().Trim();
                                if (isb > 5000 || isb == 0)
                                    rowString = isb + "\n" + title;
                                else 
                                    rowString = title;

                                obj.Add(rowString);*/
                                obj.Add(dr["Title"].ToString().Trim());
                            }
                        }

                        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                        context.Response.Write(jsSerializer.Serialize(obj));
                    }
                    catch(Exception e){}
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}