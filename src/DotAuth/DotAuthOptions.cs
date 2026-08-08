using DotAuth.Domain.Enums;
using DotAuth.Infrastructure.Options;

namespace DotAuth
{
    public sealed class DotAuthOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public DatabaseProvider Provider { get; set; }
        public JwtOptions Jwt { get; } = new();

        public void UsePostgreSql(string connectionString)
        {
            Provider = DatabaseProvider.PostgreSql;
            ConnectionString = connectionString;
        }
    }
}
