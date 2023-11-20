using System.Data;
using System.Reflection;
using TrustyORM.ModelInteractions;

namespace TrustyORM.Extensions;
internal static class MapperPropertyExtensions
{
    /// <summary>
    /// Выдает исключение, если приссваимое значение равно Null, но свойство не принимает значения типа Null
    /// </summary>
    /// <param name="property"></param>
    /// <param name="readerValue"></param>
    /// <exception cref="InvalidCastException"></exception>
    private static object? GetValueOrThrowExceptionIfPropertyIsNotNullableType(this PropertyInfo property, object readerValue)
    {
        Type propertyType = property.PropertyType;

        if (readerValue is not DBNull)
        {
            return readerValue;
        }

        if (!propertyType.IsValueType)
        {
            return null;
        }

        if (Nullable.GetUnderlyingType(propertyType) != null)
        {
            return null;
        }

        throw new InvalidCastException($"Поле {property.Name} вернуло NULL, тогда как тип не принимает такие значения");
    }

    private static void SetValue(this PropertyInfo property, object obj, int columnOrdinal, IDataRecord dataReader)
    {
        object readerValue = dataReader[columnOrdinal];
        object? value = property.GetValueOrThrowExceptionIfPropertyIsNotNullableType(readerValue);

        property.SetValue(obj, value);
    }

    private static void SetValue(this PropertyInfo property, object obj, string columnName, IDataRecord dataReader)
    {
        object readerValue = dataReader[columnName];
        object? value = property.GetValueOrThrowExceptionIfPropertyIsNotNullableType(readerValue);

        property.SetValue(obj, value);
    }

    public static void SetDataReaderValue(this PropertyInfo property, object obj, IDataRecord dataReader) =>
        property.SetValue(obj, property.Name, dataReader);

    public static void SetDataReaderValue(this ModelPropertyInformation mapperProperty, object obj, IDataRecord dataReader) =>
        mapperProperty.Property.SetValue(obj, mapperProperty.Column.ColumnOrdinal!.Value, dataReader);

    public static bool IsCollection(this KeyValuePair<PropertyInfo, ForeignTableAttribute> property)
    {
        ArgumentNullException.ThrowIfNull(property.Key);

        var propertyType = property.Key.PropertyType;

        return propertyType.IsArray || propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }
}