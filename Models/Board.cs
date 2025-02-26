using GameOfLife.Models.Dto;
using GameOfLife.Services;
using System;

namespace GameOfLife.Models;

// Represents the game board for the Game of Life
public class Board
{
    public Guid Id { get; private set; }
    public bool[,] Grid { get; private set; }
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    // Creates a new board with the specified initial state
    public Board(bool[,] initialState)
    {
        if (initialState == null)
            throw new ArgumentNullException(nameof(initialState));
            
        if (initialState.GetLength(0) <= 0 || initialState.GetLength(1) <= 0)
            throw new ArgumentException("Grid dimensions must be greater than 0");

        Id = Guid.NewGuid();
        Grid = initialState;
        Rows = initialState.GetLength(0);
        Columns = initialState.GetLength(1);
    }

    // Computes and returns the next generation state of the board
    public Board GetNextState()
    {
        var gameOfLife = new GameOfLifeService();
        return new Board(gameOfLife.NextGeneration(Grid));
    }

    // Converts the board state to a DTO for API responses
    public BoardResponseDto ToDto()
    {
        var gridArray = new bool[Rows][];
        for (int i = 0; i < Rows; i++)
        {
            gridArray[i] = new bool[Columns];
            for (int j = 0; j < Columns; j++)
            {
                gridArray[i][j] = Grid[i, j];
            }
        }

        return new BoardResponseDto
        {
            Id = Id,
            Grid = gridArray,
            Rows = Rows,
            Columns = Columns
        };
    }
}
