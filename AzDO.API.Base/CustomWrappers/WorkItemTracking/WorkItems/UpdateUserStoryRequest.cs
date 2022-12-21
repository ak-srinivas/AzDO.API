using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems
{
    public class UpdateUserStoryRequest : UpdateWorkItemRequest
    {
        public string QAReviewer { get; set; }
        public List<int> RelatedTo { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string AcceptanceCriteria { get; set; }
    }
}
