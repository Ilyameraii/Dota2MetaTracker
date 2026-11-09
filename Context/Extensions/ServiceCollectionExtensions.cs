using Context.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts.Repositories;

namespace Context.Extensions;

/// <summary>
///     Расширения регистрации зависимостей слоя доступа к базе данных
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Регистрирует контекст базы данных и единицу работы
    /// </summary>
    public static IServiceCollection AddContextServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MetaTracker")
            ?? throw new InvalidOperationException(
                "Не задана строка подключения к базе данных в секции ConnectionStrings:MetaTracker.");

        services.AddDbContext<MetaTrackerDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsAssembly(typeof(MetaTrackerDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<MetaTrackerDbContext>());

        services.AddRepositories();

        return services;
    }
}
