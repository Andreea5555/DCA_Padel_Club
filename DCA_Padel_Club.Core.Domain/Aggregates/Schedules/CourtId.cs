using DCA_Padel_Club.Core.Tools.OperationResult;

public class CourtId
{
    public string Value { get; }

    private CourtId(string value)
    {
        Value = value;
    }
    
    public static Result<CourtId> CreateCourtId(string courtName)
    {
        var errors = new List<OperationError>();

        if (string.IsNullOrWhiteSpace(courtName))
        {
            errors.Add(OperationError.Create("CourtId.Empty", "Court name cannot be empty."));
            return Result<CourtId>.Failure(errors);
        }

        courtName = courtName.Trim().ToUpper();

        if (courtName.Length < 2 || courtName.Length > 3)
        {
            errors.Add(OperationError.Create("CourtId.InvalidLength",
                "Court name must be 2 or 3 characters long (e.g. S1, S10, D1, D10)."));
        }

        char prefix = courtName[0];
        if (prefix != 'S' && prefix != 'D')
        {
            errors.Add(OperationError.Create("CourtId.InvalidPrefix",
                "Court name must start with S (Singles court) or D (Doubles court)."));
        }

        if (!int.TryParse(courtName[1..], out int number) || number < 1 || number > 10)
        {
            errors.Add(OperationError.Create("CourtId.InvalidNumber",
                "Court number must be a digit between 1 and 10."));
        }

        if (errors.Count > 0)
            return Result<CourtId>.Failure(errors);

        return Result<CourtId>.Success(new CourtId(courtName));
    }

    public string GetValue()
    {
        return Value;
    }
}