using AzDO.API.Wrappers.Test.TestSuites;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.Test.TestSuites
{
    [TestClass]
    public class CreateTestSuitesTests : TestBase
    {
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public CreateTestSuitesTests()
        {
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
        }

        [TestMethod, Ignore]
        public void AddTestCasesToSuite()
        {
            int planId = 308875; // [Created-Using-RestAPI] Sample Test Plan (ID: 308876)
            int suiteId = 341893;
            var testCaseIds = new List<int> { 321900 };

            List<SuiteTestCase> suiteTestCases = _testSuitesCustomWrapper.AddTestCasesToSuite(planId, suiteId, testCaseIds);
            Assert.IsTrue(suiteTestCases.Count > 0, $"Failed to add test cases to test suite with id '{suiteId}'.");
        }
    }
}
