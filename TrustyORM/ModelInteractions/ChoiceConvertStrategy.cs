using System.Data.Common;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions;
internal static class ChoiceConvertStrategy
{
    public static IConvertStrategy<T> GetStrategy<T>(DbDataReader dataReader)
    {
        Type type = typeof(T);

        if (type.IsSystemType())
        {
            return new SystemTypeConvertStrategy<T>(dataReader);
        }

        return new ModelConvertStrategy<T>(dataReader);
    }
}