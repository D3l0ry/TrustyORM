using System.Data;
using System.Data.Common;
using TrustyORM.Exceptions;
using TrustyORM.ModelInteractions;

namespace TrustyORM;
public static class SqlExtensions
{
    public static IEnumerable<T> Query<T>(this DbConnection connection, string query) where T : new()
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (connection.State == ConnectionState.Closed)
        {
            throw new DbConnectionException("Попытка выполнить запрос с закрытым соединением");
        }

        DbCommand command = connection.CreateCommand();
        command.CommandText = query;

        DbDataReader reader = command.ExecuteReader();

        if (!reader.HasRows)
        {
            return Enumerable.Empty<T>();
        }

        return new ModelConverter<T>(reader);
    }
}