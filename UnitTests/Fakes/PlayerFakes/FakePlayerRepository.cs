    using DCA_Padel_Club.Core.Domain.Aggregates.Players;
    using DCA_Padel_Club.Core.Domain.Common.Contracts;

    namespace UnitTests.Fakes.PlayerFakes;

    public class FakePlayerRepository : IPlayerRepository
    {
        private readonly Dictionary<ViaId, Player> _players = new();
        
        public Task AddAsync(Player player)
        {
            _players[player.Id] = player;
            return Task.CompletedTask;
        }

        public Task<Player?> GetByIdAsync(ViaId id)
        {
            _players.TryGetValue(id, out var player);
            return Task.FromResult(player);
        }
    }