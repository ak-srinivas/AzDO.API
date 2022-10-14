using AzDO.API.Wrappers.WorkItemTracking.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Tests.WorkItemTracking.Comments
{
    [TestClass]
    public class GetCommentsTests : TestBase
    {
        private readonly CommentsCustomWrapper commentsCustomWrapper;

        public GetCommentsTests()
        {
            commentsCustomWrapper = new CommentsCustomWrapper();
        }

        [TestMethod]
        public void GetComments()
        {
            int workItemId = 143646;
            var commentList = commentsCustomWrapper.GetComments(workItemId);
            Assert.IsTrue(commentList != null, $"The comments were not found for work item with id '{workItemId}'!");



        }
    }
}
