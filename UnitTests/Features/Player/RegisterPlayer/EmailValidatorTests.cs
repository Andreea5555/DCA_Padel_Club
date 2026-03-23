using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Xunit;

namespace UnitTests.Features.Player.RegisterPlayer;

public class EmailValidatorTests
{
    // ===== HAPPY PATH SCENARIOS =====
    
    [Theory]
    [InlineData("abc@via.dk")]
    [InlineData("abcd@via.dk")]
    [InlineData("123456@via.dk")]
    [InlineData("test@via.dk")]
    [InlineData("ABC@via.dk")] // Should be normalized to lowercase
    public void Create_WithValidEmail_ShouldSucceed(string email)
    {
        var result = Email.Create(email);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(email.ToLower(), result.value.Value);
    }

    [Fact]
    public void Create_ShouldNormalizeToLowercase()
    {
        var result = Email.Create("ABCD@VIA.DK");
        
        Assert.True(result.IsSuccess);
        Assert.Equal("abcd@via.dk", result.value.Value);
    }

    // ===== FAILURE SCENARIOS =====

    // F3: Empty Email
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyEmail_ShouldFail(string email)
    {
        var result = Email.Create(email);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.Empty");
    }

    // F1: Wrong Domain
    [Theory]
    [InlineData("abc@gmail.com")]
    [InlineData("abc@via.com")]
    [InlineData("abc@hotmail.com")]
    [InlineData("abc@via.se")]
    public void Create_WithWrongDomain_ShouldFail(string email)
    {
        var result = Email.Create(email);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.WrongDomain");
    }

    // F2: Bad Format - Missing @
    [Fact]
    public void Create_WithoutAtSymbol_ShouldFail()
    {
        var result = Email.Create("abcvia.dk");
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.InvalidFormat");
    }

    // F2: Bad Format - Invalid prefix length
    [Theory]
    [InlineData("ab@via.dk")] // 2 characters - too short
    [InlineData("abcde@via.dk")] // 5 characters - invalid
    [InlineData("abcdefg@via.dk")] // 7 characters - too long
    public void Create_WithInvalidPrefixLength_ShouldFail(string email)
    {
        var result = Email.Create(email);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.InvalidPrefix");
    }

    // F2: Bad Format - 3-4 chars with non-letters
    [Theory]
    [InlineData("ab1@via.dk")] // Contains digit
    [InlineData("a@2@via.dk")] // Contains special char
    [InlineData("123@via.dk")] // All digits (3 chars should be letters)
    public void Create_With3Or4CharsNonLetters_ShouldFail(string email)
    {
        var result = Email.Create(email);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.InvalidPrefix");
    }

    // F2: Bad Format - 6 chars with non-digits
    [Theory]
    [InlineData("abcdef@via.dk")] // All letters (6 chars should be digits)
    [InlineData("12345a@via.dk")] // Contains letter
    [InlineData("123@45@via.dk")] // Contains special char
    public void Create_With6CharsNonDigits_ShouldFail(string email)
    {
        var result = Email.Create(email);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.InvalidPrefix");
    }
}


