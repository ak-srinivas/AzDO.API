using AzDO.API.Wrappers.Git.PullRequests;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AzDO.API.Tests.Git.PullRequests
{
    [TestClass]
    public class GetPullRequestsTests : TestBase
    {
        private readonly PullRequestsCustomWrapper _pullRequestsCustomWrapper;

        public GetPullRequestsTests()
        {
            _pullRequestsCustomWrapper = new PullRequestsCustomWrapper();
        }

        [TestMethod]
        public void GetPullRequests()
        {
            var repositoryId = "YourRepoNameOrId";
            //var repositoryId = "YourRepoNameOrId";

            int? maxCommentLength = null;
            int? skip = null;
            int? top = null;

            var searchCriteria = new GitPullRequestSearchCriteria()
            {
                SourceRefName = "refs/heads/story/NameOfYourStory",
                TargetRefName = "refs/heads/develop"
            };
            List<GitPullRequest> pullRequests = _pullRequestsCustomWrapper.GetPullRequests(repositoryId, searchCriteria, maxCommentLength, skip, top);
            Console.WriteLine();
        }

        [TestMethod]
        public void GetPullRequestById()
        {
            int pullRequestId = 20740;
            GitPullRequest gitPullRequestToUpdate = _pullRequestsCustomWrapper.GetPullRequestById(ProjectNames.Ploceus, pullRequestId);
            Console.WriteLine();
        }
    }
}
