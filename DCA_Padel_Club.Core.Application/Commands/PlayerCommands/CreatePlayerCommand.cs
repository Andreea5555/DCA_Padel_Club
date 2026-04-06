using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.PlayerCommands;

public class CreatePlayerCommand
{
    public ViaId PlayerId { get; }
    public Name FirstName { get; }
    public Name LastName { get; }
    public Email Email { get; }
    public Password Password { get; }
    public ProfilePicture ProfilePicture { get; }

    private CreatePlayerCommand(
        ViaId playerId,
        Name firstName,
        Name lastName,
        Email email,
        Password password,
        ProfilePicture profilePicture)
    {
        PlayerId = playerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        ProfilePicture = profilePicture;
    }

    public static Result<CreatePlayerCommand> Create(
        int playerId,
        string firstName,
        string lastName,
        string email,
        string password,
        string profilePictureUri)
    {
        var errors = new List<OperationError>();

        var firstNameResult = Name.Create(firstName);
        if (firstNameResult.IsFailure)
        {
            errors.AddRange(firstNameResult.errorMessages);
        }

        var lastNameResult = Name.Create(lastName);
        if (lastNameResult.IsFailure)
        {
            errors.AddRange(lastNameResult.errorMessages);
        }

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            errors.AddRange(emailResult.errorMessages);
        }

        var passwordResult = Password.Create(password);
        if (passwordResult.IsFailure)
        {
            errors.AddRange(passwordResult.errorMessages);
        }

        var profilePictureResult = ProfilePicture.Create(profilePictureUri);
        if (profilePictureResult.IsFailure)
        {
            errors.AddRange(profilePictureResult.errorMessages);
        }

        if (errors.Any())
        {
            return Result<CreatePlayerCommand>.Failure(errors);
        }

        return Result<CreatePlayerCommand>.Success(
            new CreatePlayerCommand(
                new ViaId(playerId),
                firstNameResult.value,
                lastNameResult.value,
                emailResult.value,
                passwordResult.value,
                profilePictureResult.value));
    }
}