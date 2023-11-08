using System.Data;
using System.Data.Common;

namespace TrustyORM;
public static class SqlExtensions
{
    public static DbCommand CreateCommand(this DbConnection connection, string query)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(query);

        var command = connection.CreateCommand();
        command.CommandText = query;

        return command;
    }

    public static DbDataReader ExecuteReader(this DbConnection connection, string query)
    {
        var command = connection.CreateCommand(query);

        return command.ExecuteReader();
    }

    public static DbDataReader ExecuteReader(this DbConnection connection, string query, CommandBehavior behavior)
    {
        var command = connection.CreateCommand(query);

        return command.ExecuteReader(behavior);
    }

    public static int ExecuteNonQuery(this DbConnection connection, string query)
    {
        var command = connection.CreateCommand(query);

        return command.ExecuteNonQuery();
    }

    public static IEnumerable<IDataRecord> Enumerate(this DbDataReader reader)
    {
        if (!reader.HasRows)
        {
            yield break;
        }

        foreach (IDataRecord currentReader in reader)
        {
            yield return currentReader;
        }
    }
}