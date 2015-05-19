using System.Collections.Generic;
using ServiceStack;

namespace Dash.Api.Operations
{
    [Route("/api/experience/summary", "GET POST")]
    [Authenticate(ApplyTo.Post | ApplyTo.Put | ApplyTo.Delete)] 
    public class ExperienceSummary
    {
        public string Setup { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public double SuccessRatio { get; set; }
    }
    
    public class ExperienceSummaryResponse : ResponseStatus
    {
        public int Total { get; set; }
        public List<ExperienceSummary> Results { get; set; }
    }
}
