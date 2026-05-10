using System.Text.Json;
using DCA_Padel_Club.Infrastructure.EfcQueries.Models;
using DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories.Json;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories;

public static class BookingSeeder
{
    private const string Json = """
[
    { "Id": "b2c3d4e5-0001-0000-0000-000000000001", "CourtId": "a8f0e1d2-c3b4-a5d6-e7f8-a9b0c1d2e3f4", "PlayerId": "f47ac10b-58cc-4372-a567-0e02b2c3d479", "TimeStart": "10:30:00", "TimeEnd": "12:00:00" },
    { "Id": "b2c3d4e5-0002-0000-0000-000000000002", "CourtId": "a8f0e1d2-c3b4-a5d6-e7f8-a9b0c1d2e3f4", "PlayerId": "ec68f9e7-4b0d-4d4f-b32f-8d8c4a6b2e0a", "TimeStart": "13:00:00", "TimeEnd": "14:30:00" },
    { "Id": "b2c3d4e5-0003-0000-0000-000000000003", "CourtId": "b9a1f2e3-d4c5-b6e7-f8a9-b0c1d2e3f4a5", "PlayerId": "11223344-5566-7788-99aa-bbccddeeff00", "TimeStart": "11:30:00", "TimeEnd": "13:00:00" },
    { "Id": "b2c3d4e5-0004-0000-0000-000000000004", "CourtId": "b9a1f2e3-d4c5-b6e7-f8a9-b0c1d2e3f4a5", "PlayerId": "0a1b3c5d-7e9f-1a3c-5e7f-9a1b3c5d7e9f", "TimeStart": "14:00:00", "TimeEnd": "15:30:00" },
    { "Id": "b2c3d4e5-0005-0000-0000-000000000005", "CourtId": "c0b2a3f4-e5d6-c7f8-a9b0-c1d2e3f4a5b6", "PlayerId": "5a6b7c8d-9e0f-1a2b-3c4d-5e6f7a8b9c0d", "TimeStart": "15:00:00", "TimeEnd": "16:30:00" },
    { "Id": "b2c3d4e5-0006-0000-0000-000000000006", "CourtId": "c0b2a3f4-e5d6-c7f8-a9b0-c1d2e3f4a5b6", "PlayerId": "e3f2a1b0-c9d8-e7f6-a5b4-c3d2e1f0a9b8", "TimeStart": "18:00:00", "TimeEnd": "19:30:00" },
    { "Id": "b2c3d4e5-0007-0000-0000-000000000007", "CourtId": "d1c3b4a5-f6e7-d8a9-b0c1-d2e3f4a5b6c7", "PlayerId": "ec68f9e7-4b0d-4d4f-b32f-8d8c4a6b2e0a", "TimeStart": "16:00:00", "TimeEnd": "18:00:00" },
    { "Id": "b2c3d4e5-0008-0000-0000-000000000008", "CourtId": "e2d4c5b6-a7f8-e9b0-c1d2-e3f4a5b6c7d8", "PlayerId": "a1b2c3d4-e5f6-7890-1234-567890abcdef", "TimeStart": "15:00:00", "TimeEnd": "17:00:00" },
    { "Id": "b2c3d4e5-0009-0000-0000-000000000009", "CourtId": "f3e5d6c7-b8a9-f0c1-d2e3-f4a5b6c7d8e9", "PlayerId": "a5b4c3d2-e1f0-a9b8-c7d6-b5a4e3f2a1b0", "TimeStart": "12:30:00", "TimeEnd": "14:30:00" },
    { "Id": "b2c3d4e5-0010-0000-0000-000000000010", "CourtId": "f3e5d6c7-b8a9-f0c1-d2e3-f4a5b6c7d8e9", "PlayerId": "b9c8d7e6-f5a4-b3c2-d1e0-f9a8b7c6d5e4", "TimeStart": "16:00:00", "TimeEnd": "17:30:00" },
    { "Id": "b2c3d4e5-0011-0000-0000-000000000011", "CourtId": "a4f6e7d8-c9b0-a1d2-e3f4-a5b6c7d8e9f0", "PlayerId": "c7d6b5a4-e3f2-a1b0-c9d8-e7f6a5b4c3d2", "TimeStart": "11:30:00", "TimeEnd": "13:30:00" },
    { "Id": "b2c3d4e5-0012-0000-0000-000000000012", "CourtId": "a4f6e7d8-c9b0-a1d2-e3f4-a5b6c7d8e9f0", "PlayerId": "f0e1d2c3-b4a5-9876-5432-10fedcba9876", "TimeStart": "14:30:00", "TimeEnd": "16:00:00" },
    { "Id": "b2c3d4e5-0014-0000-0000-000000000014", "CourtId": "b5a7f8e9-d0c1-b2e3-f4a5-b6c7d8e9f0a1", "PlayerId": "b9c8d7e6-f5a4-b3c2-d1e0-f9a8b7c6d5e4", "TimeStart": "14:00:00", "TimeEnd": "15:30:00" },
    { "Id": "b2c3d4e5-0015-0000-0000-000000000015", "CourtId": "b5a7f8e9-d0c1-b2e3-f4a5-b6c7d8e9f0a1", "PlayerId": "f47ac10b-58cc-4372-a567-0e02b2c3d479", "TimeStart": "19:00:00", "TimeEnd": "20:30:00" },
    { "Id": "b2c3d4e5-0016-0000-0000-000000000016", "CourtId": "c6b8a9f0-e1d2-c3f4-a5b6-c7d8e9f0a1b2", "PlayerId": "ec68f9e7-4b0d-4d4f-b32f-8d8c4a6b2e0a", "TimeStart": "16:30:00", "TimeEnd": "18:30:00" },
    { "Id": "b2c3d4e5-0017-0000-0000-000000000017", "CourtId": "d7c9b0a1-f2e3-d4a5-b6c7-d8e9f0a1b2c3", "PlayerId": "11223344-5566-7788-99aa-bbccddeeff00", "TimeStart": "14:00:00", "TimeEnd": "15:30:00" },
    { "Id": "b2c3d4e5-0018-0000-0000-000000000018", "CourtId": "d7c9b0a1-f2e3-d4a5-b6c7-d8e9f0a1b2c3", "PlayerId": "c7d6b5a4-e3f2-a1b0-c9d8-e7f6a5b4c3d2", "TimeStart": "18:30:00", "TimeEnd": "20:00:00" },
    { "Id": "b2c3d4e5-0019-0000-0000-000000000019", "CourtId": "a0f2e3d4-c5b6-a7d8-e9f0-a1b2c3d4e5f6", "PlayerId": "e3f2a1b0-c9d8-e7f6-a5b4-c3d2e1f0a9b8", "TimeStart": "15:00:00", "TimeEnd": "17:00:00" },
    { "Id": "b2c3d4e5-0020-0000-0000-000000000020", "CourtId": "b1a3f4e5-d6c7-b8e9-f0a1-b2c3d4e5f6a7", "PlayerId": "f1a0b9c8-d7e6-f5a4-b3c2-d1e0f9a8b7c6", "TimeStart": "16:00:00", "TimeEnd": "18:30:00" },
    { "Id": "b2c3d4e5-0021-0000-0000-000000000021", "CourtId": "c2b4a5f6-e7d8-c9f0-a1b2-c3d4e5f6a7b8", "PlayerId": "b9c8d7e6-f5a4-b3c2-d1e0-f9a8b7c6d5e4", "TimeStart": "13:30:00", "TimeEnd": "15:00:00" },
    { "Id": "b2c3d4e5-0022-0000-0000-000000000022", "CourtId": "d3c5b6a7-f8e9-d0a1-b2c3-d4e5f6a7b8c9", "PlayerId": "11223344-5566-7788-99aa-bbccddeeff00", "TimeStart": "14:00:00", "TimeEnd": "15:30:00" },
    { "Id": "b2c3d4e5-0023-0000-0000-000000000023", "CourtId": "e4d6c7b8-a9f0-e1b2-c3d4-e5f6a7b8c9da", "PlayerId": "ec68f9e7-4b0d-4d4f-b32f-8d8c4a6b2e0a", "TimeStart": "12:30:00", "TimeEnd": "13:30:00" },
    { "Id": "b2c3d4e5-0024-0000-0000-000000000024", "CourtId": "f5e7d8c9-b0a1-f2c3-d4e5-f6a7b8c9daeb", "PlayerId": "f0e1d2c3-b4a5-9876-5432-10fedcba9876", "TimeStart": "10:00:00", "TimeEnd": "11:00:00" },
    { "Id": "b2c3d4e5-0025-0000-0000-000000000025", "CourtId": "f5e7d8c9-b0a1-f2c3-d4e5-f6a7b8c9daeb", "PlayerId": "c7d6b5a4-e3f2-a1b0-c9d8-e7f6a5b4c3d2", "TimeStart": "11:00:00", "TimeEnd": "13:00:00" },
    { "Id": "b2c3d4e5-0026-0000-0000-000000000026", "CourtId": "f5e7d8c9-b0a1-f2c3-d4e5-f6a7b8c9daeb", "PlayerId": "b3c2d1e0-f9a8-b7c6-d5e4-f3a2b1c0d9e8", "TimeStart": "17:00:00", "TimeEnd": "18:30:00" },
    { "Id": "b2c3d4e5-0027-0000-0000-000000000027", "CourtId": "a6f8e9d0-c1b2-a3d4-e5f6-a7b8c9daebfc", "PlayerId": "11223344-5566-7788-99aa-bbccddeeff00", "TimeStart": "12:00:00", "TimeEnd": "14:00:00" },
    { "Id": "b2c3d4e5-0028-0000-0000-000000000028", "CourtId": "a6f8e9d0-c1b2-a3d4-e5f6-a7b8c9daebfc", "PlayerId": "e3f2a1b0-c9d8-e7f6-a5b4-c3d2e1f0a9b8", "TimeStart": "15:00:00", "TimeEnd": "16:30:00" },
    { "Id": "b2c3d4e5-0029-0000-0000-000000000029", "CourtId": "a6f8e9d0-c1b2-a3d4-e5f6-a7b8c9daebfc", "PlayerId": "b9c8d7e6-f5a4-b3c2-d1e0-f9a8b7c6d5e4", "TimeStart": "19:00:00", "TimeEnd": "20:00:00" },
    { "Id": "b2c3d4e5-0030-0000-0000-000000000030", "CourtId": "b7a9f0e1-d2c3-b4e5-f6a7-b8c9daebfc0d", "PlayerId": "f47ac10b-58cc-4372-a567-0e02b2c3d479", "TimeStart": "14:00:00", "TimeEnd": "16:00:00" },
    { "Id": "b2c3d4e5-0031-0000-0000-000000000031", "CourtId": "b7a9f0e1-d2c3-b4e5-f6a7-b8c9daebfc0d", "PlayerId": "b9c8d7e6-f5a4-b3c2-d1e0-f9a8b7c6d5e4", "TimeStart": "17:00:00", "TimeEnd": "18:30:00" },
    { "Id": "b2c3d4e5-0032-0000-0000-000000000032", "CourtId": "b7a9f0e1-d2c3-b4e5-f6a7-b8c9daebfc0d", "PlayerId": "f1a0b9c8-d7e6-f5a4-b3c2-d1e0f9a8b7c6", "TimeStart": "19:30:00", "TimeEnd": "21:00:00" },
    { "Id": "b2c3d4e5-0033-0000-0000-000000000033", "CourtId": "c8ba0fb2-e3d4-c5f6-a7b8-c9daebfc0d1e", "PlayerId": "11223344-5566-7788-99aa-bbccddeeff00", "TimeStart": "11:00:00", "TimeEnd": "12:30:00" },
    { "Id": "b2c3d4e5-0034-0000-0000-000000000034", "CourtId": "d9cba1fc-f4e5-d6a7-b8c9-daebfc0d1e2f", "PlayerId": "b9c8d7e6-f5a4-b3c2-d1e0-f9a8b7c6d5e4", "TimeStart": "12:00:00", "TimeEnd": "13:30:00" },
    { "Id": "b2c3d4e5-0035-0000-0000-000000000035", "CourtId": "eadcb2ad-a5f6-e7b8-c9da-ebfc0d1e2f30", "PlayerId": "ec68f9e7-4b0d-4d4f-b32f-8d8c4a6b2e0a", "TimeStart": "15:30:00", "TimeEnd": "17:30:00" },
    { "Id": "b2c3d4e5-0036-0000-0000-000000000036", "CourtId": "eadcb2ad-a5f6-e7b8-c9da-ebfc0d1e2f30", "PlayerId": "b3c2d1e0-f9a8-b7c6-d5e4-f3a2b1c0d9e8", "TimeStart": "18:30:00", "TimeEnd": "20:00:00" },
    { "Id": "b2c3d4e5-0037-0000-0000-000000000037", "CourtId": "ce1fe6e1-e9da-cbdb-0d1e-2f3041526374", "PlayerId": "c7d6b5a4-e3f2-a1b0-c9d8-e7f6a5b4c3d2", "TimeStart": "13:00:00", "TimeEnd": "15:00:00" },
    { "Id": "b2c3d4e5-0038-0000-0000-000000000038", "CourtId": "ce1fe6e1-e9da-cbdb-0d1e-2f3041526374", "PlayerId": "f1a0b9c8-d7e6-f5a4-b3c2-d1e0f9a8b7c6", "TimeStart": "16:00:00", "TimeEnd": "18:00:00" },
    { "Id": "b2c3d4e5-0039-0000-0000-000000000039", "CourtId": "ce1fe6e1-e9da-cbdb-0d1e-2f3041526374", "PlayerId": "d1e0f9a8-b7c6-d5e4-f3a2-b1c0d9e8f7a6", "TimeStart": "19:00:00", "TimeEnd": "21:00:00" }
]
""";

