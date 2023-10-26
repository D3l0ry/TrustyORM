namespace TrustyORM.ModelInteractions.ConvertStrategies;
internal interface IConvertStrategy<T>
{
    T Convert();
}