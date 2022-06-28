namespace Bayteq.SnakesAndLadders.Domain.Entities;

public class GamePlayer
{
    public string Name { get; }
    public int Order { get; }
    public int CurrentPosition { get; set; }
    
    private GamePlayer(string name, int order, int currentPosition)
    {
        Name = name;
        Order = order;
        CurrentPosition = currentPosition;
    }

    public static GamePlayer Create(string name, int order, int currentPosition)
    {
        return new GamePlayer(name, order, currentPosition);
    }
}