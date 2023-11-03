using System.Data.Common;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class SystemTypeConvertStrategy<T> : ConvertStrategyContext<T>
{
    public SystemTypeConvertStrategy(DbDataReader dataReader) : base(dataReader) { }

    public override IEnumerable<T> Convert()
    {
        using (Reader)
        {
            if (!Reader.HasRows)
            {
                yield break;
            }

            while (Reader.Read())
            {
                yield return Reader.GetFieldValue<T>(0);
            }
        }
    }
}