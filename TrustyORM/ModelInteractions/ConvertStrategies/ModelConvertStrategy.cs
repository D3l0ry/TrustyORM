using System.Data.Common;
using TrustyORM.ModelInteractions.ConvertStrategies.ModelStrategies;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy<T> : ConvertStrategyContext<T>
{
    private static readonly bool _isModelOnlyToMany = typeof(T).IsModelRelationOnlyToMany();
    private readonly ModelConvertStrategyBase<T?>? _choicedConvertStrategy;

    public ModelConvertStrategy(DbDataReader dataReader) : base(dataReader)
    {
        if (_isModelOnlyToMany)
        {
            _choicedConvertStrategy = new ModelOnlyToManyConvertStrategy<T?>(dataReader);
        }
        else
        {
            _choicedConvertStrategy = new ModelOnlyToOnlyConvertStrategy<T?>(dataReader);
        }
    }

    public override T? GetObject() => _choicedConvertStrategy!.GetObject();

    public override IEnumerator<T?> GetEnumerator() => _choicedConvertStrategy!.GetEnumerator();
}