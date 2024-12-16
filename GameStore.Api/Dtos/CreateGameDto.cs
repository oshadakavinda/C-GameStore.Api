namespace GameStore.Api.Dtos;

public record class CreateGameDto(
    //int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
