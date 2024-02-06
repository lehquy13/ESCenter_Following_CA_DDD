using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Persistence.Entity_Framework_Core;
using ESCenter.Persistence.Persistence.Repositories;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            ConfigurationManager configuration
        )
        {
            // set configuration settings to emailSettingName and turn it into Singleton
          
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                )
            );

            // Dependency Injection for repository
            // services.AddScoped<ISubjectRepository, SubjectRepository>();
            // services.AddScoped<ICourseRepository, CourseRepository>();
            // services.AddScoped<IUserRepository, UserRepository>();
            // services.AddScoped<ITutorRepository, TutorRepository>();

            return services;
        }

       
    }
}