using Bayteq.SnakesAndLadders.Application.Common.Configuration;
using Bayteq.SnakesAndLadders.Domain.Entities;
using Bayteq.SnakesAndLadders.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Bayteq.SnakesAndLadders.Application.Services.Board;

public class Board : IBoard
{
    private readonly ILogger<Board> _logger;
    private readonly BoardConfiguration _boardConfiguration;

    public Board(IGameConfiguration configuration, ILogger<Board> logger)
    {
        _logger = logger;
        _boardConfiguration = configuration.LoadGameConfiguration();
        ValidateConfiguration();
    }

    public int GetStartPosition() => _boardConfiguration.StartPosition;
    public int GetLastPosition() => _boardConfiguration.LastPosition;

    public int GetNextPlayerCell(int cellNumber)
    {
        var specialObject = _boardConfiguration.SpecialObjects.FirstOrDefault(sp => sp.StartNumber == cellNumber);
        if (specialObject == null) return cellNumber;
        LogNextMovement(specialObject);
        return specialObject.EndNumber;
    }

    #region Private Methods

    private void ValidateConfiguration()
    {
        var ladders = _boardConfiguration.SpecialObjects.Where(sp => sp.SpecialType == SpecialType.Ladder);
        var snakes = _boardConfiguration.SpecialObjects.Where(sp => sp.SpecialType == SpecialType.Snake);
        ValidateLadders(ladders);
        ValidateSnakes(snakes);
        ValidateCrossCells(ladders, snakes);
    }
    private void LogNextMovement(SpecialObject currentSpecialObject)
    {
        switch (currentSpecialObject.SpecialType)
        {
            case SpecialType.Snake:
                _logger.LogInformation("Oh.. You are in a Snake. You must go back to cell {@NewPosition}", currentSpecialObject.EndNumber);
                break;
            case SpecialType.Ladder:
                _logger.LogInformation("Congrats.. You are in a Ladder. You must go to cell {@Positions}", currentSpecialObject.EndNumber);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void ValidateLadders(IEnumerable<SpecialObject> ladders)
    {
        var ladderMaxEndPos = ladders.Max(l => l.EndNumber);
        if (ladderMaxEndPos >= _boardConfiguration.LastPosition)
            throw new Exception("Ladder end number cannot be greater than " + _boardConfiguration.LastPosition);

        var ladderMinStartPos = ladders.Min(l => l.StartNumber);
        if (ladderMinStartPos <= _boardConfiguration.StartPosition)
            throw new Exception("Ladder start number must be greater than " + _boardConfiguration.StartPosition);

        if (ladders.Any(ladder => ladder.EndNumber <= ladder.StartNumber))
            throw new Exception("The Ladder end position must be greater than start position");
    }
    private void ValidateSnakes(IEnumerable<SpecialObject> snakes)
    {
        var snakeMaxStartPos = snakes.Max(s => s.StartNumber);
        if (snakeMaxStartPos >= _boardConfiguration.LastPosition)
            throw new Exception("Snake start number cannot be greater than " + _boardConfiguration.LastPosition);

        var snakeMinEndPos = snakes.Min(s => s.EndNumber);
        if (snakeMinEndPos <= _boardConfiguration.StartPosition)
            throw new Exception("Snake end number must be greater than " + _boardConfiguration.StartPosition);

        if (snakes.Any(s => s.EndNumber >= s.StartNumber))
            throw new Exception("Snake end position must be lower than start position");
    }
    private void ValidateCrossCells(IEnumerable<SpecialObject> ladders, IEnumerable<SpecialObject> snakes)
    {
        var snakeStartPositions = snakes.Select(s => s.StartNumber);
        var snakeEndPositions = snakes.Select(s => s.EndNumber);
        var ladderStartPositions = ladders.Select(l => l.StartNumber);
        var ladderEndPositions = ladders.Select(l => l.EndNumber);
        
        var startPosIntersection = snakeStartPositions.Intersect(ladderStartPositions);
        if (startPosIntersection.Any())
            throw new Exception("A Ladder and a Snake cannot have the same Start Position");

        var endPositionsIntersection = snakeEndPositions.Intersect(ladderEndPositions);
        if (endPositionsIntersection.Any())
            throw new Exception("A Ladder and a Snake cannot have the same End Position");

        var snakeStartLadderEndIntersection = snakeStartPositions.Intersect(ladderEndPositions);
        if (snakeStartLadderEndIntersection.Any())
            throw new Exception("A Snake start cannot be at the same position als a Ladder end");

        var snakeEndLadderStartIntersection = snakeEndPositions.Intersect(ladderStartPositions);
        if (snakeEndLadderStartIntersection.Any())
            throw new Exception("A Snake end cannot be at the same position als a Ladder start");
        
    }
    
    #endregion
}