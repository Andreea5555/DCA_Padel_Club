using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.WebAPI.PlayerEndpointTests;

internal static class PlayerEndpointTestHelpers
{
    public static async Task<Player> CreateAndSavePlayerAsync(
        WebApplicationFactory<Program> factory,
        int playerId,
        string firstName = "Alice",
        string lastName = "Smith",
        string email = "123456@via.dk",
        string password = "secret123",
        string profilePictureUri = "https://example.com/profile.png")
    {
        var result = Player.Register(
            new ViaId(playerId),
            firstName,
            lastName,
            email,
            password,
            profilePictureUri);

        if (result.IsFailure)
        {
            throw new InvalidOperationException(string.Join(", ", result.errorMessages.Select(e => e.ErrorCode)));
        }

        await SavePlayerAsync(factory, result.value);
        return result.value;
    }

    public static async Task SavePlayerAsync(
        WebApplicationFactory<Program> factory,
        Player player)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        EfcDbContext context = scope.ServiceProvider.GetRequiredService<EfcDbContext>();

        context.Players.Add(player);
        await context.SaveChangesAsync();
    }

    public static Task<Player?> GetPlayerAsync(
        WebApplicationFactory<Program> factory,
        int playerId)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        EfcDbContext context = scope.ServiceProvider.GetRequiredService<EfcDbContext>();

        Player? player = context.Players.AsEnumerable().SingleOrDefault(player => player.Id.Value == playerId);
        return Task.FromResult(player);
    }
}




