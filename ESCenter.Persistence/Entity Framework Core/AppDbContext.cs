using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.CourseRequests;
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
using ESCenter.Domain.Aggregates.Users.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ESCenter.Persistence.Entity_Framework_Core;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<IdentityUser> IdentityUsers { get; set; } = null!;
    public DbSet<IdentityRole> IdentityRoles { get; set; } = null!;
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<ChangeVerificationRequest> ChangeVerificationRequests { get; set; } = null!;
    public DbSet<ChangeVerificationRequestDetail> ChangeVerificationRequestDetails { get; set; } = null!;
    public DbSet<CourseRequest> CourseRequests { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tutor> Tutors { get; set; } = null!;
    public DbSet<Verification> Verifications { get; set; } = null!;
    public DbSet<TutorMajor> TutorMajors { get; set; } = null!;
    public DbSet<TutorRequest> TutorRequests { get; set; } = null!;
    public DbSet<Subscriber> Subscribers { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Discovery> Discoveries { get; set; } = null!;
    public DbSet<DiscoveryUser> DiscoveryUsers { get; set; } = null!;
    public DbSet<DiscoverySubject> DiscoverySubjects { get; set; } = null!;

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
