using AzDO.API.Base.Common;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems
{
    public class CreateWorkItemRequest
    {
        public WorkItemTypeEnum WorkItemType { get; set; }
        public string Title { get; set; }
        public string AreaPath { get; set; }
        public string IterationPath { get; set; }
        public List<string> Tags { get; set; }
        public string AssignedTo { get; set; }

        public CreateWorkItemRequest()
        {
            Tags = new List<string>();
            AssignedTo = "";
        }
    }

    public class CreateUserStoryRequest : CreateWorkItemRequest
    {
        public string DueDate { get; set; }
    }
}
