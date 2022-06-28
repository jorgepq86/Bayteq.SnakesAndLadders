using Bayteq.SnakesAndLadders.Application.Services.Game;
using Bayteq.SnakesAndLadders.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Bayteq.SnakesAndLadders.Tests.GameService.US1;

public class WhenTheTokenIsMoved : TestForGame
{
    private int _playerPosition;

    protected override void Setup()
    {
        base.Setup();
        _boardMock.Setup(b => b.GetStartPosition()).Returns(1);
        _boardMock.Setup(b => b.GetLastPosition()).Returns(20);
        _boardMock.Setup(b => b.GetNextPlayerCell(It.IsAny<int>())).Returns<int>((cellNumber) => cellNumber);
        _diceMock.Setup(d => d.GetNextRoll()).Returns(3);
    }

    /// <summary>
    /// Given the token is on square 1
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
    /// When the token is moved 3 spaces
    /// </summary>
    protected override void WhenAsync()
    {
        _playerPosition = Subject.Play();
    }
    
    /// <summary>
    /// Then the token is on square 4
    /// </summary>
    [Test]
    public void ThenTheTokenIsOnSquareFour()
    {
        _playerPosition.Should().Be(4);
    }
}