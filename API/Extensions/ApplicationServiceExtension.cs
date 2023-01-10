using API.Data;
using API.Interface;
using API.Service;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextFactory<DataContext>(opt =>{
             opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}