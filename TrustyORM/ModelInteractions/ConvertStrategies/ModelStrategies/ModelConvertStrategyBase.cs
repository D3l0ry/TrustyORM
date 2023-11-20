using System.Data.Common;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal abstract class ModelConvertStrategyBase<T> : ConvertStrategyContext<T>
{
    private readonly IEnumerable<DbColumn> _schema;
    private readonly ModelPropertyInformation[] _properties;
    private readonly ForeignTableConverter[] _foreignTableConverters;

    public ModelConvertStrategyBase(DbDataReader dataReader) : base(dataReader)
    {
        _schema = dataReader.GetColumnSchema();
        _properties = ElementType.GetModelPropertiesFromSchema(_schema).ToArray();
        _foreignTableConverters = ElementType.GetForeignTableConverters(_schema).ToArray();
    }

    protected IEnumerable<DbColumn> Schema => _schema;

    protected ModelPropertyInformation[] Properties => _properties;

    protected ForeignTableConverter[] ForeignTableConverters => _foreignTableConverters;
}