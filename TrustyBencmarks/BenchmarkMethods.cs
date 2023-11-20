using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;

namespace TrustyBencmarks;

public class BenchmarkMethods
{
    internal static readonly SqlConnection _connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    [Benchmark]
    public void BenchTrustyORM1()
    {
        _connection.Open();
        var users = TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(1) * FROM [User]").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyORM10()
    {
        _connection.Open();
        TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(10) * FROM [User]").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyORM100()
    {
        TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(100) * FROM [User]").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchTrustyORM1000()
    {
        TrustyORM.SqlMapper.Query<UserModel>(_connection, "SELECT TOP(1000) * FROM [User]").ToArray();
        _connection.Close();
    }

    [Benchmark]
    public void BenchSQL1()
    {
        _connection.Open();
        var users = UserModel.GetUsers(_connection, 1);
        _connection.Close();
    }

    [Benchmark]
    public void BenchSQL10()
    {
        _connection.Open();
        UserModel.GetUsers(_connection, 10);
        _connection.Close();
    }

    [Benchmark]
    public void BenchSQL100()
    {
        _connection.Open();
        UserModel.GetUsers(_connection, 100);
        _connection.Close();
    }

    [Benchmark]
    public void BenchSQL1000()
    {
        _connection.Open();
        UserModel.GetUsers(_connection, 1000);
        _connection.Close();
    }
}