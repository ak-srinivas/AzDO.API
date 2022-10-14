using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Linq;

namespace AzDO.API.Wrappers.TestPlan.TestSuites
{
    public sealed class TestSuitesCustomWrapper : TestSuitesWrapper
    {
        /// <summary>
        /// Finds the test suite by it's name and returns the TestSuite object
        /// </summary>
        public TestSuite GetTestSuiteByNameWithinTestPlan(string project, int planId, string testSuiteName)
        {
            string continuationToken = null;
            TestSuite testSuite = null;

            do
            {
                PagedList<TestSuite> testSuites = GetTestSuitesForPlan(project, planId, expand: SuiteExpand.Children, continuationToken, asTreeView: false);
                continuationToken = testSuites.ContinuationToken;
                testSuite = testSuites.Where(item => item.Name.Equals(testSuiteName)).Select(item => item).FirstOrDefault();
            } while (testSuite == null && continuationToken != null);

            return testSuite;
        }
    }
}
