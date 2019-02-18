using NUnit.Framework;
using Domain;
using Domain.Player;
using UseCase.Player;
using UseCase;
using System.Linq;
using static NUnit.Framework.Assert;
using UseCaseSuccess = UseCase.Success<Domain.Player.IPlayer>;
using UseCaseFailure = UseCase.Failure<Domain.Player.IPlayer>;

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
                case UseCaseSuccess success:
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

        [Test]
        public void プレイヤーデータ取得時に失敗した場合その情報を取得できる()
        {
            #region 下ごしらえ
            var longPlayerId = 1L;
            var playerId = new PlayerId(longPlayerId);
            var playerName = new PlayerName("name");
            var playerMock = PlayerFactory.Create(playerId, playerName);
            var playerRepositoryMock = new PlayerRepositoryFindErrorMock(playerMock);
            #endregion

            // protocol を定義
            var protocol = RenamePlayerProtocol.Create(longPlayerId, "renamed name");
            var renamePlayerUseCase = new RenamePlayerUseCase(playerRepositoryMock);

            var result = renamePlayerUseCase.Execute(protocol);

            switch (result)
            {
                case UseCaseFailure failure:
                    AreEqual("想定外の例外が発生しました", failure.Reason.Message);
                    break;

                default:
                    Fail();
                    break;
            }
        }

        [Test]
        public void プレイヤーデータ保存時に失敗した場合その情報を取得できる()
        {
            #region 下ごしらえ
            var longPlayerId = 1L;
            var playerId = new PlayerId(longPlayerId);
            var playerName = new PlayerName("name");
            var playerMock = PlayerFactory.Create(playerId, playerName);
            var playerRepositoryMock = new PlayerRepositorySaveErrorMock(playerMock);
            #endregion

            // protocol を定義
            var protocol = RenamePlayerProtocol.Create(longPlayerId, "renamed name");
            var renamePlayerUseCase = new RenamePlayerUseCase(playerRepositoryMock);

            var result = renamePlayerUseCase.Execute(protocol);

            switch (result)
            {
                case UseCaseFailure failure:
                    AreEqual("想定外の例外が発生しました", failure.Reason.Message);
                    break;

                default:
                    Fail();
                    break;
            }
        }

        private sealed class PlayerRepositoryMock : IPlayerRepository
        {
            private readonly IPlayer m_playerMock;

            public PlayerRepositoryMock(IPlayer playerMock) => m_playerMock = playerMock;

            public IDomainResult<IPlayer> FindById(PlayerId id) => DomainResult.Success(m_playerMock);

            public IDomainResult<IPlayer> Save(IPlayer player) => DomainResult.Success(player);
        }

        private sealed class PlayerRepositoryFindErrorMock : IPlayerRepository
        {
            private readonly IPlayer m_playerMock;

            public PlayerRepositoryFindErrorMock(IPlayer playerMock) => m_playerMock = playerMock;

            public IDomainResult<IPlayer> FindById(PlayerId id) => DomainResult.Failure<IPlayer>(new UnexpectedError());

            public IDomainResult<IPlayer> Save(IPlayer player) => DomainResult.Success(player);
        }

        private sealed class PlayerRepositorySaveErrorMock : IPlayerRepository
        {
            private readonly IPlayer m_playerMock;

            public PlayerRepositorySaveErrorMock(IPlayer playerMock) => m_playerMock = playerMock;

            public IDomainResult<IPlayer> FindById(PlayerId id) => DomainResult.Success(m_playerMock);

            public IDomainResult<IPlayer> Save(IPlayer player) => DomainResult.Failure<IPlayer>(new UnexpectedError());
        }
    }
}