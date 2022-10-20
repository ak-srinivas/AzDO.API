using AzDO.API.Wrappers.Git.PullRequests;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzDO.API.Tests.Git.PullRequests
{
    [TestClass]
    public class UpdatePullRequestsTests : TestBase
    {
        private readonly PullRequestsCustomWrapper _pullRequestsCustomWrapper;

        public UpdatePullRequestsTests()
        {
            _pullRequestsCustomWrapper = new PullRequestsCustomWrapper();
        }

        [TestMethod, Ignore]
        public void UpdatePullRequest()
        {
            //  Note: You can only update reviewers, descriptions, titles, merge status, and status.
            // Note: You cannot approve pull requests using this API.

            GitPullRequest gitPullRequestToUpdate = null;
            string project = ProjectNames.Ploceus;
            var repositoryId = "Your Repo Name";
            int pullRequestId = 0;

            GitPullRequest pullRequest = _pullRequestsCustomWrapper.UpdatePullRequest(gitPullRequestToUpdate, project, repositoryId, pullRequestId);
        }
    }
}
