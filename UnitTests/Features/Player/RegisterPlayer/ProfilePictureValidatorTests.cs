using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Xunit;

namespace UnitTests.Features.Player.RegisterPlayer;

public class ProfilePictureValidatorTests
{
    // ===== HAPPY PATH SCENARIOS =====

    [Theory]
    [InlineData("https://example.com/image.jpg")]
    [InlineData("http://example.com/profile.png")]
    [InlineData("https://cdn.example.com/path/to/image.jpg")]
    [InlineData("https://example.com/image?size=large")]
    public void Create_WithValidUri_ShouldSucceed(string uri)
    {
        var result = ProfilePicture.Create(uri);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(uri, result.value.Value);
    }

    // ===== FAILURE SCENARIOS =====

    // F4: Invalid URI - null or empty
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyUri_ShouldFail(string uri)
    {
        var result = ProfilePicture.Create(uri);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "ProfilePicture.InvalidUri");
        Assert.Contains(result.errorMessages, e => e.ErrorMessage == "URL has incorrect format.");
    }

    // F4: Invalid URI format
    [Theory]
    [InlineData("not a url")]
    [InlineData("example.com/image.jpg")] // Missing protocol
    [InlineData("http://")] // Incomplete
    public void Create_WithInvalidUriFormat_ShouldFail(string uri)
    {
        var result = ProfilePicture.Create(uri);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "ProfilePicture.InvalidUri");
    }
}



