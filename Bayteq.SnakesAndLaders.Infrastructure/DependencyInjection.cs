using Bayteq.SnakesAndLadders.Application.Common.Configuration;
using Bayteq.SnakesAndLadders.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bayteq.SnakesAndLadders.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IGameConfiguration, GameConfigurationFromFile>();
    }
}