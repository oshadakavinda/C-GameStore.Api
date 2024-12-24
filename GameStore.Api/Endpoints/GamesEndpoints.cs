using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
// Initialize a list of GameDto objects with sample game data
private static readonly List<GameDto> games = [
    // Each GameDto object represents a game with Id, Name, Genre, Price, and ReleaseDate
    new (1, "Elden Ring", "Action RPG", 59.99m, new DateOnly(2022, 2, 25)),
    new (2, "The Legend of Zelda", "Adventure", 49.99m, new DateOnly(2017, 3, 3)),
    new (3, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
];

public static RouteGroupBuilder MapGamesEndpoints (this WebApplication app)
{
    var group = app.MapGroup("games");
    // Define a GET endpoint at "games" that returns the list of games
group.MapGet("/", () => games);

//GET /games
group.MapGet("/{id}", (int id) => 
{
    GameDto? game = games.Find(game =>game.Id == id);

    return game is null? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

group.MapPost("/",(CreateGameDto newGame,GameStoreContext dbContext)=>{

    Game game = newGame.ToEntity();
    game.Genre = dbContext.Genres.Find(newGame.GenreId);

    

    dbContext.Games.Add(game);
    dbContext.SaveChanges();    

    

    return Results.CreatedAtRoute(
        GetGameEndpointName,new {id = game.Id},game.ToDto());
}).WithParameterValidation()
;

//PUT /games
group.MapPut("/{id}",(int id, UpdateGameDto updatedGame) =>{
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }
    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

//Delete /games/1
group.MapDelete("/{id}",(int id)=>{
    games.RemoveAll(game =>game.Id ==id);

    return Results.NoContent();
});
return group; 
}
    
}
