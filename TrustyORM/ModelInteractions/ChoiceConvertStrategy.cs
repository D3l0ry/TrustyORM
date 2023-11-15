using System.Data.Common;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;
using TrustyORM.ModelInteractions.ConvertStrategies.ModelStrategies;

namespace TrustyORM.ModelInteractions;
internal static class ChoiceConvertStrategy
{
    public static ConvertStrategyContext<T?> GetStrategy<T>(DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(dataReader);

        var type = typeof(T);
        var selectedStrategy = default(ConvertStrategyContext<T?>);

        if (type.IsSystemType())
        {
            if (dataReader.VisibleFieldCount != 1)
            {
                throw new InvalidCastException($"Не удалось преобразовать значения из запроса в тип модели {type}");
            }

            selectedStrategy = new SystemTypeConvertStrategy<T?>(dataReader);
        }
        if (type.IsModelRelationOnlyToMany())
        {
            selectedStrategy = new ModelOnlyToManyConvertStrategy<T?>(dataReader);
        }
        else
        {
            selectedStrategy = new ModelOnlyToOnlyConvertStrategy<T?>(dataReader);
        }

        return selectedStrategy;
    }
}