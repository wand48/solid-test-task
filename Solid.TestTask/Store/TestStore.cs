using Microsoft.Data.SqlClient;
using Solid.TestTask.Config;
using Solid.TestTask.Registration;

namespace Solid.TestTask.Store
{
    public class TestStore
    {
        protected readonly SqlConnection Connection;
        private readonly AppConfig _config;

        protected TestStore()
        {
            _config = ConfigurationRegistration.Config;

            Connection = new SqlConnection(_config.SqlServer.ConnectionString);
            Connection.Open();
        }
    }
}
