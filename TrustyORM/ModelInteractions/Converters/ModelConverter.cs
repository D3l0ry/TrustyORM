using System.Data.Common;
using System.Runtime.CompilerServices;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions.Converters;

internal class ModelConverter<T>
{
    private readonly bool _isSystemType;
    private readonly IConvertStrategy<T> _strategy;

    internal ModelConverter(DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(dataReader);

        _isSystemType = typeof(T).IsSystemType();
        _strategy = ChoiceConvertStrategy.GetStrategy<T>(dataReader);
    }

    public Type ElementType => typeof(T);

    public bool IsSystemType => _isSystemType;

    /// <summary>
    /// Получение объекта из таблицы
    /// </summary>
    /// <param name="dataReader"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetObject() => _strategy.Convert();
}