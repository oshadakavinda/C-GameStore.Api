// Import the namespace that contains the GameDto record class
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;

// Create a web application builder (entry point for configuring the app)
var builder = WebApplication.CreateBuilder(args);

// Build the application using the configured builder
var app = builder.Build();

app.MapGamesEndpoints();
// Start the web application and listen for incoming requests
app.Run();
