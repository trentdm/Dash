using ServiceStack;

namespace Dash.Api.Operations
{
    [Route("/api/version", "GET")]
    public class Version
    {
        public double FullVersion { get { return 0.2; } }
    }

    public class VersionResponse : ResponseStatus
    {
        public Version Result { get; set; }
    }
}
