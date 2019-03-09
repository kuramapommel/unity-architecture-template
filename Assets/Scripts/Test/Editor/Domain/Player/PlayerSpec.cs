using Domain;
using Domain.Player;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Test.Domain.Player
{
    public sealed class PlayerFactorySpec
    {
        private static readonly PlayerId id = new PlayerId(1);
        private static readonly PlayerName name = new PlayerName("name");

        [Test]
        public void プレイヤーファクトリは与えられたパラメータを元にプレイヤを生成する()
        {
            var player = PlayerFactory.Create(id, name);

            AreEqual(id, player.Id);
            AreEqual(name, player.Name);
        }

        [Test]
        public void プレイヤーは名前を変更できる()
        {
            var player = PlayerFactory.Create(id, name);
            var createAt = player.CreateAt;

            var renamedName = new PlayerName("updated name");
            var success = player.Rename(name: renamedName) as Success<IPlayer>;

            var renamedPlayer = success.Result;
            AreEqual(player, renamedPlayer);
            AreEqual(renamedName, renamedPlayer.Name);
            // Name の変更に合わせて UpdateAt も更新されている
            Less(createAt, renamedPlayer.UpdateAt);
        }
    }
}