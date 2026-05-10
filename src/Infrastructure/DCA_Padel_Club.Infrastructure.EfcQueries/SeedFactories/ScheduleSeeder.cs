using System.Text.Json;
using DCA_Padel_Club.Infrastructure.EfcQueries.Models;
using DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories.Json;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories;

public static class ScheduleSeeder
{
    private const string Json = """
[
    { "Id": "c1e2a3f4-b5d6-c7e8-f9a0-b1d2c3e4f5a6", "Date": "2025-04-01", "CurrentState": "Active",  "TimesStart": "10:30:00", "TimesEnd": "15:30:00" },
    { "Id": "d2f3b4a5-c6e7-d8f9-a0b1-c2d3e4f5a6b7", "Date": "2025-04-03", "CurrentState": "Active",  "TimesStart": "15:00:00", "TimesEnd": "22:00:00" },
    { "Id": "e3a4c5b6-d7f8-e9a0-b1c2-d3e4f5a6b7c8", "Date": "2025-04-04", "CurrentState": "Active",  "TimesStart": "11:30:00", "TimesEnd": "19:30:00" },
    { "Id": "f4b5d6c7-e8a9-f0b1-c2d3-e4f5a6b7c8d9", "Date": "2025-04-06", "CurrentState": "Active",  "TimesStart": "13:00:00", "TimesEnd": "22:00:00" },
    { "Id": "a5c6e7d8-f9b0-a1c2-d3e4-f5a6b7c8d9ea", "Date": "2025-04-07", "CurrentState": "Deleted", "TimesStart": "10:00:00", "TimesEnd": "13:00:00" },
    { "Id": "b6d7f8e9-a0c1-b2d3-e4f5-a6b7c8d9eaf0", "Date": "2025-04-09", "CurrentState": "Active",  "TimesStart": "15:00:00", "TimesEnd": "22:00:00" },
    { "Id": "c7e8a9f0-b1d2-c3e4-f5a6-b7c8d9eaf0b1", "Date": "2025-04-10", "CurrentState": "Active",  "TimesStart": "12:30:00", "TimesEnd": "16:30:00" },
    { "Id": "d8f9b0a1-c2e3-d4f5-a6b7-c8d9eaf0b1c2", "Date": "2025-04-12", "CurrentState": "Active",  "TimesStart": "10:00:00", "TimesEnd": "20:00:00" },
    { "Id": "e9a0c1b2-d3f4-e5a6-b7c8-d9eaf0b1c2d3", "Date": "2025-04-14", "CurrentState": "Active",  "TimesStart": "14:00:00", "TimesEnd": "21:00:00" },
    { "Id": "f0b1d2c3-e4a5-f6b7-c8d9-eaf0b1c2d3e4", "Date": "2025-04-15", "CurrentState": "Active",  "TimesStart": "11:00:00", "TimesEnd": "15:00:00" },
    { "Id": "a1c2e3d4-f5b6-a7c8-d9ea-f0b1c2d3e4f5", "Date": "2025-04-17", "CurrentState": "Active",  "TimesStart": "14:30:00", "TimesEnd": "21:30:00" },
    { "Id": "b2d3f4e5-a6c7-b8d9-eaf0-b1c2d3e4f5a6", "Date": "2025-04-18", "CurrentState": "Draft",   "TimesStart": "10:30:00", "TimesEnd": "18:30:00" },
    { "Id": "c3e4a5f6-b7d8-c9ea-f0b1-c2d3e4f5a6b7", "Date": "2025-04-20", "CurrentState": "Active",  "TimesStart": "12:00:00", "TimesEnd": "22:00:00" },
    { "Id": "d4f5b6a7-c8e9-d0f0-b1c2-d3e4f5a6b7c8", "Date": "2025-04-21", "CurrentState": "Draft",   "TimesStart": "14:30:00", "TimesEnd": "19:30:00" },
    { "Id": "e5a6c7b8-d9f0-e1a0-b1c2-d3e4f5a6b7c8", "Date": "2025-04-23", "CurrentState": "Deleted", "TimesStart": "11:00:00", "TimesEnd": "14:00:00" },
    { "Id": "f6b7d8c9-eaf0-f1b0-c2d3-e4f5a6b7c8d9", "Date": "2025-04-24", "CurrentState": "Draft",   "TimesStart": "12:00:00", "TimesEnd": "22:00:00" },
    { "Id": "a7c8e9d0-f0b1-a1c2-d3e4-f5a6b7c8d9ea", "Date": "2025-04-26", "CurrentState": "Draft",   "TimesStart": "13:00:00", "TimesEnd": "18:00:00" },
    { "Id": "b8d9f0e1-a0c1-b2d3-e4f5-a6b7c8d9eaf0", "Date": "2025-04-27", "CurrentState": "Draft",   "TimesStart": "10:00:00", "TimesEnd": "20:00:00" },
    { "Id": "c9ea0fb1-b1d2-c3e4-f5a6-b7c8d9eaf0b1", "Date": "2025-04-29", "CurrentState": "Draft",   "TimesStart": "14:00:00", "TimesEnd": "22:00:00" },
    { "Id": "d0f0b1a1-c2e3-d4f5-a6b7-c8d9eaf0b1c2", "Date": "2025-04-30", "CurrentState": "Draft",   "TimesStart": "12:30:00", "TimesEnd": "15:30:00" }
]
""";

    public static List<Schedule> CreateSchedules()
    {
        var dtos = JsonSerializer.Deserialize<List<ScheduleDto>>(Json)!;
        var schedules = new List<Schedule>(dtos.Count);

        foreach (var dto in dtos)
        {
            var (isDraft, isDeleted) = MapState(dto.CurrentState);
            schedules.Add(new Schedule
            {
                Id = dto.Id,
                Date = dto.Date,
                StartTime = dto.TimesStart,
                EndTime = dto.TimesEnd,
                IsDraft = isDraft,
                IsDeleted = isDeleted
            });
        }

        return schedules;
    }

    private static (int isDraft, int isDeleted) MapState(string currentState) => currentState switch
    {
        "Draft"   => (1, 0),
        "Active"  => (0, 0),
        "Deleted" => (0, 1),
        _ => throw new ArgumentOutOfRangeException(nameof(currentState), currentState, "Unknown schedule state")
    };
}
