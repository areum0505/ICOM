using ICOM.Application.Interfaces;
using ICOM.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ICOM.Application;

/// <summary>
/// Application 계층 DI 등록 확장 메서드
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICodeService, CodeService>();
        return services;
    }
}
