using System;
using Gamestore.Api.Data;
using Gamestore.Api.Dtos;
using Gamestore.Api.Models;
using Microsoft.EntityFrameworkCore;

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
public static async Task MapGamesEndpoints(this WebApplication app)
    {

 var group = app.MapGroup("/games");
        
//GEt /games
group.MapGet("/", async (GameStoreContext dbContext) => {
    var games = await dbContext.Games.ToListAsync();
    return Results.Ok(games);
});




//Get /games/1
group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
{
    //  var game = games.Find(game => game.Id == id);
    var game = await dbContext.Games.FindAsync(id);

    return game is null? Results.NotFound() : Results.Ok(
        new GameDetailsDto(
        game.Id,
        game.Name,
        game.GenreId,
        game.Price,
        game.RelaseDate
    )
    );
} )
   .WithName(GetGameEndpointName);


//Post /games
group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
{
    //adding validation

    // if (string.IsNullOrEmpty(newGame.Name))
    // {
    //     return Results.BadRequest("Name is required");
    // }

    // GameDto game = new(
    //     games.Count + 1,
    //     newGame.Name,
    //     newGame.Genre,
    //     newGame.Price,
    //     newGame.RelaseDate
    // );

// our new instance
 Game game = new()
 {
     Name= newGame.Name,
     GenreId =newGame.GenreId,
     Price=newGame.Price,
     RelaseDate=newGame.RelaseDate
 };
   
//    games.Add(game);
dbContext.Games.Add(game);
await dbContext.SaveChangesAsync();


GameDetailsDto gameDto = new(
    game.Id,
    game.Name,
     game.GenreId,
    game.Price,
    game.RelaseDate
);

   return Results.CreatedAtRoute(GetGameEndpointName, new {id = gameDto.Id}, gameDto );

});

//Update /games/id
group.MapPut("/{id}",async (int id, UpdateGameDto dto, GameStoreContext dbContext)=>
{
//    var index = games.FindIndex(game => game.Id == id);
var game = await dbContext.Games.FindAsync(id);

// if(index == -1)
//     {
//         return Results.NotFound();
//     }
if(game is null)
    {
        return Results.NotFound();
    }

//    games[index] = new GameDto(id, dto.Name, dto.Genre, dto.Price, dto.RelaseDate);

     game.Name=dto.Name;
     game.GenreId=dto.GenreId;
     game.Price=dto.Price;

    await dbContext.SaveChangesAsync();

   return Results.NoContent();

});

//Delete /games.id
group.MapDelete("/{id}", async (int id, GameStoreContext dbContext)=>
{
    // var index = games.FindIndex(game => game.Id == id);

    // games.Remove(games[index]);

    var game = await dbContext.Games.FindAsync(id);
    if( game is null)
    {
        return Results.NotFound();
    } 

    // games.RemoveAll(game => game.Id == id);
    dbContext.Games.Remove(game);
   await dbContext.SaveChangesAsync();

    return Results.NoContent();
});
    }

}
