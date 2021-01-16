using System;

namespace TestMVC.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string UserTable { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string UserTable { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
