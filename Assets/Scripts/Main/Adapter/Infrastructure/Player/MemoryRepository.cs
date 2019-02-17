using System;
using Domain;
using Domain.Player;
using System.Collections.Generic;

namespace Adapter.Infrastructure.Player
{
    public sealed class MemoryRepository : IPlayerRepository
    {
        public IDomainResult<IPlayer> FindById(PlayerId id)
        {
            try
            {
                var nullablePlayer = MemoryStore.FindMyId(id);
                return DomainResult.Success(nullablePlayer);
            }
            catch(ArgumentNullException exception)
            {
                return DomainResult.Failure<IPlayer>(new NotFoundError("プレイヤーが見つかりませんでした。", exception, ErrorLevel.ERROR));
            }
        }

        public IDomainResult<IPlayer> Save(IPlayer player) => throw new NotImplementedException();
        private static class MemoryStore
        {
            private static readonly IDictionary<PlayerId, IPlayer> m_store = new Dictionary<PlayerId, IPlayer>();

            public static IPlayer FindMyId(PlayerId id) => m_store[id];
        }
    }


}