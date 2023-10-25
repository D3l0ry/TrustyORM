namespace TrustyORM.Extensions;
internal static class TypeExtensions
{
    public static bool IsSystemType(this Type type)
    {
        Type[] systemTypes = new[]
        {
            typeof(bool),
            typeof(byte) ,
            typeof(sbyte) ,
            typeof(short),
            typeof(ushort),
            typeof(int) ,
            typeof(uint),
            typeof(long) ,
            typeof(ulong) ,
            typeof(float) ,
            typeof(double) ,
            typeof(decimal) ,
            typeof(string) ,
            typeof(DateTime),
            typeof(Guid)
        };

        return systemTypes.Any(currentType => currentType == type);
    }
}