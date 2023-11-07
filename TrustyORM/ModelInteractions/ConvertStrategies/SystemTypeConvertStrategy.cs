using System.Data.Common;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class SystemTypeConvertStrategy<T> : ConvertStrategyContext<T>
{
    public SystemTypeConvertStrategy(DbDataReader dataReader) : base(dataReader) { }

    protected override T? GetObject() => Reader.GetFieldValue<T>(0);
}