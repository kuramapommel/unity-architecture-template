using NUnit.Framework;
using Domain.Player;
using UseCase.Player;
using UseCase;
using static NUnit.Framework.Assert;

namespace Test.UseCase.Player
{
    public sealed class RenamePlayerUseCaseSpec
    {
        [Test]
        public void プレイヤーの名前を変更し保存することができる()
        {
            #region 下ごしらえ
            var intPlayerId = 1;
            var playerId = new PlayerId(intPlayerId);
            var playerName = new PlayerName("name");
            var playerMock = PlayerFactory.Create(playerId, playerName);
            var playerRepositoryMock = new PlayerRepositoryMock(playerMock);
            #endregion

            // protocol を定義
            var protocol = RenamePlayerProtocol.Create(intPlayerId, "renamed name");
            var renamePlayerUseCase = new RenamePlayerUseCase(playerRepositoryMock);

            var renamedName = protocol.ToDomainType().renamedNmae;
            var result = renamePlayerUseCase.Execute(protocol);

            // IEnumerable による値取得のテスト
            foreach (var savedPlayer in result)
            {
                AreEqual(playerMock, savedPlayer); // player id が変わっていないことのテスト
                AreNotEqual(playerName, savedPlayer.Name); // 名前が変わっていることのテスト
                AreEqual(renamedName, savedPlayer.Name); // 名前が renamedName に変わっていることのテスト
            }

            // type switch による値取得のテスト
            switch (result)
            {
                case Success<IPlayer> success:
                    var savedPlayer = success.Result;
                    AreEqual(playerMock, savedPlayer); // player id が変わっていないことのテスト
                    AreNotEqual(playerName, savedPlayer.Name); // 名前が変わっていることのテスト
                    AreEqual(renamedName, savedPlayer.Name); // 名前が renamedName に変わっていることのテスト
                    return;

                default:
                    Fail();
                    return;
            }
        }

        private sealed class PlayerRepositoryMock : IPlayerRepository
        {
            private readonly IPlayer m_playerMock;

            public PlayerRepositoryMock(IPlayer playerMock) => m_playerMock = playerMock;

            public IPlayer FindById(PlayerId id) => m_playerMock;

            public IPlayer Save(IPlayer player) => player;
        }
    }
}