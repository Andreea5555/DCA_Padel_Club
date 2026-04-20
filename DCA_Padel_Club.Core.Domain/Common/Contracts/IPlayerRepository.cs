using DCA_Padel_Club.Core.Domain.Aggregates.Players;

namespace DCA_Padel_Club.Core.Domain.Common.Contracts;

public interface IPlayerRepository: IGenericRepository<Player,ViaId>
{
    Task AddAsync(Player player);
    Task<Player?> GetByIdAsync(ViaId id);
}