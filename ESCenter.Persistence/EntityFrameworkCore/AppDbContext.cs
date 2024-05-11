using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.Notifications;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subscribers;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Persistence.Middleware;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ESCenter.Persistence.EntityFrameworkCore;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IHttpContextAccessor? httpContextAccessor = null
)
    : IdentityDbContext<IdentityUser, IdentityRole, string>(options)
{
    public DbSet<Subject> Subjects { get; init; } = null!;
    public DbSet<Course> Courses { get; init; } = null!;
    public DbSet<ChangeVerificationRequest> ChangeVerificationRequests { get; init; } = null!;
    public DbSet<CourseRequest> CourseRequests { get; init; } = null!;
    public DbSet<Customer> Customers { get; init; } = null!;
    public DbSet<Tutor> Tutors { get; init; } = null!;
    public DbSet<TutorRequest> TutorRequests { get; init; } = null!;
    public DbSet<Subscriber> Subscribers { get; init; } = null!;
    public DbSet<Discovery> Discoveries { get; init; } = null!;
    public DbSet<DiscoveryUser> DiscoveryUsers { get; init; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (httpContextAccessor?.HttpContext is null) return result;

        Queue<IDomainEvent> domainEventsQueue =
            httpContextAccessor.HttpContext.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey,
                out var value) &&
            value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;

        return result;
    }
}

//using to support adding migration
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB; Database=EduSmart_6; Trusted_Connection=True;MultipleActiveResultSets=true"
            // "Server=homelab-quy.duckdns.org,1433;Database=es_mssql;TrustServerCertificate=True;User Id=sa;Password=1q2w3E**;MultipleActiveResultSets=true"
            // "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=EduSmart;Trusted_Connection=True;TrustServerCertificate=True"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}