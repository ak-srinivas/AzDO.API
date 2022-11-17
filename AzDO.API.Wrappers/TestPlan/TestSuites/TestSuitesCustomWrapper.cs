using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.TestPlan.TestSuites
{
    public sealed class TestSuitesCustomWrapper : TestSuitesWrapper
    {
        /// <summary>
        /// Finds the test suite by it's name and returns the TestSuite object
        /// </summary>
        public TestSuite GetTestSuiteByNameWithinTestPlan(string project, int planId, string testSuiteName, SuiteExpand expand = SuiteExpand.Children)
        {
            string continuationToken = null;
            TestSuite testSuite = null;
            do
            {
                PagedList<TestSuite> testSuites = GetTestSuitesForPlan(project, planId, expand, continuationToken, asTreeView: false);
                continuationToken = testSuites.ContinuationToken;
                testSuite = testSuites.Where(item => item.Name.Equals(testSuiteName)).Select(item => item).FirstOrDefault();
            } while (testSuite == null && continuationToken != null);

            return testSuite;
        }

        public List<TestSuite> GetTestSuitesWithinTestSuite(int planId, int suiteId, SuiteExpand expand = SuiteExpand.Children)
        {
            string continuationToken = null;
            PagedList<TestSuite> testSuites = null;

            do
            {
                testSuites = GetTestSuitesForPlan(GetProjectName(), planId, expand, continuationToken, asTreeView: false);
                foreach (var item in testSuites)
                {
                    if (item.Id == planId + 1)
                    {
                        testSuites.Remove(item);
                        break;
                    }
                }
                continuationToken = testSuites.ContinuationToken;
            } while (testSuites == null && continuationToken != null);

            List<TestSuite> allSuites = testSuites.Where(item => item.ParentSuite.Id.Equals(suiteId)).OrderBy(item => item.Name).ToList();
            return allSuites;
        }
    }
}
