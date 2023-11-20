using Microsoft.Data.SqlClient;
using TrustyORM;

namespace TrustyTest;

internal partial class Program
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

        var users = connection.Query<User>("SELECT TOP(1) * FROM [User] JOIN Profile ON Profile.Id=[User].ProfileId").ToArray();
        foreach (var currentUser in users)
        {
            Console.WriteLine($"Номер:{currentUser.Id}; Login:{currentUser.Login};");
        }
    }
}