using AzDO.API.Base.Common;
using AzDO.API.Wrappers.Core.Projects;
using AzDO.API.Wrappers.Git.PullRequests;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AzDO.API.Wrappers.Work.Iterations
{
    public sealed class IterationsCustomWrapper : IterationsWrapper
    {
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;
        private readonly PullRequestsCustomWrapper _pullRequestsCustomWrapper;
        private readonly ProjectsCustomWrapper _projectsCustomWrapper;

        private readonly string _teamBoardName;

        private readonly SortedSet<string> witTypes = new SortedSet<string>()
        {
            GetWorkItemTypeNameAsString(WorkItemTypeEnum.Bug),
            //GetWorkItemTypeNameAsString(WorkItemTypeEnum.SecurityIssue),
            GetWorkItemTypeNameAsString(WorkItemTypeEnum.UserStory),
        };

        public IterationsCustomWrapper(string teamBoardName)
        {
            _teamBoardName = teamBoardName;

            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
            _pullRequestsCustomWrapper = new PullRequestsCustomWrapper();
            _projectsCustomWrapper = new ProjectsCustomWrapper();
        }

        public new string GetProjectName()
        {
            return base.GetProjectName();
        }

        #region Custom-Iteration

        public List<int> GetWorkItemIds_InIteration(Guid iterationId)
        {
            var currentSprintWorkItemIds = new List<int>();
            var teamContext = new TeamContext(GetProjectName(), _teamBoardName);
            IterationWorkItems iterationWorkItems = GetIterationWorkItems(teamContext, iterationId);
            var list = new List<WorkItem>();

            var workItemIdsToIgnore = new List<int>();

            if (iterationWorkItems != null)
            {
                foreach (WorkItemLink itemLink in iterationWorkItems.WorkItemRelations)
                {
                    WorkItem witInfo = _workItemsCustomWrapper.GetWorkItem(itemLink.Target.Id);
                    string witType = witInfo.Fields[FieldNames.SystemWorkItemType].ToString();

                    if (witTypes.Contains(witType))
                    {
                        if (witInfo.Relations != null && witInfo.Relations.Count > 0)
                        {
                            List<string> parentRelationUrls = witInfo.Relations.Where(item => item.Rel.Equals(FieldNames.SystemLinkTypesHierarchyReverse)).Select(item => item.Url).ToList();
                            foreach (string parentWitUrl in parentRelationUrls)
                            {
                                workItemIdsToIgnore.Add(Convert.ToInt32(parentWitUrl.Split("/", StringSplitOptions.RemoveEmptyEntries).Last()));
                            }
                        }

                        currentSprintWorkItemIds.Add(itemLink.Target.Id);
                    }
                }

                return currentSprintWorkItemIds.Except(workItemIdsToIgnore).ToList();
            }
            return currentSprintWorkItemIds;
        }

        public HashSet<WorkItem> GetWorkItems_InIteration(Guid iterationId)
        {
            var currentWorkItemsSet = new HashSet<WorkItem>();
            List<int> currentWorkItemIds = GetWorkItemIds_InIteration(iterationId);

            foreach (int currentWorkItemId in currentWorkItemIds)
            {
                WorkItem witInfo = _workItemsCustomWrapper.GetWorkItem(currentWorkItemId);
                currentWorkItemsSet.Add(witInfo);
            }
            return currentWorkItemsSet;
        }

        public List<int> GetQAWorkItemIds_InIteration_FilterBy_EmailIds(Guid iterationId, List<string> workEmailIds)
        {
            if (workEmailIds != null && workEmailIds.Count > 0)
                workEmailIds = workEmailIds.Select(item => item.Trim().ToUpper()).ToList();

            const string CustomQA = "Custom.QA";
            List<int> myWorkItems = new List<int>();
            List<int> currentWorkItems = GetWorkItemIds_InIteration(iterationId);

            foreach (int workItemId in currentWorkItems)
            {
                WorkItem workItem = _workItemsCustomWrapper.GetWorkItem(workItemId);
                string workItemTitle = workItem.Fields[FieldNames.SystemTitle].ToString().Trim();

                if (workItemTitle.StartsWith("Sprint Release Management")
                    || workItemTitle.StartsWith("Sprint Goal"))
                {
                    // Ignore work items that are related to vision of the sprint
                    continue;
                }

                if (workItem.Fields.ContainsKey(CustomQA))
                {
                    IdentityRef workItemQA = (IdentityRef)workItem.Fields[CustomQA];
                    if (workEmailIds.Contains(workItemQA.UniqueName.ToUpper()))
                    {
                        myWorkItems.Add(workItemId);
                    }
                }
            }

            return myWorkItems.Distinct().ToList();
        }

        #endregion

        #region Current-Iteration

        public TeamSettingsIteration GetCurrentIteration()
        {
            var teamContext = new TeamContext(GetProjectName(), _teamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = GetTeamIterations(teamContext, "Current");
            return teamSettingsIterations[0];
        }

        public List<int> GetWorkItemIds_InCurrentIteration()
        {
            TeamSettingsIteration teamSettingsIteration = GetCurrentIteration();
            return GetWorkItemIds_InIteration(teamSettingsIteration.Id);
        }

        public HashSet<WorkItem> GetWorkItems_InCurrentIteration()
        {
            var currentWorkItemsSet = new HashSet<WorkItem>();
            List<int> currentWorkItemIds = GetWorkItemIds_InCurrentIteration();

            foreach (int currentWorkItemId in currentWorkItemIds)
            {
                WorkItem witInfo = _workItemsCustomWrapper.GetWorkItem(currentWorkItemId);
                currentWorkItemsSet.Add(witInfo);
            }
            return currentWorkItemsSet;
        }

        public SortedSet<int> GetQAWorkItemIds_In_CurrentIteration_FilterBy_EmailIds(List<string> workEmailIds)
        {
            if (workEmailIds != null && workEmailIds.Count > 0)
                workEmailIds = workEmailIds.Select(item => item.Trim().ToUpper()).ToList();

            const string CustomQA = "Custom.QA";
            SortedSet<int> myWorkItems = new SortedSet<int>();
            List<int> currentWorkItems = GetWorkItemIds_InCurrentIteration();

            foreach (int workItemId in currentWorkItems)
            {
                WorkItem workItem = _workItemsCustomWrapper.GetWorkItem(workItemId);
                string workItemTitle = workItem.Fields[FieldNames.SystemTitle].ToString().Trim();

                if (workItemTitle.StartsWith("Sprint Release Management")
                    || workItemTitle.StartsWith("Sprint Goal"))
                {
                    // Ignore work items that are related to vision of the sprint
                    continue;
                }

                if (workItem.Fields.ContainsKey(CustomQA))
                {
                    IdentityRef workItemQA = (IdentityRef)workItem.Fields[CustomQA];
                    if (workEmailIds.Contains(workItemQA.UniqueName.ToUpper()))
                    {
                        myWorkItems.Add(workItemId);
                    }
                }
            }

            return myWorkItems;
        }

        #endregion

        /// <summary>
        /// Gets the story points from a given iteration
        /// </summary>
        public double GetStoryPoints(Guid iterationId)
        {
            double storyPoints = 0;

            List<int> currentWorkItems = GetWorkItemIds_InIteration(iterationId);

            foreach (int currentWorkItem in currentWorkItems)
            {
                WorkItem witInfo = _workItemsCustomWrapper.GetWorkItem(currentWorkItem);
                string witType = witInfo.Fields[FieldNames.SystemWorkItemType].ToString();

                if (witTypes.Contains(witType))
                {
                    if (witInfo.Fields.ContainsKey(FieldNames.MicrosoftVSTSSchedulingStoryPoints))
                    {
                        double points = Convert.ToDouble(witInfo.Fields[FieldNames.MicrosoftVSTSSchedulingStoryPoints]);
                        storyPoints += points;
                    }
                }
            }

            return storyPoints;
        }

        public DataTable GetLinks_FromWorkItems_InIteration(Guid iterationId)
        {
            var csvTable = new DataTable();
            int tableRowCount = 1;

            // Column Names
            const string SNo = "S.No.";
            const string Id = "Id";
            const string Type = "Type";
            const string Title = "Title";
            const string State = "State";
            const string AssignedTo = "Assigned To";
            const string QA = "QA";
            const string StoryPoints = "Story Points";
            const string PullRequestIds = "Pull Request Ids";
            const string RepoNames = "Repository Names";
            const string PRStatuses = "PR(s) Status";
            const string PullRequestIdLinks = "Pull Request Id Links";

            List<string> csvHeaders = new List<string>()
            {
                SNo,
                Id,
                Type,
                Title,
                State,
                AssignedTo,
                QA,
                StoryPoints,
                PRStatuses,
                PullRequestIds,
                RepoNames,
                PullRequestIdLinks
            };

            foreach (string header in csvHeaders)
            {
                csvTable.Columns.Add(header);
            }

            ///////////////////////////////////////////////////////////////////////////
            //HashSet<WorkItem> currentWorkItemsSet = GetWorkItems_InCurrentIteration();
            HashSet<WorkItem> currentWorkItemsSet = GetWorkItems_InIteration(iterationId);

            if (currentWorkItemsSet != null && currentWorkItemsSet.Count > 0)
            {
                foreach (WorkItem witInfo in currentWorkItemsSet)
                {
                    string witType = witInfo.Fields[FieldNames.SystemWorkItemType].ToString();

                    if (witTypes.Contains(witType) && witInfo.Relations != null)
                    {
                        DataRow newRow = csvTable.NewRow();
                        try
                        {
                            newRow[SNo] = tableRowCount++;
                            newRow[Id] = witInfo.Id;
                            newRow[Type] = witInfo.Fields[FieldNames.SystemWorkItemType];
                            newRow[Title] = witInfo.Fields[FieldNames.SystemTitle];
                            newRow[State] = witInfo.Fields[FieldNames.SystemState];

                            if (witInfo.Fields.ContainsKey(FieldNames.MicrosoftVSTSSchedulingStoryPoints))
                            {
                                newRow[StoryPoints] = Convert.ToInt32(witInfo.Fields[FieldNames.MicrosoftVSTSSchedulingStoryPoints]);
                            }

                            if (witInfo.Fields.ContainsKey(FieldNames.SystemAssignedTo))
                            {
                                IdentityRef identity = (IdentityRef)witInfo.Fields[FieldNames.SystemAssignedTo];
                                newRow[AssignedTo] = identity.DisplayName;
                            }

                            if (witInfo.Fields.ContainsKey("Custom.QA"))
                            {
                                IdentityRef identity = (IdentityRef)witInfo.Fields["Custom.QA"];
                                newRow[QA] = identity.DisplayName;
                            }

                            if (!witInfo.Fields[FieldNames.SystemTitle].ToString().Contains("Release Management"))
                            {
                                IList<string> artifactLinks = witInfo.Relations.Where(obj => obj.Rel.Equals("ArtifactLink")).Select(item => item.Url).ToList();
                                if (artifactLinks != null && artifactLinks.Count > 0)
                                {
                                    GetDataFromArtifacts(artifactLinks,
                                        out List<string> links_for_pullRequestIds,
                                        out List<int> pullRequestIds,
                                        out List<string> repoNames,
                                        out List<string> prStatuses);

                                    newRow[PullRequestIds] = string.Join(" ; ", pullRequestIds);
                                    newRow[PRStatuses] = string.Join(" ; ", prStatuses);
                                    newRow[PullRequestIdLinks] = string.Join(" ; ", links_for_pullRequestIds);

                                    newRow[RepoNames] = string.Join(" ; ", repoNames);
                                }
                            }

                            csvTable.Rows.Add(newRow);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            csvTable.Rows.Add(newRow);
                            //throw;
                        }
                    }
                }

            }
            return csvTable;
        }

        public DataTable GetMyWorkItems_InIteration(Guid iterationId, string emailId, string iterationName)
        {
            var csvTable = new DataTable();
            int tableRowCount = 1;

            // Column Names
            const string SNo = "S.No.";
            const string Iteration = "Iteration";
            const string Id = "Id";
            const string Type = "Type";
            const string Title = "Title";
            const string State = "State";
            const string Priority = "Priority";
            const string AssignedTo = "Assigned To";
            const string QA = "QA";
            const string StoryPoints = "Story Points";
            const string PullRequestIds = "Pull Request Ids";
            const string RepoNames = "Repository Names";
            const string PRStatuses = "PR(s) Status";
            const string PullRequestIdLinks = "Pull Request Id Links";

            List<string> csvHeaders = new List<string>()
            {
                SNo,
                Iteration,
                Id,
                Type,
                Title,
                State,
                Priority,
                AssignedTo,
                QA,
                StoryPoints,
                PRStatuses,
                PullRequestIds,
                RepoNames,
                PullRequestIdLinks
            };

            foreach (string header in csvHeaders)
            {
                csvTable.Columns.Add(header);
            }

            ///////////////////////////////////////////////////////////////////////////
            //HashSet<WorkItem> currentWorkItemsSet = GetWorkItems_InCurrentIteration();
            HashSet<WorkItem> currentWorkItemsSet = GetWorkItems_InIteration(iterationId);

            if (currentWorkItemsSet != null && currentWorkItemsSet.Count > 0)
            {
                foreach (WorkItem witInfo in currentWorkItemsSet)
                {
                    if (witInfo.Fields[FieldNames.SystemTitle].ToString().Contains("Release Management"))
                        continue;

                    string witType = witInfo.Fields[FieldNames.SystemWorkItemType].ToString();

                    if (witTypes.Contains(witType) && witInfo.Relations != null && witInfo.Fields.ContainsKey("Custom.QA"))
                    {
                        IdentityRef primaryId = (IdentityRef)witInfo.Fields["Custom.QA"];
                        if (primaryId.UniqueName.Equals(emailId, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                DataRow newRow = csvTable.NewRow();

                                newRow[SNo] = tableRowCount++;
                                newRow[Iteration] = iterationName;
                                newRow[Id] = witInfo.Id;
                                newRow[Type] = witInfo.Fields[FieldNames.SystemWorkItemType];
                                newRow[Title] = witInfo.Fields[FieldNames.SystemTitle];
                                newRow[State] = witInfo.Fields[FieldNames.SystemState];
                                newRow[Priority] = witInfo.Fields[FieldNames.MicrosoftVSTSCommonPriority];

                                if (witInfo.Fields.ContainsKey(FieldNames.MicrosoftVSTSSchedulingStoryPoints))
                                {
                                    newRow[StoryPoints] = Convert.ToInt32(witInfo.Fields[FieldNames.MicrosoftVSTSSchedulingStoryPoints]);
                                }

                                if (witInfo.Fields.ContainsKey(FieldNames.SystemAssignedTo))
                                {
                                    IdentityRef identity = (IdentityRef)witInfo.Fields[FieldNames.SystemAssignedTo];
                                    newRow[AssignedTo] = identity.DisplayName;
                                }

                                if (witInfo.Fields.ContainsKey("Custom.QA"))
                                {
                                    IdentityRef identity = (IdentityRef)witInfo.Fields["Custom.QA"];
                                    newRow[QA] = identity.DisplayName;
                                }

                                IList<string> artifactLinks = witInfo.Relations.Where(obj => obj.Rel.Equals("ArtifactLink")).Select(item => item.Url).ToList();
                                if (artifactLinks != null && artifactLinks.Count > 0)
                                {
                                    GetDataFromArtifacts(artifactLinks,
                                        out List<string> links_for_pullRequestIds,
                                        out List<int> pullRequestIds,
                                        out List<string> repoNames,
                                        out List<string> prStatuses);

                                    newRow[PullRequestIds] = string.Join(" ; ", pullRequestIds);
                                    newRow[PRStatuses] = string.Join(" ; ", prStatuses);
                                    newRow[PullRequestIdLinks] = string.Join(" ; ", links_for_pullRequestIds);

                                    newRow[RepoNames] = string.Join(" ; ", repoNames);
                                }

                                csvTable.Rows.Add(newRow);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                //throw;
                            }
                        }
                    }
                }

            }
            return csvTable;
        }

        public void GetDataFromArtifacts(
            IList<string> artifactLinks,
            out List<string> links_for_pullRequestIds,
            out List<int> pullRequestIds,
            out List<string> repoNames,
            out List<string> prStatuses, bool getOnlyPRIds = false)
        {
            links_for_pullRequestIds = new List<string>();
            pullRequestIds = new List<int>();
            repoNames = new List<string>();
            prStatuses = new List<string>();

            foreach (string link in artifactLinks)
            {
                GitPullRequest prInfo = null;

                if (link.StartsWith(GitCommit))
                {
                    string commitId = link.ToLower().Split("%2f", StringSplitOptions.RemoveEmptyEntries).Last().Substring(0, 5);
                }
                else if (link.StartsWith(GitPullRequestId))
                {
                    string prIdAsString = link.ToLower().Split("%2f", StringSplitOptions.RemoveEmptyEntries).Last();
                    int prId = Convert.ToInt32(prIdAsString);
                    pullRequestIds.Add(prId);

                    if (!getOnlyPRIds)
                    {
                        string repoGuidId = link.Replace("vstfs:///Git/PullRequestId/", string.Empty).ToLower().Split("%2f", StringSplitOptions.RemoveEmptyEntries).First();
                        var projectInfo = _projectsCustomWrapper.GetProject(repoGuidId);

                        try
                        {
                            prInfo = _pullRequestsCustomWrapper.GetPullRequestById(projectInfo.Name, prId);

                            string prWebUrl = $@"{prInfo.Repository.WebUrl}/PullRequest/{prId}";

                            links_for_pullRequestIds.Add(prWebUrl);
                            prStatuses.Add(prInfo.Status.ToString());

                            if (!links_for_pullRequestIds.Contains(prInfo.Repository.Name))
                                repoNames.Add(prInfo.Repository.Name);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            throw;
                        }
                    }
                }
                else if (link.StartsWith(GitRef))
                {
                    string storyName = link.ToLower().Split("%2f", StringSplitOptions.RemoveEmptyEntries).Last();
                }
            }
        }

    }
}
