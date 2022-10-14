using AzDO.API.Wrappers.TestPlan.TestSuites;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzDO.API.Tests.TestPlan.TestSuites
{
    [TestClass]
    public class UpdateTestSuitesTests : TestBase
    {
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public UpdateTestSuitesTests()
        {
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
        }

        [TestMethod]
        public void UpdateTestSuite()
        {
            string project = ProjectNames.YourProjectName;
            int suiteId = 123141; // Sprint 2022.04

            string sprintName = "Sprint 2022.04";
            TestSuiteUpdateParams testSuiteUpdateParams = new TestSuiteUpdateParams()
            {
                Name = sprintName,
                InheritDefaultConfigurations = true,
                ParentSuite = new TestSuiteReference()
                {
                    Id = DefaultTestSuiteId,
                    Name = TestPlanName
                },
            };

            TestSuite testSuite = _testSuitesCustomWrapper.UpdateTestSuite(project, testSuiteUpdateParams, TestPlanId, suiteId);
            Assert.IsTrue(testSuite != null, $"Failed to create test suite.");
        }
    }
}
