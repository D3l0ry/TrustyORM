using System.Data.Common;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions;
internal static class ChoiceConvertStrategy<T>
{
    public static IConvertStrategy GetStrategy(DbDataReader dataReader)
    {
        Type type = typeof(T);

        if (type.IsSystemType())
        {
            return new SystemTypeConvertStrategy(dataReader);
        }

        return new ModelConvertStrategy(type, dataReader);
    }
}