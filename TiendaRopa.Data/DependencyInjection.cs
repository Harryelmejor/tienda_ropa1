using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TiendaRopa.Data.Repositories;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TiendaDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICategoriaService, CategoriaRepository>();
        services.AddScoped<IPrendaService, PrendaRepository>();
        services.AddScoped<IClienteService, ClienteRepository>();
        services.AddScoped<IEmpleadoService, EmpleadoRepository>();
        services.AddScoped<IVentaService, VentaRepository>();

        return services;
    }
}
