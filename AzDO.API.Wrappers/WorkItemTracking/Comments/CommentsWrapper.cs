using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.WorkItemTracking.Comments
{
    public abstract class CommentsWrapper : WrappersBase
    {
        /// <summary>
        /// Add a comment on a work item.
        /// </summary>
        /// <param name="request">Comment create request.</param>
        /// <param name="workItemId">Id of a work item.</param>
        public Comment AddComment(CommentCreate request, int workItemId)
        {
            return WorkItemTrackingClient.AddCommentAsync(request, GetProjectName(), workItemId).Result;
        }

        /// <summary>
        /// Returns a list of work item comments, pageable
        /// </summary>
        /// <param name="workItemId">Id of a work item to get comments for.</param>
        /// <param name="top">Max number of comments to return.</param>
        /// <param name="continuationToken">Used to query for the next page of comments.</param>
        /// <param name="includeDeleted">Specify if the deleted comments should be retrieved.</param>
        /// <param name="expand">Specifies the additional data retrieval options for work item comments.</param>
        /// <param name="order">Order in which the comments should be returned.</param>
        public CommentList GetComments(int workItemId, int? top = null, string continuationToken = null, bool includeDeleted = false, CommentExpandOptions expand = CommentExpandOptions.All, CommentSortOrder order = CommentSortOrder.Desc)
        {
            return WorkItemTrackingClient.GetCommentsAsync(GetProjectName(), workItemId, top, continuationToken, includeDeleted, expand, order).Result;
        }

        /// <summary>
        /// Returns a work item comment.
        /// </summary>
        /// <param name="workItemId">Id of a work item to get the comment.</param>
        /// <param name="commentId">Id of the comment to return.</param>
        /// <param name="includeDeleted">Specify if the deleted comment should be retrieved.</param>
        /// <param name="expand">Specifies the additional data retrieval options for work item comments.</param>
        public Comment GetComment(int workItemId, int commentId, bool includeDeleted = false, CommentExpandOptions expand = CommentExpandOptions.All)
        {
            return WorkItemTrackingClient.GetCommentAsync(GetProjectName(), workItemId, commentId, includeDeleted, expand).Result;
        }
    }
}
