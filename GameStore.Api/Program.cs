using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    
    new (1, "Elden Ring", "Action RPG", 59.99m, new DateOnly(2022, 2, 25)),
    new (2, "The Legend of Zelda", "Adventure", 49.99m, new DateOnly(2017, 3, 3)),
    new (3, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))

];

app.MapGet("/", () => "Hello World!");

app.Run();
