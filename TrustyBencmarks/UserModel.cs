using System.Data.Common;
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

    //[ForeignTable("Profile")]
    //public ProfileModel Profile { get; set; }

    public static IEnumerable<UserModel> GetUsers(DbConnection connection, int count)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT TOP({count}) * FROM [User]";

        using var dataReader = command.ExecuteReader();

        List<UserModel> list = new();

        if (!dataReader.HasRows)
        {
            return list;
        }

        while (dataReader.Read())
        {
            var item = new UserModel();

            item.Id = dataReader.GetInt32(0);
            item.ProfileId = dataReader.GetInt32(1);
            item.Login = dataReader.GetString(2);
            item.Email = dataReader.GetString(3);
            item.Password = dataReader.GetString(4);

            list.Add(item);
        }

        return list;
    }
}

public class UserModelForeignTableCollection
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

    [ForeignTable("Profile")]
    public ProfileModel Profiles { get; set; }
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
