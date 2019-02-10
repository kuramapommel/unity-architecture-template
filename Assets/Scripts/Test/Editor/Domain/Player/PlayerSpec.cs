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
        public void PlayerのCopyは渡されたパラメータを元に同じentityのコピーを生成する()
        {
            var player = PlayerFactory.Create(id, name, createAt);

            var updatedName = new PlayerName("updated name");

            var copiedPlayer = player.Copy(name: updatedName);

            AreEqual(id, copiedPlayer.Id);
            AreEqual(updatedName, copiedPlayer.Name);
            AreEqual(createAt, copiedPlayer.CreateAt);
            // Name が更新されているため update at も更新されている
            AreNotEqual(createAt, copiedPlayer.UpdateAt);
            // PlayerCreateAt, PlayerUpdateAt それぞれ IComparable, IComparable<DateTime> を実装し、Less比較する
            // Less(createAt, copiedPlayer.UpdateAt);
        }
    }
}