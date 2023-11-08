using System.Data;
using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelOnlyToManyConvertStrategy<T> : ConvertStrategyContext<T>
{
    private readonly IEnumerable<DbColumn> _schema;
    private readonly ModelPropertyInformation[] _properties;
    private readonly KeyValuePair<PropertyInfo, ForeignTableAttribute>[] _foreignTableProperties;

    private IGrouping<object, IDataRecord>? _currentGroupRecord;

    public ModelOnlyToManyConvertStrategy(DbDataReader dataReader) : base(dataReader)
    {
        var type = typeof(T);

        _schema = dataReader.GetColumnSchema();
        _properties = type.GetModelPropertiesFromSchema(_schema).ToArray();
        _foreignTableProperties = type.GetForeignTableProperties();
    }

    protected override T? GetObject()
    {
        ArgumentNullException.ThrowIfNull(_currentGroupRecord);

        var firstReader = _currentGroupRecord.First();

        T newObject = Activator.CreateInstance<T>();

        foreach (var currentProperty in _properties)
        {
            currentProperty.SetDataReaderValue(newObject!, firstReader);
        }

        foreach (var currentForeignTable in _foreignTableProperties)
        {
            var foreignTableConverter = new ForeignTableConverter(currentForeignTable, _schema);
            var result = default(object);

            if (currentForeignTable.IsCollection())
            {
                result = foreignTableConverter.GetObjects(_currentGroupRecord.AsEnumerable());
            }
            else
            {
                result = foreignTableConverter.GetObject(firstReader);
            }

            currentForeignTable.Key.SetValue(newObject, result);
        }

        return newObject;
    }

    public override IEnumerator<T?> GetEnumerator()
    {
        using (Reader)
        {
            var primaryKeyProperty = _properties.FirstOrDefault(currentProperty => currentProperty.Column.IsKey.GetValueOrDefault());

            if (primaryKeyProperty == null)
            {
                throw new InvalidCastException($"Нет первичного ключа в модели {typeof(T)} для получения коллекций внешних таблиц");
            }

            var records = Reader.Enumerate()
                .GroupBy(currentReader => currentReader.GetValue(primaryKeyProperty.Column.ColumnOrdinal!.Value))
                .ToArray();

            foreach (var currentRecord in records)
            {
                _currentGroupRecord = currentRecord;

                yield return GetObject();
            }
        }
    }
}