using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;

public class EfcUnitOfWork: IUnitOfWork
{
    private readonly EfcDbContext _context;

    public EfcUnitOfWork(EfcDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}