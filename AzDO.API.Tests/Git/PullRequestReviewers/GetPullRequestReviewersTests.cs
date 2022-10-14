using AzDO.API.Wrappers.Git.PullRequestReviewers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Tests.Git.PullRequestReviewers
{
    [TestClass]
    public class GetPullRequestReviewersTests : TestBase
    {
        private readonly PullRequestReviewersCustomWrapper _pullRequestReviewersCustomWrapper;

        public GetPullRequestReviewersTests()
        {
            _pullRequestReviewersCustomWrapper = new PullRequestReviewersCustomWrapper();
        }
    }
}
