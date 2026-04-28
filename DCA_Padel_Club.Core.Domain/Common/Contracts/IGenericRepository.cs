namespace DCA_Padel_Club.Core.Domain.Common.Contracts;

public interface IGenericRepository<AggregateType, IdType>
{
    Task<AggregateType> GetAsync(IdType id);
    Task AddAsync(AggregateType aggregate);
    Task RemoveAsync(IdType id);
}