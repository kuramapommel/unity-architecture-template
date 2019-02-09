/* 
 * ValueObject.cs にはその境界づけられたコンテキストで使用する ValueObject を定義する
 * ValueObject は immutable を担保するため readonly struct で定義する
 * 各 ValueObject は IValueObject を実装する
 */

using System;
using System.Collections.Generic;

namespace Domain.Player
{
    /// <summary>
    /// プレイヤーを一意に識別するための識別子
    /// </summary>
    /// 識別子を表す ValueObject は IEntityId を実装する
    /// また IEntityId 実装の ValueObject は operator == を使用するため
    /// bool Equals(object obj) と int GetHashCode() を override する
    public readonly struct PlayerId : IEntityId, IValueObject<long>
    {
        /// <summary>
        /// 識別子の値
        /// </summary>
        /// <value>The value.</value>
        public long Value { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">Value.</param>
        public PlayerId(long value) => Value = value;

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>同じIDならtrue</returns>
        public override bool Equals(object obj) => obj is PlayerId && Value == ((PlayerId)obj).Value;

        /// <summary>
        /// hash取得
        /// </summary>
        /// <returns>hash</returns>
        public override int GetHashCode() => EqualityComparer<long>.Default.GetHashCode(Value);

        /// <summary>
        /// 等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerId"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PlayerId self, object that) => self.Equals(that);

        /// <summary>
        /// 非等価比較
        /// </summary>
        /// <param name="self">The first <see cref="Domain.Player.PlayerId"/> to compare.</param>
        /// <param name="that">The second <see cref="object"/> to compare.</param>
        /// <returns><c>true</c> if <c>self</c> and <c>that</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PlayerId self, object that) => !(self == that);
    }

    /// <summary>
    /// プレイヤー名
    /// </summary>
    public readonly struct PlayerName : IValueObject<string>
    {
        /// <summary>
        /// プレイヤー名の値
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">Value.</param>
        public PlayerName(string value) => Value = value;
    }

    public readonly struct PlayerCreateAt : IValueObject<DateTime>
    {
        public DateTime Value { get; }

        public PlayerCreateAt(DateTime value) => Value = value;
    }

    public readonly struct PlayerUpdateAt : IValueObject<DateTime>
    {
        public DateTime Value { get; }

        public PlayerUpdateAt(DateTime value) => Value = value;
    }
}