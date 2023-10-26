using System.Data.Common;
using TrustyORM.Extensions;
using TrustyORM.ModelInteractions.ConvertStrategies;

namespace TrustyORM.ModelInteractions;
internal static class ChoiceConvertStrategy<T>
{
    public static IConvertStrategy<T> GetStrategy(DbDataReader dataReader)
    {
        if (typeof(T).IsSystemType())
        {
            return new SystemTypeConvertStrategy<T>(dataReader);
        }

        return new ModelConvertStrategy<T>(dataReader);
    }
}