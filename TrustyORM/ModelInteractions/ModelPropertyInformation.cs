using System.Data.Common;
using System.Reflection;

namespace TrustyORM.ModelInteractions;
internal class ModelPropertyInformation
{
    private readonly PropertyInfo _property;
    private readonly DbColumn _column;

    public ModelPropertyInformation(PropertyInfo property, DbColumn column)
    {
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(column);

        _property = property;
        _column = column;
    }

    public PropertyInfo Property => _property;

    public DbColumn Column => _column;
}