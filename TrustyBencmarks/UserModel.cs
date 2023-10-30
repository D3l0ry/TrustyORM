using TrustyORM;

namespace TrustyBencmarks;

public class UserModel
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("Login")]
    public string Login { get; set; }

    [Column("ProfileId")]
    public int ProfileId { get; set; }

    [Column("Password")]
    public string Password { get; set; }


    [Column("Email")]
    public string Email { get; set; }

    [ForeignTable("Profile")]
    public ProfileModel Profile { get; set; }
}

public class ProfileModel
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

public class UserDapperModel
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public ProfileDapperModel Profile { get; set; }
}

public class ProfileDapperModel
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }
}
