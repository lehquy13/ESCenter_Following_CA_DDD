﻿using ESCenter.Domain.Aggregates.Courses;
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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ESCenter.Persistence.EntityFrameworkCore;

public class CustomUserStore : UserStore<EsIdentityUser>  // important to use custom Identity user object here!  
{
    public CustomUserStore(AppDbContext context)
        : base(context)
    {
        AutoSaveChanges = false;
    }
}

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<EsIdentityUser, EsIdentityRole, string>(options)
{
    public DbSet<Subject> Subjects { get; init; } = null!;
    public DbSet<Course> Courses { get; init; } = null!;
    public DbSet<ChangeVerificationRequest> ChangeVerificationRequests { get; init; } = null!;
    public DbSet<ChangeVerificationRequestDetail> ChangeVerificationRequestDetails { get; init; } = null!;
    public DbSet<CourseRequest> CourseRequests { get; init; } = null!;
    public DbSet<Customer> Customers { get; init; } = null!;
    public DbSet<Tutor> Tutors { get; init; } = null!;
    public DbSet<Verification> Verifications { get; init; } = null!;
    public DbSet<TutorMajor> TutorMajors { get; init; } = null!;
    public DbSet<TutorRequest> TutorRequests { get; init; } = null!;
    public DbSet<Subscriber> Subscribers { get; init; } = null!;
    public DbSet<Notification> Notifications { get; init; } = null!;
    public DbSet<Discovery> Discoveries { get; init; } = null!;
    public DbSet<DiscoveryUser> DiscoveryUsers { get; init; } = null!;
    public DbSet<DiscoverySubject> DiscoverySubjects { get; init; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

//using to support adding migration
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            //"Server=(localdb)\\MSSQLLocalDB; Database=EduSmart_5; Trusted_Connection=True;MultipleActiveResultSets=true"
            // "Server=(localdb)\\MSSQLLocalDB; Database=EduSmart_4; Trusted_Connection=True;MultipleActiveResultSets=true"
             "Server=abcdavid-knguyen.ddns.net,30019;Database=es_mssql2;TrustServerCertificate=True;User Id=sa;Password=LHQuy12@306lkjh?;MultipleActiveResultSets=true"
            // "Server=abcdavid-knguyen.ddns.net,30019;Database=es_mssql1;TrustServerCertificate=True;User Id=sa;Password=LHQuy12@306lkjh?;MultipleActiveResultSets=true"
            // "Server=(localdb)\\MSSQLLocalDB; Database=EduSmart_3; Trusted_Connection=True;MultipleActiveResultSets=true"
            // "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=EduSmart;Trusted_Connection=True;TrustServerCertificate=True"
            //"workstation id=es_mssql.mssql.somee.com;packet size=4096;user id=lehquy13_SQLLogin_1;pwd=5kf5v3wgao;data source=es_mssql.mssql.somee.com;persist security info=False;initial catalog=es_mssql;TrustServerCertificate=True"
            // "workstation id=edusmart.mssql.somee.com;packet size=4096;user id=EduSmart_SQLLogin_3;pwd=84cjobbnby;data source=edusmart.mssql.somee.com;persist security info=False;initial catalog=edusmart;TrustServerCertificate=True"
            // "workstation id=CED_Database.mssql.somee.com;packet size=4096;user id=Matty_SQLLogin_1;pwd=rg12f5urma;data source=CED_Database.mssql.somee.com;persist security info=False;initial catalog=CED_Database; TrustServerCertificate=True;"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}