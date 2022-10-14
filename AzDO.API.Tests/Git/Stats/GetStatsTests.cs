using AzDO.API.Wrappers.Git.Stats;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AzDO.API.Tests.Git.Stats
{
    [TestClass]
    public class GetStatsTests : TestBase
    {
        private readonly StatsCustomWrapper _statsCustomWrapper;

        public GetStatsTests()
        {
            _statsCustomWrapper = new StatsCustomWrapper();
        }

        [TestMethod]
        public void GetBranch()
        {
            var repositoryName = "Your Repo Name";
            var branchName = "story/Your Branch Name";
            GitVersionDescriptor baseVersionDescriptor = null;

            GitBranchStats repoBranchStats = _statsCustomWrapper.GetBranch(repositoryName, branchName, baseVersionDescriptor);
            Console.WriteLine();
        }

        [TestMethod]
        public void GetBranchNames()
        {
            var repositoryName = "Your Repo Name";
            //var repositoryName = "Your Repo Name";

            GitVersionDescriptor baseVersionDescriptor = null;
            SortedSet<string> branchNames = _statsCustomWrapper.GetBranchNames(repositoryName, baseVersionDescriptor);
            Console.WriteLine();
        }
    }
}
