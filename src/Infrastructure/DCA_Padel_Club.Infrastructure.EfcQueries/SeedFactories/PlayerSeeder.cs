using System.Text.Json;
using DCA_Padel_Club.Infrastructure.EfcQueries.Models;
using DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories.Json;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories;

public static class PlayerSeeder
{
    private const string DefaultPassword = "DefaultPassword123!";

    private const string Json = """
[
    {
      "Id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
      "Email": "123456@via.dk",
      "FirstName": "Alice",
      "IsBlacklisted": 0,
      "LastName": "Smith",
      "ProfilePicture": "https://i.pravatar.cc/150?u=123456",
      "QuarantinedUntil": null,
      "VipUntil": "2025-04-10"
    },
    {
      "Id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
      "Email": "987654@via.dk",
      "FirstName": "Bob",
      "IsBlacklisted": 0,
      "LastName": "Jones",
      "ProfilePicture": "https://i.pravatar.cc/150?u=987654",
      "QuarantinedUntil": "2025-04-01",
      "VipUntil": null
    },
    {
      "Id": "11223344-5566-7788-99aa-bbccddeeff00",
      "Email": "112233@via.dk",
      "FirstName": "Charlie",
      "IsBlacklisted": 0,
      "LastName": "Brown",
      "ProfilePicture": "https://i.pravatar.cc/150?u=112233",
      "QuarantinedUntil": null,
      "VipUntil": "2025-04-22"
    },
    {
      "Id": "ec68f9e7-4b0d-4d4f-b32f-8d8c4a6b2e0a",
      "Email": "445566@via.dk",
      "FirstName": "Diana",
      "IsBlacklisted": 0,
      "LastName": "Williams",
      "ProfilePicture": "https://i.pravatar.cc/150?u=445566",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "9f8e7d6c-5b4a-3c2d-1e0f-abcdef123456",
      "Email": "778899@via.dk",
      "FirstName": "Ethan",
      "IsBlacklisted": 1,
      "LastName": "Davis",
      "ProfilePicture": "https://i.pravatar.cc/150?u=778899",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "0a1b3c5d-7e9f-1a3c-5e7f-9a1b3c5d7e9f",
      "Email": "101010@via.dk",
      "FirstName": "Fiona",
      "IsBlacklisted": 0,
      "LastName": "Miller",
      "ProfilePicture": "https://i.pravatar.cc/150?u=101010",
      "QuarantinedUntil": null,
      "VipUntil": "2025-04-05"
    },
    {
      "Id": "f0e1d2c3-b4a5-9876-5432-10fedcba9876",
      "Email": "202020@via.dk",
      "FirstName": "George",
      "IsBlacklisted": 0,
      "LastName": "Wilson",
      "ProfilePicture": "https://i.pravatar.cc/150?u=202020",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "5a6b7c8d-9e0f-1a2b-3c4d-5e6f7a8b9c0d",
      "Email": "303030@via.dk",
      "FirstName": "Hannah",
      "IsBlacklisted": 0,
      "LastName": "Moore",
      "ProfilePicture": "https://i.pravatar.cc/150?u=303030",
      "QuarantinedUntil": "2025-04-02",
      "VipUntil": null
    },
    {
      "Id": "b9c8d7e6-f5a4-b3c2-d1e0-f9a8b7c6d5e4",
      "Email": "404040@via.dk",
      "FirstName": "Ian",
      "IsBlacklisted": 0,
      "LastName": "Taylor",
      "ProfilePicture": "https://i.pravatar.cc/150?u=404040",
      "QuarantinedUntil": null,
      "VipUntil": "2025-04-15"
    },
    {
      "Id": "e3f2a1b0-c9d8-e7f6-a5b4-c3d2e1f0a9b8",
      "Email": "505050@via.dk",
      "FirstName": "Julia",
      "IsBlacklisted": 0,
      "LastName": "Anderson",
      "ProfilePicture": "https://i.pravatar.cc/150?u=505050",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "c7d6b5a4-e3f2-a1b0-c9d8-e7f6a5b4c3d2",
      "Email": "606060@via.dk",
      "FirstName": "Kevin",
      "IsBlacklisted": 0,
      "LastName": "Thomas",
      "ProfilePicture": "https://i.pravatar.cc/150?u=606060",
      "QuarantinedUntil": null,
      "VipUntil": "2025-04-29"
    },
    {
      "Id": "a5b4c3d2-e1f0-a9b8-c7d6-b5a4e3f2a1b0",
      "Email": "707070@via.dk",
      "FirstName": "Laura",
      "IsBlacklisted": 0,
      "LastName": "Jackson",
      "ProfilePicture": "https://i.pravatar.cc/150?u=707070",
      "QuarantinedUntil": "2025-04-03",
      "VipUntil": null
    },
    {
      "Id": "f9a8b7c6-d5e4-f3a2-b1c0-d9e8f7a6b5c4",
      "Email": "808080@via.dk",
      "FirstName": "Mike",
      "IsBlacklisted": 0,
      "LastName": "White",
      "ProfilePicture": "https://i.pravatar.cc/150?u=808080",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "d9e8f7a6-b5c4-d3e2-f1a0-b9c8d7e6f5a4",
      "Email": "909090@via.dk",
      "FirstName": "Nora",
      "IsBlacklisted": 0,
      "LastName": "Harris",
      "ProfilePicture": "https://i.pravatar.cc/150?u=909090",
      "QuarantinedUntil": null,
      "VipUntil": "2025-04-01"
    },
    {
      "Id": "b1c0d9e8-f7a6-b5c4-d3e2-f1a0b9c8d7e6",
      "Email": "191919@via.dk",
      "FirstName": "Oscar",
      "IsBlacklisted": 1,
      "LastName": "Martin",
      "ProfilePicture": "https://i.pravatar.cc/150?u=191919",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "f3a2b1c0-d9e8-f7a6-b5c4-d3e2f1a0b9c8",
      "Email": "282828@via.dk",
      "FirstName": "Penny",
      "IsBlacklisted": 0,
      "LastName": "Thompson",
      "ProfilePicture": "https://i.pravatar.cc/150?u=282828",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "d3e2f1a0-b9c8-d7e6-f5a4-b3c2d1e0f9a8",
      "Email": "373737@via.dk",
      "FirstName": "Quentin",
      "IsBlacklisted": 0,
      "LastName": "Garcia",
      "ProfilePicture": "https://i.pravatar.cc/150?u=373737",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "b3c2d1e0-f9a8-b7c6-d5e4-f3a2b1c0d9e8",
      "Email": "464646@via.dk",
      "FirstName": "Rachel",
      "IsBlacklisted": 0,
      "LastName": "Martinez",
      "ProfilePicture": "https://i.pravatar.cc/150?u=464646",
      "QuarantinedUntil": null,
      "VipUntil": "2025-04-18"
    },
    {
      "Id": "f1a0b9c8-d7e6-f5a4-b3c2-d1e0f9a8b7c6",
      "Email": "545454@via.dk",
      "FirstName": "Steve",
      "IsBlacklisted": 0,
      "LastName": "Robinson",
      "ProfilePicture": "https://i.pravatar.cc/150?u=545454",
      "QuarantinedUntil": null,
      "VipUntil": null
    },
    {
      "Id": "d1e0f9a8-b7c6-d5e4-f3a2-b1c0d9e8f7a6",
      "Email": "636363@via.dk",
      "FirstName": "Tina",
      "IsBlacklisted": 0,
      "LastName": "Clark",
      "ProfilePicture": "https://i.pravatar.cc/150?u=636363",
      "QuarantinedUntil": null,
      "VipUntil": null
    }
]
""";

    public static (List<Player> players, Dictionary<string, int> guidToViaId) CreatePlayers()
    {
        var dtos = JsonSerializer.Deserialize<List<PlayerDto>>(Json)!;
        var players = new List<Player>(dtos.Count);
        var guidToViaId = new Dictionary<string, int>(dtos.Count);

        foreach (var dto in dtos)
        {
            var atIndex = dto.Email.IndexOf('@');
            var viaId = int.Parse(dto.Email[..atIndex]);
            guidToViaId[dto.Id] = viaId;

            players.Add(new Player
            {
                Id = viaId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = DefaultPassword,
                ProfilePicture = dto.ProfilePicture,
                Blacklisted = dto.IsBlacklisted,
                QuarantineEndDate = dto.QuarantinedUntil,
                CooldownExpiresAt = null
            });
        }

        return (players, guidToViaId);
    }
}
