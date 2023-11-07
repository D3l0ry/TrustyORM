using System.Collections;
using System.Data;
using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ForeignTableConverter
{
    private readonly Type _type;
    private readonly MapperPropertyInformation[] _properties;

    public ForeignTableConverter(KeyValuePair<PropertyInfo, ForeignTableAttribute> property, IEnumerable<DbColumn> schema)
    {
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(schema);

        _type = property.Key.PropertyType.IsGenericType ? property.Key.PropertyType.GetGenericArguments()[0] : property.Key.PropertyType;
        _properties = _type
            .GetModelPropertiesFromSchema(schema, property.Value.TableName)
            .ToArray();
    }

    public object? GetObject(IDataRecord reader)
    {
        if (_properties.Length == 0)
        {
            return null;
        }

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

    public ICollection GetObjects(IEnumerable<IDataRecord> readers)
    {
        var collection = Array.CreateInstance(_type, readers.Count());
        var index = 0;

        foreach (var currentReader in readers)
        {
            collection.SetValue(GetObject(currentReader), index);
            index++;
        }

        return collection;
    }
}