using System.IO;
using Dash.Api.Models;

namespace Dash.Api.Utils
{
    public static class EnvironmentSerializer
    {
        public static Environment GetEnvironment()
        {
            return new Environment {GmailUser = "user", GmailPass = "pass"};
            //this directory read is failing when deployed to vm, runs localy in debug and release mode fine
            //var json = File.ReadAllText("Resources\\environment.json");
            //return json.FromJson<Environment>();
        }
    }
}
