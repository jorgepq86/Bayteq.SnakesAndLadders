using Bayteq.SnakesAndLadders.Application.Services.Game;
using Bayteq.SnakesAndLadders.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Bayteq.SnakesAndLadders.Tests.GameService.US2;

public class WhenThePlayerHasNottWonTheGame : TestForGame
{
    private int _playerPosition;

    protected override void Setup()
    {
        base.Setup();
        _boardMock.Setup(b => b.GetStartPosition()).Returns(1);
        _boardMock.Setup(b => b.GetLastPosition()).Returns(100);
        _boardMock.Setup(b => b.GetNextPlayerCell(It.IsAny<int>())).Returns<int>((cellNumber) => cellNumber);
        _diceMock.SetupSequence(d => d.GetNextRoll())
            .Returns(96)
            .Returns(4);

    }

    /// <summary>
    /// Given the token is on square 97
    /// </summary>
    /// <returns></returns>
    protected override IGame Given()
    {
        var game = new Game(_loggerMock.Object, _boardMock.Object, _diceMock.Object, _gameConfigOptions);
        game.AddPlayers(1);
        game.StartGame();
        game.Play();
        return game;
    }

    /// <summary>
    /// When the token is moved 4 spaces
    /// </summary>
    protected override void WhenAsync()
    {
        _playerPosition = Subject.Play();
    }
    
    /// <summary>
    /// Then the token is on square 97
    /// </summary>
    [Test]
    public void ThenTheTokenIsOnSquareNinetySeven()
    {
        _playerPosition.Should().Be(97);
    }
    
    /// <summary>
    /// And the player has won the game
    /// </summary>
    [Test]
    public void ThenTheTokenIsOnSquareOne()
    {
        Subject.IsGameFinished().Should().BeFalse();
    }
}