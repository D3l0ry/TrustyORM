using System.Data.Common;
using System.Reflection;

namespace TrustyORM.ModelInteractions;
internal class MapperPropertyInformation
{
    private readonly PropertyInfo _property;
    private readonly DbColumn _column;

    public MapperPropertyInformation(PropertyInfo property, DbColumn column)
    {
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(column);

        _property = property;
        _column = column;
    }

    public PropertyInfo Property => _property;

    public DbColumn Column => _column;
}