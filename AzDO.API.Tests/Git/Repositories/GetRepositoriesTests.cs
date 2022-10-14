using AzDO.API.Wrappers.Git.Repositories;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.Git.Repositories
{
    [TestClass]
    public class GetRepositoriesTests : TestBase
    {
        private readonly RepositoriesCustomWrapper _repositoriesCustomWrapper;

        public GetRepositoriesTests()
        {
            _repositoriesCustomWrapper = new RepositoriesCustomWrapper();
        }

        [TestMethod]
        public void GetRepositories()
        {
            bool includeLinks = false;
            bool includeAllUrls = false;
            bool includeHidden = false;

            List<GitRepository> gitRepos = _repositoriesCustomWrapper.GetRepositories(ProjectNames.YourProjectName, includeLinks, includeAllUrls, includeHidden);
            Assert.IsTrue(gitRepos != null && gitRepos.Count > 0, $"No git repositories were found.");
        }

        [TestMethod]
        public void GetRepositoryNames()
        {
            SortedSet<string> gitRepoNames = _repositoriesCustomWrapper.GetRepositoryNames(ProjectNames.YourProjectName);
            Assert.IsTrue(gitRepoNames.Count > 0, $"No git repositories were found.");
        }
    }
}
