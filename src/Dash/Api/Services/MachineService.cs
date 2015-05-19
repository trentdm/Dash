using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dash.Api.Operations;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Dash.Api.Services
{
    public class MachineService : Service
    {
        private IDbConnectionFactory DbConnectionFactory { get; set; }
        private AuthUserSession UserSession
        {
            get
            {
                try { return base.SessionAs<AuthUserSession>(); }
                catch { return new AuthUserSession(); } //fallback for unittesting
            }
        }

        public MachineService(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
        }

        public MachineResponse Get(Machine request)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                var machines = request.Id == 0 ?
                    db.LoadSelect<Machine>()
                    : db.LoadSelect<Machine>(t => t.Id == request.Id);

                return new MachineResponse { Total = machines.Count, Results = machines.OrderBy(m => m.Name).ToList() };
            }
        }

        public MachineResponse Post(Machine request)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                db.Save(request, true);
            }

            return Get(request);
        }
    }
}
