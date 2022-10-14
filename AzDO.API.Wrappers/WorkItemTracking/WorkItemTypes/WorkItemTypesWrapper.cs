using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.WorkItemTracking.WorkItemTypes
{
    public abstract class WorkItemTypesWrapper : WrappersBase
    {
        /// <summary>
        /// Returns a work item type definition.
        /// </summary>
        /// <param name="type">Work item type name</param>
        /// <returns></returns>
        public WorkItemType GetWorkItemType(string type)
        {
            return WorkItemTrackingClient.GetWorkItemTypeAsync(GetProjectName(), type).Result;
        }

        /// <summary>
        /// Returns the list of work item types.
        /// </summary>
        public List<WorkItemType> ListWorkItemTypes()
        {
            return WorkItemTrackingClient.GetWorkItemTypesAsync(GetProjectName()).Result;
        }
    }
}
