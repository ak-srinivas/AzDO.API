using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.WorkItemTracking.Queries
{
    public abstract class QueriesWrapper : WrappersBase
    {
        /// <summary>
        /// Creates a query, or moves a query.
        /// </summary>
        /// <param name="postedQuery">The query to create.</param>
        /// <param name="query">The parent id or path under which the query is to be created.</param>
        /// <param name="validateWiqlOnly">If you only want to validate your WIQL query without actually creating one, set it to true. <br/>
        /// Default is false.</param>
        /// <returns></returns>
        public QueryHierarchyItem CreateQuery(QueryHierarchyItem postedQuery, string query, bool? validateWiqlOnly = null)
        {
            return WorkItemTrackingClient.CreateQueryAsync(postedQuery, GetProjectName(), query, validateWiqlOnly).Result;
        }

        /// <summary>
        /// Retrieves an individual query and its children.
        /// </summary>
        /// <param name="query">ID or path of the query.</param>
        /// <param name="expand">Include the query string (wiql), clauses, query result columns, and sort options in the results.</param>
        /// <param name="depth">In the folder of queries, return child queries and folders to this depth.</param>
        /// <param name="includeDeleted">Include deleted queries and folders.</param>
        /// <returns></returns>
        public QueryHierarchyItem GetQuery(string query, QueryExpand expand = QueryExpand.All, int? depth = null, bool? includeDeleted = null)
        {
            return WorkItemTrackingClient.GetQueryAsync(GetProjectName(), query, expand, depth, includeDeleted).Result;
        }

        /// <summary>
        /// Gets a list of queries by ids. (Maximum 1000)
        /// </summary>
        /// <param name="queryGetRequest">Query get request.</param>
        public List<QueryHierarchyItem> GetQueriesBatch(QueryBatchGetRequest queryGetRequest)
        {
            return WorkItemTrackingClient.GetQueriesBatchAsync(queryGetRequest, GetProjectName()).Result;
        }

        /// <summary>
        /// Gets the root queries and their children
        /// </summary>
        /// <param name="expand">Include the query string (wiql), clauses, query result columns, and sort options in the results.</param>
        /// <param name="depth">In the folder of queries, return child queries and folders to this depth.</param>
        /// <param name="includeDeleted">Include deleted queries and folders.</param>
        public List<QueryHierarchyItem> ListQueries(QueryExpand expand = QueryExpand.All, int? depth = null, bool? includeDeleted = null)
        {
            return WorkItemTrackingClient.GetQueriesAsync(GetProjectName(), expand, depth, includeDeleted).Result;
        }

        /// <summary>
        /// Searches all queries the user has access to in the current project.
        /// </summary>
        /// <param name="filter">The text to filter the queries with.</param>
        /// <param name="top">The number of queries to return (Default is 50 and maximum is 200).</param>
        /// <param name="expand">Include the query string (wiql), clauses, query result columns, and sort options in the results.</param>
        /// <param name="includeDeleted">Include deleted queries and folders.</param>
        public QueryHierarchyItemsResult SearchQueries(string filter, int? top = null, QueryExpand expand = QueryExpand.All, bool? includeDeleted = null)
        {
            return WorkItemTrackingClient.SearchQueriesAsync(GetProjectName(), filter, top, expand, includeDeleted).Result;
        }

        /// <summary>
        /// Update a query or a folder. This allows you to update, rename and move queries and folders.
        /// </summary>
        /// <param name="queryUpdate">The query to update.</param>
        /// <param name="query">The ID or path for the query to update.</param>
        /// <param name="undeleteDescendants">Undelete the children of this folder. It is important to note that this will not bring back the permission changes that were previously applied to the descendants.</param>
        public QueryHierarchyItem UpdateQuery(QueryHierarchyItem queryUpdate, string query, bool? undeleteDescendants = null)
        {
            return WorkItemTrackingClient.UpdateQueryAsync(queryUpdate, GetProjectName(), query, undeleteDescendants).Result;
        }
    }
}
