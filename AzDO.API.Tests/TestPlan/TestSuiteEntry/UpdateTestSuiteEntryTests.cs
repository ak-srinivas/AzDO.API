using AzDO.API.Wrappers.TestPlan.TestSuiteEntry;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.TestPlan.TestSuiteEntry
{
    [TestClass]
    public class UpdateTestSuiteEntryTests : TestBase
    {
        private readonly TestSuiteEntryCustomWrapper _testSuiteEntryCustomWrapper;

        public UpdateTestSuiteEntryTests()
        {
            _testSuiteEntryCustomWrapper = new TestSuiteEntryCustomWrapper();
        }

        [TestMethod, Ignore]
        public void ReorderSuiteEntries()
        {
            SuiteEntryUpdateParams first = new SuiteEntryUpdateParams
            {
                Id = 108882,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 1
            };

            SuiteEntryUpdateParams second = new SuiteEntryUpdateParams
            {
                Id = 104992,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 2
            };

            SuiteEntryUpdateParams third = new SuiteEntryUpdateParams
            {
                Id = 104986,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 3
            };

            SuiteEntryUpdateParams fourth = new SuiteEntryUpdateParams
            {
                Id = 104985,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 4
            };

            var suiteEntries = new List<SuiteEntryUpdateParams>
            {
                first, second, third, fourth
            };

            int suiteId = 104916;

            List<SuiteEntry> testSuites = _testSuiteEntryCustomWrapper.ReorderSuiteEntries(suiteEntries, suiteId);
            Assert.IsTrue(testSuites != null, $"Failed to reorder test cases in test suite.");
        }
    }
}
