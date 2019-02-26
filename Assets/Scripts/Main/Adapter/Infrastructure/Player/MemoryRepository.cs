using System;
using System.Collections.Generic;
using Domain;
using Domain.Player;

namespace Adapter.Infrastructure.Player
{
    /// <summary>
    /// メモリリポジトリ
    /// </summary>
    public sealed class MemoryRepository : IPlayerRepository
    {
        /// <summary>
        /// プレイヤーidを元にプレイヤーを検索する
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public IDomainResult<IPlayer> FindById(PlayerId id)
        {
            try
            {
                var nullablePlayer = MemoryStore.FindById(id);
                return DomainResult.Success(nullablePlayer);
            }
            catch(ArgumentNullException exception)
            {
                return DomainResult.Failure<IPlayer>(new NotFoundError("プレイヤーが見つかりませんでした。", exception, ErrorLevel.ERROR));
            }
        }

        /// <summary>
        /// プレイヤーを保存する
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="player">Player.</param>
        public IDomainResult<IPlayer> Save(IPlayer player) => throw new NotImplementedException();

        /// <summary>
        /// メモリストア
        /// </summary>
        private static class MemoryStore
        {
            /// <summary>
            /// ストア
            /// </summary>
            private static readonly IDictionary<PlayerId, IPlayer> m_store = new Dictionary<PlayerId, IPlayer>();

            /// <summary>
            /// プレイヤー検索
            /// </summary>
            /// <returns>The by identifier.</returns>
            /// <param name="id">Identifier.</param>
            public static IPlayer FindById(PlayerId id) => m_store[id];
        }
    }


}