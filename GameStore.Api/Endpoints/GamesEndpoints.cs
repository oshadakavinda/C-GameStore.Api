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
    group.MapGet("/",  (GameStoreContext dbContext) => 
             dbContext.Games
                        .Include(game=>game.Genre)
                        .Select(game => game.ToGameSummaryDto())
                        .AsNoTracking()
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
    dbContext.SaveChanges();    
    

    return Results.CreatedAtRoute(
        GetGameEndpointName,
        new {id = game.Id},
        game.ToGameDetailsDto());
}).WithParameterValidation()
;

//PUT /games
group.MapPut("/{id}",(int id, UpdateGameDto updatedGame ,GameStoreContext dbContext) =>{
   var existingGame =  dbContext.Games.Find(id);
    if (existingGame is null)
            {
                return Results.NotFound();
            }

    dbContext.Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id));

    dbContext.SaveChanges();
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
