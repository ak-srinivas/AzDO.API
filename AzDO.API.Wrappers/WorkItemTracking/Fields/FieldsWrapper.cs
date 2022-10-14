using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.WorkItemTracking.Fields
{
    public abstract class FieldsWrapper : WrappersBase
    {
        /// <summary>
        /// Gets information on a specific field.
        /// </summary>
        /// <param name="fieldNameOrRefName">Field simple name or reference name</param>
        public WorkItemField GetField(string fieldNameOrRefName)
        {
            return WorkItemTrackingClient.GetFieldAsync(GetProjectName(), fieldNameOrRefName).Result;
        }

        /// <summary>
        /// Returns information for all fields.
        /// </summary>
        /// <param name="expand">Use ExtensionFields to include extension fields, otherwise exclude them.</param>
        public List<WorkItemField> ListFields(GetFieldsExpand expand = GetFieldsExpand.None)
        {
            return WorkItemTrackingClient.GetFieldsAsync(GetProjectName(), expand).Result;
        }
    }
}
