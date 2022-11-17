using AzDO.API.Base.Common.Utilities;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace AzDO.API.Wrappers.WorkItemTracking.Wiql
{
    public sealed class WiqlCustomWrapper : WiqlWrapper
    {
        /// <summary>
        /// Finds the test suite by it's name and returns the TestSuite object
        /// </summary>
        //public TestSuite GetWiqlQueryResults(string project, int planId, string testSuiteName)
        //{
        //    string continuationToken = null;
        //    TestSuite testSuite = null;

        //    do
        //    {
        //        PagedList<TestSuite> testSuites = QueryByWiql(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Wiql wiql, bool ? timePrecision = null, int ? top = null)
        //        continuationToken = testSuites.ContinuationToken;
        //        testSuite = testSuites.Where(item => item.Name.Equals(testSuiteName)).Select(item => item).FirstOrDefault();
        //    } while (testSuite == null && continuationToken != null);

        //    return testSuite;
        //}

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
