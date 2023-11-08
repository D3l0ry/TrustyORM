using TrustyORM;

namespace TrustyTest;

internal partial class Program
{
    private class Profile
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("MiddleName")]
        public string MiddleName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }
    }
}