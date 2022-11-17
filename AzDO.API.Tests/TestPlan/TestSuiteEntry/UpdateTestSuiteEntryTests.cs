using AzDO.API.Wrappers.TestPlan.TestSuiteEntry;
using AzDO.API.Wrappers.TestPlan.TestSuites;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AzDO.API.Tests.TestPlan.TestSuiteEntry
{
    [TestClass]
    public class UpdateTestSuiteEntryTests : TestBase
    {
        private readonly TestSuiteEntryCustomWrapper _testSuiteEntryCustomWrapper;
        private readonly TestSuitesCustomWrapper _testSuitesCustomWrapper;

        public UpdateTestSuiteEntryTests()
        {
            _testSuiteEntryCustomWrapper = new TestSuiteEntryCustomWrapper();
            _testSuitesCustomWrapper = new TestSuitesCustomWrapper();
        }

        [TestMethod]
        public void ReorderTestSuites()
        {
            int planId = 53;
            int suiteId = 289;
            int sequenceCount = 1;
            var suiteEntries = new List<SuiteEntryUpdateParams>();

            List<TestSuite> testSuites = _testSuitesCustomWrapper.GetTestSuitesWithinTestSuite(planId, suiteId);

            foreach (var testSuite in testSuites)
            {
                SuiteEntryUpdateParams updateParams = new SuiteEntryUpdateParams
                {
                    Id = testSuite.Id,
                    SuiteEntryType = SuiteEntryTypes.Suite,
                    SequenceNumber = sequenceCount++
                };
                suiteEntries.Add(updateParams);
            }

            List<SuiteEntry> orderedList = _testSuiteEntryCustomWrapper.ReorderSuiteEntries(suiteEntries, suiteId);
            Assert.IsTrue(testSuites != null, $"Failed to reorder test cases in test suite.");
        }

        [TestMethod, Ignore]
        public void ReorderSuiteEntries()
        {
            SuiteEntryUpdateParams first = new SuiteEntryUpdateParams
            {
                Id = 108882,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 1
            };

            SuiteEntryUpdateParams second = new SuiteEntryUpdateParams
            {
                Id = 104992,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 2
            };

            SuiteEntryUpdateParams third = new SuiteEntryUpdateParams
            {
                Id = 104986,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 3
            };

            SuiteEntryUpdateParams fourth = new SuiteEntryUpdateParams
            {
                Id = 104985,
                SuiteEntryType = SuiteEntryTypes.TestCase,
                SequenceNumber = 4
            };

            var suiteEntries = new List<SuiteEntryUpdateParams>
            {
                first, second, third, fourth
            };

            int suiteId = 104916;

            List<SuiteEntry> testSuites = _testSuiteEntryCustomWrapper.ReorderSuiteEntries(suiteEntries, suiteId);
            Assert.IsTrue(testSuites != null, $"Failed to reorder test cases in test suite.");
        }
    }
}
