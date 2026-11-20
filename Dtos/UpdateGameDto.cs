using System.ComponentModel.DataAnnotations;

namespace Gamestore.Api.Dtos;

public record UpdateGameDto(
   [Required][StringLength(50)] string Name,
   [Range(0, 60)] int GenreId,
   [Range(0,100)] decimal Price,
    DateOnly RelaseDate
);

 