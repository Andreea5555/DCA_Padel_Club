using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Xunit;

namespace UnitTests.Features.Player.RegisterPlayer;

public class RegisterPlayerAggregateTests
{
    private DCA_Padel_Club.Core.Domain.Aggregates.Players.Player CreateTestPlayer()
    {
        var id = new ViaId(1);
        var email = Email.Create("test@via.dk"); 
        var passwordResult = Password.Create("ValidPassword123!");
        var password = passwordResult.value; 
        
        return DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(id, "John", "Doe", email, password);
    }

    [Fact]
    public void Register_WithValidData_ShouldCreatePlayerWithCorrectInitialState()
    {
        var player = CreateTestPlayer();

        Assert.Equal("John", player.FirstName);
        Assert.Equal("Doe", player.LastName);
        Assert.False(player.IsVip); 
        Assert.False(player.Blacklisted); 
    }

    [Fact]
    public void Register_WithEmptyPassword_ShouldReturnFailure()
    {
        var id = new ViaId(1);
        var email = Email.Create("test@test.com");
        
        var passwordResult = Password.Create(""); 

        Assert.True(passwordResult.IsFailure); 
    }
}