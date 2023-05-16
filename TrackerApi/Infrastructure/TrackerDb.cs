using System.Data.Common;
using Microsoft.Extensions.Logging;
using Npgsql;
using NPoco;
using TrackerApi.Helpers;

namespace TrackerApi.Infrastructure
{
    public interface IDbFactory
    {
        IDatabase GetConnection();

        IDatabase GetConnection(ILogger logger);
    }

    public class DbFactory : IDbFactory
    {
        private readonly string _connectionString;

        public DbFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDatabase GetConnection()
        {
            var db = new TrackerDb(_connectionString, DatabaseType.PostgreSQL, NpgsqlFactory.Instance);
            db.Mappers.Add(new JsonMapper());
            return db;
        }

        public IDatabase GetConnection(ILogger logger)
        {
            return new TrackerDb(_connectionString, DatabaseType.PostgreSQL, NpgsqlFactory.Instance, logger);
        }
    }

    public class TrackerDb : Database
    {
        private readonly ILogger _logger;

        public TrackerDb(string connectionString, DatabaseType databaseType, DbProviderFactory provider) : base(
            connectionString, databaseType, provider)
        {
        }

        public TrackerDb(string connectionString, DatabaseType databaseType, DbProviderFactory provider, ILogger logger)
            : base(connectionString, databaseType, provider)
        {
            _logger = logger;
        }

        protected override void OnExecutingCommand(DbCommand cmd)
        {
            _logger?.LogDebug(FormatCommand(cmd));
            base.OnExecutingCommand(cmd);
        }
    }
}