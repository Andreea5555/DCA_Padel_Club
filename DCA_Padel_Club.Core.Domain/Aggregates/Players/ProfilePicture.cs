using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Players;

public class ProfilePicture : ValueObject
{
    public string Value { get; }

    private ProfilePicture(string value) => Value = value;

    public static Result<ProfilePicture> Create(string uri)
    {
        var validationResult = Validate(uri);
        if (validationResult.IsFailure)
        {
            return Result<ProfilePicture>.Failure(validationResult.errorMessages.ToList());
        }

        return Result<ProfilePicture>.Success(new ProfilePicture(uri));
    }

    private static Result<ProfilePicture> Validate(string uri)
    {
        var errors = new List<OperationError>();

        if (string.IsNullOrWhiteSpace(uri))
        {
            errors.Add(OperationError.Create("ProfilePicture.InvalidUri", "URL has incorrect format."));
            return Result<ProfilePicture>.Failure(errors);
        }

        if (!Uri.TryCreate(uri, UriKind.Absolute, out var _))
        {
            errors.Add(OperationError.Create("ProfilePicture.InvalidUri", "URL has incorrect format."));
            return Result<ProfilePicture>.Failure(errors);
        }

        return Result<ProfilePicture>.Success(new ProfilePicture(uri));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}


