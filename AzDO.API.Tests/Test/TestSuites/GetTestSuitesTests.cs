using AzDO.API.Wrappers.Test.TestSuites;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.Test.TestSuites
{
    [TestClass]
    public class GetTestSuitesTests : TestBase
    {
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public GetTestSuitesTests()
        {
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
        }

        [TestMethod, Ignore]
        public void GetTestCaseById()
        {
            int planId = 0;
            int suiteId = 0;
            int testCaseId = 0;

            SuiteTestCase suiteTestCase = _testSuitesCustomWrapper.GetTestCaseById(planId, suiteId, testCaseId);
            Assert.IsTrue(suiteTestCase != null, $"Failed to get test case by id.");
        }

        [TestMethod]
        public void GetTestCases()
        {
            int planId = 104912;
            int suiteId = 104916;

            List<SuiteTestCase> suiteTestCases = _testSuitesCustomWrapper.GetTestCases(planId, suiteId);
            Assert.IsTrue(suiteTestCases.Count > 0, $"Failed to get test cases from test suite.");
        }

        [TestMethod, Ignore]
        public void RemoveTestCasesFromSuiteUrl()
        {
            int planId = 0;
            int suiteId = 0;
            var testCaseIds = new List<int>();

            _testSuitesCustomWrapper.RemoveTestCasesFromSuiteUrl(planId, suiteId, testCaseIds);
        }
    }
}
