using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Main.Context
{
    public class MainDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainContext>
    {
        public MainContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebApi"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("MainConnection"));

            return new MainContext(optionsBuilder.Options);
        }
    }
}
