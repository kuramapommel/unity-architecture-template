/*
 * Entity 定義
 * [Entity].cs には、interface, Factory, impl をそれぞれまとめて定義する
 * 
 * namespace は境界づけられたコンテキスト名を使用する
 */

using System;
using Domain.Exceptions;

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
        /// 名前変更
        /// </summary>
        /// <returns>The rename.</returns>
        /// <param name="name">Name.</param>
        IDomainResult<IPlayer> Rename(PlayerName name);
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
        public static IPlayer Create(PlayerId id, PlayerName name)
        {
            var createAt = new PlayerCreateAt(DateTime.Now);
            return new PlayerImpl(id, name, createAt, createAt);
        }

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
            /// 名前変更
            /// </summary>
            /// <returns>The rename.</returns>
            /// <param name="name">Name.</param>
            /// 状態の変化は新しいオブジェクトを生成することで immutable に実現する
            /// 振る舞いは関連する一連の処理（トランザクション整合性を担保する処理）を一つのふるまいとして記述する
            /// ここでは名前を変更することによって、プレイヤー更新日時も変わるため合わせて更新し、
            /// 識別子など変わらない部分はそのままプロパティの値を注入する
            public IDomainResult<IPlayer> Rename(PlayerName name)
            {
                try
                {
                    return DomainResult.Success<IPlayer>(new PlayerImpl(
                        Id,
                        name,
                        CreateAt,
                        new PlayerUpdateAt(DateTime.Now)));
                }
                catch(ValidationException e)
                {
                    return DomainResult.Failure<IPlayer>(new ValueObjectCreatedError(e));
                }
            }

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
            public override int GetHashCode() => Id.GetHashCode();

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
