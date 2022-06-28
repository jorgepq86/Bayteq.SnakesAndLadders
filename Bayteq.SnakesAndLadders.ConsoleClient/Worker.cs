using Bayteq.SnakesAndLadders.Application.Services.Game;

namespace Bayteq.SnakesAndLadders.ConsoleClient;

public class Worker : BackgroundService
{
    private readonly IGame _game;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, IGame game)
    {
        _logger = logger;
        _game = game;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Welcome to Snake and Ladders");
        var numberOfPlayers = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("\nPress a number to continue or press Q to Exit");
            _logger.LogInformation("Current number of players: {@NumPlayers}", numberOfPlayers);
            _logger.LogInformation("P -> Number of Players");
            _logger.LogInformation("J -> Start Game");
            _logger.LogInformation("Q -> Quit");
            var selectedOption = Console.ReadKey();
            if (selectedOption.Key != null)
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (selectedOption.Key)
                {
                    case ConsoleKey.P:
                        numberOfPlayers = GetPlayers();
                        break;
                    case ConsoleKey.J:
                        Play(numberOfPlayers);
                        break;
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        _logger.LogInformation("You must select a valid option");
                        break;
                }
            }
            else
                _logger.LogInformation("You must select a valid option");
        }
    }

    private int GetPlayers()
    {
        while (true)
        {
            _logger.LogInformation("\nPlease enter the numbers of players or R to return");
            var playersKey = Console.ReadKey();
            if (playersKey.Key == ConsoleKey.R) return 0;
            if (int.TryParse(playersKey.KeyChar.ToString(), out var numPlayers) && numPlayers >= 2)
                return numPlayers;
            _logger.LogInformation("\nThe number of player must a number and greater than 1");
        }
    }

    private void Play(int numberOfPlayers)
    {
        if (numberOfPlayers <= 1)
        {
            _logger.LogInformation("\nThe number of player must a number and greater than 1");
            return;
        }

        _game.AddPlayers(numberOfPlayers);
        _game.StartGame();
        while (!_game.IsGameFinished())
        {
            var nextPlayer = _game.GetCurrentPlayer();
            var nextPlayerPosition = _game.GetPlayerPosition(nextPlayer);
            _logger.LogInformation("Player -> {@PlayerName} your in position: {@PlayerPositiom} ", nextPlayer, nextPlayerPosition);
            _logger.LogInformation("Player -> {@PlayerName} is your turn. Press any key to roll the dice and play", nextPlayer);
            Console.ReadKey();
            _game.Play();
        }
    }
}