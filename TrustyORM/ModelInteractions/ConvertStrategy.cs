using System.Data.Common;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions;
internal static class ConvertStrategy<T>
{
    private static readonly bool _isSystemType;

    static ConvertStrategy() => _isSystemType = typeof(T).IsSystemType();

    public static ConvertStrategyContext<T?> GetStrategy(DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(dataReader);

        if (_isSystemType)
        {
            if (dataReader.VisibleFieldCount != 1)
            {
                throw new InvalidCastException($"Не удалось преобразовать значения из запроса в тип модели {typeof(T)}");
            }

            return new SystemTypeConvertStrategy<T?>(dataReader);
        }

        return new ModelConvertStrategy<T?>(dataReader);
    }
}