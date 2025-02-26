using Xunit;
using GameOfLife.Services;
using GameOfLife.Models;

namespace GameOfLife.Tests;

public class GameOfLifeServiceTests
{
    private readonly IGameOfLifeService _service;

    public GameOfLifeServiceTests()
    {
        _service = new GameOfLifeService();
    }

    [Fact]
    public void CreateBoard_WithValidState_ReturnsBoard()
    {
        // Arrange
        var initialState = new bool[3, 3];

        // Act
        var board = _service.CreateBoard(initialState);

        // Assert
        Assert.NotNull(board);
        Assert.Equal(3, board.Rows);
        Assert.Equal(3, board.Columns);
    }

    [Fact]
    public void CreateBoard_WithNullState_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.CreateBoard(null!));
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(3, 0)]
    public void CreateBoard_WithZeroDimensions_ThrowsArgumentException(int rows, int cols)
    {
        // Arrange
        var initialState = new bool[rows == 0 ? 0 : 3, cols == 0 ? 0 : 3];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.CreateBoard(initialState));
    }

    [Theory]
    [InlineData(-1, 3)]
    [InlineData(3, -1)]
    public void CreateBoard_WithNegativeDimensions_ThrowsArgumentException(int rows, int cols)
    {
        // Act & Assert
        var service = (GameOfLifeService)_service;
        Assert.Throws<ArgumentException>(() => service.ValidateDimensions(rows, cols));
    }

    [Fact]
    public void GetNextState_WithBlinkerPattern_OscillatesProperly()
    {
        // Arrange
        var initialState = new bool[,] {
            { false, true, false },
            { false, true, false },
            { false, true, false }
        };
        var board = _service.CreateBoard(initialState);

        // Act
        var nextState = _service.GetNextState(board.Id);

        // Assert
        Assert.True(nextState.Grid[1, 0]);
        Assert.True(nextState.Grid[1, 1]);
        Assert.True(nextState.Grid[1, 2]);
    }

    [Fact]
    public void GetNextState_WithInvalidId_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => 
            _service.GetNextState(Guid.NewGuid()));
    }

    [Fact]
    public void GetFinalState_WithStablePattern_ReturnsUnchangedState()
    {
        // Arrange
        var initialState = new bool[,] {
            { true, true },
            { true, true }
        };
        var board = _service.CreateBoard(initialState);

        // Act
        var finalState = _service.GetFinalState(board.Id);

        // Assert
        Assert.Equal(board.Grid, finalState.Grid);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void GetStateAfterGenerations_WithValidGenerations_ReturnsCorrectState(int generations)
    {
        // Arrange
        var initialState = new bool[,] {
            { false, true, false },
            { false, true, false },
            { false, true, false }
        };
        var board = _service.CreateBoard(initialState);

        // Act
        var futureState = _service.GetStateAfterGenerations(board.Id, generations);

        // Assert
        Assert.NotNull(futureState);
        Assert.Equal(3, futureState.Rows);
        Assert.Equal(3, futureState.Columns);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GetStateAfterGenerations_WithInvalidGenerations_ThrowsArgumentException(int generations)
    {
        // Arrange
        var board = _service.CreateBoard(new bool[3, 3]);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _service.GetStateAfterGenerations(board.Id, generations));
    }
}
