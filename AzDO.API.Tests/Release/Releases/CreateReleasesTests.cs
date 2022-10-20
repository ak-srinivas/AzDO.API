using AzDO.API.Wrappers.Build.Latest;
using AzDO.API.Wrappers.Git.PullRequests;
using AzDO.API.Wrappers.Git.Statuses;
using AzDO.API.Wrappers.Release.Releases;
using AzDO.API.Wrappers.Work.Iterations;
using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Tests.Release.Releases
{
    [TestClass]
    public class CreateReleasesTests : TestBase
    {
        private readonly ReleasesCustomWrapper _releasesCustomWrapper;
        private readonly Wrappers.Build.Definitions.DefinitionsCustomWrapper _buildDefCustomWrapper;
        private readonly Wrappers.Release.Definitions.DefinitionsCustomWrapper _releaseDefCustomWrapper;

        private readonly IterationsCustomWrapper _iterationsCustomWrapper;
        private readonly LatestCustomWrapper _latestCustomWrapper;
        private readonly PullRequestsCustomWrapper _pullRequestsCustomWrapper;
        private readonly StatusesCustomWrapper _statusesCustomWrapper;
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;

        public CreateReleasesTests()
        {
            _releasesCustomWrapper = new ReleasesCustomWrapper();
            _buildDefCustomWrapper = new Wrappers.Build.Definitions.DefinitionsCustomWrapper();
            _releaseDefCustomWrapper = new Wrappers.Release.Definitions.DefinitionsCustomWrapper();

            _iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
            _latestCustomWrapper = new LatestCustomWrapper();
            _pullRequestsCustomWrapper = new PullRequestsCustomWrapper();
            _statusesCustomWrapper = new StatusesCustomWrapper();
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();

            #region Validate-Build-Definitions

            var buildDefinition = _buildDefCustomWrapper.GetBuildDefinition(BuildDefinitions.Pipeline1).First();
            Assert.IsTrue(buildDefinition != null && buildDefinition.Id.Equals(BuildDefs[BuildDefinitions.Pipeline1]), $"The build definition name '{BuildDefinitions.Pipeline1}' was incorrect.");

            buildDefinition = _buildDefCustomWrapper.GetBuildDefinition(BuildDefinitions.Pipeline2).First();
            Assert.IsTrue(buildDefinition != null && buildDefinition.Id.Equals(BuildDefs[BuildDefinitions.Pipeline2]), $"The build definition name '{BuildDefinitions.Pipeline2}' was incorrect.");

            buildDefinition = _buildDefCustomWrapper.GetBuildDefinition(BuildDefinitions.Pipeline3).First();
            Assert.IsTrue(buildDefinition != null && buildDefinition.Id.Equals(BuildDefs[BuildDefinitions.Pipeline3]), $"The build definition name '{BuildDefinitions.Pipeline3}' was incorrect.");

            #endregion

            #region Validate-Build-Definitions

            var releaseDefinition = _releaseDefCustomWrapper.GetReleaseDefinition(ReleaseDefs[ReleaseDefinitions.Pipeline1]);
            Assert.IsTrue(releaseDefinition != null && releaseDefinition.Name.Equals(ReleaseDefinitions.Pipeline1), $"The release definition id '{ReleaseDefs[ReleaseDefinitions.Pipeline1]}' was incorrect.");

            releaseDefinition = _releaseDefCustomWrapper.GetReleaseDefinition(ReleaseDefs[ReleaseDefinitions.Pipeline2]);
            Assert.IsTrue(releaseDefinition != null && releaseDefinition.Name.Equals(ReleaseDefinitions.Pipeline2), $"The release definition id '{ReleaseDefs[ReleaseDefinitions.Pipeline2]}' was incorrect.");

            releaseDefinition = _releaseDefCustomWrapper.GetReleaseDefinition(ReleaseDefs[ReleaseDefinitions.Pipeline3]);
            Assert.IsTrue(releaseDefinition != null && releaseDefinition.Name.Equals(ReleaseDefinitions.Pipeline3), $"The release definition id '{ReleaseDefs[ReleaseDefinitions.Pipeline3]}' was incorrect.");

            #endregion
        }

        [TestMethod]
        public void DeployDevLatest()
        {
            string branchName = "develop";

            string appServerStageName = FilterVRAServerInfo(Emails.Srinivas, ServerType.AppServer);
            string dbServerStageName = FilterVRAServerInfo(Emails.Srinivas, ServerType.DatabaseServer);

            var buildId1 = _latestCustomWrapper.GetLatestBuild(BuildDefinitions.Pipeline1, branchName);
            if (buildId1.Result.Equals(Microsoft.TeamFoundation.Build.WebApi.BuildResult.Succeeded))
                _releasesCustomWrapper.DeployStage(ReleaseDefs[ReleaseDefinitions.Pipeline1], buildId1.Id.ToString(), buildId1.BuildNumber, appServerStageName);

            var buildId2 = _latestCustomWrapper.GetLatestBuild(BuildDefinitions.Pipeline1, branchName);
            if (buildId2.Result.Equals(Microsoft.TeamFoundation.Build.WebApi.BuildResult.Succeeded))
                _releasesCustomWrapper.DeployStage(ReleaseDefs[ReleaseDefinitions.Pipeline2], buildId2.Id.ToString(), buildId2.BuildNumber, dbServerStageName);

            var buildId3 = _latestCustomWrapper.GetLatestBuild(BuildDefinitions.Pipeline1, branchName);
            if (buildId3.Result.Equals(Microsoft.TeamFoundation.Build.WebApi.BuildResult.Succeeded))
                _releasesCustomWrapper.DeployStage(ReleaseDefs[ReleaseDefinitions.Pipeline3], buildId3.Id.ToString(), buildId3.BuildNumber, appServerStageName);
        }

        [TestMethod]
        public void DeployPipeline1()
        {
            int definitionId = 11;
            string buildId = "123456";
            string buildNumber = "1.0.1234";
            string stageName = "Your Stage Name";
            ReleaseEnvironment releasedEnv = _releasesCustomWrapper.DeployStage(definitionId, buildId, buildNumber, stageName);
            Assert.IsTrue(releasedEnv.Name.Equals(stageName), "Wrong stage was deployed.");
        }
        

        [TestMethod]
        public void DeployFromStoryId()
        {
            int storyId = 123456;
            string branchType = "story";

            string appServerStageName = FilterVRAServerInfo(Emails.Srinivas, ServerType.AppServer);
            string dbServerStageName = FilterVRAServerInfo(Emails.Srinivas, ServerType.DatabaseServer);

            WorkItem storyInfo = _workItemsCustomWrapper.GetWorkItem(storyId);
            IList<string> artifactLinks = storyInfo.Relations.Where(obj => obj.Rel.Equals("ArtifactLink")).Select(item => item.Url).ToList();

            if (artifactLinks != null && artifactLinks.Count > 0)
            {
                _iterationsCustomWrapper.GetDataFromArtifacts(artifactLinks, out _, out List<int> pullRequestIds, out _, out _, getOnlyPRIds: true);
                if (pullRequestIds != null && pullRequestIds.Count > 0)
                {
                    foreach (int pullRequestId in pullRequestIds)
                    {
                        // Notes:
                        // lastMergeCommit --> The commit of the most recent pull request merge. If empty, the most recent merge is in progress or was unsuccessful.
                        // lastMergeSourceCommit --> The commit at the head of the source branch at the time of the last pull request merge.
                        // lastMergeTargetCommit --> The commit at the head of the target branch at the time of the last pull request merge.

                        GitPullRequest pullRequest = _pullRequestsCustomWrapper.GetPullRequestById(ProjectNames.Ploceus, pullRequestId);
                        string repoName = pullRequest.Repository.Name.ToLower().Trim();
                        string commitId = null;

                        if (branchType.ToLower().Trim().Equals("story"))
                        {
                            //commitId = pullRequest.LastMergeSourceCommit.CommitId;
                            commitId = pullRequest.LastMergeCommit.CommitId;
                        }
                        else if (branchType.ToLower().Trim().Equals("develop"))
                        {
                            commitId = pullRequest.LastMergeCommit.CommitId;
                        }
                        else
                            throw new NullReferenceException($"'{nameof(branchType)}' cannot be null or empty");

                        List<GitStatus> commitStatuses = _statusesCustomWrapper.GetStatuses(commitId, repoName);
                        GitStatus commitStatus = commitStatuses.FirstOrDefault(item => item.Description.Contains("#") && item.State.Equals(GitStatusState.Succeeded));
                        if (commitStatus == null)
                        {
                            throw new Exception($"The build id was not found for commit: '{commitStatus.Id}' in repo: '{repoName}' branch: ('{branchType}')");
                        }

                        string buildId = commitStatus.TargetUrl.Split("/").Last().Trim();
                        string buildNumber = commitStatus.Description.Split("#").Last().Replace("succeeded", "").Trim();

                        bool isRepoKnown = CommonRepoNames.Contains(repoName);

                        if (isRepoKnown)
                        {
                            if (repoName.Equals("repo1"))
                            {
                                ReleaseEnvironment releasedEnv = _releasesCustomWrapper.DeployStage(ReleaseDefs[ReleaseDefinitions.Pipeline1], buildId, buildNumber, appServerStageName);
                                Assert.IsTrue(releasedEnv.Name.Equals(appServerStageName), "Wrong stage was deployed.");
                            }
                            else if(repoName.Equals("repo2"))
                            {
                                ReleaseEnvironment releasedEnv = _releasesCustomWrapper.DeployStage(ReleaseDefs[ReleaseDefinitions.Pipeline3], buildId, buildNumber, appServerStageName);
                                Assert.IsTrue(releasedEnv.Name.Equals(appServerStageName), "Wrong stage was deployed.");
                            }
                            else if (repoName.Equals("repo3"))
                            {
                                ReleaseEnvironment releasedEnv = _releasesCustomWrapper.DeployStage(ReleaseDefs[ReleaseDefinitions.Pipeline2], buildId, buildNumber, dbServerStageName);
                                Assert.IsTrue(releasedEnv.Name.Equals(dbServerStageName), "Wrong stage was deployed.");
                            }
                        }
                        else
                        {
                            throw new Exception($"Unknown repository name '{repoName}'.");
                        }
                    }
                }
            }
        }
    }
}
