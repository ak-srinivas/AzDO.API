using AzDO.API.Wrappers.Build.Latest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzDO.API.Tests.Build.Latest
{
    [TestClass]
    public class GetLatestTests : TestBase
    {
        private readonly LatestCustomWrapper _latestCustomWrapper;

        public GetLatestTests()
        {
            _latestCustomWrapper = new LatestCustomWrapper();
        }

        [TestMethod]
        public void GetLatestBuild()
        {
            string definition = "22";
            string branchName = "develop";

            Microsoft.TeamFoundation.Build.WebApi.Build buildInfo = _latestCustomWrapper.GetLatestBuild(definition, branchName);
            Assert.IsTrue(buildInfo != null, $"The latest build information was not found for definition '{definition}'");
        }

    }
}
