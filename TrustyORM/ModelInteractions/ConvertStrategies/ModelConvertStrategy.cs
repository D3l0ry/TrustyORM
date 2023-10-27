using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy : IConvertStrategy
{
    private readonly Type _type;
    private readonly DbDataReader _dataReader;
    private readonly KeyValuePair<PropertyInfo, ColumnAttribute>[] _properties;

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
    }

    public ModelConvertStrategy(KeyValuePair<PropertyInfo, ColumnAttribute> property, DbDataReader dataReader)
    {
        if (dataReader == null)
        {
            throw new ArgumentNullException(nameof(dataReader));
        }

        _type = property.Key.PropertyType;
        _dataReader = dataReader;
        var schema = dataReader.GetColumnSchema();
        _properties = _type.GetModelPropertiesFromSchema(schema, property.Value.Name).ToArray();
    }

    public object Convert()
    {
        object newObject = Activator.CreateInstance(_type);

        foreach (KeyValuePair<PropertyInfo, ColumnAttribute> currentProperty in _properties)
        {
            if (currentProperty.Value.IsForeignTable)
            {
                var foreignTableConvertStrategy = new ModelConvertStrategy(currentProperty, _dataReader);
                var result = foreignTableConvertStrategy.Convert();

                currentProperty.Key.SetValue(newObject, result);
                continue;
            }

            currentProperty.SetDataReaderValue(newObject, _dataReader);
        }

        return newObject;
    }
}