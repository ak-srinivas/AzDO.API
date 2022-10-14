using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.Services.TestResults.WebApi;
using System;
using System.Collections.Generic;

namespace AzDO.API.Base.Common
{
    public class WrappersBase : AzDOBase
    {
        protected readonly BuildHttpClient BuildClient;
        protected readonly ProjectHttpClient ProjectClient;
        protected readonly ReleaseHttpClient ReleaseClient;
        protected readonly GitHttpClient GitClient;

        protected readonly WorkHttpClient WorkClient;
        protected readonly WorkItemTrackingHttpClient WorkItemTrackingClient;
        protected readonly TestManagementHttpClient TestManagementClient;
        protected readonly TestPlanHttpClient TestPlanClient;
        protected readonly TestResultsHttpClient TestResultsClient;

        protected const string GitCommit = "vstfs:///Git/Commit/";
        protected const string GitPullRequestId = "vstfs:///Git/PullRequestId/";
        protected const string GitRef = "vstfs:///Git/Ref/";

        public WrappersBase()
        {
            BuildClient = ServiceConnection.GetClient<BuildHttpClient>();
            ProjectClient = ServiceConnection.GetClient<ProjectHttpClient>();
            ReleaseClient = ServiceConnection.GetClient<ReleaseHttpClient>();
            GitClient = ServiceConnection.GetClient<GitHttpClient>();

            WorkClient = ServiceConnection.GetClient<WorkHttpClient>();
            WorkItemTrackingClient = ServiceConnection.GetClient<WorkItemTrackingHttpClient>();
            TestManagementClient = ServiceConnection.GetClient<TestManagementHttpClient>();
            TestPlanClient = ServiceConnection.GetClient<TestPlanHttpClient>();
            TestResultsClient = ServiceConnection.GetClient<TestResultsHttpClient>();
        }

        protected string GetEnumValueAsString<EnumType>(EnumType enumValue)
        {
            return Enum.GetName(typeof(EnumType), enumValue);
        }

        /// <summary>
        /// Returns a given work item type as a string.
        /// </summary>
        /// <param name="wit">An enum work item type.</param>
        /// <returns></returns>
        public static string GetWorkItemTypeNameAsString(WorkItemTypeEnum wit)
        {
            string type = null;
            switch (wit)
            {
                case WorkItemTypeEnum.Bug:
                    type = "Bug";
                    break;
                case WorkItemTypeEnum.CodeReviewRequest:
                    type = "Code Review Request";
                    break;
                case WorkItemTypeEnum.CodeReviewResponse:
                    type = "Code Review Response";
                    break;
                case WorkItemTypeEnum.Epic:
                    type = "Epic";
                    break;
                case WorkItemTypeEnum.Feature:
                    type = "Feature";
                    break;
                case WorkItemTypeEnum.FeedbackRequest:
                    type = "Feedback Request";
                    break;
                case WorkItemTypeEnum.FeedbackResponse:
                    type = "Feedback Response";
                    break;
                case WorkItemTypeEnum.SharedSteps:
                    type = "Shared Steps";
                    break;
                case WorkItemTypeEnum.Task:
                    type = "Task";
                    break;
                case WorkItemTypeEnum.TestCase:
                    type = "Test Case";
                    break;
                case WorkItemTypeEnum.TestPlan:
                    type = "Test Plan";
                    break;
                case WorkItemTypeEnum.TestSuite:
                    type = "Test Suite";
                    break;
                case WorkItemTypeEnum.UserStory:
                    type = "User Story";
                    break;
                case WorkItemTypeEnum.Issue:
                    type = "Issue";
                    break;
                case WorkItemTypeEnum.SharedParameter:
                    type = "Shared Parameter";
                    break;
            }
            return type;
        }

        public static class FieldNames
        {
            public const string SystemTitle = "System.Title";
            public const string SystemState = "System.State";
            public const string SystemWorkItemType = "System.WorkItemType";
            public const string SystemBoardColumn = "System.BoardColumn";
            public const string SystemLinkTypesHierarchyReverse = "System.LinkTypes.Hierarchy-Reverse";
            public const string SystemLinkTypesHierarchyForward = "System.LinkTypes.Hierarchy-Forward";
            public const string SystemAssignedTo = "System.AssignedTo";

            public const string MicrosoftVSTSCommonPriority = "Microsoft.VSTS.Common.Priority";
            public const string MicrosoftVSTSSchedulingStoryPoints = "Microsoft.VSTS.Scheduling.StoryPoints";
            public const string MicrosoftVSTSCommonStateChangeDate = "Microsoft.VSTS.Common.StateChangeDate";
            public const string MicrosoftVSTSTCMSteps = "Microsoft.VSTS.TCM.Steps";
            public const string MicrosoftVSTSCommonTestedByReverse = "Microsoft.VSTS.Common.TestedBy-Reverse";
        }

    }
}
