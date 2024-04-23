using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.ServiceTests;

public class InMemoryDb
{
    protected AppDbContext DbContext { get; private set; }
    
    protected InMemoryDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            
            .Options;
        DbContext = new AppDbContext(options);
    }
}