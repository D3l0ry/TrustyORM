using System.Data.Common;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions;
internal static class ChoiceConvertStrategy
{
    public static ConvertStrategyContext<T> GetStrategy<T>(DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(dataReader);

        Type type = typeof(T);

        if (type.IsSystemType())
        {
            if (dataReader.VisibleFieldCount != 1)
            {
                throw new InvalidCastException($"Не удалось преобразовать значения из запроса в тип модели {type}");
            }

            return new SystemTypeConvertStrategy<T>(dataReader);
        }

        return new ModelConvertStrategy<T>(dataReader);
    }
}