using System.Collections;
using System.Data;
using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ForeignTableConverter
{
    private readonly Type _type;
    private readonly ModelPropertyInformation[] _properties;

    public ForeignTableConverter(KeyValuePair<PropertyInfo, ForeignTableAttribute> property, IEnumerable<DbColumn> schema)
    {
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(schema);

        var propertyType = property.Key.PropertyType;

        if (propertyType.IsArray)
        {
            _type = propertyType.GetElementType()!;
        }
        else if (propertyType.IsGenericType)
        {
            _type = property.Key.PropertyType.GetGenericArguments().First();
        }
        else
        {
            _type = propertyType;
        }

        _properties = _type
            .GetModelPropertiesFromSchema(schema, property.Value.TableName)
            .ToArray();
    }

    private object? GetInternalObject(IDataRecord reader)
    {
        var newObject = Activator.CreateInstance(_type);

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

        var collection = Array.CreateInstance(_type, readers.Count());
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