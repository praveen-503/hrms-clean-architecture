using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using HRPayroll.Application.Contracts.Services;
using HRPayroll.Application.Services;

namespace HRPayroll.Application;

/// <summary>
/// Service registration extension methods for the Application layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register Business Services
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}
