using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = "Data Source=GameStore.db";

builder.Services.AddSqlite<GameStoreContext>(connString);

// Build the application using the configured builder
var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

// Start the web application and listen for incoming requests
app.Run();
