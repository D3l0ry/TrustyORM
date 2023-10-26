using System.Data.Common;

namespace TrustyORM.SqlExceptions;
public class DbConnectionException : DbException
{
    public DbConnectionException(string? message) : base(message) { }
}