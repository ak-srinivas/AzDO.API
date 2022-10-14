using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems
{
    public class UpdateWorkItemRequest
    {
        public int WorkItemId { get; set; }
        public int ParentWorkItemId { get; set; }

        public string Title { get; set; }
        public string AreaPath { get; set; }
        public string IterationPath { get; set; }
        public List<string> Tags { get; set; }
        public string AssignedTo { get; set; }
    }
}
