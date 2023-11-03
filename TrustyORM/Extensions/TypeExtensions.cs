namespace TrustyORM.Extensions;
internal static class TypeExtensions
{
    public static bool IsSystemType(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type == typeof(bool)
            || type == typeof(byte)
            || type == typeof(sbyte)
            || type == typeof(short)
            || type == typeof(ushort)
            || type == typeof(int)
            || type == typeof(uint)
            || type == typeof(long)
            || type == typeof(ulong)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(decimal)
            || type == typeof(string)
            || type == typeof(DateTime)
            || type == typeof(Guid);
    }
}