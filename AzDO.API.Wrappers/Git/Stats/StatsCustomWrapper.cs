using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Git.Stats
{
    public sealed class StatsCustomWrapper : StatsWrapper
    {
        /// <summary>
        /// Get all branch names in a given repository.
        /// </summary>
        /// <returns>A list of branch names in the given repository</returns>
        public SortedSet<string> GetBranchNames(string repositoryName, GitVersionDescriptor baseVersionDescriptor = null)
        {
            var repoBrancheNames = new SortedSet<string>();
            List<GitBranchStats> gitRepoBranches = GetBranches(repositoryName, baseVersionDescriptor);

            if (gitRepoBranches != null)
            {
                foreach (GitBranchStats girRepoBranch in gitRepoBranches)
                {
                    repoBrancheNames.Add(girRepoBranch.Name);
                }
            }

            return repoBrancheNames;
        }
    }
}
