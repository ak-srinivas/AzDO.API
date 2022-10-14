using AzDO.API.Base.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace AzDO.API.Wrappers.TestPlan.TestPlans
{
    public abstract class TestPlansWrapper : WrappersBase
    {
        /// <summary>
        /// Get a list of test plans
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="owner">Filter for test plan by owner ID or name</param>
        /// <param name="continuationToken">If the list of plans returned is not complete, a continuation token to query next batch of plans is included in the response header as "x-ms-continuationtoken".<br/>Omit this parameter to get the first batch of test plans.</param>
        /// <param name="includePlanDetails">Get all properties of the test plan</param>
        /// <param name="filterActivePlans">Get just the active plans</param>
        /// <returns>A list of test plans.</returns>
        public PagedList<Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPlan> GetTestPlans(string project, string owner = null, string continuationToken = null, bool includePlanDetails = true, bool filterActivePlans = false)
        {
            return TestPlanClient.GetTestPlansAsync(project, owner, continuationToken, includePlanDetails, filterActivePlans).Result;
        }

        /// <summary>
        /// Get a test plan by Id.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="planId">ID of the test plan to get.</param>
        /// <returns>A test plan by Id.</returns>
        public Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPlan GetTestPlanById(string project, int planId)
        {
            return TestPlanClient.GetTestPlanByIdAsync(project, planId).Result;
        }
    }
}
