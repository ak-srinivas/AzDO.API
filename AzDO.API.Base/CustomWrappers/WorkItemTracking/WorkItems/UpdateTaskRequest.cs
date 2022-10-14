using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems
{
    public class UpdateTaskRequest : UpdateWorkItemRequest
    {
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Activity { get; set; }

        public string OriginalEstimate { get; set; }
        public string RemainingEstimate { get; set; }
        public string CompletedEstimate { get; set; }
    }
}
