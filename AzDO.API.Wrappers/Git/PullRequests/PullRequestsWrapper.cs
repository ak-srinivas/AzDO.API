using AzDO.API.Base.Common;
using System.Collections.Generic;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace AzDO.API.Wrappers.Git.PullRequests
{
    public abstract class PullRequestsWrapper : WrappersBase
    {
        /// <summary>
        /// Retrieve all pull requests matching a specified criteria.
        /// </summary>
        /// <param name="repositoryId">The repository ID of the pull request's target branch.</param>
        /// <param name="searchCriteria"> Pull requests will be returned that match this search criteria.</param>
        /// <param name="maxCommentLength"> Not used.</param>
        /// <param name="skip">The number of pull requests to ignore. For example, to retrieve results 101-150, set top to 50 and skip to 100.</param>
        /// <param name="top">The number of pull requests to retrieve.</param>
        /// <returns></returns>
        public List<GitPullRequest> GetPullRequests(string repositoryId, GitPullRequestSearchCriteria searchCriteria, int? maxCommentLength = null, int? skip = null, int? top = null)
        {
            return GitClient.GetPullRequestsAsync(GetProjectName(), repositoryId, searchCriteria, maxCommentLength, skip, top).Result;
        }

        /// <summary>
        /// Retrieve a pull request.
        /// </summary>
        /// <param name="pullRequestId">The ID of the pull request to retrieve.</param>
        /// <returns></returns>
        public GitPullRequest GetPullRequestById(string project, int pullRequestId)
        {
            return GitClient.GetPullRequestByIdAsync(GetProjectName(), pullRequestId).Result;
        }

        /// <summary>
        /// Update a pull request.
        /// Note: You can only update reviewers, descriptions, titles, merge status, and status.
        /// </summary>
        /// <param name="gitPullRequestToUpdate">The pull request content that should be updated.</param>
        /// <param name="project">Project ID or project name</param>
        /// <param name="repositoryId">The repository ID of the pull request's target branch.</param>
        /// <param name="pullRequestId">ID of the pull request to update.</param>
        /// <returns></returns>
        public GitPullRequest UpdatePullRequest(GitPullRequest gitPullRequestToUpdate, string project, string repositoryId, int pullRequestId)
        {
            return GitClient.UpdatePullRequestAsync(gitPullRequestToUpdate, project, repositoryId, pullRequestId).Result;
        }
    }
}
