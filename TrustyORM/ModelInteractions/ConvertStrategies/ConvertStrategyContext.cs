using System.Data.Common;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal abstract class ConvertStrategyContext<T>
{
    private readonly DbDataReader _dataReader;

    protected ConvertStrategyContext(DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(dataReader);

        _dataReader = dataReader;
    }

    public Type ElementType => typeof(T);

    public DbDataReader Reader => _dataReader;

    public abstract IEnumerable<T> Convert();
}