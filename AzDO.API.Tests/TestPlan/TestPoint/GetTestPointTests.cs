using AzDO.API.Wrappers.TestPlan.TestPoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.TestPlan.TestPoint
{
    [TestClass]
    public class GetTestPointTests : TestBase
    {
        private readonly TestPointCustomWrapper _testPointCustomWrapper;

        public GetTestPointTests()
        {
            _testPointCustomWrapper = new TestPointCustomWrapper();
        }

        [TestMethod]
        public void GetPointsList()
        {
            string project = ProjectNames.Ploceus;
            int planId = 104912;
            int suiteId = 104916;
            string testPointIds = null;
            string testCaseId = null;
            string continuationToken = null;
            bool returnIdentityRef = true;
            bool includePointDetails = true;
            bool isRecursive = true;

            List<Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPoint> testPoints = _testPointCustomWrapper.GetPointsList(project, planId, suiteId, testPointIds, testCaseId, continuationToken, returnIdentityRef, includePointDetails, isRecursive);
            Assert.IsTrue(testPoints != null, $"Failed to get test points.");
        }

        [TestMethod]
        public void GetPoints()
        {
            string project = ProjectNames.Ploceus;
            int planId = 0;
            int suiteId = 0;
            string pointIds = "";
            bool returnIdentityRef = true;
            bool includePointDetails = true;

            List<Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPoint> testPoints = _testPointCustomWrapper.GetPoints(project, planId, suiteId, pointIds, returnIdentityRef, includePointDetails);
            Assert.IsTrue(testPoints != null, $"Failed to get test points.");
        }
    }
}
