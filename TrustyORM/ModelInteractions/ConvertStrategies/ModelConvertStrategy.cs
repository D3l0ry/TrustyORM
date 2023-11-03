using System.Data.Common;
using System.Reflection;
using TrustyORM.Extensions;

namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal class ModelConvertStrategy<T> : ConvertStrategyContext<T>
{
    private readonly KeyValuePair<PropertyInfo, ColumnAttribute>[] _properties;
    private readonly KeyValuePair<PropertyInfo, ForeignTableAttribute>[] _foreignTableProperties;

    public ModelConvertStrategy(DbDataReader dataReader) : base(dataReader)
    {
        var type = typeof(T);
        _properties = type.GetModelProperties();
        _foreignTableProperties = type.GetForeignTableProperties();
    }

    public override IEnumerable<T> Convert()
    {
        using (Reader)
        {
            if (!Reader.HasRows)
            {
                yield break;
            }

            while (Reader.Read())
            {
                T? newObject = Activator.CreateInstance<T>();

                foreach (var currentProperty in _properties)
                {
                    currentProperty.SetDataReaderValue(newObject, Reader);
                }

                foreach (var currentForeignTable in _foreignTableProperties)
                {
                    var foreignTableConverter = new ForeignTableConverter(currentForeignTable, Reader);
                    var result = foreignTableConverter.GetObject();

                    currentForeignTable.Key.SetValue(newObject, result);
                }

                yield return newObject;
            }
        }
    }
}