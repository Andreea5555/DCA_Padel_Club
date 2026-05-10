namespace DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories.Json;

public record BookingDto(
    string Id,
    string CourtId,
    string PlayerId,
    string TimeStart,
    string TimeEnd
);
