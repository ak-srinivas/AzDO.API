using AzDO.API.Wrappers.Release.Definitions;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzDO.API.Tests.Release.Definitions
{
    [TestClass]
    public class GetDefinitionsTests : TestBase
    {
        private readonly DefinitionsCustomWrapper _definitionsCustomWrapper;

        public GetDefinitionsTests()
        {
            _definitionsCustomWrapper = new DefinitionsCustomWrapper();
        }

        [TestMethod]
        public void GetDefinition()
        {
            int definitionId = 12;
            ReleaseDefinition releaseDefinition = _definitionsCustomWrapper.GetReleaseDefinition(definitionId);
            Assert.IsTrue(releaseDefinition != null && releaseDefinition.Name.Equals("Your Pepeline Name"), "The release definition id was incorrect.");
        }
    }
}
