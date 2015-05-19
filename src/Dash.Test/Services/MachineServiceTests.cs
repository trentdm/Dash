using Dash.Api.Services;
using Dash.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Dash.Test.Services
{
    [TestClass]
    public class MachineServiceTests
    {
        private MachineService Service { get; set; }
        private IDbConnectionFactory DbConnectionFactory { get; set; }
        private ITestHelper TestHelper { get; set; }

        [TestInitialize]
        public void Setup()
        {
            DbConnectionFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            TestHelper = new TestHelper();
            TestHelper.SetupTestDb(DbConnectionFactory);
            Service = new MachineService(DbConnectionFactory);
        }
    }
}
