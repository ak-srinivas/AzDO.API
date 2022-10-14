using AzDO.API.Wrappers.WorkItemTracking.Fields;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.WorkItemTracking.Fields
{
    [TestClass]
    public class GetFieldsTests : TestBase
    {
        private readonly FieldsCustomWrapper _fieldsCustomWrapper;

        public GetFieldsTests()
        {
            _fieldsCustomWrapper = new FieldsCustomWrapper();
        }

        [TestMethod]
        public void GetField()
        {
            string fieldName = "Automated Test Name";
            WorkItemField workItemField = _fieldsCustomWrapper.GetField(fieldName);
            Assert.IsTrue(workItemField.Name.Equals(fieldName), $"No information was found for field '{fieldName}'.");
        }

        [TestMethod]
        public void ListFields()
        {
            List<WorkItemField> workItemFields = _fieldsCustomWrapper.ListFields();
            Assert.IsTrue(workItemFields.Count > 0, "No fields were found.");
        }

        [TestMethod]
        public void GetReadOnlyWorkItemFields()
        {
            List<WorkItemField> readOnlyWorkItemFields = _fieldsCustomWrapper.GetReadOnlyWorkItemFields();
            Assert.IsTrue(readOnlyWorkItemFields.Count > 0, "No read only work item fields were found.");
        }

        [TestMethod]
        public void GetFieldsNameWithReferenceNames()
        {
            Dictionary<string, string> fieldsNameAndRefNames = _fieldsCustomWrapper.GetFieldsNameWithReferenceNames();
            Assert.IsTrue(fieldsNameAndRefNames.Count > 0, "No fields were found.");
        }
    }
}
