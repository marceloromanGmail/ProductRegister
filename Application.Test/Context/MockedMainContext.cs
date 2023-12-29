using DataAccess.Main.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Test.Context
{
    public abstract class MockedMainContext
    {
        public MainContext Context { get; private set; }

        public MockedMainContext()
        {
            var options = new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            Context = new InMemoryDbContext(options);
        }
    }
}