using Microsoft.Data.SqlClient;
using System;

namespace Server.Database
{
    public class DatabaseConfiguration
    {
        public string DbName { get; set; } = "funat";

        public DatabaseType DbType { get; set; } = DatabaseType.MSSQL;

        public string Host { get; set; } = "localhost";

        public bool DNS { get; set; }

        public ushort? Port { get; set; }

        public string Username { get; set; } = "root";

        public string Password { get; set; } = "@BestPasswordEUWest";

        public override string ToString()
        {
            switch (DbType)
            {
                case DatabaseType.MySQL:
                    return $"Server={Host}; {(Port != null ? $"Port={Port};" : string.Empty)} Database = {DbName}; " +
                        $"Uid = {Username}; Pwd = {Password}; SslMode = Preferred;";
                case DatabaseType.MSSQL:
                    return $"Server={Host},{(Port != null ? $"{Port}" : "1433")};User id={Username};Password={Password};Initial Catalog={DbName};";
                case DatabaseType.PostgreSQL:
                    return $"Host={Host};" +
                        $"{(Port != null ? $"Port={Port};" : string.Empty)}" +
                        $"Username={Username};Password={Password};Database={DbName}";
                /*case DatabaseType.MongoDB:
                    return $"mongodb{(DNS ? "+srv" : string.Empty)}://" +
                    $"{(string.IsNullOrWhiteSpace(Username) ? "" : Uri.EscapeUriString($"{Username}:{Password}"))}@" +
                    Uri.EscapeUriString($"{Host}") +
                    $"{(DNS || Port == null ? string.Empty : $":{Port}")}" +
                    "?retryWrites=true&w=majority";*/
                default:
                    return null;
            }
        }
    }
}
