using Bayteq.SnakesAndLadders.Application.Services.Board;
using Bayteq.SnakesAndLadders.Application.Services.Dice;
using Bayteq.SnakesAndLadders.Domain.Configuration;
using Bayteq.SnakesAndLadders.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bayteq.SnakesAndLadders.Application.Services.Game;

public class Game : IGame
{
    private readonly IDice _dice;
    private readonly IBoard _board;
    private readonly ILogger<Game> _logger;
    private readonly GameConfiguration _gameConfiguration;

    private bool _isGameStarted;
    private bool _isGameFinished;
    private int _numberOfPlayers;
    private GamePlayer _currentPlayer;
    private Queue<GamePlayer> _players = new(); 

    public Game(ILogger<Game> logger, IBoard board, IDice dice, IOptions<GameConfiguration> gameConfigOptions)
    {
        _dice = dice;
        _board = board;
        _logger = logger;
        _gameConfiguration = gameConfigOptions.Value;
    }
    public void AddPlayers(int numberOfPlayers)
    {
        if (numberOfPlayers < _gameConfiguration.MinimumPlayers) throw new Exception("The number of players must be at least 2");
        _numberOfPlayers = numberOfPlayers;
        for (var i = 1; i <= numberOfPlayers; i++)
        {
            var playerName = $"Player{i}";
            _players.Enqueue(GamePlayer.Create(playerName, i, _board.GetStartPosition()));
            _logger.LogInformation("Player {@PlayerName} added to the game. Position -> {@PlayerPosition}", playerName, i);
        }
    }

    public void StartGame()
    {
        _logger.LogInformation("Starting the game..");
        _isGameStarted = true;
        _isGameFinished = false;
        SetNextPlayerToPlay(true);
    }

    public string GetCurrentPlayer() => _currentPlayer.Name;

    public int Play()
    {
        if (!IsGameStarted()) throw new Exception("The game is not started. Please start the game");
        if (IsGameFinished()) throw new Exception("The game is over. The Player: " + _currentPlayer?.Name + " has won..!!");
        
        //Roll the dice
        _logger.LogInformation("Rolling.....");
        var rollDice = _dice.GetNextRoll();
        _logger.LogInformation("{@PlayerName} got a -> {@RollDice}", _currentPlayer?.Name, rollDice);
        var currentCell = _currentPlayer.CurrentPosition;
        //Get next position from board
        var nextBoardCell = GetNextTempPlayerPosition(rollDice);
        if (nextBoardCell > 0)
        {
            var nextPosition = _board.GetNextPlayerCell(nextBoardCell);
            UpdatePlayerPosition(_currentPlayer, nextPosition);
            if (!IsGameFinished())
                SetNextPlayerToPlay();
            return nextPosition;
        }
        SetNextPlayerToPlay();
        return currentCell;
    }

    public int GetPlayerPosition(string playerName) => _currentPlayer.CurrentPosition;

    public bool IsGameStarted() => _isGameStarted;

    public bool IsGameFinished() => _isGameFinished;

    #region Private Methods

    private void SetNextPlayerToPlay(bool initialPlayer = false)
    {
        if (_isGameFinished) return;
        if (!initialPlayer)
            _players.Enqueue(_currentPlayer);
        _currentPlayer = _players.Dequeue();
        _logger.LogInformation("Next player to play -> {@PlayerName}", _currentPlayer.Name);
    }

    private void UpdatePlayerPosition(GamePlayer? player, int newPosition)
    {
        if (newPosition == _board.GetLastPosition())
        {
            _logger.LogInformation("Congratulations player {@PlayerName}. You win the game..!!!!", player?.Name);
            EndGame();
            return;
        }

        if (player != null) player.CurrentPosition = newPosition;
    }
    private int GetNextTempPlayerPosition(int rollDice)
    {
        var newPosition = _currentPlayer.CurrentPosition + rollDice;
        if (newPosition <= _board.GetLastPosition()) return newPosition;
        _logger.LogInformation("Ohh.. You need the exact roll dice to finish: {@RollDiceNeeded}. Try again..!!", _board.GetLastPosition() - _currentPlayer?.CurrentPosition);
        return -1;
    }
    private void EndGame()
    {
        _isGameStarted = false;
        _isGameFinished = true;
        _players = new Queue<GamePlayer>();
        _currentPlayer = null;
    }
    
    #endregion
}