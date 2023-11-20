using TrustyORM;

namespace TrustyTest;

internal partial class Program
{
    private class User
    {
        [Column]
        public int Id { get; set; }

        [Column]
        public string Login { get; set; }

        [Column]
        public int ProfileId { get; set; }

        [Column]
        public string Email { get; set; }

        [Column]
        public string Password { get; set; }

        [ForeignTable("Profile")]
        public Profile[] Profiles { get; set; }
    }
}