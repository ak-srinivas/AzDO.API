using AzDO.API.Wrappers.TestPlan.TestSuites;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.TestPlan.TestSuites
{
    [TestClass]
    public class GetTestSuitesTests : TestBase
    {
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public GetTestSuitesTests()
        {
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
        }

        [TestMethod]
        public void GetTestSuiteById()
        {
            string project = ProjectNames.YourProjectName;
            int planId = 104912;
            int suiteId = 104916;
            SuiteExpand expand = SuiteExpand.Children;

            TestSuite testSuite = _testSuitesCustomWrapper.GetTestSuiteById(project, planId, suiteId, expand);
            Assert.IsTrue(testSuite != null, $"Failed to get test suite.");
        }

        [TestMethod]
        public void GetSuitesByTestCaseId()
        {
            int testCaseId = 104940;
            List<TestSuite> testSuites = _testSuitesCustomWrapper.GetSuitesByTestCaseId(testCaseId);
            Assert.IsTrue(testSuites != null, $"Failed to get test suites by test case id.");
        }

        [TestMethod]
        public void GetTestSuitesForPlan()
        {
            string project = ProjectNames.YourProjectName;
            int planId = 121077;
            SuiteExpand expand = SuiteExpand.Children;
            string continuationToken = null;
            bool? asTreeView = null;

            List<TestSuite> testSuites = _testSuitesCustomWrapper.GetTestSuitesForPlan(project, planId, expand, continuationToken, asTreeView);
            Assert.IsTrue(testSuites != null, $"Failed to get test suites for test plan id '{planId}'.");
        }

        [TestMethod]
        public void GetTestSuiteByNameWithinTestPlan()
        {
            string project = ProjectNames.YourProjectName;
            int planId = 121077;
            string suiteName = "Sprint 2022.07";

            TestSuite testSuite = _testSuitesCustomWrapper.GetTestSuiteByNameWithinTestPlan(project, planId, suiteName);
            Assert.IsTrue(testSuite != null, $"Test suite with name '{suiteName}' does not exists in test plan with id '{planId}'.");
        }
    }
}
