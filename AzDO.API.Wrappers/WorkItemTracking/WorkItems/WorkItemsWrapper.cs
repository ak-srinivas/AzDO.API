using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.WorkItemTracking.WorkItems
{
    public abstract class WorkItemsWrapper : WrappersBase
    {
        /// <summary>
        /// Creates a single work item.
        /// </summary>
        /// <param name="document">The JSON Patch document representing the work item</param>
        /// <param name="workItemType">The work item type of the work item to create</param>
        /// <param name="validateOnly">Indicate if you only want to validate the changes without saving the work item.<br/> When this is set to true, the work item will not be created.</param>
        /// <param name="expand">The expand parameters for work item attributes. Possible options are { None, Relations, Fields, Links, All }.</param>
        public WorkItem CreateWorkItem(JsonPatchDocument document, WorkItemTypeEnum workItemType, bool validateOnly = false)
        {
            string witType = GetWorkItemTypeNameAsString(workItemType);
            return WorkItemTrackingClient.CreateWorkItemAsync(document, GetProjectName(), witType, validateOnly).Result;
        }

        /// <summary>
        /// Returns a single work item.<br/>
        /// </summary>
        /// <param name="id">The work item id</param>
        public WorkItem GetWorkItem(int id, IEnumerable<string> fields = null, WorkItemExpand expand = WorkItemExpand.All)
        {
            return WorkItemTrackingClient.GetWorkItemAsync(GetProjectName(), id, fields, null, expand).Result;
        }

        /// <summary>
        /// Returns a single work item from a template.<br/>
        /// </summary>
        /// <param name="workItemType">The work item type.</param>
        /// <param name="fieldsSet">(Nullable) A set of fields.</param>
        /// <param name="expand">(Nullable) Work items to be expanded.</param>
        public WorkItem GetWorkItemTemplate(WorkItemTypeEnum workItemType, IEnumerable<string> fieldsSet = null, WorkItemExpand expand = WorkItemExpand.All)
        {
            string witType = GetWorkItemTypeNameAsString(workItemType);
            string fields = (fieldsSet == null) ? null : string.Join(",", fieldsSet);

            return WorkItemTrackingClient.GetWorkItemTemplateAsync(GetProjectName(), witType, fields, null, expand).Result;
        }

        /// <summary>
        /// Gets work items for a list of work item ids (Maximum 200) <br/>
        /// </summary>
        public List<WorkItem> GetWorkItemsBatch(WorkItemBatchGetRequest workItemGetRequest)
        {
            return WorkItemTrackingClient.GetWorkItemsBatchAsync(workItemGetRequest, GetProjectName()).Result;
        }

        /// <summary>
        /// Returns a list of work items (Maximum 200)
        /// </summary>
        /// <param name="workItemIds">The list of requested work item ids (Maximum 200 ids allowed)</param>
        /// <param name="fieldNames">The list of requested fields</param>
        /// <param name="asOf">As of UTC date time</param>
        /// <param name="expand">The expand parameters for work item attributes. Possible options are { None, Relations, Fields, Links, All }.</param>
        public List<WorkItem> ListWorkItems(IEnumerable<int> workItemIds, IEnumerable<string> fieldNames = null, DateTime? asOf = null, WorkItemExpand expand = WorkItemExpand.All)
        {
            return WorkItemTrackingClient.GetWorkItemsAsync(GetProjectName(), workItemIds, fieldNames, asOf, expand).Result;
        }

        /// <summary>
        /// Updates a single work item.
        /// </summary>
        /// <param name="document">The JSON Patch document representing the update</param>
        /// <param name="workItemId">The id of the work item to update</param>
        /// <param name="validateOnly">Indicate if you only want to validate the changes without saving the work item.<br/> When this is set to true, the work item will not be created.</param>
        /// <param name="expand">The expand parameters for work item attributes. Possible options are { None, Relations, Fields, Links, All }.</param>
        public WorkItem UpdateWorkItem(JsonPatchDocument document, int workItemId, bool validateOnly = false, WorkItemExpand expand = WorkItemExpand.All)
        {
            return WorkItemTrackingClient.UpdateWorkItemAsync(document, GetProjectName(), workItemId, validateOnly, null, null, expand).Result;
        }
    }
}
