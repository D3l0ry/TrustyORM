using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy<T> : ConvertStrategyContext<T>
{
    private readonly IEnumerable<DbColumn> _schema;
    private readonly MapperPropertyInformation[] _properties;
    private readonly KeyValuePair<PropertyInfo, ForeignTableAttribute>[] _foreignTableProperties;
    private readonly bool _foreignTableIsCollection;

    public ModelConvertStrategy(DbDataReader dataReader) : base(dataReader)
    {
        var type = typeof(T);

        _schema = dataReader.GetColumnSchema();
        _properties = type.GetModelPropertiesFromSchema(_schema).ToArray();
        _foreignTableProperties = type.GetForeignTableProperties();
        _foreignTableIsCollection = _foreignTableProperties
            .Any(currentProperty => currentProperty.Key.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>));
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

    private IEnumerator<T> GetObjectsEnumerator()
    {
        using (Reader)
        {
            var primaryKeyProperty = _properties.FirstOrDefault(currentProperty => currentProperty.Column.IsKey.GetValueOrDefault());

            if (primaryKeyProperty == null)
            {
                throw new InvalidCastException($"Нет первичного ключа в модели {typeof(T)} для получения внешних таблиц");
            }

            var records = Reader.Enumerate()
            .GroupBy(currentReader => currentReader.GetValue(primaryKeyProperty.Column.ColumnOrdinal!.Value))
            .ToArray();

            foreach (var currentRecord in records)
            {
                var firstReader = currentRecord.First();

                T newObject = Activator.CreateInstance<T>();

                foreach (var currentProperty in _properties)
                {
                    currentProperty.SetDataReaderValue(newObject!, firstReader);
                }

                foreach (var currentForeignTable in _foreignTableProperties)
                {
                    var foreignTableConverter = new ForeignTableConverter(currentForeignTable, _schema);
                    var result = foreignTableConverter.GetObjects(currentRecord.AsEnumerable());

                    currentForeignTable.Key.SetValue(newObject, result);
                }

                yield return newObject;
            }
        }
    }

    public override IEnumerator<T?> GetEnumerator()
    {
        if (_foreignTableIsCollection)
        {
            return GetObjectsEnumerator();
        }

        return base.GetEnumerator();
    }
}