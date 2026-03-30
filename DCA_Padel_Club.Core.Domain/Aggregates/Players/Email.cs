using System.Text.RegularExpressions;
using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Players;

public class  Email : ValueObject
{
    private const string Domain = "@via.dk";
    private const int MinPrefixLength = 3;
    private const int MaxPrefixLength = 6;
    
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Email.Empty", "Email cannot be empty.") 
            };
            return Result<Email>.Failure(errors);
        }

        email = email.ToLower();

        if (!email.Contains("@"))
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Email.InvalidFormat", "Email format is invalid. Must contain @.") 
            };
            return Result<Email>.Failure(errors);
        }

        if (!email.EndsWith(Domain))
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Email.WrongDomain", "Only people with a VIA mail can register.") 
            };
            return Result<Email>.Failure(errors);
        }

        var prefix = email.Substring(0, email.IndexOf("@"));

        if (prefix.Length != 3 && prefix.Length != 4 && prefix.Length != 6)
        {
            var errors = new List<OperationError> 
            { 
                OperationError.Create("Email.InvalidPrefix", "Email prefix must be 3, 4, or 6 characters long.") 
            };
            return Result<Email>.Failure(errors);
        }

        if (prefix.Length == 3 || prefix.Length == 4)
        {
            if (!Regex.IsMatch(prefix, "^[a-z]+$"))
            {
                var errors = new List<OperationError> 
                { 
                    OperationError.Create("Email.InvalidPrefix", "Email prefix (3-4 chars) must contain only letters.") 
                };
                return Result<Email>.Failure(errors);
            }
        }
        else if (prefix.Length == 6)
        {
            if (!Regex.IsMatch(prefix, "^[0-9]+$"))
            {
                var errors = new List<OperationError> 
                { 
                    OperationError.Create("Email.InvalidPrefix", "Email prefix (6 chars) must contain only digits.") 
                };
                return Result<Email>.Failure(errors);
            }
        }

        return Result<Email>.Success(new Email(email));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}