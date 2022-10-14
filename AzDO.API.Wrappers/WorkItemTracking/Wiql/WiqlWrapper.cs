using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;

namespace AzDO.API.Wrappers.WorkItemTracking.Wiql
{
    public abstract class WiqlWrapper : WrappersBase
    {
        /// <summary>
        /// Gets the results of the query given the query ID.
        /// </summary>
        /// <param name="id">The query ID.</param>
        /// <param name="timePrecision">Whether or not to use time precision.</param>
        /// <param name="top">The max number of results to return.</param>
        public WorkItemQueryResult QueryById(Guid id, bool? timePrecision = null, int? top = null)
        {
            return WorkItemTrackingClient.QueryByIdAsync(GetProjectName(), id, timePrecision, top).Result;
        }

        /// <summary>
        /// Gets the results of the query given its WIQL.
        /// </summary>
        /// <param name="wiql">The query containing the WIQL.</param>
        /// <param name="timePrecision">Whether or not to use time precision.</param>
        /// <param name="top">The max number of results to return.</param>
        public WorkItemQueryResult QueryByWiql(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Wiql wiql, bool? timePrecision = null, int? top = null)
        {
            return WorkItemTrackingClient.QueryByWiqlAsync(wiql, GetProjectName(), timePrecision, top).Result;
        }
    }
}
