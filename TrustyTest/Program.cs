using Microsoft.Data.SqlClient;
using TrustyORM;

namespace TrustyTest;

internal class Program
{
    private static void Main(string[] args)
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Connection Timeout=600;Command Timeout=600;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;");
        //SqlCommand command = connection.CreateCommand();

        //for (int i = 10001; i < 1000000; i++)
        //{
        //    command.CommandText = $"INSERT [Profile] VALUES('First{i}', 'Middle{i}', 'Last{i}', 'photo{i}')";
        //    command.ExecuteNonQuery();
        //}

        //for (int i = 10001; i < 1000000; i++)
        //{
        //    command.CommandText = $"INSERT [User] VALUES({i}, 'User{i}', 'User{i}@mail.ru', 'userpassword{i}')";
        //    command.ExecuteNonQuery();
        //}
        //var users0 = Dapper.SqlMapper.Query<User>(connection, "SELECT * FROM [User]", buffered: false);
        //var users2 = connection.Query<User>("SELECT * FROM [User]").Where(x => x.Id == 999002).ToArray();
        //var test = connection.Query<User>("SELECT * FROM [User]");

        var users = connection.Query<User>("SELECT * FROM [User] LEFT JOIN [Profile] ON [Profile].Id=[User].ProfileId")
            .Where(x => x.Id == 999999);
        foreach (var currentUser in users)
        {
            Console.WriteLine($"Номер:{currentUser.Id}; Login:{currentUser.Login};FirstName:{currentUser.Profile.FirstName}");
        }

        //foreach (var currentUser in test)
        //{
        //    Console.WriteLine($"Номер:{currentUser.Id}; Email:{currentUser.Email}; Login:{currentUser.Login}; Password:{currentUser.Password}");
        //}
        //Console.WriteLine("--------------------------------------");
        //var users1 = connection.Query<User>("SELECT * FROM [User]").Skip(15).Take(10);
        //foreach (var currentUser in users1)
        //{
        //    Console.WriteLine($"Номер:{currentUser.Id}; Email:{currentUser.Email}; Login:{currentUser.Login}; Password:{currentUser.Password}");
        //}
    }

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
        public Profile Profile { get; set; }
    }

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