using Domain.Player;
using System.Linq;

namespace UseCase.Player
{
    /// <summary>
    /// プレイヤーリネームユースケース
    /// </summary>
    public sealed class RenamePlayerUseCase : IUseCase<IPlayer>
    {
        /// <summary>
        /// プレイヤーリポジトリ
        /// </summary>
        /// DI できる環境であれば DI する
        private readonly IPlayerRepository m_playerRepository;

        /// <summary>
        /// リネーム対象のプレイヤーID
        /// </summary>
        private readonly PlayerId m_playerId;

        /// <summary>
        /// リネーム後プレイヤー名
        /// </summary>
        private readonly PlayerName m_renamedName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="playerRepository">Player repository.</param>
        /// <param name="playerId">Player identifier.</param>
        /// <param name="renamedName">Renamed name.</param>
        /// what （なにをするか）だけを意識するため
        /// how （どうやってするか）に関するもの（ここでいうなら playerRepository ）は DI するのがベスト
        public RenamePlayerUseCase(IPlayerRepository playerRepository, PlayerId playerId, PlayerName renamedName) =>
            (m_playerRepository, m_playerId, m_renamedName) =
            (playerRepository, playerId, renamedName);

        /// <summary>
        /// リネームユースケース実行
        /// </summary>
        /// <returns>リネーム後のプレイヤー</returns>
        /// UniTask が使える環境であれば async メソッドにする
        public ApplicationResult<IPlayer> Execute()
        {
            var player = m_playerRepository.FindById(m_playerId);

            var renamedPlayer = player.Rename(m_renamedName);

            var savedPlayer = m_playerRepository.Save(renamedPlayer);

            return new ApplicationResult<IPlayer>(savedPlayer, Enumerable.Empty<ApplicationError>());
        }
    }
}