using DCA_Padel_Club.Core.QueryContracts.Abstractions;

namespace DCA_Padel_Club.Core.QueryContracts.Queries;

public class PlayerOverview
{
    public record Query() : IQuery<Answer>;

    public record Answer(List<PlayerInfo> Players);

    public record PlayerInfo(
        int PlayerId,
        string FirstName,
        string LastName,
        string Email,
        bool Blacklisted
    );
}

