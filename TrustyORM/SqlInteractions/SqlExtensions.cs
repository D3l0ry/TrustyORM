using System.Data;
using System.Data.Common;

namespace TrustyORM.SqlInteractions;
internal static class SqlExtensions
{
    public static DbCommand CreateCommand(this DbConnection connection, string query)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        var command = connection.CreateCommand();
        command.CommandText = query;

        return command;
    }

    public static DbDataReader ExecuteReader(this DbConnection connection, string query)
    {
        var command = connection.CreateCommand(query);

        return command.ExecuteReader(CommandBehavior.KeyInfo);
    }
}