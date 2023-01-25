using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace freshSite2.Email
{
    public partial class email : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sendEmail();
        }



        protected void sendEmail()
        {
            /*SmtpClient client = new SmtpClient("mail.doghorn.arvixe.com", 465)
            {
                Credentials = new NetworkCredential("squabble@squabblebooks.com", "Fruitybooty1"),
                EnableSsl = true
            };


            MailAddress from = new MailAddress(@"squabble@squabblebooks.com", "Squabble Auto Mail");
            MailAddress to = new MailAddress(@"jeffnavabiz@yahoo.com", "Jeff the great");
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            // set subject and encoding
            myMail.Subject = "111";
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            myMail.Body = "Hi test ";

            myMail.BodyEncoding = System.Text.Encoding.UTF8;

            myMail.IsBodyHtml = true;

            client.Send(myMail);
            */



            string senderEmail = "squabble@squabblebooks.com";
            NetworkCredential mailAuthentication = new NetworkCredential(senderEmail, "Fruitybooty1");
            SmtpClient mailClient = new SmtpClient("mail.doghorn.arvixe.com", 25);
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = mailAuthentication;
            //to user
            MailMessage MyMailMessage;
            MyMailMessage = new MailMessage(senderEmail, "jeffnavabiz@yahoo.com", "SUBJECT", "MESSAGE");
            MyMailMessage.IsBodyHtml = false;
            mailClient.Send(MyMailMessage);


           /* System.Net.Mail.MailMessage eMail = new System.Net.Mail.MailMessage();
            eMail.IsBodyHtml = true;
            eMail.Body = "Test body";
            eMail.From = new System.Net.Mail.MailAddress(@"squabble@squabblebooks.com");
            eMail.To.Add(@"jeffnavabiz@yahoo.com");
            eMail.Subject = "TEST SUB";
            System.Net.Mail.SmtpClient SMTP = new System.Net.Mail.SmtpClient();*/

            //SmtpClient client1 = new SmtpClient("mail.squabblebooks.com", 465)
            //{
             //   Credentials = new NetworkCredential("squabble", "Fruitybooty1"),
              //  EnableSsl = true
            //};


           // SMTP.Credentials = new System.Net.NetworkCredential("squabble", "Fruitybooty1");
            //SMTP.Host = "mail.squabblebooks.com";
            //SMTP.Send(eMail);
        }
    }
}