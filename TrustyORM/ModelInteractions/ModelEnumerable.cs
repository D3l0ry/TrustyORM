using System.Collections;
using System.Data.Common;

namespace TrustyORM.ModelInteractions;
internal class ModelEnumerable<T> : IEnumerable<T>
{
    private readonly DbDataReader _dataReader;
    private readonly ModelConverter<T> _converter;

    public ModelEnumerable(DbDataReader dataReader)
    {
        _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        _converter = new ModelConverter<T>(dataReader);
    }

    private IEnumerator<T> GetInternalEnumerator()
    {
        using DbDataReader reader = _dataReader;
        bool isOnlyField = _dataReader.FieldCount == 1;

        if (_converter.IsSystemType && !isOnlyField)
        {
            throw new InvalidCastException($"Не удалось преобразовать значения из запроса в тип модели {_converter.ElementType}");
        }

        if (!reader.HasRows)
        {
            yield break;
        }

        while (reader.Read())
        {
            yield return _converter.GetObject(reader);
        }
    }

    public IEnumerator<T> GetEnumerator() => GetInternalEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerator();
}