using System.IO;
using Dash.Api.Operations;
using Dash.Api.Services;
using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using ServiceStack.Validation;

namespace Dash.AppStart
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("HttpListener Self-Host", typeof(VersionService).Assembly) { }

        public override void Configure(Container container)
        {
            EnableDiWiring(container);
            EnableLogging();
            EnableValidation(container);
            SetJsonCamelCase();
            EnableAutomaticContentReload();
            EnablePersistence(container);
            EnableAuthentication(container);
        }

        private void EnableDiWiring(Container container)
        {
            container.RegisterAutoWiredAs<DataService, IDataService>();
        }

        private void EnableValidation(Container container)
        {
            container.RegisterValidators(typeof(VersionService).Assembly);
            Plugins.Add(new ValidationFeature());
        }

        private static void SetJsonCamelCase()
        {
            JsConfig.EmitCamelCaseNames = true;
        }

        private void EnableLogging()
        {
            LogManager.LogFactory = new ConsoleLogFactory(debugEnabled: true);
            LogManager.LogFactory = new DebugLogFactory(debugEnabled: true);
            Plugins.Add(new RequestLogsFeature());
        }

        private void EnableAutomaticContentReload()
        {
            //for automatic reload of running content after saving changes in IDE
            SetConfig(new HostConfig
                {
                #if DEBUG
                    DebugMode = true,
                    WebHostPhysicalPath = Path.GetFullPath(Path.Combine("~".MapServerPath(), "..", "..")),
                #endif
                });
        }

        private static void EnablePersistence(Container container)
        {
            container.Register<IDbConnectionFactory>( //":memory:" for non-persistance, @"Data Source=dash.db;Version=3" for persistence
                new OrmLiteConnectionFactory(@"Data Source=dash.db;Version=3", SqliteDialect.Provider));

            using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
            {
            }
        }

        private void EnableAuthentication(Container container)
        {
            Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] {
                new BasicAuthProvider(), //Sign-in with Basic Auth
                new CredentialsAuthProvider() //HTML Form post of UserName/Password credentials
            }) {IncludeAssignRoleServices = false}); //Not utilizing roles at this time, so simplifying API.

            Plugins.Add(new RegistrationFeature());

            container.Register<ICacheClient>(new MemoryCacheClient());
            var userRep = new OrmLiteAuthRepository(container.Resolve<IDbConnectionFactory>());
            container.Register<IUserAuthRepository>(userRep);
            container.Resolve<IUserAuthRepository>().InitSchema();
        }
    }
}
