using AzDO.API.Wrappers.Git.Repositories;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.Git.Refs
{
    public sealed class RefsCustomWrapper : RefsWrapper
    {
        private readonly RepositoriesCustomWrapper _repositoriesCustomWrapper;

        public RefsCustomWrapper()
        {
            _repositoriesCustomWrapper = new RepositoriesCustomWrapper();
        }

        /// <summary>
        /// Gets repository names if a given sprint branch exists.
        /// </summary>
        /// <param name="branchName">The branch name that we need to find.</param>
        /// <returns>A sorted set of repository names</returns>
        public SortedSet<string> GetRepositoryNames_IfOurBranchExists(string projectName, string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
                throw new NullReferenceException("The parameter 'branchName' cannot be null or empty.");

            var repoNames = new SortedSet<string>();
            branchName = $"refs/heads/sprint/{branchName.Trim()}";

            // Get all git repository names in our project
            SortedSet<string> gitRepoNames = _repositoriesCustomWrapper.GetRepositoryNames(projectName);

            foreach (string gitRepoName in gitRepoNames)
            {
                // Get all branches in our git repository
                List<GitRef> branches = GetRefs(gitRepoName);

                if (branches != null && branches.Count > 0)
                {
                    // Get all branch names as a list
                    List<string> branchNames = branches.Select(item => item.Name).ToList();

                    // Find out if our name is in the list, if yes then save the repository name.
                    bool isBranchFound = branchNames.Any(item => item.Equals(branchName, StringComparison.OrdinalIgnoreCase));
                    if (isBranchFound)
                    {
                        repoNames.Add(gitRepoName);
                    }
                }
            }

            return repoNames;
        }
    }
}
