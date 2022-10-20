using AzDO.API.Base.Common;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Release.Definitions
{
    public abstract class DefinitionsWrapper : WrappersBase
    {
        /// <summary>
        /// Get a list of release definitions.
        /// </summary>
        /// <param name="searchText">Get release definitions with names containing searchText.</param>
        /// <param name="expand">The properties that should be expanded in the list of Release definitions.</param>
        /// <param name="artifactType">Release definitions with given artifactType will be returned. Values can be Build, Jenkins, GitHub, Nuget, Team Build (external), ExternalTFSBuild, Git, TFVC, ExternalTfsXamlBuild.</param>
        /// <param name="artifactSourceId">Release definitions with given artifactSourceId will be returned.</param>
        /// <param name="top">Number of release definitions to get.</param>
        /// <param name="continuationToken">Gets the release definitions after the continuation token provided.</param>
        /// <param name="queryOrder">Gets the results in the defined order. Default is 'IdAscending'.</param>
        /// <param name="path">Gets the release definitions under the specified path.</param>
        /// <param name="isExactNameMatch">'true' to gets the release definitions with exact match as specified in searchText. Default is 'false'.</param>
        /// <param name="tagFilter">A comma-delimited list of tags. Only release definitions with these tags will be returned</param>
        /// <param name="propertyFilters">A comma-delimited list of extended properties to be retrieved.</param>
        /// <param name="definitionIdFilter">A comma-delimited list of release definitions to retrieve.</param>
        /// <param name="isDeleted">'true' to get release definitions that has been deleted. Default is 'false'</param>
        /// <param name="searchTextContainsFolderName">'true' to get the release definitions under the folder with name as specified in searchText. Default is 'false'.</param>
        public List<ReleaseDefinition> GetReleaseDefinitions(string searchText = null, ReleaseDefinitionExpands? expand = null, string artifactType = null, string artifactSourceId = null, int? top = null, string continuationToken = null, ReleaseDefinitionQueryOrder? queryOrder = null, string path = null, bool? isExactNameMatch = null, IEnumerable<string> tagFilter = null, IEnumerable<string> propertyFilters = null, IEnumerable<string> definitionIdFilter = null, bool? isDeleted = null, bool? searchTextContainsFolderName = null)
        {
            return ReleaseClient.GetReleaseDefinitionsAsync(GetProjectName(), searchText, expand, artifactType, artifactSourceId, top, continuationToken, queryOrder, path, isExactNameMatch, tagFilter, propertyFilters, definitionIdFilter, isDeleted, searchTextContainsFolderName).Result;
        }

        /// <summary>
        /// Get a release definition.
        /// </summary>
        /// <param name="definitionId">Id of the release definition.</param>
        /// <param name="propertyFilters">A comma-delimited list of extended properties to be retrieved. If set, the returned Release Definition will contain values for the specified property Ids (if they exist). If not set, properties will not be included.</param>
        public ReleaseDefinition GetReleaseDefinition(int definitionId, IEnumerable<string> propertyFilters = null)
        {
            return ReleaseClient.GetReleaseDefinitionAsync(GetProjectName(), definitionId, propertyFilters).Result;
        }
    }
}
