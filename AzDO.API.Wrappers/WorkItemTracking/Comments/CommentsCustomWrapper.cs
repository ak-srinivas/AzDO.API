using System;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace AzDO.API.Wrappers.WorkItemTracking.Comments
{
    public sealed class CommentsCustomWrapper : CommentsWrapper
    {
        public bool IsCommentExists(int workItemId, string text)
        {
            CommentList commentList = GetComments(workItemId);

            foreach (Comment comment in commentList.Comments)
            {
                if (comment.Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
