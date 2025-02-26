using GameOfLife.Models;
using System;
using System.Linq;

namespace GameOfLife.Services;

public interface IGameOfLifeService
{
    // Creates a new game board with the given initial state
    Board CreateBoard(bool[,] initialState);
    // Calculates and returns the next generation state for a board
    Board GetNextState(Guid boardId);
    // Returns the board state after specified number of generations
    Board GetStateAfterGenerations(Guid boardId, int generations);
    // Finds the final stable or oscillating state of the board
    Board GetFinalState(Guid boardId);
    // Computes the next generation state using Conway's Game of Life rules
    bool[,] NextGeneration(bool[,] currentState);
}

public class GameOfLifeService : IGameOfLifeService
{
    private readonly Dictionary<Guid, Board> _boards = new();
    private const int MaxIterations = 1000;

    public Board CreateBoard(bool[,] initialState)
    {
        if (initialState == null)
            throw new ArgumentNullException(nameof(initialState));

        int rows = initialState.GetLength(0);
        int cols = initialState.GetLength(1);

        if (rows == 0 || cols == 0)
            throw new ArgumentException("Grid dimensions must be greater than 0");

        var board = new Board(initialState);
        _boards[board.Id] = board;
        return board;
    }

    public Board GetNextState(Guid boardId)
    {
        var board = GetBoardById(boardId);
        var nextState = board.GetNextState();
        _boards[boardId] = nextState;
        return nextState;
    }

    public Board GetStateAfterGenerations(Guid boardId, int generations)
    {
        if (generations <= 0)
            throw new ArgumentException("Number of generations must be greater than 0");

        var board = GetBoardById(boardId);
        
        for (int i = 0; i < generations; i++)
        {
            board = board.GetNextState();
        }
        
        _boards[boardId] = board;
        return board;
    }

    public Board GetFinalState(Guid boardId)
    {
        var board = GetBoardById(boardId);
        var iteration = 0;
        var states = new HashSet<string>();

        while (iteration < MaxIterations)
        {
            var boardState = SerializeBoard(board);
            if (states.Contains(boardState))
                return board;

            states.Add(boardState);
            board = board.GetNextState();
            iteration++;
        }

        throw new Exception("Board did not reach a final state after 1000 iterations");
    }

    public bool[,] NextGeneration(bool[,] currentState)
    {
        int rows = currentState.GetLength(0);
        int columns = currentState.GetLength(1);
        bool[,] nextState = new bool[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int liveNeighbors = CountLiveNeighbors(currentState, row, col);
                nextState[row, col] = ShouldCellLive(currentState[row, col], liveNeighbors);
            }
        }

        return nextState;
    }

    // Retrieves a board by its ID or throws KeyNotFoundException
    private Board GetBoardById(Guid boardId)
    {
        if (!_boards.TryGetValue(boardId, out var board))
            throw new KeyNotFoundException("Board not found");
        return board;
    }

    // Converts board state to string for comparison
    private string SerializeBoard(Board board)
    {
        return string.Join("", board.Grid.Cast<bool>().Select(b => b ? '1' : '0'));
    }

    // Counts live neighbors for a cell at given position
    private int CountLiveNeighbors(bool[,] grid, int row, int col)
    {
        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);
        int liveNeighbors = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                
                int neighborRow = row + i;
                int neighborCol = col + j;
                
                if (IsValidCell(neighborRow, neighborCol, rows, columns))
                {
                    liveNeighbors += grid[neighborRow, neighborCol] ? 1 : 0;
                }
            }
        }

        return liveNeighbors;
    }

    // Checks if cell coordinates are within grid boundaries
    private bool IsValidCell(int row, int col, int maxRows, int maxCols)
    {
        return row >= 0 && row < maxRows && col >= 0 && col < maxCols;
    }

    // Determines if a cell should live or die based on Conway's rules
    private bool ShouldCellLive(bool currentState, int liveNeighbors)
    {
        return currentState 
            ? liveNeighbors == 2 || liveNeighbors == 3  // Cell survives
            : liveNeighbors == 3;                       // Cell is born
    }

    public void ValidateDimensions(int rows, int cols)
    {
        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("Grid dimensions must be greater than 0");
        if (rows < 0 || cols < 0)
            throw new ArgumentException("Grid dimensions cannot be negative");
    }
}