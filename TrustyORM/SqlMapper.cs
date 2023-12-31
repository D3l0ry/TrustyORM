﻿using System.Data;
using System.Data.Common;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions;

namespace TrustyORM;
public static class SqlMapper
{
    private static IEnumerable<T?> QueryImpl<T>(this DbCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        try
        {
            if (command.Connection!.State == ConnectionState.Closed)
            {
                command.Connection.Open();
            }

            var reader = command.ExecuteReader(CommandBehavior.KeyInfo);

            return ConvertStrategy<T?>.GetStrategy(reader);
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
    public static IEnumerable<T?> Query<T>(this DbConnection connection, string query)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var command = connection.CreateCommand(query);

        return command.QueryImpl<T>();
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
    public static IEnumerable<T?> Query<T>(this DbConnection connection, string query, params DbParameter[] arguments)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var command = connection.CreateCommand(query, arguments);

        return command.QueryImpl<T>();
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
    public static IEnumerable<T?> Query<T>(this DbConnection connection, string query, object arguments)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (arguments == null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        var command = connection
            .CreateCommand(query)
            .SetDbParameters(arguments);

        return command.QueryImpl<T>();
    }

    public static IEnumerable<T?> ExecuteProcedure<T>(this DbConnection connection, string procedure)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (procedure == null)
        {
            throw new ArgumentNullException(nameof(procedure));
        }

        var command = connection.CreateCommand(procedure, CommandType.StoredProcedure);

        return command.QueryImpl<T>();
    }

    public static IEnumerable<T?> ExecuteProcedure<T>(this DbConnection connection, string procedure, params DbParameter[] parameters)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (procedure == null)
        {
            throw new ArgumentNullException(nameof(procedure));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var command = connection.CreateCommand(procedure, CommandType.StoredProcedure, parameters);

        return command.QueryImpl<T>();
    }

    public static int Execute(this DbConnection connection, string query)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }

        var command = connection.CreateCommand(query);

        return command.ExecuteNonQuery();
    }
}