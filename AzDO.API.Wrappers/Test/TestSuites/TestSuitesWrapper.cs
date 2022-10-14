using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Test.TestSuites
{
    public abstract class TestSuitesWrapper : WrappersBase
    {
        /// <summary>
        /// Add test cases to suite.
        /// </summary>
        /// <param name="planId">ID of the test plan that contains the suite.</param>
        /// <param name="suiteId">ID of the test suite to which the test cases must be added.</param>
        /// <param name="testCaseIds">IDs of the test cases to add to the suite. </param>
        public List<SuiteTestCase> AddTestCasesToSuite(int planId, int suiteId, List<int> testCaseIds)
        {
            string commaSeperatedTestCaseIds = string.Join(",", testCaseIds);
            return TestManagementClient.AddTestCasesToSuiteAsync(GetProjectName(), planId, suiteId, commaSeperatedTestCaseIds).Result;
        }

        /// <summary>
        /// Get a specific test case in a test suite with test case id.
        /// </summary>
        /// <param name="planId">ID of the test plan that contains the suites.</param>
        /// <param name="suiteId">ID of the suite that contains the test case.</param>
        /// <param name="testCaseId">ID of the test case to get.</param>
        public SuiteTestCase GetTestCaseById(int planId, int suiteId, int testCaseId)
        {
            return TestManagementClient.GetTestCaseByIdAsync(GetProjectName(), planId, suiteId, testCaseId).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planId">ID of the test plan that contains the suites.</param>
        /// <param name="suiteId">ID of the suite to get.</param>
        public List<SuiteTestCase> GetTestCases(int planId, int suiteId)
        {
            return TestManagementClient.GetTestCasesAsync(GetProjectName(), planId, suiteId).Result;
        }

        /// <summary>
        /// The test points associated with the test cases are removed from the test suite. <br/>
        /// The test case work item is not deleted from the system. See test cases resource to delete a test case permanently.
        /// </summary>
        /// <param name="planId">ID of the test plan that contains the suite.</param>
        /// <param name="suiteId">ID of the suite to get.</param>
        /// <param name="testCaseIds">IDs of the test cases to remove from the suite.</param>
        public void RemoveTestCasesFromSuiteUrl(int planId, int suiteId, List<int> testCaseIds)
        {
            string commaSeperatedTestCaseIds = string.Join(",", testCaseIds);
            TestManagementClient.RemoveTestCasesFromSuiteUrlAsync(GetProjectName(), planId, suiteId, commaSeperatedTestCaseIds).Wait();
        }

        /// <summary>
        /// Updates the properties of the test case association in a suite.
        /// </summary>
        /// <param name="suiteTestCaseUpdateModel">Model for updation of the properties of test case suite association.</param>
        /// <param name="planId">ID of the test plan that contains the suite.</param>
        /// <param name="suiteId">ID of the test suite to which the test cases must be added.</param>
        /// <param name="testCaseIds">IDs of the test cases to add to the suite.</param>
        /// <returns></returns>
        public List<SuiteTestCase> UpdateSuiteTestCasesAsync(SuiteTestCaseUpdateModel suiteTestCaseUpdateModel, int planId, int suiteId, List<int> testCaseIds)
        {
            string commaSeperatedTestCaseIds = string.Join(",", testCaseIds);
            return TestManagementClient.UpdateSuiteTestCasesAsync(suiteTestCaseUpdateModel, GetProjectName(), planId, suiteId, commaSeperatedTestCaseIds).Result;
        }
    }
}
