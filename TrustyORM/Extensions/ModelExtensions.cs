using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions;
internal static class ModelExtensions
{
    private static readonly Dictionary<Type, KeyValuePair<PropertyInfo, ColumnAttribute>[]> _modelPropertiesDictionary = new();
    private static readonly Dictionary<Type, KeyValuePair<PropertyInfo, ForeignTableAttribute>[]> _foreignTablePropertiesDictionary = new();

    public static KeyValuePair<PropertyInfo, ColumnAttribute>[] GetModelProperties(this Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (_modelPropertiesDictionary.TryGetValue(type, out var result))
        {
            return result;
        }

        var list = new List<KeyValuePair<PropertyInfo, ColumnAttribute>>();
        var properties = type.GetProperties();

        foreach (var currentProperty in properties)
        {
            var columnAttribute = currentProperty.GetCustomAttribute<ColumnAttribute>();

            if (columnAttribute != null)
            {
                columnAttribute.Name ??= currentProperty.Name;

                var newPropertyValuePair = new KeyValuePair<PropertyInfo, ColumnAttribute>(currentProperty, columnAttribute);

                list.Add(newPropertyValuePair);

                continue;
            }
        }

        var modelProperties = list.ToArray();
        _modelPropertiesDictionary.Add(type, modelProperties);

        return modelProperties;
    }

    public static KeyValuePair<PropertyInfo, ForeignTableAttribute>[] GetForeignTableProperties(this Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (_foreignTablePropertiesDictionary.TryGetValue(type, out var result))
        {
            return result;
        }

        var list = new List<KeyValuePair<PropertyInfo, ForeignTableAttribute>>();
        var properties = type.GetProperties();

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

    public static IEnumerable<ModelPropertyInformation> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (schema == null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        if (!schema.Any())
        {
            yield break;
        }

        var modelProperties = type.GetModelProperties();

        foreach (var currentProperty in modelProperties)
        {
            var columnAttribute = currentProperty.Value;
            var foundColumnSchema = schema.FirstOrDefault(currentColumn => currentColumn.ColumnName == columnAttribute.Name);

            if (foundColumnSchema == null)
            {
                throw new MissingFieldException($"Столбец с именем {currentProperty.Value.Name} не найден при получении полей внешней таблицы");
            }

            yield return new ModelPropertyInformation(currentProperty.Key, foundColumnSchema);
        }
    }

    public static IEnumerable<ModelPropertyInformation> GetModelPropertiesFromSchema(this Type type, IEnumerable<DbColumn> schema, string tableName)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (schema == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (tableName == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var selectedTableColumnsSchema = schema
            .Where(currentColumn => currentColumn.BaseTableName == tableName)
            .ToArray();

        return type.GetModelPropertiesFromSchema(selectedTableColumnsSchema);
    }

    public static IEnumerable<ForeignTableConverter> GetForeignTableConverters(this Type type, IEnumerable<DbColumn> schema)
    {
        var modelForeignTableProperties = type.GetForeignTableProperties();

        foreach (var currentProperty in modelForeignTableProperties)
        {
            yield return new ForeignTableConverter(currentProperty, schema);
        }
    }

    public static bool IsModelRelationOnlyToMany(this Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var foreignTableProperties = type.GetForeignTableProperties();

        foreach (var currentProperty in foreignTableProperties)
        {
            if (currentProperty.IsCollection())
            {
                return true;
            }
        }

        return false;
    }
}