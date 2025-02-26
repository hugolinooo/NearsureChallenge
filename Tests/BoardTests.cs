using Xunit;
using GameOfLife.Models;

namespace GameOfLife.Tests;

public class BoardTests
{
    [Fact]
    public void Board_Constructor_InitializesCorrectly()
    {
        // Arrange
        var grid = new bool[3, 3];

        // Act
        var board = new Board(grid);

        // Assert
        Assert.NotEqual(Guid.Empty, board.Id);
        Assert.Equal(3, board.Rows);
        Assert.Equal(3, board.Columns);
        Assert.Same(grid, board.Grid);
    }

    [Fact]
    public void ToDto_ConvertsCorrectly()
    {
        // Arrange
        var grid = new bool[,] {
            { true, false },
            { false, true }
        };
        var board = new Board(grid);

        // Act
        var dto = board.ToDto();

        // Assert
        Assert.Equal(board.Id, dto.Id);
        Assert.Equal(2, dto.Rows);
        Assert.Equal(2, dto.Columns);
        Assert.True(dto.Grid[0][0]);
        Assert.False(dto.Grid[0][1]);
        Assert.False(dto.Grid[1][0]);
        Assert.True(dto.Grid[1][1]);
    }

    [Fact]
    public void GetNextState_WithBlinkerPattern_TransformsCorrectly()
    {
        // Arrange
        var grid = new bool[,] {
            { false, false, false },
            { true, true, true },
            { false, false, false }
        };
        var board = new Board(grid);

        // Act
        var nextState = board.GetNextState();

        // Assert
        Assert.True(nextState.Grid[0, 1]);
        Assert.True(nextState.Grid[1, 1]);
        Assert.True(nextState.Grid[2, 1]);
    }

    [Fact]
    public void Board_WithNullGrid_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Board(null!));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void Board_WithInvalidDimensions_ThrowsArgumentException(int rows, int cols)
    {
        // Arrange
        var grid = new bool[rows, cols];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Board(grid));
    }
}
