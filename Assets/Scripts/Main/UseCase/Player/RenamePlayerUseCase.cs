using Domain.Player;
using System.Linq;

namespace UseCase.Player
{
    /// <summary>
    /// プレイヤーリネームユースケース
    /// </summary>
    public sealed class RenamePlayerUseCase : IUseCase<IPlayer, IRenamePlayerProtocol, (PlayerId, PlayerName)>
    {
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

        /// <summary>
        /// リネームユースケース実行
        /// </summary>
        /// <returns>リネーム後のプレイヤー</returns>
        /// <param name="protocol">Protocol.</param>
        public IApplicationResult<IPlayer> Execute(IRenamePlayerProtocol protocol)
        {
            var (playerId, renamedName) = protocol.ToDomainType();

            var player = m_playerRepository.FindById(playerId);

            var renamedPlayerResult = player.Rename(renamedName);

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

    public interface IRenamePlayerProtocol : IUseCaseProtocol<(PlayerId playerId, PlayerName renamedNmae)>
    {
        int PlayerId { get; }

        string RenamedName { get; }
    }

    public static class RenamePlayerProtocol
    {
        public static IRenamePlayerProtocol Create(int playerId, string renamedName) => new RenamePlayerProtocolImpl(playerId, renamedName);

        private readonly struct RenamePlayerProtocolImpl : IRenamePlayerProtocol
        {
            public int PlayerId { get; }

            public string RenamedName { get; }

            public RenamePlayerProtocolImpl(int playerId, string renamedName) => (PlayerId, RenamedName) = (playerId, renamedName);

            public (PlayerId playerId, PlayerName renamedNmae) ToDomainType() => (playerId: new PlayerId(PlayerId), renamedNmae: new PlayerName(RenamedName));
        }
    }
}