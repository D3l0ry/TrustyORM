using System.Data;
using System.Data.Common;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelOnlyToManyConvertStrategy<T> : ModelConvertStrategyBase<T>
{
    private IGrouping<object, IDataRecord>? _currentGroupRecord;

    public ModelOnlyToManyConvertStrategy(DbDataReader dataReader) : base(dataReader) { }

    public override T? GetObject()
    {
        if (_currentGroupRecord == null)
        {
            throw new ArgumentNullException(nameof(_currentGroupRecord));
        }

        var firstReader = _currentGroupRecord.First();

        T newObject = Activator.CreateInstance<T>();

        foreach (var currentProperty in Properties)
        {
            currentProperty.SetDataReaderValue(newObject!, firstReader);
        }

        foreach (var currentConverter in ForeignTableConverters)
        {
            var result = default(object);

            if (currentConverter.IsCollection)
            {
                result = currentConverter.GetObjects(_currentGroupRecord.AsEnumerable());
            }
            else
            {
                result = currentConverter.GetObject(firstReader);
            }

            currentConverter.Property.SetValue(newObject, result);
        }

        return newObject;
    }

    public override IEnumerator<T?> GetEnumerator()
    {
        using (Reader)
        {
            var primaryKeyProperty = Properties.FirstOrDefault(currentProperty => currentProperty.Column.IsKey.GetValueOrDefault());

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