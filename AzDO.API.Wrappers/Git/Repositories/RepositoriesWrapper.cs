using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.Git.Repositories
{
    public abstract class RepositoriesWrapper : WrappersBase
    {
        /// <summary>
        /// Retrieve git repositories.
        /// </summary>
        /// <param name="includeLinks">[optional] True to include reference links. The default value is false.</param>
        /// <param name="includeAllUrls">[optional] True to include all remote URLs. The default value is false.</param>
        /// <param name="includeHidden">[optional] True to include hidden repositories. The default value is false.</param>
        /// <returns></returns>
        public List<GitRepository> GetRepositories(string projectName, bool includeLinks = false, bool includeAllUrls = false, bool includeHidden = false)
        {
            return GitClient.GetRepositoriesAsync(projectName, includeLinks, includeAllUrls, includeHidden).Result.OrderBy(item => item.Name).ToList();
        }
    }
}
