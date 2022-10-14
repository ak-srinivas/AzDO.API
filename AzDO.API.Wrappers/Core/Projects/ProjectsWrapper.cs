using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.Core.WebApi;

namespace AzDO.API.Wrappers.Core.Projects
{
    public abstract class ProjectsWrapper : WrappersBase
    {
        /// <summary>
        ///  Get project with the specified id or name, optionally including capabilities.
        /// </summary>
        /// <param name="id">The name or id of the project.</param>
        /// <param name="includeCapabilities">Include capabilities (such as source control) in the team project result.</param>
        /// <param name="includeHistory">Search within renamed projects (that had such name in the past).</param>
        /// <returns>Project information with the specified id or name.</returns>
        public TeamProject GetProject(string id, bool? includeCapabilities = null, bool includeHistory = false)
        {
            return ProjectClient.GetProject(id, includeCapabilities, includeHistory).Result;
        }
    }
}
