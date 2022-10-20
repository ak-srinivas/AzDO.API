using AzDO.API.Wrappers.TestPlan.TestPlans;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.TestPlan.TestPlans
{
    [TestClass]
    public class GetTestPlansTests : TestBase
    {
        private readonly TestPlansCustomWrapper _testPlansCustomWrapper;

        public GetTestPlansTests()
        {
            _testPlansCustomWrapper = new TestPlansCustomWrapper();
        }

        [TestMethod]
        public void GetTestPlans()
        {
            string project = ProjectNames.Ploceus;
            string owner = null;
            string continuationToken = null;
            bool includePlanDetails = true;
            bool filterActivePlans = false;

            List<Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPlan> testPlans = _testPlansCustomWrapper.GetTestPlans(project, owner, continuationToken, includePlanDetails, filterActivePlans);
            Assert.IsTrue(testPlans != null, $"Failed to get test plans.");
        }

        [TestMethod]
        public void GetTestPlanById()
        {
            string project = ProjectNames.Ploceus;
            int planId = 104912;

            Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPlan testPlan = _testPlansCustomWrapper.GetTestPlanById(project, planId);
            Assert.IsTrue(testPlan != null, $"Failed to get test plan.");
        }
    }
}
