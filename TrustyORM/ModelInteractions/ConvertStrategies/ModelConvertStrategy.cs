using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.Converters;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy<T> : IConvertStrategy<T>
{
    private readonly DbDataReader _dataReader;
    private readonly KeyValuePair<PropertyInfo, ColumnAttribute>[] _properties;
    private readonly KeyValuePair<PropertyInfo, ForeignTableAttribute>[] _foreignTableProperties;

    public ModelConvertStrategy(DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(dataReader);

        var type = typeof(T);
        _dataReader = dataReader;
        _properties = type.GetModelProperties();
        _foreignTableProperties = type.GetForeignTableProperties();
    }

    public T? Convert()
    {
        T? newObject = Activator.CreateInstance<T>();

        foreach (var currentProperty in _properties)
        {
            currentProperty.SetDataReaderValue(newObject, _dataReader);
        }

        foreach (var currentForeignTable in _foreignTableProperties)
        {
            var foreignTableConverter = new ForeignTableConverter(currentForeignTable, _dataReader);
            var result = foreignTableConverter.GetObject();

            currentForeignTable.Key.SetValue(newObject, result);
        }

        return newObject;
    }
}