using Bayteq.SnakesAndLadders.Application.Services.Game;
using Bayteq.SnakesAndLadders.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Bayteq.SnakesAndLadders.Tests.GameService.US1;

public class WhenTheTokenIsPlacedOnTheBoard : TestForGame
{
    private int _playerPosition;

    protected override void Setup()
    {
        base.Setup();
        _boardMock.Setup(b => b.GetStartPosition()).Returns(1);
    }

    /// <summary>
    /// Given the game is started
    /// </summary>
    /// <returns></returns>
    protected override IGame Given()
    {
        var game = new Game(_loggerMock.Object, _boardMock.Object, _diceMock.Object, _gameConfigOptions);
        game.AddPlayers(2);
        game.StartGame();
        return game;
    }

    /// <summary>
    /// When the token is placed on the board
    /// </summary>
    protected override void WhenAsync()
    {
        _playerPosition = Subject.GetPlayerPosition(Subject.GetCurrentPlayer());
    }
    
    /// <summary>
    /// Then the token is on square 1 
    /// </summary>
    [Test]
    public void ThenTheTokenIsOnSquareOne()
    {
        _playerPosition.Should().Be(1);
    }
}