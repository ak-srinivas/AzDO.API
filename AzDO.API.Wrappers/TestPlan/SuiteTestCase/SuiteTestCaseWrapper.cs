using AzDO.API.Base.Common;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.TestPlan.SuiteTestCase
{
    public abstract class SuiteTestCaseWrapper : WrappersBase
    {
        /// <summary>
        /// Get Test Cases For a Suite. <i>This API is broken</i>.
        /// </summary>
        /// <param name="planId">ID of the test plan for which test cases are requested.</param>
        /// <param name="suiteId">ID of the test suite for which test cases are requested.</param>
        /// <param name="testCaseIds">Test Case Ids to be fetched.</param>
        /// <param name="witFields">Get the list of witFields.</param>
        /// <param name="returnIdentityRef">
        /// If set to true, returns all identity fields, like AssignedTo, ActivatedBy etc., as IdentityRef objects.<br/>
        /// If set to false, these fields are returned as unique names in string format. <br/>This is false by default.
        /// </param>
        /// <returns>List of test cases for a suite.</returns>
        public List<TestCase> GetTestCases(int planId, int suiteId, string testCaseIds, bool returnIdentityRef = false, string witFields = "Id")
        {
            return TestPlanClient.GetTestCaseAsync(GetProjectName(), planId, suiteId, testCaseIds, witFields, returnIdentityRef).Result;
        }
    }
}
