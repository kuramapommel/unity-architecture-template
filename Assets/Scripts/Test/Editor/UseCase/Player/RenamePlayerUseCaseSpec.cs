using NUnit.Framework;
using Domain.Player;
using UseCase.Player;
using UseCase;
using System.Linq;
using static NUnit.Framework.Assert;

namespace Test.UseCase.Player
{
    public sealed class RenamePlayerUseCaseSpec
    {
        [Test]
        public void プレイヤーの名前を変更し保存することができる()
        {
            #region 下ごしらえ
            var longPlayerId = 1L;
            var playerId = new PlayerId(longPlayerId);
            var playerName = new PlayerName("name");
            var playerMock = PlayerFactory.Create(playerId, playerName);
            var playerRepositoryMock = new PlayerRepositoryMock(playerMock);
            #endregion

            // protocol を定義
            var protocol = RenamePlayerProtocol.Create(longPlayerId, "renamed name");
            var renamePlayerUseCase = new RenamePlayerUseCase(playerRepositoryMock);

            var renamedName = protocol.ToUseCaseProtocol().FirstOrDefault().renamedName;
            var result = renamePlayerUseCase.Execute(protocol);

            // foreach による値取得のテスト
            foreach (var renamedPlayerForeach in result)
            {
                AreEqual(playerMock, renamedPlayerForeach); // player id が変わっていないことのテスト
                AreNotEqual(playerName, renamedPlayerForeach.Name); // 名前が変わっていることのテスト
                AreEqual(renamedName, renamedPlayerForeach.Name); // 名前が renamedName に変わっていることのテスト
            }

            // linq による値取得のテスト
            var renamedPlayerLinq = result.FirstOrDefault();
            AreEqual(playerMock, renamedPlayerLinq); // player id が変わっていないことのテスト
            AreNotEqual(playerName, renamedPlayerLinq.Name); // 名前が変わっていることのテスト
            AreEqual(renamedName, renamedPlayerLinq.Name); // 名前が renamedName に変わっていることのテスト

            // type switch による値取得のテスト
            switch (result)
            {
                case Success<IPlayer> success:
                    var renamedPlayerTypeSwitch = success.Result;
                    AreEqual(playerMock, renamedPlayerTypeSwitch); // player id が変わっていないことのテスト
                    AreNotEqual(playerName, renamedPlayerTypeSwitch.Name); // 名前が変わっていることのテスト
                    AreEqual(renamedName, renamedPlayerTypeSwitch.Name); // 名前が renamedName に変わっていることのテスト
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