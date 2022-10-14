using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.WorkItemTracking.WorkItemTypesField
{
    public sealed class WorkItemTypesFieldCustomWrapper : WorkItemTypesFieldWrapper
    {
        public List<WorkItemTypeFieldWithReferences> GetMandatoryFieldsInAWorkItem(WorkItemTypeEnum workItemType)
        {
            string witType = GetWorkItemTypeNameAsString(workItemType);
            return ListWorkItemTypeFieldsWithReferences(witType).Where(item => item.AlwaysRequired.Equals(true)).ToList();
        }

        public Dictionary<string, string> GetFieldsNameWithReferenceNames(WorkItemTypeEnum workItemType)
        {
            string witType = GetWorkItemTypeNameAsString(workItemType);
            return ListWorkItemTypeFieldsWithReferences(witType).OrderBy(obj => obj.Name).ToDictionary(item => item.Name, item => $"/fields/{item.ReferenceName}");
        }
    }
}
