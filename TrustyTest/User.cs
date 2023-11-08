using TrustyORM;

namespace TrustyTest;

internal partial class Program
{
    private class User
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Login")]
        public string Login { get; set; }

        [Column("ProfileId")]
        public int ProfileId { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [ForeignTable("Profile")]
        public Profile[] Profiles { get; set; }
    }
}