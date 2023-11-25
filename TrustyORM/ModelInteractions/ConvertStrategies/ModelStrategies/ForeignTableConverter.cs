using System.Collections;
using System.Data;
using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ForeignTableConverter
{
    private readonly PropertyInfo _property;
    private readonly Type _propertyBaseType;
    private readonly bool _isCollection;
    private readonly ModelPropertyInformation[] _properties;

    public ForeignTableConverter(KeyValuePair<PropertyInfo, ForeignTableAttribute> property, IEnumerable<DbColumn> schema)
    {
        if (property.Key == null)
        {
            throw new ArgumentNullException(nameof(property.Key));
        }

        if (schema == null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        _property = property.Key;
        var propertyType = property.Key.PropertyType;

        if (propertyType.IsArray)
        {
            _propertyBaseType = propertyType.GetElementType()!;
            _isCollection = true;
        }
        else if (propertyType.IsGenericType)
        {
            _propertyBaseType = propertyType.GetGenericArguments().First();
            _isCollection = true;
        }
        else
        {
            _propertyBaseType = propertyType;
        }

        _properties = _propertyBaseType
            .GetModelPropertiesFromSchema(schema, property.Value.TableName)
            .ToArray();
    }

    public PropertyInfo Property => _property;

    public bool IsCollection => _isCollection;

    private object? GetInternalObject(IDataRecord reader)
    {
        var newObject = Activator.CreateInstance(_propertyBaseType);

        foreach (var currentProperty in _properties)
        {
            var oridnal = currentProperty.Column.ColumnOrdinal!.Value;

            if (currentProperty.Column.AllowDBNull.GetValueOrDefault() && reader.IsDBNull(oridnal))
            {
                return null;
            }

            currentProperty.SetDataReaderValue(newObject, reader);
        }

        return newObject;
    }

    public object? GetObject(IDataRecord reader)
    {
        if (_properties.Length == 0)
        {
            return null;
        }

        return GetInternalObject(reader);
    }

    public ICollection? GetObjects(IEnumerable<IDataRecord> readers)
    {
        if (_properties.Length == 0)
        {
            return null;
        }

        var collection = Array.CreateInstance(_propertyBaseType, readers.Count());
        var index = 0;

        foreach (var currentReader in readers)
        {
            var value = GetInternalObject(currentReader);

            collection.SetValue(value, index);
            index++;
        }

        return collection;
    }
}