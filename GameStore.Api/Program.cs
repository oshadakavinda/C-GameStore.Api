// Import the namespace that contains the GameDto record class
using GameStore.Api.Data;
using GameStore.Api.Endpoints;

// Create a web application builder (entry point for configuring the app)
var builder = WebApplication.CreateBuilder(args);

var connString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(connString);
// Build the application using the configured builder
var app = builder.Build();

app.MapGamesEndpoints();
// Start the web application and listen for incoming requests
app.Run();
