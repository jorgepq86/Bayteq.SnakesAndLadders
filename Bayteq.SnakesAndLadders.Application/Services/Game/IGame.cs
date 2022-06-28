namespace Bayteq.SnakesAndLadders.Application.Services.Game;

public interface IGame
{
    void AddPlayers(int numberOfPlayers);
    void StartGame();
    string GetCurrentPlayer();
    int Play();
    int GetPlayerPosition(string player);
    bool IsGameStarted();
    bool IsGameFinished();
}