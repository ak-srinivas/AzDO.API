using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Git.PullRequestReviewers
{
    public abstract class PullRequestReviewersWrapper : WrappersBase
    {
        /// <summary>
        /// Retrieve the reviewers for a pull request
        /// </summary>
        /// <param name="project">Project ID or project name</param>
        /// <param name="repositoryId">The repository ID of the pull request's target branch.</param>
        /// <param name="pullRequestId">ID of the pull request.</param>
        /// <returns></returns>
        public List<IdentityRefWithVote> GetPullRequestReviewers(string project, string repositoryId, int pullRequestId)
        {
            return GitClient.GetPullRequestReviewersAsync(project, repositoryId, pullRequestId).Result;
        }

        /// <summary>
        /// Edit a reviewer entry. These fields are patchable: isFlagged, hasDeclined
        /// NOTE: This endpoint only supports updating votes, but does not support updating required reviewers (use policy) or display names.
        /// </summary>
        /// <param name="reviewer">Reviewer data. If the reviewer's ID is included here, it must match the reviewerID parameter.</param>
        /// <param name="project">Project ID or project name</param>
        /// <param name="repositoryId">The repository ID of the pull request's target branch.</param>
        /// <param name="pullRequestId">ID of the pull request.</param>
        /// <param name="reviewerId">ID of the reviewer.</param>
        /// <returns></returns>
        public IdentityRefWithVote UpdatePullRequestReviewerAsync(IdentityRefWithVote reviewer, string project, string repositoryId, int pullRequestId, string reviewerId)
        {
            return GitClient.UpdatePullRequestReviewerAsync(reviewer, project, repositoryId, pullRequestId, reviewerId).Result;
        }
    }
}
