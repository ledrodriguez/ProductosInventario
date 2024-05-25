using System.Globalization;

namespace Domain;

public static class GlobalConfigurations
{
    public static readonly CultureInfo CultureInfo = new("pt-BR");

    public static bool IsDevelopment => ApplicationEnvironment == "development";

    public static string JwtAudience => Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "DEFAULT_JWT_AUDIENCE";

    public static string JwtIssuer => Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "DEFAULT_JWT_ISSUER";

    public static string JwtSecurityKey =>
        Environment.GetEnvironmentVariable("JWT_SECURITY_KEY") ?? "DEFAULT_JWT_SECURITY_KEY";

    public static string PostgresHost => Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "127.0.0.1";
    public static int PostgresPort => int.Parse(Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432");
    public static string PostgresDatabase => Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? "template";
    public static string PostgresUsername => Environment.GetEnvironmentVariable("POSTGRES_USERNAME") ?? "postgres";
    public static string PostgresPassword => Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";
    public static int PostgresTimeout => int.Parse(Environment.GetEnvironmentVariable("POSTGRES_TIMEOUT") ?? "60");

    private static string ApplicationEnvironment => Environment.GetEnvironmentVariable(
        "ASPNETCORE_ENVIRONMENT")?.Trim().ToLowerInvariant() ?? "development";
}