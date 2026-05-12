using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace UnitTests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public int SaveChangesCallCount { get; private set; }

    public Task SaveChangesAsync()
    {
        SaveChangesCallCount++;
        return Task.CompletedTask;
    }
}