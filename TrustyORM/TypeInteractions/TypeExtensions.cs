using System.Data.Common;
using System.Reflection;
using TrustyORM.TypeInteractions;

namespace TrustyORM.Extensions;
internal static class TypeExtensions
{
    public static bool IsSystemType(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

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
        ArgumentNullException.ThrowIfNull(type);

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(currentProperty => currentProperty.IsDefined(typeof(ColumnAttribute)))
            .Select(currentProperty =>
            {
                var columnAttribute = currentProperty.GetCustomAttribute<ColumnAttribute>();
                return new KeyValuePair<PropertyInfo, ColumnAttribute>(currentProperty, columnAttribute);
            });

        return properties;
    }

    public static IEnumerable<KeyValuePair<PropertyInfo, ForeignTableAttribute>> GetModelForeignTableProperties(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(currentProperty => currentProperty.IsDefined(typeof(ForeignTableAttribute)))
            .Select(currentProperty =>
            {
                var foreignTableAttribute = currentProperty.GetCustomAttribute<ForeignTableAttribute>();
                return new KeyValuePair<PropertyInfo, ForeignTableAttribute>(currentProperty, foreignTableAttribute);
            });

        return properties;
    }

    public static IEnumerable<KeyValuePair<MapperTypeInformation, ColumnAttribute>> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(schema);

        if (!schema.Any())
        {
            yield break;
        }

        foreach (var currentProperty in type.GetModelProperties())
        {
            var foundColumnSchema = schema.FirstOrDefault(currentColumn => currentColumn.ColumnName == currentProperty.Value.Name);

            if (foundColumnSchema == null)
            {
                continue;
            }

            var mapperTypeInformation = new MapperTypeInformation(currentProperty.Key, foundColumnSchema.ColumnOrdinal!.Value);

            yield return new KeyValuePair<MapperTypeInformation, ColumnAttribute>(mapperTypeInformation, currentProperty.Value);
        }
    }

    public static IEnumerable<KeyValuePair<MapperTypeInformation, ColumnAttribute>> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema, string tableName)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(tableName);

        var selectedTableColumnsSchema = schema
            .Where(currentColumn => currentColumn.BaseTableName == tableName)
            .ToArray();

        return type.GetModelPropertiesFromSchema(selectedTableColumnsSchema);
    }
}