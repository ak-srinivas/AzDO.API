using AzDO.API.Base.Common;
using AzDO.API.Wrappers.WorkItemTracking.WorkItemTypesField;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.WorkItemTracking.WorkItemTypesField
{
    [TestClass]
    public class GetWorkItemTypesFieldTests : TestBase
    {
        private readonly WorkItemTypesFieldCustomWrapper _workItemTypesFieldCustomWrapper;

        public GetWorkItemTypesFieldTests()
        {
            _workItemTypesFieldCustomWrapper = new WorkItemTypesFieldCustomWrapper();
        }

        [TestMethod]
        public void ListWorkItemTypeFields()
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.UserStory;
            string witType = WrappersBase.GetWorkItemTypeNameAsString(workItemType);

            List<WorkItemTypeFieldWithReferences> workItemTypeFields = _workItemTypesFieldCustomWrapper.ListWorkItemTypeFieldsWithReferences(witType);
            Assert.IsTrue(workItemTypeFields.Count > 0, $"Field information was not found for work item type '{witType}'.");
        }

        [TestMethod]
        public void GetWorkItemTypesField()
        {
            string fieldName = "Iteration Path";
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.UserStory;
            string witType = WrappersBase.GetWorkItemTypeNameAsString(workItemType);

            WorkItemTypeFieldWithReferences workItemTypeField = _workItemTypesFieldCustomWrapper.GetWorkItemTypeFieldWithReferences(witType, fieldName);
            Assert.IsTrue(workItemTypeField.Name.Equals(fieldName), $"Field information was not found for work item type '{witType}' and field name '{fieldName}'.");
        }

        [TestMethod]
        public void GetWorkItemTypesMandatoryFieldsList()
        {
            WorkItemTypeEnum workItemType = WorkItemTypeEnum.UserStory;
            string witType = WrappersBase.GetWorkItemTypeNameAsString(workItemType);

            List<WorkItemTypeFieldWithReferences> mandatoryFieldsList = _workItemTypesFieldCustomWrapper.GetMandatoryFieldsInAWorkItem(workItemType);
            Assert.IsTrue(mandatoryFieldsList.Count > 0, $"No mandatory fields were found for work item type '{witType}'.");
        }
    }
}
