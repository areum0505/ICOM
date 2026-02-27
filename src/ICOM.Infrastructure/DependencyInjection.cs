using ICOM.Application.Interfaces;
using ICOM.Infrastructure.Data;
using ICOM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ICOM.Infrastructure;

/// <summary>
/// Infrastructure 계층 DI 등록 확장 메서드
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // AppDbContext 등록 - MSSQL 연결
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repository 등록
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICodeRepository, CodeRepository>();

        return services;
    }
}
