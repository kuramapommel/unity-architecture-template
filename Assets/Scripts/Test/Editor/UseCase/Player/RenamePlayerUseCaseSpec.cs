using System.Linq;
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
            var playerId = new PlayerId(1);
            var playerName = new PlayerName("name");
            var playerMock = PlayerFactory.Create(playerId, playerName);
            var playerRepositoryMock = new PlayerRepositoryMock(playerMock);

            var renamedName = new PlayerName("renamed name");
            var renamePlayerUseCase = new RenamePlayerUseCase(
                playerRepositoryMock,
                playerId,
                renamedName
                );

            var result = renamePlayerUseCase.Execute();
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