using System;
using Gamestore.Api.Dtos;

namespace Gamestore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

private static readonly List<Dtos.GameDto> games = [
    new(1, "Total footbal", "Football", 19.99M, new DateOnly(2000, 7, 15)),
    new(2, "FIFA 25", "Sports", 59.99M, new DateOnly(2024, 9, 27)),
    new(3, "Minecraft", "Adventure", 29.99M, new DateOnly(2011, 11, 18)),
    new(4, "Grand Theft Auto V", "Action", 39.99M, new DateOnly(2013, 9, 17)),
    new(5, "The Witcher 3", "RPG", 49.99M, new DateOnly(2015, 5, 19)),
    new(6, "Call of Duty", "Shooter", 69.99M, new DateOnly(2022, 10, 28)),
    new(7, "Forza Horizon 5", "Racing", 59.99M, new DateOnly(2021, 11, 9)),
    new(8, "Red Dead Redemption 2", "Adventure", 59.99M, new DateOnly(2018, 10, 26))
];

// extension method
public static void MapGamesEndpoints(this WebApplication app)
    {

 var group = app.MapGroup("/games");
        
//GEt /games
group.MapGet("/", () => games);




//Get /games/1
group.MapGet("/{id}", (int id) =>
{
     var game = games.Find(game => game.Id == id);

    return game is null? Results.NotFound() : Results.Ok(game);
} )
   .WithName(GetGameEndpointName);


//Post /games
group.MapPost("/", (CreateGameDto newGame) =>
{
    //adding validation

    // if (string.IsNullOrEmpty(newGame.Name))
    // {
    //     return Results.BadRequest("Name is required");
    // }

    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.RelaseDate
    );

   games.Add(game);

   return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game );

});

//Update /games/id
group.MapPut("/{id}", (int id, UpdateGameDto dto)=>
{
   var index = games.FindIndex(game => game.Id == id);

if(index == -1)
    {
        return Results.NotFound();
    }

   games[index] = new GameDto(id, dto.Name, dto.Genre, dto.Price, dto.RelaseDate);

   return Results.NoContent();

});

//Delete /games.id
group.MapDelete("/{id}", (int id)=>
{
    // var index = games.FindIndex(game => game.Id == id);

    // games.Remove(games[index]);

    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});
    }

}
