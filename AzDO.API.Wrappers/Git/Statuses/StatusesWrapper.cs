using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Git.Statuses
{
    public abstract class StatusesWrapper : WrappersBase
    {
        /// <summary>
        /// Get statuses associated with the Git commit.
        /// </summary>
        /// <param name="commitId">ID of the Git commit.</param>
        /// <param name="repositoryId">ID of the repository.</param>
        /// <param name="top">Optional. The number of statuses to retrieve. Default is 1000.</param>
        /// <param name="skip">Optional. The number of statuses to ignore. Default is 0. For example, to retrieve results 101-150, set top to 50 and skip to 100.</param>
        /// <param name="latestOnly">The flag indicates whether to get only latest statuses grouped by `Context.Name` and `Context.Genre`.</param>
        public List<GitStatus> GetStatuses(string commitId, string repositoryId, int? top = null, int? skip = null, bool? latestOnly = null)
        {
            return GitClient.GetStatusesAsync(GetProjectName(), commitId, repositoryId, top, skip, latestOnly).Result;
        }
    }
}
