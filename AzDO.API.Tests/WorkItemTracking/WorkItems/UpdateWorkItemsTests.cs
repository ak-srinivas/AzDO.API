using AzDO.API.Base.Common;
using AzDO.API.Base.Common.Utilities;
using AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems;
using AzDO.API.Tests.Work.Iterations;
using AzDO.API.Wrappers.Test.TestSuites;
using AzDO.API.Wrappers.Work.Iterations;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Tests.WorkItemTracking.WorkItems
{
    [TestClass]
    public class UpdateWorkItemsTests : TestBase
    {
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;
        private readonly IterationsCustomWrapper _iterationsCustomWrapper;

        public UpdateWorkItemsTests()
        {
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
            _iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
        }

        [TestMethod]
        public void CreateAndUpdate_TaskWorkItem_ForAMN()
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.Task;

            var tasksData = new List<(string, string, double)>()
            {
                ( "QA Analysis","Requirements",2),
                ( "QA Test Case Design","Testing",1),
                ( "QA Test Case Execution","Testing",4),
                ( "QA Demo Preparation","Testing",1),
            };

            var getIterationsTests = new GetIterationsTests();
            //SortedSet<int> parentIds = _iterationsCustomWrapper.GetQAWorkItemIds_In_CurrentIteration_FilterBy_EmailIds(new List<string>() { Emails.Srinivas });

            // Note: Change Iteration Path First
            List<int> parentIds = new List<int>() { 164395, 159273 };

            for (int i = 0; i < parentIds.Count; i++)
            {
                List<int> taskIds = new List<int>();

                foreach ((string, string, double) taskData in tasksData)
                {
                    var createWorkItemRequest = new CreateWorkItemRequest()
                    {
                        WorkItemType = workItemType,
                        Title = taskData.Item1,
                        AreaPath = AREA,
                        IterationPath = ITERATION,
                    };
                    WorkItem newTask = _workItemsCustomWrapper.CreateWorkItem(createWorkItemRequest);
                    Assert.IsTrue(newTask.Id > 0, "Failed while creating new work item.");
                    taskIds.Add((int)newTask.Id);

                    var updateTaskRequest = new UpdateTaskRequest()
                    {
                        WorkItemId = (int)newTask.Id,
                        AssignedTo = Emails.Srinivas,

                        Priority = "2",
                        Activity = taskData.Item2,

                        OriginalEstimate = taskData.Item3.ToString(),
                        RemainingEstimate = taskData.Item3.ToString(),

                        ParentWorkItemId = parentIds.ElementAt(i)
                    };

                    WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTaskWorkItem(updateTaskRequest);
                    Assert.IsTrue(updatedWorkItem.Id.Equals(updateTaskRequest.WorkItemId), $"Unable to update task work item with id '{updateTaskRequest.WorkItemId}'.");
                }
            }
        }

        [TestMethod]
        public void UpdateTestCaseWorkItem_1()
        {
            List<string> steps = new List<string>() { "step-1", "step-2" };
            string stepsAsString = string.Join(",", steps);

            var updateTestCaseRequest = new UpdateTestCaseRequest()
            {
                WorkItemId = 103669,

                Title = "AzDO SDK TestCase-5",
                AreaPath = AREA,
                IterationPath = ITERATION,
                AssignedTo = Emails.Srinivas, // Input your email id here

                Description = "This is a sample test case created to test the working of azure devops automation.",
                Priority = "1",
                Steps = stepsAsString,


                Tags = new List<string> { "RestAPISample" },

                AutomatedTestName = "AutoBot.Specs.MsTest.Features.Folder1.Folder2.Folder3.Folder4.Folder5.Folder6.TestName",
                AutomatedTestStorage = "AutoBot.Specs.MsTest.dll",
                AutomatedTestId = Guid.NewGuid().ToString(),
                AutomatedTestType = "Unit Test"
            };

            WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTestCaseWorkItem(updateTestCaseRequest);
            Assert.IsTrue(updatedWorkItem.Id.Equals(updateTestCaseRequest.WorkItemId), $"Unable to update task work item with id '{updateTestCaseRequest.WorkItemId}'.");
        }

        [TestMethod]
        public void UpdateTestCaseWorkItem_CopySteps()
        {
            int workitemId = 120500; // Source Test Case
            WorkItem workItemResponse = _workItemsCustomWrapper.GetWorkItem(workitemId);
            //string title = workItemResponse.Fields["System.Title"].ToString();

            string copySteps = workItemResponse.Fields["Microsoft.VSTS.TCM.Steps"].ToString();

            var updateTestCaseRequest = new UpdateTestCaseRequest()
            {
                WorkItemId = 121131,
                //Title = title,
                Steps = copySteps,
                AssignedTo = Emails.Srinivas,

                AutomatedTestName = string.Empty,
                AutomatedTestStorage = string.Empty,
                AutomatedTestId = string.Empty,
            };

            WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTestCaseWorkItem(updateTestCaseRequest);
            Assert.IsTrue(updatedWorkItem.Id.Equals(updateTestCaseRequest.WorkItemId), $"Unable to update task work item with id '{updateTestCaseRequest.WorkItemId}'.");
        }

        [TestMethod]
        public void UpdateCompletedHours_ForMyClosedTasks_InCurrentIteration()
        {
            // Get my tasks from current iteration
            SortedSet<int> myStoryIds = _iterationsCustomWrapper.GetQAWorkItemIds_In_CurrentIteration_FilterBy_EmailIds(new List<string>() { Emails.Srinivas });
            SortedSet<int> myTaskIds = _workItemsCustomWrapper.GetTaskWorkItemIds_FilterBy_EmailId(myStoryIds, Emails.Srinivas);

            foreach (int taskId in myTaskIds)
            {
                WorkItem workItemResponse = _workItemsCustomWrapper.GetWorkItem(taskId);

                if (workItemResponse.Fields["System.State"].ToString().Equals("Closed"))
                {
                    Dictionary<string, string> taskReferenceNames = Helpers.GetFieldNamesWithRefNames();
                    string originalEstimateRef = taskReferenceNames["Original Estimate"].ToString().Replace("/fields/", string.Empty);
                    string completedWorkRef = taskReferenceNames["Completed Work"].ToString().Replace("/fields/", string.Empty);

                    if (workItemResponse.Fields.ContainsKey(originalEstimateRef))
                    {
                        string originalEstimateValue = workItemResponse.Fields[originalEstimateRef].ToString();
                        string completedWorkValue = null;

                        bool isCompletedWorkKeyExists = workItemResponse.Fields.ContainsKey(completedWorkRef);

                        if (isCompletedWorkKeyExists)
                            completedWorkValue = workItemResponse.Fields[completedWorkRef].ToString();

                        if (!isCompletedWorkKeyExists)
                        {
                            // 1. If task is closed and complete work field is null then assign original estimate to completed work field
                            var updateTaskRequest = new UpdateTaskRequest()
                            {
                                WorkItemId = (int)workItemResponse.Id,
                                CompletedEstimate = originalEstimateValue
                            };

                            WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTaskWorkItem(updateTaskRequest);
                            Assert.IsTrue(updatedWorkItem.Fields[completedWorkRef].ToString().Equals(originalEstimateValue), $"Failed to update completed work field in task with id {updatedWorkItem.Id}.");
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(workItemResponse.Fields[completedWorkRef].ToString())
                            || !completedWorkValue.Equals(originalEstimateValue))
                        {
                            // 1. If complete work field is null or empty or whitespace then assign original estimate to completed work
                            // 2. If complete work field is not equal to original estimate then assign original estimate to completed work field

                            var updateTaskRequest = new UpdateTaskRequest()
                            {
                                WorkItemId = (int)workItemResponse.Id,
                                CompletedEstimate = originalEstimateValue
                            };

                            WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTaskWorkItem(updateTaskRequest);
                            Assert.IsTrue(updatedWorkItem.Fields[completedWorkRef].ToString().Equals(originalEstimateValue), $"Failed to update completed work field in task with id {updatedWorkItem.Id}.");
                        }
                    }
                }
            }
        }

        [TestMethod, Ignore]
        public void RemoveTestCaseAssociation()
        {
            List<int> testcaseIds = new List<int>
            {
                321461
            };

            var updatedWorkItem = _workItemsCustomWrapper.RemoveTestCaseAssociation(testcaseIds);
            Assert.IsTrue(updatedWorkItem, $"Failed to remove association for a list of test cases.");
        }

        [TestMethod, Ignore]
        public void RemoveStylesFromSteps_InTestCaseWorkItems_CompletesSuccessfully()
        {
            int planId = 101051;
            int suiteId = 101053;

            List<SuiteTestCase> suiteTestCases = _testSuitesCustomWrapper.GetTestCases(planId, suiteId);

            const string styleToRemove1 = " style=\"background-color: rgba(var(--palette-primary-tint-40,239, 246, 252),1);color: var(--text-primary-color,rgba(0, 0, 0, 0.9))\"";
            const string styleToRemove2 = " style=\"color: var(--text-primary-color,rgba(0, 0, 0, 0.9));background-color: rgba(var(--palette-primary-tint-40,239, 246, 252),1)\"";

            foreach (SuiteTestCase testCase in suiteTestCases)
            {
                int testCaseId = Convert.ToInt32(testCase.Workitem.Id);
                WorkItem workItemResponse = _workItemsCustomWrapper.GetWorkItem(testCaseId);

                string title = workItemResponse.Fields["System.Title"].ToString();
                string ourSteps = workItemResponse.Fields["Microsoft.VSTS.TCM.Steps"].ToString();
                string newSteps = ourSteps.Replace(styleToRemove1, string.Empty).Replace(styleToRemove2, string.Empty);

                if (ourSteps.Equals(newSteps))
                    continue;

                var updateTestCaseRequest = new UpdateTestCaseRequest()
                {
                    WorkItemId = testCaseId,
                    Steps = ourSteps,

                    AutomatedTestName = string.Empty,
                    AutomatedTestStorage = string.Empty,
                    AutomatedTestId = string.Empty,
                };

                WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateTestCaseWorkItem(updateTestCaseRequest);
                Assert.IsTrue(updatedWorkItem.Id.Equals(updateTestCaseRequest.WorkItemId), $"Unable to update task work item with id '{updateTestCaseRequest.WorkItemId}'.");
            }
        }
    }
}
