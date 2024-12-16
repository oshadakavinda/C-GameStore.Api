// Import the namespace that contains the GameDto record class
using GameStore.Api.Dtos;

// Create a web application builder (entry point for configuring the app)
var builder = WebApplication.CreateBuilder(args);

// Build the application using the configured builder
var app = builder.Build();
const string GetGameEndpointName = "GetGame";
// Initialize a list of GameDto objects with sample game data
List<GameDto> games = [
    // Each GameDto object represents a game with Id, Name, Genre, Price, and ReleaseDate
    new (1, "Elden Ring", "Action RPG", 59.99m, new DateOnly(2022, 2, 25)),
    new (2, "The Legend of Zelda", "Adventure", 49.99m, new DateOnly(2017, 3, 3)),
    new (3, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
];

// Define a GET endpoint at "games" that returns the list of games
app.MapGet("games", () => games);

app.MapGet("games/{id}", (int id) => games.Find(game =>game.Id == id))
.WithName(GetGameEndpointName);

app.MapPost("games",(CreateGameDto newGame)=>{
    GameDto game = new(
        games.Count+1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName,new {id = game.Id},game);
});
// Start the web application and listen for incoming requests
app.Run();
