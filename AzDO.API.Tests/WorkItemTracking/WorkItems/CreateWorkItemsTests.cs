using AzDO.API.Base.Common;
using AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems;
using AzDO.API.Wrappers.Test.TestSuites;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.WorkItemTracking.WorkItems
{
    [TestClass]
    public class CreateWorkItemsTests : TestBase
    {
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public CreateWorkItemsTests()
        {
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
        }

        [TestMethod, Ignore]
        public void CreateTestCaseWorkItem_1()
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.TestCase;

            var createWorkItemRequest = new CreateWorkItemRequest()
            {
                WorkItemType = workItemType,
                Title = "Sample TestCase",
                AreaPath = AREA,
                IterationPath = ITERATION,
                Tags = new List<string>()
                {
                    "MSTest"
                }
            };

            WorkItem newWorkItem = _workItemsCustomWrapper.CreateWorkItem(createWorkItemRequest);
            Assert.IsTrue(newWorkItem.Id > 0, "Created new test case work item.");
        }

        [TestMethod]
        public void UpdateTestCaseWorkItem_And_CopySteps()
        {
            int sourceTestCaseId = 111831;
            List<int> testCaseIds = new List<int>() { 111845 };
            WorkItem workItemResponse = _workItemsCustomWrapper.GetWorkItem(sourceTestCaseId);
            string copySteps = workItemResponse.Fields["Microsoft.VSTS.TCM.Steps"].ToString();

            foreach (int testCaseId in testCaseIds)
            {
                var updateTestCaseRequest = new UpdateTestCaseRequest()
                {
                    WorkItemId = testCaseId,
                    Steps = copySteps,

                    AutomatedTestName = string.Empty,
                    AutomatedTestStorage = string.Empty,
                    AutomatedTestId = string.Empty,
                };

                WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTestCaseWorkItem(updateTestCaseRequest);
                Assert.IsTrue(updatedWorkItem.Id.Equals(updateTestCaseRequest.WorkItemId), $"Unable to update test case work item with id '{updateTestCaseRequest.WorkItemId}'.");
            }
        }

        [TestMethod]
        public void CreateTestCases_And_AddUnder_TestSuite()
        {
            int planId = TestPlanId; // [Created-Using-RestAPI] Sample Test Plan (ID: 308876)
            int suiteId = 292; // Key Vault

            List<string> titles = new List<string>()
            {
                "Verify that the 'Subscription' information exists",
                "Verify that the 'Resource Group' name exists",
                "Verify that the 'Key Vault Name' exists",
                "Verify that the 'Region' name exists",
                "Verify that the 'Pricing Tier (Standard/Premium)' exists",
                "Verify for the existence of an input for 'Days To Retain Deleted Vaults'",
                "Verify for the existence of an input for 'Access Configuration'",
                "Verify for the existence of an input for 'Enable Public Access (Yes/No) or Create A Private Endpoint",
            };

            foreach (string title in titles)
            {
                var createWorkItemRequest = new CreateWorkItemRequest()
                {
                    WorkItemType = WorkItemTypeEnum.TestCase,
                    Title = title,
                    AssignedTo = Emails.EmaildName2,
                    AreaPath = AREA,
                    IterationPath = ITERATION,
                };

                WorkItem newWorkItem = _workItemsCustomWrapper.CreateWorkItem(createWorkItemRequest);
                Assert.IsTrue(newWorkItem.Id > 0, "Created new test case work item.");

                List<SuiteTestCase> suiteTestCases = _testSuitesCustomWrapper.AddTestCasesToSuite(planId, suiteId, new List<int> { (int)newWorkItem.Id });
                Assert.IsTrue(suiteTestCases.Count > 0, $"Failed to add test cases to test suite with id '{suiteId}'.");
            }
        }

        [TestMethod]
        public void CreateTestCaseWorkItems_With_Title()
        {
            int userStoryId = 163809; // User Story

            List<string> titles = new List<string>()
            {
                "Verify that the 'Subscription' information exists",
            };

            foreach (string title in titles)
            {
                var createWorkItemRequest = new CreateWorkItemRequest()
                {
                    WorkItemType = WorkItemTypeEnum.TestCase,
                    Title = title,
                    AssignedTo = Emails.EmaildName1,
                    AreaPath = AREA,
                    IterationPath = ITERATION,
                };

                WorkItem newWorkItem = _workItemsCustomWrapper.CreateWorkItem(createWorkItemRequest);
                Assert.IsTrue(newWorkItem.Id > 0, "Created new test case work item.");

                var updateTestCaseRequest = new UpdateTestCaseRequest()
                {
                    WorkItemId = (int)newWorkItem.Id,
                    ParentWorkItemId = userStoryId,
                    Steps = null,

                    AutomatedTestName = string.Empty,
                    AutomatedTestStorage = string.Empty,
                    AutomatedTestId = string.Empty,
                };

                WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTestCaseWorkItem(updateTestCaseRequest);
                Assert.IsTrue(updatedWorkItem.Id.Equals(updateTestCaseRequest.WorkItemId), $"Unable to update test case work item with id '{updateTestCaseRequest.WorkItemId}'.");
            }
        }

        [TestMethod]
        public void CreateTaskWorkItem()
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.Task;

            var createWorkItemRequest = new CreateWorkItemRequest()
            {
                WorkItemType = workItemType,
                Title = "Develop and test automation for azure devops work item tracking using public rest apis",
                AreaPath = AREA,
                IterationPath = ITERATION,
                Tags = new List<string>()
                {
                    "Scrum"
                }
            };

            WorkItem newWorkItem = _workItemsCustomWrapper.CreateWorkItem(createWorkItemRequest);
            Assert.IsTrue(newWorkItem.Id > 0, "Created new work item.");
        }
    }
}
