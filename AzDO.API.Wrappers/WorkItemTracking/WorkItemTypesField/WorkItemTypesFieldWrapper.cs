using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.WorkItemTracking.WorkItemTypesField
{
    public abstract class WorkItemTypesFieldWrapper : WrappersBase
    {
        /// <summary>
        /// Get a field for a work item type with detailed references.
        /// </summary>
        /// <param name="type">Work item type.</param>
        /// <param name="field"></param>
        /// <param name="expand">Expand level for the API response. <br/> Properties: to include allowedvalues, default value, isRequired etc. as a part of response; None: to skip these properties.</param>
        public WorkItemTypeFieldWithReferences GetWorkItemTypeFieldWithReferences(string type, string field, WorkItemTypeFieldsExpandLevel expand = WorkItemTypeFieldsExpandLevel.All)
        {
            return WorkItemTrackingClient.GetWorkItemTypeFieldWithReferencesAsync(GetProjectName(), type, field, expand).Result;
        }

        /// <summary>
        /// Get a list of fields for a work item type with detailed references.
        /// </summary>
        /// <param name="type">Work item type.</param>
        /// <param name="expand">Expand level for the API response. <br/> Properties: to include allowedvalues, default value, isRequired etc. as a part of response; None: to skip these properties.</param>
        /// <returns></returns>
        public List<WorkItemTypeFieldWithReferences> ListWorkItemTypeFieldsWithReferences(string type, WorkItemTypeFieldsExpandLevel expand = WorkItemTypeFieldsExpandLevel.All)
        {
            return WorkItemTrackingClient.GetWorkItemTypeFieldsWithReferencesAsync(GetProjectName(), type, expand).Result;
        }
    }
}
