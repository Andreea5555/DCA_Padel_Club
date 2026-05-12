using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using IntegrationTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IntegrationTests.Fakes.Schedule;

namespace IntegrationTests.WebAPI;

public class DCAWebApplicationFactory: WebApplicationFactory<Program>
{
    private IServiceCollection? _services;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureTestServices(services =>
        {
            _services = services;

            services.RemoveAll(typeof(DbContextOptions<EfcDbContext>));
            services.RemoveAll(typeof(DbContextOptions<ViapadelClubContext>));
            services.AddScoped<ICurrentDate, FakeCurrentDate>();
            services.AddScoped<IActiveScheduleOnDate>(_ =>
                new FakeActiveScheduleOnDate(false));
            services.AddScoped<ICurrentTime>(_ =>
                new FakeCurrentTime(new DateTime(2026, 5, 13, 10, 0, 0)));

            string connectionString = GetConnectionString();

            services.AddDbContext<EfcDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddDbContext<ViapadelClubContext>(options =>
                options.UseSqlite(connectionString));

            SetupCleanDatabase(services);
        });
    }

    private static void SetupCleanDatabase(IServiceCollection services)
    {
        using ServiceProvider provider = services.BuildServiceProvider();

        EfcDbContext dmContext = provider.GetRequiredService<EfcDbContext>();
        dmContext.Database.EnsureDeleted();
        dmContext.Database.EnsureCreated();

        ViapadelClubContext queryContext = provider.GetRequiredService<ViapadelClubContext>();
        queryContext.Database.EnsureDeleted();
        queryContext.Database.EnsureCreated();
    }

    private static string GetConnectionString()
    {
        string testDbName = "Test" + Guid.NewGuid() + ".db";
        return "Data Source=" + testDbName;
    }

    protected override void Dispose(bool disposing)
    {
        if (_services is not null)
        {
            using ServiceProvider provider = _services.BuildServiceProvider();

            provider.GetRequiredService<EfcDbContext>().Database.EnsureDeleted();
            provider.GetRequiredService<EfcDbContext>().Database.EnsureDeleted();
        }

        base.Dispose(disposing);
    }
}