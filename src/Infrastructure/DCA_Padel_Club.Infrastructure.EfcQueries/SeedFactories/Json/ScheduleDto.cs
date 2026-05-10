namespace DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories.Json;

public record ScheduleDto(
    string Id,
    string Date,
    string CurrentState,
    string TimesStart,
    string TimesEnd
);
