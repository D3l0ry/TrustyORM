using System.Data;
using System.Data.Common;
using TrustyORM.ModelInteractions;
using TrustyORM.SqlInteractions;

namespace TrustyORM;
public static class SqlMapper
{
    /// <summary>
    /// Выполняет запрос к базе данных, используя "ленивое" преобразование
    /// </summary>
    /// <remarks>Запрос выполняется сразу при вызове метода, 
    /// но дальнейшее сопоставление типа будет проивзедено при дальнейшей итерации (Ленивое преобразование) для получения объектов.</remarks>
    /// <typeparam name="T">Тип элемента для преобразования значений из базы данных</typeparam>
    /// <param name="connection">Соединение к базе данных</param>
    /// <param name="query">Запрос</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> Query<T>(this DbConnection connection, string query)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var reader = connection.ExecuteReader(query);

            return new ModelEnumerable<T>(reader);
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