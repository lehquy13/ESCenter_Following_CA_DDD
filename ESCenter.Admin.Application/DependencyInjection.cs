using System.Reflection;
using ESCenter.Admin.Application.Contracts.Courses.Dtos;
using ESCenter.Admin.Application.DashBoards;
using ESCenter.Admin.Application.ServiceImpls;
using ESCenter.Admin.Application.ServiceImpls.Subjects.Queries.GetSubjects;
using ESCenter.Application;
using ESCenter.Application.Behaviors;
using ESCenter.Application.Mapping;
using Matt.Paginated;
using Matt.ResultObject;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ESCenter.Admin.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddBaseApplication(typeof(DependencyInjection).Assembly);
        services.AddScoped<IDashboardServices, DashboardServices>();

        services.AddScoped(
            typeof(IPipelineBehavior<GetAllSubjectsQuery, Result<List<SubjectDto>>>),
            typeof(CachingBehavior<GetAllSubjectsQuery, Result<List<SubjectDto>>>));

        return services;
    }

    public static Assembly[] GetApplicationCoreAssemblies =>
    [
        typeof(DependencyInjection).Assembly,
        typeof(ESCenter.Domain.DependencyInjection).Assembly
    ];
}