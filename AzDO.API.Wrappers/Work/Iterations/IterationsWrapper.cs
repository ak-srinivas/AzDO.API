using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using System;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Work.Iterations
{
    public abstract class IterationsWrapper : WrappersBase
    {
        /// <summary>
        /// Get a team's iterations using timeframe filter
        /// </summary>
        /// <param name="teamContext">The team context for the operation</param>
        /// <param name="timeframe"> A filter for which iterations are returned based on relative time. <br>
        /// <i>Only Microsoft.TeamFoundation.Work.WebApi.TimeFrame.Current is supported currently.</i></param>
        /// <returns>A team's iterations using timeframe filter</returns>
        public List<TeamSettingsIteration> GetTeamIterations(TeamContext teamContext, string timeframe = null)
        {
            return WorkClient.GetTeamIterationsAsync(teamContext, timeframe).Result;
        }

        /// <summary>
        /// Get work items for iteration
        /// </summary>
        /// <param name="teamContext">The team context for the operation</param>
        /// <param name="iterationId">ID of the iteration</param>
        /// <returns>Work items for a given iteration</returns>
        public IterationWorkItems GetIterationWorkItems(TeamContext teamContext, Guid iterationId)
        {
            return WorkClient.GetIterationWorkItemsAsync(teamContext, iterationId).Result;
        }

    }
}
