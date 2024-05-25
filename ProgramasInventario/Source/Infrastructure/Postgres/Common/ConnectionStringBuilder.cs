using Domain;
using Npgsql;

namespace Infrastructure.Postgres.Common;

public class ConnectionStringBuilder : IConnectionStringBuilder
{
    public string Build() => new NpgsqlConnectionStringBuilder
    {
        Host = GlobalConfigurations.PostgresHost,
        Port = GlobalConfigurations.PostgresPort,
        Database = GlobalConfigurations.PostgresDatabase,
        Username = GlobalConfigurations.PostgresUsername,
        Password = GlobalConfigurations.PostgresPassword,
        Timeout = GlobalConfigurations.PostgresTimeout,
        CommandTimeout = GlobalConfigurations.PostgresTimeout
    }.ToString();
}