using AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems;
using AzDO.API.Wrappers.TestPlan.TestSuiteEntry;
using AzDO.API.Wrappers.TestPlan.TestSuites;
using AzDO.API.Wrappers.Work.Iterations;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Tests.TestPlan.TestSuites
{
    [TestClass]
    public class CreateTestSuitesTests : TestBase
    {
        private readonly TestSuiteEntryCustomWrapper _testSuiteEntryCustomWrapper;
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;
        private readonly IterationsCustomWrapper _iterationsCustomWrapper;
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public CreateTestSuitesTests()
        {
            _testSuiteEntryCustomWrapper = new TestSuiteEntryCustomWrapper();
            _iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
        }

        [TestMethod]
        public void CreateTestSuites()
        {
            var suiteNames = new List<string>()
            {
                "Resource Group",
                "Disk Encryption Set",
                "Key Vault",
                "Network Security Group",
                "Public IP Address",
                "Virtual Machine",
                "Virtual Network"
            };

            string project = ProjectNames.YourProjectName;
            //string sprintName = "Sprint 2022.04";

            foreach (string suiteName in suiteNames)
            {
                TestSuiteCreateParams testSuiteCreateParams = new TestSuiteCreateParams()
                {
                    Name = suiteName,
                    InheritDefaultConfigurations = true,
                    SuiteType = TestSuiteType.StaticTestSuite,
                    ParentSuite = new TestSuiteReference()
                    {
                        Id = 289,
                        Name = "Regresssion Tests"
                    },
                };

                TestSuite parentTestSuite = _testSuitesCustomWrapper.CreateTestSuite(project, testSuiteCreateParams, TestPlanId);
                Assert.IsTrue(parentTestSuite != null, $"Failed to create test suite.");
            }
        }

        [TestMethod]
        public void FindRequirementBasedSuites_ThatAreNotInAGivenSprint()
        {
            string project = ProjectNames.YourProjectName;

            const string sprintNumber = "2022.12";
            string sprintName = $"Sprint {sprintNumber}";
            string iterationName = $"Your Iteration Name {sprintName}";

            string iterationPath = $@"{AREA}\{iterationName}";

            var teamContext = new TeamContext(project, TeamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = _iterationsCustomWrapper.GetTeamIterations(teamContext);
            Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

            List<int> storyIdsFromSprintBoard = _iterationsCustomWrapper.GetQAWorkItemIds_InIteration_FilterBy_EmailIds(iterationId, new List<string>() { Emails.EmaildName1 });

            if (storyIdsFromSprintBoard.Count > 0)
            {
                TestSuite sprintTestSuite = _testSuitesCustomWrapper.GetTestSuiteByNameWithinTestPlan(project, TestPlanId, sprintName);
                if (sprintTestSuite != null)
                {
                    // We should find out if our requirement based suites are already existing within our static test suite!
                    List<SuiteEntry> reqBasedTestSuites = _testSuiteEntryCustomWrapper.GetSuiteEntries(project, sprintTestSuite.Id, SuiteEntryTypes.Suite);
                    var existingReqBasedSuiteIds = new List<int>();

                    foreach (SuiteEntry reqBasedTestSuite in reqBasedTestSuites)
                    {
                        string title = _workItemsCustomWrapper.GetWorkItem(reqBasedTestSuite.Id).Fields["System.Title"].ToString();
                        int storyId = Convert.ToInt32(title.Split(":", StringSplitOptions.RemoveEmptyEntries).First().Trim());
                        existingReqBasedSuiteIds.Add(storyId);
                    }

                    List<int> reqBasedSuitesToBeDeleted = existingReqBasedSuiteIds.Except(storyIdsFromSprintBoard).ToList();
                    Console.WriteLine();
                }
            }

        }

        [TestMethod]
        public void CreateTestSuite_ChangeIteration_AddRequirementBasedSuite_ForAGivenSprint()
        {
            string project = ProjectNames.YourProjectName;

            const string sprintNumber = "2022.19";
            string sprintName = $"Sprint {sprintNumber}";
            string iterationName = $"Your Iteration Name {sprintName}";

            string iterationPath = $@"{AREA}\{iterationName}";

            var teamContext = new TeamContext(project, TeamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = _iterationsCustomWrapper.GetTeamIterations(teamContext);
            Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

            List<int> storyIdsFromSprintBoard = _iterationsCustomWrapper.
                GetQAWorkItemIds_InIteration_FilterBy_EmailIds(iterationId, new List<string>() { Emails.EmaildName1 });

            if (storyIdsFromSprintBoard.Count > 0)
            {
                TestSuite sprintTestSuite = _testSuitesCustomWrapper.GetTestSuiteByNameWithinTestPlan(project, TestPlanId, sprintName);

                if (sprintTestSuite == null)
                {
                    // Static test suite does not exists, so we will create it!
                    TestSuiteCreateParams testSuiteCreateParams = new TestSuiteCreateParams()
                    {
                        Name = sprintName,
                        InheritDefaultConfigurations = true,
                        SuiteType = TestSuiteType.StaticTestSuite,
                        ParentSuite = new TestSuiteReference()
                        {
                            Id = DefaultTestSuiteId,
                            Name = TestPlanName
                        },
                    };

                    sprintTestSuite = _testSuitesCustomWrapper.CreateTestSuite(project, testSuiteCreateParams, TestPlanId);
                    Assert.IsTrue(sprintTestSuite != null, $"Failed to create static test suite.");

                    // Update area and iteration paths for our static test suite!
                    var updateWorkItemRequest = new UpdateWorkItemRequest()
                    {
                        WorkItemId = sprintTestSuite.Id,
                        AreaPath = AREA,
                        IterationPath = iterationPath
                    };

                    Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem updatedStaticTestSuite =
                    _workItemsCustomWrapper.UpdateWorkItem(updateWorkItemRequest);
                    Assert.IsTrue(updatedStaticTestSuite != null, $"Failed to update static test suite.");
                }

                // At this point our static test suite will be created.
                // We should find out if our requirement based suites are already existing within our static test suite!
                List<SuiteEntry> reqBasedTestSuites = _testSuiteEntryCustomWrapper.GetSuiteEntries(project, sprintTestSuite.Id, SuiteEntryTypes.Suite);
                var existingReqBasedSuiteIds = new List<int>();

                foreach (SuiteEntry reqBasedTestSuite in reqBasedTestSuites)
                {
                    string title = _workItemsCustomWrapper.GetWorkItem(reqBasedTestSuite.Id).Fields["System.Title"].ToString();
                    if (title.ToLower().Contains("customer service") || title.ToLower().Contains("release management"))
                        continue;

                    int storyId = Convert.ToInt32(title.Split(":", StringSplitOptions.RemoveEmptyEntries).First().Trim());
                    existingReqBasedSuiteIds.Add(storyId);
                }

                List<int> toBeAddedSuiteIds = storyIdsFromSprintBoard.Except(existingReqBasedSuiteIds).ToList();

                // Link requirement based test suites under our static test suite!
                foreach (int storyId in toBeAddedSuiteIds)
                {
                    var testSuiteCreateParams = new TestSuiteCreateParams()
                    {
                        InheritDefaultConfigurations = true,
                        SuiteType = TestSuiteType.RequirementTestSuite,
                        RequirementId = storyId,

                        ParentSuite = new TestSuiteReference()
                        {
                            Id = sprintTestSuite.Id,
                            Name = sprintName
                        },
                    };

                    TestSuite requirementTestSuite = _testSuitesCustomWrapper.CreateTestSuite(project, testSuiteCreateParams, TestPlanId);
                    Assert.IsTrue(requirementTestSuite != null, $"Failed to create requirement test suite.");
                }
            }
        }
    }
}
