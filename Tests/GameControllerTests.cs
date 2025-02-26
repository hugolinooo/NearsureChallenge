using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using GameOfLife.Controllers;
using GameOfLife.Services;
using GameOfLife.Models;
using GameOfLife.Models.Dto;

namespace GameOfLife.Tests;

public class GameControllerTests
{
    private readonly Mock<IGameOfLifeService> _mockService;
    private readonly GameController _controller;

    public GameControllerTests()
    {
        _mockService = new Mock<IGameOfLifeService>();
        _controller = new GameController(_mockService.Object);
    }

    [Fact]
    public void CreateBoard_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new BoardRequestDto
        {
            Grid = new bool[][] {
                new bool[] { false, true, false },
                new bool[] { false, true, false },
                new bool[] { false, true, false }
            }
        };
        var board = new Board(new bool[3, 3]);
        _mockService.Setup(s => s.CreateBoard(It.IsAny<bool[,]>()))
            .Returns(board);

        // Act
        var result = _controller.CreateBoard(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<BoardResponseDto>(okResult.Value);
        Assert.Equal(board.Id, returnValue.Id);
    }

    [Fact]
    public void GetNextState_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetNextState(It.IsAny<Guid>()))
            .Throws<KeyNotFoundException>();

        // Act
        var result = _controller.GetNextState(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void GetFinalState_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var board = new Board(new bool[3, 3]);
        _mockService.Setup(s => s.GetFinalState(It.IsAny<Guid>()))
            .Returns(board);

        // Act
        var result = _controller.GetFinalState(Guid.NewGuid());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}
