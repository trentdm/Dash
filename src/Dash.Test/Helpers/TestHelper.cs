using System;
using System.Collections.Generic;
using Dash.Api.Operations;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Dash.Test.Helpers
{
    public interface ITestHelper
    {
        void SetupTestDb(IDbConnectionFactory dbConnection);
        Machine GetStubMachine();
    }

    public class TestHelper : ITestHelper
    {
        public void SetupTestDb(IDbConnectionFactory dbConnection)
        {
            using (var db = dbConnection.OpenDbConnection())
            {
                db.CreateTableIfNotExists<Machine>();
            }
        }

        public Machine GetStubMachine()
        {
            return new Machine
            {
            };
        }
    }
}