using AzDO.API.Wrappers.Test.Runs;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AzDO.API.Tests.Test.Runs
{
    [TestClass]
    public class GetRunsTests : TestBase
    {
        private readonly RunsCustomWrapper _runsCustomWrapper;

        public GetRunsTests()
        {
            _runsCustomWrapper = new RunsCustomWrapper();
        }

        [TestMethod, Ignore]
        public void GetTestRunById()
        {
            int runId = 0;
            bool? includeDetails = null;

            TestRun testRun = _runsCustomWrapper.GetTestRunById(runId, includeDetails);
            Assert.IsTrue(testRun != null, $"Unable to fetch test run by id.");
        }

        [TestMethod, Ignore]
        public void GetTestRunStatistics()
        {
            int runId = 0;

            TestRunStatistic testRunStatistic = _runsCustomWrapper.GetTestRunStatistics(runId);
            Assert.IsTrue(testRunStatistic != null, $"Failed to fetch test run statistics by run id.");
        }

        [TestMethod, Ignore]
        public void ListTestRuns()
        {
            string buildUri = null;
            string owner = null;
            string tmiRunId = null;
            int? planId = null;
            bool? includeRunDetails = null;
            bool? automated = null;
            int? skip = null;
            int? top = null;

            List<TestRun> testRuns = _runsCustomWrapper.ListTestRuns(buildUri, owner, tmiRunId, planId, includeRunDetails, automated, skip, top);
            Assert.IsTrue(testRuns.Count > 0, $"Failed to fetch test runs.");
        }

        [TestMethod, Ignore]
        public void QueryTestRuns()
        {
            DateTime minLastUpdatedDate = new DateTime();
            DateTime maxLastUpdatedDate = new DateTime();
            TestRunState? state = null;
            IEnumerable<int> planIds = null;
            bool? isAutomated = null;
            TestRunPublishContext? publishContext = null;
            IEnumerable<int> buildIds = null;
            IEnumerable<int> buildDefIds = null;
            string branchName = null;
            IEnumerable<int> releaseIds = null;
            IEnumerable<int> releaseDefIds = null;
            IEnumerable<int> releaseEnvIds = null;
            IEnumerable<int> releaseEnvDefIds = null;
            string runTitle = null;
            int? top = null;
            string continuationToken = null;

            PagedList<TestRun> testRuns = _runsCustomWrapper.QueryTestRuns(
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
                continuationToken);

            Assert.IsTrue(testRuns.Count > 0, $"No test runs were found with the given query.");
        }
    }
}
