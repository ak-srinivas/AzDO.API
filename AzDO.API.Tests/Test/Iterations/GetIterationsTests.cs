using AzDO.API.Wrappers.Test.Iterations;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.Test.Iterations
{
    [TestClass]
    public class GetIterationsTests : TestBase
    {
        private readonly IterationsCustomWrapper _iterationsCustomWrapper;

        public GetIterationsTests()
        {
            _iterationsCustomWrapper = new IterationsCustomWrapper();
        }


        [TestMethod, Ignore]
        public void GetTestIteration()
        {
            int runId = 2092532;
            int testCaseResultId = 100002;
            int iterationId = 0;
            bool? includeActionResults = null;

            TestIterationDetailsModel testIterationDetailsModel = _iterationsCustomWrapper.GetTestIteration(runId, testCaseResultId, iterationId, includeActionResults);
            Assert.IsTrue(testIterationDetailsModel.Id.Equals(runId), $"Test iteration information was found for run id '{runId}'.");
        }

        [TestMethod, Ignore]
        public void ListTestIterations()
        {
            int runId = 0;
            int testCaseResultId = 0;
            bool? includeActionResults = null;

            List<TestIterationDetailsModel> testIterationDetailsModels = _iterationsCustomWrapper.ListTestIterations(runId, testCaseResultId, includeActionResults);
            Assert.IsTrue(testIterationDetailsModels.Count > 0, $"No test iteration were found for run id '{runId}'.");
        }
    }
}
