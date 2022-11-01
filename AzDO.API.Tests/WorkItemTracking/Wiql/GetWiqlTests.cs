using AzDO.API.Base.Common.Extensions;
using AzDO.API.Wrappers.WorkItemTracking.Wiql;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using static AzDO.API.Base.Common.WrappersBase;

namespace AzDO.API.Tests.WorkItemTracking.Wiql
{
    [TestClass]
    public class GetWiqlTests : TestBase
    {
        private readonly WiqlCustomWrapper _wiqlCustomWrapper;
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;

        public GetWiqlTests()
        {
            _wiqlCustomWrapper = new WiqlCustomWrapper();
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
        }

        [TestMethod]
        public void SendEmail()
        {
            try
            {
                SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.office365.com",
                    Port = 25,
                    //UseDefaultCredentials = false,
                    Timeout = 900000
                };

                MailAddress fromMail = new MailAddress("srinivas.akkapeddi@neudesic.com");
                MailAddress toMail = new MailAddress("srinivas.akkapeddi@neudesic.com");
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

        [TestMethod]
        public void GetWorkItemTitles()
        {
            string filePath = @"D:\titles.txt";
            File.Delete(filePath);
            for (int startId = 126556; startId <= 126565; startId++)
            {
                using (StreamWriter swriter = new StreamWriter(filePath, true))
                {
                    WorkItem workItem = _workItemsCustomWrapper.GetWorkItem(startId);
                    swriter.WriteLine(workItem.Fields[FieldNames.SystemTitle]);
                }
            }
            Console.WriteLine();
        }

        [TestMethod]
        public void QueryById_GenerateTable_SendEmail()
        {
            string queryId = "1e6c6762-53f9-4254-8685-ff750c4f67be";
            string targetFilePath = "D:\\Test.csv";
            DataTable resultTable = new DataTable();

            Guid id = new Guid(queryId);
            WorkItemQueryResult workItemQueryResult = _wiqlCustomWrapper.QueryById(id);

            if (workItemQueryResult != null && workItemQueryResult.WorkItems != null)
            {
                // Key -> Name, Value -> Reference Name
                Dictionary<string, string> columnsInfo = workItemQueryResult.Columns.ToDictionary(item => item.Name, item => item.ReferenceName);

                foreach (string columnName in columnsInfo.Keys)
                {
                    resultTable.Columns.Add(columnName);
                }

                foreach (WorkItemReference item in workItemQueryResult.WorkItems)
                {
                    WorkItem workItem = _workItemsCustomWrapper.GetWorkItem(item.Id);
                    DataRow newRow = resultTable.NewRow();

                    foreach (WorkItemFieldReference itemColumn in workItemQueryResult.Columns)
                    {
                        if (itemColumn.ReferenceName.Equals("System.AssignedTo"))
                        {
                            IdentityRef test = (IdentityRef)workItem.Fields[itemColumn.ReferenceName];
                            newRow[itemColumn.Name] = test.DisplayName;
                        }
                        else
                            newRow[itemColumn.Name] = workItem.Fields[itemColumn.ReferenceName];
                    }

                    resultTable.Rows.Add(newRow);
                }

                resultTable.ConvertTableToFile(targetFilePath);
                string html = _wiqlCustomWrapper.GenerateHtmlFromTable(resultTable);
                _wiqlCustomWrapper.SendEmail(html);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        [TestMethod]
        public void QueryById()
        {
            string queryId = "1e6c6762-53f9-4254-8685-ff750c4f67be";
            bool? timePrecision = null;
            int? top = null;

            Guid id = new Guid(queryId);
            WorkItemQueryResult workItemQueryResult = _wiqlCustomWrapper.QueryById(id, timePrecision, top);
            Assert.IsTrue(workItemQueryResult != null, $"Unable to fetch query by guid.");
        }

        [TestMethod]
        public void QueryByWiql()
        {
            Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Wiql wiql = new Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Wiql();
            bool? timePrecision = null;
            int? top = null;

            WorkItemQueryResult workItemQueryResult = _wiqlCustomWrapper.QueryByWiql(wiql, timePrecision, top);
            Assert.IsTrue(workItemQueryResult != null, $"Unable to fetch query by work item query language.");
        }
    }
}
