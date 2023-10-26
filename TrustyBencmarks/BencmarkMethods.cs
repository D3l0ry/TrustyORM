﻿using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;

namespace TrustyBencmarks;

public class BenchamarkMethods
{
    [Benchmark]
    public void BenchDapper1()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        connection.Open();

        var user = Dapper.SqlMapper.Query<UserModel>(connection, "SELECT TOP(1) * FROM [User]").ToArray();
    }

    [Benchmark]
    public void BenchDapper10()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        connection.Open();

        var user = Dapper.SqlMapper.Query<UserModel>(connection, "SELECT TOP(10) * FROM [User]").ToArray();
    }

    [Benchmark]
    public void BenchDapper100()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        connection.Open();

        var user = Dapper.SqlMapper.Query<UserModel>(connection, "SELECT TOP(100) * FROM [User]").ToArray();
    }

    [Benchmark]
    public void BenchTrustyORM1()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        connection.Open();

        var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT TOP(1) * FROM [User]").ToArray();
    }

    [Benchmark]
    public void BenchTrustyORM10()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        connection.Open();

        var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT TOP(10) * FROM [User]").ToArray();
    }

    [Benchmark]
    public void BenchTrustyORM100()
    {
        using SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrustyORM;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        connection.Open();

        var users = TrustyORM.SqlMapper.Query<UserModel>(connection, "SELECT TOP(100) * FROM [User]").ToArray();
    }
}