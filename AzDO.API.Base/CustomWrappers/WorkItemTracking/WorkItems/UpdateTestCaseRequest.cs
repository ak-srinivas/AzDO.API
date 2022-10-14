using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems
{
    public class UpdateTestCaseRequest : UpdateWorkItemRequest
    {
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Steps { get; set; }

        public string QATeam { get; set; }
        public string QATestType { get; set; }

        public string AutomatedTestName { get; set; }
        public string AutomatedTestStorage { get; set; }
        public string AutomatedTestType { get; set; }

        /// <summary>
        /// Guid.NewGuid().ToString()
        /// </summary>
        public string AutomatedTestId { get; set; }
    }
}
