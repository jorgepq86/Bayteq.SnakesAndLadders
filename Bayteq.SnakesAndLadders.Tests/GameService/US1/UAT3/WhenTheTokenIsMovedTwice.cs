using Bayteq.SnakesAndLadders.Application.Services.Game;
using Bayteq.SnakesAndLadders.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Bayteq.SnakesAndLadders.Tests.GameService.US1;

public class WhenTheTokenIsMovedTwice : TestForGame
{
    private int _playerPosition;

    protected override void Setup()
    {
        base.Setup();
        _boardMock.Setup(b => b.GetStartPosition()).Returns(1);
        _boardMock.Setup(b => b.GetLastPosition()).Returns(20);
        _boardMock.Setup(b => b.GetNextPlayerCell(It.IsAny<int>())).Returns<int>((cellNumber) => cellNumber);
        _diceMock.SetupSequence(d => d.GetNextRoll())
            .Returns(3)
            .Returns(4);

    }

    /// <summary>
    /// Given the token is on square 1
    /// </summary>
    /// <returns></returns>
    protected override IGame Given()
    {
        var game = new Game(_loggerMock.Object, _boardMock.Object, _diceMock.Object, _gameConfigOptions);
        game.AddPlayers(1);
        game.StartGame();
        return game;
    }

    /// <summary>
    /// When the token is moved 3 spaces
    /// And then it is moved 4 space
    /// </summary>
    protected override void WhenAsync()
    {
        Subject.Play();
        _playerPosition = Subject.Play();
    }
    
    /// <summary>
    /// Then the token is on square 8
    /// </summary>
    [Test]
    public void ThenTheTokenIsOnSquareEight()
    {
        _playerPosition.Should().Be(8);
    }
}