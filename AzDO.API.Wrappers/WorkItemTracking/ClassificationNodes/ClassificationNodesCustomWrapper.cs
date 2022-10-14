using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzDO.API.Wrappers.WorkItemTracking.ClassificationNodes
{
    public sealed class ClassificationNodesCustomWrapper : ClassificationNodesWrapper
    {
        public WorkItemClassificationNode GetFullTree(TreeStructureGroup structureType)
        {
            WorkItemClassificationNode rootNode = GetClassificationNode(structureType, null, 1000);
            ShowNodeTree(rootNode);
            return rootNode;
        }

        public Dictionary<int, string> GetAreaPaths()
        {
            return GetPaths(TreeStructureGroup.Areas, "Area");
        }

        public Dictionary<int, string> GetIterationPaths()
        {
            return GetPaths(TreeStructureGroup.Iterations, "Iteration");
        }

        #region Private-Helpers

        private void ShowNodeTree(WorkItemClassificationNode node, string path = "")
        {
            path = path + "/" + node.Name;
            Console.WriteLine(path);

            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    ShowNodeTree(child, path);
                }
            }
        }

        private Dictionary<int, string> GetPaths(TreeStructureGroup structureType, string typeName)
        {
            WorkItemClassificationNode rootNode = GetClassificationNode(structureType, null, 1000);
            var allPaths = new Dictionary<int, string>();

            if (rootNode.Children.Count() > 0)
            {
                foreach (WorkItemClassificationNode childNode in rootNode.Children)
                {
                    GetIdAndPaths(childNode, typeName, allPaths);
                }

                return allPaths.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            return null;
        }

        private void GetIdAndPaths(WorkItemClassificationNode node, string typeName, Dictionary<int, string> allPaths)
        {
            allPaths.Add(node.Id, node.Path.Replace($"\\AutoBot\\{typeName}\\", "\\AutoBot\\"));

            if (node.Children == null)
                return;

            foreach (WorkItemClassificationNode child in node.Children)
            {
                GetIdAndPaths(child, typeName, allPaths);
            }
        }

        #endregion

    }
}
