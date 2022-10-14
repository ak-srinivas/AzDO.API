using AzDO.API.Base.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;

namespace AzDO.API.Wrappers.TestPlan.Configurations
{
    public abstract class ConfigurationsWrapper : WrappersBase
    {
        /// <summary>
        /// Get a list of test configurations.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="continuationToken">If the list of configurations returned is not complete, a continuation token to query next batch of configurations is included in the response header as "x-ms-continuationtoken". Omit this parameter to get the first batch of test configurations.</param>
        /// <returns>VA list of test configurations.</returns>
        public PagedList<TestConfiguration> GetTestConfigurations(string project, string continuationToken = null)
        {
            return TestPlanClient.GetTestConfigurationsAsync(project, continuationToken).Result;
        }

        /// <summary>
        /// Get a test configuration
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="testConfigurationId">ID of the test configuration to get.</param>
        /// <returns>A test configuration.</returns>
        public TestConfiguration GetTestConfigurationById(string project, int testConfigurationId)
        {
            return TestPlanClient.GetTestConfigurationByIdAsync(project, testConfigurationId).Result;
        }
    }
}
