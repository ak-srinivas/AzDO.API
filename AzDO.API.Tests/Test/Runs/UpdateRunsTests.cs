using AzDO.API.Wrappers.Test.Runs;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Tests.Test.Runs
{
    [TestClass]
    public class UpdateRunsTests : TestBase
    {
        private readonly RunsCustomWrapper _runsCustomWrapper;

        public UpdateRunsTests()
        {
            _runsCustomWrapper = new RunsCustomWrapper();
        }

        [TestMethod, Ignore]
        public void UpdateTestRun()
        {
            RunUpdateModel runUpdateModel = null;
            int runId = 0;

            TestRun testRun = _runsCustomWrapper.UpdateTestRun(runUpdateModel, runId);
            Assert.IsTrue(testRun.Id.Equals(runId), $"Failed to update test run with id as '{runId}'.");
        }
    }
}
