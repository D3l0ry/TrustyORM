namespace TrustyORM;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnAttribute : Attribute
{
    public ColumnAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;
    }

    /// <summary>
    /// Наименование колонки
    /// </summary>
    internal string? Name { get; set; }
}