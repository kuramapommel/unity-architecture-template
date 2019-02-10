/*
 * Entity 定義
 * [Entity].cs には、interface, Factory, impl をそれぞれまとめて定義する
 * 
 * namespace は境界づけられたコンテキスト名を使用する
 */

using System.Collections.Generic;
using System;

namespace Domain.Player
{
    /// <summary>
    /// プレイヤー Entity
    /// </summary>
    /// 外部無形に interface を定義する
    /// 他のサブドメインやレイヤーからはこの interface を参照する
    public interface IPlayer : IEntity<PlayerId>
    {
        /// <summary>
        /// プレイヤーID
        /// </summary>
        /// <value>The identifier.</value>
        PlayerId Id { get; }

        /// <summary>
        /// プレイヤー名
        /// </summary>
        /// <value>The name.</value>
        PlayerName Name { get; }

        /// <summary>
        /// プレイヤー作成日時
        /// </summary>
        /// <value>The create at.</value>
        PlayerCreateAt CreateAt { get; }

        /// <summary>
        /// プレイヤー更新日時
        /// </summary>
        /// <value>The update at.</value>
        PlayerUpdateAt UpdateAt { get; }

        /// <summary>
        /// コピー
        /// </summary>
        /// <returns>The copy.</returns>
        /// <param name="name">Name.</param>
        /// <param name="updateAt">UpdateAt.</param>
        /// コピーメソッドを用意しておくことで、
        /// 更新後のインスタンスを作り直して return する場合などのファクトリとして使える
        /// コピーメソッドで重要なのは、 Id や CreateAt の様に恒久な値は引数で受けないようにしておくこと
        /// 業務的に変更されることがあるプロパティのみを引数で受けるようにすることで同じエンティティ（ copy ）であることを保証する
        IPlayer Copy(PlayerName name = default, PlayerUpdateAt updateAt = default);
    }

    /// <summary>
    /// プレイヤーファクトリ
    /// </summary>
    /// 対象の entity の実装クラスを生成するための static ファクトリも合わせて宣言する
    public static class PlayerFactory
    {
        /// <summary>
        /// プレイヤーファクトリメソッド
        /// </summary>
        /// <returns>The player.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="createAt">Create at.</param>
        public static IPlayer Create
        (
            PlayerId id,
            PlayerName name,
            PlayerCreateAt createAt
        ) => new PlayerImpl(
            id,
            name,
            createAt,
            (PlayerUpdateAt)createAt
        );

        /// <summary>
        /// プレイヤー Entity 実装
        /// </summary>
        /// 業務実装はファクトリ内に構造体ごと隠蔽してしまうことで、
        /// 他のサブドメインやレイヤーから直接インスタンス化されるのを防げる
        /// entity は state を持たないため readonly struct として宣言する
        /// 状態の更新がある場合は更新後の値をセットした新しいインスタンスを生成する
        private readonly struct PlayerImpl : IPlayer
        {
            /// <summary>
            /// プレイヤーID
            /// </summary>
            /// <value>The identifier.</value>
            public PlayerId Id { get; }

            /// <summary>
            /// プレイヤー名
            /// </summary>
            /// <value>The name.</value>
            public PlayerName Name { get; }

            /// <summary>
            /// プレイヤー作成日時
            /// </summary>
            /// <value>The create at.</value>
            public PlayerCreateAt CreateAt { get; }

            /// <summary>
            /// プレイヤー更新日時
            /// </summary>
            /// <value>The update at.</value>
            public PlayerUpdateAt UpdateAt { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id">Identifier.</param>
            /// <param name="name">Name.</param>
            /// <param name="createAt">Create at.</param>
            /// <param name="updateAt">Update at.</param>
            public PlayerImpl
            (
                PlayerId id,
                PlayerName name,
                PlayerCreateAt createAt,
                PlayerUpdateAt updateAt
            ) => (
                Id,
                Name,
                CreateAt,
                UpdateAt
            ) = (
                id,
                name,
                createAt,
                updateAt
            );

            /// <summary>
            /// コピー
            /// </summary>
            /// <returns>The copy.</returns>
            /// <param name="name">Name.</param>
            /// <param name="updateAt">Update at.</param>
            /// デフォルト引数を使用することで
            /// ex.
            /// var copiedPlayer = player.Copy(name: "updated name");
            /// の様に更新が必要な部分だけ名前付き引数で更新させることができる
            /// デフォルト引数はコンパイル時定数を使用しないといけないため
            /// 冗長ではあるが一度 default と比較するようにしている
            /// 将来的に C# のバージョンが上がってコンパイル時定数以外も初期値としてセットできるようになれば
            /// default との比較は不要になる
            public IPlayer Copy(
                PlayerName name = default,
                PlayerUpdateAt updateAt = default
            ) => new PlayerImpl(
                Id, 
                name.Equals(default) ? Name : name,
                CreateAt,
                updateAt.Equals(default) && !name.Equals(default) ? new PlayerUpdateAt(DateTime.Now) : updateAt
            );

            /// <summary>
            /// 等価比較
            /// </summary>
            /// <param name="obj">比較対象のオブジェクト</param>
            /// <returns>同じIDならtrue</returns>
            /// entity は id が同じであれば同じであるとみなすため、
            /// override して、 id を比較する様に実装する
            public override bool Equals(object obj) => obj is IPlayer && Id == ((IPlayer)obj).Id;

            /// <summary>
            /// hash取得
            /// </summary>
            /// <returns>hash</returns>
            public override int GetHashCode() => EqualityComparer<PlayerId>.Default.GetHashCode(Id);

            /// <summary>
            /// 等価比較
            /// </summary>
            /// <param name="self">The first <see cref="Domain.Player.PlayerFactory.PlayerImpl"/> to compare.</param>
            /// <param name="that">The second <see cref="object"/> to compare.</param>
            /// <returns><c>true</c> if <c>self</c> and <c>that</c> are equal; otherwise, <c>false</c>.</returns>
            /// entity の比較は Equals メソッドを使用するより 等号 によるを主に使用するため演算子オーバーロードを使用する
            public static bool operator ==(PlayerImpl self, object that) => self.Equals(that);

            /// <summary>
            /// 非等価比較
            /// </summary>
            /// <param name="self">The first <see cref="Domain.Player.PlayerFactory.PlayerImpl"/> to compare.</param>
            /// <param name="that">The second <see cref="object"/> to compare.</param>
            /// <returns><c>true</c> if <c>self</c> and <c>that</c> are not equal; otherwise, <c>false</c>.</returns>
            public static bool operator !=(PlayerImpl self, object that) => !(self == that);
        }
    }
}
