using Gamestore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<GameDto> games = [
    new(1, "Total footbal", "Football", 19.99M, new DateOnly(2000, 7, 15)),
    new(2, "FIFA 25", "Sports", 59.99M, new DateOnly(2024, 9, 27)),
    new(3, "Minecraft", "Adventure", 29.99M, new DateOnly(2011, 11, 18)),
    new(4, "Grand Theft Auto V", "Action", 39.99M, new DateOnly(2013, 9, 17)),
    new(5, "The Witcher 3", "RPG", 49.99M, new DateOnly(2015, 5, 19)),
    new(6, "Call of Duty", "Shooter", 69.99M, new DateOnly(2022, 10, 28)),
    new(7, "Forza Horizon 5", "Racing", 59.99M, new DateOnly(2021, 11, 9)),
    new(8, "Red Dead Redemption 2", "Adventure", 59.99M, new DateOnly(2018, 10, 26))
];

//GEt /games
app.MapGet("/games", () => games);


//Get /games/1
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id) )
.WithName("GetGame");


//Post /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.RelaseDate
    );

   games.Add(game);

   return Results.CreatedAtRoute("GetGame", new {id = game.Id}, game );

});

app.Run();
