using AzDO.API.Base.Common;

namespace AzDO.API.Wrappers.Build.Latest
{
    public abstract class LatestWrapper : WrappersBase
    {
        /// <summary>
        /// [Preview API] Gets the latest build for a definition, optionally scoped to aspecific branch.
        /// </summary>
        /// <param name="definition">definition name with optional leading folder path, or the definition id</param>
        /// <param name="branchName">optional parameter that indicates the specific branch to use</param>
        public Microsoft.TeamFoundation.Build.WebApi.Build GetLatestBuild(string definition, string branchName = null)
        {
            return BuildClient.GetLatestBuildAsync(GetProjectName(), definition, branchName).Result;
        }
    }
}
