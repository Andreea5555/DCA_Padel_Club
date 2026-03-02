

namespace DCA_Padel_Club.Core.Tools.OperationResult;

public class Result<T>
{
    public T value { get; }
    public IReadOnlyList<OperationError> errorMessages { get; }
    public bool IsFailure { get; } 

    private Result(T value, bool IsFailure, IReadOnlyList<OperationError> errorMessages)
    {
        this.value = value;
        this.IsFailure = IsFailure;
        this.errorMessages = errorMessages;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, false, new List<OperationError>());
    }

    public static Result<T> Failure(List<OperationError> errors)
    {
        //default! means it takes the default value, while also accepting null as value 
        return new Result<T>(default!, true,errors);
    }
}
