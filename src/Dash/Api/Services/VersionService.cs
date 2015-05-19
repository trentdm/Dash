using Dash.Api.Operations;
using ServiceStack;

namespace Dash.Api.Services
{
    public class VersionService : Service
    {
        public VersionResponse Get(Version request)
        {
            return new VersionResponse {Result = new Version()};
        }
    }
}
