using DCA_Padel_Club.Core.Domain.Common.Bases;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Players;

public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string email)
    {
        return new Email(email);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}