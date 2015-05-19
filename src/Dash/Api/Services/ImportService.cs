using System;
using System.Collections.Generic;
using System.IO;
using Dash.Api.Operations;
using ServiceStack;
using ServiceStack.Data;

namespace Dash.Api.Services
{
    public class ImportService : Service
    {
        private IDbConnectionFactory DbConnectionFactory { get; set; }

        public ImportService(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
        }

        public ImportResponse Get(Import request)
        {
            var experiences = new List<Import>();
            return new ImportResponse { Total = experiences.Count, Results = experiences };
        }

        public ImportResponse Post(Import import)
        {
            var experiences = import.Experiences;
            
            if (experiences.Count > 0)
                File.WriteAllText("Resources\\export.js", experiences.ToJson());
            else
                throw new ArgumentException("import");

            return new ImportResponse { Total = 1, Results = new List<Import>{import} };
        }
    }
}
