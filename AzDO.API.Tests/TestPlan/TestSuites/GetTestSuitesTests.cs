using AzDO.API.Wrappers.TestPlan.TestSuites;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SuiteExpand = Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.SuiteExpand;
using TestSuite = Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestSuite;

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
            string project = ProjectNames.Ploceus;
            int planId = 104912;
            int suiteId = 104916;
            SuiteExpand expand = SuiteExpand.Children;

            TestSuite testSuite = _testSuitesCustomWrapper.GetTestSuiteById(project, planId, suiteId, expand);
            Assert.IsTrue(testSuite != null, $"Failed to get test suite.");
        }

        [TestMethod]
        public void GetSuitesByTestCaseId()
        {
            int testCaseId = 1005;
            List<TestSuite> testSuites = _testSuitesCustomWrapper.GetSuitesByTestCaseId(testCaseId);
            Assert.IsTrue(testSuites != null, $"Failed to get test suites by test case id.");
        }

        [TestMethod]
        public void GetTestCaseSuiteHierarchy()
        {
            string project = ProjectNames.Ploceus;
            int testPlanId = 53;
            int testCaseId = 526;
            string path = null;

            List<TestSuite> testCaseSuites = _testSuitesCustomWrapper.GetSuitesByTestCaseId(testCaseId);
            TestSuite testCaseSuite = testCaseSuites[0];
            GetPath(project, testPlanId, testCaseSuite, ref path);
            Console.WriteLine($"Path is: {path}");

            Assert.IsTrue(testCaseSuites != null, $"Failed to get test suites by test case id.");
        }

        private string GetPath(string project, int testPlanId, TestSuite parentTestSuite, ref string path)
        {
            if (parentTestSuite.Id - 1 == testPlanId)
            {
                path = path + $" --> {parentTestSuite.Name}";
                return null;
            }

            if (string.IsNullOrEmpty(path))
                path = $"{parentTestSuite.Name}";
            else
                path = path + $" --> {parentTestSuite.Name}";

            parentTestSuite = _testSuitesCustomWrapper.GetTestSuiteByNameWithinTestPlan(project, testPlanId, parentTestSuite.ParentSuite.Name);

            GetPath(project, testPlanId, parentTestSuite, ref path);
            return path;
        }

        [TestMethod]
        public void GetTestSuitesForPlan()
        {
            string project = ProjectNames.Ploceus;
            int planId = 53;
            SuiteExpand expand = SuiteExpand.Children;
            string continuationToken = null;
            bool? asTreeView = null;

            List<TestSuite> testSuites = _testSuitesCustomWrapper.GetTestSuitesForPlan(project, planId, expand, continuationToken, asTreeView);


            Assert.IsTrue(testSuites != null, $"Failed to get test suites for test plan id '{planId}'.");
        }

        [TestMethod]
        public void GetTestSuiteByNameWithinTestPlan()
        {
            string project = ProjectNames.Ploceus;
            int planId = 121077;
            string suiteName = "Sprint 2022.07";

            TestSuite testSuite = _testSuitesCustomWrapper.GetTestSuiteByNameWithinTestPlan(project, planId, suiteName);
            Assert.IsTrue(testSuite != null, $"Test suite with name '{suiteName}' does not exists in test plan with id '{planId}'.");
        }
    }
}
