using AzDO.API.Base.Common;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.TestPlan.TestSuites
{
    public abstract class TestSuitesWrapper : WrappersBase
    {
        /// <summary>
        /// Get test suite by suite id.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="planId">ID of the test plan that contains the suites.</param>
        /// <param name="suiteId">ID of the suite to get.</param>
        /// <param name="expand">Include the children suites and testers details.</param>
        /// <returns></returns>
        public TestSuite GetTestSuiteById(string project, int planId, int suiteId, SuiteExpand expand = SuiteExpand.DefaultTesters)
        {
            return TestPlanClient.GetTestSuiteByIdAsync(project, planId, suiteId, expand).Result;
        }

        /// <summary>
        /// Find the list of all test suites in which a given test case is present. <br/>
        /// This is helpful if you need to find out which test suites are using a test case, when you need to make changes to a test case.
        /// </summary>
        /// <param name="testCaseId">ID of the test case for which suites need to be fetched.</param>
        public List<TestSuite> GetSuitesByTestCaseId(int testCaseId)
        {
            return TestPlanClient.GetSuitesByTestCaseIdAsync(testCaseId).Result;
        }

        /// <summary>
        /// Get test suites for plan.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="planId">ID of the test plan for which suites are requested.</param>
        /// <param name="expand">Include the children suites and testers details.</param>
        /// <param name="continuationToken">If the list of suites returned is not complete, a continuation token to query next batch of suites is included in the response header as "x-ms-continuationtoken".<br/>Omit this parameter to get the first batch of test plans.</param>
        /// <param name="asTreeView">If the suites returned should be in a tree structure.</param>
        /// <returns>Test suites for plan.</returns>
        public PagedList<TestSuite> GetTestSuitesForPlan(string project, int planId, SuiteExpand expand = SuiteExpand.Children, string continuationToken = null, bool? asTreeView = null)
        {
            return TestPlanClient.GetTestSuitesForPlanAsync(project, planId, expand, continuationToken, asTreeView).Result;
        }

        /// <summary>
        /// Create test suite.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="testSuiteCreateParams">Parameters for suite creation</param>
        /// <param name="planId">ID of the test plan that contains the suites.</param>
        /// <returns>Created test suite information</returns>
        public TestSuite CreateTestSuite(string project, TestSuiteCreateParams testSuiteCreateParams, int planId)
        {
            return TestPlanClient.CreateTestSuiteAsync(testSuiteCreateParams, project, planId).Result;
        }

        /// <summary>
        /// Update test suite.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="testSuiteUpdateParams">Parameters for suite updation</param>
        /// <param name="planId">ID of the test plan that contains the suites.</param>
        /// <param name="suiteId">ID of the parent suite.</param>
        /// <returns>Updated test suite information</returns>
        public TestSuite UpdateTestSuite(string project, TestSuiteUpdateParams testSuiteUpdateParams, int planId, int suiteId)
        {
            return TestPlanClient.UpdateTestSuiteAsync(testSuiteUpdateParams, project, planId, suiteId).Result;
        }
    }
}
