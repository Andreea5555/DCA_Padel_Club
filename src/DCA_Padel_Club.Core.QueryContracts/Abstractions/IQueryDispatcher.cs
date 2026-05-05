namespace DCA_Padel_Club.Core.QueryContracts.Abstractions;

public interface IQueryDispatcher
{
    Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
}