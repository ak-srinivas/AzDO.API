using AzDO.API.Base.Common;
using System.Collections.Generic;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace AzDO.API.Wrappers.Git.Stats
{
    public abstract class StatsWrapper : WrappersBase
    {
        /// <summary>
        /// Retrieve statistics about all branches within a repository.
        /// </summary>
        /// <param name="repositoryId">The name or ID of the repository.</param>
        /// <param name="baseVersionDescriptor">Identifies the commit or branch to use as the base.</param>
        /// <returns></returns>
        public List<GitBranchStats> GetBranches(string repositoryId, GitVersionDescriptor baseVersionDescriptor = null)
        {
            return GitClient.GetBranchesAsync(GetProjectName(), repositoryId, baseVersionDescriptor).Result;
        }

        /// <summary>
        /// Retrieve statistics about a single branch.
        /// </summary>
        /// <param name="repositoryId">The name or ID of the repository.</param>
        /// <param name="name">Name of the branch.</param>
        /// <param name="baseVersionDescriptor">Identifies the commit or branch to use as the base.</param>
        /// <returns></returns>
        public GitBranchStats GetBranch(string repositoryId, string name, GitVersionDescriptor baseVersionDescriptor = null)
        {
            return GitClient.GetBranchAsync(GetProjectName(), repositoryId, name, baseVersionDescriptor).Result;
        }
    }
}
