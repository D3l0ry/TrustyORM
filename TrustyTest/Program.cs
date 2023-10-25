using Microsoft.Data.SqlClient;
using TrustyORM;

namespace TrustyTest;

internal class Program
{
    private static void Main(string[] args)
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        connection.Open();

        try
        {
            //SqlCommand command = connection.CreateCommand();

            //for (int i = 1; i < 10000; i++)
            //{
            //    command.CommandText = $"INSERT [Profile] VALUES('First{i}', 'Middle{i}', 'Last{i}', 'photo{i}')";
            //    command.ExecuteNonQuery();
            //}

            //for (int i = 1; i < 10000; i++)
            //{
            //    command.CommandText = $"INSERT [User] VALUES({i}, 'User{i}', 'User{i}@mail.ru', 'userpassword{i}')";
            //    command.ExecuteNonQuery();
            //}

            //var users = connection.Query<string>("SELECT Email FROM [User]");

            //foreach (var currentUser in users)
            //{
            //    Console.WriteLine($"Номер:{currentUser};");
            //}

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM [User] JOIN Profile pr ON pr.Id = [User].ProfileId";

            SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.KeyInfo);
            var schema = reader.GetColumnSchema();
            //while (reader.Read())
            //{
            //    //var schema = reader.GetColumnSchema()[0];
            //}
        }
        finally
        {
            connection.Close();
        }
    }

    private class User
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

    private class TestUser
    {
        [Column("Id")]
        public int Id { get; set; }
    }
}