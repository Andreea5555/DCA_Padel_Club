using System.Runtime.InteropServices.JavaScript;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Repositories.Schedule;
using IntegrationTests.Fakes;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.ScheduleTests;

public class ScheduleRepositoryTests
{
    private static EfcDbContext CreateContext(SqliteConnection connection)
    {
        DbContextOptions<EfcDbContext> options = new DbContextOptionsBuilder<EfcDbContext>()
            .UseSqlite(connection)
            .Options;

        EfcDbContext context = new(options);
        context.Database.EnsureCreated();
        return context;
    }
    
    [Fact]
    public async Task AddAsync_ThenGetAsync_BasicSchedule_ReturnsSchedule()
    {
        await using SqliteConnection connection = new("DataSource=:memory:");
        await connection.OpenAsync();

        await using EfcDbContext context = CreateContext(connection);
        ScheduleRepositoryEfc repository = new(context);
        EfcUnitOfWork unitOfWork = new(context);

        Schedule schedule = Schedule.Create();

        await repository.AddAsync(schedule);
        await unitOfWork.SaveChangesAsync();

        context.ChangeTracker.Clear();

        Schedule loaded = await repository.GetAsync(schedule.Id);

        Assert.NotNull(loaded);
        Assert.Equal(schedule.Id, loaded.Id);
    }

    [Fact]
    public async Task AddAsync_ThenGetAsync_WithCourts_ReturnsSchedule()
    {
        await using SqliteConnection connection = new("DataSource=:memory:");
        await connection.OpenAsync();

        await using EfcDbContext context = CreateContext(connection);
        ScheduleRepositoryEfc repository = new(context);
        EfcUnitOfWork unitOfWork = new(context);
        
        Schedule schedule = Schedule.Create();

        var currentDate =new FakeCurrentDate();
        
        schedule.AddCourt(new CourtId("S1"),currentDate);
        schedule.AddCourt(new CourtId("D1"),currentDate);

        await repository.AddAsync(schedule);
        await unitOfWork.SaveChangesAsync();

        context.ChangeTracker.Clear();

        Schedule loaded = await repository.GetAsync(schedule.Id);

        Assert.NotNull(loaded);

        Assert.Equal(schedule.Courts.Count, loaded.Courts.Count);
    }
    //TODO: add one more bookingTest here
    
}