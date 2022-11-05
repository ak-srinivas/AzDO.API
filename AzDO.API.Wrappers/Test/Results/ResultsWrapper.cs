using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Test.Results
{
    public abstract class ResultsWrapper : WrappersBase
    {
        /// <summary>
        /// Get test results for a test run.
        /// </summary>
        /// <param name="runId">Test run ID of test results to fetch.</param>
        /// <param name="detailsToInclude">Details to include with test results. Default is None. Other values are Iterations and WorkItems.</param>
        /// <param name="skip">Number of test results to skip from beginning.</param>
        /// <param name="top">Number of test results to return. Maximum is 1000 when detailsToInclude is None and 200 otherwise.</param>
        /// <param name="outcomes">Comma separated list of test outcomes to filter test results.</param>
        /// <returns>List of test results for a test run.</returns>
        public List<TestCaseResult> GetTestResults(int runId, ResultDetails detailsToInclude = ResultDetails.Iterations | ResultDetails.SubResults | ResultDetails.Point | ResultDetails.WorkItems, int? skip = null, int? top = null, IEnumerable<TestOutcome> outcomes = null)
        {
            return TestManagementClient.GetTestResultsAsync(GetProjectName(), runId, detailsToInclude, skip, top, outcomes).Result;
        }
    }
}
