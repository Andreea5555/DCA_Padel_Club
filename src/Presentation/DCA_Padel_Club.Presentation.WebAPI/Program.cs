using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Dispatching;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Repositories.Schedule;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }