using System.Text.RegularExpressions;
using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Players;

public class Name : ValueObject
{
    private const int MinLength = 2;
    private const int MaxLength = 25;
    
    public string Value { get; }

    private Name(string value) => Value = value;

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Name.Empty", "Name cannot be empty.") 
            };
            return Result<Name>.Failure(errors);
        }

        if (name.Length < MinLength || name.Length > MaxLength)
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Name.InvalidLength", $"Name must be between {MinLength} and {MaxLength} characters.") 
            };
            return Result<Name>.Failure(errors);
        }

        if (!Regex.IsMatch(name, "^[a-zA-Z]+$"))
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Name.InvalidCharacters", "Name must contain only letters. No numbers, symbols, or spaces allowed.") 
            };
            return Result<Name>.Failure(errors);
        }

        var formattedName = ToTitleCase(name);
        return Result<Name>.Success(new Name(formattedName));
    }

    private static string ToTitleCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        return char.ToUpper(value[0]) + value.Substring(1).ToLower();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}



