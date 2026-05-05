namespace DCA_Padel_Club.Core.QueryContracts.Abstractions;

public interface IQueryHandler<in TQuery, TAnswer>
    where TQuery : IQuery<TAnswer>
{
    Task<TAnswer> HandleAsync(TQuery query);
}