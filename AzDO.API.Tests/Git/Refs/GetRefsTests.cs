using AzDO.API.Wrappers.Git.Refs;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace AzDO.API.Tests.Git.Refs
{
    [TestClass]
    public class GetRefsTests : TestBase
    {
        private readonly RefsCustomWrapper _refsCustomWrapper;

        public GetRefsTests()
        {
            _refsCustomWrapper = new RefsCustomWrapper();
        }

        [TestMethod]
        public void GetRefs()
        {
            string repositoryId = "Your Repo Name";
            string filter = null;
            bool includeLinks = false;
            bool includeStatuses = false;
            bool includeMyBranches = false;
            bool latestStatusesOnly = false;
            bool peelTags = false;
            string filterContains = null;

            List<GitRef> branches = _refsCustomWrapper.GetRefs(repositoryId, filter, includeLinks, includeStatuses, includeMyBranches, latestStatusesOnly, peelTags, filterContains);
            Assert.IsTrue(branches != null && branches.Count > 0, $"No branches were found in the given repository.");
        }

        [TestMethod]
        public void GetRepositoryNames_IfASprintBranchExists()
        {
            string branchName = $"SS-2021.20";
            string folderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Desktop\Test";

            string outputFile = $@"{folderPath}\Repo-Names-of-{branchName}-Branch.txt";
            const string arrow = " --> ";

            Directory.CreateDirectory(folderPath);
            File.Delete(outputFile);

            SortedSet<string> repoNames = _refsCustomWrapper.GetRepositoryNames_IfOurBranchExists(ProjectNames.Ploceus, branchName);
            if (repoNames.Count > 0)
            {
                using (StreamWriter swriter = new StreamWriter(outputFile, true))
                {
                    string allNames = branchName + arrow + string.Join(", ", repoNames);
                    if (allNames.EndsWith(arrow))
                        allNames = allNames.Replace(arrow, string.Empty);

                    swriter.WriteLine(allNames);
                }
            }
        }

        [TestMethod]
        public void GetRepositoryNames_IfSprintBranchExists()
        {
            string folderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Desktop\Test";
            string outputFile = $@"{folderPath}\Repo-Names-of-All-SS-2021-Branches.txt";
            const string arrow = " --> ";

            Directory.CreateDirectory(folderPath);
            File.Delete(outputFile);

            for (int i = 1; i <= 21; i++)
            {
                string branchName = $"SS-2021.{i}";

                SortedSet<string> repoNames = _refsCustomWrapper.GetRepositoryNames_IfOurBranchExists(ProjectNames.Ploceus, branchName);
                if (repoNames.Count > 0)
                {
                    using (StreamWriter swriter = new StreamWriter(outputFile, true))
                    {
                        string allNames = branchName + arrow + string.Join(", ", repoNames);
                        if (allNames.EndsWith(arrow))
                            allNames = allNames.Replace(arrow, string.Empty);

                        swriter.WriteLine(allNames);
                    }
                }
            }

            Assert.IsTrue(File.Exists(outputFile), "Failed to create a file with repository names where our branch exists.");
        }
    }
}
