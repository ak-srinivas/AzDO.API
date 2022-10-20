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
            int suiteId = 525; // Key Vault

            List<string> titles = new List<string>()
            {
                "Create VM with data disk, after VM gets provisoned create and attach new data disk",
"Create another VM and swap OS disk with the one that you created earlier",
"Create VM, create snapshot of OS disk with snapshot type as Full",
"Create VM, create snapshot of OS disk with snapshot type as Incremental",
"Create VM, create snapshot of OS disk with encryption types (3 Types)",
"Create VM, create snapshot of OS disk and enable access to your snapshot either publicly using public IP addresses or privately using private endpoints",
"Create VM, resize from B2s to B4ms and verify for 4 CPUs and 16GiB RAM, the VM should restart",
"Create VM, go to extensions and uninstall extensions",
"Create VM, add new extension like (Azure Performance Diagnostics Extension) and verify its installation from within the VM",
"Create VM, add VM applications",
"[Not Sure] Create VM and update fault, update, availability set and VM scale set",
"Create VM by choosing an existing Windows Server License",
"Create VM and create a user assigned managed identity so that the VM authenticates with cloud services role",
"Create locks on subscription or resource group or vm scope",
"Create VM, update auto-shutdown settings",
"Create VM, enable backup",
"Create VM, and setup disaster recovery",
            };

            foreach (string title in titles)
            {
                var createWorkItemRequest = new CreateWorkItemRequest()
                {
                    WorkItemType = WorkItemTypeEnum.TestCase,
                    Title = title,
                    AssignedTo = Emails.Srinivas,
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
                    AssignedTo = Emails.Srinivas,
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
