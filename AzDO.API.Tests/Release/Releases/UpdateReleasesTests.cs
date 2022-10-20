using AzDO.API.Wrappers.WorkItemTracking.WorkItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzDO.API.Tests.Release.Releases
{
    [TestClass]
    public class UpdateReleasesTests : TestBase
    {
        private readonly WorkItemsCustomWrapper _workItemsCustomWrapper;

        public UpdateReleasesTests()
        {
            _workItemsCustomWrapper = new WorkItemsCustomWrapper();
        }
    }
}
