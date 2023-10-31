using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.Converters;
internal class ForeignTableConverter
{
    private readonly Type _type;
    private readonly DbDataReader _dataReader;
    private readonly MapperPropertyInformation[] _properties;

    public ForeignTableConverter(KeyValuePair<PropertyInfo, ForeignTableAttribute> property, DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(dataReader);

        _type = property.Key.PropertyType;
        _dataReader = dataReader;
        _properties = property.Key.PropertyType
            .GetModelPropertiesFromSchema(dataReader.GetColumnSchema(), property.Value.TableName)
            .ToArray();
    }

    public object? GetObject()
    {
        if (_properties.Length == 0)
        {
            return null;
        }

        var newObject = Activator.CreateInstance(_type);

        foreach (var currentProperty in _properties)
        {
            var oridnal = currentProperty.Column.ColumnOrdinal!.Value;

            if (currentProperty.Column.AllowDBNull.GetValueOrDefault() && _dataReader.IsDBNull(oridnal))
            {
                return null;
            }

            currentProperty.SetDataReaderValue(newObject, _dataReader);
        }

        return newObject;
    }
}