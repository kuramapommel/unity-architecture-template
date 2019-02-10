using System;
using NUnit.Framework;
using Domain.Player;
using static NUnit.Framework.Assert;

namespace Test.Domain.Player
{
    public sealed class PlayerFactorySpec
    {
        private static readonly PlayerId id = new PlayerId(1);
        private static readonly PlayerName name = new PlayerName("name");
        private static readonly PlayerCreateAt createAt = new PlayerCreateAt(DateTime.Now);

        [Test]
        public void PlayerFactoryは渡されたパラメータを元にPlayerを生成する()
        {
            var player = PlayerFactory.Create(id, name, createAt);

            AreEqual(id, player.Id);
            AreEqual(name, player.Name);
            AreEqual(createAt, player.CreateAt);
            AreEqual(createAt, player.UpdateAt);
        }

        [Test]
        public void PlayerはRenameによって名前を変更でき_その際更新日付も更新される()
        {
            var player = PlayerFactory.Create(id, name, createAt);

            var renamedName = new PlayerName("updated name");

            var renamedPlayer = player.Rename(name: renamedName);

            AreEqual(id, renamedPlayer.Id);
            AreEqual(renamedName, renamedPlayer.Name);
            AreEqual(createAt, renamedPlayer.CreateAt);
            // Name の変更に合わせて UpdateAt も更新されている
            Less(createAt, renamedPlayer.UpdateAt);
        }
    }
}