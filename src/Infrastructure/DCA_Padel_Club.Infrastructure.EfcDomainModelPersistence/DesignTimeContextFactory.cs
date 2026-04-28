using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<EfcDbContext>
{
    public EfcDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EfcDbContext>();
        optionsBuilder.UseSqlite("Data Source=VIAPadelClub.db");

        return new EfcDbContext(optionsBuilder.Options);
    }
    
}