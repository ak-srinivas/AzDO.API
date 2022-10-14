using AzDO.API.Base.Common.Utilities;
using System;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace AzDO.API.Wrappers.WorkItemTracking.Wiql
{
    public sealed class WiqlCustomWrapper : WiqlWrapper
    {
        public string GenerateHtmlFromTable(DataTable table)
        {
            return Helpers.GenerateHtmlFromTable(table);
        }

        public void SendEmail(string bodyHtml)
        {
            try
            {
                SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.office365.com",
                    Port = 25,
                    UseDefaultCredentials = false,
                    Timeout = 100000
                };

                MailAddress fromMail = new MailAddress("srinivas.akkapeddi@neudesic.com");
                MailAddress toMail = new MailAddress("navya.saggam@neudesic.com");
                MailMessage message = new MailMessage
                {
                    From = fromMail,
                    Subject = "Test Email",
                    IsBodyHtml = true,
                    Body = "Test Message"
                };

                message.To.Add(toMail);
                //message.CC.Add(to);
                //message.Bcc.Add(to);

                client.Send(message);
                client.Dispose();
                message.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to send email");
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine(
                    "\nHelpLink ---\n{0}", ex.HelpLink);
                Console.WriteLine("\nSource ---\n{0}", ex.Source);
                Console.WriteLine(
                    "\nStackTrace ---\n{0}", ex.StackTrace);
                Console.WriteLine(
                    "\nTargetSite ---\n{0}", ex.TargetSite);
            }

        }
    }
}
