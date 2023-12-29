using DataAccess.Main.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Application.Test.Context
{
    public class InMemoryDbContext : MainContext, IDisposable
    {
        private readonly SqliteConnection _connection;

        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {
            SQLitePCL.Batteries.Init();
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
        }

        private static DbContextOptions<InMemoryDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseSqlite(CreateInMemoryDatabase())
                .Options;
        }

        private static SqliteConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        public override void Dispose()
        {
            base.Dispose();
            _connection.Close();
        }
    }
}