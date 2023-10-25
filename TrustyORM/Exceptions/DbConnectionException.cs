using System.Data.Common;

namespace TrustyORM.Exceptions;
public class DbConnectionException : DbException
{
    public DbConnectionException(string? message) : base(message) { }
}