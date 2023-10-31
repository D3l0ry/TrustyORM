using System.Data.Common;
using System.Reflection;

namespace TrustyORM.ModelInteractions;
internal static class ModelExtensions
{
    private static readonly Dictionary<Type, KeyValuePair<PropertyInfo, ColumnAttribute>[]> _propertiesDictionary = new();
    private static readonly Dictionary<Type, KeyValuePair<PropertyInfo, ForeignTableAttribute>[]> _foreignTablePropertiesDictionary = new();

    public static KeyValuePair<PropertyInfo, ColumnAttribute>[] GetModelProperties(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (_propertiesDictionary.TryGetValue(type, out var result))
        {
            return result;
        }

        var list = new List<KeyValuePair<PropertyInfo, ColumnAttribute>>();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var currentProperty in properties)
        {
            var columnAttribute = currentProperty.GetCustomAttribute<ColumnAttribute>();

            if (columnAttribute != null)
            {
                list.Add(new KeyValuePair<PropertyInfo, ColumnAttribute>(currentProperty, columnAttribute));

                continue;
            }
        }

        var modelProperties = list.ToArray();
        _propertiesDictionary.Add(type, modelProperties);

        return modelProperties;
    }

    public static KeyValuePair<PropertyInfo, ForeignTableAttribute>[] GetForeignTableProperties(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (_foreignTablePropertiesDictionary.TryGetValue(type, out var result))
        {
            return result;
        }

        var list = new List<KeyValuePair<PropertyInfo, ForeignTableAttribute>>();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var currentProperty in properties)
        {
            var foreignTableAttribute = currentProperty.GetCustomAttribute<ForeignTableAttribute>();

            if (foreignTableAttribute != null)
            {
                list.Add(new KeyValuePair<PropertyInfo, ForeignTableAttribute>(currentProperty, foreignTableAttribute));

                continue;
            }
        }

        var modelProperties = list.ToArray();
        _foreignTablePropertiesDictionary.Add(type, modelProperties);

        return modelProperties;
    }

    public static IEnumerable<MapperPropertyInformation> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema)
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

            yield return new MapperPropertyInformation(currentProperty.Key, foundColumnSchema);
        }
    }

    public static IEnumerable<MapperPropertyInformation> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema, string tableName)
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