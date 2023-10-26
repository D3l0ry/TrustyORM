using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy<T> : IConvertStrategy<T>
{
    private readonly DbDataReader _dataReader;
    private readonly KeyValuePair<PropertyInfo, ColumnAttribute>[] _properties;

    public ModelConvertStrategy(DbDataReader dataReader)
    {
        _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));

        var schema = dataReader.GetColumnSchema();
        _properties = typeof(T).GetModelPropertiesFromSchema(schema).ToArray();
    }

    public T Convert()
    {
        T newObject = Activator.CreateInstance<T>();

        foreach (KeyValuePair<PropertyInfo, ColumnAttribute> currentProperty in _properties)
        {
            currentProperty.SetDataReaderValue(newObject, _dataReader);
        }

        return newObject;
    }
}