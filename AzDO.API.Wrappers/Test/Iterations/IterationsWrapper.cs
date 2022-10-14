using AzDO.API.Base.Common;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using System.Collections.Generic;

namespace AzDO.API.Wrappers.Test.Iterations
{
    public abstract class IterationsWrapper : WrappersBase
    {
        /// <summary>
        /// Get iteration for a result.
        /// </summary>
        /// <param name="runId">ID of the test run that contains the result.</param>
        /// <param name="testCaseResultId">ID of the test result that contains the iterations.</param>
        /// <param name="iterationId">Id of the test results Iteration.</param>
        /// <param name="includeActionResults">Include result details for each action performed in the test iteration. <br/>
        /// ActionResults refer to outcome (pass/fail) of test steps that are executed as part of a running a manual test. <br/>
        /// Including the ActionResults flag gets the outcome of test steps in the actionResults section and test parameters in the parameters section for each test iteration.</param>
        public TestIterationDetailsModel GetTestIteration(int runId, int testCaseResultId, int iterationId, bool? includeActionResults = null)
        {
            return TestManagementClient.GetTestIterationAsync(GetProjectName(), runId, testCaseResultId, iterationId, includeActionResults).Result;
        }

        /// <summary>
        /// Get iterations for a result.
        /// </summary>
        /// <param name="runId">ID of the test run that contains the result.</param>
        /// <param name="testCaseResultId">ID of the test result that contains the iterations.</param>
        /// <param name="includeActionResults">Include result details for each action performed in the test iteration. <br/>
        /// ActionResults refer to outcome (pass/fail) of test steps that are executed as part of a running a manual test. <br/>
        /// Including the ActionResults flag gets the outcome of test steps in the actionResults section and test parameters in the parameters section for each test iteration.</param>
        public List<TestIterationDetailsModel> ListTestIterations(int runId, int testCaseResultId, bool? includeActionResults = null)
        {
            return TestManagementClient.GetTestIterationsAsync(GetProjectName(), runId, testCaseResultId, includeActionResults).Result;
        }
    }
}
