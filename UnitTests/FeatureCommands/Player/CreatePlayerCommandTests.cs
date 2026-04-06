using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;

namespace UnitTests.FeatureCommands.Player;

public class CreatePlayerCommandTests
{
    [Fact]
    public void Create_ValidInput_ReturnsSuccess()
    {
        const int playerId = 123456;
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "abc@via.dk";
        const string password = "secret123";
        const string profilePictureUri = "https://example.com/profile.png";

        var result = CreatePlayerCommand.Create(
            playerId,
            firstName,
            lastName,
            email,
            password,
            profilePictureUri);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(playerId, result.value.PlayerId.Value);
        Assert.Equal("John", result.value.FirstName.Value);
        Assert.Equal("Doe", result.value.LastName.Value);
        Assert.Equal("abc@via.dk", result.value.Email.Value);
        Assert.Equal("secret123", result.value.Password.Value);
        Assert.Equal(profilePictureUri, result.value.ProfilePicture.Value);
    }

    [Fact]
    public void Create_InvalidEmail_ReturnsFailure()
    {
        var result = CreatePlayerCommand.Create(
            123456,
            "John",
            "Doe",
            "abc@gmail.com",
            "secret123",
            "https://example.com/profile.png");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, error => error.ErrorCode == "Email.WrongDomain");
    }
}