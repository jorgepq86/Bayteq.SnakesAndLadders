namespace Bayteq.SnakesAndLadders.Application.Services.Dice;

public class Dice : IDice
{
    private readonly Random _randomGenerator = new();
    
    public int GetNextRoll()
    {
        return _randomGenerator.Next(1, 7);
    }
}