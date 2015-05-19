using System.Linq;
using Dash.Api.Operations;
using Dash.Api.Services;
using Dash.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Dash.Test.Services
{
    [TestClass]
    public class ExperienceServiceTests
    {
        private ExperienceService Service { get; set; }
        private IDataService DataStore { get; set; }

        [TestInitialize]
        public void Setup()
        {
            DataStore = new DataService();
            Service = new ExperienceService(DataStore);
        }

        [TestMethod]
        public void TestDeserialization()
        {
            var response = Service.Get(new Experience());
            Assert.IsTrue(response.Results.Count() > 0, "More than 0 experiences.");
        }
    }
}
