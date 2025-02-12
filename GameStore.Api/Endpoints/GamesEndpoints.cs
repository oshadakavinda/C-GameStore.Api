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

public static RouteGroupBuilder MapGamesEndpoints (this WebApplication app)
{
    var group = app.MapGroup("games");

    // GET/gems
    group.MapGet("/", async (GameStoreContext dbContext) => 
             await dbContext.Games
                        .Include(game=>game.Genre)
                        .Select(game => game.ToGameSummaryDto())
                        .AsNoTracking()
                        .ToListAsync()
                     );



//GET /games/id
    group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? 
                Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndpointName);

group.MapPost("/",(CreateGameDto newGame,GameStoreContext dbContext)=>{

    Game game = newGame.ToEntity();
    
    dbContext.Games.Add(game);
    dbContext.SaveChangesAsync();    
    

    return Results.CreatedAtRoute(
        GetGameEndpointName,
        new {id = game.Id},
        game.ToGameDetailsDto());
}).WithParameterValidation()
;

// PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                     .CurrentValues
                     .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


// DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                     .Where(game => game.Id == id)
                     .ExecuteDeleteAsync();

            return Results.NoContent();
        });

return group; 
}
    
}
