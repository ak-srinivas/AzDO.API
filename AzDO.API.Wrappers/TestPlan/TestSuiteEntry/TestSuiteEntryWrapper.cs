using AzDO.API.Base.Common;
using System.Collections.Generic;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;

namespace AzDO.API.Wrappers.TestPlan.TestSuiteEntry
{
    public abstract class TestSuiteEntryWrapper : WrappersBase
    {
        /// <summary>
        /// Get a list of test suite entries in the test suite.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="suiteId">Id of the parent suite.</param>
        /// <param name="suiteEntryType"></param>
        /// <returns></returns>
        public List<SuiteEntry> GetSuiteEntries(string project, int suiteId, SuiteEntryTypes? suiteEntryType = null)
        {
            return TestPlanClient.GetSuiteEntriesAsync(project, suiteId, suiteEntryType).Result;
        }

        /// <summary>
        /// Reorder test suite entries in the test suite.
        /// </summary>
        /// <param name="suiteEntries">List of Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.SuiteEntry to reorder.</param>
        /// <param name="suiteId">Id of the parent test suite.</param>
        /// <returns></returns>
        public List<SuiteEntry> ReorderSuiteEntries(IEnumerable<SuiteEntryUpdateParams> suiteEntries, int suiteId)
        {
            return TestPlanClient.ReorderSuiteEntriesAsync(suiteEntries, GetProjectName(), suiteId).Result;
        }
    }
}
