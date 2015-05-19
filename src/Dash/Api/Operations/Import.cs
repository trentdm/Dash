using System.Collections.Generic;
using ServiceStack;

namespace Dash.Api.Operations
{
    [Route("/api/import", "GET POST")]
    public class Import
    {
        public List<Experience> Experiences { get; set; }
    }
    
    public class ImportResponse : ResponseStatus
    {
        public int Total { get; set; }
        public List<Import> Results { get; set; }
    }
}
