using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;
using TrustyORM.TypeInteractions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy : IConvertStrategy
{
    private readonly Type _type;
    private readonly DbDataReader _dataReader;
    private readonly KeyValuePair<MapperTypeInformation, ColumnAttribute>[] _properties;
    private readonly KeyValuePair<PropertyInfo, ForeignTableAttribute>[] _foreignTablesProperties;

    public ModelConvertStrategy(Type type, DbDataReader dataReader)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (dataReader == null)
        {
            throw new ArgumentNullException(nameof(dataReader));
        }

        _type = type;
        _dataReader = dataReader;
        var schema = dataReader.GetColumnSchema();
        _properties = type.GetModelPropertiesFromSchema(schema).ToArray();
        _foreignTablesProperties = type.GetModelForeignTableProperties().ToArray();
    }

    public ModelConvertStrategy(KeyValuePair<PropertyInfo, ForeignTableAttribute> property, DbDataReader dataReader)
    {
        if (dataReader == null)
        {
            throw new ArgumentNullException(nameof(dataReader));
        }

        _type = property.Key.PropertyType;
        _dataReader = dataReader;
        var schema = dataReader.GetColumnSchema();
        _properties = _type.GetModelPropertiesFromSchema(schema, property.Value.TableName).ToArray();
    }

    public object? Convert()
    {
        object? newObject = Activator.CreateInstance(_type);

        foreach (var currentProperty in _properties)
        {
            currentProperty.Key.SetDataReaderValue(newObject, _dataReader);
        }

        if (_foreignTablesProperties != null)
        {
            foreach (var currentForeignTable in _foreignTablesProperties)
            {
                var foreignTableConvertStrategy = new ModelConvertStrategy(currentForeignTable, _dataReader);
                var result = foreignTableConvertStrategy.Convert();

                currentForeignTable.Key.SetValue(newObject, result);
            }
        }

        return newObject;
    }
}