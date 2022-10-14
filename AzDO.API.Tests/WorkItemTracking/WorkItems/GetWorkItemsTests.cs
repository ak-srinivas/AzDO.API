using AzDO.API.Base.Common;
using AzDO.API.Wrappers.Work.Iterations;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.WorkItemTracking.WorkItems
{
    [TestClass]
    public class GetWorkItemsTests : TestBase
    {
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;
        private readonly IterationsCustomWrapper _iterationsCustomWrapper;
        public GetWorkItemsTests()
        {
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
            _iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
        }

        [TestMethod]
        public void GetWorkItem()
        {
            int workitemId = 101882; // Test Case
            var workItemResponse = _workItemsCustomWrapper.GetWorkItem(workitemId);
            Assert.IsTrue(workItemResponse.Id.Equals(workitemId));
        }

        [TestMethod]
        public void GetWorkItemTemplate()
        {
            var fieldsSet = new HashSet<string>
            {
                "System.AreaPath",
                "System.IterationPath",
                "System.AssignedTo",
                "System.State"
            };

            var workItemResponse = _workItemsCustomWrapper.GetWorkItemTemplate(WorkItemTypeEnum.TestCase, fieldsSet, WorkItemExpand.All);
            Assert.IsTrue(workItemResponse != null, "Failed to get work item template.");
        }

        [TestMethod]
        public void GetWorkItemsBatch()
        {
            var getBatchRequest = new WorkItemBatchGetRequest
            {
                Ids = new List<int>
                {
                    303315
                }
            };
            getBatchRequest.Expand = WorkItemExpand.All;
            getBatchRequest.ErrorPolicy = WorkItemErrorPolicy.Omit;

            var workItemResponse = _workItemsCustomWrapper.GetWorkItemsBatch(getBatchRequest);
            Assert.IsTrue(workItemResponse.Count > 0, "Failed to get work items batch.");
        }

        [TestMethod]
        public void ListWorkItems()
        {
            List<int> workItemIds = new List<int>
            {
                297003, // Epic
                303324, // Feature
                303315, // User Story
                306364, // Bug
                296500, // Task
                130653, // Test Plan
                144063, // Test Suite
                43110, // Test Case
                308312, // Code Review Request
                308263 // Code Review Response
            };

            List<WorkItem> workItems = _workItemsCustomWrapper.ListWorkItems(workItemIds, null, null, WorkItemExpand.All);
            Assert.IsTrue(workItems.Count > 0, "No work items were fetched");
            Assert.IsTrue(workItems.Count.Equals(workItemIds.Count), "Not all work items were found");
        }

        [TestMethod]
        public void GetCycleTimeFromCurrentSprint()
        {
            HashSet<WorkItem> currentWorkItemsSet = _iterationsCustomWrapper.GetWorkItems_InCurrentIteration();
            foreach (var workItem in currentWorkItemsSet)
            {
                System.Console.WriteLine();
            }
        }
    }
}
