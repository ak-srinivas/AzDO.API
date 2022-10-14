using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.Build.WebApi;
using System;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Build.Definitions
{
    public abstract class DefinitionsWrapper : WrappersBase
    {
        /// <summary>
        /// Gets a list of definitions.
        /// </summary>
        /// <param name="name">If specified, filters to definitions whose names match this pattern.</param>
        /// <param name="repositoryId">A repository ID. If specified, filters to definitions that use this repository.</param>
        /// <param name="repositoryType">If specified, filters to definitions that have a repository of this type.</param>
        /// <param name="queryOrder">Indicates the order in which definitions should be returned.</param>
        /// <param name="top">The maximum number of definitions to return.</param>
        /// <param name="continuationToken">A continuation token, returned by a previous call to this method, that can be used to return the next set of definitions.</param>
        /// <param name="minMetricsTimeInUtc">If specified, indicates the date from which metrics should be included.</param>
        /// <param name="definitionIds">A comma-delimited list that specifies the IDs of definitions to retrieve.</param>
        /// <param name="path">If specified, filters to definitions under this folder.</param>
        /// <param name="builtAfter">If specified, filters to definitions that have builds after this date.</param>
        /// <param name="notBuiltAfter">If specified, filters to definitions that do not have builds after this date.</param>
        /// <param name="includeAllProperties">Indicates whether the full definitions should be returned. By default, shallow representations of the definitions are returned.</param>
        /// <param name="includeLatestBuilds">Indicates whether to return the latest and latest completed builds for this definition.</param>
        /// <param name="taskIdFilter">If specified, filters to definitions that use the specified task.</param>
        /// <param name="processType">If specified, filters to definitions with the given process type.</param>
        /// <param name="yamlFilename">If specified, filters to YAML definitions that match the given filename.</param>
        public List<BuildDefinitionReference> GetBuildDefinition(string name = null, string repositoryId = null, string repositoryType = null, DefinitionQueryOrder? queryOrder = null, int? top = null, string continuationToken = null, DateTime? minMetricsTimeInUtc = null, IEnumerable<int> definitionIds = null, string path = null, DateTime? builtAfter = null, DateTime? notBuiltAfter = null, bool? includeAllProperties = null, bool? includeLatestBuilds = null, Guid? taskIdFilter = null, int? processType = null, string yamlFilename = null)
        {
            return BuildClient.GetDefinitionsAsync(GetProjectName(), name, repositoryId, repositoryType, queryOrder, top, continuationToken, minMetricsTimeInUtc, definitionIds, path, builtAfter, notBuiltAfter, includeLatestBuilds, taskIdFilter, processType, yamlFilename).Result;
        }
    }
}
