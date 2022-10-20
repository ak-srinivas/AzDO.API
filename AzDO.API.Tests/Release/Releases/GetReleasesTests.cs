using AzDO.API.Wrappers.Release.Releases;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.Release.Releases
{
    [TestClass]
    public class GetReleasesTests : TestBase
    {
        private readonly ReleasesCustomWrapper _releasesCustomWrapper;

        public GetReleasesTests()
        {
            _releasesCustomWrapper = new ReleasesCustomWrapper();
        }

        [TestMethod]
        public void GetRelease()
        {
            int releaseId = 14022;
            ApprovalFilters? approvalFilters = null;
            IEnumerable<string> propertyFilters = null;
            Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts.SingleReleaseExpands? expand = null;
            int? topGateRecords = null;

            Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release release = _releasesCustomWrapper.GetRelease(releaseId, approvalFilters, propertyFilters, expand, topGateRecords);
            Assert.IsTrue(release.Id == releaseId, "Wrong release information was retrieved");
        }

        [TestMethod]
        public void GetApplicationBuilds()
        {

        }
    }
}
