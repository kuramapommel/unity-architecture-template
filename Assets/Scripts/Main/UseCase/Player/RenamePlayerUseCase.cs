using Domain;
using Domain.Exceptions;
using Domain.Player;

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
            var result = m_playerRepository.FindById(protocol.playerId)
                .FlatMap(player => player.Rename(protocol.renamedName))
                .FlatMap(m_playerRepository.Save);

            switch (result)
            {
                case Domain.Success<IPlayer> success:
                    return ApplicationResult.Success(success.Result);
                case Domain.Failure<IPlayer> failure:
                    return ApplicationResult.Failure<IPlayer>(failure.Reason.ToApplicationError());
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
            /// アプリケーション層以下で使用する型に変換する
            /// </summary>
            /// <returns>The use case protocol.</returns>
            public IDomainResult<(PlayerId playerId, PlayerName renamedName)> ToUseCaseProtocol()
            {
                try
                {
                    return DomainResult.Success((playerId: new PlayerId(m_playerId), renamedNmae: new PlayerName(m_renamedName)));
                }
                catch (ValidationException exception)
                {
                    return DomainResult.Failure<(PlayerId playerId, PlayerName renamedNmae)>(new ValidationError(exception, ErrorLevel.WARNING));
                }
            }
        }
    }
    #endregion
}