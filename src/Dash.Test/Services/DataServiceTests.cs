using System.Linq;
using Dash.Api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dash.Test.Services
{
    [TestClass]
    public class DataServiceTests
    {
        private DataService Service { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Service = new DataService();
        }

        [TestMethod]
        public void TestGetExperiences()
        {
            var response = Service.GetExperiences();
            Assert.IsTrue(response.Any(), "More than 0 experiences.");
        }
    }
}
