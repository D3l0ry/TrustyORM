using System.Data.Common;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy<T> : ModelConvertStrategyBase<T>
{
    public ModelConvertStrategy(DbDataReader dataReader) : base(dataReader) { }

    protected override T? GetObject()
    {
        T newObject = Activator.CreateInstance<T>();

        foreach (var currentProperty in Properties)
        {
            currentProperty.SetDataReaderValue(newObject!, Reader);
        }

        foreach (var currentForeignTable in ForeignTableProperties)
        {
            var foreignTableConverter = new ForeignTableConverter(currentForeignTable, Schema);
            var result = foreignTableConverter.GetObject(Reader);

            currentForeignTable.Key.SetValue(newObject, result);
        }

        return newObject;
    }
}