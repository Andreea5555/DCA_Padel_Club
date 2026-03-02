using System;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Xunit;

namespace UnitTests.Features.Player.ManagePlayerStatus;

public class ManagePlayerStatusAggregateTests
{
    private DCA_Padel_Club.Core.Domain.Aggregates.Players.Player CreateTestPlayer()
    {
        var id = new ViaId(2);
        var email = Email.Create("status@via.dk"); 
        var passwordResult = Password.Create("ValidPassword123!");
        var password = passwordResult.value; 
        
        return DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(id, "Jane", "Doe", email, password);
    }

    [Fact]
    public void BlackListPlayer_ShouldSetBlacklistedToTrue()
    {
        var player = CreateTestPlayer();

        player.BlackListPlayer();

        Assert.True(player.Blacklisted);
    }

    [Fact]
    public void UnblacklistPlayer_FromBlacklistedState_ShouldSetBlacklistedToFalse()
    {
        var player = CreateTestPlayer();
        player.BlackListPlayer(); 

        player.UnblacklistPlayer();

        Assert.False(player.Blacklisted);
    }

    [Fact]
    public void QuarantinePlayer_ShouldSetQuarantineEndDateCorrectly()
    {
        var player = CreateTestPlayer();
        int quarantineDays = 7;

        player.QuarantinePlayer(quarantineDays);

        Assert.NotNull(player.QuarantineEndDate);
        var expectedDate = DateTime.Now.AddDays(quarantineDays).Date;
        Assert.Equal(expectedDate, player.QuarantineEndDate.Value.Date);
    }
    
    [Fact]
    public void RenewVip_ShouldSetIsVipToTrue()
    {
        var player = CreateTestPlayer();

        player.RenewVip(12);

        Assert.True(player.IsVip);
    }

    [Fact]
    public void RevokeVip_ShouldSetIsVipToFalse()
    {
        var player = CreateTestPlayer();
        player.RenewVip(12); 

        player.RevokeVip();

        Assert.False(player.IsVip);
    }

    [Fact]
    public void IsEligibleForVipCourt_WhenVipAndNotBlacklisted_ReturnsTrue()
    {
        var player = CreateTestPlayer();
        player.RenewVip(12);
        
        player.UnblacklistPlayer(); 

        var isEligible = player.IsEligibleForVipCourt();

        Assert.True(isEligible);
    }
    
    [Fact]
    public void IsEligibleToBook_WhenPlayerInGoodStanding_ReturnsTrue()
    {
        var player = CreateTestPlayer();
        // Ensure clean state
        player.UnblacklistPlayer();

        var isEligible = player.IsEligibleToBook();

        Assert.True(isEligible);
    }

    [Fact]
    public void IsEligibleToBook_WhenPlayerIsBlacklisted_ReturnsFalse()
    {
        var player = CreateTestPlayer();
        player.BlackListPlayer();

        var isEligible = player.IsEligibleToBook();

        Assert.False(isEligible);
    }

    [Fact]
    public void IsEligibleToBook_WhenPlayerInQuarantine_ReturnsFalse()
    {
        var player = CreateTestPlayer();
        player.QuarantinePlayer(5); 

        var isEligible = player.IsEligibleToBook();

        Assert.False(isEligible);
    }
}

