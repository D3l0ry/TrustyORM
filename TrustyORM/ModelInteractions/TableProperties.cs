using System.Collections;
using System.Reflection;

namespace TrustyORM.ModelInteractions;

internal class TableProperties<T> : IEnumerable<KeyValuePair<PropertyInfo, ColumnAttribute?>>
{
    private readonly Type _tableType;
    private readonly KeyValuePair<PropertyInfo, ColumnAttribute?>[] _properties;

    public TableProperties()
    {
        _tableType = typeof(T);
        _properties = GetProperties().ToArray();
    }

    public KeyValuePair<PropertyInfo, ColumnAttribute?>[] Properties => _properties;

    private IEnumerable<KeyValuePair<PropertyInfo, ColumnAttribute?>> GetProperties()
    {
        IEnumerable<PropertyInfo> properties = _tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo currentProperty in properties)
        {
            ColumnAttribute currentAttribute = currentProperty.GetCustomAttribute<ColumnAttribute>();

            KeyValuePair<PropertyInfo, ColumnAttribute> newPropertyKeyValuePair =
                new KeyValuePair<PropertyInfo, ColumnAttribute>(currentProperty, currentAttribute);

            yield return newPropertyKeyValuePair;
        }
    }

    public KeyValuePair<PropertyInfo, ColumnAttribute?> GetProperty(int propertyIndex) => _properties[propertyIndex];

    public KeyValuePair<PropertyInfo, ColumnAttribute?> GetProperty(string propertyColumnName)
    {
        if (string.IsNullOrWhiteSpace(propertyColumnName))
        {
            throw new ArgumentNullException(nameof(propertyColumnName));
        }

        KeyValuePair<PropertyInfo, ColumnAttribute> selectedProperty = _properties
            .FirstOrDefault(currentProperty => currentProperty.Value.Name == propertyColumnName);

        if (selectedProperty.Key == null)
        {
            throw new KeyNotFoundException($"Поле {propertyColumnName} не найдено");
        }

        return selectedProperty;
    }

    public KeyValuePair<PropertyInfo, ColumnAttribute?> GetProperty(PropertyInfo property)
    {
        KeyValuePair<PropertyInfo, ColumnAttribute> selectedProperty = _properties
            .FirstOrDefault(currentProperty => currentProperty.Key == property);

        if (selectedProperty.Key == null)
        {
            throw new KeyNotFoundException($"Поле {property.Name} не найдено");
        }

        return selectedProperty;
    }

    public IEnumerator<KeyValuePair<PropertyInfo, ColumnAttribute?>> GetEnumerator() => _properties.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();
}