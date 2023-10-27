using System.Data.Common;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class SystemTypeConvertStrategy : IConvertStrategy
{
    private readonly DbDataReader _dataReader;

    public SystemTypeConvertStrategy(DbDataReader dataReader)
    {
        _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
    }

    public object Convert() => _dataReader.GetValue(0);
}