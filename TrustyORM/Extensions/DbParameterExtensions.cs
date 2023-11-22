using System.Data.Common;

namespace TrustyORM.Extensions;
internal static class DbParameterExtensions
{
    public static DbCommand SetDbParameters(this DbCommand command, object obj)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(obj);

        var typeFields = obj.GetType().GetProperties();

        foreach (var currentField in typeFields)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = currentField.Name;
            parameter.Value = currentField.GetValue(obj);

            command.Parameters.Add(parameter);
        }

        return command;
    }
}