using System.Reflection;

namespace TrustyORM.TypeInteractions;
internal class MapperTypeInformation
{
    private readonly PropertyInfo _property;
    private readonly int _columnIndex;

    public MapperTypeInformation(PropertyInfo property, int columnIndex)
    {
        if (property == null)
        {
            throw new ArgumentNullException(nameof(property));
        }

        _property = property;
        _columnIndex = columnIndex;
    }

    public PropertyInfo Property => _property;

    public int ColumnIndex => _columnIndex;
}