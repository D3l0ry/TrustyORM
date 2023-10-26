using TrustyORM;

namespace TrustyBencmarks;

public class UserModel
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("ProfileId")]
    public int ProfileId { get; set; }

    [Column("Login")]
    public string Login { get; set; }

    [Column("Email")]
    public string Email { get; set; }

    [Column("Password")]
    public string Password { get; set; }
}
