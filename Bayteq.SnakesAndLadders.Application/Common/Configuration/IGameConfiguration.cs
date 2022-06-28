using Bayteq.SnakesAndLadders.Domain.Entities;

namespace Bayteq.SnakesAndLadders.Application.Common.Configuration;

public interface IGameConfiguration
{
    BoardConfiguration LoadGameConfiguration();
}