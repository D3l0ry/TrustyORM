using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;

namespace TrustyBencmarks;
public class BenchmarkTrustyMethods
{
    private static readonly SqlConnection _connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    [Benchmark]
    public void BenchTrustyORM1()
    {
        _connection.Open();
        var users = TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(1) * FROM [User]").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyForeignTableORM1()
    {
        _connection.Open();
        var users = TrustyORM.SqlMapper.Query<UserModelForeignTableCollection>(_connection, "SELECT TOP(1) * FROM [User] JOIN Profile ON Profile.Id=[User].Id").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyORM10()
    {
        _connection.Open();
        var users = TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(10) * FROM [User]").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyForeignTableORM10()
    {
        _connection.Open();
        var users = TrustyORM.SqlMapper.Query<UserModelForeignTableCollection>(_connection, "SELECT TOP(10) * FROM [User] JOIN Profile ON Profile.Id=[User].Id").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyORM100()
    {
        _connection.Open();
        var users = TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(100) * FROM [User]").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyForeignTableORM100()
    {
        _connection.Open();
        var users = TrustyORM.SqlMapper.Query<UserModelForeignTableCollection>(_connection, "SELECT TOP(100) * FROM [User] JOIN Profile ON Profile.Id=[User].Id").ToArray();
        _connection.Close();
    }

    //[Benchmark]
    //public void BenchTrustyORM1000()
    //{
    //    _connection.Open();
    //    var users = TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(1000) * FROM [User]").ToArray();
    //    _connection.Close();
    //}

    //[Benchmark]
    //public void BenchTrustyForeignTableORM1000()
    //{
    //    _connection.Open();
    //    var users = TrustyORM.SqlMapper.Query<UserModelForeignTableCollection>(_connection, "SELECT TOP(1000) * FROM [User] JOIN Profile ON Profile.Id=[User].Id").ToArray();
    //    _connection.Close();
    //}
}