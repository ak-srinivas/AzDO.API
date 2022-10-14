using System;
using System.Collections.Generic;
using System.Data;

namespace AzDO.API.Base.Common.Utilities
{
    public class Helpers
    {
        public static Dictionary<string, string> GetFieldNamesWithRefNames()
        {
            return new Dictionary<string, string>()
            {
                {"Title", "/fields/System.Title"},
                {"Area Path", "/fields/System.AreaPath"},
                {"Iteration Path", "/fields/System.IterationPath"},
                {"Tags", "/fields/System.Tags"},
                {"Assigned To", "/fields/System.AssignedTo"},
                {"Description", "/fields/System.Description"},
                {"Priority", "/fields/Microsoft.VSTS.Common.Priority"},
                
                {"Activity", "/fields/Microsoft.VSTS.Common.Activity"},
                {"Original Estimate", "/fields/Microsoft.VSTS.Scheduling.OriginalEstimate"},
                {"Remaining Work", "/fields/Microsoft.VSTS.Scheduling.RemainingWork"},
                {"Completed Work", "/fields/Microsoft.VSTS.Scheduling.CompletedWork"},

                {"Steps", "/fields/Microsoft.VSTS.TCM.Steps"},
                {"QATeam", "/fields/Custom.QATeam"},
                {"QA Test Type", "/fields/Custom.QATestType"},
                {"Automated Test Name", "/fields/Microsoft.VSTS.TCM.AutomatedTestName"},
                {"Automated Test Storage", "/fields/Microsoft.VSTS.TCM.AutomatedTestStorage"},
                {"Automated Test Type", "/fields/Microsoft.VSTS.TCM.AutomatedTestType"},
                {"Automated Test Id", "/fields/Microsoft.VSTS.TCM.AutomatedTestId"},
                {"Automation status", "/fields/Microsoft.VSTS.TCM.AutomationStatus"},

            };
        }

        public static string GenerateHtmlFromTable(DataTable table)
        {
            // Source: https://stackoverflow.com/questions/8811353/send-a-table-in-email
            try
            {
                //string messageBody = "<font> " + title + " </font><br><br>";
                string messageBody = "<br><br>";

                if (table.Rows.Count == 0)
                    return messageBody;
                string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style =\"color:#555555;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
                string htmlTdEnd = "</td>";

                messageBody += htmlTableStart;

                messageBody += htmlHeaderRowStart;

                foreach (DataColumn column in table.Columns)
                    messageBody += htmlTdStart + column + htmlTdEnd;

                messageBody += htmlHeaderRowEnd;

                foreach (DataRow row in table.Rows)
                {
                    messageBody += htmlTrStart;


                    foreach (string item in row.ItemArray)
                    {
                        messageBody += htmlTdStart;
                        messageBody += item;
                        messageBody += htmlTdEnd;
                    }
                    messageBody += htmlTrEnd;
                }
                messageBody += htmlTableEnd;


                return messageBody;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
