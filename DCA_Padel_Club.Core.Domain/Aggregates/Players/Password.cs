using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Players;

public class Password : ValueObject
{
    public string Value { get; private set; }

    private Password(string value) => Value = value;

    public static Result<Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Password.Empty", "Password cannot be empty.") 
            };
            return Result<Password>.Failure(errors);
        }
    
        return Result<Password>.Success(new Password(password));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}