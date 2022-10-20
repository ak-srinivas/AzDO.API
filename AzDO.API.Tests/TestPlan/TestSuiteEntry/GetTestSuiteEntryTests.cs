using AzDO.API.Wrappers.TestPlan.TestSuiteEntry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Tests.TestPlan.TestSuiteEntry
{
    [TestClass]
    public class GetTestSuiteEntryTests : TestBase
    {
        private readonly TestSuiteEntryCustomWrapper _testSuiteEntryCustomWrapper;

        public GetTestSuiteEntryTests()
        {
            _testSuiteEntryCustomWrapper = new TestSuiteEntryCustomWrapper();
        }

        [TestMethod]
        public void GetSuiteEntries()
        {
            string project = ProjectNames.Ploceus;
            int suiteId = 126043;
            SuiteEntryTypes? suiteEntryType = SuiteEntryTypes.TestCase;

            List<SuiteEntry> testSuites = _testSuiteEntryCustomWrapper.GetSuiteEntries(project, suiteId, suiteEntryType);
            Assert.IsTrue(testSuites != null, $"Failed to get test suite entries.");
        }
    }
}
