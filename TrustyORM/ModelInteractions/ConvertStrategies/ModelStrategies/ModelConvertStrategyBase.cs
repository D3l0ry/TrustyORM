using System.Data.Common;
using System.Reflection;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal abstract class ModelConvertStrategyBase<T> : ConvertStrategyContext<T>
{
    private readonly IEnumerable<DbColumn> _schema;
    private readonly ModelPropertyInformation[] _properties;
    private readonly KeyValuePair<PropertyInfo, ForeignTableAttribute>[] _foreignTableProperties;

    public ModelConvertStrategyBase(DbDataReader dataReader) : base(dataReader)
    {
        var type = typeof(T);

        _schema = dataReader.GetColumnSchema();
        _properties = type.GetModelPropertiesFromSchema(_schema).ToArray();
        _foreignTableProperties = type.GetForeignTableProperties();
    }

    protected IEnumerable<DbColumn> Schema => _schema;

    protected ModelPropertyInformation[] Properties => _properties;

    protected KeyValuePair<PropertyInfo, ForeignTableAttribute>[] ForeignTableProperties => _foreignTableProperties;
}