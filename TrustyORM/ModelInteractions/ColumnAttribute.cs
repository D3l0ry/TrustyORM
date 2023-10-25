namespace TrustyORM;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute : Attribute
{
    private readonly string _columnName;

    public ColumnAttribute(string columnName)
    {
        if (string.IsNullOrWhiteSpace(columnName))
        {
            throw new ArgumentNullException(nameof(columnName));
        }

        _columnName = columnName;
    }

    public string Name => _columnName;
}