using Microsoft.AspNetCore.Mvc;
using GameOfLife.Models;
using GameOfLife.Models.Dto;
using GameOfLife.Services;
using System;

namespace GameOfLife.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameOfLifeService _gameService;

    public GameController(IGameOfLifeService gameService)
    {
        _gameService = gameService;
    }

    // Creates a new game board from the provided initial state
    [HttpPost("create")]
    public IActionResult CreateBoard([FromBody] BoardRequestDto request)
    {
        var rows = request.Grid.Length;
        var cols = request.Grid[0].Length;
        var grid = new bool[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = request.Grid[i][j];
            }
        }

        var board = _gameService.CreateBoard(grid);
        return Ok(board.ToDto());
    }

    // Returns the next generation state for the specified board
    [HttpGet("{boardId}/next")]
    public IActionResult GetNextState(Guid boardId)
    {
        try
        {
            var nextState = _gameService.GetNextState(boardId);
            return Ok(nextState.ToDto());
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Board not found");
        }
    }

    // Returns the board state after specified number of generations
    [HttpGet("{boardId}/generations/{count}")]
    public IActionResult GetStateAfterGenerations(Guid boardId, int count)
    {
        try
        {
            var finalState = _gameService.GetStateAfterGenerations(boardId, count);
            return Ok(finalState.ToDto());
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Board not found");
        }
    }

    // Returns the final stable or oscillating state of the board
    [HttpGet("{boardId}/final")]
    public IActionResult GetFinalState(Guid boardId)
    {
        try
        {
            var finalState = _gameService.GetFinalState(boardId);
            return Ok(finalState.ToDto());
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Board not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
