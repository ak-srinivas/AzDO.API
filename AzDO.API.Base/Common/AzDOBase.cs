using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.IO;

namespace AzDO.API.Base.Common
{
    public abstract class AzDOBase
    {
        public AzDOBase()
        {
            GetVssConnectionInstance();
        }

        internal protected static VssConnection ServiceConnection { get; set; }

        /// <summary>
        /// The host name for AzDO cloud.<br/>
        /// </summary>
        private const string _instance = @"https://dev.azure.com";

        /// <summary>
        /// The name of the Azure DevOps organization.<br/>
        /// </summary>
        private string _organization; // Input your organization name

        /// <summary>
        /// Project name.<br/>
        /// </summary>
        private string _project; // Input your project name

        internal protected VssConnection GetVssConnectionInstance()
        {
            _organization = Environment.GetEnvironmentVariable("AzDO_ORGANIZATION_NAME", EnvironmentVariableTarget.Process);
            _project = Environment.GetEnvironmentVariable("AzDO_PROJECT_NAME", EnvironmentVariableTarget.Process);

            if (ServiceConnection == null)
            {
                var instanceUri = new Uri($"{_instance}/{_organization}");
                ServiceConnection = new VssConnection(instanceUri, new VssBasicCredential(string.Empty, GetToken()));
            }
            return ServiceConnection;
        }

        protected string GetOrganizationName()
        {
            return _organization;
        }
        protected string GetProjectName()
        {
            return _project;
        }
        private static string GetToken()
        {
            string patFile = Environment.GetEnvironmentVariable("AzDO_ACCESSTOKEN_FROM_PIPELINE", EnvironmentVariableTarget.Process);
            patFile ??= Environment.GetEnvironmentVariable("AzDO_PAT_FILE_PATH_FROM_USER", EnvironmentVariableTarget.Process);
            if (string.IsNullOrWhiteSpace(patFile))
            {
                throw new NullReferenceException("Environment variable pointing to PAT token was null.");
            }
            else if (File.Exists(patFile))
            {
                return File.ReadAllText(patFile);
            }
            else
            {
                throw new NullReferenceException("Environment variable pointing to PAT token was null.");
            }
        }
    }
}
