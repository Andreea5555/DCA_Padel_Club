using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Xunit;

namespace UnitTests.Features.Player.RegisterPlayer;

public class NameValidatorTests
{
    // ===== HAPPY PATH SCENARIOS =====

    [Theory]
    [InlineData("John")]
    [InlineData("Jo")]
    [InlineData("Alexander")]
    [InlineData("Al")]
    [InlineData("SomeName")]
    public void Create_WithValidName_ShouldSucceed(string name)
    {
        var result = Name.Create(name);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
    }

    [Fact]
    public void Create_WithValidName_ShouldFormatToTitleCase()
    {
        var result = Name.Create("jOHN");
        
        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.value.Value);
    }

    [Fact]
    public void Create_WithAllUppercase_ShouldFormatToTitleCase()
    {
        var result = Name.Create("JOHN");
        
        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.value.Value);
    }

    [Fact]
    public void Create_WithAllLowercase_ShouldFormatToTitleCase()
    {
        var result = Name.Create("john");
        
        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.value.Value);
    }

    [Fact]
    public void Create_WithTwoCharacters_ShouldSucceed()
    {
        var result = Name.Create("Jo");
        
        Assert.True(result.IsSuccess);
        Assert.Equal("Jo", result.value.Value);
    }

    [Fact]
    public void Create_With25Characters_ShouldSucceed()
    {
        var name = "Abcdefghijklmnopqrstuvwxy"; // Exactly 25 characters
        var result = Name.Create(name);
        
        Assert.True(result.IsSuccess);
    }

    // ===== FAILURE SCENARIOS =====

    // F5 & F6: Empty Name
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyName_ShouldFail(string name)
    {
        var result = Name.Create(name);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.Empty");
    }

    // F5 & F6: Too short (less than 2 characters)
    [Theory]
    [InlineData("A")]
    [InlineData("")]
    public void Create_WithTooShortName_ShouldFail(string name)
    {
        if (string.IsNullOrEmpty(name)) return; // Already covered by empty test
        
        var result = Name.Create(name);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidLength");
    }

    // F5 & F6: Too long (more than 25 characters)
    [Fact]
    public void Create_WithTooLongName_ShouldFail()
    {
        var longName = "Abcdefghijklmnopqrstuvwxyz"; // 26 characters
        var result = Name.Create(longName);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidLength");
    }

    // F5 & F6: Contains numbers
    [Theory]
    [InlineData("John1")]
    [InlineData("123")]
    [InlineData("John123")]
    public void Create_WithNumbers_ShouldFail(string name)
    {
        var result = Name.Create(name);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidCharacters");
    }

    // F5 & F6: Contains symbols
    [Theory]
    [InlineData("John-Doe")]
    [InlineData("John.Doe")]
    [InlineData("John_Doe")]
    [InlineData("John@Doe")]
    [InlineData("John!")]
    public void Create_WithSymbols_ShouldFail(string name)
    {
        var result = Name.Create(name);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidCharacters");
    }

    // F5 & F6: Contains spaces
    [Theory]
    [InlineData("John Doe")]
    [InlineData("John ")]
    [InlineData(" John")]
    public void Create_WithSpaces_ShouldFail(string name)
    {
        var result = Name.Create(name);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidCharacters");
    }
}



