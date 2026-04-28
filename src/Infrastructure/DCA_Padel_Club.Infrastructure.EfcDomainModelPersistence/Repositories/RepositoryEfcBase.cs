using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Repositories;

public abstract class RepositoryEfcBase<AggregateType, IdType> : IGenericRepository<AggregateType, IdType>
    where AggregateType : class
{
        protected readonly EfcDbContext Context;

        protected RepositoryEfcBase(EfcDbContext context)
        {
            Context = context;
        }

        public abstract Task<AggregateType> GetAsync(IdType id);

        public virtual async Task AddAsync(AggregateType aggregate)
        {
            await Context.Set<AggregateType>().AddAsync(aggregate);
        }

        public virtual async Task RemoveAsync(IdType id)
        {
            AggregateType aggregate = await GetAsync(id);
            Context.Set<AggregateType>().Remove(aggregate);
        }
    }