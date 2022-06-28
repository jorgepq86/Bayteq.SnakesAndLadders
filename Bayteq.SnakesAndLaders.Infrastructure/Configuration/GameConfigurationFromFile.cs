using System.Text.Json;
using Bayteq.SnakesAndLadders.Application.Common.Configuration;
using Bayteq.SnakesAndLadders.Domain.Entities;

namespace Bayteq.SnakesAndLadders.Infrastructure.Configuration;

public class GameConfigurationFromFile : IGameConfiguration
{
    private const string configFileName = "./Configuration/GameConfiguration.json";
    public BoardConfiguration LoadGameConfiguration()
    {
        var configFile = File.ReadAllText(configFileName);
        return JsonSerializer.Deserialize<BoardConfiguration>(configFile);
    }
}