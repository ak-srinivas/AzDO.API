using AzDO.API.Wrappers.TestPlan.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.TestPlan.Configurations
{
    [TestClass]
    public class GetConfigurationsTests : TestBase
    {
        private readonly ConfigurationsCustomWrapper _configurationsCustomWrapper;

        public GetConfigurationsTests()
        {
            _configurationsCustomWrapper = new ConfigurationsCustomWrapper();
        }

        [TestMethod]
        public void GetTestConfigurations()
        {
            string project = ProjectNames.YourProjectName;
            string continuationToken = null;

            List<Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestConfiguration> testConfigurations = _configurationsCustomWrapper.GetTestConfigurations(project, continuationToken);
            Assert.IsTrue(testConfigurations != null, $"Failed to get test configurations.");
        }

        [TestMethod]
        public void GetTestConfigurationById()
        {
            string project = ProjectNames.YourProjectName;
            int testConfigurationId = 2;

            Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestConfiguration testConfiguration = _configurationsCustomWrapper.GetTestConfigurationById(project, testConfigurationId);
            Assert.IsTrue(testConfiguration != null, $"Failed to get test configuration.");
        }
    }
}
