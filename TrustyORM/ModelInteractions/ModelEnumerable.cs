using System.Collections;
using System.Data.Common;

namespace TrustyORM.ModelInteractions;
internal class ModelEnumerable<T> : IEnumerable<T>
{
    private readonly DbDataReader _dataReader;

    public ModelEnumerable(DbDataReader dataReader)
    {
        _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
    }

    private IEnumerator<T> GetInternalEnumerator()
    {
        var converter = new ModelConverter<T>(_dataReader);
        var isOnlyField = _dataReader.VisibleFieldCount == 1;

        if (converter.IsSystemType && !isOnlyField)
        {
            throw new InvalidCastException($"Не удалось преобразовать значения из запроса в тип модели {converter.ElementType}");
        }

        if (!_dataReader.HasRows)
        {
            yield break;
        }

        while (_dataReader.Read())
        {
            yield return converter.GetObject();
        }
    }

    public IEnumerator<T> GetEnumerator() => GetInternalEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerator();
}
