namespace Solid.TestTask.Config
{
    public class AppConfig
    {
        public Cbr Cbr { get; set; } = null!;

        public SqlServer SqlServer { get; set; } = null!;
    }

    public class Cbr
    {
        public Uri XmlDaily { get; set; } = null!;

        public Uri ValCursXsd { get; set; } = null!;
    }

    public class SqlServer
    {
        public string ConnectionString { get; set; } = null!;
    }
}
