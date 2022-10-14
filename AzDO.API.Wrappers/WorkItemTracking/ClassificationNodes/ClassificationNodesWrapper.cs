using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.WorkItemTracking.ClassificationNodes
{
    public abstract class ClassificationNodesWrapper : WrappersBase
    {
        /// <summary>
        /// Gets root classification nodes or list of classification nodes for a given list of nodes ids, for a given project. <br/>
        /// In case ids parameter is supplied you will get list of classification nodes for those ids. <br/>
        /// Otherwise you will get root classification nodes for this project.
        /// </summary>
        /// <param name="ids">List of integer classification nodes ids.</param>
        /// <param name="depth">Depth of children to fetch.</param>
        public List<WorkItemClassificationNode> GetClassificationNodes(IEnumerable<int> ids, int depth = 0)
        {
            return WorkItemTrackingClient.GetClassificationNodesAsync(GetProjectName(), ids, depth).Result;
        }

        /// <summary>
        /// Gets the classification node for a given node path.
        /// </summary>
        /// <param name="structureGroup">Structure group of the classification node, area or iteration.</param>
        /// <param name="path">Path of the classification node.</param>
        /// <param name="depth">Depth of children to fetch.</param>
        public WorkItemClassificationNode GetClassificationNode(TreeStructureGroup structureGroup, string path, int depth = 0)
        {
            return WorkItemTrackingClient.GetClassificationNodeAsync(GetProjectName(), structureGroup, path, depth).Result;
        }

        /// <summary>
        /// Gets root classification nodes under the project.
        /// </summary>
        /// <param name="depth">Depth of children to fetch.</param>
        public List<WorkItemClassificationNode> GetRootNodes(int? depth = null)
        {
            return WorkItemTrackingClient.GetRootNodesAsync(GetProjectName(), depth).Result;
        }

    }
}
