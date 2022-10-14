using AzDO.API.Wrappers.Test.TestSuites;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.Test.TestSuites
{
    [TestClass]
    public class UpdateTestSuitesTests : TestBase
    {
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public UpdateTestSuitesTests()
        {
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
        }

        [TestMethod, Ignore]
        public void UpdateSuiteTestCases()
        {
            SuiteTestCaseUpdateModel suiteTestCaseUpdateModel = null;
            int planId = 0;
            int suiteId = 0;
            var testCaseIds = new List<int>();

            List<SuiteTestCase> suiteTestCases = _testSuitesCustomWrapper.UpdateSuiteTestCasesAsync(suiteTestCaseUpdateModel, planId, suiteId, testCaseIds);
            Assert.IsTrue(suiteTestCases.Count > 0, $"Failed to update test cases in test suite with id '{suiteId}'.");
        }
    }
}
