using AzDO.API.Base.Common.Utilities;
using AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems;
using AzDO.API.Wrappers.WorkItemTracking.WorkItemTypesField;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.WorkItemTracking.WorkItems
{
    public sealed class WorkItemsCustomWrapper : WorkItemsWrapper
    {
        private readonly WorkItemTypesFieldCustomWrapper _workItemTypesFieldCustomWrapper;

        public WorkItemsCustomWrapper()
        {
            _workItemTypesFieldCustomWrapper = new WorkItemTypesFieldCustomWrapper();
        }

        public WorkItem CreateWorkItem(CreateWorkItemRequest request)
        {
            string tagValue = null;
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            Dictionary<string, string> referenceNames = _workItemTypesFieldCustomWrapper.GetFieldsNameWithReferenceNames(request.WorkItemType);

            if (string.IsNullOrEmpty(request.AssignedTo))
            {
                // If AssignedTo is null or empty then assign empty string otherwise
                request.AssignedTo = null;
            }

            // If tagValue is null or empty then assign empty string otherwise do nothing.
            if (request.Tags != null && request.Tags.Count > 0)
                tagValue = string.Join(",", request.Tags);

            string titlePath = referenceNames["Title"];
            string areaPath = referenceNames["Area Path"];
            string iterationPath = referenceNames["Iteration Path"];
            string tagPath = referenceNames["Tags"];
            string assignedToPath = referenceNames["Assigned To"];

            var pathsAndValues = new Dictionary<string, string>
            {
                { titlePath, request.Title },
                { areaPath, request.AreaPath },
                { iterationPath, request.IterationPath },
                { tagPath, tagValue },
                { assignedToPath, request.AssignedTo },
            };

            foreach (KeyValuePair<string, string> pair in pathsAndValues)
            {
                if (pair.Value != null)
                {
                    var pathOperation = new JsonPatchOperation()
                    {
                        Operation = Operation.Add,
                        Path = pair.Key,
                        Value = pair.Value,
                    };

                    patchDocument.Add(pathOperation);
                }
            }

            WorkItem createdWorkItem = CreateWorkItem(patchDocument, request.WorkItemType);
            return createdWorkItem;
        }

        public WorkItem UpdateTaskWorkItem(UpdateTaskRequest request)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            Dictionary<string, string> taskReferenceNames = Helpers.GetFieldNamesWithRefNames();

            // When request.Tags is an empty list then it means the user wants to remove all existing tags, so set 'tags' default value to string.
            string tags = string.Empty;

            // To remove tags, the user needs to pass request.Tags as an empty collection.
            if (request.Tags == null)
                tags = null;

            else if (request.Tags != null && request.Tags.Count > 0)
                tags = string.Join(";", request.Tags);

            if (string.IsNullOrEmpty(request.AssignedTo))
            {
                // If AssignedTo is null or empty then assign empty string otherwise
                request.AssignedTo = null;
            }

            var pathsAndValues = new Dictionary<string, string>
            {
                { taskReferenceNames["Title"], request.Title },
                { taskReferenceNames["Area Path"], request.AreaPath },
                { taskReferenceNames["Iteration Path"], request.IterationPath },
                { taskReferenceNames["Tags"], tags },
                { taskReferenceNames["Assigned To"], request.AssignedTo },

                { taskReferenceNames["Description"], request.Description },
                { taskReferenceNames["Priority"], request.Priority },
                { taskReferenceNames["Activity"], request.Activity },

                { taskReferenceNames["Original Estimate"], request.OriginalEstimate },
                { taskReferenceNames["Remaining Work"], request.RemainingEstimate },
                { taskReferenceNames["Completed Work"], request.CompletedEstimate },
            };

            foreach (KeyValuePair<string, string> pair in pathsAndValues)
            {
                if (pair.Value != null)
                {
                    var pathOperation = new JsonPatchOperation()
                    {
                        Operation = Operation.Replace,
                        Path = pair.Key,
                        Value = pair.Value,
                    };

                    patchDocument.Add(pathOperation);
                }
            }

            if (request.ParentWorkItemId > 0)
            {
                var pathOperation = new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/relations/-",
                    Value = new Link()
                    {
                        //Add a parent link
                        Rel = FieldNames.SystemLinkTypesHierarchyReverse,
                        Url = $"https://dev.azure.com/{GetOrganizationName()}/{GetProjectName()}/_apis/wit/workItems/{request.ParentWorkItemId}",
                    },
                };

                patchDocument.Add(pathOperation);
            }

            return UpdateWorkItem(patchDocument, request.WorkItemId);
        }

        public WorkItem UpdateTestCaseWorkItem(UpdateTestCaseRequest request)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            Dictionary<string, string> testCaseReferenceNames = Helpers.GetFieldNamesWithRefNames();

            // When request.Tags is an empty list then it means the user wants to remove all existing tags, so set 'tags' default value to string.
            string tags = string.Empty;

            // When request.Steps is an empty list then it means the user wants to remove all existing steps, so set 'steps' default value to string.
            string steps = string.Empty;

            // This variable will set automation status to 'Automated' when few conditions are satisfied.
            string automationStatus = null;

            if (string.IsNullOrEmpty(request.AssignedTo))
            {
                // If AssignedTo is null or empty then assign empty string
                request.AssignedTo = null;
            }

            // To remove tags, the user needs to pass request.Tags as an empty collection.
            if (request.Tags == null)
                tags = null;

            else if (request.Tags != null && request.Tags.Count > 0)
                tags = string.Join(";", request.Tags);

            // To remove tags, the user needs to pass request.Steps as an empty collection.
            if (request.Steps == null)
                steps = null;

            else if (request.Steps != null)
                steps = request.Steps;

            if (!string.IsNullOrEmpty(request.AutomatedTestName) &&
                !string.IsNullOrEmpty(request.AutomatedTestStorage) &&
                !string.IsNullOrEmpty(request.AutomatedTestId))
            {
                /* The automation status is tied to 'Automated Test Id', meaning we can set the status to 'automated' only when:
                 * 1. 'Automated Test Name' is not null or empty.
                 * 2. 'Automated Test Storage' is not null or empty.
                 * 3. 'Automated Test Id' has a guid.
                 */

                automationStatus = "Automated";
            }
            else if (request.AutomatedTestName.Equals(string.Empty) &&
                request.AutomatedTestStorage.Equals(string.Empty) &&
                request.AutomatedTestId.Equals(string.Empty))
            {
                automationStatus = "Not Automated";
            }

            var pathsAndValues = new Dictionary<string, string>
            {
                { testCaseReferenceNames["Title"], request.Title },
                { testCaseReferenceNames["Area Path"], request.AreaPath },
                { testCaseReferenceNames["Iteration Path"], request.IterationPath },
                { testCaseReferenceNames["Tags"], tags },
                { testCaseReferenceNames["Assigned To"], request.AssignedTo },

                { testCaseReferenceNames["Description"], request.Description },
                { testCaseReferenceNames["Priority"], request.Priority },
                { testCaseReferenceNames["Steps"], steps},

                { testCaseReferenceNames["QATeam"], request.QATeam},
                { testCaseReferenceNames["QA Test Type"], request.QATestType},

                { testCaseReferenceNames["Automated Test Name"], request.AutomatedTestName},
                { testCaseReferenceNames["Automated Test Storage"], request.AutomatedTestStorage},
                { testCaseReferenceNames["Automated Test Type"], request.AutomatedTestType},
                { testCaseReferenceNames["Automated Test Id"], request.AutomatedTestId},
                { testCaseReferenceNames["Automation status"], automationStatus}
            };

            foreach (KeyValuePair<string, string> pair in pathsAndValues)
            {
                if (pair.Value != null)
                {
                    var pathOperation = new JsonPatchOperation()
                    {
                        Operation = Operation.Replace,
                        Path = pair.Key,
                        Value = pair.Value,
                    };

                    patchDocument.Add(pathOperation);
                }
            }

            if (request.ParentWorkItemId > 0)
            {
                var pathOperation = new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/relations/-",
                    Value = new Link()
                    {
                        //Add a tests link
                        Rel = FieldNames.MicrosoftVSTSCommonTestedByReverse,
                        Url = $"https://dev.azure.com/{GetOrganizationName()}/{GetProjectName()}/_apis/wit/workItems/{request.ParentWorkItemId}"
                    },
                };

                patchDocument.Add(pathOperation);
            }

            return UpdateWorkItem(patchDocument, request.WorkItemId);
        }

        public WorkItem UpdateWorkItem(UpdateWorkItemRequest request)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            Dictionary<string, string> testCaseReferenceNames = Helpers.GetFieldNamesWithRefNames();

            // When request.Tags is an empty list then it means the user wants to remove all existing tags, so set 'tags' default value to string.
            string tags = string.Empty;

            if (string.IsNullOrEmpty(request.AssignedTo))
            {
                // If AssignedTo is null or empty then assign empty string
                request.AssignedTo = null;
            }

            // To remove tags, the user needs to pass request.Tags as an empty collection.
            if (request.Tags == null)
                tags = null;

            else if (request.Tags != null && request.Tags.Count > 0)
                tags = string.Join(";", request.Tags);

            var pathsAndValues = new Dictionary<string, string>
            {
                { testCaseReferenceNames["Title"], request.Title },
                { testCaseReferenceNames["Area Path"], request.AreaPath },
                { testCaseReferenceNames["Iteration Path"], request.IterationPath },
                { testCaseReferenceNames["Tags"], tags },
                { testCaseReferenceNames["Assigned To"], request.AssignedTo },
            };

            foreach (KeyValuePair<string, string> pair in pathsAndValues)
            {
                if (pair.Value != null)
                {
                    var pathOperation = new JsonPatchOperation()
                    {
                        Operation = Operation.Replace,
                        Path = pair.Key,
                        Value = pair.Value,
                    };

                    patchDocument.Add(pathOperation);
                }
            }

            if (request.ParentWorkItemId > 0)
            {
                var pathOperation = new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/relations/-",
                    Value = new Link()
                    {
                        //Add a tests link
                        Rel = FieldNames.MicrosoftVSTSCommonTestedByReverse,
                        Url = $"https://dev.azure.com/{GetOrganizationName()}/{GetProjectName()}/_apis/wit/workItems/{request.ParentWorkItemId}"
                    },
                };

                patchDocument.Add(pathOperation);
            }

            return UpdateWorkItem(patchDocument, request.WorkItemId);
        }

        public bool RemoveTestCaseAssociation(List<int> testCaseIds)
        {
            bool finalStatus = false;

            foreach (int testCaseId in testCaseIds)
            {
                var updateTestCaseRequest = new UpdateTestCaseRequest()
                {
                    WorkItemId = testCaseId,
                    AutomatedTestName = string.Empty,
                    AutomatedTestStorage = string.Empty,
                    AutomatedTestType = string.Empty,
                    AutomatedTestId = string.Empty,
                    QATestType = string.Empty,
                    Steps = string.Empty
                };

                WorkItem updatedWorkItem = UpdateTestCaseWorkItem(updateTestCaseRequest);

                if ((updatedWorkItem != null) && (updatedWorkItem.Id == testCaseId))
                    finalStatus = true;
                else
                    finalStatus = false;
            }

            return finalStatus;
        }

        public SortedSet<int> GetTaskWorkItemIds_FilterBy_EmailId(SortedSet<int> parentStoryIds, string emailId)
        {
            SortedSet<int> taskIds = new SortedSet<int>();

            foreach (int storyId in parentStoryIds)
            {
                WorkItem storyInfo = GetWorkItem(storyId);
                List<string> childTaskUrls = storyInfo.Relations.Where(item => item.Rel.Equals(FieldNames.SystemLinkTypesHierarchyForward)).Select(item => item.Url).ToList();

                foreach (string taskUrl in childTaskUrls)
                {
                    int taskId = Convert.ToInt32(taskUrl.Split("/").Last());
                    WorkItem taskInfo = GetWorkItem(taskId);
                    IdentityRef identity = (IdentityRef)taskInfo.Fields[FieldNames.SystemAssignedTo];
                    if (identity.UniqueName.ToString().Equals(emailId, StringComparison.OrdinalIgnoreCase))
                    {
                        taskIds.Add(taskId);
                    }
                }
            }

            return taskIds;
        }
    }
}
