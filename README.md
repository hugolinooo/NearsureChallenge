# Game of Life API

A REST API implementation of Conway's Game of Life using .NET 7.0.

## Problem Description

This API implements Conway's Game of Life with the following features:
- Upload new board states
- Calculate next state for a board
- Calculate state after X iterations
- Calculate final state (or error if no conclusion)

## Technical Stack

- .NET 7.0
- ASP.NET Core Web API
- xUnit for testing
- In-memory cache for state persistence
- Swagger for API documentation

## Architecture

### Project Structure
```
/
 GameOfLife/
   ├── Controllers/
   │   └── GameController.cs          # API endpoints
   ├── Models/
   │   ├── Board.cs                   # Board domain model
   │   └── Dtos/
   │       ├── BoardRequest.cs        # Request data model
   │       └── BoardResponse.cs       # Response data model
   ├── Services/
   │   ├── IGameOfLifeService.cs      # Service interface
   │   └── GameOfLifeService.cs       # Game logic implementation
   ├── Properties/
   │   └── launchSettings.json        # Launch configuration
   ├── Program.cs                     # Application entry point
   ├── appsettings.json              # Configuration file
   ├── GameOfLife.csproj             # Main project file
   ├── Tests/
   │    ├── GameControllerTests.cs       # Controller unit tests
   │    ├── GameOfLifeServiceTests.cs    # Service unit tests
   │    ├── BoardTests.cs                # Model unit tests
   │    └── GameOfLife.Tests.csproj      # Test project file
   └─ README.md                          # Project documentation
```

### Design Patterns & Principles
- SOLID principles
- Repository pattern for state persistence
- Factory pattern for board creation
- Strategy pattern for game rules

## API Endpoints

### POST /api/game/board
Upload a new board state
- Returns: Board ID

### GET /api/game/board/{id}/next
Get next state for a board
- Returns: Next board state

### GET /api/game/board/{id}/future/{steps}
Get board state after X steps
- Returns: Future board state

### GET /api/game/board/{id}/final
Get final stable state
- Returns: Final state or error

## Setup Instructions

1. Prerequisites:
   - .NET 7.0 SDK
   - Visual Studio 2022 or VS Code

2. Clone and Run:
   ```bash
   git clone [repository-url]
   cd NearsureChallengeFinal
   dotnet restore
   dotnet build
   dotnet run
   ```

3. The API will be available at:
   - HTTP: http://localhost:5000
   - HTTPS: https://localhost:5001

## Running the Application

1. Navigate to the project directory:
```bash
cd /GameOfLife
```

2. Build the project:
```bash
dotnet build
```

3. Run the application:
```bash
dotnet run
```

The API will be available at `http://localhost:5000`. You can access the Swagger documentation at `http://localhost:5000/swagger`.

## Running the Tests

Execute all tests using:
```bash
cd /Tests
dotnet test
```

## Unit Tests Documentation

### GameControllerTests
Tests the API endpoints functionality:
- `CreateBoard_WithValidRequest_ReturnsOkResult`: Verifies board creation with valid input
- `GetNextState_WithInvalidId_ReturnsNotFound`: Ensures proper handling of invalid board IDs
- `GetFinalState_WithValidId_ReturnsOkResult`: Validates final state retrieval

### GameOfLifeServiceTests
Tests the core game logic:
- `CreateBoard_WithValidState_ReturnsBoard`: Validates board creation
- `CreateBoard_WithNullState_ThrowsArgumentNullException`: Tests null input handling
- `CreateBoard_WithZeroDimensions_ThrowsArgumentException`: Verifies dimension validation
- `CreateBoard_WithNegativeDimensions_ThrowsArgumentException`: Tests negative dimension handling
- `GetNextState_WithBlinkerPattern_OscillatesProperly`: Validates blinker pattern oscillation
- `GetNextState_WithInvalidId_ThrowsKeyNotFoundException`: Tests invalid board ID handling
- `GetFinalState_WithStablePattern_ReturnsUnchangedState`: Verifies stable pattern detection
- `GetStateAfterGenerations_WithValidGenerations_ReturnsCorrectState`: Tests multi-generation progression
- `GetStateAfterGenerations_WithInvalidGenerations_ThrowsArgumentException`: Validates generation count

### BoardTests
Tests the Board model functionality:
- `Board_Constructor_InitializesCorrectly`: Validates board initialization
- `ToDto_ConvertsCorrectly`: Tests DTO conversion
- `GetNextState_WithBlinkerPattern_TransformsCorrectly`: Verifies state transformation
- `Board_WithNullGrid_ThrowsArgumentNullException`: Tests null grid handling
- `Board_WithInvalidDimensions_ThrowsArgumentException`: Validates dimension constraints

## Using the API with Swagger

1. Access Swagger UI:
   - Open your browser and navigate to: https://localhost:5001/swagger

2. Available Endpoints:

   ### Create a New Board
   - Endpoint: POST `/Game/create`
   - Example request body:
   ```json
   {
     "grid": [
       [false, true, false],
       [false, true, false],
       [false, true, false]
     ]
   }
   ```
   - Response: Returns board ID and initial state

   ### Get Next Generation
   - Endpoint: GET `/Game/{boardId}/next`
   - Replace `{boardId}` with the GUID received from create
   - Response: Returns the next state of the board

   ### Get Multiple Generations
   - Endpoint: GET `/Game/{boardId}/generations/{count}`
   - Replace `{boardId}` with board GUID and `{count}` with number of generations
   - Example: `/Game/123e4567-e89b-12d3-a456-426614174000/generations/5`

   ### Get Final State
   - Endpoint: GET `/Game/{boardId}/final`
   - Returns the stable state or oscillating pattern

## Example Usage

1. Create a Glider Pattern:
   ```json
   [
     [false, true, false],
     [false, false, true],
     [true, true, true]
   ]
   ```

2. Create a Blinker Pattern:
   ```json
   [
     [false, false, false],
     [true, true, true],
     [false, false, false]
   ]
   ```

3. Copy the returned board ID and use it in subsequent requests to observe the pattern evolution.

## Response Format

All board states are returned in the following format:
```json
{
  "id": "guid-here",
  "grid": [
    [false, true, false],
    [false, true, false],
    [false, true, false]
  ],
  "rows": 3,
  "columns": 3
}
```

## Technical Decisions

### State Persistence
- Using in-memory cache with backup persistence
- Ensures service resilience during restarts
- Thread-safe implementation

### Error Handling
- Global exception middleware
- Structured error responses
- Comprehensive logging

### Performance Optimizations
- Efficient board state calculations
- Async/await patterns
- Caching strategies

### Testing Strategy
- Unit tests with xUnit
- Integration tests for API endpoints
- High code coverage target (>80%)

## Assumptions & Constraints

1. Board Size:
   - Maximum board size: 100x100
   - Minimum board size: 3x3

2. Performance:
   - Maximum steps calculation: 1000
   - Timeout for final state: 30 seconds

3. State Management:
   - States persist for 24 hours
   - Maximum concurrent boards: 1000

## Production Readiness

### Monitoring & Logging
- Structured logging with Serilog
- Performance metrics
- Health check endpoints

### Security
- Input validation
- Rate limiting
- No sensitive data exposure

### Scalability
- Horizontally scalable design
- Stateless API architecture
- Distributed caching support

## Future Improvements

1. Authentication/Authorization
2. Distributed cache implementation
3. Real-time updates via SignalR
4. Performance optimizations for large boards
5. Additional board patterns and presets

## License

MIT
````
