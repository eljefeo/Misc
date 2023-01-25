using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace freshSite2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       

        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

                  sb.Append(@"<script type='text/javascript'>");

                  sb.Append("$('#currentdetail').modal('show');");

                  sb.Append(@"</script>");

                  ScriptManager.RegisterClientScriptBlock(this, this.GetType(),

                               "ModalScript", sb.ToString(), false);

        }

    }
}