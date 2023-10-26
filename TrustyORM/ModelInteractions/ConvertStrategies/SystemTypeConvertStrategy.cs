using System.Data.Common;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class SystemTypeConvertStrategy<T> : IConvertStrategy<T>
{
    private readonly DbDataReader _dataReader;

    public SystemTypeConvertStrategy(DbDataReader dataReader)
    {
        _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
    }

    public T Convert() => _dataReader.GetFieldValue<T>(0);
}