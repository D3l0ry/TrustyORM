using System.Data.Common;
using System.Reflection;

namespace TrustyORM.Extensions;
internal static class TypeExtensions
{
    public static bool IsSystemType(this Type type)
    {
        Type[] systemTypes = new[]
        {
            typeof(bool),
            typeof(byte) ,
            typeof(sbyte) ,
            typeof(short),
            typeof(ushort),
            typeof(int) ,
            typeof(uint),
            typeof(long) ,
            typeof(ulong) ,
            typeof(float) ,
            typeof(double) ,
            typeof(decimal) ,
            typeof(string) ,
            typeof(DateTime),
            typeof(Guid)
        };

        return systemTypes.Any(currentType => currentType == type);
    }

    public static IEnumerable<KeyValuePair<PropertyInfo, ColumnAttribute>> GetModelProperties(this Type type)
    {
        IEnumerable<PropertyInfo> properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(currentProperty => currentProperty.IsDefined(typeof(ColumnAttribute)));

        foreach (PropertyInfo currentProperty in properties)
        {
            var currentAttribute = currentProperty.GetCustomAttribute<ColumnAttribute>();
            var newPropertyKeyValuePair = new KeyValuePair<PropertyInfo, ColumnAttribute>(currentProperty, currentAttribute);

            yield return newPropertyKeyValuePair;
        }
    }

    public static IEnumerable<KeyValuePair<PropertyInfo, ColumnAttribute>> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema)
    {
        if (schema == null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        if (!schema.Any())
        {
            yield break;
        }

        foreach (var currentProperty in type.GetModelProperties())
        {
            if (currentProperty.Value.IsForeignTable)
            {
                yield return currentProperty;
                continue;
            }

            bool tableNameIsNull = string.IsNullOrWhiteSpace(currentProperty.Value.Name);
            bool propertyExistsInSchema = schema
                .Any(currentColumn =>
                {
                    bool columnIsFound = currentColumn.ColumnName == currentProperty.Value.Name;

                    if (tableNameIsNull)
                    {
                        return columnIsFound;
                    }

                    return currentColumn.BaseTableName == currentProperty.Value.Name && columnIsFound;
                });

            if (propertyExistsInSchema)
            {
                yield return currentProperty;
            }
        }
    }

    public static IEnumerable<KeyValuePair<PropertyInfo, ColumnAttribute>> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema, string tableName)
    {
        if (schema == null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentNullException(nameof(tableName));
        }

        if (!schema.Any())
        {
            yield break;
        }

        var selectedTableColumnsSchema = schema
            .Where(currentColumn => currentColumn.BaseTableName == tableName)
            .ToArray();

        foreach (var currentProperty in type.GetModelProperties())
        {
            bool propertyExistsInSchema = selectedTableColumnsSchema
                .Any(currentColumn => currentColumn.ColumnName == currentProperty.Value.Name);

            if (propertyExistsInSchema)
            {
                yield return currentProperty;
            }
        }
    }
}