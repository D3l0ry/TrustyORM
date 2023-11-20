using TrustyORM;

namespace TrustyTest;

internal partial class Program
{
    private class Profile
    {
        [Column()]
        public int Id { get; set; }

        [Column()]
        public string FirstName { get; set; }

        [Column()]
        public string MiddleName { get; set; }

        [Column()]
        public string LastName { get; set; }
    }
}