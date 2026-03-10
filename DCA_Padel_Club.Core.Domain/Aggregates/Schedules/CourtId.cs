using DCA_Padel_Club.Core.Tools.OperationResult;

public class CourtId
{
    private static int nextNumber = 1;

    public string Value { get; }

    private CourtId(string value)
    {
        Value = value;
    }

    public static Result<CourtId> CreateCourtId(string courtType)
    {
        var errors = new List<OperationError>();

        if (string.IsNullOrWhiteSpace(courtType))
        {
            errors.Add(OperationError.Create(
                "CourtId.EmptyType",
                "Court type cannot be empty."
            ));
        }

        courtType = courtType.ToUpper();

        if (courtType != "S" && courtType != "D")
        {
            errors.Add(OperationError.Create(
                "CourtId.InvalidCourtType",
                "Court type must be S (Single) or D (Double)."
            ));
        }

        int number = nextNumber;

        if (number > 10)
        {
            errors.Add(OperationError.Create(
                "CourtId.NumberExceeded",
                "Maximum number of courts is 10."
            ));
        }

        if (errors.Count > 0)
            return Result<CourtId>.Failure(errors);

        string id = $"{courtType}{number}";
        nextNumber++;

        return Result<CourtId>.Success(new CourtId(id));
    }

    public string GetValue()
    {
        return Value;
    }
}