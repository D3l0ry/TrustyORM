using System.Data.Common;
using System.Runtime.CompilerServices;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions;

internal class ModelConverter<T>
{
    private readonly DbDataReader _dataReader;
    private readonly bool _isSystemType;
    private readonly IConvertStrategy<T> _strategy;

    internal ModelConverter(DbDataReader dataReader)
    {
        if (dataReader == null)
        {
            throw new ArgumentNullException(nameof(dataReader));
        }

        _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
        _isSystemType = typeof(T).IsSystemType();
        _strategy = ChoiceConvertStrategy<T>.GetStrategy(dataReader);
    }

    public Type ElementType => typeof(T);

    public bool IsSystemType => _isSystemType;

    /// <summary>
    /// Получение объекта из таблицы
    /// </summary>
    /// <param name="dataReader"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetObject() => _strategy.Convert();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetObjects()
    {
        if (!_dataReader.HasRows)
        {
            Enumerable.Empty<T>();
        }

        var list = new List<T>();

        while (_dataReader.Read())
        {
            var item = GetObject();

            list.Add(item);
        }

        return list;
    }
}