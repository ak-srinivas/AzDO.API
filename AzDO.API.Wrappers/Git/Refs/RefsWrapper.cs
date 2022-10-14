using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Git.Refs
{
    public abstract class RefsWrapper : WrappersBase
    {
        /// <summary>
        /// Queries the provided repository for its refs and returns them.
        /// </summary>
        /// <param name="repositoryId"> The name or ID of the repository.</param>
        /// <param name="filter">[optional] A filter to apply to the refs (starts with).</param>
        /// <param name="includeLinks">[optional] Specifies if referenceLinks should be included in the result. default is false.</param>
        /// <param name="includeStatuses">[optional] Includes up to the first 1000 commit statuses for each ref. The default value is false.</param>
        /// <param name="includeMyBranches">[optional] Includes only branches that the user owns, the branches the user favorites, and the default branch. <br>The default value is false. Cannot be combined with the filter parameter.</param>
        /// <param name="latestStatusesOnly">[optional] True to include only the tip commit status for each ref. This option requires `includeStatuses` to be true. The default value is false.</param>
        /// <param name="peelTags">[optional] Annotated tags will populate the PeeledObjectId property. default is false.</param>
        /// <param name="filterContains">[optional] A filter to apply to the refs (contains).</param>
        /// <returns>Queries the provided repository for its refs and returns them.</returns>
        public List<GitRef> GetRefs(string repositoryId, string filter = null, bool includeLinks = false, bool includeStatuses = false, bool includeMyBranches = false, bool latestStatusesOnly = false, bool peelTags = false, string filterContains = null)
        {
            return GitClient.GetRefsAsync(GetProjectName(), repositoryId, filter, includeLinks, includeStatuses, includeMyBranches, latestStatusesOnly, peelTags, filterContains).Result;
        }
    }
}
