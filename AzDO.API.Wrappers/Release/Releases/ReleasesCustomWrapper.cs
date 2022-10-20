using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.Release.Releases
{
    public sealed class ReleasesCustomWrapper : ReleasesWrapper
    {
        public enum ServerType
        {
            AppServer,
            DatabaseServer
        }

        public ArtifactMetadata GetArtifactMetadata(string alias, string buildId, string buildNumber)
        {
            return new ArtifactMetadata
            {
                Alias = alias,
                InstanceReference = new BuildVersion()
                {
                    Id = buildId,
                    Name = buildNumber
                }
            };
        }

        public bool DoesReleaseExist(int definitionId, string buildNumber, out Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release latestRelease)
        {
            latestRelease = null;

            ReleaseExpands? releaseExpands = ReleaseExpands.Artifacts | ReleaseExpands.Environments;

            List<Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release> releases =
                GetReleases(definitionId, expand: releaseExpands);

            foreach (var release in releases)
            {
                string releaseBuildId = release.Artifacts.First().DefinitionReference["version"].Name;
                if (releaseBuildId.Equals(buildNumber.Trim()))
                {
                    latestRelease = release;
                    return true;
                }
            }
            return false;
        }

        public ReleaseEnvironment DeployStage(int definitionId, string buildId, string buildNumber, string stageName)
        {
            string buildAlias = null;

            if (stageName.ToUpper().StartsWith("VA"))
                buildAlias = "BuildOutput";
            else if (stageName.ToUpper().StartsWith("VS"))
                buildAlias = "sql-server";

            bool isReleaseExists = DoesReleaseExist(definitionId, buildNumber, out Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Release latestRelease);
            if (!isReleaseExists)
            {
                // Create new release
                var releaseStartMetadata = new ReleaseStartMetadata
                {
                    DefinitionId = definitionId,
                    Description = "Creating release",
                    Artifacts = new List<ArtifactMetadata>
                    {
                        GetArtifactMetadata(buildAlias, buildId, buildNumber),
                    }
                };

                latestRelease = CreateRelease(releaseStartMetadata);
            }

            // Deploy our stage in the release
            var releaseEnv = latestRelease.Environments.First(item => item.Name.Equals(stageName));
            if (releaseEnv.Status.Equals(EnvironmentStatus.InProgress))
            {
                throw new Exception($"The environment '{releaseEnv.Name}' that you are trying to deploy is currently in progress.");
            }

            var environmentUpdateData = new ReleaseEnvironmentUpdateMetadata
            {
                Status = EnvironmentStatus.InProgress
            };

            ReleaseEnvironment updatedReleaseEnv = UpdateReleaseEnvironment(environmentUpdateData, latestRelease.Id, releaseEnv.Id);
            return updatedReleaseEnv;
        }
    }
}
