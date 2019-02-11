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
        public IApplicationResult<IPlayer> Execute()
        {
            var player = m_playerRepository.FindById(m_playerId);

            var renamedPlayerResult = player.Rename(m_renamedName);

            // type switch を使ってパターンマッチで failure/success を実装する
            switch (renamedPlayerResult)
            {
                case Domain.Failure<IPlayer> failure:
                    return ApplicationResult.Failure<IPlayer>(failure.Errors.Select(error => error.ToApplicationError()));

                case Domain.Success<IPlayer> success:
                    var renamedPlayer = success.Result;
                    var savedPlayer = m_playerRepository.Save(renamedPlayer);
                    return ApplicationResult.Success(savedPlayer);
            }

            // TODO 想定外の例外であることを詰めて返す
            return ApplicationResult.Failure<IPlayer>();
        }
    }
}