using Microsoft.Data.SqlClient;
using TrustyORM;

namespace TrustyTest;

internal partial class Program
{
    private static void Main(string[] args)
    {
        Console.ReadLine();
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Connection Timeout=600;Command Timeout=600;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;");

        //for (int i = 10001; i < 1000000; i++)
        //{
        //    connection.Execute($"INSERT [Profile] VALUES('First{i}', 'Middle{i}', 'Last{i}', 'photo{i}')");
        //    connection.Execute($"INSERT [User] VALUES({i}, 'User{i}', 'User{i}@mail.ru', 'userpassword{i}')");
        //}

        var users = connection
            .Query<User>("SELECT * FROM [User] JOIN Profile ON Profile.Id=[User].ProfileId WHERE [User].Id > @UserId", new { UserId = 5 })
            .ToArray();

        foreach (var currentUser in users)
        {
            Console.WriteLine($"Номер:{currentUser.Id}; Login:{currentUser.Login};");
        }
    }
}