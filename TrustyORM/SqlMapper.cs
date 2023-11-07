using System.Data;
using System.Data.Common;
using TrustyORM.ModelInteractions;

namespace TrustyORM;
public static class SqlMapper
{
    /// <summary>
    /// Выполняет запрос к базе данных, используя "ленивое" преобразование без буфера
    /// </summary>
    /// <remarks>Запрос выполняется сразу при вызове метода, 
    /// но дальнейшее сопоставление типа будет проивзедено при дальнейшей итерации (Ленивое преобразование) для получения объектов.</remarks>
    /// <typeparam name="T">Тип элемента для преобразования значений из базы данных</typeparam>
    /// <param name="connection">Соединение к базе данных</param>
    /// <param name="query">Запрос</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T?> Query<T>(this DbConnection connection, string query)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var reader = connection.ExecuteReader(query);
            var converter = ChoiceConvertStrategy.GetStrategy<T?>(reader);

            return converter;
        }
        catch
        {
            throw;
        }
    }

    public static int Execute(this DbConnection connection, string query)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(query);

        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }

        var command = connection.CreateCommand(query);

        return command.ExecuteNonQuery();
    }
}