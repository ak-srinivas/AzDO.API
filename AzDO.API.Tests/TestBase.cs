using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AzDO.API.Tests
{
    public abstract class TestBase
    {
        public abstract class BuildDefinitions
        {
            public const string Pipeline1 = "Pipeline1"; // Input the name of your build pipeline
            public const string Pipeline2 = "Pipeline2"; // Input the name of your build pipeline
            public const string Pipeline3 = "Pipeline3"; // Input the name of your build pipeline
        }

        public abstract class ReleaseDefinitions
        {
            public const string Pipeline1 = "Pipeline1"; // Input the name of your release pipeline
            public const string Pipeline2 = "Pipeline2"; // Input the name of your release pipeline
            public const string Pipeline3 = "Pipeline3"; // Input the name of your release pipeline
        }

        public abstract class ProjectNames
        {
            public const string Ploceus = "Ploceus";
        }

        public abstract class Emails
        {
            public const string Srinivas = "srinivas.akkapeddi@neudesic.com";
            public const string Poojitha = "poojitha.kandati@neudesic.com";
            public const string Meenakshi = "meenakshi.rana@neudesic.com";
            public const string Anjani = "anjani.burra@neudesic.com";
        }

        // Email, VA Server Stage Name, VS Server Stage Name
        protected List<(string, string, string)> VRAServerInfo = new List<(string, string, string)>
        {
            (Emails.Srinivas, "Stage Name 1", "Stage Name 2"),
        };


        // TODO: Get this from release definitions API endpoint
        protected Dictionary<string, int> BuildDefs = new Dictionary<string, int>()
        {
            { BuildDefinitions.Pipeline1, 22 }, // Your pipeline name and it's definition id
            { BuildDefinitions.Pipeline2, 23 },
            { BuildDefinitions.Pipeline3, 24 },
        };

        protected Dictionary<string, int> ReleaseDefs = new Dictionary<string, int>()
        {
            { ReleaseDefinitions.Pipeline1, 11 }, // Your pipeline name and it's definition id
            { ReleaseDefinitions.Pipeline2, 12 },
            { ReleaseDefinitions.Pipeline3, 13 },
        };

        protected SortedSet<string> CommonRepoNames = new SortedSet<string>
        {
            "Repo1",
            "Repo2",
            "Repo3"
        };

        protected const string TeamBoardName = "Ploceus Team";
        protected const string TestPlanName = "Ploceus Module Test Plan";

        protected const string AREA = "Ploceus";
        protected string ITERATION = "Ploceus";

        protected const int TestPlanId = 53; // Your Test Plan Id
        protected const int DefaultTestSuiteId = 54; // Your Default Test Suite Id, Usually TestPlanId+1

        private const string _patFilePath = @"C:\Users\Srinivas.Akkapeddi\Desktop\DevOps\MyToken.txt";

        protected enum ServerType
        {
            AppServer,
            DatabaseServer
        }

        protected TestBase()
        {
            SetOrganizationName("NeuCTSolutions");
            SetProjectName(ProjectNames.Ploceus);
            SetPATTokenFilePath(_patFilePath);
        }

        protected string FilterVRAServerInfo(string forEmail = Emails.Srinivas, ServerType serverType = ServerType.AppServer)
        {
            foreach (var info in VRAServerInfo)
            {
                if (info.Item1.Equals(forEmail))
                {
                    switch (serverType)
                    {
                        case ServerType.AppServer:
                            return info.Item2;
                        case ServerType.DatabaseServer:
                            return info.Item3;
                    }
                }
            }
            return null;
        }

        private void SetOrganizationName(string orgName)
        {
            Environment.SetEnvironmentVariable("AzDO_ORGANIZATION_NAME", orgName, EnvironmentVariableTarget.Process);
        }

        private void SetProjectName(string projectName)
        {
            Environment.SetEnvironmentVariable("AzDO_PROJECT_NAME", projectName, EnvironmentVariableTarget.Process);
        }

        private void SetPATTokenFilePath(string patFilePath)
        {
            Environment.SetEnvironmentVariable("AzDO_PAT_FILE_PATH_FROM_USER", patFilePath, EnvironmentVariableTarget.Process);
        }
    }
}
