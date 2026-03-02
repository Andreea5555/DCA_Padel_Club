namespace DCA_Padel_Club.Core.Tools.OperationResult;

public class OperationError
{
    public string ErrorCode { get; } 
    public string ErrorMessage { get; }

    public OperationError(string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public static OperationError Create(string code, string message)
    {
        return new OperationError(code, message);
    }
}