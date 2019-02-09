using System;
using NUnit.Framework;
using Domain.Player;
using static NUnit.Framework.Assert;

namespace Test.Domain.Player
{
    public sealed class PlayerFactorySpec
    {
        [Test]
        public void PlayerFactoryは渡されたパラメータを元にPlareImplを生成する()
        {
            var id = new PlayerId(1);
            var name = new PlayerName("name");
            var createAt = new PlayerCreateAt(new DateTime());

            var playerImpl = PlayerFactory.Create(id, name, createAt);

            AreEqual(id, playerImpl.Id);
            AreEqual(name, playerImpl.Name);
            AreEqual(createAt, playerImpl.CreateAt);
            AreEqual(createAt, playerImpl.UpdateAt);
        }
    }
}