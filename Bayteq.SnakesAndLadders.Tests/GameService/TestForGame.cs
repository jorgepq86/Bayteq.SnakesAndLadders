using Bayteq.SnakesAndLadders.Application.Services.Board;
using Bayteq.SnakesAndLadders.Application.Services.Dice;
using Bayteq.SnakesAndLadders.Application.Services.Game;
using Bayteq.SnakesAndLadders.Domain.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Bayteq.SnakesAndLadders.Tests.GameService;

public abstract class TestForGame : TestFor<IGame>
{
    protected Mock<ILogger<Game>> _loggerMock;
    protected Mock<IBoard> _boardMock;
    protected Mock<IDice> _diceMock;
    protected IOptions<GameConfiguration> _gameConfigOptions;
    protected override void Setup()
    {
        _loggerMock = new Mock<ILogger<Game>>();
        _boardMock = new Mock<IBoard>();
        _diceMock = new Mock<IDice>();
        _gameConfigOptions = ConfigureGameOptions();
    }
    
    protected override void TearDown()
    {
        _loggerMock.Reset();
        _boardMock.Reset();
        _diceMock.Reset();
        _gameConfigOptions = null;
    }

    protected virtual IOptions<GameConfiguration> ConfigureGameOptions()
    {
        return Options.Create(new GameConfiguration
        {
            MinimumPlayers = 1
        });
    }
}