using AzDO.API.Wrappers.Work.Iterations;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AzDO.API.Tests.Work.Iterations
{
    [TestClass]
    public class GetIterationsTests : TestBase
    {
        private readonly IterationsCustomWrapper _iterationsCustomWrapper;
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;

        public GetIterationsTests()
        {
            _iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
        }

        [TestMethod]
        public void GetTeamIterations()
        {
            var teamContext = new TeamContext(_iterationsCustomWrapper.GetProjectName(), TeamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = _iterationsCustomWrapper.GetTeamIterations(teamContext);
            Assert.IsTrue(teamSettingsIterations != null && teamSettingsIterations.Count > 0, "No iterations were found.");
        }

        [TestMethod]
        public void GetIterationWorkItems()
        {
            TeamSettingsIteration teamSettingsIteration = _iterationsCustomWrapper.GetCurrentIteration();
            Guid iterationId = teamSettingsIteration.Id;

            var teamContext = new TeamContext(_iterationsCustomWrapper.GetProjectName(), TeamBoardName);
            IterationWorkItems iterationWorkItems = _iterationsCustomWrapper.GetIterationWorkItems(teamContext, iterationId);
            Assert.IsTrue(iterationWorkItems != null, $"Unable to fetch work items for the given iteration id.");
        }

        [TestMethod]
        public void GetStoryPoints()
        {
            const string sprintNumber = "2022.18";
            string iterationName = $"Your Iteration Name Sprint {sprintNumber}"; // This name should match with your sprint board name

            var teamContext = new TeamContext(_iterationsCustomWrapper.GetProjectName(), TeamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = _iterationsCustomWrapper.GetTeamIterations(teamContext);
            Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

            int storyPoints = (int)_iterationsCustomWrapper.GetStoryPoints(iterationId);
            Assert.IsTrue(storyPoints > 0, $"Story points for the current sprint is 0.");
        }

        [TestMethod]
        public void Get_PRData_FromWorkItems_InCurrentIteration()
        {
            const string sprintNumber = "2022.19";
            string iterationName = $"Your Iteration Name Sprint {sprintNumber}";
            string targetFilePath = $"D:\\Files\\PRs\\PR_Status_For_YourBoardName_{sprintNumber.Replace(".", "_")}.csv";

            var teamContext = new TeamContext(_iterationsCustomWrapper.GetProjectName(), TeamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = _iterationsCustomWrapper.GetTeamIterations(teamContext);
            Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

            DataTable csvTable = _iterationsCustomWrapper.GetLinks_FromWorkItems_InIteration(iterationId);
            ConvertTableToFile(csvTable, targetFilePath);
            Assert.IsTrue(File.Exists(targetFilePath), $"Csv file with PR information releated to current sprint items was not exported.");
        }

        [TestMethod]
        public void Get_My_WorkItems()
        {
            const string sprintNumber = "2021.<number>";
            
            var tables = new List<DataTable>();
            string targetFilePath = $"D:\\MyWork\\MyWork_{TeamBoardName.Replace(" ", "_")}.csv";

            for (int i = 19; i <= 25; i++)
            {
                string sprint = sprintNumber.Replace("<number>", i.ToString());
                string iterationName = $"Your Iteration Name Sprint {sprint}";

                var teamContext = new TeamContext(_iterationsCustomWrapper.GetProjectName(), TeamBoardName);
                List<TeamSettingsIteration> teamSettingsIterations = _iterationsCustomWrapper.GetTeamIterations(teamContext);
                Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

                DataTable csvTable = _iterationsCustomWrapper.GetMyWorkItems_InIteration(iterationId, Emails.EmaildName1, iterationName);
                tables.Add(csvTable);
            }

            if (tables.Count > 0)
            {
                var finalTable = new DataTable();

                foreach (DataTable table in tables)
                {
                    finalTable.Merge(table);
                    finalTable.AcceptChanges();
                }

                ConvertTableToFile(finalTable, targetFilePath);
            }
        }
    }
}
