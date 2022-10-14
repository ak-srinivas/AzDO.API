using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Test.Runs
{
    public abstract class RunsWrapper : WrappersBase
    {
        // Under API Version: 6.1 Preview

        /// <summary>
        /// Create new test run.
        /// </summary>
        /// <param name="testRun">Run details</param>
        /// <returns></returns>
        public TestRun CreateTestRun(RunCreateModel testRun)
        {
            return TestManagementClient.CreateTestRunAsync(testRun, GetProjectName()).Result;
        }

        /// <summary>
        /// Get a test run by its ID.
        /// </summary>
        /// <param name="runId">ID of the run to get.</param>
        /// <param name="includeDetails">Default value is true. It includes details like run statistics, release, build, test environment, post process state, and more.</param>
        /// <returns></returns>
        public TestRun GetTestRunById(int runId, bool? includeDetails = null)
        {
            return TestManagementClient.GetTestRunByIdAsync(GetProjectName(), runId, includeDetails).Result;
        }

        /// <summary>
        /// Get test run statistics , used when we want to get summary of a run by outcome.
        /// </summary>
        /// <param name="runId">ID of the run to get.</param>
        /// <returns></returns>
        public TestRunStatistic GetTestRunStatistics(int runId)
        {
            return TestManagementClient.GetTestRunStatisticsAsync(GetProjectName(), runId).Result;
        }

        /// <summary>
        /// Get a list of test runs.
        /// </summary>
        /// <param name="buildUri">URI of the build that the runs used.</param>
        /// <param name="owner">Team foundation ID of the owner of the runs.</param>
        /// <param name="tmiRunId"></param>
        /// <param name="planId">ID of the test plan that the runs are a part of.</param>
        /// <param name="includeRunDetails">If true, include all the properties of the runs.</param>
        /// <param name="automated">If true, only returns automated runs.</param>
        /// <param name="skip">Number of test runs to skip.</param>
        /// <param name="top">Number of test runs to return.</param>
        /// <returns></returns>
        public List<TestRun> ListTestRuns(string buildUri = null, string owner = null, string tmiRunId = null, int? planId = null, bool? includeRunDetails = null, bool? automated = null, int? skip = null, int? top = null)
        {
            return TestManagementClient.GetTestRunsAsync(GetProjectName(), buildUri, owner, tmiRunId, planId, includeRunDetails, automated, skip, top).Result;
        }

        /// <summary>
        /// Query Test Runs based on filters. Mandatory fields are minLastUpdatedDate and maxLastUpdatedDate.
        /// </summary>
        /// <param name="minLastUpdatedDate"> Minimum Last Modified Date of run to be queried (Mandatory).</param>
        /// <param name="maxLastUpdatedDate">Maximum Last Modified Date of run to be queried (Mandatory, difference between min and max date can be atmost 7 days).</param>
        /// <param name="state">Current state of the Runs to be queried.</param>
        /// <param name="planIds"> Plan Ids of the Runs to be queried, comma separated list of valid ids (limit no. of ids 10).</param>
        /// <param name="isAutomated">Automation type of the Runs to be queried.</param>
        /// <param name="publishContext">PublishContext of the Runs to be queried.</param>
        /// <param name="buildIds">Build Ids of the Runs to be queried, comma separated list of valid ids (limit no. of ids 10).</param>
        /// <param name="buildDefIds">Build Definition Ids of the Runs to be queried, comma separated list of valid ids (limit no. of ids 10).</param>
        /// <param name="branchName">Source Branch name of the Runs to be queried.</param>
        /// <param name="releaseIds">Release Ids of the Runs to be queried, comma separated list of valid ids (limit no. of ids 10).</param>
        /// <param name="releaseDefIds">Release Definition Ids of the Runs to be queried, comma separated list of valid ids (limit no. of ids 10).</param>
        /// <param name="releaseEnvIds">Release Environment Ids of the Runs to be queried, comma separated list of valid ids (limit no. of ids 10).</param>
        /// <param name="releaseEnvDefIds">Release Environment Definition Ids of the Runs to be queried, comma separated list of valid ids (limit no. of ids 10).</param>
        /// <param name="runTitle">Run Title of the Runs to be queried.</param>
        /// <param name="top">Number of runs to be queried. Limit is 100.</param>
        /// <param name="continuationToken">continuationToken received from previous batch or null for first batch. <br/>
        /// It is not supposed to be created (or altered, if received from last batch) by user.</param>
        public PagedList<TestRun> QueryTestRuns(
            DateTime minLastUpdatedDate,
            DateTime maxLastUpdatedDate,
            TestRunState? state = null,
            IEnumerable<int> planIds = null,
            bool? isAutomated = null,
            TestRunPublishContext? publishContext = null,
            IEnumerable<int> buildIds = null,
            IEnumerable<int> buildDefIds = null,
            string branchName = null,
            IEnumerable<int> releaseIds = null,
            IEnumerable<int> releaseDefIds = null,
            IEnumerable<int> releaseEnvIds = null,
            IEnumerable<int> releaseEnvDefIds = null,
            string runTitle = null,
            int? top = null,
            string continuationToken = null)
        {
            return TestManagementClient.QueryTestRunsAsync(
                GetProjectName(),
                minLastUpdatedDate,
                maxLastUpdatedDate,
                state,
                planIds,
                isAutomated,
                publishContext,
                buildIds,
                buildDefIds,
                branchName,
                releaseIds,
                releaseDefIds,
                releaseEnvIds,
                releaseEnvDefIds,
                runTitle,
                top,
                continuationToken).Result;
        }

        /// <summary>
        /// Update test run by its ID.
        /// </summary>
        /// <param name="runUpdateModel">Run details</param>
        /// <param name="runId">ID of the run to update.</param>
        /// <returns></returns>
        public TestRun UpdateTestRun(RunUpdateModel runUpdateModel, int runId)
        {
            return TestManagementClient.UpdateTestRunAsync(runUpdateModel, GetProjectName(), runId).Result;
        }
    }
}
