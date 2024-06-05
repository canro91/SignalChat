using Microsoft.Data.SqlClient;
using ServiceStack.OrmLite.Dapper;

namespace SignalChat.Database;

public class SqlDatabase
{
    private readonly string _database;

    private readonly SqlConnection _connection;

    public SqlDatabase(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        _database = builder.InitialCatalog;

        if (string.IsNullOrEmpty(_database))
        {
            throw new ArgumentException("The initial catalog in the connection string is not specified.");
        }

        builder.InitialCatalog = string.Empty;
        _connection = new SqlConnection(builder.ConnectionString);
    }

    public bool Exists()
    {
        IEnumerable<dynamic> results = _connection.Query<dynamic>("SELECT database_id, name FROM sys.databases WHERE name = @database", new { database = _database });

        return results.FirstOrDefault() != null;
    }

    public void Create()
    {
        _connection.Execute($"CREATE DATABASE [{_database}];");
    }
}