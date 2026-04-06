using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace UnitTests.Helpers;

public class FakeUnitOfWork: IUnitOfWork
{
    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}