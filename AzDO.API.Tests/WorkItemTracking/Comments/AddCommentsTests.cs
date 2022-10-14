using AzDO.API.Wrappers.WorkItemTracking.Comments;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using AzDO.API.Wrappers.Work.Iterations;
using System.Linq;

namespace AzDO.API.Tests.WorkItemTracking.Comments
{
    [TestClass]
    public class AddCommentsTests : TestBase
    {
        private readonly CommentsCustomWrapper commentsCustomWrapper;
        private readonly IterationsCustomWrapper _iterationsCustomWrapper;

        public AddCommentsTests()
        {
            commentsCustomWrapper = new CommentsCustomWrapper();
            _iterationsCustomWrapper = new IterationsCustomWrapper(TeamBoardName);
        }

        [TestMethod]
        public void Add_Ready_For_The_Demo_Comment()
        {
            string project = ProjectNames.YourProjectName;
            string text = "Ready for the demo!";

            const string sprintNumber = "2022.19";
            string sprintName = $"Sprint {sprintNumber}";
            string iterationName = $"Your Iteration Name {sprintName}";

            var teamContext = new TeamContext(project, TeamBoardName);
            List<TeamSettingsIteration> teamSettingsIterations = _iterationsCustomWrapper.GetTeamIterations(teamContext);
            Guid iterationId = teamSettingsIterations.Where(item => item.Name.Equals(iterationName)).Select(item => item.Id).FirstOrDefault();

            //List<int> storyIds = _iterationsCustomWrapper.GetQAWorkItemIds_InIteration_FilterBy_EmailIds(iterationId, new List<string>() { Emails.EmaildName1 });
            List<int> storyIds = new List<int>() { 164395 };

            foreach (int storyId in storyIds)
            {
                bool status = commentsCustomWrapper.IsCommentExists(storyId, text);
                if (!status)
                {
                    var request = new CommentCreate()
                    {
                        Text = text + " 😊"
                    };

                    var comment = commentsCustomWrapper.AddComment(request, storyId);
                    Assert.IsTrue(comment != null, $"The comment was not added to work item with id '{storyId}'!");
                }
            }
        }
    }
}
