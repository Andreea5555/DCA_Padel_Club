namespace DCA_Padel_Club.Core.Tools.OperationResult;

public class OperationError
{
    public int errorCode { get; }
    public string errorMessage { get; }

    private OperationError(int errorCode, string errorMessage)
    {
        this.errorCode = errorCode;
        this.errorMessage = errorMessage;
    }
    
}