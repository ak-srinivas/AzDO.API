using AzDO.API.Base.Common;
using AzDO.API.Base.Common.Extensions;
using AzDO.API.Base.Common.Utilities;
using AzDO.API.Base.CustomWrappers.WorkItemTracking.WorkItems;
using AzDO.API.Wrappers.WorkItemTracking.WorkItemTypesField;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AzDO.API.Wrappers.WorkItemTracking.WorkItems
{
    public sealed class WorkItemsCustomWrapper : WorkItemsWrapper
    {
        private readonly WorkItemTypesFieldCustomWrapper _workItemTypesFieldCustomWrapper;

        private const string TestCaseCreation = "Test Case Creation";
        private const string TestCaseExecution = "Test Case Execution";

        private const string prepTag = "test case prep";
        private const string execTag = "test case exec";

        public WorkItemsCustomWrapper()
        {
            _workItemTypesFieldCustomWrapper = new WorkItemTypesFieldCustomWrapper();
        }

        public DataTable GetDuplicates(int parentWorkItemId, DataTable earlierTable)
        {
            var csvTable = new DataTable();

            // Column Names
            const string Id = "Id";
            const string Type = "Type";
            const string Title = "Title";
            string Duplicate = "Duplicate";

            List<string> csvHeaders = new List<string>()
            {
                Id,
                Type,
                Title,
                Duplicate
            };

            var ignoreList = new List<string>()
            {
                "/attachments/",
                "vstfs:///"
            };

            foreach (string header in csvHeaders)
            {
                csvTable.Columns.Add(header);
            }

            var workItemResponse = GetWorkItem(parentWorkItemId);
            foreach (WorkItemRelation relations in workItemResponse.Relations)
            {
                if (relations.Url.StartsWith("vstfs:///") || relations.Url.Contains("/attachments/"))
                    continue;

                int childWorkItemId = Convert.ToInt32(relations.Url.Split("/").Last());
                var relationsWorkItem = GetWorkItem(childWorkItemId);
                string type = relationsWorkItem.Fields[FieldNames.SystemWorkItemType].ToString().Trim();
                string title = relationsWorkItem.Fields[FieldNames.SystemTitle].ToString().Trim();

                bool status = csvTable.GetColumnFromDataTable<string>(Id).Contains(childWorkItemId.ToString()) || earlierTable.GetColumnFromDataTable<string>(Id).Contains(childWorkItemId.ToString());

                if (csvTable.GetColumnFromDataTable<string>(Id).Contains(childWorkItemId.ToString()))
                    continue;

                DataRow newRow = csvTable.NewRow();
                newRow[Id] = childWorkItemId;
                newRow[Type] = type;
                newRow[Title] = title;

                if (csvTable.GetColumnFromDataTable<string>(Title).Contains(title) || earlierTable.GetColumnFromDataTable<string>(Title).Contains(title))
                {
                    newRow[Duplicate] = "Yes";
                }

                csvTable.Rows.Add(newRow);
            }

            return csvTable;
        }

        private bool IsTestPlanCreationUserStoryExists(WorkItem userStoryWit)
        {
            if (userStoryWit.Relations == null || userStoryWit.Relations.Count() == 0)
                return false;
            else
            {
                foreach (WorkItemRelation witRelation in userStoryWit.Relations)
                {
                    if (witRelation.Url.Contains("/attachments/"))
                        continue;

                    if (witRelation.Url.StartsWith("vstfs:///"))
                        continue;

                    int witId = Convert.ToInt32(witRelation.Url.Split("/").Last());
                    var workItem = GetWorkItem(witId);
                    string workItemTitle = workItem.Fields[FieldNames.SystemTitle].ToString();

                    if (workItemTitle.EndsWith(TestCaseCreation))
                        return true;
                }
            }
            return false;
        }

        private bool IsTestPlanExecutionUserStoryExists(WorkItem userStoryWit)
        {
            if (userStoryWit.Relations == null || userStoryWit.Relations.Count() == 0)
                return false;
            else
            {
                foreach (WorkItemRelation witRelation in userStoryWit.Relations)
                {
                    if (witRelation.Url.Contains("/attachments/"))
                        continue;

                    if (witRelation.Url.StartsWith("vstfs:///"))
                        continue;

                    int witId = Convert.ToInt32(witRelation.Url.Split("/").Last());
                    var workItem = GetWorkItem(witId);
                    string workItemTitle = workItem.Fields[FieldNames.SystemTitle].ToString();

                    if (workItemTitle.EndsWith(TestCaseExecution))
                        return true;
                }
            }
            return false;
        }

        public void UpdateWorkItemTitles(int witId, Dictionary<string, string> oldAndNewTitles)
        {
            var workItemResponse = GetWorkItem(witId);
            string oldTitle = workItemResponse.Fields[FieldNames.SystemTitle].ToString();
            if (oldAndNewTitles.ContainsKey(oldTitle))
            {
                string newTitle = oldAndNewTitles[oldTitle];
                var updateUserStoryRequest = new UpdateUserStoryRequest()
                {
                    WorkItemId = witId,
                    Title = newTitle,
                };
                WorkItem updateResponse = UpdateUserStoryWorkItem(updateUserStoryRequest);
                Console.WriteLine();
            }
        }

        public void GetWorkItemLinks(int witId, ref List<int> witWithoutLinks)
        {
            WorkItem workItem = GetWorkItem(witId);
            if (workItem.Relations == null || workItem.Relations.Count == 0)
            {
                witWithoutLinks.Add(witId);
                return;
            }

        }

        public void GetWorkItemTitles(int witId, string filterWord, string tag, ref List<string> workItemTitles, ref List<int> witsWithoutTag, ref List<int> witsWithoutProperTitle)
        {
            var workItemResponse = GetWorkItem(witId);
            string title = workItemResponse.Fields[FieldNames.SystemTitle].ToString();
            string titleAsLower = workItemResponse.Fields[FieldNames.SystemTitle].ToString().ToLower();

            if (!workItemResponse.Fields.ContainsKey("System.Tags"))
            {
                witsWithoutTag.Add(witId);
                return;
            }
            if (!workItemResponse.Fields["System.Tags"].ToString().Equals(tag))
                return;

            if (!title.EndsWith(filterWord, false, CultureInfo.InvariantCulture))
            {
                witsWithoutProperTitle.Add(witId);
                return;
            }

            if (titleAsLower.Contains(filterWord.ToLower()))
                workItemTitles.Add(workItemResponse.Fields[FieldNames.SystemTitle].ToString());
        }

        public void CreateQAUserStoriesForPloceus(int parentWitId, int qaFeatureWitId, string dueDate = "2022-11-30T00:00:00Z")
        {
            var workItemResponse = GetWorkItem(parentWitId);
            if (workItemResponse.Relations == null || workItemResponse.Relations.Count() == 0)
                return;

            List<string> relationsUrls = workItemResponse.Relations.Select(item => item.Url).ToList();
            relationsUrls.RemoveAll(item => item.StartsWith("vstfs:///"));

            List<int> devUserStoryWitIds = relationsUrls.Select(urls => Convert.ToInt32(urls.Split("/").Last())).ToList();
            devUserStoryWitIds.Sort();

            foreach (int devUserStoryWitId in devUserStoryWitIds)
            {
                if (devUserStoryWitId == parentWitId)
                    continue;

                var devUserStoryWit = GetWorkItem(devUserStoryWitId);
                if (!devUserStoryWit.Fields[FieldNames.SystemWorkItemType].Equals("User Story"))
                    continue;

                WorkItem creationWIT = null;
                WorkItem executionWIT = null;

                CultureInfo culterInfo = CultureInfo.InvariantCulture;

                string title = devUserStoryWit.Fields[FieldNames.SystemTitle].ToString().Trim();
                string newTitle = title.Replace("v.3.9.0.", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("_v.3.9.0.", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("v3.9.0.", string.Empty, true, culterInfo);

                newTitle = newTitle.Replace("_v.3.9.0.", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("_v3.9.0.", string.Empty, true, culterInfo);

                newTitle = newTitle.Replace("v3.9.0", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("v.3.9.0", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("3.9.0.", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("3.9.0", string.Empty, true, culterInfo);

                newTitle = newTitle.Replace(":", string.Empty);
                newTitle = newTitle.Replace("v1.1.0", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("v.1.1.0", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("v.1.1.0.", string.Empty, true, culterInfo);
                newTitle = newTitle.Replace("using AzureRM version", string.Empty, true, culterInfo);
                newTitle = newTitle.Trim();

                if (newTitle.StartsWith("Create ", false, CultureInfo.InvariantCulture))
                    newTitle = newTitle.Replace("Create ", string.Empty, true, CultureInfo.InvariantCulture);

                if (!IsTestPlanCreationUserStoryExists(devUserStoryWit))
                {
                    string creationTitle = $"{newTitle} {TestCaseCreation}";
                    var creationRequest = new CreateUserStoryRequest()
                    {
                        WorkItemType = WorkItemTypeEnum.UserStory,
                        Title = creationTitle,
                        AreaPath = "Ploceus",
                        IterationPath = "Ploceus",
                        DueDate = dueDate,

                        Tags = new List<string>()
                        {
                            prepTag
                        }
                    };

                    creationWIT = CreateUserStoryWorkItem(creationRequest);

                    var updateUSCreation = new UpdateUserStoryRequest()
                    {
                        Description = creationTitle,
                        WorkItemId = (int)creationWIT.Id,
                        ParentWorkItemId = qaFeatureWitId // QA Activities
                    };
                    WorkItem updateCreationWIT_Update = UpdateUserStoryWorkItem(updateUSCreation);
                }

                if (!IsTestPlanExecutionUserStoryExists(devUserStoryWit))
                {
                    string executionTitle = $"{newTitle} {TestCaseExecution}";
                    var executionRequest = new CreateUserStoryRequest()
                    {
                        WorkItemType = WorkItemTypeEnum.UserStory,
                        Title = executionTitle,
                        AreaPath = "Ploceus",
                        IterationPath = "Ploceus",
                        DueDate = dueDate,

                        Tags = new List<string>()
                        {
                            execTag
                        }
                    };
                    executionWIT = CreateUserStoryWorkItem(executionRequest);

                    var updateUSExecution = new UpdateUserStoryRequest()
                    {
                        Description = executionTitle,
                        WorkItemId = (int)executionWIT.Id,
                        ParentWorkItemId = qaFeatureWitId // QA Activities
                    };
                    WorkItem updateExecutionWIT_Update = UpdateUserStoryWorkItem(updateUSExecution);
                }

                if (creationWIT != null)
                {
                    var updateUserStoryWorkItem = new UpdateUserStoryRequest()
                    {
                        WorkItemId = devUserStoryWitId,

                        RelatedTo = new List<int>()
                        {
                            (int)creationWIT.Id,
                        }
                    };
                    WorkItem updatedWorkItem = UpdateUserStoryWorkItem(updateUserStoryWorkItem);
                }
                if (executionWIT != null)
                {
                    var updateUserStoryWorkItem = new UpdateUserStoryRequest()
                    {
                        WorkItemId = devUserStoryWitId,

                        RelatedTo = new List<int>()
                        {
                            (int)executionWIT.Id,
                        }
                    };
                    WorkItem updatedWorkItem = UpdateUserStoryWorkItem(updateUserStoryWorkItem);
                }
            }
        }

        public WorkItem AddUserStoryToFeature(UpdateWorkItemRequest request)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            Dictionary<string, string> taskReferenceNames = Helpers.GetFieldNamesWithRefNames();

            if (request.ParentWorkItemId != 0)
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

        public WorkItem CreateUserStoryWorkItem(CreateUserStoryRequest request)
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
            string dueDatePath = referenceNames["Due Date"];

            var pathsAndValues = new Dictionary<string, string>
            {
                { titlePath, request.Title },
                { areaPath, request.AreaPath },
                { iterationPath, request.IterationPath },
                { tagPath, tagValue },
                { assignedToPath, request.AssignedTo },
                { dueDatePath, request.DueDate },
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

            WorkItem createdUserStoryWit = CreateWorkItem(patchDocument, request.WorkItemType);
            return createdUserStoryWit;
        }

        public WorkItem UpdateUserStoryWorkItem(UpdateUserStoryRequest request)
        {
            string tagValue = null;
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            Dictionary<string, string> referenceNames = _workItemTypesFieldCustomWrapper.GetFieldsNameWithReferenceNames(WorkItemTypeEnum.UserStory);

            if (string.IsNullOrEmpty(request.AssignedTo))
            {
                // If AssignedTo is null or empty then assign empty string otherwise
                request.AssignedTo = null;
            }

            // If tagValue is null or empty then assign empty string otherwise do nothing.
            if (request.Tags != null && request.Tags.Count > 0)
                tagValue = string.Join(",", request.Tags);

            string titlePath = referenceNames["Title"];
            string descriptionPath = referenceNames["Description"];
            string areaPath = referenceNames["Area Path"];
            string iterationPath = referenceNames["Iteration Path"];
            string tagPath = referenceNames["Tags"];
            string assignedToPath = referenceNames["Assigned To"];
            string dueDatePath = referenceNames["Due Date"];
            string acceptanceCriteriaPath = referenceNames["Acceptance Criteria"];

            var pathsAndValues = new Dictionary<string, string>
            {
                { titlePath, request.Title },
                { areaPath, request.AreaPath },
                { iterationPath, request.IterationPath },
                { tagPath, tagValue },
                { assignedToPath, request.AssignedTo },

                { descriptionPath, request.Description},
                { acceptanceCriteriaPath, request.AcceptanceCriteria},
                { dueDatePath,request.DueDate}
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

            if (request.RelatedTo != null && request.RelatedTo.Count > 0)
            {
                foreach (var relationWorkItemId in request.RelatedTo)
                {
                    var pathOperation = new JsonPatchOperation()
                    {
                        Operation = Operation.Add,
                        Path = "/relations/-",
                        Value = new Link()
                        {
                            Rel = FieldNames.SystemLinkTypesRelated,
                            Url = $"https://dev.azure.com/{GetOrganizationName()}/{GetProjectName()}/_apis/wit/workItems/{relationWorkItemId}",
                        },
                    };

                    patchDocument.Add(pathOperation);
                }
            }

            if (request.ParentWorkItemId != 0)
            {
                var pathOperation = new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/relations/-",
                    Value = new Link()
                    {
                        Rel = FieldNames.SystemLinkTypesHierarchyReverse,
                        Url = $"https://dev.azure.com/{GetOrganizationName()}/{GetProjectName()}/_apis/wit/workItems/{request.ParentWorkItemId}",
                    },
                };

                patchDocument.Add(pathOperation);
            }

            WorkItem updatedWorkItem = UpdateWorkItem(patchDocument, request.WorkItemId);
            return updatedWorkItem;
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

            // To remove steps, the user needs to pass request.Steps as an empty collection.
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