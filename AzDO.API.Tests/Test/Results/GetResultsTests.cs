using AzDO.API.Wrappers.Test.Results;
using AzDO.API.Wrappers.Test.Runs;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Tests.Test.Results
{
    [TestClass]
    public class GetResultsTests : TestBase
    {
        private readonly ResultsCustomWrapper _resultsCustomWrapper;

        public GetResultsTests()
        {
            _resultsCustomWrapper = new ResultsCustomWrapper();
        }

        [TestMethod]
        public void GetTestResults()
        {
            int runId = 166;
            ResultDetails detailsToInclude = ResultDetails.Iterations | ResultDetails.SubResults | ResultDetails.Point | ResultDetails.WorkItems;
            int? skip = null;
            int? top = 200;
            IEnumerable<TestOutcome> outcomes = null;

            List<TestCaseResult> testCaseResults = _resultsCustomWrapper.GetTestResults(runId, detailsToInclude, skip, top, outcomes);
            Assert.IsTrue(testCaseResults != null, $"Unable to fetch test results by run id.");
        }
    }
}
