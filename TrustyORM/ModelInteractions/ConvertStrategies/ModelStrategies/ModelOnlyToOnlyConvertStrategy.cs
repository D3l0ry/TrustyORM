using System.Data.Common;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies.ModelStrategies;
internal class ModelOnlyToOnlyConvertStrategy<T> : ModelConvertStrategyBase<T>
{
    public ModelOnlyToOnlyConvertStrategy(DbDataReader dataReader) : base(dataReader) { }

    public override T? GetObject()
    {
        T newObject = Activator.CreateInstance<T>();

        foreach (var currentProperty in Properties)
        {
            currentProperty.SetDataReaderValue(newObject!, Reader);
        }

        foreach (var currentConverter in ForeignTableConverters)
        {
            var result = currentConverter.GetObject(Reader);

            currentConverter.Property.SetValue(newObject, result);
        }

        return newObject;
    }
}
