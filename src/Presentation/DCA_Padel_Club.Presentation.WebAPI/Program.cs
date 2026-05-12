using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Repositories.Schedule;

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