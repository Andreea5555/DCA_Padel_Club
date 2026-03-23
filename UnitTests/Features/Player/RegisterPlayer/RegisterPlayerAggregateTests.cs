using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Xunit;

namespace UnitTests.Features.Player.RegisterPlayer;

public class RegisterPlayerAggregateTests
{
    private const string ValidEmail = "john@via.dk";
    private const string ValidFirstName = "John";
    private const string ValidLastName = "Doe";
    private const string ValidPassword = "SecurePassword123";
    private const string ValidProfilePictureUri = "https://example.com/profile.jpg";
    private readonly ViaId ValidId = new ViaId(1);

    // ===== HAPPY PATH =====

    [Fact]
    public void Register_WithValidData_ShouldSucceed()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
    }

    [Fact]
    public void Register_WithValidData_ShouldCreatePlayerWithCorrectInitialState()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsSuccess);
        var player = result.value;
        
        Assert.Equal("John", player.FirstName.Value);
        Assert.Equal("Doe", player.LastName.Value);
        Assert.Equal("john@via.dk", player.Email.Value);
        Assert.NotNull(player.Password);
        Assert.NotNull(player.ProfilePicture);
        Assert.False(player.IsVip);
        Assert.False(player.Blacklisted);
    }

    [Fact]
    public void Register_ShouldFormatNamesToTitleCase()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            "jOHN",
            "dOE",
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.value.FirstName.Value);
        Assert.Equal("Doe", result.value.LastName.Value);
    }

    [Fact]
    public void Register_ShouldNormalizeEmailToLowercase()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            "JOHN@VIA.DK",
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsSuccess);
        Assert.Equal("john@via.dk", result.value.Email.Value);
    }

    // ===== FAILURE SCENARIOS =====

    // F1: Wrong Domain
    [Fact]
    public void Register_WithGmailDomain_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            "john@gmail.com",
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.WrongDomain");
    }

    // F2: Bad Email Format
    [Fact]
    public void Register_WithMissingAtSymbol_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            "johnvia.dk",
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.InvalidFormat");
    }

    [Fact]
    public void Register_WithInvalidEmailPrefix_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            "ab@via.dk", // Prefix too short
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.InvalidPrefix");
    }

    // F3: Empty Email
    [Fact]
    public void Register_WithEmptyEmail_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            "",
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Email.Empty");
    }

    // F4: Invalid Profile Picture URI
    [Fact]
    public void Register_WithEmptyProfilePictureUri_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            ValidEmail,
            ValidPassword,
            ""
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "ProfilePicture.InvalidUri");
    }

    [Fact]
    public void Register_WithInvalidProfilePictureUri_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            ValidEmail,
            ValidPassword,
            "not a valid url"
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "ProfilePicture.InvalidUri");
    }

    // F5: Invalid First Name
    [Fact]
    public void Register_WithInvalidFirstName_Numbers_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            "John123",
            ValidLastName,
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidCharacters");
    }

    [Fact]
    public void Register_WithTooShortFirstName_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            "J",
            ValidLastName,
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidLength");
    }

    [Fact]
    public void Register_WithTooLongFirstName_ShouldFail()
    {
        var longName = "Abcdefghijklmnopqrstuvwxyz"; // 26 characters
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            longName,
            ValidLastName,
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidLength");
    }

    // F6: Invalid Last Name
    [Fact]
    public void Register_WithInvalidLastName_Symbols_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            "Doe-Smith",
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidCharacters");
    }

    [Fact]
    public void Register_WithTooShortLastName_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            "D",
            ValidEmail,
            ValidPassword,
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Name.InvalidLength");
    }

    // Invalid Password
    [Fact]
    public void Register_WithEmptyPassword_ShouldFail()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            ValidFirstName,
            ValidLastName,
            ValidEmail,
            "",
            ValidProfilePictureUri
        );

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Password.Empty");
    }

    // ===== MULTIPLE VALIDATION ERRORS =====

    [Fact]
    public void Register_WithMultipleInvalidFields_ShouldFailWithAllErrors()
    {
        var result = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player.Register(
            ValidId,
            "J", // Too short
            "D", // Too short
            "invalid", // Wrong domain
            "", // Empty password
            "" // Empty URI
        );

        Assert.True(result.IsFailure);
        Assert.True(result.errorMessages.Count >= 4); // At least 4 errors
    }
}

