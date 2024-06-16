using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ESCenter.Persistence.EntityFrameworkCore;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
             "Server=(localdb)\\MSSQLLocalDB; Database=esmssql; Trusted_Connection=True;MultipleActiveResultSets=true"
            //"Server=matthomelab.dns.army,1433;Database=esmssql;TrustServerCertificate=True;User Id=sa;Password=1q2w3E**;MultipleActiveResultSets=true"
            // "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=EduSmart;Trusted_Connection=True;TrustServerCertificate=True"
        );

        return new AppDbContext(optionsBuilder.Options);
    }

    public AppDbContext CreateDbContext(string dbName, bool isLocal = false)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            isLocal
                ? $"Server=(localdb)\\MSSQLLocalDB; Database={dbName}; Trusted_Connection=True;MultipleActiveResultSets=true"
                : $"Server=matthomelab.dns.army,1433;Database={dbName};TrustServerCertificate=True;User Id=sa;Password=1q2w3E**;MultipleActiveResultSets=true"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}