namespace TrustyORM.Extensions;
internal static class TypeExtensions
{
    public static bool IsSystemType(this Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

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