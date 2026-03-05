using ICOM.Application.Interfaces;
using ICOM.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ICOM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICodeService, CodeService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        return services;
    }
}
