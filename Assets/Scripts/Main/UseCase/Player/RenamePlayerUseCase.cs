using Domain;
using Domain.Player;
using Domain.Exceptions;

namespace UseCase.Player
{
    /// <summary>
    /// プレイヤーリネームユースケース
    /// </summary>
    public sealed class RenamePlayerUseCase : UseCase<IPlayer, (PlayerId playerId, PlayerName renamedName)>
    {
        #region configure
        /// <summary>
        /// プレイヤーリポジトリ
        /// </summary>
        /// DI できる環境であれば DI する
        private readonly IPlayerRepository m_playerRepository;

        /// <summary>
        /// バリデーションエラーレベル
        /// </summary>
        /// <value>The validation error level.</value>
        protected override ErrorLevel ValidationErrorLevel => ErrorLevel.WARNING;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="playerRepository">Player repository.</param>
        /// how （どうやってするか）に関するもの（ここでいうなら playerRepository ）はコンストラクタで注入する
        /// 環境的に可能であれば DI するのがベスト
        public RenamePlayerUseCase(IPlayerRepository playerRepository) => m_playerRepository = playerRepository;
        #endregion

        /// <summary>
        /// リネームユースケース実行
        /// </summary>
        /// <returns>リネーム後のプレイヤー</returns>
        /// <param name="protocol">Protocol.</param>
        protected override IApplicationResult<IPlayer> ExecuteImpl((PlayerId playerId, PlayerName renamedName) protocol)
        {
            var player = m_playerRepository.FindById(protocol.playerId);

            var renamedPlayerResult = player.Rename(protocol.renamedName);

            // type switch を使ってパターンマッチで failure/success を実装する
            switch (renamedPlayerResult)
            {
                case Domain.Failure<IPlayer> failure:
                    return ApplicationResult.Failure<IPlayer>(failure.Reason.ToApplicationError());

                case Domain.Success<IPlayer> success:
                    var renamedPlayer = success.Result;
                    var savedPlayer = m_playerRepository.Save(renamedPlayer);
                    return ApplicationResult.Success(savedPlayer);
            }

            return ApplicationResult.Unexpected<IPlayer>();
        }
    }

    #region protocol
    /// <summary>
    /// リネームユースケースで使用するプロトコル定義
    /// </summary>
    /// ユースケースとプロトコルを合わせて定義することで
    /// そのユースケースを実行するのに必要な情報を同ファイル内に閉じ込める
    public static class RenamePlayerProtocol
    {
        /// <summary>
        /// プロトコルファクトリ
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="playerId">Player identifier.</param>
        /// <param name="renamedName">Renamed name.</param>
        public static IProtocol<(PlayerId playerId, PlayerName renamedName)> Create(long playerId, string renamedName) => new RenamePlayerProtocolImpl(playerId, renamedName);

        /// <summary>
        /// リネームユースケースプロトコル具象実装
        /// </summary>
        /// 簡略化のためにユースケースプロトコルをタプルで記載しているが
        /// 要素が多いようであればこちらも型定義する
        private readonly struct RenamePlayerProtocolImpl : IProtocol<(PlayerId playerId, PlayerName renamedName)>
        {
            /// <summary>
            /// プレイヤーIDの値
            /// </summary>
            /// <value>The player identifier.</value>
            private readonly long m_playerId;

            /// <summary>
            /// 変更後の名前の値
            /// </summary>
            /// <value>The name of the renamed.</value>
            private readonly string m_renamedName;

            /// <summary>
            /// コストラクタ
            /// </summary>
            /// <param name="playerId">Player identifier.</param>
            /// <param name="renamedName">Renamed name.</param>
            public RenamePlayerProtocolImpl(long playerId, string renamedName) => (m_playerId, m_renamedName) = (playerId, renamedName);

            /// <summary>
            /// ドメインタイプに変換する
            /// </summary>
            /// <returns>The domain type.</returns>
            public (PlayerId playerId, PlayerName renamedName) ToUseCaseProtocol() => (playerId: new PlayerId(m_playerId), renamedName: new PlayerName(m_renamedName));
        }
    }
    #endregion
}