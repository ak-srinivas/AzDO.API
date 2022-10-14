using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.WorkItemTracking.Fields
{
    public sealed class FieldsCustomWrapper : FieldsWrapper
    {
        public List<WorkItemField> GetReadOnlyWorkItemFields()
        {
            List<WorkItemField> workItemFields = WorkItemTrackingClient.GetFieldsAsync().Result;
            return workItemFields.Where(field => field.ReadOnly).ToList();
        }

        public Dictionary<string, string> GetFieldsNameWithReferenceNames()
        {
            List<WorkItemField> workItemFields = ListFields();
            return workItemFields.OrderBy(field => field.Name).ToDictionary(item => item.Name, item => item.ReferenceName);
        }
    }
}
