using DCA_Padel_Club.Core.Domain.Aggregates.Players;

namespace DCA_Padel_Club.Core.Domain.Common.Contracts;

public interface IPlayerRepository
{
    Task AddAsync(Player player);
    Task<Player?> GetByIdAsync(ViaId id);
}