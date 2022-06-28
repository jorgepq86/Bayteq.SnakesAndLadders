namespace Bayteq.SnakesAndLadders.Application.Services.Board;

public interface IBoard
{
    int GetNextPlayerCell(int cellNumber);
    int GetStartPosition();
    int GetLastPosition();
}