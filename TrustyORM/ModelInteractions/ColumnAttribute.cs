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
    /// Наименование колонки в запросе или наименование таблицы при IsForeignTable = true
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Является ли свойство внешней таблицей
    /// </summary>
    public bool IsForeignTable { get; set; }
}