using AzDO.API.Base.Common;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using System;
using System.Collections.Generic;
using System.IO;

namespace AzDO.API.Wrappers.Release.Releases
{
    public abstract class ReleasesWrapper : WrappersBase
    {
        /// <summary>
        /// Create a release.
        /// </summary>
        /// <param name="releaseStartMetadata">Metadata to create a release.</param>
        public Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release CreateRelease(ReleaseStartMetadata releaseStartMetadata)
        {
            return ReleaseClient.CreateReleaseAsync(releaseStartMetadata, GetProjectName()).Result;
        }

        /// <summary>
        /// Get logs for a release Id.
        /// </summary>
        /// <param name="releaseId">Id of the release.</param>
        public Stream GetLogs(int releaseId)
        {
            return ReleaseClient.GetLogsAsync(GetProjectName(), releaseId).Result;
        }

        /// <summary>
        /// Get a Release
        /// </summary>
        /// <param name="releaseId">Id of the release.</param>
        /// <param name="approvalFilters">A filter which would allow fetching approval steps selectively based on whether <br> it is automated, or manual. This would also decide whether we should fetch pre <br> and post approval snapshots. Assumes All by default</param>
        /// <param name="propertyFilters">A comma-delimited list of extended properties to be retrieved. If set, the returned <br> Release will contain values for the specified property Ids (if they exist). If <br> not set, properties will not be included.</param>
        /// <param name="expand">A property that should be expanded in the release.</param>
        /// <param name="topGateRecords">Number of release gate records to get. Default is 5.</param>
        public Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release GetRelease(int releaseId, ApprovalFilters? approvalFilters = null, IEnumerable<string> propertyFilters = null, Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts.SingleReleaseExpands? expand = null, int? topGateRecords = null)
        {
            return ReleaseClient.GetReleaseAsync(GetProjectName(), releaseId, approvalFilters, propertyFilters, expand, topGateRecords).Result;
        }

        /// <summary>
        /// Get a release environment.
        /// </summary>
        /// <param name="releaseId">Id of the release.</param>
        /// <param name="environmentId">Id of the release environment.</param>
        public ReleaseEnvironment GetReleaseEnvironment(int releaseId, int environmentId)
        {
            return ReleaseClient.GetReleaseEnvironmentAsync(GetProjectName(), releaseId, environmentId).Result;
        }

        /// <summary>
        /// Gets the task log of a release as a plain text file.
        /// </summary>
        /// <param name="releaseId">Id of the release.</param>
        /// <param name="environmentId">Id of release environment.</param>
        /// <param name="releaseDeployPhaseId">Release deploy phase Id.</param>
        /// <param name="taskId">ReleaseTask Id for the log.</param>
        /// <param name="startLine">Starting line number for logs</param>
        /// <param name="endLine">Ending line number for logs</param>
        public Stream GetTaskLog(int releaseId, int environmentId, int releaseDeployPhaseId, int taskId, long? startLine = null, long? endLine = null)
        {
            return ReleaseClient.GetTaskLogAsync(GetProjectName(), releaseId, environmentId, releaseDeployPhaseId, taskId, startLine, endLine).Result;
        }