    public static List<Booking> CreateBookings(
        ViapadelClubContext ctx,
        Dictionary<string, int> guidToViaId,
        Dictionary<string, (string scheduleId, string courtName)> courtLookup,
        Dictionary<string, string> scheduleDates)
    {
        var dtos = JsonSerializer.Deserialize<List<BookingDto>>(Json)!;
        var bookings = new List<Booking>(dtos.Count);

        foreach (var dto in dtos)
        {
            var (scheduleId, courtName) = courtLookup[dto.CourtId];
            var bookerViaId = guidToViaId[dto.PlayerId];
            var slotDate = scheduleDates[scheduleId];

            var booking = new Booking
            {
                BookingId = dto.Id,
                ScheduleId = scheduleId,
                CourtNumber = courtName,
                BookerId = bookerViaId,
                SlotDate = slotDate,
                SlotStartTime = dto.TimeStart,
                SlotEndTime = dto.TimeEnd
            };

            // Attach the booker to the many-to-many Players join. Use the tracked entity
            // so EF re-uses the existing Player row (no INSERT into Players).
            var bookerPlayer = ctx.Players.Local.SingleOrDefault(p => p.Id == bookerViaId)
                               ?? ctx.Players.Find(bookerViaId);
            if (bookerPlayer is not null)
            {
                booking.Players.Add(bookerPlayer);
            }

            bookings.Add(booking);
        }

        return bookings;
    }
}
