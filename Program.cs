using GameOfLife.Services;
using GameOfLife.Models;
using GameOfLife.Models.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Game of Life API",
        Version = "v1",
        Description = "A Conway's Game of Life implementation"
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<IGameOfLifeService, GameOfLifeService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game of Life API v1");
    c.RoutePrefix = "swagger";
});

// Enable CORS
app.UseCors();

// Remove this line if you want to allow HTTP access
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

// Ensure the app listens on port 5000 for HTTP
if (app.Environment.IsDevelopment())
{
    app.Run("http://localhost:5000");
}
else
{
    app.Run();
}
