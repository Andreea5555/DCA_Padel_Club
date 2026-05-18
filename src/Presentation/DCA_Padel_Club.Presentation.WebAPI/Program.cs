using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.PlayerFeatures;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Dispatching;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Repositories.Player;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Repositories.Schedule;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;
using DCA_Padel_Club.Presentation.WebAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

builder.Services.AddScoped<ICurrentDate, SystemCurrentDate>();
builder.Services.AddScoped<ICurrentTime, SystemCurrentTime>();
builder.Services.AddScoped<IActiveScheduleOnDate, SystemActiveScheduleOnDate>();

builder.Services.AddScoped<ICommandDispatcher, Dispatcher>();
builder.Services.AddScoped<ICommandHandler<ActivateScheduleCommand>, ActivateScheduleHandler>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepositoryEfc>();
builder.Services.AddScoped<IUnitOfWork, EfcUnitOfWork>();

builder.Services.AddScoped<ICommandHandler<AddCourtCommand>, AddCourtHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateScheduleDateCommand>, UpdateScheduleDateHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateScheduleTimeCommand>, UpdateScheduleTimeHandler>();
builder.Services.AddScoped<ICommandHandler<RemoveCourtCommand>, RemoveCourtHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteScheduleCommand>, DeleteScheduleHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBookingCommand>, CreateBookingHandler>();
builder.Services.AddScoped<ICommandHandler<CreateScheduleCommand>, CreateScheduleHandler>();

builder.Services.AddScoped<ICommandHandler<CreatePlayerCommand>, CreatePlayerHandler>();
builder.Services.AddScoped<ICommandHandler<ManagePlayerStatusCommand>, ManagePlayerStatusHandler>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepositoryEfc>();

builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped<
    IQueryHandler<ScheduleOverview.Query, ScheduleOverview.Answer>,
    ScheduleOverviewHandler>();
builder.Services.AddScoped<
    IQueryHandler<ManagerScheduleOverview.Query, ManagerScheduleOverview.Answer>,
    ManagerScheduleOverviewHandler>();
builder.Services.AddScoped<
    IQueryHandler<DailyScheduleBookingOverview.Query, DailyScheduleBookingOverview.Answer>,
    DailyScheduleBookingOverviewHandler>();
builder.Services.AddScoped<
    IQueryHandler<BookingDetailsOverview.Query, BookingDetailsOverview.Answer>,
    BookingDetailsOverviewHandler>();

builder.Services.AddScoped<
    IQueryHandler<PlayerOverview.Query, PlayerOverview.Answer>,
    PlayerOverviewHandler>();
builder.Services.AddScoped<
    IQueryHandler<PlayerPage.Query, PlayerPage.Answer>,
    PlayerPageHandler>();

string connectionString = "Data Source=VIAPadelClub.db";

builder.Services.AddDbContext<EfcDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDbContext<ViapadelClubContext>(options =>
    options.UseSqlite(connectionString));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Ensure database schema is applied on startup (will create SQLite file and tables if missing)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var efc = services.GetService<EfcDbContext>();
        efc?.Database.Migrate();

        var read = services.GetService<ViapadelClubContext>();
        read?.Database.Migrate();
    }
    catch (Exception ex)
    {
        // If migration fails, rethrow so the host fails fast and logs the error
        Console.WriteLine($"Database migration failed: {ex}");
        throw;
    }
}

app.Run();

public partial class Program { }