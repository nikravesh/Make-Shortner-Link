using Microsoft.EntityFrameworkCore;
using ShortenerLink.BLL.UriShortener.Classes;
using ShortenerLink.BLL.UriShortener.Interfaces;
using ShortenerLink.DAL.DataContext;

namespace ShortenerLink.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<UriShortenerDBContext>(c =>
            c.UseSqlServer(config.GetConnectionString("DefaultConnectionString")));
        services.AddTransient<IUriShortenerService, UriShortenerService>();


        return services;
    }
}
