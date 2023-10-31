namespace TrustyORM;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute : Attribute
{
    private readonly string _name;

    public ColumnAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        _name = name;
    }

    /// <summary>
    /// Наименование колонки
    /// </summary>
    public string Name => _name;
}