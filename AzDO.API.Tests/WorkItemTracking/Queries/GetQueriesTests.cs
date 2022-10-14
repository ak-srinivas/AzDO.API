using AzDO.API.Wrappers.WorkItemTracking.Queries;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzDO.API.Tests.WorkItemTracking.Queries
{
    [TestClass]
    public class GetFieldsTests : TestBase
    {
        private readonly QueriesCustomWrapper _queriesCustomWrapper;

        public GetFieldsTests()
        {
            _queriesCustomWrapper = new QueriesCustomWrapper();
        }

        [TestMethod, Ignore]
        public void CreateQuery()
        {
            QueryHierarchyItem postedQuery = null;
            string query = null;
            bool? validateWiqlOnly = null;

            QueryHierarchyItem queryHierarchyItem = _queriesCustomWrapper.CreateQuery(postedQuery, query, validateWiqlOnly);
            Assert.IsTrue(queryHierarchyItem != null, $"Unable to create query.");
        }

        [TestMethod, Ignore]
        public void GetQuery()
        {
            string query = null;
            QueryExpand expand = QueryExpand.All;
            int? depth = null;
            bool? includeDeleted = null;

            QueryHierarchyItem queryHierarchyItem = _queriesCustomWrapper.GetQuery(query, expand, depth, includeDeleted);
            Assert.IsTrue(queryHierarchyItem != null, $"Unable to get query.");
        }

        [TestMethod, Ignore]
        public void GetQueriesBatch()
        {
            QueryBatchGetRequest queryGetRequest = null;

            List<QueryHierarchyItem> queryHierarchyItems = _queriesCustomWrapper.GetQueriesBatch(queryGetRequest);
            Assert.IsTrue(queryHierarchyItems != null, $"Unable to get queries by batch.");
        }

        [TestMethod, Ignore]
        public void ListQueries()
        {
            QueryExpand expand = QueryExpand.All;
            int? depth = null;
            bool? includeDeleted = null;

            List<QueryHierarchyItem> queryHierarchyItems = _queriesCustomWrapper.ListQueries(expand, depth, includeDeleted);
            Assert.IsTrue(queryHierarchyItems.Count >= 0, $"No queries were found.");
        }

        [TestMethod, Ignore]
        public void SearchQueries()
        {
            string filter = null;
            int? top = null;
            QueryExpand expand = QueryExpand.All;
            bool? includeDeleted = null;

            QueryHierarchyItemsResult queryHierarchyItemsResult = _queriesCustomWrapper.SearchQueries(filter, top, expand, includeDeleted);
            Assert.IsTrue(queryHierarchyItemsResult != null, $"Could not find query.");
        }

        [TestMethod, Ignore]
        public void UpdateQuery()
        {
            QueryHierarchyItem queryUpdate = null;
            string query = null;
            bool? undeleteDescendants = null;

            QueryHierarchyItem queryHierarchyItem = _queriesCustomWrapper.UpdateQuery(queryUpdate, query, undeleteDescendants);
            Assert.IsTrue(queryHierarchyItem != null, $"Unable to update query.");
        }
    }
}
