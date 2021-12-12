namespace Mitekat.Seedwork.Features.DependencyInjection;

using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationInjectionExtensions
{
    public static IMvcBuilder AddFluentValidation(this IMvcBuilder mvcBuilder) =>
        mvcBuilder.AddFluentValidation(Assembly.GetCallingAssembly());
    
    public static IMvcBuilder AddFluentValidation(this IMvcBuilder mvcBuilder, Assembly assembly) =>
        mvcBuilder.AddFluentValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;
            options.RegisterValidatorsFromAssembly(assembly, includeInternalTypes: true);
        });
}
