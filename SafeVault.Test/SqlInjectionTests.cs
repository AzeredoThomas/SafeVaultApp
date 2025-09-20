using Microsoft.EntityFrameworkCore;
using SafeVault.Data;
using SafeVault.Models;
using Xunit;

public class SqlInjectionTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SqlInjectionTestDb")
            .Options;

        var context = new ApplicationDbContext(options);

        // Seed med en legitim användare
        context.Users.Add(new User { Username = "admin", Password = "secure123" });
        context.SaveChanges();

        return context;
    }

    [Theory]
    [InlineData("admin' OR 1=1 --")]
    [InlineData("' OR '1'='1")]
    [InlineData("'; DROP TABLE Users; --")]
    public void SqlInjection_ShouldNotReturnUser(string maliciousInput)
    {
        // Arrange
        var context = GetDbContext();

        // Act – EF Core använder parameteriserad query
        var result = context.Users
            .FirstOrDefault(u => u.Username == maliciousInput);

        // Assert – ska inte hitta någon användare
        Assert.Null(result);
    }
}
