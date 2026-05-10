namespace DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories.Json;

public record PlayerDto(
    string Id,
    string Email,
    string FirstName,
    int IsBlacklisted,
    string LastName,
    string ProfilePicture,
    string? QuarantinedUntil,
    string? VipUntil
);
