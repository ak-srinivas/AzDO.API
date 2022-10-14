using AzDO.API.Wrappers.WorkItemTracking.WorkItemTypes;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.WorkItemTracking.WorkItemTypes
{
    [TestClass]
    public class GetWorkItemTypesTests : TestBase
    {
        private readonly WorkItemTypesCustomWrapper _workItemTypesCustomWrapper;

        public GetWorkItemTypesTests()
        {
            _workItemTypesCustomWrapper = new WorkItemTypesCustomWrapper();
        }

        [TestMethod]
        public void GetWorkItemType()
        {
            string type = "Bug";

            WorkItemType workItemType = _workItemTypesCustomWrapper.GetWorkItemType(type);
            Assert.IsTrue(workItemType.Name.Equals(type), $"Unable to fetch work item by type.");
        }

        [TestMethod]
        public void ListWorkItemTypes()
        {
            List<WorkItemType> workItemTypes = _workItemTypesCustomWrapper.ListWorkItemTypes();
            Assert.IsTrue(workItemTypes.Count > 0, $"Unable to fetch work item types.");
        }
    }
}
