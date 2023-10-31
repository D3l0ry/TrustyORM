using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;

namespace TrustyBencmarks;

public class BenchamarkMethods
{
    //[Benchmark]
    //public void BenchDapper1()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var user = Dapper.SqlMapper.Query<UserDapperModel, ProfileDapperModel, UserDapperModel>(connection, "SELECT TOP(1) UserTable.Id,UserTable.ProfileId,UserTable.Login,UserTable.Password,UserTable.Email,ProfileTable.Id, ProfileTable.FirstName, ProfileTable.MiddleName, ProfileTable.LastName FROM [User] AS UserTable JOIN [Profile] AS ProfileTable ON ProfileTable.Id=UserTable.ProfileId", (x, y) =>
    //    {
    //        x.Profile = y;
    //        return x;
    //    }, "Id");
    //}

    //[Benchmark]
    //public void BenchDapper1()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var user = Dapper.SqlMapper.Query<UserDapperModel>(connection, "SELECT TOP(1) * FROM [User]");
    //}

    //[Benchmark]
    //public void BenchTrustyORM1()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT TOP(1) * FROM [User]").ToArray();
    //}

    //[Benchmark]
    //public void BenchDapper10()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var user = Dapper.SqlMapper.Query<UserModel>(connection, "SELECT TOP(10) * FROM [User]");
    //}

    //[Benchmark]
    //public void BenchTrustyORM10()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT TOP(10) * FROM [User]").ToArray();
    //}

    //[Benchmark]
    //public void BenchDapper100()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var user = Dapper.SqlMapper.Query<UserModel>(connection, "SELECT TOP(100) * FROM [User]");
    //}

    //[Benchmark]
    //public void BenchTrustyORM100()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT TOP(100) * FROM [User]").ToArray();
    //}

    //[Benchmark]
    //public void BenchDapper50000()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var user = Dapper.SqlMapper.Query<UserModel>(connection, "SELECT TOP(5000) * FROM [User]");
    //}

    //[Benchmark]
    //public void BenchTrustyORM50000()
    //{
    //    using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //    connection.Open();

    //    var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT TOP(5000) * FROM [User]").ToArray();
    //}

    [Benchmark]
    public void BenchDapperSelect1()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;");
        connection.Open();

        var user = Dapper.SqlMapper.Query<UserModel>(connection, "SELECT * FROM [User]", buffered: false).Where(x => x.Id == 999999).ToArray();
        ;
    }

    [Benchmark]
    public void BenchTrustyORMSelect1()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;");
        connection.Open();

        var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT * FROM [User]").Where(x => x.Id == 999999).ToArray();
    }
}