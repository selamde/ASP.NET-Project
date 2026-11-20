using Gamestore.Api.Data;
using Gamestore.Api.Endpoints;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddGameStoreDb();


var app = builder.Build();


app.MigrateDb();
app.MapGamesEndpoints();



app.Run();
