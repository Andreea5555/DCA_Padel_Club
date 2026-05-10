using System.Text.Json;
using DCA_Padel_Club.Infrastructure.EfcQueries.Models;
using DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories.Json;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories;

public static class ScheduleCourtSeeder
{
    private const string Json = """
[
    { "Id": "a8f0e1d2-c3b4-a5d6-e7f8-a9b0c1d2e3f4", "Name": "S1", "DailyScheduleId": "c1e2a3f4-b5d6-c7e8-f9a0-b1d2c3e4f5a6" },
    { "Id": "b9a1f2e3-d4c5-b6e7-f8a9-b0c1d2e3f4a5", "Name": "D1", "DailyScheduleId": "c1e2a3f4-b5d6-c7e8-f9a0-b1d2c3e4f5a6" },
    { "Id": "c0b2a3f4-e5d6-c7f8-a9b0-c1d2e3f4a5b6", "Name": "S2", "DailyScheduleId": "d2f3b4a5-c6e7-d8f9-a0b1-c2d3e4f5a6b7" },
    { "Id": "d1c3b4a5-f6e7-d8a9-b0c1-d2e3f4a5b6c7", "Name": "D3", "DailyScheduleId": "d2f3b4a5-c6e7-d8f9-a0b1-c2d3e4f5a6b7" },
    { "Id": "e2d4c5b6-a7f8-e9b0-c1d2-e3f4a5b6c7d8", "Name": "D4", "DailyScheduleId": "d2f3b4a5-c6e7-d8f9-a0b1-c2d3e4f5a6b7" },
    { "Id": "f3e5d6c7-b8a9-f0c1-d2e3-f4a5b6c7d8e9", "Name": "D1", "DailyScheduleId": "e3a4c5b6-d7f8-e9a0-b1c2-d3e4f5a6b7c8" },
    { "Id": "a4f6e7d8-c9b0-a1d2-e3f4-a5b6c7d8e9f0", "Name": "D2", "DailyScheduleId": "e3a4c5b6-d7f8-e9a0-b1c2-d3e4f5a6b7c8" },
    { "Id": "b5a7f8e9-d0c1-b2e3-f4a5-b6c7d8e9f0a1", "Name": "S1", "DailyScheduleId": "f4b5d6c7-e8a9-f0b1-c2d3-e4f5a6b7c8d9" },
    { "Id": "c6b8a9f0-e1d2-c3f4-a5b6-c7d8e9f0a1b2", "Name": "D5", "DailyScheduleId": "f4b5d6c7-e8a9-f0b1-c2d3-e4f5a6b7c8d9" },
    { "Id": "d7c9b0a1-f2e3-d4a5-b6c7-d8e9f0a1b2c3", "Name": "D6", "DailyScheduleId": "f4b5d6c7-e8a9-f0b1-c2d3-e4f5a6b7c8d9" },
    { "Id": "e8d0c1b2-a3f4-e5b6-c7d8-e9f0a1b2c3d4", "Name": "S2", "DailyScheduleId": "f4b5d6c7-e8a9-f0b1-c2d3-e4f5a6b7c8d9" },
    { "Id": "f9e1d2c3-b4a5-f6c7-d8e9-f0a1b2c3d4e5", "Name": "D3", "DailyScheduleId": "a5c6e7d8-f9b0-a1c2-d3e4-f5a6b7c8d9ea" },
    { "Id": "a0f2e3d4-c5b6-a7d8-e9f0-a1b2c3d4e5f6", "Name": "D4", "DailyScheduleId": "b6d7f8e9-a0c1-b2d3-e4f5-a6b7c8d9eaf0" },
    { "Id": "b1a3f4e5-d6c7-b8e9-f0a1-b2c3d4e5f6a7", "Name": "S1", "DailyScheduleId": "b6d7f8e9-a0c1-b2d3-e4f5-a6b7c8d9eaf0" },
    { "Id": "c2b4a5f6-e7d8-c9f0-a1b2-c3d4e5f6a7b8", "Name": "S2", "DailyScheduleId": "c7e8a9f0-b1d2-c3e4-f5a6-b7c8d9eaf0b1" },
    { "Id": "d3c5b6a7-f8e9-d0a1-b2c3-d4e5f6a7b8c9", "Name": "D1", "DailyScheduleId": "c7e8a9f0-b1d2-c3e4-f5a6-b7c8d9eaf0b1" },
    { "Id": "e4d6c7b8-a9f0-e1b2-c3d4-e5f6a7b8c9da", "Name": "D2", "DailyScheduleId": "c7e8a9f0-b1d2-c3e4-f5a6-b7c8d9eaf0b1" },
    { "Id": "f5e7d8c9-b0a1-f2c3-d4e5-f6a7b8c9daeb", "Name": "D3", "DailyScheduleId": "d8f9b0a1-c2e3-d4f5-a6b7-c8d9eaf0b1c2" },
    { "Id": "a6f8e9d0-c1b2-a3d4-e5f6-a7b8c9daebfc", "Name": "D4", "DailyScheduleId": "d8f9b0a1-c2e3-d4f5-a6b7-c8d9eaf0b1c2" },
    { "Id": "b7a9f0e1-d2c3-b4e5-f6a7-b8c9daebfc0d", "Name": "D5", "DailyScheduleId": "e9a0c1b2-d3f4-e5a6-b7c8-d9eaf0b1c2d3" },
    { "Id": "c8ba0fb2-e3d4-c5f6-a7b8-c9daebfc0d1e", "Name": "D6", "DailyScheduleId": "f0b1d2c3-e4a5-f6b7-c8d9-eaf0b1c2d3e4" },
    { "Id": "d9cba1fc-f4e5-d6a7-b8c9-daebfc0d1e2f", "Name": "S1", "DailyScheduleId": "f0b1d2c3-e4a5-f6b7-c8d9-eaf0b1c2d3e4" },
    { "Id": "eadcb2ad-a5f6-e7b8-c9da-ebfc0d1e2f30", "Name": "S2", "DailyScheduleId": "a1c2e3d4-f5b6-a7c8-d9ea-f0b1c2d3e4f5" },
    { "Id": "fbedc3be-b6a7-f8c9-daeb-fc0d1e2f3041", "Name": "D1", "DailyScheduleId": "b2d3f4e5-a6c7-b8d9-eaf0-b1c2d3e4f5a6" },
    { "Id": "acfdc4cf-c7b8-a9da-ebfc-0d1e2f304152", "Name": "D2", "DailyScheduleId": "b2d3f4e5-a6c7-b8d9-eaf0-b1c2d3e4f5a6" },
    { "Id": "bd0ed5d0-d8c9-bada-fc0d-1e2f30415263", "Name": "D3", "DailyScheduleId": "b2d3f4e5-a6c7-b8d9-eaf0-b1c2d3e4f5a6" },
    { "Id": "ce1fe6e1-e9da-cbdb-0d1e-2f3041526374", "Name": "D4", "DailyScheduleId": "c3e4a5f6-b7d8-c9ea-f0b1-c2d3e4f5a6b7" },
    { "Id": "df20f7f2-fadc-dcec-1e2f-304152637485", "Name": "D5", "DailyScheduleId": "d4f5b6a7-c8e9-d0f0-b1c2-d3e4f5a6b7c8" },
    { "Id": "e0310803-0bed-edfd-2f30-415263748596", "Name": "D6", "DailyScheduleId": "d4f5b6a7-c8e9-d0f0-b1c2-d3e4f5a6b7c8" },
    { "Id": "f1421914-1cfe-f00e-3041-5263748596a7", "Name": "S1", "DailyScheduleId": "e5a6c7b8-d9f0-e1a0-b1c2-d3e4f5a6b7c8" },
    { "Id": "a2532a25-2d0f-011f-4152-63748596a7b8", "Name": "S2", "DailyScheduleId": "e5a6c7b8-d9f0-e1a0-b1c2-d3e4f5a6b7c8" },
    { "Id": "b3643b36-3e10-1230-5263-748596a7b8c9", "Name": "D1", "DailyScheduleId": "e5a6c7b8-d9f0-e1a0-b1c2-d3e4f5a6b7c8" },
    { "Id": "c4754c47-4f21-2341-6374-8596a7b8c9da", "Name": "D2", "DailyScheduleId": "f6b7d8c9-eaf0-f1b0-c2d3-e4f5a6b7c8d9" },
    { "Id": "d5865d58-5032-3452-7485-96a7b8c9daeb", "Name": "D3", "DailyScheduleId": "f6b7d8c9-eaf0-f1b0-c2d3-e4f5a6b7c8d9" },
    { "Id": "e6976e69-6143-4563-8596-a7b8c9daebfc", "Name": "D4", "DailyScheduleId": "a7c8e9d0-f0b1-a1c2-d3e4-f5a6b7c8d9ea" },
    { "Id": "f7a87f7a-7254-5674-96a7-b8c9daebfc0d", "Name": "D5", "DailyScheduleId": "b8d9f0e1-a0c1-b2d3-e4f5-a6b7c8d9eaf0" },
    { "Id": "a8b9808b-8365-6785-a7b8-c9daebfc0d1e", "Name": "D6", "DailyScheduleId": "b8d9f0e1-a0c1-b2d3-e4f5-a6b7c8d9eaf0" },
    { "Id": "b9ca919c-9476-7896-b8c9-daebfc0d1e2f", "Name": "S1", "DailyScheduleId": "b8d9f0e1-a0c1-b2d3-e4f5-a6b7c8d9eaf0" },
    { "Id": "cadaa2ad-a587-89a7-c9da-ebfc0d1e2f30", "Name": "S2", "DailyScheduleId": "b8d9f0e1-a0c1-b2d3-e4f5-a6b7c8d9eaf0" },
    { "Id": "dbdbb3be-b698-9ab8-daeb-fc0d1e2f3041", "Name": "D1", "DailyScheduleId": "c9ea0fb1-b1d2-c3e4-f5a6-b7c8d9eaf0b1" },
    { "Id": "ececc4cf-c7a9-abcd-ebfc-0d1e2f304152", "Name": "D2", "DailyScheduleId": "d0f0b1a1-c2e3-d4f5-a6b7-c8d9eaf0b1c2" },
    { "Id": "fdfdd5d0-d8ba-bdef-fc0d-1e2f30415263", "Name": "D3", "DailyScheduleId": "d0f0b1a1-c2e3-d4f5-a6b7-c8d9eaf0b1c2" }
]
""";

    public static (List<ScheduleCourt> courts, Dictionary<string, (string scheduleId, string courtName)> courtLookup) CreateScheduleCourts()
    {
        var dtos = JsonSerializer.Deserialize<List<CourtDto>>(Json)!;
        var courts = new List<ScheduleCourt>(dtos.Count);
        var lookup = new Dictionary<string, (string, string)>(dtos.Count);

        foreach (var dto in dtos)
        {
            courts.Add(new ScheduleCourt
            {
                CourtNumber = dto.Name,
                ScheduleId = dto.DailyScheduleId,
                IsOccupied = 0
            });
            lookup[dto.Id] = (dto.DailyScheduleId, dto.Name);
        }

        return (courts, lookup);
    }
}
