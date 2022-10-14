using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Git.Repositories
{
    public sealed class RepositoriesCustomWrapper : RepositoriesWrapper
    {
        /// <summary>
        /// Gets all Guids of all repositories in the current project.
        /// </summary>
        /// <returns>A dictionary of with key as repository name and value as it's Guid for all repositories in the current project</returns>
        public SortedDictionary<string, string> GetRepositoriesGuid(string projectName)
        {
            var repoGuids = new SortedDictionary<string, string>();

            List<GitRepository> gitRepositories = GetRepositories(projectName);

            if (gitRepositories != null)
            {
                foreach (GitRepository gitRepo in gitRepositories)
                {
                    repoGuids.Add(gitRepo.Name, gitRepo.Id.ToString());
                }
            }

            return repoGuids;
        }

        /// <summary>
        /// Gets all Guids of all repositories across a list of projects.
        /// </summary>
        /// <returns>A dictionary of with key as repository name and value as it's Guid for all repositories in a list of projects</returns>
        public SortedDictionary<string, string> GetRepositoriesGuid(List<string> projectNames)
        {
            var reposGuidAcrossAllProjects = new SortedDictionary<string, string>();

            foreach (string projectName in projectNames)
            {
                SortedDictionary<string, string> reposGuid = GetRepositoriesGuid(projectName);
                foreach (KeyValuePair<string, string> repoGuid in reposGuid)
                {
                    reposGuidAcrossAllProjects.Add(repoGuid.Key, repoGuid.Value);
                }
            }

            return reposGuidAcrossAllProjects;
        }

        /// <summary>
        /// Get all repository names in the current project.
        /// </summary>
        /// <returns>A list of repository names in the current project</returns>
        public SortedSet<string> GetRepositoryNames(string projectName)
        {
            var repoNames = new SortedSet<string>();
            List<GitRepository> gitRepositories = GetRepositories(projectName);

            if (gitRepositories != null)
            {
                foreach (GitRepository gitRepo in gitRepositories)
                {
                    repoNames.Add(gitRepo.Name);
                }
            }

            return repoNames;
        }
    }
}
