using System.ComponentModel.DataAnnotations;

namespace Gamestore.Api.Dtos;

public record CreateGameDto(
   [Required][StringLength(50)] string Name,
   [Required][StringLength(20)] string Genre,
   [Range(0,100)] decimal Price,
    DateOnly RelaseDate
);
