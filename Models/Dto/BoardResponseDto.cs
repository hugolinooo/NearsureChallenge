namespace GameOfLife.Models.Dto;

public class BoardResponseDto
{
    public Guid Id { get; set; }
    public bool[][] Grid { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
}
