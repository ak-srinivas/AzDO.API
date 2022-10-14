using AzDO.API.Wrappers.WorkItemTracking.ClassificationNodes;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.WorkItemTracking.ClassificationNodes
{
    [TestClass]
    public class GetClassificationNodesTests : TestBase
    {
        private readonly ClassificationNodesCustomWrapper _classificationNodesCustomWrapper;

        public GetClassificationNodesTests()
        {
            _classificationNodesCustomWrapper = new ClassificationNodesCustomWrapper();
        }

        [TestMethod]
        public void GetAreasFullTree()
        {
            WorkItemClassificationNode areasTree = _classificationNodesCustomWrapper.GetFullTree(TreeStructureGroup.Areas);
            Assert.IsTrue(areasTree.Id != 0, "No areas were found.");
        }

        [TestMethod]
        public void GetIterationsFullTree()
        {
            WorkItemClassificationNode iterationsTree = _classificationNodesCustomWrapper.GetFullTree(TreeStructureGroup.Iterations);
            Assert.IsTrue(iterationsTree.Id != 0, "No iterations were found.");
        }

        [TestMethod]
        public void GetAreaPaths()
        {
            Dictionary<int, string> areaPaths = _classificationNodesCustomWrapper.GetAreaPaths();
            Assert.IsTrue(areaPaths.Count > 0, "No area paths were found.");
        }

        [TestMethod]
        public void GetIterationPaths()
        {
            Dictionary<int, string> iterationPaths = _classificationNodesCustomWrapper.GetIterationPaths();
            Assert.IsTrue(iterationPaths.Count > 0, "No iteration paths were found.");
        }

        [TestMethod]
        public void GetRootNodes()
        {
            List<WorkItemClassificationNode> rootNodes = _classificationNodesCustomWrapper.GetRootNodes();
            Assert.IsTrue(rootNodes.Count > 0, "No root nodes were found.");
        }

        [TestMethod]
        public void GetClassificationNodes()
        {
            List<WorkItemClassificationNode> classificationNodes = _classificationNodesCustomWrapper.GetClassificationNodes(new List<int> { 2, 6 });
            Assert.IsTrue(classificationNodes.Count > 0, "No classification nodes were found.");
        }
    }
}