        /// <summary>
        /// Get a list of releases.
        /// </summary>
        /// <param name="definitionId">Releases from this release definition Id.</param>
        /// <param name="definitionEnvironmentId"></param>
        /// <param name="searchText">Releases with names containing searchText.</param>
        /// <param name="createdBy">Releases created by this user.</param>
        /// <param name="statusFilter">Releases that have this status.</param>
        /// <param name="environmentStatusFilter"></param>
        /// <param name="minCreatedTime">Releases that were created after this time.</param>
        /// <param name="maxCreatedTime">Releases that were created before this time.</param>
        /// <param name="queryOrder">Gets the results in the defined order of created date for releases. Default is descending.</param>
        /// <param name="top">Number of releases to get. Default is 50.</param>
        /// <param name="continuationToken">Gets the releases after the continuation token provided.</param>
        /// <param name="expand">The property that should be expanded in the list of releases.</param>
        /// <param name="artifactTypeId">Releases with given artifactTypeId will be returned. Values can be Build, Jenkins, GitHub, Nuget, Team Build (external), ExternalTFSBuild, Git, TFVC, ExternalTfsXamlBuild.</param>
        /// <param name="sourceId">Unique identifier of the artifact used. e.g. For build it would be {projectGuid}:{BuildDefinitionId}, <br> for Jenkins it would be {JenkinsConnectionId}:{JenkinsDefinitionId}, for TfsOnPrem <br> it would be {TfsOnPremConnectionId}:{ProjectName}:{TfsOnPremDefinitionId}. For <br> third-party artifacts e.g. TeamCity, BitBucket you may refer 'uniqueSourceIdentifier' <br> inside vss-extension.json https://github.com/Microsoft/vsts-rm-extensions/blob/master/Extensions.</param>
        /// <param name="artifactVersionId">Releases with given artifactVersionId will be returned. E.g. in case of Build artifactType, it is buildId.</param>
        /// <param name="sourceBranchFilter">Releases with given sourceBranchFilter will be returned.</param>
        /// <param name="isDeleted">Gets the soft deleted releases, if true.</param>
        /// <param name="tagFilter">A comma-delimited list of tags. Only releases with these tags will be returned.</param>
        /// <param name="propertyFilters">A comma-delimited list of extended properties to be retrieved. If set, the returned <br> Releases will contain values for the specified property Ids (if they exist). <br> If not set, properties will not be included. Note that this will not filter out <br> any Release from results irrespective of whether it has property set or not.</param>
        /// <param name="releaseIdFilter">A comma-delimited list of releases Ids. Only releases with these Ids will be returned.</param>
        /// <param name="path">Releases under this folder path will be returned</param>
        public List<Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release> GetReleases(int? definitionId = null, int? definitionEnvironmentId = null, string searchText = null, string createdBy = null, ReleaseStatus? statusFilter = ReleaseStatus.Active, int? environmentStatusFilter = null, DateTime? minCreatedTime = null, DateTime? maxCreatedTime = null, ReleaseQueryOrder? queryOrder = ReleaseQueryOrder.Descending, int? top = null, int? continuationToken = null, Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts.ReleaseExpands? expand = null, string artifactTypeId = "Build", string sourceId = null, string artifactVersionId = null, string sourceBranchFilter = null, bool? isDeleted = null, IEnumerable<string> tagFilter = null, IEnumerable<string> propertyFilters = null, IEnumerable<int> releaseIdFilter = null, string path = null)
        {
            return ReleaseClient.GetReleasesAsync(GetProjectName(), definitionId, definitionEnvironmentId, searchText, createdBy, statusFilter, environmentStatusFilter, minCreatedTime, maxCreatedTime, queryOrder, top, continuationToken, expand, artifactTypeId, sourceId, artifactVersionId, sourceBranchFilter, isDeleted, tagFilter, propertyFilters, releaseIdFilter, path).Result;
        }

        /// <summary>
        /// Update a complete release object.
        /// </summary>
        /// <param name="release">Release object for update.</param>
        /// <param name="releaseId">Id of the release to update.</param>
        public Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release UpdateRelease(Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release release, int releaseId)
        {
            return ReleaseClient.UpdateReleaseAsync(release, GetProjectName(), releaseId).Result;
        }

        /// <summary>
        /// Update the status of a release environment
        /// </summary>
        /// <param name="environmentUpdateData">Environment update meta data.</param>
        /// <param name="releaseId">Id of the release.</param>
        /// <param name="environmentId">Id of release environment.</param>
        public ReleaseEnvironment UpdateReleaseEnvironment(ReleaseEnvironmentUpdateMetadata environmentUpdateData, int releaseId, int environmentId)
        {
            return ReleaseClient.UpdateReleaseEnvironmentAsync(environmentUpdateData, GetProjectName(), releaseId, environmentId).Result;
        }

        /// <summary>
        /// Update few properties of a release.
        /// </summary>
        /// <param name="releaseUpdateMetadata">Properties of release to update.</param>
        /// <param name="releaseId">Id of the release to update.</param>
        public Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release UpdateReleaseResource(ReleaseUpdateMetadata releaseUpdateMetadata, int releaseId)
        {
            return ReleaseClient.UpdateReleaseResourceAsync(releaseUpdateMetadata, GetProjectName(), releaseId).Result;
        }
    }
}
