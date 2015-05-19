using System.Collections.Generic;
using ServiceStack;

namespace Dash.Api.Operations
{
    [Route("/api/machine/summary", "GET POST")]
    [Authenticate(ApplyTo.Post | ApplyTo.Put | ApplyTo.Delete)]
    public class MachineSummary
    {
        public string Name { get; set; }
        public int Successes { get; set; }
        public int Failures { get; set; }
        public double SuccessRatio { get; set; }
        public Node Root { get; set; }

        public MachineSummary()
        {
            Root = new Node();
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public List<Node> Children { get; set; }

        public Node()
        {
            Children = new List<Node>();
        }
    }
    
    public class MachineSummaryResponse : ResponseStatus
    {
        public int Total { get; set; }
        public List<MachineSummary> Results { get; set; }
    }
}
