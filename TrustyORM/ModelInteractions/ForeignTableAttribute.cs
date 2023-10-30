namespace TrustyORM;
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignTableAttribute : Attribute
{
    private readonly string _tableName;

    public ForeignTableAttribute(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentNullException(nameof(tableName));
        }

        _tableName = tableName;
    }

    public string TableName => _tableName;
}