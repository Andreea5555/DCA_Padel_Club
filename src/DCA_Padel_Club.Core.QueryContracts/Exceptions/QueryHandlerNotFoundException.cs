namespace DCA_Padel_Club.Core.QueryContracts.Exceptions;

public class QueryHandlerNotFoundException: Exception
{
    public QueryHandlerNotFoundException(string queryType, string answerType)
        : base($"No query handler found for query '{queryType}' with answer '{answerType}'.")
    {
    }
}