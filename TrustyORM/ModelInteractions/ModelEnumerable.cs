using System.Collections;
using System.Data.Common;

namespace TrustyORM.ModelInteractions;
internal class ModelEnumerable<T> : IEnumerable<T>
{
    private readonly DbDataReader _dataReader;

    public ModelEnumerable(DbDataReader dataReader)
    {
        if (dataReader == null)
        {
            throw new ArgumentNullException(nameof(dataReader));
        }

        _dataReader = dataReader;
    }

    private IEnumerator<T> GetInternalEnumerator()
    {
        using var reader = _dataReader;
        var converter = new ModelConverter<T>(_dataReader);
        var isOnlyField = reader.VisibleFieldCount == 1;

        if (converter.IsSystemType && !isOnlyField)
        {
            throw new InvalidCastException($"Не удалось преобразовать значения из запроса в тип модели {converter.ElementType}");
        }

        if (!reader.HasRows)
        {
            yield break;
        }

        while (reader.Read())
        {
            yield return converter.GetObject();
        }
    }

    public IEnumerator<T> GetEnumerator() => GetInternalEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerator();
}