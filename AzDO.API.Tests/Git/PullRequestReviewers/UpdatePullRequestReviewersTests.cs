using AzDO.API.Wrappers.Git.PullRequestReviewers;
using AzDO.API.Wrappers.Git.PullRequests;
using AzDO.API.Wrappers.Work.Iterations;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Tests.Git.PullRequestReviewers
{
    [TestClass]
    public class UpdatePullRequestReviewersTests : TestBase
    {
        private readonly PullRequestReviewersCustomWrapper _pullRequestReviewersCustomWrapper;
        private readonly IterationsCustomWrapper _iterationsCustomWrapper;
        private readonly PullRequestsCustomWrapper _pullRequestsCustomWrapper;

        public UpdatePullRequestReviewersTests()
        {
            _pullRequestReviewersCustomWrapper = new PullRequestReviewersCustomWrapper();
            _pullRequestsCustomWrapper = new PullRequestsCustomWrapper();
            _iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
        }

        [TestMethod, Ignore]
        public void UpdatePullRequests_In_CurrentIteration()
        {
            const string GitPullRequestId = "vstfs:///Git/PullRequestId/";

            HashSet<WorkItem> currentWorkItemsSet = _iterationsCustomWrapper.GetWorkItems_InCurrentIteration();
            foreach (WorkItem witInfo in currentWorkItemsSet)
            {
                IList<string> artifactLinks = witInfo.Relations.Where(obj => obj.Rel.Equals("ArtifactLink")).Select(item => item.Url).ToList();
                if (artifactLinks != null && artifactLinks.Count > 0)
                {
                    foreach (string link in artifactLinks)
                    {
                        if (link.StartsWith(GitPullRequestId))
                        {
                            string prIdAsString = link.ToLower().Split("%2f", StringSplitOptions.RemoveEmptyEntries).Last();
                            int pullRequestId = Convert.ToInt32(prIdAsString);

                            if (pullRequestId.Equals(15976))
                            {
                                GitPullRequest gitPullRequestToUpdate = _pullRequestsCustomWrapper.GetPullRequestById(ProjectNames.YourProjectName, pullRequestId);
                                IdentityRefWithVote identityRefWithVotes = gitPullRequestToUpdate.Reviewers.Where(obj => obj.UniqueName.Equals(Emails.EmaildName1, StringComparison.OrdinalIgnoreCase)).Select(item => item).FirstOrDefault();

                                if (identityRefWithVotes != null && identityRefWithVotes.UniqueName.Equals(Emails.EmaildName1, StringComparison.OrdinalIgnoreCase))
                                {
                                    identityRefWithVotes.Vote = 10; // Approved
                                }

                                string projectName = gitPullRequestToUpdate.Repository.ProjectReference.Name;
                                string repositoryName = gitPullRequestToUpdate.Repository.Name;

                                GitPullRequest pullRequest = _pullRequestsCustomWrapper.UpdatePullRequest(gitPullRequestToUpdate, projectName, repositoryName, pullRequestId);
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }

            //GitPullRequest gitPullRequestToUpdate = null;
            //string project = ProjectNames.YourProjectName;

            //GitPullRequest pullRequest = _pullRequestsCustomWrapper.UpdatePullRequest(gitPullRequestToUpdate, project, repositoryId, pullRequestId);
            Console.WriteLine();
        }
    }
}
