using AzDO.API.Base.Common.Extensions;
using AzDO.API.Wrappers.Work.Iterations;
using AzDO.API.Wrappers.WorkItemTracking.Updates;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace AzDO.API.Tests.WorkItemTracking.Updates
{
    [TestClass]
    public class GetUpdatesTests : TestBase
    {
        private readonly UpdatesCustomWrapper updatesCustomWrapper;
        private readonly IterationsCustomWrapper iterationsCustomWrapper;

        public GetUpdatesTests()
        {
            updatesCustomWrapper = new UpdatesCustomWrapper();
            iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
        }

        [TestMethod]
        public void GetUpdates()
        {
            const string sprintNumber = "2022.14";
            string iterationName = $"Your Iteration Name Sprint {sprintNumber}";
            string targetFilePath = $"D:\\Files\\CycleTime\\CycleTime_Status_For_S&S_Sprint_{sprintNumber.Replace(".", "_")}.csv";

            var teamContext = new TeamContext(iterationsCustomWrapper.GetProjectName(), TeamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = iterationsCustomWrapper.GetTeamIterations(teamContext);
            Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

            HashSet<WorkItem> workItems = iterationsCustomWrapper.GetWorkItems_InIteration(iterationId);
            DataTable csvTable = updatesCustomWrapper.GetCycleTimeFromWorkItems(workItems);

            csvTable.ConvertTableToFile(targetFilePath);
            Assert.IsTrue(File.Exists(targetFilePath), $"Csv file with cycle time information releated to current sprint items was not exported.");
        }

        [TestMethod]
        public void Get_All_Updates()
        {
            const string sprintNumber = "2021.<number>";
            string targetFilePath = $"D:\\Files\\CycleTime\\CycleTime_Status_For_S&S_Sprint.csv";

            var tables = new List<DataTable>();

            for (int i = 1; i <= 25; i++)
            {
                string sprint = sprintNumber.Replace("<number>", i.ToString());
                string iterationName = $"Your Iteration Name Sprint {sprint}";

                if (i < 4)
                {
                    iterationName = $"Your Iteration Name Sprint {sprintNumber.Replace("<number>", "0" + i.ToString())}";
                    //targetFilePath = $"D:\\Files\\CycleTime\\CycleTime_Status_For_S&S_Sprint_{sprintNumber.Replace("<number>", "0" + i.ToString()).Replace(".", "_")}.csv";
                }
                if (i == 4)
                {
                    iterationName = $"Your Iteration Name {sprintNumber.Replace("<number>", i.ToString())}";
                    //targetFilePath = $"D:\\Files\\CycleTime\\CycleTime_Status_For_S&S_Sprint_{sprintNumber.Replace("<number>", i.ToString()).Replace(".", "_")}.csv";
                }

                if (i == 7)
                    iterationName = $"Your Iteration Name  Sprint {sprint}";

                var teamContext = new TeamContext(iterationsCustomWrapper.GetProjectName(), TeamBoardName);
                List<TeamSettingsIteration> teamSettingsIterations = iterationsCustomWrapper.GetTeamIterations(teamContext);
                Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

                try
                {
                    HashSet<WorkItem> workItems = iterationsCustomWrapper.GetWorkItems_InIteration(iterationId);
                    DataTable csvTable = updatesCustomWrapper.GetCycleTimeFromWorkItems(workItems);
                    tables.Add(csvTable);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }

            if (tables.Count > 0)
            {
                var finalTable = new DataTable();

                foreach (DataTable table in tables)
                {
                    finalTable.Merge(table);
                    finalTable.AcceptChanges();
                }

                finalTable.ConvertTableToFile(targetFilePath);
            }
        }
    }
}
