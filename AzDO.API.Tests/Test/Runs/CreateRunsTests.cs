using AzDO.API.Wrappers.Test.Runs;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzDO.API.Tests.Test.Runs
{
    [TestClass]
    public class CreateRunsTests : TestBase
    {
        private readonly RunsCustomWrapper _runsCustomWrapper;

        public CreateRunsTests()
        {
            _runsCustomWrapper = new RunsCustomWrapper();
        }

        [TestMethod, Ignore]
        public void CreateTestRun()
        {
            RunCreateModel createModel = null;

            TestRun testRun = _runsCustomWrapper.CreateTestRun(createModel);
            Assert.IsTrue(testRun != null, $"Failed to create a new test run.");
        }
    }
}
