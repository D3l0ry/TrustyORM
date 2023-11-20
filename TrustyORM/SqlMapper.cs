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

            var command = connection.CreateCommand(query);
            var reader = command.ExecuteReader(CommandBehavior.KeyInfo);

            return ChoiceConvertStrategy<T?>.GetStrategy(reader);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Выполняет запрос к базе данных, используя "ленивое" преобразование без буфера
    /// </summary>
    /// <remarks>Запрос выполняется сразу при вызове метода, 
    /// но дальнейшее сопоставление типа будет проивзедено при дальнейшей итерации (Ленивое преобразование) для получения объектов.</remarks>
    /// <typeparam name="T">Тип элемента для преобразования значений из базы данных</typeparam>
    /// <param name="connection">Соединение к базе данных</param>
    /// <param name="query">Запрос</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T?> Query<T>(this DbConnection connection, string query, params DbParameter[] parameters)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var command = connection.CreateCommand(query, parameters);
            var reader = command.ExecuteReader(CommandBehavior.KeyInfo);

            return ChoiceConvertStrategy<T?>.GetStrategy(reader);
        }
        catch
        {
            throw;
        }
    }

    public static IEnumerable<T?> ExecuteProcedure<T>(this DbConnection connection, string procedure)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(procedure);

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var command = connection.CreateCommand(procedure, CommandType.StoredProcedure);
            var reader = command.ExecuteReader(CommandBehavior.KeyInfo);

            return ChoiceConvertStrategy<T?>.GetStrategy(reader);
        }
        catch
        {
            throw;
        }
    }

    public static IEnumerable<T?> ExecuteProcedure<T>(this DbConnection connection, string procedure, params DbParameter[] parameters)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(procedure);

        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var command = connection.CreateCommand(procedure, CommandType.StoredProcedure, parameters);
            var reader = command.ExecuteReader(CommandBehavior.KeyInfo);

            return ChoiceConvertStrategy<T?>.GetStrategy(reader);
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