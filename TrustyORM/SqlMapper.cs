using System.Data;
using System.Data.Common;
using TrustyORM.ModelInteractions;
using TrustyORM.SqlInteractions;

namespace TrustyORM;
public static class SqlMapper
{
    public static IEnumerable<T> Query<T>(this DbConnection connection, string query)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var reader = connection.ExecuteReader(query);
            var converter = new ModelConverter<T>(reader);
            var isOnlyField = reader.VisibleFieldCount == 1;

            if (converter.IsSystemType && !isOnlyField)
            {
                throw new InvalidCastException($"Не удалось преобразовать значения из запроса в тип модели {converter.ElementType}");
            }

            return new ModelEnumerable<T>(reader);
            return converter.GetObjects();
        }
        catch
        {
            throw;
        }
    }

    public static void Execute(this DbConnection connection, string query)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }

        DbCommand command = connection.CreateCommand();
        command.CommandText = query;

        command.ExecuteNonQuery();
    }
}