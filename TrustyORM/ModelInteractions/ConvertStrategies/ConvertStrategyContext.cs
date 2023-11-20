using System.Collections;
using System.Data.Common;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal abstract class ConvertStrategyContext<T> : IEnumerable<T?>
{
    private readonly DbDataReader _dataReader;

    protected ConvertStrategyContext(DbDataReader dataReader)
    {
        ArgumentNullException.ThrowIfNull(dataReader);

        _dataReader = dataReader;
    }

    public Type ElementType => typeof(T);

    public virtual DbDataReader Reader => _dataReader;

    public abstract T? GetObject();

    public virtual IEnumerator<T?> GetEnumerator()
    {
        using (Reader)
        {
            if (!Reader.HasRows)
            {
                yield break;
            }

            while (Reader.Read())
            {
                yield return GetObject();
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();
}