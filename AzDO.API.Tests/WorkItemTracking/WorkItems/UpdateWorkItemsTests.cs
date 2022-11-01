using AzDO.API.Base.Common;
using AzDO.API.Base.Common.Extensions;
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
using System.Data;
using System.Linq;
using System.Security.Cryptography;

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
        public void GetDuplicates()
        {
            var featureIds = new List<int>()
            {
                51,
                57,
                58,
                59,
                60,
                61,
                62,
                63,
                64,
                65,
                69,
                70,
                71,
                72,
                73,
                74,
                75,
                76,
                77,
                78,
                79,
                80,
                81,
                82,
                83,
                84,
                86,
                87,
                110,
                111,
                112,
                113,
                117,
                119,
                120,
                121,
                122,
                124,
                126,
                129,
                140,
                142,
                146,
                191,
                193,
                196,
                199,
                201,
                202,
                205,
                219,
                269,
                275,
                330,
                345,
                379,
                382,
                456,
                480,
                481,
                482,
                574,
                598,
                615,
                616,
                757,
                830,

            };
            var tables = new List<DataTable>();
            var earlierTable = new DataTable();

            foreach (int featureId in featureIds)
            {
                var csvTable = _workItemsCustomWrapper.GetDuplicates(featureId, earlierTable);
                earlierTable.Merge(csvTable);
            }

            string targetFilePath = $@"D:\Files\Ploceus\Duplicate_WorkItems_All.csv";
            earlierTable.ConvertTableToFile(targetFilePath);
            Console.WriteLine();
        }


        [TestMethod]
        public void CreateAndUpdate_TaskWorkItem()
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.Task;

            var tasksData = new List<(string, string, double)>()
            {
                ( "Create resource manually and analyze dependencies and requirements","Requirements",3),
                ( "Create test cases to verify ploceus code for mandatory parameters and dependencies","Design",1),
                ( "Analyze manual workflow for optional dependencies","Design",2),
                ( "Create test cases and verify ploceus code for implementation of optional dependencies","Design",2),
            };

            var getIterationsTests = new GetIterationsTests();
            //SortedSet<int> parentIds = _iterationsCustomWrapper.GetQAWorkItemIds_In_CurrentIteration_FilterBy_EmailIds(new List<string>() { Emails.Srinivas });

            // Note: Change Iteration Path First
            var parentIds = new List<int>() {
                767,
                769,
                773,
                775,
                777,
                779,
                781,
                783,
                785,
                787,
                789,
                791,
                793,
                795,
                797,
                799,
                801,
                803,
                805,
                807,
                809,
                811,
                813,
                815,
                817,
                831,
                833,
                835,
                837,
                839,
                841,
                843,
                845,
                847,
                849,
                851,
                855,
                857,
                859,
                861,
                863,
                865,
                867,
                869,
                871,
                873,
                875,
                877,
                879,
                881,
                884,
                886,
                888,
                890,
                892,
                894,
                896,
                898,
                900,
                902,
                904,
                906,
                908,
                910,
                912,
                914,
                916,
                918,
                920,
                922,
                924,
                926,
                928,
                930,
                932,
                934,
                936,
                938,
            };

            var ignoreList = new List<int>()
            {
                906,908, 914, 916, 922, 926,847,861
            };

            for (int i = 0; i < parentIds.Count; i++)
            {
                if (ignoreList.Contains(parentIds.ElementAt(i)))
                    continue;

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
                        //AssignedTo = Emails.Anjani,

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
        public void UpdateTags()
        {
            string prepTag = "test case prep";
            string execTag = "test case exec";

            var ids = new List<int>()
            {

            };

            foreach (var id in ids)
            {
                var updateWorkItem = new UpdateWorkItemRequest()
                {
                    WorkItemId = id,
                    Tags = new List<string>()
                    {
                        execTag
                    }
                };

                WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateWorkItem(updateWorkItem);
                Assert.IsTrue(updatedWorkItem.Id.Equals(updateWorkItem.WorkItemId), $"Unable to update task work item with id '{updateWorkItem.WorkItemId}'.");
            }

        }

        [TestMethod]
        public void AddUserStoryToFeature()
        {
            var ignoreList = new List<int>()
            {
                768,
                770,
                774,
                776,
                778,
                780,
                782,
                784,
                786,
                788,
                790,
                792,
                794,
                796,
                798,
                800,
                802,
                804,
                806,
                808,
                810,
                812,
                814,
                816,
                818,
                832,
                834,
                836,
                838,
                840,
                842,
                844,
                846,
                848,
                850,
                852,
                856,
                858,
                860,
                862,
                864,
                866,
                868,
                870,
                872,
                874,
                876,
                878,
                880,
                882,
                885,
                887,
                889,
                891,
                893,
                895,
                897,
                899,
                901,
                903,
                931,
                933,
                935,
                937,
                939,


            };

            for (int startId = 767; startId <= 939; startId++)
            {
                if (startId == 771 || startId == 772 || startId == 853 || startId == 854 || startId == 883)
                    continue;

                if (ignoreList.Contains(startId))
                    continue;
                else
                {
                    var updateUserStoryRequest = new UpdateUserStoryRequest()
                    {
                        WorkItemId = startId,
                        //AcceptanceCriteria = " ",
                        AcceptanceCriteria = "<div><span style=\"display:inline !important;\"><b><u>Create below test cases:</u></b></span><ol style=\"box-sizing:border-box;padding-left:40px;clear:left;\"><li style=\"box-sizing:border-box;\">Test if code works with Terraform version 1.1.7 and AzureRM version<span style=\"box-sizing:border-box;\">&nbsp;3.9.0</span> </li><li style=\"box-sizing:border-box;\">Check if secrets are pulled from key vault </li><li style=\"box-sizing:border-box;\">Ensure modules run from pipeline are success </li><li style=\"box-sizing:border-box;\">Resource created in Azure portal should reflect all inputs the user provide in tfvars<ol style=\"box-sizing:border-box;padding-left:40px;clear:left;\"><li style=\"box-sizing:border-box;\">Test boolean values </li><li style=\"box-sizing:border-box;\">check for values from data blocks </li> </ol> </li><li style=\"box-sizing:border-box;\">Validate the Terraform state files in remote backend </li> </ol> </div>"
                    };

                    WorkItem updatedWorkItem = _workItemsCustomWrapper.UpdateUserStoryWorkItem(updateUserStoryRequest);
                    Assert.IsTrue(updatedWorkItem.Id.Equals(updateUserStoryRequest.WorkItemId), $"Unable to update task work item with id '{updateUserStoryRequest.WorkItemId}'.");
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
