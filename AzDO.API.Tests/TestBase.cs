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

        protected const string TeamBoardName = "Your Board Name";
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

        /// <summary>
        /// Exports contents of any DataTable to a given file. 
        /// This is the most fastest way to export a data table of any size to a file. <br/><br/>
        /// <i>Tested with data tables having 5 million records and this function took 20 seconds to write to disk.</i>
        /// </summary>
        /// <param name="table">DataTable to fetch records.</param>
        /// <param name="targetFilePath">Destination file path.</param>
        /// <param name="delimiter">Default delimiter is comma(,).</param>
        protected void ConvertTableToFile(DataTable table, string targetFilePath, char delimiter = ',')
        {
            File.Delete(targetFilePath); // This will not throw an excpetion if file is not found.

            var fileStream = new FileStream(targetFilePath, FileMode.Create);
            using var bufstream = new BufferedStream(fileStream, 4096);
            using var swriter = new StreamWriter(bufstream);
            swriter.WriteLine(string.Join(delimiter, table.Columns.Cast<DataColumn>().Select(arg => arg.ColumnName)));
            foreach (DataRow dataRow in table.Rows)
            {
                string record = string.Join(delimiter, dataRow.ItemArray);
                swriter.WriteLine(record);
                bufstream.Flush();
            }
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
