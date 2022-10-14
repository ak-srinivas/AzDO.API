using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Wrappers.WorkItemTracking.Updates
{
    public abstract class UpdatesWrapper : WrappersBase
    {
        /// <summary>
        /// Returns the details between work item revisions
        /// </summary>
        /// <param name="id">The workitem id.</param>
        public List<WorkItemUpdate> GetUpdates(int id, int? top = null, int? skip = null)
        {
            return WorkItemTrackingClient.GetUpdatesAsync(GetProjectName(), id, top, skip).Result;
        }
    }
}
