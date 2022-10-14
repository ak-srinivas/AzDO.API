using AzDO.API.Base.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.TestPlan.TestPoint
{
    public abstract class TestPointWrapper : WrappersBase
    {
        /// <summary>
        /// Get all the points inside a suite based on some filters
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="planId">ID of the test plan for which test points are requested.</param>
        /// <param name="suiteId">ID of the test suite for which test points are requested</param>
        /// <param name="testPointIds">ID of test points to fetch.</param>
        /// <param name="testCaseId">Get Test Points for specific test case Ids.</param>
        /// <param name="continuationToken">If the list of test points returned is not complete, a continuation token to query next batch of test points is included in the response header as "x-ms-continuationtoken".<br/>Omit this parameter to get the first batch of test plans.</param>
        /// <param name="returnIdentityRef">If set to true, returns the AssignedTo field in TestCaseReference as IdentityRef object.</param>
        /// <param name="includePointDetails">If set to false, will get a smaller payload containing only basic details about the test point object.</param>
        /// <param name="isRecursive">If set to true, will also fetch test points belonging to child suites recursively.</param>
        /// <returns>All the points inside a suite based on some filters</returns>
        public PagedList<Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPoint> GetPointsList(string project, int planId, int suiteId, string testPointIds = null, string testCaseId = null, string continuationToken = null, bool returnIdentityRef = true, bool includePointDetails = true, bool isRecursive = true)
        {
            return TestPlanClient.GetPointsListAsync(project, planId, suiteId, testPointIds, testCaseId, continuationToken, returnIdentityRef, includePointDetails, isRecursive).Result;
        }

        /// <summary>
        /// Get a list of points based on point Ids provided.
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="planId">ID of the test plan for which test points are requested.</param>
        /// <param name="suiteId">ID of the test suite for which test points are requested.</param>
        /// <param name="pointIds">ID of test points to be fetched.</param>
        /// <param name="returnIdentityRef">If set to true, returns the AssignedTo field in TestCaseReference as IdentityRef object.</param>
        /// <param name="includePointDetails">If set to false, will get a smaller payload containing only basic details about the test point object.</param>
        /// <returns>A list of points based on point Ids provided.</returns>
        public List<Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestPoint> GetPoints(string project, int planId, int suiteId, string pointIds, bool returnIdentityRef = true, bool includePointDetails = true)
        {
            return TestPlanClient.GetPointsAsync(project, planId, suiteId, pointIds, returnIdentityRef, includePointDetails).Result;
        }
    }
}
