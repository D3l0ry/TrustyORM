namespace TrustyORM.Extensions;
internal static class TypeExtensions
{
    public static bool IsSystemType(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.IsPrimitive)
        {
            return true;
        }

        var nullableType = Nullable.GetUnderlyingType(type);

        if (nullableType != null && nullableType.IsPrimitive)
        {
            return true;
        }

        return type == typeof(string)
            || type == typeof(decimal) || type == typeof(decimal?)
            || type == typeof(DateTime) || type == typeof(DateTime?)
            || type == typeof(Guid) || type == typeof(Guid?);
    }
}