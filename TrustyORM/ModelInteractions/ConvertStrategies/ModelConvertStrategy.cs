using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy<T> : ConvertStrategyContext<T>
{
    private readonly IEnumerable<DbColumn> _schema;
    private readonly ModelPropertyInformation[] _properties;
    private readonly KeyValuePair<PropertyInfo, ForeignTableAttribute>[] _foreignTableProperties;

    public ModelConvertStrategy(DbDataReader dataReader) : base(dataReader)
    {
        var type = typeof(T);

        _schema = dataReader.GetColumnSchema();
        _properties = type.GetModelPropertiesFromSchema(_schema).ToArray();
        _foreignTableProperties = type.GetForeignTableProperties();
    }

    protected override T? GetObject()
    {
        T newObject = Activator.CreateInstance<T>();

        foreach (var currentProperty in _properties)
        {
            currentProperty.SetDataReaderValue(newObject!, Reader);
        }

        foreach (var currentForeignTable in _foreignTableProperties)
        {
            var foreignTableConverter = new ForeignTableConverter(currentForeignTable, _schema);
            var result = foreignTableConverter.GetObject(Reader);

            currentForeignTable.Key.SetValue(newObject, result);
        }

        return newObject;
    }
}