﻿using System.Data;
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

    public static DbCommand CreateCommand(this DbConnection connection, string query, params DbParameter[] parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var command = connection.CreateCommand(query);
        command.Parameters.AddRange(parameters);

        return command;
    }

    public static DbCommand CreateCommand(this DbConnection connection, string query, CommandType commandType)
    {
        var command = connection.CreateCommand(query);
        command.CommandType = commandType;

        return command;
    }

    public static DbCommand CreateCommand(this DbConnection connection, string query, CommandType commandType, params DbParameter[] parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var command = connection.CreateCommand(query, commandType);
        command.Parameters.AddRange(parameters);

        return command;
    }

    public static IEnumerable<IDataRecord> Enumerate(this DbDataReader reader)
    {
        if (!reader.HasRows)
        {
            yield break;
        }

        foreach (IDataRecord currentRecord in reader)
        {
            yield return currentRecord;
        }
    }
}