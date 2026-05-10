using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.EfcQueriesTests.Helpers;

public static class TestDbFactory
{
    public static async Task<ViapadelClubContext> CreateMigratedReadContextAsync()
    {
        var dbPath = Path.Combine(Path.GetTempPath(), $"PadelTest_{Guid.NewGuid():N}.db");
        var connectionString = $"Data Source={dbPath}";

        var writeOptions = new DbContextOptionsBuilder<EfcDbContext>()
            .UseSqlite(connectionString)
            .Options;
        await using (var writeCtx = new EfcDbContext(writeOptions))
        {
            await writeCtx.Database.MigrateAsync();
        }

        var readOptions = new DbContextOptionsBuilder<ViapadelClubContext>()
            .UseSqlite(connectionString)
            .Options;
        return new ViapadelClubContext(readOptions);
    }
}
