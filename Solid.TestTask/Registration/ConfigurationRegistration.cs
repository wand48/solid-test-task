using Microsoft.Extensions.Configuration;
using Solid.TestTask.Config;
using Solid.TestTask.Helpers;

namespace Solid.TestTask.Registration
{
    public static class ConfigurationRegistration
    {
        private static AppConfig _config = null!;

        public static AppConfig Config => _config;

        public static void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            _config = new AppConfig();
            _config.Cbr = new Cbr();
            _config.SqlServer = new SqlServer();

            configuration
                .GetSection(nameof(Cbr))
                .Bind(_config.Cbr);

            configuration
                .GetSection(nameof(SqlServer))
                .Bind(_config.SqlServer);

            _config.Cbr.XmlDaily.Validate("для получения котировок на заданный день");
            _config.Cbr.ValCursXsd.Validate("для XSD схемы");
        }
    }
}
