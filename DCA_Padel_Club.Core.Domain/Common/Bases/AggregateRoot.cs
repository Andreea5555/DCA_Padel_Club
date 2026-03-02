namespace DCA_Padel_Club.Core.Domain.Common.Bases;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    protected AggregateRoot(TId id) : base(id) { }
}