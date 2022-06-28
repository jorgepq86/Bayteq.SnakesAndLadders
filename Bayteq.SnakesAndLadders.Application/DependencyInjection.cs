using Bayteq.SnakesAndLadders.Application.Services.Board;
using Bayteq.SnakesAndLadders.Application.Services.Dice;
using Bayteq.SnakesAndLadders.Application.Services.Game;
using Bayteq.SnakesAndLadders.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bayteq.SnakesAndLadders.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GameConfiguration>(configuration.GetSection("GameConfiguration"));
        services.AddTransient<IBoard, Board>();
        services.AddTransient<IDice, Dice>();
        services.AddTransient<IGame, Game>();
    }
}